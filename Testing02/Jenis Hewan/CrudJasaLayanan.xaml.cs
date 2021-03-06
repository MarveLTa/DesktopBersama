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

namespace Testing02
{
    /// <summary>
    /// Interaction logic for CrudJasaLayanan.xaml
    /// </summary>
    public partial class CrudJasaLayanan : UserControl
    {
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        public DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        private string connection;
        MySqlConnection conn;

        public CrudJasaLayanan()
        {
            InitializeComponent();
            try
            {
                connection = "Server=localhost; User Id=root;Password=;Database=petshop;Allow Zero Datetime=True";
                conn = new MySqlConnection(connection);
                conn.Open();
                FillComboBoxUkuranHewan();
                TampilDataGrid();
                conn.Close();
            }
            catch (MySqlException e)
            {
                MessageBox.Show(e.Message, "Warning");
            }
        }

        public void FillComboBoxUkuranHewan()
        {
            // Ambil ID dan Nama Ukuran Hewan dari tabel pegawai ke combobox
            string Query = "select ID_UKURAN_HEWAN, NAMA_UKURAN from petshop.ukuran_hewan;";
            MySqlCommand cmdComboBox = new MySqlCommand(Query, conn);
            MySqlDataReader reader;
            try
            {
                reader = cmdComboBox.ExecuteReader();

                while (reader.Read())
                {
                    int idUkuran = reader.GetInt32("ID_UKURAN_HEWAN");
                    string namaUkuran = reader.GetString("NAMA_UKURAN");
                    ComboBoxIdUkuranHewan.Items.Add(idUkuran + " - " + namaUkuran);
                }
                reader.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void TampilDataGrid()
        {
            ds = new DataSet();

            //Tampil data ke DataGrid

            adapter = new MySqlDataAdapter("Select ID_JASA_LAYANAN as 'ID JASA LAYANAN', ID_UKURAN_HEWAN as 'ID UKURAN HEWAN', NAMA_JASA_LAYANAN as 'NAMA JASA LAYANAN', HARGA_JASA_LAYANAN as 'HARGA JASA LAYANAN' from jasa_layanan", conn);

            try
            {
                adapter.Fill(ds, "jasa_layanan");
                conn.Close();
                GetRecords();
            }
            catch (MySqlException d)
            {
                MessageBox.Show(d.Message);
                conn.Close();
                return;
            }
        }

        private void GetRecords()
        {
            MySqlCommand cmd = new MySqlCommand("Select ID_JASA_LAYANAN as 'ID JASA LAYANAN', ID_UKURAN_HEWAN as 'ID UKURAN HEWAN', NAMA_JASA_LAYANAN as 'NAMA JASA LAYANAN', HARGA_JASA_LAYANAN as 'HARGA JASA LAYANAN' from jasa_layanan", conn);
            DataGrid.Items.Refresh();
            conn.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            conn.Close();

            DataGrid.DataContext = dt;
        }

        private void NamaJasaLayananText_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Fungsi untuk mencari hewan sesuai nama
            // auto mencari data
            DataTable dt = new DataTable();
            MySqlDataAdapter adp = new MySqlDataAdapter("Select ID_JASA_LAYANAN as 'ID JASA LAYANAN', ID_UKURAN_HEWAN as 'ID UKURAN HEWAN', NAMA_JASA_LAYANAN as 'NAMA JASA LAYANAN', HARGA_JASA_LAYANAN as 'HARGA JASA LAYANAN' from jasa_layanan where Nama_Jasa_Layanan LIKE '" + NamaJasaLayananText.Text + "%'", conn);
            adp.Fill(dt);
            DataGrid.DataContext = dt;
        }

        private void BtnTambah_Click(object sender, RoutedEventArgs e)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = "INSERT INTO JASA_LAYANAN(ID_UKURAN_HEWAN, NAMA_JASA_LAYANAN, HARGA_JASA_LAYANAN) VALUES(@idukuran, @namajasa, @hargajasa)";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;

                    cmd.Parameters.AddWithValue("@idukuran", ComboBoxIdUkuranHewan.SelectedValue);
                    cmd.Parameters.AddWithValue("@namajasa", NamaJasaLayananText.Text);
                    cmd.Parameters.AddWithValue("@hargajasa", HargaJasaLayananText.Text);

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

                adapter = new MySqlDataAdapter("update jasa_layanan set ID_UKURAN_HEWAN = '" + ComboBoxIdUkuranHewan.SelectedValue + "', NAMA_JASA_LAYANAN = '" + NamaJasaLayananText.Text + "', HARGA_JASA_LAYANAN = '" + HargaJasaLayananText.Text + "' where ID_JASA_LAYANAN = '" + IdJasaLayananText.Text + "'", conn);
                adapter.Fill(ds, "jasa_layanan");
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
                    cmd.CommandText = "DELETE FROM JASA_LAYANAN WHERE ID_JASA_LAYANAN = @idjasa";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;

                    cmd.Parameters.AddWithValue("@idjasa", IdJasaLayananText.Text);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    GetRecords();
                    MessageBox.Show("Berhasil Dihapus!");
                    ClearData();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                conn.Close();
            }
        }

        private void BtnTampil_Click(object sender, RoutedEventArgs e)
        {
            // Tampil semua data ke dataGrid
            MySqlCommand cmd = new MySqlCommand("Select ID_JASA_LAYANAN as 'ID JASA LAYANAN', ID_UKURAN_HEWAN as 'ID UKURAN HEWAN', NAMA_JASA_LAYANAN as 'NAMA JASA LAYANAN', HARGA_JASA_LAYANAN as 'HARGA JASA LAYANAN' from jasa_layanan", conn);
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

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView selected_row = gd.SelectedItem as DataRowView;
            if (selected_row != null)
            {
                IdJasaLayananText.Text = selected_row["ID JASA LAYANAN"].ToString();
                ComboBoxIdUkuranHewan.Text = selected_row["ID UKURAN HEWAN"].ToString();
                NamaJasaLayananText.Text = selected_row["NAMA JASA LAYANAN"].ToString();
                HargaJasaLayananText.Text = selected_row["HARGA JASA LAYANAN"].ToString();
            }
        }

        private void ClearData()
        {
            ComboBoxIdUkuranHewan.SelectedIndex = -1;
            NamaJasaLayananText.Clear();
            HargaJasaLayananText.Clear();
        }
    }
}
