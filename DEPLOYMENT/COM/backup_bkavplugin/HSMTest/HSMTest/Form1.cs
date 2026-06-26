using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BkavCASign;
using System.Diagnostics;

namespace HSMTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SignServer ss = new SignServer();
        String strToSign = "He";
        String strSignature = String.Empty;
        String strEcryptData = String.Empty;
        String strEncyptSk = String.Empty;
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (!checkForm())
                    return;
                ss.HsmDllName = txtDllName.Text;
                ss.SlotId = Convert.ToUInt32(txtSlotId.Text);
                ss.PIN = txtPassword.Text;
                ss.PublicKeyName = "VSD";
                ss.PrivateKeyName = "VSD";
                ss.CertificateName = "VSD";

                int iResult = 0;

                // 2. Action
                //for (int i = 0; i <= 12; i++)
                //    strToSign += strToSign;
                listLog.Items.Add("Size Data Test:  " + Convert.ToString(strToSign.Length));
                Stopwatch st = new Stopwatch();
                st.Start();
                iResult = ss.OpenConnection("1", "2", "3");

                if (iResult == 0)
                {
                    st.Stop();
                    listLog.Items.Add("Connect Success!          Total Time: " + st.Elapsed.ToString());
                }
                else
                {
                    listLog.Items.Add(ss.ErrorMessage);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            try
            {
                Stopwatch st = new Stopwatch();
                st.Start();
                CertificateClient cc = new CertificateClient();
                cc.LoadCertificate();
                //strToSign = "0001|1523";

                SessionKey sk = new SessionKey();
                sk.Generate();
                int i = sk.EncryptString(strToSign, ref strEcryptData);
                //i = ss.EncryptSessionKey(sk, ref strEncyptSk);
                cc.EncryptSessionKey(sk, ref strEncyptSk); 
                st.Stop();
                if (i == 0)
                    listLog.Items.Add("Encrypt OK!             Total Time: " + st.Elapsed.ToString());
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                Stopwatch st = new Stopwatch();
                st.Start();
                int i = 0;
                String strText = String.Empty;
                SessionKey sk = new SessionKey();
                //v_strFileData = "C:\testssk.txt"


                //strEncyptSk = System.IO.File.ReadAllText("C:\testssk.txt");
                //strEncyptSk= "<?xml version=""1.0" encoding="utf-16"?> <SessionCirpherInfo>   <EncryptionMethod>http://www.w3.org/2001/04/xmlenc#rsa-1_5</EncryptionMethod>   <SessionCirpher>BGG5y1UspdxY+EeDFoc+pFYuzvYIcS6QOnBV6lHxZ5za1r71X24aTbFbxmPbSV8x2VS0ZdfdHcgeoyoIDc4VJ3yYXwCy3l8Jass4GcoZxSNhkw+lP1t9MZ7wTwp+pDDcdm3wree3jqlIgf4R3jufMQhHblKxrHmyaBF7eDVwo+BgI81Xq1vk6KvNkkYY4Bo5a8/M/T+Kg2Iu+pTqud1wSY8275VwPxxqtexBxK+p5pHttoX5hlPXi/eDQQwpyPaatEbXdJGba+FOtjeG5uzIBgnvCyNr+uo8w75pUyw/SGuWZq/nfpGxjDPadsx8wPTT63qJZ3DkOyi8OhGyuy4Idw==</SessionCirpher>   <Version>1.0.0.2</Version>   <Certificate>     <Issuer>CN=BkavCA, O=Bkav Corporation, L=Hanoi, C=VN</Issuer>     <UID>0.9.2342.19200300.100.1.1=CMND:</UID>     <SerialNumber>54031C34DA6EE25F265B7403E7F5FFA3</SerialNumber>   </Certificate> </SessionCirpherInfo>"
                i = ss.DecryptSessionKey(strEncyptSk, ref sk);
                i = sk.DecryptString(strEcryptData, ref strText);
                st.Stop();
                if (i == 0)
                    listLog.Items.Add("Decrypt OK!             Total Time: " + st.Elapsed.ToString());
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnSign_Click(object sender, EventArgs e)
        {
            try
            {
                //strToSign = "0001|1523";
                Stopwatch st = new Stopwatch();
                st.Start();

                int iResult = 0;
                iResult = ss.SignString(strToSign, ref strSignature);
                st.Stop();
                if (iResult == 0)
                    listLog.Items.Add("Sign Success!          Total Time: " + st.Elapsed.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            try
            {
                Stopwatch st = new Stopwatch();
                st.Start();
                CertificateClient sc = new CertificateClient();
                int loadcert = sc.LoadCertificate();
                String loi = null;
                if (loadcert != 0)
                    loi = sc.ErrorMessage;
                // CertificateClient cc = new CertificateClient();
                // cc.LoadCertificate();
                //int j = cc.VerifyString(strToSign, strSignature);
                int i = sc.VerifyString(strToSign, strSignature);
                st.Stop();
                if (i == 0)
                    listLog.Items.Add("Verify ok!                  Total Time: " + st.Elapsed.ToString());
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnSignString_Click(object sender, EventArgs e)
        {
            try
            {
                Stopwatch st = new Stopwatch();
                st.Start();
                int i = 0;
                String strText = String.Empty;
                String strEncyptSk = "ThuNV";
                String sk = null;
                i = ss.SignString(strEncyptSk, ref sk);
                st.Stop();
                if (i == 0)
                    listLog.Items.Add("Sign string OK!             Total Time: " + st.Elapsed.ToString());
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnClean_Click(object sender, EventArgs e)
        {
            try
            {
                listLog.Items.Clear();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnLoadCert_Click(object sender, EventArgs e)
        {
            try
            {
                SignClient certClient = new SignClient();
                certClient.LoadCertificate();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnChoice_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.ShowDialog();
                txtDllName.Text = ofd.FileName;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private Boolean checkForm() {
            if (String.IsNullOrEmpty(txtDllName.Text))
            {
                MessageBox.Show("DllName is null.");
                return false;
            }
            if (String.IsNullOrEmpty(txtSlotId.Text)) {
                MessageBox.Show("Slot is null");
                return false;
            }
            if (String.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Password is null");
                return false;
            }
            return true;
        }
    }
}
