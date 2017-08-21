using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseFiller.DB;
using System.IO;

namespace CSVExporter
{
    class Program
    {
        static void Main(string[] args)
        {
            StringBuilder builder = new StringBuilder();

            using (var dbContext = new BCCReclaimContext())
            {
                var records = from record in dbContext.MultiSigs
                              select record;

                foreach (var record in records)
                {
                    builder.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6}",
                        record.MultsigAddress, record.clientAmount, record.hubAmount, record.Pubkey01, record.Pubkey02, record.BothUnsignedTx, record.HubSignedTx));
                }
            }

            using (StreamWriter writer = new StreamWriter("data\\output.csv"))
            {                writer.WriteLine(builder.ToString());
            }
        }
    }
}
