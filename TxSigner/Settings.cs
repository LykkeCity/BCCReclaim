using NBitcoin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace TxSigner
{
    public class Settings
    {
        public Network Network
        {
            get;
            set;
        }

        public string LykkeSignerAddress
        {
            get;
            set;
        }

        public int StartLine
        {
            get;
            set;
        }

        public int EndLine
        {
            get;
            set;
        }

        public static Settings ReadAppSettings()
        {
            Settings settings = new Settings();

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            settings.Network = config.AppSettings.Settings["Network"].Value.ToLower().Equals("main") ? NBitcoin.Network.Main : NBitcoin.Network.TestNet;
            settings.LykkeSignerAddress = config.AppSettings.Settings["LykkeSignerAddress"].Value;
            settings.StartLine = Int32.Parse(config.AppSettings.Settings["StartLine"].Value);
            settings.EndLine = Int32.Parse(config.AppSettings.Settings["EndLine"].Value);
            return settings;
        }
    }
}
