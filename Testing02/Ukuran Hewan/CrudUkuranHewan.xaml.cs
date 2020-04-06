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
    /// Interaction logic for CrudUkuranHewan.xaml
    /// </summary>
    public partial class CrudUkuranHewan : UserControl
    {
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        public DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        private string connection;
        MySqlConnection conn;
        MySqlCommand cmd;

        public CrudUkuranHewan()
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
            }
        }
  
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView selected_row = gd.SelectedItem as DataRowView;
            if (selected_row != null)
            {
                ComboBoxUkuranHewan.SelectedValue = selected_row["NAMA_UKURAN"];
                IdUkuranHewanText.Text = selected_row["ID_UKURAN_HEWAN"].ToString();
            }
        }

        private void GetRecords()
        {
            MySqlCommand cmd = new MySqlCommand("select ID_UKURAN_HEWAN, NAMA_UKURAN from ukuran_hewan", conn);
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
            MySqlCommand cmd = new MySqlCommand("select ID_UKURAN_HEWAN, NAMA_UKURAN from ukuran_hewan", conn);
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
                    cmd.CommandText = "INSERT INTO UKURAN_HEWAN(NAMA_UKURAN) VALUES(@ukuran)";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;

                    string ukuran = ((ComboBoxItem)ComboBoxUkuranHewan.SelectedItem).Content.ToString();
                    if(ukuran == "" || ukuran == "-- Pilih --")
                    {
                        MessageBox.Show("Field tidak boleh kosong", "Warning");
                        conn.Close();
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ukuran", ukuran);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        GetRecords();
                        MessageBox.Show("Berhasil ditambahkan");
                    }
                    
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                conn.Open();
                ds = new DataSet();
                adapter = new MySqlDataAdapter("update ukuran_hewan set NAMA_UKURAN = '" + ComboBoxUkuranHewan.SelectedValue + "'where ID_UKURAN_HEWAN = '" + IdUkuranHewanText.Text + "'", conn);
                adapter.Fill(ds, "ukuran_hewan");
                conn.Close();
                GetRecords();
                MessageBox.Show("Berhasil Diedit!");
                IdUkuranHewanText.Clear();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void BtnHapus_Click(object sender, RoutedEventArgs e)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                conn.Open();
                cmd.CommandText = "DELETE FROM UKURAN_HEWAN WHERE ID_UKURAN_HEWAN = @idukuran";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@idukuran", IdUkuranHewanText.Text);
                cmd.ExecuteNonQuery();
                conn.Close();
                GetRecords();
                MessageBox.Show("Berhasil Dihapus!");
                IdUkuranHewanText.Clear();
            }
        }

        private void BtnTampil_Click(object sender, RoutedEventArgs e)
        {
            // Tampil data ke dataGrid
            MySqlCommand cmd = new MySqlCommand("select ID_UKURAN_HEWAN, NAMA_UKURAN from ukuran_hewan", conn);
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

        private void CariUkuranText_TextChanged(object sender, TextChangedEventArgs e)
        {
            DataTable dt = new DataTable();
            MySqlDataAdapter adp = new MySqlDataAdapter("select ID_UKURAN_HEWAN, NAMA_UKURAN from ukuran_hewan where Nama_Ukuran LIKE '" + CariUkuranText.Text + "%'", conn);
            adp.Fill(dt);
            //DataGrid.Items.Refresh();
            DataGrid.DataContext = dt;
        }
    }
}
