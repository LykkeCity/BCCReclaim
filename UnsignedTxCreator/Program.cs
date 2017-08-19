using NBitcoin;
using QBitNinja.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnsignedTxCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = PubKeyExtraction.Settings.ReadAppSettings();
            QBitNinjaClient client = new QBitNinjaClient(settings.QBitNinjaUrl, settings.Network);
            StringBuilder resultLines = new StringBuilder();

            using (StreamReader reader = new StreamReader("data\\BCC.csv"))
            {
                var str = reader.ReadToEnd();
                var splitted = str.Split(new char[] { '\r', '\n' }).Where(c => !string.IsNullOrEmpty(c)).ToArray();

                for (int counter = settings.StartLine; counter < settings.EndLine; counter++)
                {
                    System.Console.WriteLine(counter);
                    var line = splitted[counter];
                    IList<Coin> coins = new List<Coin>();

                    if (!string.IsNullOrEmpty(line))
                    {
                        var parts = line.Split(new char[] { ',' });
                        var multisigAddr = BitcoinAddress.Create(parts[0]);
                        var pubkey01 = new PubKey(parts[1]);
                        var pubkey02 = new PubKey(parts[2]);
                        var redeemScript = PayToMultiSigTemplate.Instance.GenerateScriptPubKey(2, new PubKey[] { pubkey01, pubkey02 });

                        var ops = client.GetBalanceBetween(new QBitNinja.Client.Models.BalanceSelector(multisigAddr),
                            new QBitNinja.Client.Models.BlockFeature(settings.BCCHeight), null, false, true).Result;
                        foreach(var op in ops.Operations)
                        {
                            foreach(var rc in op.ReceivedCoins)
                            {
                                var tx = client.GetTransaction(rc.Outpoint.Hash).Result.Transaction;
                                var coin = new ScriptCoin(tx, rc.Outpoint.N, redeemScript);
                                coins.Add(coin);
                            }
                        }

                        if(coins.Count() == 0)
                        {

                        }
                    }
                }
            }

            using (StreamWriter writer = new StreamWriter(string.Format("data\\UnsignedTx_{0}_{1}.csv", settings.StartLine, settings.EndLine)))
            {
                writer.WriteLine(resultLines.ToString());
            }
        }
    }
}
