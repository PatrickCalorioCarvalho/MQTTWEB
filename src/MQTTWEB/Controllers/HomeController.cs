using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            try
            {
                return Json(mqtt.OrderBy(x => x.titulo).OrderBy(x => x.local));
            }
            catch (Exception)
            {
            
            }
            return null;


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
            try
            {
                Regex rgx = new Regex(@"/[A-Za-z0-9]*/[A-Za-z0-9]*/[A-Za-z0-9]*");
                if (rgx.IsMatch(e.Topic))
                {
                    var topicos = e.Topic.Split('/');

                    MQTT novo = new MQTT()
                    {
                        topico = e.Topic,
                        titulo = topicos[2],
                        local = topicos[1],
                        isPublish = topicos[3] == "label" ? false : true,
                        valor = Encoding.UTF8.GetString(e.Message)
                    };

                    if (mqtt.Select(x => x.topico).Contains(e.Topic))
                        mqtt.Remove(mqtt.Where(x => x.topico == e.Topic).First());
                    mqtt.Add(novo);
                }
            }
            catch (Exception)
            {

            }

        }
    }
}
