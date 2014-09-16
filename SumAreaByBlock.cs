using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace DisaggregationTool
{
    class SumAreaByBlock
    {
        public static double[] Calculate(int lUseVars_T, int numLUseVars_B, int numTaz, int numBlocks,
            int numMZ, double[] area_M, Dictionary<long, int> blockIndDictionary, long[] blockId_M, long[] blockId_B, string outfolder)
        {
            int totEmplIndex_T;
            //int hhIndex_B = 0;
            int totEmplIndex_B;

            totEmplIndex_T = lUseVars_T - 1;
            totEmplIndex_B = numLUseVars_B - 1;
            
            //out double[] areaSum_B
            double[] areaSum_B = new double[numBlocks];

            // 1. loop micro-zones - sum area by block
            for (int mz = 0; mz < numMZ; mz++)
            {
                int block_ind = blockIndDictionary[blockId_M[mz]];
                areaSum_B[block_ind] = areaSum_B[block_ind] + area_M[mz];
            }

            // to verify calculations
            string test_areasum = Path.Combine(outfolder,"test_areasum.csv");
            StreamWriter sw2 = new StreamWriter(File.Create(test_areasum));
            sw2.Write("BLOCKID,AREA_SUM");

            for (int i = 0; i < numBlocks; i++)
            {
                long value1 = blockId_B[i];
                double value2 = areaSum_B[i];

                sw2.Write(Environment.NewLine + blockId_B[i]);
                sw2.Write("," + areaSum_B[i]);
            }

            sw2.Dispose();

            return areaSum_B;

        }
    }
}
