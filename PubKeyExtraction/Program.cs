using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QBitNinja.Client;
using NBitcoin;

namespace PubKeyExtraction
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = PubKeyExtraction.Settings.ReadAppSettings();
            QBitNinjaClient client = new QBitNinjaClient(settings.QBitNinjaUrl, settings.Network);
            StringBuilder resultLines = new StringBuilder();

            using (StreamReader reader = new StreamReader("data\\BCCChannelStates.csv"))
            {
                var str = reader.ReadToEnd();
                var splitted = str.Split(new char[] { '\r', '\n' }).Where(c => !string.IsNullOrEmpty(c)).ToArray();

                for (int counter = settings.StartLine; counter < settings.EndLine; counter++)
                {
                    System.Console.WriteLine(counter);
                    var line = splitted[counter];
                    if (!string.IsNullOrEmpty(line))
                    {
                        var parts = line.Split(new char[] { ',' });
                        var result = client.GetBalance(BitcoinAddress.Create(parts[0])).Result;

                        bool found = false;
                        foreach (var operation in result.Operations)
                        {
                            if (operation.SpentCoins.Count > 0)
                            {
                                var tx = client.GetTransaction(operation.TransactionId).Result.Transaction;

                                foreach (var input in tx.Inputs)
                                {
                                    if (input.PrevOut.Hash == operation.SpentCoins[0].Outpoint.Hash
                                        && input.PrevOut.N == operation.SpentCoins[0].Outpoint.N)
                                    {
                                        var p2shParams = PayToScriptHashTemplate.Instance.ExtractScriptSigParameters(input.ScriptSig);
                                        var multisigParams = PayToMultiSigTemplate.Instance.ExtractScriptPubKeyParameters(p2shParams.RedeemScript);
                                        var pubkey01 = multisigParams.PubKeys[0].ToHex();
                                        var pubkey02 = multisigParams.PubKeys[1].ToHex(); 

                                        resultLines.AppendLine(string.Format("{0},{1},{2},{3},{4}", parts[0], parts[1], parts[2], pubkey01, pubkey02));
                                        found = true;
                                        break;
                                    }
                                }

                                if (found == true)
                                {
                                    break;
                                }
                            }
                        }

                        if (found == false)
                        {
                            resultLines.AppendLine(string.Format("{0},{1},{2},{3},{4}", parts[0], parts[1], parts[2], -1, -1));
                        }
                    }
                }
            }

            using (StreamWriter writer = new StreamWriter(string.Format("data\\Pubkeys_{0}_{1}.csv",settings.StartLine, settings.EndLine)))
            {
                writer.WriteLine(resultLines.ToString());
            }
        }
    }
}
