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

namespace FlappyBook
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (tbName.Text.Length >= 3)
            {
                GameWindow gwWindow = new GameWindow(tbName.Text);
                gwWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Uw naam moet minimaal 3 karakaters bevatten", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TbName_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox temp = sender as TextBox;
            if (temp.Text == temp.Tag.ToString())
            {
                temp.Text = "";
            }
        }

        private void textBox1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Z ]"))
            {
                e.Handled = true;
            }
        }
    }
}
