using Npgsql;
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
    public partial class yoneticigiris : Form
    {

        NpgsqlConnection baglanti = new NpgsqlConnection("server=localhost; port=5432; Database=dbdeneme; user ID=postgres; password=2121458 ");
        public yoneticigiris()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            string eposta = textBox1.Text;
            string sifre = textBox2.Text;

            NpgsqlCommand komut5 = new NpgsqlCommand("SELECT eposta FROM iletisim WHERE eposta = @p1", baglanti);

            komut5.Parameters.AddWithValue("@p1", eposta);
            string geleneposta = (string)komut5.ExecuteScalar();

            if (geleneposta == null)
            {
                MessageBox.Show("e posta bulunamadı");
                textBox1.Clear();
                textBox2.Clear();
                baglanti.Close();
            }
            else
            {
                NpgsqlCommand komut1 = new NpgsqlCommand("SELECT iletisimid FROM iletisim WHERE eposta = @p1", baglanti);

                komut1.Parameters.AddWithValue("@p1", eposta);
                int iletisimid = (int)komut1.ExecuteScalar();

                NpgsqlCommand komut2 = new NpgsqlCommand("SELECT kisiid FROM kisi WHERE iletisimid = @p2", baglanti);
                komut2.Parameters.AddWithValue("@p2", iletisimid);
                int kisiid = (int)komut2.ExecuteScalar();

                NpgsqlCommand komut3 = new NpgsqlCommand("SELECT sifre FROM yonetici WHERE kisiid = @p3", baglanti);
                komut3.Parameters.AddWithValue("@p3", kisiid);
                string sifregelen = (string)komut3.ExecuteScalar();


                NpgsqlCommand komut6 = new NpgsqlCommand("SELECT sirketid FROM sirket WHERE yoneticiid = @p6", baglanti);
                komut6.Parameters.AddWithValue("@p6", kisiid);
                int sirketid = (int)komut6.ExecuteScalar();


                NpgsqlCommand komut7 = new NpgsqlCommand("SELECT sirketadi FROM sirket WHERE sirketid = @p7", baglanti);
                komut7.Parameters.AddWithValue("@p7", sirketid);
                string sirketad = (string)komut7.ExecuteScalar();


                if (sifre == sifregelen && eposta == geleneposta)
                {
                    using (etkinlik etkinlikekranı =new etkinlik())
                    {
                        this.Hide();
                        etkinlikekranı.label2.Text =sirketad;
                        etkinlikekranı.ShowDialog();
                        this.Show();

                    }

                }
                else
                {
                    MessageBox.Show(" şifre hatalı tekrar deneyin");
                    textBox1.Clear();
                    textBox2.Clear();
                    baglanti.Close();

                }

                baglanti.Close();

            }

        }
    }
}
