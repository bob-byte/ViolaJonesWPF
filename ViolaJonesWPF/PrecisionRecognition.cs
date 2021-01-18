using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViolaJonesWPF
{
    public class PrecisionRecognition
    {
        public Int32 GetAmountRightRect(Rectangle[] roadSignsRecog, Double coefFault,
            Int32[] topRow, Int32[] leftCol, Int32[] bottomRow, Int32[] rightCol)
        {
            Int32 amountSign = topRow.Length;
            Int32 amountRightRect = 0;

            for (Int32 i = 0; i < amountSign; i++)
            {
                Double fault = (bottomRow[i] - topRow[i]) * coefFault;

                for (Int32 j = 0; j < roadSignsRecog.Length; j++)
                {
                    if (Math.Abs(roadSignsRecog[j].Top - topRow[i]) <= fault && Math.Abs(roadSignsRecog[j].Left - leftCol[i]) <= fault &&
                    Math.Abs(roadSignsRecog[j].Bottom - bottomRow[i]) <= fault && Math.Abs(roadSignsRecog[j].Right - rightCol[i]) <= fault)
                    {
                        amountRightRect++;
                        break;
                    }
                }
            }

            return amountRightRect;
        }

        public Double DetermPrecision(Int32 amountSign, Int32 amountRightRect) =>
            (Double)amountRightRect / amountSign * 100;
    }
}
