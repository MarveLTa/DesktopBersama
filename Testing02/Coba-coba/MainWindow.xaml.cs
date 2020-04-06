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

namespace Testing02
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"The description is: {this.DescriptionText.Text}");
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            this.WeldCheckBox.IsChecked = this.AssemblyCheckBox.IsChecked = this.PLasmaCheckBox.IsChecked = this.PurchaseCheckBox.IsChecked = this.LatheCheckBox.IsChecked = this.DrillCheckBox.IsChecked = this.LaserCheckBox.IsChecked = this.FoldCheckBox.IsChecked = this.RollCheckBox.IsChecked = this.SawCheckBox.IsChecked = false;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.LengthText.Text += (string)((CheckBox)sender).Content;
        }

        private void FinishDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.NoteText == null)
                return;

            var combo = (ComboBox)sender;
            var value = (ComboBoxItem)combo.SelectedValue;
            this.NoteText.Text = (string)value.Content;
           // this.NoteText.Text = (string)((ComboBoxItem)((ComboBox)sender).SelectedValue).Content;
        }

        private void SupplierInformationText_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.MassText.Text = this.SupplierInformationText.Text;
        }
    }
}
