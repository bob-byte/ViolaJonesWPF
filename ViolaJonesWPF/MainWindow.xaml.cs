using Emgu.CV;
using Emgu.CV.Structure;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        private DataImages dataImages = new DataImages();
        private PrecisionRecognition precisionRecog = new PrecisionRecognition();
        private OpenFileDialog openImage, openDataSet;
        private Image<Bgr, Byte> image;
        private TimeSpan timeRecog;
        private Rectangle[] roadSignsRects;
        private Boolean isDrawnRects = false;

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            openImage = new OpenFileDialog();
            Boolean? findImage = openImage.ShowDialog();
            if (findImage == true)
            {
                image = new Image<Bgr, Byte>(openImage.FileName);
                pictureBox1.Source = ConvertToBitmapSource.LoadBitmap(image.ToBitmap());
                isDrawnRects = false;
            }
        }

        private void CompulatePrecision_Click(object sender, RoutedEventArgs e)
        {
            if(isDrawnRects)
            {
                try
                {
                    FileInfo file = new FileInfo(openImage.FileName);
                    Int32 amountSign;

                    List<String> data = new List<String>();
                    if (openImage.FileName != "" && openDataSet.FileName != "")
                    {
                        Boolean isRightRecord;
                        dataImages.ReadTextFile(openDataSet.FileName, file.Name, data, out isRightRecord, out amountSign);

                        if (isRightRecord == false)
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

                    dataImages.ParseDataFromDescriptionRoadSigns(data, amountSign,
                        out Int32[] topRow, out Int32[] leftCol, out Int32[] bottomRow, out Int32[] rightCol);
                    Int32 amountRightRects = precisionRecog.GetAmountRightRect(roadSignsRects, 0.4, topRow, leftCol, bottomRow, rightCol);
                    Int32 wrongCountSigns = roadSignsRects.Length - amountRightRects;

                    Double precision = precisionRecog.DetermPrecision(amountSign, amountRightRects);
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
                CascadeClassifier classifier = new CascadeClassifier(FileXML);
                recognise.RecogniseSigns(classifier, image, ScaleFactor, MinNeighbors, out roadSignsRects, out timeRecog);

                DrawRects(roadSignsRects, image.Bitmap);
                Dispatcher.Invoke(() =>
                {
                    pictureBox1.Source = ConvertToBitmapSource.LoadBitmap(image.Bitmap);
                });

                isDrawnRects = true;

                MessageBox.Show($"Time of recognising objects: {timeRecog.TotalMilliseconds} miliseconds",
                    "Object Recognition Time", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

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