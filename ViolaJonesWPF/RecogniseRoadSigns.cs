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
        public void RecogniseSigns(CascadeClassifier cascadeClassifier, Image<Bgr, Byte> image, Double scaleFactor, Int32 minNeighbors, 
            out Rectangle[] rects, out TimeSpan timeRecognition)
        {
            try
            {
                Bitmap bitmap = image.ToBitmap();
                Image<Bgr, Byte> grayImage = new Image<Bgr, Byte>(bitmap);

                DateTime start = new DateTime();
                start = DateTime.Now;
                rects = cascadeClassifier.DetectMultiScale(grayImage, scaleFactor, minNeighbors);
                timeRecognition = DateTime.Now.Subtract(start);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
