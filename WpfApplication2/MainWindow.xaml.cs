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
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Drawing;
namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Person ActivePlayer = new Person(1,3,1);

        public MainWindow()
        {
            InitializeComponent();
            //InitBinding();
        }

        private void btnLosuj_Click(object sender, RoutedEventArgs e)
        {
            bool DiceSelected = false;

            if (ActivePlayer.DrawsLeft == 3)
            {
                cbKostka1.IsChecked = true;
                cbKostka2.IsChecked = true;
                cbKostka3.IsChecked = true;
                cbKostka4.IsChecked = true;
                cbKostka5.IsChecked = true;
            }


            if (ActivePlayer.DrawsLeft > 0)
            {
                if (cbKostka1.IsChecked == true)
                {
                    DiceSelected = true;
                    imgKostka1.Source = Kostka.Main();
                    imgKostka1.ToolTip = imgKostka1.Source.ToString().Substring(imgKostka1.Source.ToString().Length - 5, 1);
                    cbKostka1.IsChecked = false;
                }
                if (cbKostka2.IsChecked == true)
                {
                    DiceSelected = true;
                    imgKostka2.Source = Kostka.Main();
                    imgKostka2.ToolTip = imgKostka2.Source.ToString().Substring(imgKostka2.Source.ToString().Length - 5, 1);
                    cbKostka2.IsChecked = false;
                }
                if (cbKostka3.IsChecked == true)
                {
                    DiceSelected = true;
                    imgKostka3.Source = Kostka.Main();
                    imgKostka3.ToolTip = imgKostka3.Source.ToString().Substring(imgKostka3.Source.ToString().Length - 5, 1);
                    cbKostka3.IsChecked = false;
                }
                if (cbKostka4.IsChecked == true)
                {
                    DiceSelected = true;
                    imgKostka4.Source = Kostka.Main();
                    imgKostka4.ToolTip = imgKostka4.Source.ToString().Substring(imgKostka4.Source.ToString().Length - 5, 1);
                    cbKostka4.IsChecked = false;
                }
                if (cbKostka5.IsChecked == true)
                {
                    DiceSelected = true;
                    imgKostka5.Source = Kostka.Main();
                    imgKostka5.ToolTip = imgKostka5.Source.ToString().Substring(imgKostka5.Source.ToString().Length - 5, 1);
                    cbKostka5.IsChecked = false;
                }
                if (DiceSelected) ActivePlayer.DrawsLeft--;// = ActivePlayer.DrawsLeft - 1;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Możesz przelosować kości tylko 2 razy.");
            }
            //MessageBox.Show(Kostka.Main());
        }

        private void imgKostka_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var myImage = sender as System.Windows.Controls.Image;
            String strName = myImage.Name.Replace("img","cb");
            IEnumerable<System.Windows.Controls.CheckBox> collection = MainGrid.Children.OfType<System.Windows.Controls.CheckBox>();

            string UriStringCheck = "/WpfApplication2;Component/Kostki/r" + myImage.ToolTip + ".png";
            string UriStringUncheck = "/WpfApplication2;Component/Kostki/b" + myImage.ToolTip + ".png";

            foreach (System.Windows.Controls.CheckBox c in collection){
                if (c.Name == strName)
                {
                    var myControl = c as System.Windows.Controls.CheckBox;
                    if (myControl.IsChecked == true)
                    {
                        myControl.IsChecked = false;
                        myImage.Source = new BitmapImage(new Uri("/WpfApplication2;Component/Kostki/b" + myImage.ToolTip + ".png", UriKind.RelativeOrAbsolute));
                    }
                    else
                    {
                        myControl.IsChecked = true;
                        myImage.Source = new BitmapImage(new Uri("/WpfApplication2;Component/Kostki/r" + myImage.ToolTip + ".png", UriKind.RelativeOrAbsolute));
                    }
                    break;
                }
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            btnStart.Visibility = Visibility.Collapsed;
            string input = "";
            Int32 numCols = Int32.Parse(Inputbox.ShowInputDialog(ref input, "Podaj liczbę graczy", 500,70));

            //create column for each player
            for (int i = 0; i < numCols; ++i) 
                resultTable.ColumnDefinitions.Add(new ColumnDefinition());
            //create row for each possible category
            for (int i = 0; i < 18; ++i)    
                resultTable.RowDefinitions.Add(new RowDefinition());
            //set height of each created row
            foreach (var g in resultTable.RowDefinitions) 
            {
                g.Height = new GridLength(1, GridUnitType.Auto);
            }
            // set width of each created column
            foreach (var g in resultTable.ColumnDefinitions) 
            {
                g.Width = new GridLength(1,GridUnitType.Star);
                g.MinWidth = 120 ;
            }

            //create laber for each cell in table
            for (int j = 0; j < numCols; j++) 
            {
                for (int i = 0; i < 18; ++i)
                {
                    int idx = resultTable.Children.Add(new System.Windows.Controls.Label());
                    System.Windows.Controls.Label x = resultTable.Children[idx] as System.Windows.Controls.Label;
                    x.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                    
                    //check wheteher it is first row (names)
                    if (i == 0)
                    {
                        string imie = Inputbox.ShowInputDialog(ref input, "Podaj imię gracza nr" + (j + 1), 500, 70);
                        x.Content = imie;
                        x.FontWeight = FontWeights.ExtraBold;
                        x.Name = "Player" + (j+1);
                        RegisterName(x.Name, x);
                    }
                    else
                    {
                        x.Content = string.Empty;
                        x.Name = "P" + (j+1) + "B" +i;
                        RegisterName(x.Name, x);
                    }   
                    x.Height = 20;
                    x.FontSize = 18;
                    x.Margin = new System.Windows.Thickness(5);
                    x.Padding = new System.Windows.Thickness(1);
                    x.SetValue(Grid.RowProperty, i);
                    x.SetValue(Grid.ColumnProperty, j);
                    x.SetValue(Grid.ColumnSpanProperty, 1);
                    x.SetValue(Grid.RowSpanProperty, 1);
                    
                }
            }
            ActivePlayer.PersonId = 1;
            ActivePlayer.NumberOfPlayers= numCols;

        }

        private void btnFigury_Click(object sender, RoutedEventArgs e)
        {
            //does not let the player to use result of the previous player
            if (ActivePlayer.DrawsLeft == 3)
            {
                System.Windows.Forms.MessageBox.Show("Najpierw przelosuj kości!");
                return;
            }
            //System.Windows.Controls.Button Przycisk = (sender as System.Windows.Controls.Button);
            System.Windows.Controls.Button btn = (System.Windows.Controls.Button)sender;
                 
            int index = 7;
            //System.Windows.Forms.MessageBox.Show(Convert.ToString(btn.Parent.TransformToVisual(btn)));
            foreach (Object child in Figury.Children)
            {
                index = index + 1;
                if (child == sender) break;

            }
            System.Windows.Controls.Label labelind = (resultTable.RowDefinitions.ElementAt(index).FindName("P"+ActivePlayer.PersonId+"B"+index) as System.Windows.Controls.Label);
            //check whether clicked button was clicket for the first time, if yes do not do anything
            if ((string)labelind.Content.ToString() != "") return;

            labelind.Content = liczpunkty.figury(imgKostka1.ToolTip.ToString()[0], imgKostka2.ToolTip.ToString()[0], imgKostka3.ToolTip.ToString()[0], imgKostka4.ToolTip.ToString()[0], imgKostka5.ToolTip.ToString()[0], btn);
            //reset draws for next player
            ActivePlayer.DrawsLeft = 3;

            if (ActivePlayer.PersonId + 1 > ActivePlayer.NumberOfPlayers)
            {
                System.Windows.Controls.Label playerNameLabel = (resultTable.FindName("Player" + ActivePlayer.PersonId) as System.Windows.Controls.Label);
                playerNameLabel.Foreground = System.Windows.Media.Brushes.Black;
                ActivePlayer.PersonId = 1;
                playerNameLabel = (resultTable.FindName("Player" + ActivePlayer.PersonId) as System.Windows.Controls.Label);
                playerNameLabel.Foreground = System.Windows.Media.Brushes.Green;
            }
            else
            {
                System.Windows.Controls.Label playerNameLabel = (resultTable.FindName("Player" + ActivePlayer.PersonId) as System.Windows.Controls.Label);
                playerNameLabel.Foreground = System.Windows.Media.Brushes.Black;
                ActivePlayer.PersonId = ActivePlayer.PersonId + 1;
                playerNameLabel = (resultTable.FindName("Player" + ActivePlayer.PersonId) as System.Windows.Controls.Label);
                playerNameLabel.Foreground = System.Windows.Media.Brushes.Green;
            }

        }
        private void btnSzkola_Click(object sender, RoutedEventArgs e)
        {
            //does not let the player to use result of the previous player
            if (ActivePlayer.DrawsLeft == 3)
            {
                System.Windows.Forms.MessageBox.Show("Najpierw przelosuj kości!");
                return;
            }

            System.Windows.Controls.Button btn = (System.Windows.Controls.Button)sender;            
            int index = 0;
 
            foreach (Object child in Szkola.Children)
            {
                index = index + 1;
                if (child == sender) break;

            }

            System.Windows.Controls.Label labelind = (resultTable.RowDefinitions.ElementAt(index).FindName("P" + ActivePlayer.PersonId + "B" + index) as System.Windows.Controls.Label);
            if ((string) labelind.Content.ToString() != "") return;

            labelind.Content = liczpunkty.szkola(imgKostka1.ToolTip.ToString(), imgKostka2.ToolTip.ToString(), imgKostka3.ToolTip.ToString(), imgKostka4.ToolTip.ToString(), imgKostka5.ToolTip.ToString(), btn);
            ActivePlayer.DrawsLeft = 3;

           

            if (ActivePlayer.PersonId + 1 > ActivePlayer.NumberOfPlayers)
            {
                System.Windows.Controls.Label playerNameLabel = (resultTable.FindName("Player" + ActivePlayer.PersonId) as System.Windows.Controls.Label);
                playerNameLabel.Foreground = System.Windows.Media.Brushes.Black;
                ActivePlayer.PersonId = 1;
                playerNameLabel = (resultTable.FindName("Player" + ActivePlayer.PersonId) as System.Windows.Controls.Label);
                playerNameLabel.Foreground = System.Windows.Media.Brushes.Green;
            }
            else
            {
                System.Windows.Controls.Label playerNameLabel = (resultTable.FindName("Player" + ActivePlayer.PersonId) as System.Windows.Controls.Label);
                playerNameLabel.Foreground = System.Windows.Media.Brushes.Black;
                ActivePlayer.PersonId = ActivePlayer.PersonId + 1;
                playerNameLabel = (resultTable.FindName("Player" + ActivePlayer.PersonId) as System.Windows.Controls.Label);
                playerNameLabel.Foreground = System.Windows.Media.Brushes.Green;
            }
        }
        
    }


}
    public class Person
    {
        public int PersonId { get; set; }
        public int DrawsLeft { get; set; }
        public int NumberOfPlayers { get; set; }

        public Person(int nPersonId, int nDraws,
            int Number)
        {
            PersonId = nPersonId;
            DrawsLeft = nDraws;
            NumberOfPlayers = Number;
        }
    }
    public class Kostka
    {
        public static ImageSource Main()
        {
            Random liczba = new Random();
            {
                using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                {
                    byte[] randomNumber = new byte[4];//4 for int32
                    rng.GetBytes(randomNumber);
                    Int32 value = BitConverter.ToInt32(randomNumber, 0);
                     
                    BitmapImage myBitmapImage = new BitmapImage();
                    Int32 numerek = Math.Abs(value % 6)+1; // (liczba.Next(6) + 1);
                    // BitmapImage.UriSource must be in a BeginInit/EndInit block
                    myBitmapImage.BeginInit();
                    string UriString = "/WpfApplication2;Component/Kostki/b" + numerek + ".png";
                    //"pack://application:,,,/Subfolder/ContentFile.xaml"
                    //UriString = @"D:\VisualStudio\WPF App\WpfApplication2\WpfApplication2\Kostki\" + 1 + ".jpg";
                    myBitmapImage.UriSource = new Uri(UriString, UriKind.RelativeOrAbsolute);

                    // To save significant application memory, set the DecodePixelWidth or  
                    // DecodePixelHeight of the BitmapImage value of the image source to the desired 
                    // height or width of the rendered image. If you don't do this, the application will 
                    // cache the image as though it were rendered as its normal size rather then just 
                    // the size that is displayed.
                    // Note: In order to preserve aspect ratio, set DecodePixelWidth
                    // or DecodePixelHeight but not both.
                    myBitmapImage.DecodePixelWidth = 150;
                    myBitmapImage.EndInit();
                    ImageSource ImgSource = myBitmapImage;
                    //ImgSource.url (source = "Kostka/kostki.jpg";
                    //MessageBox.Show(numerek.ToString());
                    return ImgSource;
                }
            }
        }
    }
    public class Inputbox
    {
        public static string ShowInputDialog(ref string input, string what, Int32 i32Width, Int32 i32Height)
        {
            System.Drawing.Size size = new System.Drawing.Size(i32Width, i32Height);
            Form inputBox = new Form();

            inputBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            inputBox.Text = what;

            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox();
            textBox.Size = new System.Drawing.Size(size.Width - 10, 23);
            textBox.Location = new System.Drawing.Point(5, 5);
            //textBox.Text = input;
            inputBox.Controls.Add(textBox);

            System.Windows.Forms.Button okButton = new System.Windows.Forms.Button();
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(75, 23);
            okButton.Text = "&OK";
            okButton.Location = new System.Drawing.Point(size.Width - 80 - 80, 39);
            inputBox.Controls.Add(okButton);

            System.Windows.Forms.Button cancelButton = new System.Windows.Forms.Button();
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 23);
            cancelButton.Text = "&Cancel";
            cancelButton.Location = new System.Drawing.Point(size.Width - 80, 39);
            inputBox.Controls.Add(cancelButton);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            DialogResult result = inputBox.ShowDialog();
            input = textBox.Text;
            return input;
        }
    }
    public class liczpunkty
    {
        public static double szkola(string dice1, string dice2, string dice3, string dice4, string dice5, System.Windows.Controls.Button btnButton)
        {
            string sDices = dice1 + dice2 + dice3 + dice4 + dice5;
            double nDicesCount = 0;
            foreach (char c in sDices)
                    if (c == btnButton.Name[9]) nDicesCount++;

            return (nDicesCount - 3) * char.GetNumericValue(btnButton.Name[9]);
        }
        public static double figury(char dice1, char dice2, char dice3, char dice4, char dice5, System.Windows.Controls.Button btnButton)
        {
            char[] arrDices = new char[] { dice1, dice2, dice3, dice4, dice5 };
            double wynik = 0;
            Array.Sort(arrDices);
            Array.Reverse(arrDices);
            //arrDices.OrderByDescending(x => x); //.ToList();
            if (btnButton.Name == "btnPara")
            {
                for (int i = 0; i < 4; i++)
                {
                    if (arrDices[i] == arrDices[i + 1])
                    {
                        wynik = 2*char.GetNumericValue(arrDices[i]);
                        break;
                    }
                }
            }
      
            if(btnButton.Name == "btnDwiePary")
            {
                for (int i = 0; i < 4; i++)
                {
                    if (arrDices[i] == arrDices[i + 1])
                    {
                        wynik = wynik +2 * char.GetNumericValue(arrDices[i]);
                        i++;
                    }
                }
            }

            if (btnButton.Name == "btnTrojka")
            {
                for (int i = 0; i < 3; i++)
                {
                    if (arrDices[i] == arrDices[i + 2])
                    {
                        wynik = 3 * char.GetNumericValue(arrDices[i]);
                    
                    }
                }
            }

            if (btnButton.Name == "btnMalyStrit")
            {
                if (arrDices[0] == '5' & arrDices[1] == '4' & arrDices[2] == '3' & arrDices[3] == '2' & arrDices[4] == '1') wynik = 15;
            }
                           
            if(btnButton.Name == "btnDuzyStrit")
            {
                if (arrDices[0] == '6' & arrDices[1] == '5' & arrDices[2] == '4' & arrDices[3] == '3' & arrDices[4] == '2') wynik = 20;
            }
                                  
            if(btnButton.Name == "btnFull")
            {
                if (arrDices[1] == arrDices[0] & arrDices[3] == arrDices[4] & (arrDices[2] == arrDices[0] || arrDices[2] == arrDices[4])) wynik = 2 * char.GetNumericValue(arrDices[0]) + 2 * char.GetNumericValue(arrDices[4]) + char.GetNumericValue(arrDices[2])+30;
            }
                                         
            if(btnButton.Name == "btnKareta")
            {
                for (int i = 0; i < 2; i++)
                {
                    if (arrDices[i] == arrDices[i + 3])
                    {
                        wynik = 4 * char.GetNumericValue(arrDices[i])+40;
                    }
                }
            }

            if (btnButton.Name == "btnGeneral")
            { 
                if (arrDices[0] == arrDices[4])
                {
                    wynik = 5 * char.GetNumericValue(arrDices[0])+50;
                }
            }

            if (btnButton.Name == "btnSzansa")
            {
                for (int i = 0; i < 5; i++)
                {
                    wynik = wynik + char.GetNumericValue(arrDices[i]);
                }
            }
            return wynik;
        }
    }


