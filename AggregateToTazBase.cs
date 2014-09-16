using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DisaggregationTool
{
    class AggregateToTazBase
    {
        public static void Aggregate(out double[,] LUse_T, out double[] area, out Dictionary<int,int> tazIndDictionary,
            long[] parcelID, int[] tazId, double[,] lUse_P, double[] area_P, int numLUseVars_P, int numTaz)
        {
            //int numTaz = tazId.Distinct().ToArray().Length;
            int numParcel = parcelID.Length;

            LUse_T = new double[numTaz, numLUseVars_P];
            area = new double[numTaz];
            tazIndDictionary = new Dictionary<int,int>();

            string outputFileName = "C:\\Projects\\FDOT Allocation Tool\\inputs\\Input Data\\BaseTaz.csv";
            StreamWriter sw = new StreamWriter(File.Create(outputFileName));

            for (int i = 0; i < numParcel; i++)
            {
                int taz = tazId[i];

                // aggregate employment categories
                for (int j = 0; j < numLUseVars_P; j++)
                {
                    LUse_T[taz-1,j] = LUse_T[taz-1,j] + lUse_P[i,j];
                }

                // aggregate area
                area[taz-1] = area[taz-1] + area_P[i];

                // save the correspondece of tazid and index
                tazIndDictionary[taz] = i; // "key = taz; value = index - is it needed?
            }

            for (int i = 0; i < numTaz; i++)
            {
                sw.Write(Environment.NewLine + (i + 1) + "," + LUse_T[i,9]);
            }
            sw.Dispose();

        }
    }
}
