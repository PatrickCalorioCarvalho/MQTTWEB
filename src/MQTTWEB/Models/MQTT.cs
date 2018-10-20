using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MQTTWEB.Models
{
    public class MQTT
    {
        public MQTT()
        {
            Topico = string.Empty;
            Menssagem = string.Empty;
        }
        public string Topico { get; set; }

        public string Menssagem { get; set; }

    }
}
