using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MacListen
{
    class Config
    {
        private String configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "MacListen.xml");
        public String[] Macs { get; set; }

        private static Config _intall;

        public static Config GetIntall()
        {
            if (_intall == null)
            {
                _intall = new Config();
                _intall.LoadConfig();
            }
            return _intall;
        }

        private void LoadConfig()
        {
            XElement config = XElement.Load(configPath);
            var _macs = config.Elements("mac");
            List<string> list = new List<string>();
            foreach (var mac in _macs)
            {
                list.Add(mac.Value);
            }
            this.Macs = list.ToArray();
        }
    }
}
