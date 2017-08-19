using Common.Settings;
using NBitcoin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCCReclaim.Settings
{
    public class SettingsProvider : Common.Settings.ISettingsProvider
    {
        public ISettings GetSettings()
        {
            return new Settings { Network = Network.Main };
        }
    }
}
