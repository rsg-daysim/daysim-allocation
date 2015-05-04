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
            out int[] numParcels_T, out int[] avgEnrolSize, out double[] avgEnrolAreaSize, out double fractionStugrd, out double[] maxEnrolDensity,
            long[] parcelID, int[] tazId, 
            int[] HH_P, double[] GQ_P, double[,] lUse_P,
            int[,] K12Enroll_P, int[] HiEduc_P, double[] area_P, int numLUseVars_P, int numTaz, int[,] parcelDriCorres, string outFolder)
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
            maxEnrolDensity = new double[3];
            double enrolDensity = 0;

            int[] regionTotalEnrol = new int[3];
            int[] regionNumEnrolParcels = new int[3];
            double[] regionEnrolArea = new double[3];
            avgEnrolSize = new int[3];
            avgEnrolAreaSize = new double[3];

            fractionStugrd = 0;
            double fractionStuhgh = 0;

            string outputFileName = outFolder + "\\BaseTaz.csv";
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

                // stugrd sum for the region
                if (K12Enroll_P[i, 0] > 0)
                {
                    regionTotalEnrol[0] += K12Enroll_P[i, 0];
                    regionNumEnrolParcels[0] += 1;
                    regionEnrolArea[0] += area_P[i];
                    enrolDensity = (double)K12Enroll_P[i, 0] / (double)area_P[i];

                    if (enrolDensity > maxEnrolDensity[0]) maxEnrolDensity[0] = enrolDensity;

                }

                // stuhgh sum for the region
                if (K12Enroll_P[i, 1] > 0)
                {
                    regionTotalEnrol[1] += K12Enroll_P[i, 1];
                    regionNumEnrolParcels[1] += 1;
                    regionEnrolArea[1] += area_P[i];
                    enrolDensity = (double)K12Enroll_P[i, 1] / (double)area_P[i];

                    if (enrolDensity > maxEnrolDensity[1]) maxEnrolDensity[1] = enrolDensity;
                }
                
                // Enrollment - higher education (universities)
                HiEduc_T[taz - 1] = HiEduc_T[taz - 1] + HiEduc_P[i];

                // stuuni sum for the region
                if (HiEduc_P[i] > 0)
                {
                    regionTotalEnrol[2] += HiEduc_P[i];
                    regionNumEnrolParcels[2] += 1;
                    regionEnrolArea[2] += area_P[i];

                    enrolDensity = (double)HiEduc_P[i] / (double)area_P[i];

                    if (enrolDensity > maxEnrolDensity[2]) maxEnrolDensity[2] = enrolDensity;
                }

                // aggregate area
                area[taz-1] = area[taz-1] + area_P[i];

                // save the correspondece of tazid and index
                tazIndDictionary[taz] = i; // "key = taz; value = index - is it needed?

            }

            for (int i = 0; i < 3; i++)
            {
                avgEnrolSize[i] = (int)regionTotalEnrol[i] / regionNumEnrolParcels[i];
                avgEnrolAreaSize[i] = regionEnrolArea[i] / regionNumEnrolParcels[i];
            }

            // should this be enrollment, instead of area?
            fractionStugrd = regionTotalEnrol[0] / (regionTotalEnrol[0] + regionTotalEnrol[1]);
            //fractionStugrd = regionEnrolArea[0] / (regionEnrolArea[0] + regionEnrolArea[1]);
            fractionStuhgh = 1 - fractionStugrd;

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
