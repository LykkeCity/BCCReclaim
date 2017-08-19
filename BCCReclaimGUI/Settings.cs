using NBitcoin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace BCCReclaimGUI
{
    public class Settings
    {
        public string BroadcastUrlBase
        {
            get;
            set;
        }
        public int DustAmountInSatoshi
        {
            get;
            set;
        }
        public int FeePerKInSatoshi
        {
            get;
            set;
        }
        public int ThresholdBlockHeigh
        {
            get;
            set;
        }
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
        public static Settings ReadAppSettings()
        {
            Settings settings = new Settings();

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            settings.Network = config.AppSettings.Settings["Network"].Value.ToLower().Equals("main") ? NBitcoin.Network.Main : NBitcoin.Network.TestNet;
            settings.QBitNinjaUrl = config.AppSettings.Settings["QBitNinjaUrl"].Value;
            settings.ThresholdBlockHeigh = Int32.Parse(config.AppSettings.Settings["ThresholdBlockHeigh"].Value); 
            settings.FeePerKInSatoshi = Int32.Parse(config.AppSettings.Settings["FeePerKInSatoshi"].Value);
            settings.DustAmountInSatoshi = Int32.Parse(config.AppSettings.Settings["DustAmountInSatoshi"].Value);
            settings.BroadcastUrlBase = config.AppSettings.Settings["BroadcastUrlBase"].Value;
            return settings;
        }
    }
}
