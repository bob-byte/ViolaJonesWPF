using System;
using System.Collections.Generic;
using System.IO;

namespace ViolaJonesWPF
{
    class DataImages
    {
        public void ReadTextFile(String nameTextFile, String nameImage, List<String> data, 
            out Boolean isRightRecord, out Int32 amountSign)
        {
            using (StreamReader sr = new StreamReader(nameTextFile))
            {
                String shortNameFile;
                String[] arrayData;

                isRightRecord = false;
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
                        isRightRecord = true;
                    }

                    while (isRightRecord)
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
