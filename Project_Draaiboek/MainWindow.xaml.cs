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

namespace Porject_Draaiboek
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int SpelerHeeftAas = 0;
        private int BankHeeftAas = 0;
        private int SpelerPunten = 0;
        private int BankPunten = 0;
        bool IsGewonnenOfVerloren = false;
        private Random Random = new Random();

        List<int> getallen;
        public MainWindow()
        {
            InitializeComponent();
        }
        //hit moet disabled worden en delen van een kaart duurt 1sec
        //private void geefkaart() { 
            
        //}
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
        }
        private int HeeftAasSpeler()
        {
            if (SpelerPunten > 21 & SpelerHeeftAas > 0)
            {
                SpelerHeeftAas -= 1;
                SpelerPunten -= 10;
            }
            else
            {

            }
            return SpelerPunten;         
            
        }
        private int HeeftAasbank()
        {
            if (BankPunten > 21 & BankHeeftAas > 0)
            {
                BankPunten -= 10;
                BankHeeftAas -= 1;


            }
            return BankPunten;
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
                SpelerHeeftAas += 1;
            }
            else
            {
                nummer = Convert.ToInt32(getal);
            }
            SpelerPunten += nummer;
            return SpelerPunten;
        }
        private string HeeftGewonnen()
        {
            string uitkomst = "";

            if (SpelerPunten > 21)
            {
                uitkomst = "verloren";
            }
            else if (BankPunten > 21)
            {
                uitkomst = "gewonnen";
            }
            else if (SpelerPunten == BankPunten)
            {
                uitkomst = "push";
            }
            else if (SpelerPunten > BankPunten & BankPunten < 21)
            {
                uitkomst = "gewonnen";
            }
            else
            {
                uitkomst = "verloren";
            }
            return uitkomst;
        }
        private string CheckVerlorenHit()
        {
            string uitkomst = "";
            if (SpelerPunten > 21)
            {
                uitkomst = "verloren";
                btn_stand.IsEnabled = false;
                btn_hit.IsEnabled = false;
                btn_deel.IsEnabled = true;
            }
            else
            {

            }
            return uitkomst;
        }
        private string Trek_Nummer()
        {
            int number = Random.Next(1, 14);
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

            int teken = Random.Next(1, 5);
            string uitput = "";


            switch (teken)
            {
                case 1:
                    uitput += " harten\n";
                    break;
                case 2:
                    uitput += " ruiten\n";
                    break;
                case 3:
                    uitput += " klaveren\n";
                    break;
                case 4:
                    uitput += " schoppen\n";
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
                BankHeeftAas += 1;
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
                SpelerHeeftAas += 1;
            }
            else if (Convert.ToInt32(speler) > 0)
            {
                speler1n = Convert.ToInt32(speler);
            }
            return speler1n;
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
                btn_deel.IsEnabled = true;
                btn_hit.IsEnabled = false;
                btn_stand.IsEnabled = false;
            
            string bank1 = Trek_Nummer();
            txt_Bank.Text += bank1;
            txt_Bank.Text += Trek_Teken();
            string speler1 = Trek_Nummer();
            txt_speler.Text += speler1;
            txt_speler.Text += Trek_Teken();
            string speler2 = Trek_Nummer();
            txt_speler.Text += speler2;
            txt_speler.Text += Trek_Teken();
            btn_deel.IsEnabled = false;
            btn_hit.IsEnabled = true;
            btn_stand.IsEnabled = true;

            SpelerPunten = SpelerPuntenTellen(speler2) + SpelerPuntenTellen(speler1);
            BankPunten = BankPuntenTellen(bank1);

            
            speler_ptn.Content = SpelerPunten;
            bank_ptn.Content = BankPunten;

            uitkomst_txt.Content = CheckVerlorenHit();

            KapitaalOnderNull(kapitaal_txt.Text);
        }
        private void Btn_Stand_Click(object sender, RoutedEventArgs e)
        {
            btn_hit.IsEnabled = false;
            Inzet_Slider.IsEnabled = true;

            bank_ptn.Content = HeeftAasbank();

            while (BankPunten <= 16 && HeeftAasbank() != 0)
            {

                string bank1 = Trek_Nummer();
                txt_Bank.Text += bank1;
                txt_Bank.Text += Trek_Teken();


                BankPunten += BankPuntenTellen(bank1);

                bank_ptn.Content = BankPunten;
                bank_ptn.Content = HeeftAasbank();
            }


            speler_ptn.Content = HeeftAasSpeler();

            uitkomst_txt.Content = HeeftGewonnen();

            btn_stand.IsEnabled = false;

            KapitaalOptellenOfAftrekken();

            KapitaalOnderNull(kapitaal_txt.Text);
        }

        
        private void Btn_Hit_Click(object sender, RoutedEventArgs e)
        {
            SliderMaxEnMin();

            string Speler = Trek_Nummer();
            txt_speler.Text += Speler + Trek_Teken();

            
            
            speler_ptn.Content = WaardeKaart(Speler);

            bank_ptn.Content = HeeftAasbank();

            speler_ptn.Content = HeeftAasSpeler();

            uitkomst_txt.Content = CheckVerlorenHit();

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
                btn_deel.IsEnabled = true;
                btn_hit.IsEnabled = false;
                btn_stand.IsEnabled = false;
                kapitaal_txt.Text = "100";
        }

        private void Inzet_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            btn_deel.IsEnabled = true;

            SliderMaxEnMin();

            KapitaalOnderNull(kapitaal_txt.Text);
        }
    }
}
