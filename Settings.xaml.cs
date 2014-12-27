using System;
using System.Collections.Generic;
using System.IO;
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
using Gat.Controls;
using Gat.Controls.Model;
using Microsoft.Win32;
using VKAUDIO.Configuration;
using Path = System.IO.Path;

namespace VKAUDIO
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            DirectoryTextBox.Text = ApplicationConfiguration.Instance.SavingDirectory;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(DirectoryTextBox.Text))
            {
                MessageBox.Show("Неправильная папка");
                return;
            }
            ApplicationConfiguration.Instance.SavingDirectory = DirectoryTextBox.Text;
            
            try
            {
                ApplicationConfiguration.Instance.Save();
                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось сохранить настройки");
            }
           
           
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenDialogView openDialog = new Gat.Controls.OpenDialogView(); 
                Gat.Controls.OpenDialogViewModel vm = (Gat.Controls.OpenDialogViewModel)openDialog.DataContext;
                vm.IsDirectoryChooser = true;
                vm.StartupLocation = WindowStartupLocation.CenterOwner;
                var result = vm.Show();
                if (result == true)
                {
                    DirectoryTextBox.Text = vm.SelectedFolder.Path;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
