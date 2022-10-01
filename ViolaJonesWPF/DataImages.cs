using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ViolaJonesWPF
{
    class ImageData
    {
        public void ReadTextFile(String nameTextFile, String nameImage, List<String> data, 
            out Boolean isRightRecord, out Int32 amountSign)
        {
            using (StreamReader sr = new StreamReader(nameTextFile))
            {
                isRightRecord = false;
                amountSign = 0;

                while (sr.Peek() >= 0)
                {
                    String textRow = sr.ReadLine();
                    Int32 symbolsOfParCount = textRow.IndexOf(';') - 1;

                    var shortNameFile = new StringBuilder(capacity: symbolsOfParCount);
                    for (Int32 numSymbol = 0; numSymbol <= symbolsOfParCount; numSymbol++)
                    {
                        shortNameFile.Append(textRow[numSymbol]);
                    }

                    isRightRecord = nameImage.Equals(shortNameFile.ToString(), StringComparison.OrdinalIgnoreCase);
                    while (isRightRecord)
                    {
                        String[] arrayData = textRow.Split(';');

                        data.Add(arrayData[1]);
                        data.Add(arrayData[2]);
                        data.Add(arrayData[3]);
                        data.Add(arrayData[4]);

                        amountSign++;

                        if (sr.Peek() >= 0)
                        {
                            textRow = sr.ReadLine();
                            symbolsOfParCount = textRow.IndexOf(';') - 1;
                            shortNameFile = new StringBuilder(capacity: symbolsOfParCount);

                            for (Int32 numSymbol = 0; numSymbol <= symbolsOfParCount; numSymbol++)
                            {
                                shortNameFile.Append(textRow[numSymbol]);
                            }

                            if (!nameImage.Equals(shortNameFile.ToString(), StringComparison.OrdinalIgnoreCase))
                            {
                                return;
                            }
                        }
                    }
                }
            }
        }

        public void ParseDataFromDescriptionRoadSigns(List<String> data, Int32 amountSign,
            out Int32[] topRow, out Int32[] leftCol, out Int32[] bottomRow, out Int32[] rightCol)
        {
            topRow = new Int32[amountSign];
            leftCol = new Int32[amountSign];
            bottomRow = new Int32[amountSign];
            rightCol = new Int32[amountSign];

            for (Int32 i = 0, k = 0; i < amountSign * 4 && k < amountSign; i++, k++)
            {
                if (!Int32.TryParse(data[i], out leftCol[k]))
                {
                    throw new FormatException("Left X coordinate could not be read from the test data set");
                }
                else if (!Int32.TryParse(data[++i], out topRow[k]))
                {
                    throw new FormatException("Upper Y coordinate could not be read from the test data set");
                }
                else if (!Int32.TryParse(data[++i], out rightCol[k]))
                {
                    throw new FormatException("Right X coordinate could not be read from the test data set");
                }
                else if (!Int32.TryParse(data[++i], out bottomRow[k]))
                {
                    throw new FormatException("Lower Y coordinate could not be read from the test data set");
                }
            }
        }
    }
}
