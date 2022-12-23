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
using System.Windows.Threading;
using System.Windows.Media.Imaging;

namespace Porject_Draaiboek
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int spelerHeeftAas = 0;
        private int bankHeeftAas = 0;
        private int spelerPunten = 0;
        private int bankPunten = 0;
        private int rondeNummer = 1;
        private Random random = new Random();
        DispatcherTimer seconden = new DispatcherTimer();
        DispatcherTimer secondenStand = new DispatcherTimer();
        private DispatcherTimer klok = new DispatcherTimer();
        private int aantalKaartenSpeler = 0;
        private bool isSpeler = false;
        private bool isKaartGenomen = false;
        private string[] kaartenDeck = {"aasharten", "2harten" , "3harten" , "4harten" , "5harten" ,"6harten" ,"7harten" ,"8harten" ,"9harten" ,"10harten" ,"boerharten" ,"koninginharten" ,"koningharten",
        "aasruiten", "2ruiten" , "3ruiten" , "4ruiten" , "5ruiten" ,"6ruiten" ,"7ruiten" ,"8ruiten" ,"9ruiten" ,"10ruiten" ,"boerruiten" ,"koninginruiten" ,"koningruiten",
                "aasklaveren", "2klaveren" , "3klaveren" , "4klaveren" , "5klaveren" ,"6klaveren" ,"7klaveren" ,"8klaveren" ,"9klaveren" ,"10klaveren" ,"boerklaveren" ,"koninginklaveren" ,"koningklaveren",
                "aasschoppen", "2schoppen" , "3schoppen" , "4schoppen" , "5schoppen" ,"6schoppen" ,"7schoppen" ,"8schoppen" ,"9schoppen" ,"10schoppen" ,"boerschoppen" ,"koninginschoppen" ,"koningschoppen" };
        private bool isDoubleDown = false;
        private List<string> deckInGebruik;
        public MainWindow()
        {
            InitializeComponent();
            klok.Interval = new TimeSpan(0, 0, 1);
            klok.Tick += Klok_Tick;
            klok.Start();
            TxtTijd.Text = DateTime.Now.ToLongTimeString();
            seconden.Interval = TimeSpan.FromMilliseconds(1000);
            seconden.Tick += KaartDelen;
            secondenStand.Interval = TimeSpan.FromMilliseconds(1000);
            secondenStand.Tick += StandDelen;
            deckInGebruik = kaartenDeck.ToList();
        }
        private void Klok_Tick(object sender, EventArgs e)
        {
            TxtTijd.Text = DateTime.Now.ToLongTimeString();
        }
        private void TimerDeel()
        {
            seconden.Start();
            btn_hit.IsEnabled = false;
            btn_stand.IsEnabled = false;
            Btn_DoubleDown.IsEnabled = false;
        }
        private void KaartDelen(object sender, EventArgs e)
        {
            if (aantalKaartenSpeler < 2)
            {
                SpelerKaart();
                aantalKaartenSpeler++;
            }
            else
            {
                isSpeler = false;
                BankKaart();
                seconden.Stop();
                btn_hit.IsEnabled = true;
                btn_stand.IsEnabled = true;
                Btn_DoubleDown.IsEnabled = true;
                isSpeler = true;
            }
        }
        private void SpelerKaart()
        {
            string spelerNummer = Trek_Nummer();
            string tekenSpeler = Trek_Teken();

            DeckHervullen();
            KaartenDeckGebruiken(tekenSpeler, spelerNummer);

        }
        private void DeckHervullen()
        {
            if (deckInGebruik.Count == 0)
            {
                MessageBox.Show("kaarten hervuld",
                    "Hulp",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                    );
                deckInGebruik = kaartenDeck.ToList();
                int AantalKaarten = deckInGebruik.Count;
                Aantal_KaartenOver.Content = AantalKaarten;
            }
        }
        private void KaartenDeckGebruiken(string teken, string nummer)
        {

            string kaart = nummer + teken;

            if (deckInGebruik.Contains(kaart))
            {
                deckInGebruik.Remove(kaart);
                isKaartGenomen = true;
                int AantalKaarten = deckInGebruik.Count;
                Aantal_KaartenOver.Content = AantalKaarten;

                GeefBankOfSpelerZijnVisueleKaart(teken, nummer);
            }
            else
            {
                GeefKaartenSpelerBank(isSpeler);
            }

        }
        private void GeefBankOfSpelerZijnVisueleKaart(string teken, string nummer)
        {
            if (isSpeler)
            {
                txt_speler.Text += nummer + " " + teken + "\n";

                spelerPunten += SpelerPuntenTellen(nummer);

                speler_ptn.Content = HeeftAasSpeler();

                speler_ptn.Content = spelerPunten;

                SpelerKaartFoto.Source = new BitmapImage(new Uri($"/assets/{nummer + teken}.png", UriKind.Relative));
                Image afbeelding = new Image();
                afbeelding.Source = new BitmapImage(new Uri($"/assets/{nummer + teken}.png", UriKind.Relative));
                Alle_Kaarten.Items.Add(afbeelding);
            }
            else if (isDoubleDown)
            {
                txt_speler.Text += nummer + " " + teken + "\n";



                spelerPunten += SpelerPuntenTellen(nummer);

                speler_ptn.Content = HeeftAasSpeler();

                speler_ptn.Content = spelerPunten;

                BitmapImage afbeelding = new BitmapImage();
                afbeelding = new BitmapImage();
                afbeelding.BeginInit();
                afbeelding.UriSource = new Uri($"/assets/{nummer + teken}.png", UriKind.Relative);
                afbeelding.Rotation = Rotation.Rotate90;
                afbeelding.EndInit();
                Image foto_NieuweKaart_DoubleDown = new Image();
                foto_NieuweKaart_DoubleDown.Source = afbeelding;
                DoubleDown_Kaart.Source = afbeelding;
                Alle_Kaarten.Items.Add(foto_NieuweKaart_DoubleDown);
                vieuwbox.Visibility = Visibility.Visible;
                isDoubleDown = false;
            }
            else
            {

                txt_Bank.Text += nummer + " " + teken + "\n";

                bankPunten += BankPuntenTellen(nummer);

                bank_ptn.Content = HeeftAasbank();

                bank_ptn.Content = bankPunten;

                BankKaartFoto.Source = new BitmapImage(new Uri($"/assets/{nummer + teken}.png", UriKind.Relative));
                Image afbeelding = new Image();
                afbeelding.Source = new BitmapImage(new Uri($"/assets/{nummer + teken}.png", UriKind.Relative));
                Alle_Kaarten_Bank.Items.Add(afbeelding);
            }
        }

        private void BankKaart()
        {
            string bank1 = Trek_Nummer();
            string bankteken = Trek_Teken();

            DeckHervullen();
            KaartenDeckGebruiken(bankteken, bank1);

        }
        private void TimerStand()
        {
            secondenStand.Start();
        }
        private void StandDelen(object sender, EventArgs e)
        {
            if (bankPunten < 17)
            {
                isSpeler = false;
                BankKaart();

            }
            else
            {


                uitkomst_txt.Content = HeeftGewonnen();
                if (isDoubleDown)
                {
                    KapitaalOptellenOfAftrekken();
                    KapitaalOptellenOfAftrekken();
                }
                else
                {
                    KapitaalOptellenOfAftrekken();
                }

                KapitaalOnderNull(kapitaal_txt.Text);
                LastGame();
                Historiek();
                secondenStand.Stop();
            }
        }
        private void GeefKaartenSpelerBank(bool isSpeler)
        {
            if (isSpeler)
            {
                SpelerKaart();
            }
            else
            {
                BankKaart();
            }
        }
        private void KapitaalOnderNull(string kapitaal)
        {
            int waarde = Convert.ToInt32(kapitaal);
            if (waarde == 0)
            {
                MessageBox.Show("Je hebt niet genoeg kapitaal",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                    );
                nieuw_spel.IsEnabled = true;
                btn_deel.IsEnabled = false;
                btn_hit.IsEnabled = false;
                btn_stand.IsEnabled = false;

            }
            else
            {

            }
        }
        private void SliderMaxEnMin()
        {
            Inzet_Slider.Minimum = Math.Round(Convert.ToDouble(kapitaal_txt.Text) / 10);
            Inzet_Slider.Maximum = Convert.ToDouble(kapitaal_txt.Text);

            inzet_txt.Text = Convert.ToString(Inzet_Slider.Value);
        }
        private void KapitaalOptellenOfAftrekken()
        {
            if (uitkomst_txt.Content == "gewonnen")
            {
                kapitaal_txt.Text = Convert.ToString(Convert.ToInt32(kapitaal_txt.Text) + Convert.ToInt32(inzet_txt.Text));
            }
            else if (uitkomst_txt.Content == "verloren")
            {
                kapitaal_txt.Text = Convert.ToString(Convert.ToInt32(kapitaal_txt.Text) - Convert.ToInt32(inzet_txt.Text));
            }
            else
            {

            }
            LastGame();
        }
        private int HeeftAasSpeler()
        {
            if (spelerPunten > 21 & spelerHeeftAas > 0)
            {
                spelerHeeftAas -= 1;
                spelerPunten -= 10;
            }
            else
            {

            }
            return spelerPunten;

        }
        private int HeeftAasbank()
        {
            if (bankPunten > 21 & bankHeeftAas > 0)
            {
                bankPunten -= 10;
                bankHeeftAas -= 1;


            }
            return bankPunten;
        }
        private int WaardeKaart(string getal)
        {
            int nummer;
            if (getal == "boer" || getal == "koningin" || getal == "koning")
            {
                nummer = 10;
            }
            else if (getal == "aas")
            {
                nummer = 11;
                spelerHeeftAas += 1;
            }
            else
            {
                nummer = Convert.ToInt32(getal);
            }
            spelerPunten += nummer;
            return spelerPunten;
        }
        private string HeeftGewonnen()
        {
            string uitkomst = "";

            if (spelerPunten > 21)
            {
                uitkomst = "verloren";
            }
            else if (bankPunten > 21)
            {
                uitkomst = "gewonnen";
            }
            else if (spelerPunten == bankPunten)
            {
                uitkomst = "push";
            }
            else if (spelerPunten > bankPunten & bankPunten < 21)
            {
                uitkomst = "gewonnen";
            }
            else
            {
                uitkomst = "verloren";
            }
            return uitkomst;
        }
        private void CheckVerlorenHit(object sender, RoutedEventArgs e)
        {
            HeeftAasSpeler();

            string uitkomst = "";

            if (spelerPunten > 21)
            {
                Btn_Stand_Click(sender, e);
            }
            else
            {

            }
        }
        private string Trek_Nummer()
        {
            int number = random.Next(1, 14);
            string uitput = "";
            switch (number)
            {
                case 1:
                    uitput += "aas";
                    break;
                case 2:
                    uitput += "2";
                    break;
                case 3:
                    uitput += "3";
                    break;
                case 4:
                    uitput += "4";
                    break;
                case 5:
                    uitput = "5";
                    break;
                case 6:
                    uitput += "6";
                    break;
                case 7:
                    uitput += "7";
                    break;
                case 8:
                    uitput += "8";
                    break;
                case 9:
                    uitput += "9";
                    break;
                case 10:
                    uitput += "10";
                    break;
                case 11:
                    uitput += "boer";
                    break;
                case 12:
                    uitput += "koningin";
                    break;
                case 13:
                    uitput += "koning";
                    break;
                default:
                    break;
            }

            return uitput;
        }
        private string Trek_Teken()
        {

            int teken = random.Next(1, 5);
            string uitput = "";


            switch (teken)
            {
                case 1:
                    uitput += "harten";
                    break;
                case 2:
                    uitput += "ruiten";
                    break;
                case 3:
                    uitput += "klaveren";
                    break;
                case 4:
                    uitput += "schoppen";
                    break;
            }
            return uitput;
        }
        private int BankPuntenTellen(string bank1)
        {
            int bank1n = 0;

            if (bank1 == "boer" || bank1 == "koningin" || bank1 == "koning")
            {
                bank1n = 10;
            }
            else if (bank1 == "aas")
            {
                bank1n = 11;
                bankHeeftAas += 1;
            }
            else if (Convert.ToInt32(bank1) > 0)
            {
                bank1n = Convert.ToInt32(bank1);
            }

            return bank1n;
        }
        private int SpelerPuntenTellen(string speler)
        {
            int speler1n = 0;

            if (speler == "boer" || speler == "koningin" || speler == "koning")
            {
                speler1n = 10;
            }
            else if (speler == "aas")
            {
                speler1n = 11;
                spelerHeeftAas += 1;
            }
            else if (Convert.ToInt32(speler) > 0)
            {
                speler1n = Convert.ToInt32(speler);
            }
            return speler1n;
        }
        private void Historiek()
        {
            string laatste_Spel_gespeeld = Convert.ToString(Laatste_Spel.Content);
            txt_historiek.Text += rondeNummer + " " + laatste_Spel_gespeeld + "\n";
            ++rondeNummer;
        }
        private void LastGame()
        {
            int inzetPunten;
            string laatste_Spel_gespeeld;

            if (uitkomst_txt.Content == "gewonnen")
            {                
                inzetPunten = Convert.ToInt32(inzet_txt.Text);

                laatste_Spel_gespeeld = $"+{inzetPunten} - {spelerPunten} / {bankPunten}";
                Laatste_Spel.Content = laatste_Spel_gespeeld;                
            }
            else if (uitkomst_txt.Content == "verloren" || spelerPunten > 21)
            {
                inzetPunten = Convert.ToInt32(inzet_txt.Text);

                laatste_Spel_gespeeld = $"-{inzetPunten} - {spelerPunten} / {bankPunten}";
                Laatste_Spel.Content = laatste_Spel_gespeeld;                
            }
            else 
            {
                inzetPunten = Convert.ToInt32(inzet_txt.Text);

                laatste_Spel_gespeeld = $"0 - {spelerPunten} / {bankPunten}";
                Laatste_Spel.Content = laatste_Spel_gespeeld;               
            }
        }
        private void btn_Deel_Click(object sender, RoutedEventArgs e)
        {
            Inzet_Slider.IsEnabled = false;

            SliderMaxEnMin();

            uitkomst_txt.Content = string.Empty;
            txt_Bank.Text = string.Empty;
            txt_speler.Text = string.Empty;
            uitkomst_txt.Content = string.Empty;
            bank_ptn.Content = "0";
            speler_ptn.Content = "0";
            btn_deel.IsEnabled = false;
            btn_hit.IsEnabled = true;
            btn_stand.IsEnabled = true;
            Btn_DoubleDown.IsEnabled = true;
            isSpeler = true;

            TimerDeel();
            TimerDeel();

            HeeftAasSpeler();
            CheckVerlorenHit(sender, e);

            KapitaalOnderNull(kapitaal_txt.Text);

        }

        private void Btn_Stand_Click(object sender, RoutedEventArgs e)
        {
            btn_hit.IsEnabled = false;
            Inzet_Slider.IsEnabled = true;
            btn_stand.IsEnabled = false;


            TimerStand();

        }


        private void Btn_Hit_Click(object sender, RoutedEventArgs e)
        {

            Btn_DoubleDown.IsEnabled = false;

            SliderMaxEnMin();

            GeefKaartenSpelerBank(isSpeler);

            bank_ptn.Content = HeeftAasbank();

            speler_ptn.Content = HeeftAasSpeler();

            CheckVerlorenHit(sender, e);

            KapitaalOptellenOfAftrekken();

            KapitaalOnderNull(kapitaal_txt.Text);
        }

        private void Nieuw_Spel_Click(object sender, RoutedEventArgs e)
        {
            uitkomst_txt.Content = string.Empty;
            txt_Bank.Text = string.Empty;
            txt_speler.Text = string.Empty;
            uitkomst_txt.Content = string.Empty;
            bank_ptn.Content = "0";
            speler_ptn.Content = "0";
            btn_deel.IsEnabled = false;
            btn_hit.IsEnabled = false;
            btn_stand.IsEnabled = false;
            rondeNummer = 0;
            kapitaal_txt.Text = "100";
            Inzet_Slider.IsEnabled = true;
            Alle_Kaarten.Items.Clear();
            Alle_Kaarten_Bank.Items.Clear();
            Laatste_Spel.Content = "Last Game";

            BankKaartFoto.Source = new BitmapImage(new Uri("/assets/kaartachterkant.png", UriKind.Relative));
            SpelerKaartFoto.Source = new BitmapImage(new Uri("/assets/kaartachterkant.png", UriKind.Relative));

        }

        private void Inzet_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            btn_deel.IsEnabled = true;
            nieuw_spel.IsEnabled = true;
            Btn_DoubleDown.IsEnabled = false;
            vieuwbox.Visibility = Visibility.Hidden;
            bankHeeftAas = 0;
            spelerHeeftAas = 0;

            SliderMaxEnMin();

            KapitaalOnderNull(kapitaal_txt.Text);

            Alle_Kaarten.Items.Clear();
            Alle_Kaarten_Bank.Items.Clear();
            uitkomst_txt.Content = string.Empty;
            txt_Bank.Text = string.Empty;
            txt_speler.Text = string.Empty;
            uitkomst_txt.Content = string.Empty;
            bank_ptn.Content = "0";
            speler_ptn.Content = "0";


            BankKaartFoto.Source = new BitmapImage(new Uri("/assets/kaartachterkant.png", UriKind.Relative));
            SpelerKaartFoto.Source = new BitmapImage(new Uri("/assets/kaartachterkant.png", UriKind.Relative));

            spelerPunten = 0;
            bankPunten = 0;
            spelerPunten = 0;
            aantalKaartenSpeler = 0;
        }
        private void DoubleDoownFunctie()
        {
            int aantalInzet = Convert.ToInt32(inzet_txt.Text);
            int aantalKapitaal = Convert.ToInt32(kapitaal_txt.Text);

            if (aantalInzet > aantalKapitaal / 2)
            {
                MessageBox.Show("Je hebt niet genoeg kapitaal",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                    );
                btn_hit.IsEnabled = true;
                btn_stand.IsEnabled = true;
            }
            else
            {
                isDoubleDown = true;
                isSpeler = false;
                inzet_txt.Text = Convert.ToString(aantalInzet * 2);
                Btn_DoubleDown.IsEnabled = false;
                Inzet_Slider.IsEnabled = true;
                string nummer = Trek_Nummer();
                string teken = Trek_Teken();

                DeckHervullen();
                KaartenDeckGebruiken(teken, nummer);

                TimerStand();
            }



        }
        private void Btn_DoubleDown_Click(object sender, RoutedEventArgs e)
        {
            btn_deel.IsEnabled = false;
            btn_hit.IsEnabled = false;
            btn_stand.IsEnabled = false;



            DoubleDoownFunctie();

        }
        private void StatusBarItem_MouseEnter(object sender, MouseEventArgs e)
        {
            txt_historiek.Visibility = Visibility.Visible;
        }

        private void StatusBarItem_MouseLeave(object sender, MouseEventArgs e)
        {
            txt_historiek.Visibility = Visibility.Hidden;
        }
    }
}
