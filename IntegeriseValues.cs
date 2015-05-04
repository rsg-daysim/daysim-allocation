using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DisaggregationTool
{
    class IntegeriseValues
    {
        public static void Calculate(out int[,] lUse_P_round, out int[,] k12Enroll_P_round, out int[] hiEduc_P_round, int[] hh_P, double[] gq_P, double[,] lUse_P,
            double[,] k12Enroll_P, double[] hiEduc_P, long[,] parcelVacantCorrespondence, int numLUseVars_P, string outFolder)
        {
            // bucket rounding
            int numParcel = lUse_P.GetLength(0);
            long tazid = 0;
            long prev_tazid = 0;
            long prev_parcelid = 0;

            int[] hh_P_round = new int[numParcel];
            lUse_P_round = new int[numParcel, numLUseVars_P];
            k12Enroll_P_round = new int[numParcel, 2];
            hiEduc_P_round = new int[numParcel];

            double leftOver_HH = 0;
            double[] leftOver_luse = new double[numLUseVars_P];
            double[] leftOver_enroll = new double[3];

            double[,] k12EnrollTaz = new double[3000, 2];
            double[] hiEducTaz = new double[3000];
            double[] test = new double[3000];

            string outputFileName = outFolder + "\\ForecastTaz_Round.csv";
            StreamWriter sw = new StreamWriter(File.Create(outputFileName));
            string header = "TAZ,stugrd,stuhgh,stuuni,leftover";
            sw.Write(header);

            for (int p=0;p<numParcel;p++)
            {
                long parcelid = parcelVacantCorrespondence[p, 0];
                tazid = parcelVacantCorrespondence[p, 1];

                if (tazid != prev_tazid)
                {
                    // if anything is remaining - shouldn't be the case
                    if (p > 0)
                    {
                        if (leftOver_HH != 0)
                        {
                            hh_P_round[prev_parcelid-1] += (int)Math.Round(leftOver_HH);
                            leftOver_HH = 0;
                        }

                        if ((leftOver_enroll[0] + leftOver_enroll[1]) != 0) //it should always be integer
                        {
                            int extra = (int)Math.Round(leftOver_enroll[0] + leftOver_enroll[1]);
                            int enrol1 = k12Enroll_P_round[prev_parcelid - 1, 0];
                            int enrol2 = k12Enroll_P_round[prev_parcelid - 1, 1];
                            if (k12Enroll_P_round[prev_parcelid - 1, 0] > k12Enroll_P_round[prev_parcelid - 1, 1]) k12Enroll_P_round[prev_parcelid - 1, 0] += (int)Math.Round(leftOver_enroll[0] + leftOver_enroll[1]);
                            else k12Enroll_P_round[prev_parcelid - 1, 1] += (int)Math.Round(leftOver_enroll[0] + leftOver_enroll[1]);

                            test[prev_tazid - 1] = (leftOver_enroll[0] + leftOver_enroll[1]);

                            leftOver_enroll[0] = 0;
                            leftOver_enroll[1] = 0;
                            
                        }

                        if (leftOver_enroll[2] != 0)
                        {
                            hiEduc_P_round[prev_parcelid-1] += (int)Math.Round(leftOver_enroll[2]);
                            leftOver_enroll[2] = 0;
                        }
                    }

                }

                for (int i = 0; i < numLUseVars_P-1; i++)
                {
                    //employment
                    double valueLUse = lUse_P[parcelid - 1, i];
                    double diffLUse = valueLUse - Math.Round(valueLUse);

                    leftOver_luse[i] += diffLUse;

                    //assign only if value is greater than 0
                    if (valueLUse > 0 && leftOver_luse[i] >= 0.99)
                    {
                        valueLUse += 1;
                        leftOver_luse[i] -= 1;
                    }

                    if (valueLUse > 0 && leftOver_luse[i] <= -0.99)
                    {
                        valueLUse -= 1;
                        leftOver_luse[i] += 1;
                    }

                    lUse_P_round[parcelid - 1,i] = (int)Math.Round(valueLUse);
                    lUse_P_round[parcelid - 1, 9] += lUse_P_round[parcelid - 1, i];
                }

                // for enrollment

                //stugrd
                double value1 = k12Enroll_P[parcelid-1, 0];
                double diff = value1 - Math.Round(value1);
                leftOver_enroll[0] += diff;

                k12Enroll_P_round[parcelid-1, 0] = (int)Math.Round(value1);

                //stuhgh
                double value2 = k12Enroll_P[parcelid-1, 1];
                double diff2 = value2 - Math.Round(value2);
                leftOver_enroll[1] += diff2;

                k12Enroll_P_round[parcelid-1, 1] = (int)Math.Round(value2);

                if ((value1>0 || value2>0) && leftOver_enroll[0] + leftOver_enroll[1] >= 0.99)
                {
                    if (k12Enroll_P_round[parcelid - 1, 0] > k12Enroll_P_round[parcelid - 1, 1]) k12Enroll_P_round[parcelid - 1, 0] += 1;
                    else k12Enroll_P_round[parcelid - 1, 1] += 1;

                    if (leftOver_enroll[0] > leftOver_enroll[1])
                    {
                        leftOver_enroll[0] -= (1 - leftOver_enroll[1]);
                        leftOver_enroll[1] = 0;
                    }
                    else
                    {
                        leftOver_enroll[1] -= (1 - leftOver_enroll[0]);
                        leftOver_enroll[0] = 0;
                    }

                }

                if ((value1>0 || value2>0) && leftOver_enroll[0] + leftOver_enroll[1] <= -0.99)
                {
                    if (k12Enroll_P_round[parcelid - 1, 0] > k12Enroll_P_round[parcelid - 1, 1]) k12Enroll_P_round[parcelid - 1, 0] -= 1;
                    else k12Enroll_P_round[parcelid - 1, 1] -= 1;

                    if (leftOver_enroll[0] < 0)
                    {
                        if (Math.Abs(leftOver_enroll[0]) > Math.Abs(leftOver_enroll[1]))
                        {
                            if (Math.Abs(leftOver_enroll[0]) > 1)
                            {
                                leftOver_enroll[0] += 1;
                                leftOver_enroll[1] = leftOver_enroll[1];
                            }
                            else
                            {
                                // both are negative
                                leftOver_enroll[0] += (1 + leftOver_enroll[1]);
                                leftOver_enroll[1] = 0;
                            }
                        }
                        else
                        {
                            // both are negative
                            leftOver_enroll[1] += (1 + leftOver_enroll[0]);
                            leftOver_enroll[0] = 0;
                        }
                    }
                    else
                    {
                        //leftOver_enroll[1] is negative
                        if (Math.Abs(leftOver_enroll[1]) > Math.Abs(leftOver_enroll[0]))
                        {
                            if (Math.Abs(leftOver_enroll[1]) > 1)
                            {
                                leftOver_enroll[1] += 1;
                                leftOver_enroll[0] = leftOver_enroll[0];
                            }
                        }
                    }

                }


                //stuuni
                double value3 = hiEduc_P[parcelid-1];
                double diff3 = value3 - Math.Round(value3);

                leftOver_enroll[2] += diff3;

                //assign only if value is greater than 0
                if (value3 > 0 && leftOver_enroll[2] >= 1)
                {
                    value3 += 1;
                    leftOver_enroll[2] -= 1;
                }

                if (value3 > 0 && leftOver_enroll[2] <= -1)
                {
                    value3 -= 1;
                    leftOver_enroll[2] += 1;
                }

                hiEduc_P_round[parcelid-1] = (int)Math.Round(value3);

                k12EnrollTaz[tazid - 1, 0] += k12Enroll_P_round[parcelid-1, 0];
                k12EnrollTaz[tazid - 1, 1] += k12Enroll_P_round[parcelid-1, 1];
                hiEducTaz[tazid - 1] += hiEduc_P_round[parcelid-1];

                prev_tazid = tazid;
                prev_parcelid = parcelid;

            }

            for (int i = 0; i < 3000; i++)
            {
                sw.Write(Environment.NewLine + (i + 1) + "," + k12EnrollTaz[i, 0] + "," + k12EnrollTaz[i, 1] + "," + hiEducTaz[i] + "," + test[i]);
            }
            sw.Dispose();

        }
    }
}
