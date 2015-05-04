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
            out double[] shareAreaInTaz, out double[] shareAreaSchoolInTaz, out int[] schoolFlag, out int[,] numVacCommResParcels, out int[,] numVacCommParcels, out int[,] numExisCommParcels,
            out int[,] numOtherParcels, int numTaz, int numLUseVars, int[] hhBase, double[] gqBase, double[,] lUseBase, int[,] k12EnrollBase, int[] hiEducBase, double[] areaBase,
            int[] tazId, Dictionary<int, int> tazIndDictionary, int[] sumHHInTaz, double[] sumGQInTaz, double[,] sumLUseInTaz, int[,] sumK12EnrollInTaz, int[] sumHiEducInTaz,
            double[] sumAreaInTaz, double[] sumAreaSchoolInTaz, int[] parcelLUType, double[] avgEnrolAreaSize, string outFolder, double[] maxEnrolDensity, int[] avgEnrolSize, 
            double factor)
        {
            int numParcel = lUseBase.GetLength(0);
            shareHHInTaz = new double[numParcel];
            shareGQInTaz = new double[numParcel];
            shareLUseInTaz = new double[numParcel, numLUseVars];
            shareK12EnrollInTaz = new double[numParcel]; // share for the total K12 enroll = k-8 + high school
            shareHiEducInTaz = new double[numParcel];
            shareAreaInTaz = new double[numParcel];
            shareAreaSchoolInTaz = new double[numParcel];
            schoolFlag = new int[numTaz];

            numVacCommResParcels = new int[numTaz, 3];
            numVacCommParcels = new int[numTaz, 3];
            numExisCommParcels = new int[numTaz, 3];
            numOtherParcels = new int[numTaz, 3];

            // calculate min enrol area for school type
            double[] minSchlArea = new double[3];
            for (int stype = 0; stype < 3; stype++) minSchlArea[stype] = avgEnrolSize[stype] / maxEnrolDensity[stype];
            
            //string outputFileName = outFolder + "\\ParcelShareInTaz.csv";
            string outputFileName = outFolder + "\\TazNumParcelSummary.csv";
            StreamWriter sw = new StreamWriter(File.Create(outputFileName));

            //string header = "parcelid,taz_p,stuk12_p,stuuni_p,areaschool";
            string header = "tazid,stugrd,stuhgh,stuuni";

            // write header
            sw.Write(header);


            for (int i = 0; i < numParcel; i++)
            {
                int taz = tazId[i];
                //int tazIndex = tazIndDictionary[taz];// this requires key = taz; value = taz index

                // share for HH
                if (sumHHInTaz[taz - 1] > 0) shareHHInTaz[i] = (double)hhBase[i] / (double)sumHHInTaz[taz - 1];

                // share for GQ
                if (sumGQInTaz[taz - 1] > 0) shareGQInTaz[i] = (double)gqBase[i] / (double)sumGQInTaz[taz - 1];

                // share for empl
                for (int j = 0; j < numLUseVars; j++)
                {
                    double totalInTaz = sumLUseInTaz[taz - 1, j];
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
                shareAreaInTaz[i] = areaBase[i] / sumAreaInTaz[taz - 1];

                // share for area for school parcels only
                int lutype = parcelLUType[i];

                if (lutype == 24 || lutype == 25 || lutype == 37)
                {
                    schoolFlag[taz - 1] = 1;
                    shareAreaSchoolInTaz[i] = (double)areaBase[i] / (double)sumAreaSchoolInTaz[taz - 1];
                }
                else if (lutype == 35) // vacant commercial
                {
                    for (int stype = 0; stype < 3; stype++)
                    {
                        if ((areaBase[i] <= avgEnrolAreaSize[stype] * factor) && (areaBase[i] >= minSchlArea[stype]))
                        {
                            numVacCommResParcels[taz - 1, stype] += 1;
                            numVacCommParcels[taz - 1, stype] += 1;
                        }
                    }
                }
                else if (lutype == 38) // vacant residential
                {
                    for (int stype = 0; stype < 3; stype++)
                    {
                        if ((areaBase[i] <= avgEnrolAreaSize[stype] * factor) && (areaBase[i] >= minSchlArea[stype])) numVacCommResParcels[taz - 1, stype] += 1;
                    }
                }

                else if (lutype == 17 && lUseBase[taz - 1, 0] > 0) // commercial with educational employment
                {
                    for (int stype = 0; stype < 3; stype++)
                    {
                        if ((areaBase[i] <= avgEnrolAreaSize[stype] * factor) && (areaBase[i] >= minSchlArea[stype])) numExisCommParcels[taz - 1, stype] += 1;
                    }

                }

                else //any parcel with area greater than the minimum band
                {
                    for (int stype = 0; stype < 3; stype++)
                    {
                        if (areaBase[i] >= minSchlArea[stype]) numOtherParcels[taz - 1, stype] += 1;
                    }

                }

                // write to temp file
                //sw.Write(Environment.NewLine + i + "," + tazId[i] + "," + shareK12EnrollInTaz[i] + "," + shareHiEducInTaz[i] + "," + shareAreaSchoolInTaz[i]);
            }

            for (int i = 0; i < numTaz; i++)
            {
                sw.Write(Environment.NewLine + (i + 1) + "," + numVacCommResParcels[i, 0] + "," + numVacCommResParcels[i, 1] + "," + numVacCommResParcels[i, 2]);
            }
            sw.Dispose();

        }
    }
}
