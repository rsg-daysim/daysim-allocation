using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DisaggregationTool
{
    class CalculateParcelDiff
    {
        public static void Calculate(out double[] diffGQParcel, out double[,] diffLUseParcel, out double[,] diffK12EnrollParcel, out double[] diffHiEducParcel, 
            double[] gqBase, double[,] lUseBase, 
            int[,] k12EnrollBase, int[] hiEducBase, int numLUseVars, int[] tazId, Dictionary<int, int> tazIndDictionary, double[,] sumLUseInDri, 
            double[] sumAreaInDri, double[] sumGQInTaz, double[,] sumLUseInTaz, int[,] sumK12EnrollInTaz,
            int[] sumHiEducInTaz, double[] sumAreaInTaz, double[,] shareLUseInDri, double[] shareAreaInDri, double[] shareGQInTaz, double[,] shareLUseInTaz,
            double[] shareK12EnrollInTaz, double[] shareHiEducInTaz, double[] shareAreaInTaz, double[] diffGQTaz, double[,] diffLUseTaz, int[] diffK12EnrollTaz, 
            int[] diffHiEducTaz)
        {
            int numParcel = lUseBase.GetLength(0);
            diffGQParcel = new double[numParcel];
            diffLUseParcel = new double[numParcel, numLUseVars];
            diffK12EnrollParcel = new double[numParcel, 2];
            diffHiEducParcel = new double[numParcel];

            string outputFileName = "C:\\Projects\\FDOT Allocation Tool\\inputs\\Input Data\\ParcelDiff.csv";
            StreamWriter sw = new StreamWriter(File.Create(outputFileName));

            string header = "parcelid,taz_p,stugrd_p,stuhgh_p,stuuni_p";

            // write header
            sw.Write(header);

            for (int i = 0; i < numParcel; i++)
            {
                int taz = tazId[i];
                int tazIndex = tazIndDictionary[taz];// forecast - this requires key = taz; value = taz index

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
                    else if (totalSectorInTaz > 0) diffLUseParcel[i, j] = diffLUseTaz[taz-1, j] * shareAreaInTaz[i];
                    else diffLUseParcel[i, j] = diffLUseTaz[taz-1, j] * shareAreaInTaz[i];

                }

                // 2. GQ allocation
                double diffGQ = diffGQTaz[taz - 1];
                double gqTaz = sumGQInTaz[taz - 1];

                if (diffGQ > 0)
                {
                    if (gqTaz > 0) diffGQParcel[i] = (double)diffGQ * shareGQInTaz[i];
                    else diffGQParcel[i] = (double)diffGQ * shareAreaInTaz[i];
                }
                else
                {
                    if (gqTaz > 0) diffGQParcel[i] = (double)diffGQ * shareGQInTaz[i];
                    else diffGQParcel[i] = 0;
                }

                // 3. Enrollment allocation
                int diffK12Enroll = diffK12EnrollTaz[taz - 1];
                int k12Enroll = (sumK12EnrollInTaz[taz - 1, 0] + sumK12EnrollInTaz[taz - 1, 1]);

                // 3.1 Grade and high school enrollment
                if (diffK12Enroll > 0)
                {
                    // increment in enrollment
                    if (k12Enroll > 0)
                    {
                        // K12 enrollment into k-8 (grade school) and 9-12 (highschool)
                        int stuGrdParcel = k12EnrollBase[i, 0];
                        int stuHghParcel = k12EnrollBase[i, 1];
                        int totalK12EnrolParcel = stuGrdParcel + stuHghParcel;

                        if (totalK12EnrolParcel > 0)
                        {
                            diffK12EnrollParcel[i, 0] = (double)diffK12EnrollTaz[taz - 1] * shareK12EnrollInTaz[i] * ((double)stuGrdParcel / (double)totalK12EnrolParcel);
                            diffK12EnrollParcel[i, 1] = (double)diffK12EnrollTaz[taz - 1] * shareK12EnrollInTaz[i] * ((double)stuHghParcel / (double)totalK12EnrolParcel);
                        }
                    }
                    else 
                    {
                        // based on area - change it to by share at taz
                        diffK12EnrollParcel[i, 0] = (double)diffK12EnrollTaz[taz - 1] * shareAreaInTaz[i] * 0.5;
                        diffK12EnrollParcel[i, 1] = (double)diffK12EnrollTaz[taz - 1] * shareAreaInTaz[i] * 0.5;
                    }

                }
                else
                {
                    // reduction in enrollment
                    if (k12Enroll > 0)
                    {
                        // K12 enrollment into k-8 (grade school) and 9-12 (highschool)
                        int stuGrdParcel = k12EnrollBase[i, 0];
                        int stuHghParcel = k12EnrollBase[i, 1];
                        int totalK12EnrolParcel = stuGrdParcel + stuHghParcel;

                        if (totalK12EnrolParcel > 0)
                        {
                            diffK12EnrollParcel[i, 0] = (double)diffK12EnrollTaz[taz - 1] * shareK12EnrollInTaz[i] * ((double)stuGrdParcel / (double)totalK12EnrolParcel);
                            diffK12EnrollParcel[i, 1] = (double)diffK12EnrollTaz[taz - 1] * shareK12EnrollInTaz[i] * ((double)stuHghParcel / (double)totalK12EnrolParcel);
                        }
                    }
                    else //set to 0
                    {
                        diffK12EnrollParcel[i, 0] = 0;
                        diffK12EnrollParcel[i, 1] = 0;
                    }
                }

                // University Enrollment
                int diffHiEducEnroll = diffHiEducTaz[taz - 1];  // diff at a taz
                int hiEducEnroll = sumHiEducInTaz[taz-1];       // enrollment at taz

                if (diffHiEducEnroll > 0)
                {
                    // increment in enrollment
                    if (hiEducEnroll > 0) diffHiEducParcel[i] = (double)diffHiEducTaz[taz - 1] * shareHiEducInTaz[i];
                    else diffHiEducParcel[i] = (double)diffHiEducTaz[taz - 1] * shareAreaInTaz[i];
                }
                else
                {
                    //decrement in enrollment
                    if (hiEducEnroll > 0) diffHiEducParcel[i] = (double)diffHiEducTaz[taz - 1] * shareHiEducInTaz[i];
                    else diffHiEducParcel[i] = 0;
                }

                sw.Write(Environment.NewLine + i + "," + tazId[i] + "," + diffK12EnrollParcel[i,0] + "," + diffK12EnrollParcel[i,1]
                    + "," + diffHiEducParcel[i]);
            }

            sw.Dispose();
        }
    }
}
