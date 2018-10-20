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
            titulo = string.Empty;
            local = string.Empty;
            isPublish = false;
            valor = string.Empty;
            topico = string.Empty;
        }
        public string topico { get; set; }

        public string titulo { get; set; }

        public string local { get; set; }

        public bool isPublish { get; set; }

        public string valor { get; set; }

    }
}
