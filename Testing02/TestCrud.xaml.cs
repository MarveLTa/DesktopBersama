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
using System.Data;

namespace Testing02
{
    /// <summary>
    /// Interaction logic for TestCrud.xaml
    /// </summary>
    public partial class TestCrud : Window
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

        public TestCrud()
        {
            InitializeComponent();
            TampilDataGrid();
           
        }

        private void TampilDataGrid()
        {
            // Tampil data ke dataGrid
            MySqlCommand cmd = new MySqlCommand("select * from jenis_hewan", conn);
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

        private void TambahButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ds = new DataSet();
                adapter = new MySqlDataAdapter("insert into jenis_hewan(ID_JENIS_HEWAN, ID_PEGAWAI, NAMA_JENIS, CREATED_AT, UPDATE_AT, DELETE_AT, CREATED_BY, UPDATED_BY) VALUES('" + IdJenisHewanText.Text + "','" + IdJenisPegawaiText.Text + "','" + NamaJenisHewanText.Text + "','" + "2019-12-03" + "','" + "2019-12-03" + "','" + "2019-12-03" + "', '" + null + "','" + null + "')", conn);
                adapter.Fill(ds, "jenis_hewan");
                GetRecords();
                MessageBox.Show("Berhasil Ditambahkan!");
                IdJenisHewanText.Clear();
                IdJenisPegawaiText.Clear();
                NamaJenisHewanText.Clear();
            }
            catch (Exception d)
            {
                MessageBox.Show(d.Message);
            }
        }

        private void UbahButton_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void GetRecords()
        {
            MySqlCommand cmd = new MySqlCommand("select * from jenis_hewan", conn);
            DataGrid.Items.Refresh();
            conn.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            conn.Close();

            DataGrid.DataContext = dt;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ds = new DataSet();
                adapter = new MySqlDataAdapter("update jenis_hewan set ID_PEGAWAI = '" + IdJenisPegawaiText.Text + "', NAMA_JENIS = '" + NamaJenisHewanText.Text + "'where ID_JENIS_HEWAN = " + IdJenisHewanText.Text + "", conn);
                adapter.Fill(ds, "jenis_hewan");
                GetRecords();
                MessageBox.Show("Berhasil Diedit!");
                IdJenisHewanText.Clear();
                IdJenisPegawaiText.Clear();
                NamaJenisHewanText.Clear();
            }
            catch(Exception d)
            {
                MessageBox.Show(d.Message);
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView selected_row = gd.SelectedItem as DataRowView;
            if (selected_row != null)
            {
                IdJenisHewanText.Text = selected_row["ID_JENIS_HEWAN"].ToString();
                IdJenisHewanText.IsReadOnly = true;
                IdJenisPegawaiText.Text = selected_row["ID_PEGAWAI"].ToString();
                NamaJenisHewanText.Text = selected_row["NAMA_JENIS"].ToString();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ds = new DataSet();
                adapter = new MySqlDataAdapter("delete from jenis_hewan where ID_JENIS_HEWAN = " + IdJenisHewanText.Text, conn);
                adapter.Fill(ds, "jenis_hewan");
                GetRecords();
                MessageBox.Show("Berhasil Dihapus!");
                IdJenisHewanText.Clear();
                IdJenisPegawaiText.Clear();
                NamaJenisHewanText.Clear();
            }
            catch(Exception d)
            {
                MessageBox.Show(d.Message);
            }
        }

        private void ButtonProduk_Click(object sender, RoutedEventArgs e)
        {
            var NewProduk = new Produk();
            NewProduk.Show();
           // Deactivated();
            this.Close();
        }
    }
}
