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
        public MainWindow()
        {
            InitializeComponent();
        }
        private string trek_kaart()
        {
            Random random = new Random();
            int number = random.Next(1, 13);
            int teken = random.Next(1, 4);
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
        private void btn_deel_Click(object sender, RoutedEventArgs e)
        {
            txt_Bank.Text += trek_kaart();
            txt_speler.Text += trek_kaart();
            btn_deel.IsEnabled = false;
            btn_hit.IsEnabled = true;
            btn_stand.IsEnabled = true;
        }

        private void btn_hit_Click(object sender, RoutedEventArgs e)
        {
            txt_Bank.Text += trek_kaart();
            txt_speler.Text += trek_kaart();
        }

        private void btn_stand_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
