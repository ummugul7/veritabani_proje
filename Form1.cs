using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace vtproje
{
    public partial class Form1 : Form
    {
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localhost; port=5432; Database=dbdeneme; user ID=postgres; password=2121458 ");
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            


        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            baglanti.Open();

            NpgsqlCommand komut4 = new NpgsqlCommand("INSERT INTO iletisim(telefon,eposta) VALUES (@p8,@p9) RETURNING iletisimid",baglanti);
            komut4.Parameters.AddWithValue("@p8",textBox6.Text);
            komut4.Parameters.AddWithValue("@p9",textBox7.Text);

            int iletisimid=(int)komut4.ExecuteScalar();

            NpgsqlCommand komut1 = new NpgsqlCommand("INSERT INTO kisi(ad, soyad,iletisimid) VALUES (@p1, @p2, @p10) RETURNING kisiid", baglanti);
            komut1.Parameters.AddWithValue("@p1", textBox1.Text);  
            komut1.Parameters.AddWithValue("@p2", textBox2.Text); 
            komut1.Parameters.AddWithValue("@p10", iletisimid);
           
            int kisiId = (int)komut1.ExecuteScalar(); 

            NpgsqlCommand komut2 = new NpgsqlCommand("INSERT INTO adres(ulke, sehir) VALUES (@p6, @p7) RETURNING adresid", baglanti);
            komut2.Parameters.AddWithValue("@p6", textBox4.Text);  
            komut2.Parameters.AddWithValue("@p7", textBox5.Text);

            int adresid= (int)komut2.ExecuteScalar();


            NpgsqlCommand komut3 = new NpgsqlCommand("INSERT INTO uye(kisiid, yas,adresid,sifre) VALUES (@p3,@p4,@p5,@p6)", baglanti);
            komut3.Parameters.AddWithValue("@p3", kisiId);  // Yeni "kisiid" değerini kullanıyoruz
            komut3.Parameters.AddWithValue("@p4", int.Parse(textBox3.Text));  
            komut3.Parameters.AddWithValue("@p5",adresid);
            komut3.Parameters.AddWithValue("@p6", textBox8.Text);

            komut3.ExecuteNonQuery(); 

            MessageBox.Show("uye oluşturuldu");
            this.Close();
            baglanti.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }
    }
}
