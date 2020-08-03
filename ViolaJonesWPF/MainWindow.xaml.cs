using Emgu.CV;
using Emgu.CV.Structure;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

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

        internal static String fileXML;
        Int32[] topRow, leftCol, bottomRow, rightCol;
        System.Drawing.Rectangle[] roadSignsRecog;
        Boolean? findImage, findDataSet;

        Boolean ReadTextFile(String nameFile, String nameImage, out SByte amountSign, List<String> data)
        {
            using (StreamReader sr = new StreamReader(nameFile))
            {
                String shortNameFile;
                String[] arrayData;
                Boolean rightRead = false;

                amountSign = 0;
                while (sr.Peek() >= 0)
                {
                    String textRow = sr.ReadLine();
                    shortNameFile = "";

                    Int32 i = textRow.IndexOf(';') - 1;
                    for (int j = 0; j <= i; j++)
                    {
                        shortNameFile += textRow[j];
                    }

                    if (nameImage == shortNameFile)
                    {
                        rightRead = true;
                    }
                    while (nameImage == shortNameFile)
                    {
                        arrayData = new String[6];
                        arrayData = textRow.Split(';');

                        data.Add(arrayData[1]);
                        data.Add(arrayData[2]);
                        data.Add(arrayData[3]);
                        data.Add(arrayData[4]);

                        amountSign++;
                        if (sr.Peek() >= 0)
                        {
                            textRow = sr.ReadLine();
                            i = textRow.IndexOf(';') - 1;
                            shortNameFile = "";
                            for (int j = 0; j <= i; j++)
                            {
                                shortNameFile += textRow[j];
                            }
                            if (nameImage != shortNameFile)
                            {
                                return rightRead;
                            }
                        }

                    }
                }
                return rightRead;
            }
        }

        internal void DetermPrecision(out Single precision, SByte amountSign)
        {
            SByte amountRightRect = 0;

            for (Int32 i = 0; amountSign >= roadSignsRecog.Length ? 
                i < roadSignsRecog.Length : i < amountSign; i++)
            {
                Single fault = (Single)((Single)(bottomRow[i] - topRow[i]) * 0.1);

                if (Math.Abs(roadSignsRecog[i].Top - topRow[i]) <= fault && Math.Abs(roadSignsRecog[i].Left - leftCol[i]) <= fault &&
                Math.Abs(roadSignsRecog[i].Bottom - bottomRow[i]) <= fault && Math.Abs(roadSignsRecog[i].Right - rightCol[i]) <= fault)
                {
                    amountRightRect++;
                }

            }

            precision = (Single)((Single)amountRightRect / (Single)(amountSign) * 100.0);

            if (amountSign >= roadSignsRecog.Length)
            {
                MessageBox.Show($"Точніть розпізнавання: {precision}%", "Точніть розпізнавання", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Точніть розпізнавання: {precision}%. Обведено зайво {roadSignsRecog.Length - amountRightRect} областей",
                    "Точніть розпізнавання", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        OpenFileDialog openImage, openDataSet;
        Image<Bgr, Byte> image;
        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            openImage = new OpenFileDialog();
            findImage = openImage.ShowDialog();
            if (findImage == true)
            {
                image = new Image<Bgr, Byte>(openImage.FileName);
                pictureBox1.Source = Helper.LoadBitmap(image.ToBitmap());
            }
        }

        private void CompulatePrecision_Click(object sender, RoutedEventArgs e)
        {
            String imageFile = openImage.FileName;
            FileInfo file = new FileInfo(imageFile);

            SByte amountSign;

            try
            {
                List<String> data = new List<String>();
                if (openImage.FileName != "" && openDataSet.FileName != "")
                {
                    bool rightRead = ReadTextFile(openDataSet.FileName, file.Name, out amountSign, data);
                    if (rightRead == false)
                    {
                        MessageBox.Show("Тестовий набір даних неправильно записаний", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else if (openDataSet.FileName == "")
                {
                    MessageBox.Show("Виберіть, будь ласка, тестовий набір даних", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    return;
                }

                topRow = new Int32[amountSign];
                leftCol = new Int32[amountSign];
                bottomRow = new Int32[amountSign];
                rightCol = new Int32[amountSign];

                for (Int32 i = 0, k = 0; i < amountSign * 4 && k < amountSign; i += 4, k++)
                {
                    if (!Int32.TryParse(data[i], out topRow[k]))
                    {
                        MessageBox.Show("Не вдалося зчитати координату верхнього Х з тестового набору даних", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    }
                    else if (!Int32.TryParse(data[i + 1], out leftCol[k]))
                    {
                        MessageBox.Show("Не вдалося зчитати координату лівого Y з тестового набору даних", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    }
                    else if (!Int32.TryParse(data[i + 2], out bottomRow[k]))
                    {
                        MessageBox.Show("Не вдалося зчитати координату нижнього Х з тестового набору даних", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    }
                    else if (!Int32.TryParse(data[i + 3], out rightCol[k]))
                    {
                        MessageBox.Show("Не вдалося зчитати координату правого Y з тестового набору даних", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    }
                }

                Single precision;
                DetermPrecision(out precision, amountSign);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectTestSet_Click(object sender, RoutedEventArgs e)
        {
            openDataSet = new OpenFileDialog();
            findDataSet = openDataSet.ShowDialog();
        }

        private void SelectXML_Click(object sender, RoutedEventArgs e)
        {
            ChoiceXML dialogXML = new ChoiceXML();
            dialogXML.ShowDialog();
        }

        void RecSigns()
        {
            try
            {
                CascadeClassifier cascadeClassifier = new CascadeClassifier(fileXML);

                Bitmap bitmap = image.ToBitmap();
                Image<Bgr, Byte> grayImage = new Image<Bgr, Byte>(bitmap);

                DateTime time1 = new DateTime();
                time1 = DateTime.Now;
                roadSignsRecog = cascadeClassifier.DetectMultiScale(grayImage, 1.1, 43);
                TimeSpan ms = DateTime.Now.Subtract(time1);

                foreach (System.Drawing.Rectangle roadSign in roadSignsRecog)
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        using (System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Blue, 4))
                        {
                            graphics.DrawRectangle(pen, roadSign);
                        }
                    }
                }
                grayImage.Bitmap = bitmap;
                pictureBox1.Source = Helper.LoadBitmap(bitmap);

                MessageBox.Show($"Час розпізнавання об'єктів: {ms.TotalMilliseconds} мілісекунд", "Час розпізнавання",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Recognise_Click(object sender, RoutedEventArgs e)
        {
            RecSigns();
        }
    }
}