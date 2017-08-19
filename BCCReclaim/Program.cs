using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBitcoin;

namespace BCCReclaim
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrap.Start();

            var multisigStr = "3Krw39XK2F5esasvAFGqB2pd12xvq6mTbD";

            var hubAmount = Convert.ToInt64(((double)0.0045 * (double)100000000));
            var clientAmount = Convert.ToInt64(((double)0.0005 * (double)100000000));
            var dust = 2730;
            var fee = 20 * 1000;

            if(hubAmount + clientAmount < dust + fee)
            {
                System.Console.WriteLine("Hub has {0} satoshi, client has {1} satoshi, sum of them is lower than dust: {2} + fee: {3}", hubAmount, clientAmount, dust, fee);
            }

            var hubSendingAmount = hubAmount - fee;
            var clientSendingAmount = clientAmount;
            if(hubSendingAmount < 0)
            {
                clientSendingAmount += hubSendingAmount;
                hubSendingAmount = 0;
            }

            var hubPubKey = GetHubPubKeyForMultisig(multisigStr);
            var hubPrivateKey = GetHubPrivateKeyForMultisig(multisigStr);

            var clientPubKey = GetClientPubKeyForMultisig(multisigStr);

            var multisig = Helper.GetMultiSigFromTwoPubKeys(hubPubKey, clientPubKey);

            Transaction tx = new Transaction("020000000198a5ab7737bf58bd3a9ba5fba1b8bf314562e886d332e4c883d1550f5ec88b0f000000006a473044022060b8728257f6a7df568535d3cfe2e55ba77dab50f1e28d40f7ba3d63b8e34d9502202b1890a8b080e8530e60dcef5cc47a90c59d2363598a57e71bbbaa1906b8dccd4121038ae6ed2434b25c9889661d3df974bd6c14aa666f1aa5d59a0ff1e2f2e81f3cfafeffffff02a44bce00000000001976a9141952d3f7e86e46f67e012731e2f6e04820083cf088ac20a107000000000017a914c7525850d52ea77a18e3bf173fad531d6c430ac587394f0700");
            ScriptCoin coin = new ScriptCoin(tx, 1, new Script(multisig.MultiSigScript));

            TransactionBuilder builder = new TransactionBuilder();
            builder.AddCoins(coin).Send(clientPubKey, new Money(clientSendingAmount));
            if(hubSendingAmount > 0)
            {
                builder.AddKeys(hubPrivateKey);
                builder.Send(hubPubKey, new Money(hubSendingAmount));
            }
            builder.SendFees(new Money(fee));

            builder.AddKeys(new BitcoinSecret(privateKey[1]));
            var txToSend = builder.BuildTransaction(true, SigHash.All | SigHash.ForkId);

            var verify = builder.Verify(txToSend);
        }

        static string[] privateKey = new string[] {
                "cQMqC1Vqyi6o62wE1Z1ZeWDbMCkRDZW5dMPJz8QT9uMKQaMZa8JY",
                "cQyt2zxAS2uV7HJWR9hf16pFDTye8YsGL6hzd9pQzMoo9m24RGoV",
                "cSFbgd8zKDSCDHgGocccngyVSfGZsyZFiTXtimTonHyL44gTKTNU",  // 03eb5b1a93a77d6743bd4657614d87f4d2d40566558d4c8faab188d957c32c1976
                "cPBtsvLrD3DnbdGgDZ2EMbZnQurzBVmgmejiMv55jH9JehPDn5Aq"   // 035441d55de4f28fcb967472a1f9790ecfea9a9a2a92e301646d52cb3290b9e355
            };

        public static PubKey GetClientPubKeyForMultisig(string multisigAddress)
        {
            BitcoinSecret clientSecret = new BitcoinSecret(privateKey[1]);
            var clientPubKey = clientSecret.PubKey;
            if (multisigAddress == "3Krw39XK2F5esasvAFGqB2pd12xvq6mTbD")
            {
                return clientPubKey;
            }
            else
            {
                return null;
            }
        }

        public static BitcoinSecret GetHubPrivateKeyForMultisig(string multisigAddress)
        {
            if (multisigAddress == "3Krw39XK2F5esasvAFGqB2pd12xvq6mTbD")
            {
                BitcoinSecret hubSecret = new BitcoinSecret(privateKey[0]);
                return hubSecret;
            }
            else
            {
                return null;
            }
        }

        public static PubKey GetHubPubKeyForMultisig(string multisigAddress)
        {
            BitcoinSecret hubSecret = new BitcoinSecret(privateKey[0]);
            var hubPubKey = hubSecret.PubKey;
            if (multisigAddress == "3Krw39XK2F5esasvAFGqB2pd12xvq6mTbD")
            {
                return hubPubKey;
            }
            else
            {
                return null;
            }
        }
    }
}
