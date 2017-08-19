using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCCReclaimGUI.Entities
{
    public class TransactionSignRequest
    {
        public string TransactionToSign
        {
            get;
            set;
        }

        public string PrivateKey
        {
            get;
            set;
        }
    }
}
