using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace vtproje
{
    public partial class etkinlikduzenle : Form
    {
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localhost; port=5432; Database=dbdeneme; user ID=postgres; password=2121458 ");
        public etkinlikduzenle()
        {
            InitializeComponent();
        }

        private void etkinlikduzenle_Load(object sender, EventArgs e)
        {
            baglanti.Open();

            NpgsqlCommand komut1 = new NpgsqlCommand("SELECT etkinlikadi FROM sirket INNER JOIN kurulus ON sirket.sirketid=kurulus.sirketid INNER join organizator on kurulus.kurulusid=organizator.kurulusid INNER JOIN etkinlik ON organizator.kurulusid=etkinlik.organizatorid INNER JOIN etkinlikdetay ON etkinlik.detayid = etkinlikdetay.detayid  WHERE sirket.sirketid = @p1 ", baglanti);
            komut1.Parameters.AddWithValue("@p1",int.Parse(label1.Text));
            

            using (NpgsqlDataReader data = komut1.ExecuteReader())
            {
                while (data.Read())
                {
                    comboBox2.Items.Add(data[0]); // İlk sütun
                }
            }


            NpgsqlCommand komut2 = new NpgsqlCommand("SELECT sirketadi FROM sirket", baglanti);
            using (NpgsqlDataReader dr = komut2.ExecuteReader())
            {
                while (dr.Read())
                {
                    comboBox1.Items.Add(dr[0]); // İlk sütun
                }
            }

            baglanti.Close();

        }


        public void button1_Click(object sender, EventArgs e)
        {
           int  sirketid=int.Parse(label1.Text);
            baglanti.Open();
            string etkinlikadi=comboBox2.SelectedItem.ToString();

            NpgsqlCommand komut = new NpgsqlCommand("select etkinlikid from etkinlikdetay inner join etkinlik On etkinlikdetay.detayid =etkinlik.detayid where etkinlikdetay.etkinlikadi=@p1", baglanti);
            komut.Parameters.AddWithValue("@p1",etkinlikadi);
            int etkinlikid=(int)komut.ExecuteScalar();

            NpgsqlCommand komut1 =new NpgsqlCommand("SELECT detayid FROM etkinlikdetay WHERE etkinlikadi=@p1",baglanti);
            komut1.Parameters.AddWithValue("@p1", etkinlikadi);
           int detayid=(int)komut1.ExecuteScalar();

            NpgsqlCommand komut2 = new NpgsqlCommand("UPDATE etkinlikdetay SET etkinlikadi = @p2, etkinlikturu = @p3, yassiniri=@p4  WHERE detayid = @p1", baglanti);
            komut2.Parameters.AddWithValue("@p1", detayid);
            komut2.Parameters.AddWithValue("@p2", textBox1.Text);
            komut2.Parameters.AddWithValue("@p3", textBox3.Text);
            komut2.Parameters.AddWithValue("@p4", int.Parse(textBox4.Text));

            NpgsqlCommand komut3 = new NpgsqlCommand("UPDATE bilet SET tarih =@p1,saat=@p2, ucret=@p3 WHERE etkinlikid=@p4", baglanti);
            komut3.Parameters.AddWithValue("@p1", DateTime.Parse(maskedTextBox2.Text));
            komut3.Parameters.AddWithValue("@p2", TimeSpan.Parse(maskedTextBox1.Text));
            komut3.Parameters.AddWithValue("@p3",int.Parse(textBox7.Text));
            komut3.Parameters.AddWithValue("@p4",etkinlikid);

            NpgsqlCommand komut4 = new NpgsqlCommand("SELECT adresid FROM etkinlik WHERE etkinlikid=@p1", baglanti);
            komut4.Parameters.AddWithValue("@p1", etkinlikid);
            int adresid=(int)komut4.ExecuteScalar();

            NpgsqlCommand komut5 = new NpgsqlCommand("UPDATE adres SET sehir=@p1,mekan=@p2 WHERE adresid=@p3", baglanti);
            komut5.Parameters.AddWithValue("@p1",textBox5.Text);
            komut5.Parameters.AddWithValue("@p2",textBox6.Text);
            komut5.Parameters.AddWithValue("@p3",adresid);

            NpgsqlCommand komut6 =new NpgsqlCommand("SELECT kurulusid FROM kurulus WHERE sirketid=@p1",baglanti);
            komut6.Parameters.AddWithValue("@p1", sirketid);
          int  kurulusid=(int)komut6.ExecuteScalar();

            NpgsqlCommand komut7 = new NpgsqlCommand("UPDATE organizator SET kisisayisi=@p1,personelsayisi=@p2 WHERE kurulusid=@p3", baglanti);
            komut7.Parameters.AddWithValue("@p1",int.Parse(textBox9.Text));
            komut7.Parameters.AddWithValue("@p2",int.Parse(textBox10.Text));
            komut7.Parameters.AddWithValue("@p3",kurulusid);

            NpgsqlCommand komut19 = new NpgsqlCommand("SELECT sirketid FROM sirket WHERE sirketadi=@p1", baglanti);
            komut19.Parameters.AddWithValue("@p1", comboBox1.SelectedItem.ToString());
            int sirketid2 = (int)komut19.ExecuteScalar();

            NpgsqlCommand komut8 = new NpgsqlCommand("SELECT kurulusid FROM kurulus WHERE sirketid=@p1", baglanti);
            komut8.Parameters.AddWithValue("@p1",sirketid2);
            int kurulusid2=(int)komut8.ExecuteScalar();

            NpgsqlCommand komut9 = new NpgsqlCommand("UPDATE sponsor SET butce=@p1 RETURNING kurulusid", baglanti);
            komut9.Parameters.AddWithValue("@p1",int.Parse(textBox12.Text));
            int sponsorid=(int)komut9.ExecuteScalar();

            NpgsqlCommand komut10 = new NpgsqlCommand("UPDATE sponsoretkinlik SET sponsorid=@p1 WHERE etkinlikid=@p2 ", baglanti);
            komut10.Parameters.AddWithValue("@p1", sponsorid);
            komut10.Parameters.AddWithValue("@p2", etkinlikid);

            // BURADA HATA ALIYORUM  nesne oluşturumadı diyor 
            NpgsqlCommand komut11 =new NpgsqlCommand ("SELECT sanatciid FROM sanatcietkinlik WHERE etkinlikid=@p1",baglanti);
            komut11.Parameters.AddWithValue("@p1",etkinlikid);
            int sanatciid=(int)komut11.ExecuteScalar();

            if (sanatciid != 0)
            {
                NpgsqlCommand komut12 = new NpgsqlCommand("UPDATE kisi SET ad=@p1 , soyad=@p2 WHERE kisiid=@p3", baglanti);
                komut12.Parameters.AddWithValue("@p1", textBox13.Text);
                komut12.Parameters.AddWithValue("@p2", textBox14.Text);
                komut12.Parameters.AddWithValue("@p3", sanatciid);

              
                 
                komut12.ExecuteNonQuery();
             
            }

            else {
                NpgsqlCommand komut16 = new NpgsqlCommand("INSERT INTO kisi(ad, soyad) VALUES (@p1, @p2) RETURNING kisiid", baglanti);
                komut16.Parameters.AddWithValue("@p1", textBox13.Text);
                komut16.Parameters.AddWithValue("@p2", textBox14.Text);
                int kisiId = (int)komut16.ExecuteScalar();

                NpgsqlCommand komut15 = new NpgsqlCommand("UPDATE sanatcietkinlik SET sanatciid=@p1 WHERE etkinlikid=@p2", baglanti);
                komut15.Parameters.AddWithValue("@p1", kisiId);
                komut15.Parameters.AddWithValue("@p2", etkinlikid);

                komut16.ExecuteNonQuery();
                komut15.ExecuteNonQuery();  
               
            }

            int k1=komut2.ExecuteNonQuery();
            int k2=komut3.ExecuteNonQuery();
            int k3=komut5.ExecuteNonQuery();
            int k4=komut7.ExecuteNonQuery();
            int k5=komut9.ExecuteNonQuery();
            int k6=komut10.ExecuteNonQuery();
            

            if (k3!=0&& k1!=0 &&k2!=0)
            {
                MessageBox.Show("guncelleme oke ");
            }

           
            
            baglanti.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            
                // Önce Etkinlik ID ve Detay ID'lerini çek
                string etkinlikadi = comboBox2.SelectedItem.ToString();
                NpgsqlCommand komut7 = new NpgsqlCommand("SELECT etkinlikid FROM etkinlikdetay INNER JOIN etkinlik ON etkinlikdetay.detayid = etkinlik.detayid WHERE etkinlikdetay.etkinlikadi=@p1", baglanti);
                komut7.Parameters.AddWithValue("@p1", etkinlikadi);
                int etkinlikid = (int)komut7.ExecuteScalar();

                if (etkinlikid == 0)
                {
                    MessageBox.Show("Etkinlik bulunamadı.");
                    return;
                }
                else
                {


                NpgsqlCommand komut2 = new NpgsqlCommand("DELETE FROM bilet WHERE etkinlikid=@p1", baglanti);
                komut2.Parameters.AddWithValue("@p1", etkinlikid);
                komut2.ExecuteNonQuery();

                NpgsqlCommand komut3 = new NpgsqlCommand("DELETE FROM sanatcietkinlik WHERE etkinlikid=@p1", baglanti);
                komut3.Parameters.AddWithValue("@p1", etkinlikid);
                komut3.ExecuteNonQuery();

                NpgsqlCommand komut4 = new NpgsqlCommand("DELETE FROM sponsoretkinlik WHERE etkinlikid=@p1", baglanti);
                komut4.Parameters.AddWithValue("@p1", etkinlikid);
                komut4.ExecuteNonQuery();

                NpgsqlCommand komut5 = new NpgsqlCommand("DELETE FROM etkinlik WHERE etkinlikid=@p1", baglanti);
                komut5.Parameters.AddWithValue("@p1", etkinlikid);
                int k6 = komut5.ExecuteNonQuery();

                if (k6 > 0)
                {
                    MessageBox.Show("etkinlik silindi.");
                }
                else
                {
                    MessageBox.Show("etkinlik silinmedi");
                }
                }
                
               
            
           


        }
    }
}
