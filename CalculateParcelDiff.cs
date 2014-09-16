using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DisaggregationTool
{
    class CalculateParcelDiff
    {
        public static double[,] Calculate(double[,] lUseBase, int numLUseVars, int[] tazId, Dictionary<int, int> tazIndDictionary, double[,] sumLUseInDri,
            double[] sumAreaInDri, double[,] sumLUseInTaz, double[] sumAreaInTaz, double[,] shareLUseInDri, double[] shareAreaInDri, double[,] shareLUseInTaz,
            double[] shareAreaInTaz, double[,] diffTaz)
        {
            int numParcel = lUseBase.GetLength(0);
            double[,] diffParcel = new double[numParcel, numLUseVars];

            for (int i = 0; i < numParcel; i++)
            {
                int taz = tazId[i];
                int tazIndex = tazIndDictionary[taz];// forecast - this requires key = taz; value = taz index

                for (int j = 0; j < numLUseVars; j++)
                {
                    double totalSectorInDri = sumLUseInDri[taz-1, j];
                    double totalSectorInTaz = sumLUseInTaz[taz-1, j];
                    double totalEmplInDri =  sumLUseInDri[taz-1, numLUseVars - 1];
                    double totalEmplInTaz = sumLUseInTaz[taz-1, numLUseVars - 1];
                    double totalAreaInDri =  sumAreaInDri[taz-1];
                    double totalAreaInTaz = sumAreaInTaz[taz-1];

                    if (totalSectorInDri > 0) diffParcel[i, j] = diffTaz[taz-1, j] * shareLUseInDri[i, j];
                    else if (totalSectorInTaz > 0) diffParcel[i, j] = diffTaz[taz-1, j] * shareLUseInTaz[i, j];
                    else if (totalEmplInDri > 0) diffParcel[i, j] = diffTaz[taz-1, j] * shareLUseInTaz[i, numLUseVars - 1];
                    else if (totalEmplInTaz > 0) diffParcel[i, j] = diffTaz[taz-1, j] * shareLUseInTaz[i, numLUseVars - 1];
                    else if (totalSectorInTaz > 0) diffParcel[i, j] = diffTaz[taz-1, j] * shareAreaInTaz[i];
                    else diffParcel[i, j] = diffTaz[taz-1, j] * shareAreaInTaz[i];

                }

            }

            return diffParcel;
        }
    }
}
