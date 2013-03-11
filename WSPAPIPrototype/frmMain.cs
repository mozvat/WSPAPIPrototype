using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace WSPAPIPrototype
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            Payment payment = new Payment();
            payment.Process();
        }

        private void btnHostedCheckout_Click(object sender, EventArgs e)
        {

        }

        private void btnLoyalty_Click(object sender, EventArgs e)
        {

        }

        private void btnGift_Click(object sender, EventArgs e)
        {

        }
    }


}
