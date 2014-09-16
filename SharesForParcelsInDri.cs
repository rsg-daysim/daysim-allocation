using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DisaggregationTool
{
    class SharesForParcelsInDri
    {
        public static void Calculate(out double[,] shareLUseInDri, out double[] shareAreaInDri, int[,] parcelDriCorrespondence, int numTaz, 
            int numLUseVars, double[,] lUseBase, double[] areaBase, int[] tazId, Dictionary<int, int> tazIndDictionary, double[,] sumLUseInDri, 
            double[] sumAreaInDri)
        {
            int numParcel = lUseBase.GetLength(0);
            shareLUseInDri = new double[numParcel, numLUseVars];
            shareAreaInDri = new double[numParcel];

            for (int i = 0; i < numParcel; i++)
            {
                int driFlag = parcelDriCorrespondence[i, 2];
                int taz = tazId[i];
                int tazIndex = tazIndDictionary[taz];// this requires key = taz; value = taz index

                if (driFlag == 1 && sumLUseInDri[taz-1, numLUseVars-1] > 0)
                {
                    for (int j = 0; j < numLUseVars; j++)
                    {
                        double totalInDri = sumLUseInDri[taz-1, j];
                        if (totalInDri > 0) shareLUseInDri[i, j] = lUseBase[i, j] / totalInDri;
                        else shareLUseInDri[i, j] = 0;
                    }

                    shareAreaInDri[i] = areaBase[i] / sumAreaInDri[taz-1];
                }
            }
        }
    }
}
