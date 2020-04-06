﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using Microsoft.Win32;
using System.Drawing;
using Image = System.Drawing.Image;

namespace Testing02
{
    /// <summary>
    /// Interaction logic for CrudProduk.xaml
    /// </summary>
    public partial class CrudProduk : UserControl
    {
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        public DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        private string connection;
        MySqlConnection conn;
        MySqlCommand cmd;
        
        public CrudProduk()
        {
            InitializeComponent();

            try
            {
                connection = "Server=localhost; User Id=root;Password=;Database=petshop;Allow Zero Datetime=True";
                conn = new MySqlConnection(connection);
                conn.Open();
                TampilDataGrid();
                conn.Close();
            }
            catch (MySqlException e)
            {
                MessageBox.Show(e.Message, "Warning");
                return;
            }
        }

        private void TampilDataGrid()
        {
            // Tampil data ke dataGrid
            MySqlCommand cmd = new MySqlCommand("Select ID_PRODUK as 'ID PRODUK', NAMA_PRODUK, HARGA_PRODUK, JUMLAH_PRODUK, JUMLAH_MINIMUM_PRODUK, GAMBAR_PRODUK from produk", conn);
            try
            {
                //conn.Open();
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                //conn.Close();

                DataGrid.DataContext = dt;
            }
            catch (MySqlException d)
            {
                MessageBox.Show(d.Message);
                return;
            }
        }      

        private void NamaProdukText_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Fungsi untuk mencari produk sesuai nama

            DataTable dt = new DataTable();
            MySqlDataAdapter adp = new MySqlDataAdapter("Select ID_PRODUK as 'ID PRODUK', NAMA_PRODUK, HARGA_PRODUK, JUMLAH_PRODUK, JUMLAH_MINIMUM_PRODUK, GAMBAR_PRODUK from produk where Nama_Produk LIKE '" + NamaProdukText.Text + "%'", conn);
            adp.Fill(dt);
            //DataGrid.Items.Refresh();
            DataGrid.DataContext = dt;
        }
 

