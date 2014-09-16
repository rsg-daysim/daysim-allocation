using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DisaggregationTool
{
    class AllocateChangeToParcels
    {
        public static double[,] Allocate(double[,] lUseBase, double[,] diffParcel, int numLUseVars)
        {
            int numParcel = lUseBase.GetLength(0);
            double[,] lUseForecast = new double[numParcel, numLUseVars];

            for (int i = 0; i < numParcel; i++)
            {
                double totalempl = 0;
                for (int j = 0; j < numLUseVars-1; j++)
                {
                    lUseForecast[i, j] = lUseBase[i, j] + diffParcel[i, j];
                    totalempl = totalempl + lUseForecast[i, j];
                }

                lUseForecast[i, numLUseVars - 1] = totalempl;
            }

            return lUseForecast;
        }
    }
}
