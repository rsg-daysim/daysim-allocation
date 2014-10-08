using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DisaggregationTool
{
    class AggregateToTazBase
    {
        public static void Aggregate(out int[] HH_T, out double[] gq_t, out double[,] LUse_T, out int[,] K12Enroll_T, out int[] HiEduc_T, out double[] area, 
            out Dictionary<int, int> tazIndDictionary, out int[] DUDensity, out int[] numVacantParcelsInTaz,  out int[] numVacantParcelsInDri, 
            out int[] numParcels_T, long[] parcelID, int[] tazId, 
            int[] HH_P, double[] GQ_P, double[,] lUse_P,
            int[,] K12Enroll_P, int[] HiEduc_P, double[] area_P, int numLUseVars_P, int numTaz, int[,] parcelDriCorres)
        {
            //int numTaz = tazId.Distinct().ToArray().Length;
            int numParcel = parcelID.Length;

            numParcels_T = new int[numTaz];
            HH_T = new int[numTaz];
            gq_t = new double[numTaz];

            LUse_T = new double[numTaz, numLUseVars_P];
            //Enroll_T = new double[numTaz, 4];
            K12Enroll_T = new int[numTaz, 2];
            HiEduc_T = new int[numTaz];
            area = new double[numTaz];
            tazIndDictionary = new Dictionary<int,int>();
            DUDensity = new int[numTaz];
            numVacantParcelsInTaz = new int[numTaz];
            numVacantParcelsInDri = new int[numTaz];

            string outputFileName = "C:\\Projects\\FDOT Allocation Tool\\inputs\\Input Data\\BaseTaz.csv";
            StreamWriter sw = new StreamWriter(File.Create(outputFileName));
            string header = "TAZ,HH,GQ,stugrd,stuhgh,stuuni";
            sw.Write(header);

            for (int i = 0; i < numParcel; i++)
            {
                int taz = tazId[i];

                // Households
                HH_T[taz - 1] += HH_P[i];

                // GQ
                gq_t[taz - 1] += GQ_P[i];

                // to calculate DU density
                int vacant = parcelDriCorres[i, 3];
                if (vacant == 0) numParcels_T[taz - 1] += 1;
                else
                {
                    numVacantParcelsInTaz[taz - 1] += 1;
                    int driflag = parcelDriCorres[i, 3];
                    if (driflag == 1) numVacantParcelsInDri[taz - 1] += 1;
                }

                // aggregate employment categories
                for (int j = 0; j < numLUseVars_P; j++)
                {
                    LUse_T[taz-1,j] = LUse_T[taz-1,j] + lUse_P[i,j];
                }

                // Enrollment - k-12
                for (int j = 0; j < 2; j++)
                {
                    K12Enroll_T[taz - 1, j] = K12Enroll_T[taz - 1, j] + K12Enroll_P[i, j];
                }

                // Enrollment - higher education (universities)
                HiEduc_T[taz - 1] = HiEduc_T[taz - 1] + HiEduc_P[i];

                // aggregate area
                area[taz-1] = area[taz-1] + area_P[i];

                // save the correspondece of tazid and index
                tazIndDictionary[taz] = i; // "key = taz; value = index - is it needed?
            }

            var temp = tazId.Distinct().ToArray();
            int length = temp.Length;

            for (int i = 0; i < numTaz; i++)
            {
                if (numParcels_T[i] > 0)
                {
                    int density = Convert.ToInt16((double)HH_T[i] / (double)numParcels_T[i]);
                    if (density < 1) density = 1;

                    DUDensity[i] = density;
                }


                sw.Write(Environment.NewLine + (i + 1) + "," + HH_T[i] + "," + gq_t[i] + "," + K12Enroll_T[i, 0] + "," + K12Enroll_T[i, 1] + "," + HiEduc_T[i]);
            }
            sw.Dispose();

        }
    }
}
