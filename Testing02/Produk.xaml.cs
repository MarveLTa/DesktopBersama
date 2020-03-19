using System;
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
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.IO;
using System.Data;
using Microsoft.Win32;

namespace Testing02
{
    /// <summary>
    /// Interaction logic for Produk.xaml
    /// </summary>
    public partial class Produk : Window
    {
        // Untuk konek ke database
        //Bisa gini
        // Convert/Allow Zero Datetime digunakan untuk menampilkan date yang punya jam waktu
        MySqlConnection conn = new MySqlConnection("Server=localhost; User Id=root;Password=;Database=9148;Allow Zero Datetime=True");
        MySqlDataAdapter adapter = new MySqlDataAdapter();

        // Atau gini
        /*string connectionString = "Server=localhost; User Id=root;Password='';Database=9148";
        MySqlConnection conn = new MySqlConnection(connectionString); */

        // Untuk query
        MySqlCommand command = new MySqlCommand();
        public DataSet ds = new DataSet();

        public Produk()
        {
            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                 GambarProduk.Source = new BitmapImage(new Uri(op.FileName));
                //var imageBuffer = BitmapSourceToByteArray((BitmapSource)GambarProduk.Source);
                
            }

        }

        private void HapusButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UbahButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TambahButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ds = new DataSet();
                /*
                byte[] images = null;
                FileStream Stream = new FileStream(GambarProduk.ToString(), FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(Stream);
                images = br.ReadBytes((int)Stream.Length);
                */
                DataRowView SelectedRowValue = (DataRowView)DataGrid.SelectedValue;

                byte[] data = (byte[])SelectedRowValue.Row.ItemArray[1];

                adapter = new MySqlDataAdapter("insert into produk(ID_PRODUK, ID_PEGAWAI, NAMA_PRODUK, HARGA_PRODUK, JUMLAH_PRODUK, JUMLAH_MINIMUM_PRODUK, GAMBAR_PRODUK, CREATED_AT, UPDATED_AT, DELETED_AT, CREATED_BY, UPDATED_BY) VALUES('" + IdProdukText.Text + "', '" + IdPegawaiText.Text + "' ,'" + NamaProdukText.Text + "','" + HargaProdukText.Text + "','" + JumlahProdukText.Text + "', '" + JumlahMinimumProdukText.Text + "',@GAMBAR_PRODUK, '" + "2019-12-03" + "','" + "2019-12-03" + "','" + "2019-12-03" + "' , '" + null + "', '" + null + "')", conn);

                command.Parameters.Add("@GAMBAR_PRODUK", MySqlDbType.VarChar, data.Length).Value = data;
                conn.Open();
                int i = command.ExecuteNonQuery();
                if(i > 0)
                {
                    //command.ExecuteNonQuery();
                    adapter.Fill(ds, "produk");
                }
                /*
                MySqlParameter parImage = new MySqlParameter();
                parImage.ParameterName = "?Images";
                parImage.MySqlDbType = MySqlDbType.MediumBlob;
                parImage.Size = 3000000;
                parImage.Value = GambarProduk;//here you should put your byte []

                command.Parameters.Add(parImage);
                command.ExecuteNonQuery();
                */
               
                MessageBox.Show("Berhasil Ditambahkan!");
                conn.Close();
            }
            catch(Exception d)
            {
                MessageBox.Show(d.Message);
            }
        }

        private void TampilButton_Click(object sender, RoutedEventArgs e)
        {
            // Tampil data ke dataGrid
            MySqlCommand cmd = new MySqlCommand("select * from produk", conn);
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
            MySqlCommand cmd = new MySqlCommand("select * from produk", conn);
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
                IdProdukText.Text = selected_row["ID_PRODUK"].ToString();
                IdProdukText.IsReadOnly = true;
                IdPegawaiText.Text = selected_row["ID_PEGAWAI"].ToString();
                NamaProdukText.Text = selected_row["NAMA_PRODUK"].ToString();
                HargaProdukText.Text = selected_row["HARGA_PRODUK"].ToString();
                JumlahProdukText.Text = selected_row["JUMLAH_PRODUK"].ToString();
                JumlahMinimumProdukText.Text = selected_row["JUMLAH_MINIMUM_PRODUK"].ToString();
               // GambarProduk = selected_row["GAMBAR_PRODUK"];
            }
        }
    }
}
