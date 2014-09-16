using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DisaggregationTool
{
    class CalculateSum
    {
        public static void Calculate(out double[,] sumLUseDri, out double[] sumAreaDri, int[,] parcelDriCorrespondence, int numTaz, int numLUseVars, double[,] lUseBase, double[] areaBase, 
            int[] tazId, Dictionary<int, int> tazIndDictionary)
        {
            sumLUseDri = new double[numTaz, numLUseVars];
            sumAreaDri = new double[numTaz];

            int numParcel = lUseBase.GetLength(0);

            //string outputFileName = "C:\\Projects\\FDOT Allocation Tool\\inputs\\Input Data\\sumTaz.csv";
            //StreamWriter sw = new StreamWriter(File.Create(outputFileName));

            for (int i = 0; i < numParcel; i++)
            {
                int driFlag = parcelDriCorrespondence[i,2];
                int taz = tazId[i];

                //int tazIndex = tazIndDictionary[taz];// this requires key = taz; value = taz index

                if (driFlag == 1)
                {
                    for (int j = 0; j < numLUseVars; j++) sumLUseDri[taz-1, j] = sumLUseDri[taz-1, j] + lUseBase[i, j];
                    sumAreaDri[taz-1] = sumAreaDri[taz-1] + areaBase[i];
                }
                
            }

            //for (int i = 0; i < numTaz; i++)
            //{
            //    sw.Write(Environment.NewLine + (i+1) + "," + sumAreaDri[i]);
            //}
            //sw.Dispose();

        }
    }
}
