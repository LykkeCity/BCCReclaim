using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCCReclaim.Models
{
    public class Multisig
    {
        public string MultiSigAddress
        {
            get;
            set;
        }

        public string SegwitMultiSigAddress
        {
            get;
            set;
        }

        public string MultiSigScript
        {
            get;
            set;
        }

        public string SegwitWalletAddress
        {
            get;
            set;
        }
    }
}
