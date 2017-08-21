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
                            resultTx = "-1: No coins to use for building transaction.";
                        }
                        else
                        {
                            var totalAmount = coins.Sum(c => c.Amount);
                            if (totalAmount < clientAmount + hubAmount)
                            {
                                resultTx = "-2: The input amounts does not cover the required outputs.";
                            }
                            else
                            {
                                Money fee = null;

                                // Estimating fee
                                TransactionBuilder builder = new TransactionBuilder();
                                builder.AddCoins(coins);
                                builder.Send(clientPubkey.GetAddress(settings.Network), dust + 1);
                                builder.Send(hubPubkey.GetAddress(settings.Network), dust + 1);
                                builder.SetChange(clientPubkey.GetAddress(settings.Network));
                                fee = builder.EstimateFees(new FeeRate(settings.FeeRatePerK));

                                if (totalAmount - fee < dust + 1)
                                {
                                    resultTx = "-3: After providing fee for transaction nothing remains except dust.";
                                }
                                else
                                {
                                    // Building the actual transaction
                                    builder = new TransactionBuilder();
                                    builder.AddCoins(coins);

                                    long resultingHubAmount = 0;
                                    long resultingClientAmount = 0;
                                    if (clientAmount + hubAmount + fee < totalAmount)
                                    {
                                        resultingHubAmount = hubAmount;
                                    }
                                    else
                                    {
                                        resultingHubAmount = totalAmount - clientAmount - fee;
                                    }

                                    if (resultingHubAmount < dust + 1)
                                    {
                                        resultingHubAmount = 0;
                                        resultingClientAmount = totalAmount - fee;
                                    }
                                    else
                                    {
                                        resultingClientAmount = totalAmount - fee - resultingHubAmount;
                                    }

                                    if (resultingHubAmount > 0)
                                    {
                                        builder.Send(hubPubkey.GetAddress(settings.Network), resultingHubAmount);
                                    }
                                    builder.Send(clientPubkey.GetAddress(settings.Network), resultingClientAmount);
                                    builder.SetChange(clientPubkey.GetAddress(settings.Network));
                                    builder.SendFees(fee);

                                    var tx = builder.BuildTransaction(true, SigHash.All | SigHash.ForkId);
                                    resultTx = tx.ToHex();
                                }
                            }
                        }
                    }

                    resultLines.AppendLine(string.Format("{0},{1},{2},{3},{4},{5}",
                        multisigAddr, clientAmount, hubAmount, clientPubkey.ToHex(), hubPubkey.ToHex(), resultTx));
                }
            }

            using (StreamWriter writer = new StreamWriter(string.Format("data\\UnsignedTx_{0}_{1}.csv", settings.StartLine, settings.EndLine)))
            {
                writer.WriteLine(resultLines.ToString());
            }
        }
    }
}
