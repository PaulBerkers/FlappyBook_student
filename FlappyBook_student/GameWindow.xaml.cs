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
using System.Windows.Threading;
using System.Xml;

namespace FlappyBook
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private DispatcherTimer dtTimer;
        private DispatcherTimer dtLetItemFall;
        private DispatcherTimer dtShowREsultWindow;
        private Rectangle rectangleBottom;
        private Rectangle rectangleTop;
        private string Username;
        XmlDocument xmlDoc;
        int Score;
        int GetScoreCountdown = 0;
        int LevelLoader = 0;
        List<Rect> Blocks;
        int StartGameCountdown = 0;
        ResultWindow Result;

        public GameWindow(string name)
        {
            InitializeComponent();

            //Maak nieuwe timers
            dtTimer = new DispatcherTimer();
            dtTimer.Interval = TimeSpan.FromSeconds(1);
            dtTimer.Tick += DtTimer_Tick;

            dtLetItemFall = new DispatcherTimer();
            dtLetItemFall.Interval = TimeSpan.FromMilliseconds(20);
            dtLetItemFall.Tick += DtLetItemFall_Tick;

            dtShowREsultWindow = new DispatcherTimer();
            dtShowREsultWindow.Interval = TimeSpan.FromSeconds(1);
            dtShowREsultWindow.Tick += DtShowREsultWindow_Tick;

            //Laad het level in
            LoadLevel();

            //Zet de naam van de speler linksboven in het scherm
            SetName(name);

        }

        private void DtShowREsultWindow_Tick(object sender, EventArgs e)
        {
            GetScoreCountdown++;

            //Wacht 2 seconden voor dat hij het resultscherm laat zien
            if (GetScoreCountdown == 2)
            {
                Result = new ResultWindow(LevelLoader, Score);
                Result.Show();
                Close();
            }
        }

        public void SetName(string NameGiven)
        {
            //Haalt de username op
            this.Username = NameGiven;
            char[] usernameLetters = this.Username.ToCharArray();
            foreach (char letter in usernameLetters)
            {
                //Zet deze om in een image en zet die naar het scherm toe
                Image img = new Image();
                img.Source = new BitmapImage(new Uri(@"Assets/Letters/letter" + letter.ToString().ToUpper() + ".png", UriKind.Relative));
                spName.Children.Add(img);
            }
        }

        public void SetScore()
        {
            //Haalt de username op
            char[] SetTheScore = this.Score.ToString().ToCharArray();
            foreach (char number in SetTheScore)
            {
                //Zet deze om in een image en zet die naar het scherm toe
                Image img2 = new Image();
                img2.Source = new BitmapImage(new Uri(@"Assets/Numbers/number" + number.ToString().ToUpper() + ".png", UriKind.Relative));
                spScore.Children.Add(img2);
            }
        }

        private void LoadLevel()
        {
            dtTimer.Start();
            Blocks = new List<Rect>();

            xmlDoc = new XmlDocument();

            //Bekijkt welke level hij zit en laadt deze vervolgens in
            switch (LevelLoader)
            {
                case 0:
                    xmlDoc.Load(@"C:\Users\PaulB\Music\Overig WPF materiaal\FlappyBook_student\FlappyBook_student\Assets\XML\XMLFile1.xml"); //laad xmlbestand level 1 in
                    break;
                case 1:
                    xmlDoc.Load(@"C:\Users\PaulB\Music\Overig WPF materiaal\FlappyBook_student\FlappyBook_student\Assets\XML\XMLFile2.xml"); //laad xmlbestand level 2 in
                    break;
                case 2:
                    xmlDoc.Load(@"C:\Users\PaulB\Music\Overig WPF materiaal\FlappyBook_student\FlappyBook_student\Assets\XML\XMLFile3.xml"); //laad xmlbestand level 3 in
                    break;
            }
            
            //Maakt de rectangles aan
            XmlNodeList blocks = xmlDoc.SelectNodes("//GameLevel/Blocks/Block"); //Zoek nodes
            foreach (XmlNode block in blocks)
            {
                //Haalt alle gegevens op
                int Position = int.Parse(block.Attributes["Position"].InnerText);
                int GapHeight = int.Parse(block.Attributes["GapHeight"].InnerText);
                int PositionHeight = int.Parse(block.Attributes["PositionHeight"].InnerText);

                Color color = (Color)ColorConverter.ConvertFromString("#FF97714A");
                SolidColorBrush myBrush = new SolidColorBrush(color);

                Color colorGreen = (Color)ColorConverter.ConvertFromString("#FF96C23A");
                SolidColorBrush myBrush2 = new SolidColorBrush(colorGreen);

                //Maakt de rectangle Top aan
                rectangleTop = new Rectangle
                {
                    Height = 500 - GapHeight - PositionHeight,
                    Width = 75,
                    Fill = Brushes.LightBlue,
                    Stroke = Brushes.SkyBlue,
                    Margin = new Thickness(Position, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    StrokeThickness = 5
                };

                //Maakt de rectangle bottom aan
                rectangleBottom = new Rectangle
                {
                    Height = PositionHeight,
                    Width = 75,
                    Fill = myBrush,
                    Stroke = myBrush2,
                    Margin = new Thickness(Position, 500 - PositionHeight, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    StrokeThickness = 5
                };

                //Zet ze in de Rect list
                Blocks.Add(RectangleToRect(rectangleTop));
                Blocks.Add(RectangleToRect(rectangleBottom));

                //Zet ze op het scherm
                gGame.Children.Add(rectangleTop);
                gGame.Children.Add(rectangleBottom);

            }
        }

        private void DtLetItemFall_Tick(object sender, EventArgs e)
        {
            //Haalt de foto op en zet deze 5 naar voren en 2 naar beneden
            imgPlayer.Margin = new Thickness(imgPlayer.Margin.Left + 5, imgPlayer.Margin.Top + 2, imgPlayer.Margin.Right, imgPlayer.Margin.Bottom);
            Rect ImgPlayerRect = new Rect(imgPlayer.Margin.Left, imgPlayer.Margin.Top, imgPlayer.ActualWidth, imgPlayer.ActualHeight);
            Rect ImgStar = new Rect(StarImage.Margin.Left, StarImage.Margin.Top, StarImage.ActualWidth, StarImage.ActualHeight);

            spScore.Children.Clear();
            Score++;
            SetScore();

            if (Keyboard.IsKeyDown(Key.Space))
            {
                //Zet de foto 15 naar boven
                imgPlayer.Margin = new Thickness(imgPlayer.Margin.Left, imgPlayer.Margin.Top - 15, imgPlayer.Margin.Right, imgPlayer.Margin.Bottom);
            }

            foreach (Rect block in Blocks)
            {
                //Bots de foto met een block
                if (ImgPlayerRect.IntersectsWith(block))
                {
                    switch (LevelLoader)
                    {
                        case 0:
                            dtShowREsultWindow.Start();
                            GameOverImage.Visibility = Visibility.Visible;
                            LoserGif.Visibility = Visibility.Visible;
                            LoserGif_Copy.Visibility = Visibility.Visible;
                            dtLetItemFall.Stop();


                            break;
                        case 1:
                            dtShowREsultWindow.Start();
                            GameOverImage.Visibility = Visibility.Visible;
                            dtLetItemFall.Stop();
                            
                            break;
                        case 2:
                            dtShowREsultWindow.Start();
                            GameOverImage.Visibility = Visibility.Visible;
                            dtLetItemFall.Stop();
                            
                            break;
                    }
                }
            }

            //Bots de player met de star doe dan het volgende
            if (ImgPlayerRect.IntersectsWith(ImgStar))
            {
                dtTimer.Stop();
                
                gGame.Children.Clear();
                Blocks.Clear();

                LevelLoader++;
                LoadLevel();
                imgPlayer.Margin = new Thickness(10, 200, 0, 0);
                if (LevelLoader == 3)
                {
                    dtLetItemFall.Stop();

                    Result = new ResultWindow(LevelLoader, Score);
                    Result.Show();
                    Close();
                }
            }
        }

        private void DtTimer_Tick(object sender, EventArgs e)
        {
            StartGameCountdown++;

            //De countdown tot start
            switch (StartGameCountdown)
            {
                case 0:
                    GetReadyImage.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    GetReadyImage.Visibility = Visibility.Hidden;
                    NumberThree.Visibility = Visibility.Visible;
                    break;
                case 2:
                    NumberTwo.Visibility = Visibility.Visible;
                    NumberThree.Visibility = Visibility.Hidden;
                    break;
                case 3:
                    NumberOne.Visibility = Visibility.Visible;
                    NumberTwo.Visibility = Visibility.Hidden;
                    break;
                case 4:
                    dtLetItemFall.Start();
                    NumberOne.Visibility = Visibility.Hidden;
                    break;
            }
        }

        //Maakt van rectangle een Rect
        Rect RectangleToRect(Rectangle rectangle)
        {
            return new Rect() { X = rectangle.Margin.Left, Y = rectangle.Margin.Top , Width = rectangle.Width, Height = rectangle.Height };
        }

    }
}
