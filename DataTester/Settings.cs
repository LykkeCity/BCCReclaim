using NBitcoin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DataTester
{
    public class Settings
    {
        public Network Network
        {
            get;
            set;
        }

        public string QBitNinjaUrl
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

        public int DustAmount
        {
            get;
            set;
        }

        public int FeeRatePerK
        {
            get;
            set;
        }

        public int BCCHeight
        {
            get;
            set;
        }
        public static Settings ReadAppSettings()
        {
            Settings settings = new Settings();

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            settings.Network = config.AppSettings.Settings["Network"].Value.ToLower().Equals("main") ? NBitcoin.Network.Main : NBitcoin.Network.TestNet;
            settings.QBitNinjaUrl = config.AppSettings.Settings["QBitNinjaUrl"].Value;
            settings.StartLine = Int32.Parse(config.AppSettings.Settings["StartLine"].Value);
            settings.EndLine = Int32.Parse(config.AppSettings.Settings["EndLine"].Value);
            settings.DustAmount = Int32.Parse(config.AppSettings.Settings["DustAmount"].Value);
            settings.BCCHeight = Int32.Parse(config.AppSettings.Settings["BCCHeight"].Value);
            settings.FeeRatePerK = Int32.Parse(config.AppSettings.Settings["FeeRatePerK"].Value);
            return settings;
        }
    }
}
