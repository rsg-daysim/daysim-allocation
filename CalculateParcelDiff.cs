using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DisaggregationTool
{
    class CalculateParcelDiff
    {
        public static void Calculate(out double[] diffGQParcel, out double[,] diffLUseParcel, double[,] lUseBase, int numLUseVars, int[] tazId, double[,] sumLUseInDri, 
            double[] sumAreaInDri, double[] sumGQInTaz, double[,] sumLUseInTaz, double[] sumAreaInTaz, double[,] shareLUseInDri, double[] shareGQInTaz, double[,] shareLUseInTaz,
            double[] shareAreaInTaz, double[] diffGQTaz, double[,] diffLUseTaz)
        {
            int numParcel = lUseBase.GetLength(0);
            diffGQParcel = new double[numParcel];
            diffLUseParcel = new double[numParcel, numLUseVars];

            for (int i = 0; i < numParcel; i++)
            {
                int taz = tazId[i];

                // 1. Employment allocation
                for (int j = 0; j < numLUseVars; j++)
                {
                    double totalSectorInDri = sumLUseInDri[taz-1, j];
                    double totalSectorInTaz = sumLUseInTaz[taz-1, j];
                    double totalEmplInDri =  sumLUseInDri[taz-1, numLUseVars - 1];
                    double totalEmplInTaz = sumLUseInTaz[taz-1, numLUseVars - 1];
                    double totalAreaInDri =  sumAreaInDri[taz-1];
                    double totalAreaInTaz = sumAreaInTaz[taz-1];

                    if (totalSectorInDri > 0) diffLUseParcel[i, j] = diffLUseTaz[taz-1, j] * shareLUseInDri[i, j];
                    else if (totalSectorInTaz > 0) diffLUseParcel[i, j] = diffLUseTaz[taz-1, j] * shareLUseInTaz[i, j];
                    else if (totalEmplInDri > 0) diffLUseParcel[i, j] = diffLUseTaz[taz-1, j] * shareLUseInTaz[i, numLUseVars - 1];
                    else if (totalEmplInTaz > 0) diffLUseParcel[i, j] = diffLUseTaz[taz-1, j] * shareLUseInTaz[i, numLUseVars - 1];
                    //else if (totalSectorInTaz > 0) diffLUseParcel[i, j] = diffLUseTaz[taz-1, j] * shareAreaInTaz[i]; // commented out during documentation - not sure why it is there
                    else diffLUseParcel[i, j] = diffLUseTaz[taz-1, j] * shareAreaInTaz[i];

                }

                // 2. GQ allocation
                double diffGQ = diffGQTaz[taz - 1];
                double gqTaz = sumGQInTaz[taz - 1];

                // commented out during documentation
                //if (diffGQ > 0)
                //{
                //    if (gqTaz > 0) diffGQParcel[i] = (double)diffGQ * shareGQInTaz[i];
                //    else diffGQParcel[i] = (double)diffGQ * shareAreaInTaz[i];
                //}
                //else
                //{
                //    if (gqTaz > 0) diffGQParcel[i] = (double)diffGQ * shareGQInTaz[i];
                //    else diffGQParcel[i] = 0;
                //}

                // the above can simply be changed into following - changed during documentation
                if (gqTaz > 0) diffGQParcel[i] = (double)diffGQ * shareGQInTaz[i];
                else if (diffGQ>0) diffGQParcel[i] = (double)diffGQ * shareAreaInTaz[i];
                else diffGQParcel[i] = 0;
            }
        }
    }
}
