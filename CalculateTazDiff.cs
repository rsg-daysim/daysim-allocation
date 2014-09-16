using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DisaggregationTool
{
    class CalculateTazDiff
    {
        public static double[,] Calculate(double[,] lUse_T_Base, double[,] lUse_T_Forecst,
            Dictionary<int, int> tazIndBaseDictionary, Dictionary<int, int> tazIndForecasteDictionary, int numLUseVars, int numTaz)
        {
            //int numTaz = lUse_T_Forecst.Length;
            double[,] diffTaz = new double[numTaz,numLUseVars];

            for (int i = 0; i < numTaz; i++)
            {
                //int taz = tazIndForecasteDictionary[i];
                //int baseIndex = tazIndBaseDictionary[taz];

                for (int j = 0; j < 9; j++)
                {
                    diffTaz[i,j] = lUse_T_Forecst[i,j] - lUse_T_Base[i,j];
                }
            }

            return diffTaz;

        }
    }
}
