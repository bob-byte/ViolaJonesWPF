using Emgu.CV;
using Emgu.CV.Structure;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ViolaJonesWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private readonly RecogniseRoadSigns m_recognise = new RecogniseRoadSigns();
        private ImageData m_dataImages = new ImageData();
        private PrecisionRecognition m_precisionRecog = new PrecisionRecognition();
        private OpenFileDialog m_openImage, m_openDataSet;
        private Image<Bgr, Byte> m_image;
        private TimeSpan m_timeRecog;
        private Rectangle[] m_roadSignsRects;
        private Boolean m_isDrawnRects = false;

        public MainWindow()
        {
            InitializeComponent();
        }
        
        internal static String FileXML { get; set; }

        internal static Double ScaleFactor { private get; set; }

        internal static Int32 MinNeighbors { private get; set; }        

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            m_openImage = new OpenFileDialog();
            Boolean? findImage = m_openImage.ShowDialog();
            if (findImage == true)
            {
                m_image = new Image<Bgr, Byte>(m_openImage.FileName);
                pictureBox1.Source = m_image.ToBitmap().ToBitmapSource();
                m_isDrawnRects = false;
            }
        }

        private void CompulatePrecision_Click(object sender, RoutedEventArgs e)
        {
            if(m_isDrawnRects)
            {
                try
                {
                    FileInfo file = new FileInfo(m_openImage.FileName);
                    Int32 amountSign;

                    List<String> data = new List<String>();
                    if (m_openImage.FileName != "" && m_openDataSet.FileName != "")
                    {
                        m_dataImages.ReadTextFile(m_openDataSet.FileName, file.Name, data, out Boolean isRightRecord, out amountSign);

                        if (!isRightRecord)
                        {
                            MessageBox.Show("Test dataset is\'t right recorded", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    else if (m_openDataSet.FileName == "")
                    {
                        MessageBox.Show("Please, select dataset", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        return;
                    }

                    m_dataImages.ParseDataFromDescriptionRoadSigns(data, amountSign,
                        out Int32[] topRow, out Int32[] leftCol, out Int32[] bottomRow, out Int32[] rightCol);

                    Int32 amountRightRects = m_precisionRecog.GetAmountRightRect(m_roadSignsRects, 0.4, topRow, leftCol, bottomRow, rightCol);
                    Int32 wrongCountSigns = m_roadSignsRects.Length - amountRightRects;

                    Double precision = m_precisionRecog.DetermPrecision(amountSign, amountRightRects);
                    if (wrongCountSigns <= 0)
                    {
                        MessageBox.Show($"Recognition accuracy {precision}%", "Recognition accuracy", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Recognition accuracy {precision}%. {wrongCountSigns} areas have been circled excessively",
                            "Recognition accuracy", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("At first you need to recognize road signs (click on button \"Recognise\")", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SelectTestSet_Click(object sender, RoutedEventArgs e)
        {
            m_openDataSet = new OpenFileDialog();
            m_openDataSet.Filter = "TXT|*.txt";
            m_openDataSet.ShowDialog();
        }

        private void DrawRects(IEnumerable<Rectangle> rects, Bitmap bitmap)
        {
            foreach (Rectangle roadSign in rects)
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    using (Pen pen = new Pen(Color.Blue, 4))
                    {
                        graphics.DrawRectangle(pen, roadSign);
                    }
                }
            }
        }

        private void SelectResearchedCascade_Click(object sender, RoutedEventArgs e)
        {
            ChoiceXML dialogXML = new ChoiceXML();
            dialogXML.ShowDialog();
        }

        private void SelectOwnCascade_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openCascade = new OpenFileDialog();
            Boolean isSelectedCascade = openCascade.ShowDialog().Value;

            if(isSelectedCascade)
            {
                FileXML = openCascade.FileName;
            }
        }

        private async void Recognise_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var classifier = new CascadeClassifier(FileXML);
                
                await Task.Run(
                    action: () =>
                    {
                        m_recognise.RecogniseSigns(classifier, m_image, ScaleFactor, MinNeighbors, out m_roadSignsRects, out m_timeRecog);
                    
                        DrawRects(m_roadSignsRects, m_image.Bitmap);
                        Dispatcher.Invoke(() =>
                        {
                            pictureBox1.Source = m_image.Bitmap.ToBitmapSource();
                        });
                    
                        m_isDrawnRects = true;
                    }
                );

                MessageBox.Show($"Time of recognising objects: {m_timeRecog.TotalMilliseconds} miliseconds",
                    "Object Recognition Time", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, caption: "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}