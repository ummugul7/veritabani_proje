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
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace vtproje
{
    public partial class etkinlik : Form
    {
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localhost; port=5432; Database=dbdeneme; user ID=postgres; password=2121458 ");
        public etkinlik()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();

            NpgsqlCommand komut1 = new NpgsqlCommand("INSERT INTO etkinlikdetay(etkinlikadi,etkinlikturu,yassiniri) VALUES (@p1,@p2,@p3) RETURNING detayid", baglanti);
            komut1.Parameters.AddWithValue("@p1",textBox1.Text);
            komut1.Parameters.AddWithValue("@p2",textBox3.Text);
            komut1.Parameters.AddWithValue("@p3",textBox4.Text);
            int detayid=(int)komut1.ExecuteScalar();

            NpgsqlCommand komut2 = new NpgsqlCommand("INSERT INTO adres(sehir,mekan) VALUES (@p6, @p7) RETURNING adresid", baglanti);
            komut2.Parameters.AddWithValue("@p6", textBox5.Text);
            komut2.Parameters.AddWithValue("@p7", textBox6.Text);
            int adresid = (int)komut2.ExecuteScalar();

            NpgsqlCommand komut4 = new NpgsqlCommand("SELECT sirketid FROM sirket WHERE sirketadi = @p6", baglanti);
            komut4.Parameters.AddWithValue("@p6", label2.Text);
            int sirketid  = (int)komut4.ExecuteScalar();

            NpgsqlCommand komut7=new NpgsqlCommand("INSERT INTO kurulus(sirketid) VALUES (@p1) RETURNING kurulusid", baglanti);
            komut7.Parameters.AddWithValue("@p1", sirketid);
            int kurulusid= (int)komut7.ExecuteScalar();

            NpgsqlCommand komut8 = new NpgsqlCommand("INSERT INTO organizator(kurulusid,personelsayisi,kisisayisi) VALUES (@p1,@p2,@p3) ", baglanti);
            komut8.Parameters.AddWithValue("@p1", kurulusid);
            komut8.Parameters.AddWithValue("@p2",int.Parse(textBox10.Text));
            komut8.Parameters.AddWithValue("@p3", int.Parse(textBox9.Text));
            komut8.ExecuteNonQuery();

            NpgsqlCommand komut10 = new NpgsqlCommand("SELECT sirketid FROM sirket WHERE sirketadi = @p6", baglanti);
            komut10.Parameters.AddWithValue("@p6", comboBox1.SelectedItem.ToString());
            int sirketid2 = (int)komut10.ExecuteScalar();


            NpgsqlCommand komut9 = new NpgsqlCommand("INSERT INTO kurulus(sirketid) VALUES (@p1) RETURNING kurulusid", baglanti);
            komut9.Parameters.AddWithValue("@p1", sirketid2);
            int kurulusid2 = (int)komut9.ExecuteScalar();

            NpgsqlCommand komut11 = new NpgsqlCommand("INSERT INTO sponsor(kurulusid,butce) VALUES (@p1,@p2) ", baglanti);
            komut11.Parameters.AddWithValue("@p1", kurulusid2);
            komut11.Parameters.AddWithValue("@p2", int.Parse(textBox12.Text));

            NpgsqlCommand komut5 = new NpgsqlCommand("INSERT INTO etkinlik(detayid,organizatorid,adresid) VALUES (@p1, @p2,@p3) RETURNING etkinlikid", baglanti);
            komut5.Parameters.AddWithValue("@p1",detayid);
            komut5.Parameters.AddWithValue("@p2",kurulusid);
            komut5.Parameters.AddWithValue("@p3",adresid);
            int etkinlikid =(int)komut5.ExecuteScalar();

            NpgsqlCommand komut12=new NpgsqlCommand("INSERT INTO sponsoretkinlik(etkinlikid,sponsorid) VALUES (@p1, @p2) ", baglanti);
            komut12.Parameters.AddWithValue("@p1", etkinlikid);
            komut12.Parameters.AddWithValue("@p2", kurulusid2);

            NpgsqlCommand komut15 = new NpgsqlCommand("INSERT INTO iletisim(telefon) VALUES (@p1) RETURNING iletisimid", baglanti);
            komut15.Parameters.AddWithValue("@p1", textBox15.Text);
            int iletisim = (int)komut15.ExecuteScalar();
            if (textBox13.Text !="" && textBox14.Text != "")
            {
                NpgsqlCommand komut13 = new NpgsqlCommand("INSERT INTO kisi(ad, soyad,iletisimid) VALUES (@p1, @p2,@p3) RETURNING kisiid", baglanti);
                komut13.Parameters.AddWithValue("@p1", textBox13.Text);
                komut13.Parameters.AddWithValue("@p2", textBox14.Text);
                komut13.Parameters.AddWithValue("@p3", iletisim);
                int kisiId = (int)komut13.ExecuteScalar();

                NpgsqlCommand komut14 = new NpgsqlCommand("INSERT INTO sanatci(kisiid) VALUES (@p1) ", baglanti);
                komut14.Parameters.AddWithValue("@p1", kisiId);

                NpgsqlCommand komut16 = new NpgsqlCommand("INSERT INTO sanatcietkinlik(etkinlikid,sanatciid) VALUES(@p1,@p2)", baglanti);
                komut16.Parameters.AddWithValue("@p1", etkinlikid);
                komut16.Parameters.AddWithValue("@p2", kisiId);

                komut14.ExecuteNonQuery();
                komut16.ExecuteNonQuery();
            }
          

            NpgsqlCommand komut3 = new NpgsqlCommand("INSERT INTO bilet(saat,ucret,tarih,etkinlikid) VALUES (@p1, @p2,@p3,@p4)", baglanti);
            komut3.Parameters.AddWithValue("@p1", TimeSpan.Parse(maskedTextBox1.Text));
            komut3.Parameters.AddWithValue("@p2", int.Parse(textBox7.Text));
            komut3.Parameters.AddWithValue("@p3",DateTime.Parse( maskedTextBox2.Text));
            komut3.Parameters.AddWithValue("@p4", etkinlikid);

            komut3.ExecuteNonQuery();
            komut11.ExecuteNonQuery();
            komut12.ExecuteNonQuery();
            


            MessageBox.Show("etkinlik olusturuldu ");
            this.Close();


        }

        private void etkinlik_Load(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlCommand komut2 = new NpgsqlCommand("SELECT sirketadi FROM sirket", baglanti);
            NpgsqlDataReader dr = komut2.ExecuteReader();

            while (dr.Read())
            {
                comboBox1.Items.Add(dr[0]); // dr[0] burada ilk sütuna erişim sağlar
            }

            baglanti.Close();
        }
    }
}
