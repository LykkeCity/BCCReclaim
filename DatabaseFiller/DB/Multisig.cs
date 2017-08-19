using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseFiller.DB
{
    public class Multisig
    {
        [Key]
        public string MultsigAddress
        {
            get;
            set;
        }

        public string clientAmount
        {
            get;
            set;
        }

        public string hubAmount
        {
            get;
            set;
        }

        public string Pubkey01
        {
            get;
            set;
        }

        public string Pubkey02
        {
            get;
            set;
        }

        public string BothUnsignedTx
        {
            get;
            set;
        }

        public string HubSignedTx
        {
            get;
            set;
        }
    }
}
