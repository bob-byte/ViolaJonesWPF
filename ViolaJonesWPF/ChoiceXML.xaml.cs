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

namespace ViolaJonesWPF
{
    /// <summary>
    /// Логика взаимодействия для ChoiceXML.xaml
    /// </summary>
    public partial class ChoiceXML : Window
    {
        public ChoiceXML()
        {
            InitializeComponent();
        }

        private void RB_15_5000_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.fileXML = @"HaarCascade\cascade(-w 15 -h 15, 5000pos, 1neg).xml";
        }

        private void RB_15_7000_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.fileXML = @"HaarCascade\cascade(-w 15 -h 15, 7000pos, 1neg).xml";
        }

        private void RB_25_5000_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.fileXML = @"HaarCascade\cascade(-w 25 -h 25, 5000pos, 1neg).xml";
        }

        private void RB_25_7000_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.fileXML = @"HaarCascade\cascade(-w 25 -h 25, 7000pos, 1neg).xml";
        }

        private void RB_25_10000_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.fileXML = @"HaarCascade\cascade(-w 25 -h 25, 10000pos, 1neg).xml";
        }

        private void RB_25_15000_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.fileXML = @"HaarCascade\cascade(-w 25 -h 25, 15000pos, 1neg).xml";
        }

        private void RB_35_5000_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.fileXML = @"HaarCascade\cascade(-w 35 -h 35, 5000pos, 1neg).xml";
        }

        private void RB_35_7000_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.fileXML = @"HaarCascade\cascade(-w 35 -h 35, 7000pos, 1neg).xml";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
