using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DisaggregationTool
{
    class AddSchoolData
    {
        public static int[,] Calculate(double[,] lUse_M, int numSchools, Dictionary<long, int> mzIndDictionary, int[] mzId_S, int numSchVars, int numLUseVars_B,
            int[,] schEnrl_S, int numMZ)
        {
            int[,] schEnrl_M = new int[numMZ, numSchVars];

            for (int sch = 0; sch < numSchools; sch++)
            {
                int mz_id = mzId_S[sch];

                if (mz_id > 0)
                {
                    int mz_index = mzIndDictionary[mz_id];

                    for (int i = 0; i < numSchVars; i++)
                    {
                        //lUse_M[mz_index, numLUseVars_B + i] = lUse_M[mz_index, numLUseVars_B + i] + schEnrl_S[sch, i];
                        schEnrl_M[mz_index, i] = schEnrl_M[mz_index, i] + schEnrl_S[sch, i];
                        
                    }
                }

            }
            return schEnrl_M;

        }

    }
}
