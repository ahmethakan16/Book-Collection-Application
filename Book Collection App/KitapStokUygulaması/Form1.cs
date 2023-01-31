using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace KitapStokUygulaması
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-RVCRK4G\\SQLEXPRESS;Initial Catalog=Trigger;Integrated Security=True");

        public void listele()
        {
            SqlCommand komut = new SqlCommand("Select * From Tbl_KITAPLAR", baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            listele();
            sayac();
            kitapyedek();
        }

        public void kitapyedek()
        {
            SqlCommand komut2 = new SqlCommand("Select * From Tbl_KITAPYEDEK", baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut2);
            DataTable dt= new DataTable();
            da.Fill(dt);
            dataGridView2.DataSource = dt;
        } 

        public void ekle()
        {
            DialogResult kitap = new DialogResult();
            kitap=MessageBox.Show("Kitap Eklensin Mi?","Soru",MessageBoxButtons.YesNo,MessageBoxIcon.Question);

            if(DialogResult.Yes == kitap)
            {
                baglanti.Open();
                SqlCommand ekle = new SqlCommand("Insert Into Tbl_KITAPLAR(AD,YAZAR,SAYFASAYISI,YAYINEVİ,TÜR) VALUES (@p1,@p2,@p3,@p4,@p5)", baglanti);
                ekle.Parameters.AddWithValue("@p1", textBoxKitapAd.Text);
                ekle.Parameters.AddWithValue("@p2", textBoxYazar.Text);
                ekle.Parameters.AddWithValue("@p3", textBoxSayfa.Text);
                ekle.Parameters.AddWithValue("@p4", textBoxYayınEvi.Text);
                ekle.Parameters.AddWithValue("@p5", textBoxTur.Text);
                ekle.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Kitap Başarı İle Eklendi", "Bilgi");
                listele();
            }
            else
            {
                MessageBox.Show("Kitap Eklenmedi", "Bilgi");
                listele();
            }
            sayac();
        }

        public void sil()
        {
            DialogResult sil= new DialogResult();
            sil=MessageBox.Show("Kitap Silinsin Mi?","Soru",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if(DialogResult.Yes == sil)
            {
                baglanti.Open();
                SqlCommand komut2= new SqlCommand("Delete From Tbl_KITAPLAR where ID=@p1", baglanti);
                komut2.Parameters.AddWithValue("@p1", textBoxID.Text);
                komut2.ExecuteNonQuery();
                MessageBox.Show("Kitap Başarı İle Silindi.", "Bilgi");
                baglanti.Close();
                listele();
                kitapyedek();
                
            }
            else
            {
                listele();
                MessageBox.Show("Kitap Silinmedi.", "Bilgi");
            }
            sayac();


        }
        public void sayac()
        {
            baglanti.Open();
            SqlCommand komut3 = new SqlCommand("Select * From Tbl_SAYAC", baglanti);
            SqlDataReader dr = komut3.ExecuteReader();
            while (dr.Read())
            {
                labelToplamKitap.Text = dr[0].ToString();
            }
            baglanti.Close();
        }

        private void buttonEkle_Click(object sender, EventArgs e)
        {
            ekle();
        }

        private void buttonSil_Click(object sender, EventArgs e)
        {
            sil();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBoxID.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            textBoxKitapAd.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            textBoxYazar.Text= dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            textBoxSayfa.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            textBoxYayınEvi.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            textBoxTur.Text= dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
        }
    }
}
