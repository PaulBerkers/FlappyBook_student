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

namespace FlappyBook
{
    /// <summary>
    /// Interaction logic for ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        public ResultWindow(int Number, int Score)
        {
            InitializeComponent();

            switch (Number)
            {
                case 0:
                    MedalBronzeImage.Visibility = Visibility.Visible;
                    break;
                case 1:
                    MedalSilverImage.Visibility = Visibility.Visible;
                    break;
                case 2:
                    MedalGoldImage.Visibility = Visibility.Visible;
                    break;
                case 3:
                    MedalGoldImage.Visibility = Visibility.Visible;
                    FeestjeImage.Visibility = Visibility.Visible;
                    FeestjeImage_Copy.Visibility = Visibility.Visible;
                    Frog.Visibility = Visibility.Visible;
                    Frog_Copy.Visibility = Visibility.Visible;

                    tbScoreBoard.Foreground = Brushes.Gold;
                    tbScoreBoard.FontSize = 35;
                    break;
            }

            tbScoreBoard.Text += Score;
        }

        private void BtnAgain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }
    }
}
