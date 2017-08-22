using NBitcoin;
using QBitNinja.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = Settings.ReadAppSettings();
            QBitNinjaClient client = new QBitNinjaClient(settings.QBitNinjaUrl, settings.Network);
            StringBuilder resultLines = new StringBuilder();
            var dust = new Money(settings.DustAmount);

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
                    string diff = string.Empty;

                    if (!string.IsNullOrEmpty(line))
                    {
                        parts = line.Split(new char[] { ',' });
                        if(parts[3].StartsWith("-1"))
                        {
                            continue;
                        }

                        multisigAddr = BitcoinAddress.Create(parts[0]);
                        clientAmount = Convert.ToInt64(double.Parse(parts[1]) *  (long)MoneyUnit.BTC);
                        hubAmount = Convert.ToInt64(double.Parse(parts[2]) * (long)MoneyUnit.BTC);
                        clientPubkey = new PubKey(parts[3]);
                        hubPubkey = new PubKey(parts[4]);
                        var redeemScript = PayToMultiSigTemplate.Instance.GenerateScriptPubKey(2, new PubKey[] { clientPubkey, hubPubkey });

                        var ops = client.GetBalanceBetween(new QBitNinja.Client.Models.BalanceSelector(multisigAddr),
                            new QBitNinja.Client.Models.BlockFeature(settings.BCCHeight), null, false, true).Result;
                        foreach (var op in ops.Operations)
                        {
                            foreach (var rc in op.ReceivedCoins)
                            {
                                var tx = client.GetTransaction(rc.Outpoint.Hash).Result.Transaction;
                                var coin = new ScriptCoin(tx, rc.Outpoint.N, redeemScript);
                                coins.Add(coin);
                            }
                        }

                        if (coins.Count() == 0)
                        {
                            diff = "No coins found.";
                        }
                        else
                        {
                            var totalAmount = coins.Sum(c => c.Amount);
                            diff = (totalAmount - clientAmount - hubAmount).ToString();
                        }
                    }

                    resultLines.AppendLine(string.Format("{0},{1}",
                        multisigAddr, diff));
                }
            }

            using (StreamWriter writer = new StreamWriter(string.Format("data\\InvalidStates_{0}_{1}.csv", settings.StartLine, settings.EndLine)))
            {
                writer.WriteLine(resultLines.ToString());
            }
        }
    }
}
