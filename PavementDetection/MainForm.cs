using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PavementDetection
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void EM_collection_bt_Click(object sender, EventArgs e)
        {
            EMCollection emcollectionform1 = new EMCollection();
            emcollectionform1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MESMAForm mesmaform1 = new MESMAForm();
            mesmaform1.Show();
        }



        private void TXTBuild_Click(object sender, EventArgs e)
        {
            TXTFileBuild txtbuild = new TXTFileBuild();
            txtbuild.Show();
        }
    }
}
