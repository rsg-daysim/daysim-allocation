using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DisaggregationTool
{
    class FactorToMatchTazTotals
    {
        public static double[,] Calculate(double[,] lUse_M, string outfolder, int numMZ, int[] mzId_M, double[] xCoord_M, double[] yCoord_M,
            double[] area_M, long[] tazId_M, long[] blockId_M, Dictionary<long, int> blockIndDictionary, Dictionary<long, int> tazIndDictionary,
            int totEmplIndex_B, int hhIndex_B, int numLUseVars_B, double[,] lUseInNaics_T, double[,] lUseSum_T, double[] areaSum_T)
        {
            // 4. loop micro-zones - factor micro-zone data to match TAZ totals

            // Create strem writer for writing output
            string test_out = Path.Combine(outfolder, "test_mz_step4.csv");
            StreamWriter sw = new StreamWriter(File.Create(test_out));

            sw.Write("MZID,XCOORD,YCOORD,AREA,TAZID,BLOCKID,HH,CNS01,CNS02,CNS03,CNS04,CNS05,CNS06,CNS07,CNS08,CNS09,CNS10,CNS11,CNS12,CNS13,CNS14,CNS15,CNS16,CNS17,CNS18,CNS19,CNS20,C000");

            double tiny = 0.0001;

            for (int mz = 0; mz < numMZ; mz++)
            {
                // writing to output file
                sw.Write(Environment.NewLine + mzId_M[mz] + "," + xCoord_M[mz] + "," + yCoord_M[mz] + "," + area_M[mz] + "," + tazId_M[mz] + "," + blockId_M[mz]);

                int block_ind = blockIndDictionary[blockId_M[mz]];
                int taz_ind = tazIndDictionary[tazId_M[mz]];
                double totalEmpl = lUse_M[mz, totEmplIndex_B];
                double totalHH = lUse_M[mz, hhIndex_B];

                for (int k = 0; k < numLUseVars_B; k++)
                {
                    double test1 = lUseInNaics_T[taz_ind, k];

                    // no TAZ data
                    if (lUseInNaics_T[taz_ind, k] < tiny) lUse_M[mz, k] = 0;

                    // MZ data derived from block data is available
                    else if (lUse_M[mz, k] > tiny)
                    {
                        double var1 = lUse_M[mz, k];
                        double var2 = lUseInNaics_T[taz_ind, k];
                        double var3 = lUseSum_T[taz_ind, k];

                        lUse_M[mz, k] = lUse_M[mz, k] * lUseInNaics_T[taz_ind, k] / lUseSum_T[taz_ind, k];
                    }

                    // no TAZ data derived from block data 
                    else if (lUseSum_T[taz_ind, k] < tiny)
                    {
                        // empl data and total empl >0
                        if (k > hhIndex_B && lUseSum_T[taz_ind, totEmplIndex_B] > tiny)
                        {
                            lUse_M[mz, k] = lUseInNaics_T[taz_ind, k] * totalEmpl / lUseSum_T[taz_ind, totEmplIndex_B];
                        }

                        // no empl data and households>0
                        else if (k == hhIndex_B && lUseSum_T[taz_ind, hhIndex_B] > tiny)
                        {
                            lUse_M[mz, k] = lUseInNaics_T[taz_ind, k] * totalHH / lUseSum_T[taz_ind, hhIndex_B];
                        }

                        // empl+households>0
                        else if (lUseSum_T[taz_ind, hhIndex_B] + lUseSum_T[taz_ind, totEmplIndex_B] > tiny)
                        {
                            lUse_M[mz, k] = lUseInNaics_T[taz_ind, k] * (totalEmpl + totalHH) / (lUseSum_T[taz_ind, totEmplIndex_B] + lUseSum_T[taz_ind, hhIndex_B]);
                        }

                        // split based on land area
                        else
                        {
                            lUse_M[mz, k] = lUseInNaics_T[taz_ind, k] * area_M[mz] / areaSum_T[taz_ind];
                        }

                    }
                    double test = lUse_M[mz, k];

                    //sw.Write("," + lUse_M[mz, k]);
                    sw.Write("," + lUse_M[mz, k].ToString("0.##"));

                }

            }

            sw.Dispose();

            return lUse_M;

        }


    }
}
