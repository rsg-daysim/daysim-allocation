using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DisaggregationTool
{
    class SharesForParcelsInTaz
    {

        public static void Calculate(out double[,] shareLUseInTaz, out double[] shareAreaInTaz, int numTaz, 
            int numLUseVars, double[,] lUseBase, double[] areaBase, int[] tazId, Dictionary<int, int> tazIndDictionary, double[,] sumLUseInTaz, 
            double[] sumAreaInTaz)
        {
            int numParcel = lUseBase.GetLength(0);
            shareLUseInTaz = new double[numParcel, numLUseVars];
            shareAreaInTaz = new double[numParcel];

            for (int i = 0; i < numParcel; i++)
            {
                int taz = tazId[i];
                int tazIndex = tazIndDictionary[taz];// this requires key = taz; value = taz index

                for (int j = 0; j < numLUseVars; j++)
                {
                    double totalInTaz = sumLUseInTaz[taz-1, j];
                    if (totalInTaz > 0) shareLUseInTaz[i, j] = lUseBase[i, j] / totalInTaz;
                    else shareLUseInTaz[i, j] = 0;
                }

                shareAreaInTaz[i] = areaBase[i] / sumAreaInTaz[taz-1];
            }
        }
    }
}
