using Common.Settings;
using NBitcoin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCCReclaim.Settings
{
    public class Settings : ISettings
    {
        public string QBitNinjaBaseUrl
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public string QBitNinjaBalanceUrl
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public string QBitNinjaTransactionUrl
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Network Network
        {
            get; set;
        }
    }
}