        private void BtnTambah_Click(object sender, RoutedEventArgs e)
        {
            // cek jika inputan bukan angka
            int parsedValue;
            if (!int.TryParse(JumlahProdukText.Text, out parsedValue))
            {
                MessageBox.Show("Hanya boleh angka!");
                return;
            }
            if (!int.TryParse(JumlahMinimumProdukText.Text, out parsedValue))
            {
                MessageBox.Show("Hanya boleh angka!");
                return;
            }

            // convert string ke int
            int jumlahProduk = int.Parse(JumlahProdukText.Text);
            int jumlahMinimum = int.Parse(JumlahMinimumProdukText.Text);
            if (jumlahMinimum > jumlahProduk)
            {
                MessageBox.Show("Jumlah Minimum Produk harus lebih kecil dari Jumlah Produk!");
                return;
            }
            else
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    try
                    {
                        conn.Open();

                        //DataRowView SelectedRowValue = (DataRowView)DataGrid.SelectedValue;
                        //byte[] ImageBytes = (byte[])SelectedRowValue.Row.ItemArray[0];
                        cmd.CommandText = "INSERT INTO PRODUK(ID_PRODUK, NAMA_PRODUK, HARGA_PRODUK, JUMLAH_PRODUK, JUMLAH_MINIMUM_PRODUK, GAMBAR_PRODUK) VALUES(@idproduk, @namaproduk, @hargaproduk, @jumlahproduk, @jumlahminimum, @gambarproduk)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = conn;

                        cmd.Parameters.AddWithValue("@idproduk", IdProdukText.Text);
                        cmd.Parameters.AddWithValue("@namaproduk", NamaProdukText.Text);
                        cmd.Parameters.AddWithValue("@hargaproduk", HargaProdukText.Text);
                        cmd.Parameters.AddWithValue("@jumlahproduk", JumlahProdukText.Text);
                        cmd.Parameters.AddWithValue("@jumlahminimum", JumlahMinimumProdukText.Text);
                        cmd.Parameters.AddWithValue("@gambarproduk", GambarProduk);

                        //cmd.Parameters.Add("@gambarproduk", MySqlDbType.Blob, ImageBytes.Length).Value = ImageBytes;

                        cmd.ExecuteNonQuery();
                        conn.Close();
                        GetRecords();
                        MessageBox.Show("Berhasil ditambahkan");
                        NamaProdukText.Clear();
                        IdProdukText.Clear();
                        HargaProdukText.Clear();
                        JumlahMinimumProdukText.Clear();
                        JumlahProdukText.Clear();
                        // conn.Close();
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.Message);
                        return;
                    }
                }
            } 
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            int parsedValue;
            if (!int.TryParse(JumlahProdukText.Text, out parsedValue))
            {
                MessageBox.Show("Hanya boleh angka!");
                return;
            }
            if (!int.TryParse(JumlahMinimumProdukText.Text, out parsedValue))
            {
                MessageBox.Show("Hanya boleh angka!");
                return;
            }

            // convert string ke int
            int jumlahProduk = int.Parse(JumlahProdukText.Text);
            int jumlahMinimum = int.Parse(JumlahMinimumProdukText.Text);
            if (jumlahMinimum > jumlahProduk)
            {
                MessageBox.Show("Jumlah Minimum Produk harus lebih kecil dari Jumlah Produk!");
                return;
            }
            else
            {
                try
                {
                    conn.Open();
                    ds = new DataSet();
                    adapter = new MySqlDataAdapter("update produk set NAMA_PRODUK = '" + NamaProdukText.Text + "', HARGA_PRODUK ='" + HargaProdukText.Text + "', JUMLAH_PRODUK = '" + JumlahProdukText.Text + "', JUMLAH_MINIMUM_PRODUK = '" + JumlahMinimumProdukText.Text + "', GAMBAR_PRODUK = '" + GambarProduk + "' where ID_PRODUK = '" + IdProdukText.Text + "'", conn);
                    adapter.Fill(ds, "produk");
                    conn.Close();
                    GetRecords();
                    MessageBox.Show("Berhasil Diedit!");
                    NamaProdukText.Clear();
                    IdProdukText.Clear();
                    HargaProdukText.Clear();
                    JumlahMinimumProdukText.Clear();
                    JumlahProdukText.Clear();
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                    return;
                }
            }
        }

        private void BtnHapus_Click(object sender, RoutedEventArgs e)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                conn.Open();
                cmd.CommandText = "DELETE FROM PRODUK WHERE ID_PRODUK = @idproduk";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@idproduk", IdProdukText.Text);
                cmd.ExecuteNonQuery();
                conn.Close();
                GetRecords();
                MessageBox.Show("Berhasil Dihapus!");
                NamaProdukText.Clear();
                IdProdukText.Clear();
                HargaProdukText.Clear();
                JumlahMinimumProdukText.Clear();
                JumlahProdukText.Clear();
            }
        }

        private void BtnTampil_Click(object sender, RoutedEventArgs e)
        {
            // Tampil data ke dataGrid
            MySqlCommand cmd = new MySqlCommand("Select ID_PRODUK as 'ID PRODUK', NAMA_PRODUK, HARGA_PRODUK, JUMLAH_PRODUK, JUMLAH_MINIMUM_PRODUK, GAMBAR_PRODUK from produk", conn);
                try
                {
                    conn.Open();
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    conn.Close();

                    DataGrid.DataContext = dt;
                }
                catch (MySqlException d)
                {
                    MessageBox.Show(d.Message);
                }
        }

        private void GetRecords()
        {
            MySqlCommand cmd = new MySqlCommand("Select ID_PRODUK as 'ID PRODUK', NAMA_PRODUK, HARGA_PRODUK, JUMLAH_PRODUK, JUMLAH_MINIMUM_PRODUK, GAMBAR_PRODUK from produk", conn);
            DataGrid.Items.Refresh();
            conn.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            conn.Close();

            DataGrid.DataContext = dt;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView selected_row = gd.SelectedItem as DataRowView;
            if (selected_row != null)
            {
                IdProdukText.Text = selected_row["ID PRODUK"].ToString();
                NamaProdukText.Text = selected_row["NAMA_PRODUK"].ToString();
                HargaProdukText.Text = selected_row["HARGA_PRODUK"].ToString();
                JumlahProdukText.Text = selected_row["JUMLAH_PRODUK"].ToString();
                JumlahMinimumProdukText.Text = selected_row["JUMLAH_MINIMUM_PRODUK"].ToString();

                // Gambar
                //GambarProduk = selected_row["GAMBAR_PRODUK"];

                //ImageSourceConverter imgs = new ImageSourceConverter();
                //GambarProduk.SetValue(Image.SourceProperty, imgs.ConvertFromString(op.FileName.ToString()));
                //GambarProduk.Source = imageBuffer;
                //imageBuffer.GetValue = selected_row["GAMBAR_PRODUK"];
                //(byte)imageBuffer = (GambarProduk)selected_row["Gambar_Produk"];
                //GambarProduk.GetValue = 
                //GambarProduk = ByteToImage(GambarProduk);

                //Image img = byteArrayToImage(GambarProduk);
                //var imageBuffer = BitmapSourceToByteArray((BitmapSource)GambarProduk.Source);
                //GambarProduk = selected_row["GAMBAR_PRODUK", imageBuffer];

                //byte[] image = (byte[])cmd.ExecuteScalar();
                //Image newImage = byteArrayToImage(image);
                //GambarProduk = newImage.Save("GAMBAR_PRODUK");

                byte[] blob = (byte[])selected_row["Gambar_Produk"];
                MemoryStream ms = new MemoryStream();
                ms.Write(blob, 0, blob.Length);
                ms.Position = 0;
                //byteArrayToImage(blob);
                //GambarProduk.Source = blob;
            }
        }

        /*convert bytearray to image
        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            using (MemoryStream mStream = new MemoryStream(byteArrayIn))
            {
                return Image.FromStream(mStream);
            }
        }*/

        private byte[] BitmapSourceToByteArray(BitmapSource image)
        {
            using (var stream = new MemoryStream())
            {
                var encoder = new JpegBitmapEncoder(); // or some other encoder
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(stream);
                return stream.ToArray();
            }
        }

        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        
        public static Bitmap ByteToImage(byte[] blob)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                mStream.Write(blob, 0, blob.Length);
                mStream.Seek(0, SeekOrigin.Begin);

                Bitmap bm = new Bitmap(mStream);
                return bm;
            }
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog();
                op.InitialDirectory = Environment.SpecialFolder.MyPictures.ToString();
                op.Title = "Select a picture";
                op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                  "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                  "Portable Network Graphic (*.png)|*.png";
                if (op.ShowDialog() == true)
                {
                    GambarProduk.Source = new BitmapImage(new Uri(op.FileName));
                    
                    //var imageBuffer = BitmapSourceToByteArray((BitmapSource)GambarProduk.Source);
                    //imageName = op.FileName;
                    //GambarProduk.Source = new BitmapImage(new Uri(imageName));

                    /*
                    imageName = op.FileName;
                    ImageSourceConverter isc = new ImageSourceConverter();
                    GambarProduk.SetValue(Image.SourceProperty, isc.ConvertFromString(imageName));
                    */
                }
            }
            catch (Exception brw)
            {
                MessageBox.Show(brw.Message.ToString());
            }
        }
    }
}
