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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data;

namespace Testing02
{
    /// <summary>
    /// Interaction logic for CrudHewan.xaml
    /// </summary>
    public partial class CrudHewan : UserControl
    {
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        public DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        private string connection;
        MySqlConnection conn;
        MySqlCommand cmd;
        public CrudHewan()
        {
            InitializeComponent();

            try
            {
                connection = "Server=localhost; User Id=root;Password=;Database=petshop;Allow Zero Datetime=True";
                conn = new MySqlConnection(connection);
                conn.Open();
                FillComboBoxJenisHewan();
                FillComboBoxCustomer();
                TampilDataGrid();
                conn.Close();
            }
            catch (MySqlException e)
            {
                MessageBox.Show(e.Message, "Warning");
            }
        }

        public void FillComboBoxJenisHewan()
        {
            // Ambil ID dan Nama Jenis Hewan dari tabel pegawai ke combobox
            string Query = "select ID_JENIS_HEWAN, NAMA_JENIS from petshop.jenis_hewan;";
            MySqlCommand cmdComboBox = new MySqlCommand(Query, conn);
            MySqlDataReader reader;
            try
            {
                reader = cmdComboBox.ExecuteReader();

                while (reader.Read())
                {
                    int idJenis = reader.GetInt32("ID_JENIS_HEWAN");
                    string namaJenis = reader.GetString("NAMA_JENIS");
                    ComboBoxIdJenisHewan.Items.Add(idJenis + " - " + namaJenis);
                    // ComboBoxIdPegawai.Items.Add(idPegawai);
                }
                reader.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        public void FillComboBoxCustomer()
        {
            // Ambil ID dan Nama Customer dari tabel pegawai ke combobox
            string Query = "select ID_CUSTOMER, NAMA_CUSTOMER from petshop.customer;";
            MySqlCommand cmdComboBox = new MySqlCommand(Query, conn);
            MySqlDataReader reader;
            try
            {
                reader = cmdComboBox.ExecuteReader();

                while (reader.Read())
                {
                    int idCustomer = reader.GetInt32("ID_CUSTOMER");
                    string namaCustomer = reader.GetString("NAMA_CUSTOMER");
                    ComboBoxIdCustomer.Items.Add(idCustomer + " - " + namaCustomer);
                    // ComboBoxIdPegawai.Items.Add(idPegawai);
                }
                reader.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void NamaHewanText_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Fungsi untuk mencari hewan sesuai nama

            DataTable dt = new DataTable();
            MySqlDataAdapter adp = new MySqlDataAdapter("Select ID_HEWAN, ID_JENIS_HEWAN, ID_CUSTOMER, NAMA_HEWAN, TANGGALLAHIR_HEWAN from hewan where Nama_Hewan LIKE '" + NamaHewanText.Text + "%'", conn);
            adp.Fill(dt);
            //DataGrid.Items.Refresh();
            DataGrid.DataContext = dt;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView selected_row = gd.SelectedItem as DataRowView;
            if (selected_row != null)
            {
                IdHewanText.Text = selected_row["ID_HEWAN"].ToString();
                ComboBoxIdJenisHewan.Text = selected_row["ID_JENIS_HEWAN"].ToString();
                ComboBoxIdCustomer.Text = selected_row["ID_CUSTOMER"].ToString();
                NamaHewanText.Text = selected_row["NAMA_HEWAN"].ToString();
                DatePickTglLahir.Text = selected_row["TANGGALLAHIR_HEWAN"].ToString();
            }
        }

        private void GetRecords()
        {
            MySqlCommand cmd = new MySqlCommand("select ID_HEWAN, ID_JENIS_HEWAN, ID_CUSTOMER, NAMA_HEWAN, TANGGALLAHIR_HEWAN from hewan", conn);
            DataGrid.Items.Refresh();
            conn.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            conn.Close();

            DataGrid.DataContext = dt;
        }

        private void TampilDataGrid()
        {
            // Tampil data ke dataGrid
            MySqlCommand cmd = new MySqlCommand("select ID_HEWAN, ID_JENIS_HEWAN, ID_CUSTOMER, NAMA_HEWAN, TANGGALLAHIR_HEWAN from hewan", conn);
            try
            {
                //conn.Open();
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

        private void BtnTambah_Click(object sender, RoutedEventArgs e)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = "INSERT INTO HEWAN(ID_JENIS_HEWAN, ID_CUSTOMER, NAMA_HEWAN, TANGGALLAHIR_HEWAN) VALUES(@idjenis, @idcustomer, @hewan, @tanggal)";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;

                    string tanggalLahirHewan = DatePickTglLahir.SelectedDate.Value.ToString("yyyy-MM-dd");

                    cmd.Parameters.AddWithValue("@idjenis", ComboBoxIdJenisHewan.SelectedValue);
                    cmd.Parameters.AddWithValue("@idcustomer", ComboBoxIdCustomer.SelectedValue);
                    cmd.Parameters.AddWithValue("@hewan", NamaHewanText.Text);
                    cmd.Parameters.AddWithValue("@tanggal", tanggalLahirHewan);


                    cmd.ExecuteNonQuery();
                    conn.Close();
                    GetRecords();
                    MessageBox.Show("Berhasil ditambahkan");
                    ClearData();
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();
            try
            {
                ds = new DataSet();

                string tanggalLahirHewan = DatePickTglLahir.SelectedDate.Value.ToString("yyyy-MM-dd");

                adapter = new MySqlDataAdapter("update hewan set ID_JENIS_HEWAN = '" + ComboBoxIdJenisHewan.SelectedValue + "', ID_CUSTOMER = '" + ComboBoxIdCustomer.SelectedValue + "', NAMA_HEWAN = '" + NamaHewanText.Text + "', TANGGALLAHIR_HEWAN = '" + tanggalLahirHewan +"' where ID_HEWAN = '" + IdHewanText.Text + "'", conn);
                adapter.Fill(ds, "hewan");
                conn.Close();
                GetRecords();
                MessageBox.Show("Berhasil Diedit!");
                ClearData();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                conn.Close();
            }
        }

        private void BtnHapus_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "DELETE FROM HEWAN WHERE ID_HEWAN = @idhewan";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;

                    cmd.Parameters.AddWithValue("@idhewan", IdHewanText.Text);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    GetRecords();
                    MessageBox.Show("Berhasil Dihapus!");
                    ClearData();
                }
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
                conn.Close();
            }
        }

        private void BtnTampil_Click(object sender, RoutedEventArgs e)
        {
            // Tampil data ke dataGrid
            MySqlCommand cmd = new MySqlCommand("select ID_HEWAN, ID_JENIS_HEWAN, ID_CUSTOMER, NAMA_HEWAN, TANGGALLAHIR_HEWAN from hewan", conn);
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                conn.Close();

                DataGrid.DataContext = dt;
                ClearData();
            }
            catch (MySqlException d)
            {
                MessageBox.Show(d.Message);
            }
        }

        private void ClearData()
        {
            ComboBoxIdCustomer.SelectedIndex = -1;
            ComboBoxIdJenisHewan.SelectedIndex = -1;
            IdHewanText.Clear();
            NamaHewanText.Clear();
            DatePickTglLahir.SelectedDate = null;
        }
    }
}
