using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DisaggregationTool
{
    class SharesForParcelsInTaz
    {

        public static void Calculate(out double[] shareHHInTaz, out double[] shareGQInTaz, out double[,] shareLUseInTaz, out double[] shareK12EnrollInTaz, out double[] shareHiEducInTaz, 
            out double[] shareAreaInTaz, int numTaz, int numLUseVars,int[] hhBase, double[] gqBase, double[,] lUseBase, int[,] k12EnrollBase, int[] hiEducBase, double[] areaBase,
            int[] tazId, Dictionary<int, int> tazIndDictionary, int[] sumHHInTaz, double[] sumGQInTaz, double[,] sumLUseInTaz, int[,] sumK12EnrollInTaz, int[] sumHiEducInTaz,
            double[] sumAreaInTaz)
        {
            int numParcel = lUseBase.GetLength(0);
            shareHHInTaz = new double[numParcel];
            shareGQInTaz = new double[numParcel];
            shareLUseInTaz = new double[numParcel, numLUseVars];
            shareK12EnrollInTaz = new double[numParcel]; // share for the total K12 enroll = k-8 + high school
            shareHiEducInTaz = new double[numParcel];
            shareAreaInTaz = new double[numParcel];

            string outputFileName = "C:\\Projects\\FDOT Allocation Tool\\inputs\\Input Data\\ParcelShareInTaz.csv";
            StreamWriter sw = new StreamWriter(File.Create(outputFileName));

            string header = "parcelid,taz_p,stuk12_p,stuuni_p";

            // write header
            sw.Write(header);


            for (int i = 0; i < numParcel; i++)
            {
                int taz = tazId[i];
                int tazIndex = tazIndDictionary[taz];// this requires key = taz; value = taz index
                
                // share for HH
                if (sumHHInTaz[taz-1]>0) shareHHInTaz[i] = (double)hhBase[i] / (double)sumHHInTaz[taz-1];

                // share for GQ
                if (sumGQInTaz[taz - 1]>0) shareGQInTaz[i] = (double)gqBase[i] / (double)sumGQInTaz[taz - 1];

                // share for empl
                for (int j = 0; j < numLUseVars; j++)
                {
                    double totalInTaz = sumLUseInTaz[taz-1, j];
                    if (totalInTaz > 0) shareLUseInTaz[i, j] = lUseBase[i, j] / totalInTaz;
                    else shareLUseInTaz[i, j] = 0;
                }

                //share for K12 enrollment
                int totalK12EnrollInTaz = (sumK12EnrollInTaz[taz - 1, 0] + sumK12EnrollInTaz[taz - 1, 1]);
                int totalK12EnrollInParcel = (k12EnrollBase[i, 0] + k12EnrollBase[i, 1]);

                if (totalK12EnrollInTaz > 0) shareK12EnrollInTaz[i] = (double)totalK12EnrollInParcel / (double)totalK12EnrollInTaz;
                
                // share for university enrollment
                if (sumHiEducInTaz[taz - 1] > 0)
                {
                    int enrollInParcel = hiEducBase[i];
                    int enrollInTaz = sumHiEducInTaz[taz - 1];
                    double share = (double)enrollInParcel / (double)enrollInTaz;
                    shareHiEducInTaz[i] = share;
                }

                // share for area
                shareAreaInTaz[i] = areaBase[i] / sumAreaInTaz[taz-1];

                sw.Write(Environment.NewLine + i + "," + tazId[i] + "," + shareK12EnrollInTaz[i] + "," + shareHiEducInTaz[i]);
            }

            sw.Dispose();
        }
    }
}
