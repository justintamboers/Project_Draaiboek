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
        private int spelerpunten = 0;
        private int bankpunten = 0;
        private bool IsBeurt = true;
        private Random random = new Random();

        List<int> getallen;
        public MainWindow()
        {
            InitializeComponent();
        }
        //mss opsliten 
        private string trek_nummer()
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
        private string trek_teken()
        {

            int teken = random.Next(1, 5);
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
        //private string trek_kaart()
        //{
        //    string kaart = trek_nummer() + trek_teken();
        //    return kaart;
        //}
        private void btn_deel_Click(object sender, RoutedEventArgs e)
        {
            int speler1n = 0;
            int speler2n = 0;
            int bank1n = 0;

            string bank1 = trek_nummer();
            txt_Bank.Text += bank1;
            txt_Bank.Text += trek_teken();
            string speler1 = trek_nummer();
            txt_speler.Text += speler1;
            txt_speler.Text += trek_teken();
            string speler2 = trek_nummer();
            txt_speler.Text += speler2;
            txt_speler.Text += trek_teken();
            btn_deel.IsEnabled = false;
            btn_hit.IsEnabled = true;
            btn_stand.IsEnabled = true;

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

            if (speler1 == "boer" || speler1 == "koningin" || speler1 == "koning")
            {
                speler1n = 10;
            }
            else if (speler1 == "aas")
            {
                speler1n = 11;
                SpelerHeeftAas += 1;
            }
            else if (Convert.ToInt32(speler1) > 0)
            {
                speler1n = Convert.ToInt32(speler1);
            }

            if (speler2 == "boer" || speler2 == "koningin" || speler2 == "koning")
            {
                speler2n = 10;
            }
            else if (speler2 == "aas")
            {
                speler2n = 11;
                SpelerHeeftAas += 1;
            }
            else if (Convert.ToInt32(speler2) > 0)
            {
                speler2n = Convert.ToInt32(speler2);
            }

            spelerpunten = speler2n + speler1n;
            bankpunten = bank1n;

            bank_ptn.Content = bankpunten;
            speler_ptn.Content = spelerpunten;
        }
        private void btn_stand_Click(object sender, RoutedEventArgs e)
        {
            btn_hit.IsEnabled = false;

            while (bankpunten <= 16)
            {
                int bank1n = 0;

                string bank1 = trek_nummer();
                txt_Bank.Text += bank1;
                txt_Bank.Text += trek_teken();

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

                bankpunten += bank1n;

                bank_ptn.Content = bankpunten;
            }

            if (spelerpunten > 21 & SpelerHeeftAas > 0)
            {
                spelerpunten -= 10;
                speler_ptn.Content = spelerpunten;
            }
            if (bankpunten > 21 & BankHeeftAas > 0)
            {
                bankpunten -= 10;
                bank_ptn.Content = bankpunten;
            }

            if (spelerpunten > 21)
            {
                uitkomst_txt.Content = "verloren";
            }
            else if (bankpunten > 21)
            {
                uitkomst_txt.Content = "gewonnen";
            }
            else if (spelerpunten == bankpunten)
            {
                uitkomst_txt.Content = "push";
            }
            else if (spelerpunten > bankpunten & bankpunten < 21)
            {
                uitkomst_txt.Content = "gewonnen";
            }
            else
            {
                uitkomst_txt.Content = "verloren";
            }
        }
        private void btn_hit_Click(object sender, RoutedEventArgs e)
        {

            int spelern = 0;

            string speler = trek_nummer();
            txt_speler.Text += speler + trek_teken();

            if (speler == "boer" || speler == "koningin" || speler == "koning")
            {
                spelern = 10;
            }
            else if (speler == "aas")
            {
                spelern = 11;
                SpelerHeeftAas += 1;
            }
            else if (Convert.ToInt32(speler) > 0)
            {
                spelern = Convert.ToInt32(speler);
            }
            spelerpunten += spelern;
            speler_ptn.Content = spelerpunten;
        }

    }



}
