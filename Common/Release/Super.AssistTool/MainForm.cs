using Super.Framework;
using System;
using System.Windows.Forms;

namespace HelpTool
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            if(!tbSource.Text.IsNullOrEmpty())
            {
                tbDecrypt.Text = Secret.EncryptDES(tbSource.Text);
            }
            else
            {
                MessageBox.Show("计算加密字符串时，原字符串不能为空");
            }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            if(!tbDecrypt.Text.IsNullOrEmpty())
            {
                tbSource.Text = Secret.DecryptDES(tbDecrypt.Text);
            }
            else
            {
                MessageBox.Show("计算解密字符串时，加密字符串不能为空");
            }
        }
    }
}
