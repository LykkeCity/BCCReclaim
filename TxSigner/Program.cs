using System;
using Lykke.Signing.Api.Client;
using Lykke.Signing.Api.Client.Services;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using NBitcoin;
using System.Text;

namespace TxSigner
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = Settings.ReadAppSettings();
            TransactionSigningApiClient signingClient = new TransactionSigningApiClient
                (new Lykke.Signing.Api.Client.Options.RestClientConfig { Address = settings.LykkeSignerAddress });
            StringBuilder resultLines = new StringBuilder();

            using (StreamReader reader = new StreamReader("data\\BCC.csv"))
            {
                var str = reader.ReadToEnd();
                var splitted = str.Split(new char[] { '\r', '\n' }).Where(c => !string.IsNullOrEmpty(c)).ToArray();
                var resultTx = string.Empty;

                for (int counter = settings.StartLine; counter < settings.EndLine; counter++)
                {
                    System.Console.WriteLine(counter);
                    var line = splitted[counter];
                    IList<Coin> coins = new List<Coin>();

                    string[] parts = null;
                    BitcoinAddress multisigAddr = null;
                    long clientAmount = 0;
                    long hubAmount = 0;
                    PubKey clientPubkey = null;
                    PubKey hubPubkey = null;
                    string unsignedTx = null;

                    if (!string.IsNullOrEmpty(line))
                    {
                        parts = line.Split(new char[] { ',' });
                        if (parts[3].StartsWith("-1"))
                        {
                            continue;
                        }

                        multisigAddr = BitcoinAddress.Create(parts[0]);
                        clientAmount = Convert.ToInt64(double.Parse(parts[1]) * (long)MoneyUnit.BTC);
                        hubAmount = Convert.ToInt64(double.Parse(parts[2]) * (long)MoneyUnit.BTC);
                        clientPubkey = new PubKey(parts[3]);
                        hubPubkey = new PubKey(parts[4]);
                        unsignedTx = parts[5];
                        var signedTx = signingClient.Sign(unsignedTx).Result;

                        resultLines.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6}",
                            parts[0], parts[1], parts[2], parts[3], parts[4], parts[5], signedTx));                    }
                }
            }

            using (StreamWriter writer = new StreamWriter(string.Format("data\\SignedTx_{0}_{1}.csv", settings.StartLine, settings.EndLine)))
            {
                writer.WriteLine(resultLines.ToString());
            }
        }
    }
}
