using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DisaggregationTool
{
    class CalculateTazDiff
    {
        public static void Calculate(out int[] diffHHTaz, out double[] diffGQTaz, out double[,] diffLuseTaz, out int[] diffK12EnrollTaz, out int[] diffHiEducTaz, int[] hh_Base, 
            double[] gq_T_Base, double[,] lUse_T_Base, int[,] k12Enroll_T_Base, int[] hiEduc_T_Base, int[,] hh_Forecast, double[] gq_T_Forecast, double[,] lUse_T_Forecst, int[] k12Enroll_T_Forecast,
            int[] hiEduc_T_Forecast, Dictionary<int, int> tazIndBaseDictionary, Dictionary<int, int> tazIndForecasteDictionary, int numLUseVars, int numTaz, string outFolder)
        {
            //int numTaz = lUse_T_Forecst.Length;
            diffHHTaz = new int[numTaz];
            diffGQTaz = new double[numTaz];
            diffLuseTaz = new double[numTaz, numLUseVars];
            diffK12EnrollTaz = new int[numTaz];
            diffHiEducTaz = new int[numTaz];

            string outputFileName = outFolder + "\\DiffTaz.csv";
            StreamWriter sw = new StreamWriter(File.Create(outputFileName));
            string header = "TAZ,HH,stuk12,stuuni";
            sw.Write(header);

            for (int i = 0; i < numTaz; i++)
            {
                //int taz = tazIndForecasteDictionary[i];
                //int baseIndex = tazIndBaseDictionary[taz];

                for (int j = 0; j < 9; j++)
                {
                    diffLuseTaz[i, j] = lUse_T_Forecst[i, j] - lUse_T_Base[i, j];
                }

                diffHHTaz[i] = hh_Forecast[i,1] - hh_Base[i]; // 0 - totalDU, 1- Permanent Res DU, 2 - Seasonal Res DU
                diffGQTaz[i] = gq_T_Forecast[i] - gq_T_Base[i];

                diffK12EnrollTaz[i] = k12Enroll_T_Forecast[i] - (k12Enroll_T_Base[i, 0] + k12Enroll_T_Base[i, 1]);
                diffHiEducTaz[i] = hiEduc_T_Forecast[i] - hiEduc_T_Base[i];

            }

            for (int i = 0; i < numTaz; i++)
            {
                sw.Write(Environment.NewLine + (i + 1) + "," + diffHHTaz[i] + "," + diffK12EnrollTaz[i] + "," + diffHiEducTaz[i]);
            }

            sw.Dispose();

        }
    }
}
