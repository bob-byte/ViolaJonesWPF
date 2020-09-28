using Emgu.CV;
using Emgu.CV.Structure;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;

namespace ViolaJonesWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        internal static String FileXML { get; set; }
        internal static Double ScaleFactor { private get; set; }
        internal static Int32 MinNeighbors { private get; set; }

        private readonly RecogniseRoadSigns recognise = new RecogniseRoadSigns();
        private OpenFileDialog openImage, openDataSet;
        private Image<Bgr, Byte> image;
        private TimeSpan timeRecog;

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            
            openImage = new OpenFileDialog();
            Boolean? findImage = openImage.ShowDialog();
            if (findImage == true)
            {
                image = new Image<Bgr, Byte>(openImage.FileName);
                pictureBox1.Source = Helper.LoadBitmap(image.ToBitmap());
            }
        }

        private void CompulatePrecision_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileInfo file = new FileInfo(openImage.FileName);
                Int32 amountSign;

                List<String> data = new List<String>();
                if (openImage.FileName != "" && openDataSet.FileName != "")
                {
                    Boolean rightRead = recognise.ReadTextFile(openDataSet.FileName, file.Name, out amountSign, data);
                    if (rightRead == false)
                    {
                        MessageBox.Show("Test dataset is\'t right recorded", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else if (openDataSet.FileName == "")
                {
                    MessageBox.Show("Please, select dataset", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    return;
                }

                recognise.IsRightRecord(data, amountSign);                
                Int32 wrongCountSigns;
                Double precision = recognise.DetermPrecision(out wrongCountSigns, amountSign);
                if (wrongCountSigns != 0)
                {
                    MessageBox.Show($"Recognition accuracy {precision}%", "Recognition accuracy", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"Recognition accuracy {precision}%. Overly {wrongCountSigns} areas have been circled",
                        "Recognition accuracy", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectTestSet_Click(object sender, RoutedEventArgs e)
        {
            openDataSet = new OpenFileDialog();
            openDataSet.Filter = "TXT|*.txt";
            openDataSet.ShowDialog();
        }

        private void SelectXML_Click(object sender, RoutedEventArgs e)
        {
            ChoiceXML dialogXML = new ChoiceXML();
            dialogXML.ShowDialog();
        }

        private void RunThreadRecSigns()
        {
            try
            {
                timeRecog = recognise.RecSigns(pictureBox1, image, ScaleFactor, FileXML, MinNeighbors, Dispatcher);

                MessageBox.Show($"Time of recognising objects: {timeRecog.TotalMilliseconds} miliseconds",
                    "Object Recognition Time", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }

        private void Recognise_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ThreadStart recogniseDel = RunThreadRecSigns;
                Thread threadDrawRects = new Thread(recogniseDel)
                {
                    Priority = ThreadPriority.Highest
                };
                threadDrawRects.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
    }
}