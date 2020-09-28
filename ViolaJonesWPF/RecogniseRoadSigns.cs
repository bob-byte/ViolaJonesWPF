using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Threading;

namespace ViolaJonesWPF
{
    public class RecogniseRoadSigns
    {
        private Int32[] topRow, leftCol, bottomRow, rightCol;
        private Rectangle[] roadSignsRecog;

        public Boolean ReadTextFile(String nameFile, String nameImage, out Int32 amountSign, List<String> data)
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
                    for (Int32 j = 0; j <= i; j++)
                    {
                        shortNameFile += textRow[j];
                    }

                    if (nameImage == shortNameFile)
                    {
                        rightRead = true;
                    }
                    while (rightRead)
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
                            for (Int32 j = 0; j <= i; j++)
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

        public void IsRightRecord(List<String> data, Int32 amountSign)
        {
            topRow = new Int32[amountSign];
            leftCol = new Int32[amountSign];
            bottomRow = new Int32[amountSign];
            rightCol = new Int32[amountSign];

            for (Int32 i = 0, k = 0; i < amountSign * 4 && k < amountSign; i++, k++)
            {
                if (!Int32.TryParse(data[i], out topRow[k]))
                {
                    throw new FormatException("Upper X coordinate could not be read from the test data set");
                }
                else if (!Int32.TryParse(data[++i], out leftCol[k]))
                {
                    throw new FormatException("Left Y coordinate could not be read from the test data set");
                }
                else if (!Int32.TryParse(data[++i], out bottomRow[k]))
                {
                    throw new FormatException("Lower X coordinate could not be read from the test data set");
                }
                else if (!Int32.TryParse(data[++i], out rightCol[k]))
                {
                    throw new FormatException("Right Y coordinate could not be read from the test data set");
                }
            }
        }

        public Double DetermPrecision(out Int32 wrongCountSigns, Int32 amountSign)
        {
            Int32 amountRightRect = 0;

            for (Int32 i = 0; amountSign >= roadSignsRecog.Length ?
                i < roadSignsRecog.Length : i < amountSign; i++)
            {
                Double fault = (Double)(bottomRow[i] - topRow[i]) * 0.1;

                if (Math.Abs(roadSignsRecog[i].Top - topRow[i]) <= fault && Math.Abs(roadSignsRecog[i].Left - leftCol[i]) <= fault &&
                Math.Abs(roadSignsRecog[i].Bottom - bottomRow[i]) <= fault && Math.Abs(roadSignsRecog[i].Right - rightCol[i]) <= fault)
                {
                    amountRightRect++;
                }

            }

            Double precision = (Double)(amountRightRect / amountSign * 100.0);
            if(amountSign >= roadSignsRecog.Length)
            {
                wrongCountSigns = roadSignsRecog.Length - amountRightRect;
            }
            else
            {
                wrongCountSigns = 0;
            }

            return precision;
        }

        public TimeSpan RecSigns(System.Windows.Controls.Image pictureBox, Image<Bgr, Byte> image, Double scaleFactor, 
            String fileXML, Int32 minNeighbors, Dispatcher dispatcher)
        {
            try
            {
                CascadeClassifier cascadeClassifier = new CascadeClassifier(fileXML);

                Bitmap bitmap = image.ToBitmap();
                Image<Bgr, Byte> grayImage = new Image<Bgr, Byte>(bitmap);

                DateTime start = new DateTime();
                start = DateTime.Now;
                roadSignsRecog = cascadeClassifier.DetectMultiScale(grayImage, scaleFactor, minNeighbors);
                TimeSpan ms = DateTime.Now.Subtract(start);

                foreach (Rectangle roadSign in roadSignsRecog)
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        using (Pen pen = new Pen(Color.Blue, 4))
                        {
                            graphics.DrawRectangle(pen, roadSign);
                        }
                    }
                }
                grayImage.Bitmap = bitmap;
                dispatcher.Invoke(() =>
                {
                    pictureBox.Source = Helper.LoadBitmap(bitmap);
                });

                return ms;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
