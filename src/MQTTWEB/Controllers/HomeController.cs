using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MQTTWEB.Models;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MQTTWEB.Controllers
{
    public class HomeController : Controller
    {
        static List<MQTT> mqtt = new List<MQTT>();

        [HttpPost]
        public JsonResult atualizar()
        {
            return Json(mqtt.OrderBy(x=>x.Topico));
        }
        public IActionResult Index()
        {
            MqttClient client = new MqttClient("192.168.18.100", 1883, false, null, null, MqttSslProtocols.None);
            client.Connect(Guid.NewGuid().ToString(), "pi", "raspberry");
            client.Subscribe(new string[] { "#" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            mqtt.Clear();
            return View();
        }

        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            MQTT novo = new MQTT() { Topico = e.Topic, Menssagem = Encoding.UTF8.GetString(e.Message) };
            try
            {
                if (mqtt.Select(x => x.Topico).Contains(e.Topic))
                    mqtt.Remove(mqtt.Where(x => x.Topico == e.Topic).First());
                if (novo.Menssagem != string.Empty)
                    mqtt.Add(novo);
            }
            catch (Exception)
            {
                mqtt.Clear();
            }

        }
    }
}
