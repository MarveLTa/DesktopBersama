using Microsoft.Win32;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using MySql.Data.MySqlClient;
using System.Windows;
using System;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Testing02
{

    public partial class TestGambar : Window
    {
        string connectionString = @"Server=localhost; User Id=root;Password='';Database=tbl_image";
        //MySqlConnection conn = new MySqlConnection("Server=localhost; User Id=root;Password=;Database=tbl_Image;");
        DataSet ds;
        string strName, imageName;
        public TestGambar()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileDialog fldlg = new OpenFileDialog();
                fldlg.InitialDirectory = Environment.SpecialFolder.MyPictures.ToString();
                fldlg.Filter = "Image File (*.jpg;*.bmp;*.gif)|*.jpg;*.bmp;*.gif";
                fldlg.ShowDialog();
                {
                    strName = fldlg.SafeFileName;
                    imageName = fldlg.FileName;
                    ImageSourceConverter isc = new ImageSourceConverter();
                    image1.SetValue(Image.SourceProperty, isc.ConvertFromString(imageName));
                }
                fldlg = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            insertImageData();
        }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            DataTable dataTable = ds.Tables[0];

            foreach (DataRow row in dataTable.Rows)
            {
                if (row[0].ToString() == cbImages.SelectedItem.ToString())
                {
                    //Store binary data read from the database in a byte array
                    string s = (string)row[1];

                    byte[] blob = System.Text.UTF8Encoding.ASCII.GetBytes(s);
                    MemoryStream stream = new MemoryStream();
                    stream.Write(blob, 0, blob.Length);
                    stream.Position = 0;

                    System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();

                    MemoryStream ms = new MemoryStream();
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                    ms.Seek(0, SeekOrigin.Begin);
                    bi.StreamSource = ms;
                    bi.EndInit();
                    image2.Source = bi;

                }
            }
        }

        private void insertImageData()
        {
            /*
            try
            {
                if (imageName != "")
                {
                    //Initialize a file stream to read the image file
                    FileStream fs = new FileStream(imageName, FileMode.Open, FileAccess.Read);

                    //Initialize a byte array with size of stream
                    byte[] imgByteArr = new byte[fs.Length];

                    //Read data from the file stream and put into the byte array
                    fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));

                    //Close a file stream
                    fs.Close();

                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string sql = "insert into tbl_Image(id,img) values('" + TextBox.Text + "', @image)";
                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            //Pass byte array into database
                            cmd.Parameters.Add(new MySqlParameter("image", imgByteArr));
                            int result = cmd.ExecuteNonQuery();
                            if (result == 1)
                            {
                                MessageBox.Show("Image added successfully.");
                                BindImageList();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } */


        }

        private void BindImageList()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM tbl_Image", conn))
                    {
                        ds = new DataSet("myDataSet");
                        adapter.Fill(ds);
                        DataTable dt = ds.Tables[0];

                        cbImages.Items.Clear();

                        foreach (DataRow dr in dt.Rows)
                            cbImages.Items.Add(dr["id"].ToString());

                        cbImages.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
    }
}
