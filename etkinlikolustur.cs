using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vtproje
{
    public partial class etkinlikolustur : Form
    {
        public etkinlikolustur()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            yoneticigiris yoneticigiris = new yoneticigiris();
            this.Hide();
            yoneticigiris.ShowDialog();
            this.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sirketolustur sirketolustur = new sirketolustur();
            this.Hide();
            sirketolustur.ShowDialog();
            this.Show();
        }
    }
}
