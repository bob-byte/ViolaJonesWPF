using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;

namespace ViolaJonesWPF
{
    public class RecogniseRoadSigns
    {
        public void RecogniseSigns<TColor, TDepth>(
            CascadeClassifier cascadeClassifier, 
            Image<TColor, TDepth> image, 
            Double scaleFactor, 
            Int32 minNeighbors, 
            out Rectangle[] rects, 
            out TimeSpan timeRecognition)

            where TColor : struct, IColor 
            where TDepth : new()
        {
            var grayImage = new Image<Gray, TDepth>(image.Bitmap);

            DateTime start = DateTime.Now;
            rects = cascadeClassifier.DetectMultiScale(grayImage, scaleFactor, minNeighbors);
            timeRecognition = DateTime.Now.Subtract(start);
        }
    }
}
