using NBitcoin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BCCReclaimGUI.Helper.Helper;

namespace BCCReclaimGUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Alert(string text)
        {
            MessageBox.Show(text, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private async Task<BitcoinSecret> GetPWPrivateKey()
        {
            try
            {
                var dictionary = await ReadWordList();
                var settings = Settings.ReadAppSettings();

                if(string.IsNullOrEmpty(textBoxPW12Words.Text))
                {
                    Alert("12 words should not be null or empty.");
                    return null;
                }
                var wordList = textBoxPW12Words.Text;
                var splittedWordList = wordList.Split(new char[] { ' ' });
                splittedWordList = splittedWordList.Where(c => !string.IsNullOrEmpty(c)).ToArray();
                if (splittedWordList.Count() != 12)
                {
                    Alert("The count of words should be exactly 12");
                    return null;
                }
                else
                {
                    for (int i = 0; i < 12; i++)
                    {
                        if (!dictionary.ContainsKey(splittedWordList[i]))
                        {
                            Alert(string.Format("{0} is not present in the dictionary", splittedWordList[i]));
                            return null;
                        }
                    }

                    var initialKey = GenerateKeyFrom12Words(splittedWordList, dictionary);
                    if (string.IsNullOrEmpty(textBoxPWPWAddr.Text.Trim()))
                    {
                        BitcoinSecret secret = new BitcoinSecret(initialKey, settings.Network);
                        return secret;
                    }
                    else
                    {
                        var initialKeyBytes = initialKey.ToBytes();
                        BitcoinAddress addr = null;
                        try
                        {
                            addr = BitcoinAddress.Create(textBoxPWPWAddr.Text, settings.Network) as BitcoinAddress;
                        }
                        catch (Exception exp)
                        {
                            Alert("Invalid address string: " + exp.ToString());
                            return null;
                        }

                        for (byte i = 0; i <= 255; i++)
                        {
                            var keyBytes = initialKeyBytes;
                            keyBytes[0] = i;
                            Key key = new Key(keyBytes);

                            BitcoinSecret secret = new BitcoinSecret(key, settings.Network);
                            if (secret.GetAddress() == addr)
                            {
                                return secret;
                            }
                        }

                        Alert("The provided address is not a valid address for 12 words.");
                        return null;
                    }
                }
            }
            catch (Exception exp)
            {
                Alert(exp.ToString());
                return null;
            }
        }

        private async Task<BitcoinAddress> GetPWDestinationAddress()
        {
            var settings = Settings.ReadAppSettings();

            if (string.IsNullOrEmpty(textBoxPWDAddr.Text))
            {
                Alert("The destination address field should have a value.");
                return null;
            }

            BitcoinAddress destAddr = null;
            try
            {
                destAddr = BitcoinAddress.Create(textBoxPWDAddr.Text.Trim(), settings.Network);
                return destAddr;
            }
            catch(Exception exp)
            {
                Alert(exp.ToString());
                return null;
            }
        }

        private async void buttonPWSend_Click(object sender, EventArgs e)
        {
            var pk = await GetPWPrivateKey();
            if(pk == null)
            {
                return;
            }

            var dest = await GetPWDestinationAddress();
            if(dest == null)
            {
                return;
            }

            var sendResult = await SendAllCoinsFromSourceToDestination(pk, dest);
            if(sendResult != null)
            {
                Alert(sendResult);
                return;
            }
            else
            {
                Alert("Transaction sent successfully.");
                return;
            }
        }
    }
}
