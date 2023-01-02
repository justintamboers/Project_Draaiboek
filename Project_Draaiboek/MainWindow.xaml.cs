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
        List<string> historiekList = new List<string>();
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
        /// <summary>
        /// zorgt ervoor dat TxtTijd de volledige tijd van uur tot seconden afbeeld
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Klok_Tick(object sender, EventArgs e)
        {
            TxtTijd.Text = DateTime.Now.ToLongTimeString();
        }
        /// <summary>
        /// start de timer voor KaartDelen zodat er tussen elke kaar 1 seconden zit en zorgt ervoor dat hit, stand en doubledown voor de tijd van kaartdelen niet klickbaar zijn
        /// </summary>
        private void TimerDeel()
        {
            seconden.Start();
            btn_hit.IsEnabled = false;
            btn_stand.IsEnabled = false;
            Btn_DoubleDown.IsEnabled = false;
        }
        /// <summary>
        /// zorgt ervoor dat als je op delen drukt dat de speler 2 kaarten krijgt en de bank 1 en zorgt ervoor dat hit, stand en doubledown voor de tijd van kaartdelen terug klickbaar zijn ne de uitvoering
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// trekt een random nummer en teken van een kaart voor de speler en roept DeckHervullen en KaartenDeckGebruiken op
        /// </summary>
        private void SpelerKaart()
        {
            string spelerNummer = Trek_Nummer();
            string tekenSpeler = Trek_Teken();

            DeckHervullen();
            KaartenDeckGebruiken(tekenSpeler, spelerNummer);

        }
        /// <summary>
        /// zorgt ervoor dat het deck hervuld wordt als het leeg is
        /// </summary>
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
        /// <summary>
        /// zorgt ervoor dat de kaart als het nog in de deck is verwijdert wordt zoniet dat er een andere kaart getrokken wordt
        /// </summary>
        /// <param name="teken">wordt het teken van de kaart</param>
        /// <param name="nummer">wordt het nummer van de kaart</param>
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
        /// <summary>
        /// zorgt ervoor dat de kaart zichtbaar wordt zowel fotot als text bij bank, speler of doubledown (zijwaards bij speler)
        /// </summary>
        /// <param name="teken">wordt het teken van de kaart</param>
        /// <param name="nummer">wordt het nummer van de kaart</param>
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
        /// <summary>
        /// trekt een random nummer en teken van een kaart voor de bank en roept deckhervullen en KaartenDeckGebruiken op
        /// </summary>

        /// <summary>
        /// trekt een random teken
        /// </summary>
        /// <returns>een teken</returns>
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
        /// <summary>
        /// trekt een random nummer
        /// </summary>
        /// <returns>een nummer</returns>
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
        private void BankKaart()
        {
            string bank1 = Trek_Nummer();
            string bankteken = Trek_Teken();

            DeckHervullen();
            KaartenDeckGebruiken(bankteken, bank1);

        }
        /// <summary>
        /// start de timer voor StandDelen zodat er tussen elke kaar 1 seconden zit
        /// </summary>
        private void TimerStand()
        {
            secondenStand.Start();
        }
        /// <summary>
        /// zorgt ervoor dat als je stand clickt dat de bank elke seconden een kaart krijgt todat de bank hooger heeft dan 17 en checkt of je gewonnen of verloren hebt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                Inzet_Slider.IsEnabled = true;
                secondenStand.Stop();
            }
        }
        /// <summary>
        /// voert SpelerKaart uit of BanktKaart
        /// </summary>
        /// <param name="isSpeler">beslist of het speler is of bank</param>
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
        /// <summary>
        /// checkt dat je meer geld hebt dan 0
        /// </summary>
        /// <param name="kapitaal">he bedrag van geld dat je hebt</param>
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
        /// <summary>
        /// beslist het minimum en maximum waarde van de slider
        /// </summary>
        private void SliderMaxEnMin()
        {
            Inzet_Slider.Minimum = Math.Round(Convert.ToDouble(kapitaal_txt.Text) / 10);
            Inzet_Slider.Maximum = Convert.ToDouble(kapitaal_txt.Text);

            inzet_txt.Text = Convert.ToString(Inzet_Slider.Value);
        }
        /// <summary>
        /// geeft bij gewonnen je inzet en bij verloren trekt het je inzet af
        /// </summary>
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
        /// <summary>
        /// checkt als de speler een aas heeft
        /// </summary>
        /// <returns>de punten van de speler</returns>
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
        /// <summary>
        /// checkt als de bank een aas heeft
        /// </summary>
        /// <returns>de punten van de bank</returns>
        private int HeeftAasbank()
        {
            if (bankPunten > 21 & bankHeeftAas > 0)
            {
                bankPunten -= 10;
                bankHeeftAas -= 1;


            }
            return bankPunten;
        }
        /// <summary>
        /// checkt welke waarde welke kaart heeft voor speler
        /// </summary>
        /// <param name="getal">getrokken nummer van de kaart</param>
        /// <returns>de waarde van de kaart</returns>
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
        /// <summary>
        /// checkt als speler heeft gewonnen, verloren of push
        /// </summary>
        /// <returns>de uitkomst van de match (gewonnen, verloren of push)</returns>
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
        /// <summary>
        /// checkt als je al verloren hebt bij hit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// telt alle punten van de bank op
        /// </summary>
        /// <param name="gekregenBankPunten">getrokken nummerbank</param>
        /// <returns>bank punten</returns>
        private int BankPuntenTellen(string gekregenBankPunten)
        {
            int bankBankPunten = 0;

            if (gekregenBankPunten == "boer" || gekregenBankPunten == "koningin" || gekregenBankPunten == "koning")
            {
                bankBankPunten = 10;
            }
            else if (gekregenBankPunten == "aas")
            {
                bankBankPunten = 11;
                bankHeeftAas += 1;
            }
            else if (Convert.ToInt32(gekregenBankPunten) > 0)
            {
                bankBankPunten = Convert.ToInt32(gekregenBankPunten);
            }

            return bankBankPunten;
        }
        /// <summary>
        /// telt alle punten van de speler op
        /// </summary>
        /// <param name="gerkregenSpelerPunten">getrokken nummer speler</param>
        /// <returns>speler punten</returns>
        private int SpelerPuntenTellen(string gerkregenSpelerPunten)
        {
            int spelerSpelerPunten = 0;

            if (gerkregenSpelerPunten == "boer" || gerkregenSpelerPunten == "koningin" || gerkregenSpelerPunten == "koning")
            {
                spelerSpelerPunten = 10;
            }
            else if (gerkregenSpelerPunten == "aas")
            {
                spelerSpelerPunten = 11;
                spelerHeeftAas += 1;
            }
            else if (Convert.ToInt32(gerkregenSpelerPunten) > 0)
            {
                spelerSpelerPunten = Convert.ToInt32(gerkregenSpelerPunten);
            }
            return spelerSpelerPunten;
        }
        /// <summary>
        /// zorgt ervoor dat de historiek alleen maar de 10 laatste spellen laat zien en de laatste als eerste in statusbalk
        /// </summary>
        private void Historiek()
        {
            txt_historiek.Items.Clear();
            string laatste_Spel_gespeeld = Convert.ToString(Laatste_Spel.Content);
            string addInHistoriekList = rondeNummer + " " + laatste_Spel_gespeeld;
            historiekList.Add(addInHistoriekList);
            if (historiekList.Count > 10)
            {
                historiekList.RemoveAt(0);
            }
            for (int i = historiekList.Count - 1; i > -1; i--)
            {
                txt_historiek.Items.Add(historiekList.ElementAt(i));
            }
            ++rondeNummer;
        }
        /// <summary>
        /// laat de laatste game zien in statusbalk
        /// </summary>
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
        /// <summary>
        /// start de functie TimerDeel, HeeftAasSpeler, CheckVerlorenHit en KapitaalOnderNull zodat je het juiste resultaat krijgt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// start TimerStand zodat je de gewilde uitput krijgt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Stand_Click(object sender, RoutedEventArgs e)
        {
            btn_hit.IsEnabled = false;
            Inzet_Slider.IsEnabled = false;
            btn_stand.IsEnabled = false;


            TimerStand();

        }
        /// <summary>
        /// start de functie SliderMaxEnMin, GeefKaartenSpelerBank, CheckVerlorenHit en kijkt of de bank of speler een of meerdere azen heeft, KapitaalOptellenOfAftrekken en KapitaalOnderNull
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

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
        /// <summary>
        /// zorgt ervoor dat alles gereset wordt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Nieuw_Spel_Click(object sender, RoutedEventArgs e)
        {
            uitkomst_txt.Content = string.Empty;
            txt_historiek.Items.Clear();
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
            vieuwbox.Visibility = Visibility.Hidden;
            deckInGebruik = kaartenDeck.ToList();
            int AantalKaarten = deckInGebruik.Count;
            Aantal_KaartenOver.Content = AantalKaarten;

            BankKaartFoto.Source = new BitmapImage(new Uri("/assets/kaartachterkant.png", UriKind.Relative));
            SpelerKaartFoto.Source = new BitmapImage(new Uri("/assets/kaartachterkant.png", UriKind.Relative));

        }
        /// <summary>
        /// zorgt ervoor dat je op deel knop kan drukkenn als veranderd en dat je een hoeveelheid inzet kan kiezen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// kijkt of je genoeg kapitaal hebt voor doubledown zo ja trek je een kaart zet je die zijwaards en doe je Timerstand anders krijg je een messagebox 
        /// </summary>
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
        /// <summary>
        /// disabeld hit en stand en doet de functie DoubleDoownFunctie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_DoubleDown_Click(object sender, RoutedEventArgs e)
        {
            btn_hit.IsEnabled = false;
            btn_stand.IsEnabled = false;



            DoubleDoownFunctie();

        }
        /// <summary>
        /// zorgt ervoor dat de listbox voor de historiek te laten zien zichtbaar is waneer op historiek gehoverd wordt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusBarItem_MouseEnter(object sender, MouseEventArgs e)
        {
            txt_historiek.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// zorgt ervoor dat de listbox voor de historiek onzichtbaar is waneer je nie over historiek hoverd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusBarItem_MouseLeave(object sender, MouseEventArgs e)
        {
            txt_historiek.Visibility = Visibility.Hidden;
        }
    }
}
