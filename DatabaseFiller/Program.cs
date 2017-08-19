using DatabaseFiller.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseFiller
{
    class Program
    {
        private static void ImportFile(string filename)
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                var lines = reader.ReadToEnd();
                var splitted = lines.Split(new char[] { '\r', '\n' }).Where(c => !string.IsNullOrEmpty(c));
                using (BCCReclaimContext context = new BCCReclaimContext())
                {
                    foreach (var item in splitted)
                    {
                        var multisigParts = item.Split(new char[] { ',' });
                        var addr = multisigParts[0];

                        Multisig multisig = (from m in context.MultiSigs
                                             where m.MultsigAddress == addr
                                             select m).FirstOrDefault();

                        if (multisig == null)
                        {
                            multisig = new Multisig();
                            multisig.MultsigAddress = multisigParts[0];
                            context.MultiSigs.Add(multisig);
                        }
                        if (string.IsNullOrEmpty(multisig.clientAmount))
                        {
                            multisig.clientAmount = multisigParts[1];
                        }
                        if (string.IsNullOrEmpty(multisig.hubAmount))
                        {
                            multisig.hubAmount = multisigParts[2];
                        }
                        if (multisigParts.Count() > 3 && string.IsNullOrEmpty(multisig.Pubkey01))
                        {
                            multisig.Pubkey01 = multisigParts[3];
                        }
                        if (multisigParts.Count() > 4 && string.IsNullOrEmpty(multisig.Pubkey02))
                        {
                            multisig.Pubkey02 = multisigParts[4];
                        }
                        if (multisigParts.Count() > 5 && string.IsNullOrEmpty(multisig.BothUnsignedTx))
                        {
                            multisig.BothUnsignedTx = multisigParts[5];
                        }

                        if (multisigParts.Count() > 6 && string.IsNullOrEmpty(multisig.HubSignedTx))
                        {
                            multisig.HubSignedTx = multisigParts[6];
                        }
                    }

                    context.SaveChanges();
                }
            }
        }
        static void Main(string[] args)
        {
            ImportFile("data\\BCCChannelStates.csv");
        }
    }
}
