using System.Windows;

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
            MainWindow.FileXML = @"HaarCascade\cascade(-w 15 -h 15, 5000pos, 1neg).xml";
        }

        private void RB_15_7000_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.FileXML = @"HaarCascade\cascade(-w 15 -h 15, 7000pos, 1neg).xml";
        }

        private void RB_25_5000_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.FileXML = @"HaarCascade\cascade(-w 25 -h 25, 5000pos, 1neg).xml";
        }

        private void RB_25_7000_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.FileXML = @"HaarCascade\cascade(-w 25 -h 25, 7000pos, 1neg).xml";
        }

        private void RB_25_10000_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.FileXML = @"HaarCascade\cascade(-w 25 -h 25, 10000pos, 1neg).xml";
        }

        private void RB_25_15000_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.FileXML = @"HaarCascade\cascade(-w 25 -h 25, 15000pos, 1neg).xml";
        }

        private void RB_35_5000_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.FileXML = @"HaarCascade\cascade(-w 35 -h 35, 5000pos, 1neg).xml";
        }

        private void RB_35_7000_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.FileXML = @"HaarCascade\cascade(-w 35 -h 35, 7000pos, 1neg).xml";
        }

        private void RB_25_5000_3000_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.FileXML = @"HaarCascade\cascade(-w 25 -h 25, 5000pos, 3000neg).xml";
        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            if(!MainWindow.FileXML.Contains("3000neg"))
            {
                MainWindow.MinNeighbors = 43;
                MainWindow.ScaleFactor = 1.1;
            }
            else
            {
                MainWindow.MinNeighbors = 5;
                MainWindow.ScaleFactor = 1.6;
            }
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.FileXML = "";
            Close();
        }

    }
}
