using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vtproje
{
    public partial class girisekranıuye : Form
    {
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localhost; port=5432; Database=dbdeneme; user ID=postgres; password=2121458 ");
        public girisekranıuye()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            int kisiid = int.Parse(label12.Text);
            baglanti.Open();

            string sorgu = "SELECT etkinlik.etkinlikid, etkinlikdetay.etkinlikadi, bilet.saat ,bilet.tarih,bilet.ucret FROM bilet INNER JOIN etkinlik ON bilet.etkinlikid=etkinlik.etkinlikid INNER JOIN etkinlikdetay ON etkinlik.detayid=etkinlikdetay.detayid  WHERE bilet.kisiid="+0; 
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);

            DataSet data = new DataSet();
            da.Fill(data);

            dataGridView2.DataSource = data.Tables[0];

            string sorgu2 = "SELECT etkinlik.etkinlikid, etkinlikdetay.etkinlikadi, bilet.saat ,bilet.tarih, bilet.ucret FROM bilet INNER JOIN etkinlik ON bilet.etkinlikid=etkinlik.etkinlikid INNER JOIN etkinlikdetay ON etkinlik.detayid=etkinlikdetay.detayid  WHERE bilet.kisiid="+kisiid; 
            NpgsqlDataAdapter da2 = new NpgsqlDataAdapter(sorgu2, baglanti);
            DataSet data2 = new DataSet();
            da2.Fill(data2);
            dataGridView1.DataSource = data2.Tables[0];

            baglanti.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();

            int kisiid = int.Parse(label12.Text);

            NpgsqlCommand komut1 = new NpgsqlCommand("SELECT iletisimid FROM kisi WHERE kisiid = @p7", baglanti);
            komut1.Parameters.AddWithValue("@p7", kisiid);
            int iletiismid = (int)komut1.ExecuteScalar();

            NpgsqlCommand komut2 = new NpgsqlCommand("UPDATE iletisim SET telefon = @p2, eposta = @p3  WHERE iletisimid = @p1" ,baglanti);
            komut2.Parameters.AddWithValue("@p1",iletiismid);
            komut2.Parameters.AddWithValue("@p2",textBox1.Text);
            komut2.Parameters.AddWithValue("@p3", textBox2.Text);

            NpgsqlCommand komut4 = new NpgsqlCommand("SELECT adresid FROM uye WHERE kisiid = @p4", baglanti); // adres verisini guncellemek için adresid alıyoruz
            komut4.Parameters.AddWithValue("@p4", kisiid);
            int adresid = (int)komut4.ExecuteScalar();

            NpgsqlCommand komut3 = new NpgsqlCommand("UPDATE adres SET ulke = @p2, sehir = @p3  WHERE adresid = @p1", baglanti);
            komut3.Parameters.AddWithValue("@p1", adresid);
            komut3.Parameters.AddWithValue("@p2", textBox3.Text);
            komut3.Parameters.AddWithValue("@p3", textBox4.Text);

            NpgsqlCommand komut5 = new NpgsqlCommand("UPDATE kisi SET sifre = @p5  WHERE kisiid = @p6", baglanti);
            komut5.Parameters.AddWithValue("@p6", kisiid);
            komut5.Parameters.AddWithValue("@p5", textBox5.Text);

            komut5.ExecuteNonQuery();  // ınsert update işlemleri sonucunda execute fonk cagırmam gerekiyor 
            komut2.ExecuteNonQuery();
            int donusdegeri= komut3.ExecuteNonQuery();
            if (donusdegeri > 0)
            {
                MessageBox.Show("Güncelleme başarılı.");
            }
            else
            {
                MessageBox.Show("başarısız.");
                baglanti.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e) // ÇALIŞIYOR
        {
            baglanti.Open();

            int kisiid = int.Parse(label12.Text);

            NpgsqlCommand komut1 = new NpgsqlCommand("SELECT adresid FROM uye WHERE kisiid = @p4", baglanti); // adres verisini guncellemek için adresid alıyoruz
            komut1.Parameters.AddWithValue("@p4", kisiid);
            int adresid = (int)komut1.ExecuteScalar();

            NpgsqlCommand komut4 = new NpgsqlCommand("SELECT iletisimid FROM kisi WHERE kisiid = @p7", baglanti);
            komut4.Parameters.AddWithValue("@p7", kisiid);
            int iletiismid = (int)komut4.ExecuteScalar();

            // adres sadece uye tablosundan erişildigi için once adresi silicez sonra iletiism  sonra kişi 

            NpgsqlCommand komut2 = new NpgsqlCommand("DELETE FROM adres WHERE adresid = @p1",baglanti);
            komut2.Parameters.AddWithValue("@p1",adresid);

            NpgsqlCommand komut3 = new NpgsqlCommand("DELETE FROM iletisim WHERE iletisimid = @p3", baglanti);
            komut3.Parameters.AddWithValue("@p3", iletiismid);

            NpgsqlCommand komut5 = new NpgsqlCommand("DELETE FROM kisi WHERE kisiid = @p2", baglanti);
            komut5.Parameters.AddWithValue("@p2", kisiid);

            int k2 = komut2.ExecuteNonQuery();
            int k3 = komut3.ExecuteNonQuery();
            int k5 = komut5.ExecuteNonQuery();


            if (k2 > 0  && k5 > 0 && k3 > 0)
            {
                MessageBox.Show("Delete işlemi başarılı.");
            }
            else
            {
                MessageBox.Show("Delete işlemi başarısız.");
            }
            baglanti.Close();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        string saat;
        string tarih;
        string ucret;

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView2.SelectedCells[0].RowIndex;
            label13.Text = dataGridView2.Rows[secilen].Cells[0].Value.ToString();
            saat = dataGridView2.Rows[secilen].Cells[2].Value.ToString();
            tarih= dataGridView2.Rows[secilen].Cells[3].Value.ToString();
            ucret = dataGridView2.Rows[secilen].Cells[4].Value.ToString();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            int etkinlikid = int.Parse(label13.Text);
            int kisiid = int.Parse(label12.Text);

            NpgsqlCommand komut = new NpgsqlCommand("INSERT INTO bilet(saat,ucret,tarih,etkinlikid,kisiid) VALUES (@p1, @p2,@p3,@p4,@p5)", baglanti);
            komut.Parameters.AddWithValue("@p1", TimeSpan.Parse(saat));
            komut.Parameters.AddWithValue("@p2",int.Parse(ucret));
            komut.Parameters.AddWithValue("@p3", DateTime.Parse(tarih));
            komut.Parameters.AddWithValue("@p4", etkinlikid);
            komut.Parameters.AddWithValue("@p5",kisiid);

            int kontrol = komut.ExecuteNonQuery();
            if (kontrol != 0)
            {
                MessageBox.Show("bilet alma islemi gerçeklestirildi ");
            }

            string sorgu = "SELECT etkinlik.etkinlikid, etkinlikdetay.etkinlikadi, bilet.saat ,bilet.tarih,bilet.ucret FROM bilet INNER JOIN etkinlik ON bilet.etkinlikid=etkinlik.etkinlikid INNER JOIN etkinlikdetay ON etkinlik.detayid=etkinlikdetay.detayid  WHERE bilet.kisiid=" + 0;
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);

            DataSet data = new DataSet();
            da.Fill(data);

            dataGridView2.DataSource = data.Tables[0];

            string sorgu2 = "SELECT etkinlik.etkinlikid, etkinlikdetay.etkinlikadi, bilet.saat ,bilet.tarih, bilet.ucret FROM bilet INNER JOIN etkinlik ON bilet.etkinlikid=etkinlik.etkinlikid INNER JOIN etkinlikdetay ON etkinlik.detayid=etkinlikdetay.detayid  WHERE bilet.kisiid=" + kisiid;
            NpgsqlDataAdapter da2 = new NpgsqlDataAdapter(sorgu2, baglanti);
            DataSet data2 = new DataSet();
            da2.Fill(data2);
            dataGridView1.DataSource = data2.Tables[0];
            baglanti.Close();
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            label14.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            int kisiid = int.Parse(label12.Text);
            int etkinlikid = int.Parse(label14.Text);
            baglanti.Open();

            NpgsqlCommand komut=new NpgsqlCommand("DELETE FROM bilet WHERE kisiid=@p1 AND etkinlikid=@p2", baglanti);
            komut.Parameters.AddWithValue("@p1",kisiid);
            komut.Parameters.AddWithValue("@p2",etkinlikid);

            int kontrol = komut.ExecuteNonQuery();
            if (kontrol != 0)
            {
                MessageBox.Show("biletiniz iptal edilmiştir");
            }

            string sorgu = "SELECT etkinlik.etkinlikid, etkinlikdetay.etkinlikadi, bilet.saat ,bilet.tarih,bilet.ucret FROM bilet INNER JOIN etkinlik ON bilet.etkinlikid=etkinlik.etkinlikid INNER JOIN etkinlikdetay ON etkinlik.detayid=etkinlikdetay.detayid  WHERE bilet.kisiid=" + 0;
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);

            DataSet data = new DataSet();
            da.Fill(data);

            dataGridView2.DataSource = data.Tables[0];

            string sorgu2 = "SELECT etkinlik.etkinlikid, etkinlikdetay.etkinlikadi, bilet.saat ,bilet.tarih, bilet.ucret FROM bilet INNER JOIN etkinlik ON bilet.etkinlikid=etkinlik.etkinlikid INNER JOIN etkinlikdetay ON etkinlik.detayid=etkinlikdetay.detayid  WHERE bilet.kisiid=" + kisiid;
            NpgsqlDataAdapter da2 = new NpgsqlDataAdapter(sorgu2, baglanti);
            DataSet data2 = new DataSet();
            da2.Fill(data2);
            dataGridView1.DataSource = data2.Tables[0];
            baglanti.Close();

        }
    }
}
