using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DisaggregationTool
{
    class FactorBlockData
    {
        public static void Calculate(out double[,] lUse_M, out double[] areaSum_T, out double[,] lUseSum_T, int numMZ, int numLUseVars_B,
            int numSchVars, int[] mzId_M, long[] tazId_M, Dictionary<long, int> blockInd, long[] blockId_M, Dictionary<long, int> tazIndDictionary, 
            double[,] lUse_B, double[] area_M, double[] areaSum_B, long[] tazId_T, int numTaz, string outfolder)
        {

            lUse_M = new double[numMZ, numLUseVars_B + numSchVars];
            areaSum_T = new double[numTaz];
            lUseSum_T = new double[numTaz, numLUseVars_B];

            // loop 2 a- to verify calculations
            string test_lusem = Path.Combine(outfolder,"test_lusem.csv");
            StreamWriter sw4 = new StreamWriter(File.Create(test_lusem));
            sw4.Write("MZID,TAZID,HH,CNS01,CNS02,CNS03,CNS04,CNS05,CNS06,CNS07,CNS08,CNS09,CNS10,CNS11,CNS12,CNS13,CNS14,CNS15,CNS16,CNS17,CNS18,CNS19,CNS20,C000");

            // 2. loop micro-zones - factor block data by fraction of block area and sum all by TAZ
            for (int mz = 0; mz < numMZ; mz++)
            {
                sw4.Write(Environment.NewLine + mzId_M[mz] + "," + tazId_M[mz]);

                int block_ind = blockInd[blockId_M[mz]];
                int taz_ind = tazIndDictionary[tazId_M[mz]];

                for (int k = 0; k < numLUseVars_B; k++)
                {
                    double block_luse = lUse_B[block_ind, k];
                    double mz_area = area_M[mz];
                    double block_area = areaSum_B[block_ind];

                    double mz_luse = block_luse * mz_area / block_area;

                    lUse_M[mz, k] = mz_luse;

                    //lUse_M[mz, k] = lUse_B[block_ind, k] * area_M[mz] / areaSum_B[block_ind];

                    lUseSum_T[taz_ind, k] = lUseSum_T[taz_ind, k] + lUse_M[mz, k];

                    sw4.Write("," + lUse_M[mz, k]);

                    double taz_luse = lUseSum_T[taz_ind, k];

                }

                areaSum_T[taz_ind] = areaSum_T[taz_ind] + area_M[mz];
            }

            sw4.Dispose();

            // loop 2 b - to verify calculations
            string test_lusesumbytaz = Path.Combine(outfolder,"test_lusesumbytaz.csv");
            StreamWriter sw3 = new StreamWriter(File.Create(test_lusesumbytaz));
            sw3.Write("TAZID,HH,CNS01,CNS02,CNS03,CNS04,CNS05,CNS06,CNS07,CNS08,CNS09,CNS10,CNS11,CNS12,CNS13,CNS14,CNS15,CNS16,CNS17,CNS18,CNS19,CNS20,C000");

            string test_areasumbytaz = Path.Combine(outfolder, "test_areasumbytaz.csv");
            StreamWriter sw5 = new StreamWriter(File.Create(test_areasumbytaz));
            sw5.Write("TAZID,AREA");

            for (int i = 0; i < numTaz; i++)
            {
                sw3.Write(Environment.NewLine + tazId_T[i]);
                sw5.Write(Environment.NewLine + tazId_T[i]);
                sw5.Write("," + areaSum_T[i]);
                for (int k = 0; k < numLUseVars_B; k++) sw3.Write("," + lUseSum_T[i, k]);

            }

            sw3.Dispose();
            sw5.Dispose();
        }

    }
}
