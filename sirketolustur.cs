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
    public partial class sirketolustur : Form
    {
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localhost; port=5432; Database=dbdeneme; user ID=postgres; password=2121458 ");
        public sirketolustur()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            if (textBox1.Text !=null && textBox2.Text != null && textBox4.Text !=null
                 && textBox5.Text!=null && textBox6.Text != null && textBox9.Text!=null)
            {
                baglanti.Open();

                NpgsqlCommand komut1 = new NpgsqlCommand("INSERT INTO iletisim(telefon,eposta) VALUES (@p1,@p2) RETURNING iletisimid", baglanti);
                komut1.Parameters.AddWithValue("@p1", textBox3.Text);
                komut1.Parameters.AddWithValue("@p2", textBox4.Text);
                int iletisimid = (int)komut1.ExecuteScalar();


                NpgsqlCommand komut2 = new NpgsqlCommand("INSERT INTO kisi(ad, soyad,iletisimid) VALUES (@p3, @p4, @p5) RETURNING kisiid", baglanti);
                komut2.Parameters.AddWithValue("@p3", textBox1.Text);
                komut2.Parameters.AddWithValue("@p4", textBox2.Text);
                komut2.Parameters.AddWithValue("@p5", iletisimid);
                int kisiId = (int)komut2.ExecuteScalar();

                NpgsqlCommand komut5 = new NpgsqlCommand("INSERT INTO iletisim(telefon) VALUES (@p1) RETURNING iletisimid", baglanti);
                komut5.Parameters.AddWithValue("@p1", textBox9.Text);
                int sirketiletisim = (int)komut5.ExecuteScalar();


                NpgsqlCommand komut6 = new NpgsqlCommand("INSERT INTO adres(ulke, sehir) VALUES (@p6, @p7) RETURNING adresid", baglanti);
                komut6.Parameters.AddWithValue("@p6", textBox7.Text);
                komut6.Parameters.AddWithValue("@p7", textBox8.Text);
                int adresid = (int)komut6.ExecuteScalar();

                NpgsqlCommand komut4 = new NpgsqlCommand("INSERT INTO sirket(sirketadi,iletisimid,adresid) VALUES (@p6,@p7,@p8) RETURNING sirketid", baglanti);
                komut4.Parameters.AddWithValue("@p6", textBox6.Text);
                komut4.Parameters.AddWithValue("@p7", sirketiletisim);
                komut4.Parameters.AddWithValue("@p8", adresid);
                int sirketid = (int)komut4.ExecuteScalar();


                NpgsqlCommand komut3 = new NpgsqlCommand("INSERT INTO yonetici(kisiid,sifre,sirketid) VALUES (@p6,@p7,@p8)", baglanti);
                komut3.Parameters.AddWithValue("@p6", kisiId);  // Yeni "kisiid" değerini kullanıyoruz
                komut3.Parameters.AddWithValue("@p7", textBox5.Text);
                komut3.Parameters.AddWithValue("@p8", sirketid);


                komut3.ExecuteNonQuery(); // şu komut en son uye için oldugunda 1 kez kaydediyor diger durumlar için 2 kez neden 

                NpgsqlCommand komut7 = new NpgsqlCommand("UPDATE sirket SET yoneticiid = @p1 WHERE sirketid = @p2", baglanti);
                komut7.Parameters.AddWithValue("@p1", kisiId); // Yönetici ID
                komut7.Parameters.AddWithValue("@p2", sirketid); // Şirket ID
                komut7.ExecuteNonQuery();

                baglanti.Close();
                MessageBox.Show("kayıt işlemi gerçekleştirildi");
                this.Close();
            }
            else
            {
                MessageBox.Show("gerkli alanları dolduurn");
            }
           
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void sirketolustur_Load(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }
    }
}
