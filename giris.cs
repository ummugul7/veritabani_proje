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
    public partial class giris : Form
    {
        public giris()
        {
            InitializeComponent();
        }

        private void giris_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            uyegiri uyegiri = new uyegiri();
            this.Hide();
            uyegiri.ShowDialog();
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            this.Hide();
            form1.ShowDialog();
            this.Show();    
        }

        private void button3_Click(object sender, EventArgs e)
        {
            etkinlikolustur etkinlikolustur = new etkinlikolustur();
            this.Hide();
            etkinlikolustur.ShowDialog();
            this.Show();    
        }

        private void button4_Click(object sender, EventArgs e)
        {   
            duzenlegiris duzenlegiris = new duzenlegiris();
            this.Hide();
            duzenlegiris.ShowDialog();
            this.Show();

        }
    }
}
