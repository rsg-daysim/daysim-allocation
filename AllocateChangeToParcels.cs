using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DisaggregationTool
{
    class AllocateChangeToParcels
    {
        public static void AllocateLUse(out double[] gqForecast,  out double[,] lUseForecast, double[] gqBase, double[,] lUseBase, double[] diffGQParcel, double[,] diffLUseParcel, 
            int numLUseVars,int[] tazId)
        {
            int numParcel = lUseBase.GetLength(0);
            gqForecast = new double[numParcel];
            lUseForecast = new double[numParcel, numLUseVars];

            double[] gqTaz = new double[3000];

            for (int i = 0; i < numParcel; i++)
            {
                double totalempl = 0;
                int taz = tazId[i];

                gqForecast[i] = gqBase[i] + diffGQParcel[i];

                for (int j = 0; j < numLUseVars-1; j++)
                {
                    lUseForecast[i, j] = lUseBase[i, j] + diffLUseParcel[i, j];
                    totalempl = totalempl + lUseForecast[i, j];
                }

                lUseForecast[i, numLUseVars - 1] = totalempl;

                gqTaz[taz - 1] += gqForecast[i];
                
            }

        }

        public static void AllocateEnrollment(out double[,] k12EnrollForecast, out double[] hiEducForecast, long[] parcelSortedByArea, int[,] k12EnrollBase, int[] hiEducBase, 
            int[,] sumK12EnrollInTaz, int[] sumHiEducInTaz, double[,] LUse_T, double[] shareAreaSchoolInTaz, double[] areaBase,
            double[] shareK12EnrollInTaz, double[] shareHiEducInTaz, int[] diffK12EnrollTaz, int[] diffHiEducTaz, int[] parcelLUType, int[] tazId, double fracStugrd, int[] schoolFlag,
            int[] avgEnrolSize, double[] avgEnrolAreaSize, int[,] numVacCommResParcels, int[,] numVacCommParcels, int[,] numExisCommParcels, int[,] numOtherParcels, string outFolder, 
            double[] maxEnrolDensity, double factor)
        {
            int numParcel = hiEducBase.GetLength(0);
            int numTaz = sumHiEducInTaz.GetLength(0);
            string outputFileName = outFolder + "\\ParcelEnrollment.csv";
            StreamWriter sw = new StreamWriter(File.Create(outputFileName));

            string header = "parcelid,taz_p,stugrd_p,stuhgh_p,stuuni_p";

            // write header
            sw.Write(header);

            double[,] diffK12EnrollParcel = new double[numParcel, 2]; ;
            double[] diffHiEducParcel = new double[numParcel];
            double enrolSize = 0;
            int prev_taz = 0;

            int numReqParcels = 0;
            int numAvailParcels = 0;
            int numReqResParcels = 0;
            int[] numFilledResParcels = new int[3];
            double[] remK12Change = new double[numTaz];
            double[] remHiEducChange = new double[numTaz];

            double[] minSchlArea = new double[3];
            for (int s = 0; s < 3; s++) minSchlArea[s] = (double)avgEnrolSize[s] / (double)maxEnrolDensity[s];


            k12EnrollForecast = new double[numParcel,2];
            hiEducForecast = new double[numParcel];

            // start with copying the diff in taz
            Array.Copy(diffK12EnrollTaz, remK12Change, numTaz);
            Array.Copy(diffHiEducTaz, remHiEducChange, numTaz);

            for (int i = 0; i < numParcel; i++)
            {
                long parcelid = parcelSortedByArea[i]; //iterate by this array

                int taz = tazId[parcelid-1];

                // for debugging - start
                //if (taz == 2282) // also 1442, 
                //{
                //    int test = 9999;
                //    int lutype1 = parcelLUType[parcelid - 1];
                //    int availParc1 = numVacCommResParcels[taz - 1, 0];
                //    int availParc2 = numVacCommResParcels[taz - 1, 1];
                //    int availParc3 = numVacCommResParcels[taz - 1, 2];
                //    int availCommPar = numVacCommParcels[taz - 1, 2];

                //    double k12Diff = remK12Change[taz - 1];
                //    double hiEducDiff = remHiEducChange[taz - 1];

                    //for (int stype = 0; stype < 2; stype++)
                    //{
                    //    if (remK12Change[taz - 1] > 0 && (areaBase[parcelid - 1] <= avgEnrolAreaSize[stype] * factor) && (areaBase[parcelid - 1] >= minSchlArea[taz - 1, stype]))
                    //    {
                    //        if (lutype1 == 35)
                    //        {
                    //            int temp1 = 9999;
                    //        }
                    //        else if (lutype1 == 38)
                    //        {
                    //            int temp2 = 9999;
                    //        }

                    //    }

                    //}

                    //if (remHiEducChange[taz - 1] > 0 && areaBase[parcelid - 1] <= avgEnrolAreaSize[2] && (areaBase[parcelid - 1] >= minSchlArea[taz - 1, 2]))
                    //{
                    //    if (lutype1 == 35)
                    //    {
                    //        int temp3 = 9999;
                    //    }
                    //    else if (lutype1 == 38)
                    //    {
                    //        int temp4 = 9999;
                    //    }

                    //}

                //}

                // end- for debugging

                if (prev_taz != taz)
                {
                    numFilledResParcels[0] = 0;
                    numFilledResParcels[1] = 0;
                    numFilledResParcels[2] = 0;

                }

                // 3. Enrollment allocation
                int diffK12Enroll = diffK12EnrollTaz[taz - 1];
                int k12Enroll = (sumK12EnrollInTaz[taz - 1, 0] + sumK12EnrollInTaz[taz - 1, 1]);

                // 3.1 Grade and high school enrollment
                if (diffK12Enroll > 0)
                {
                    // Existing enrollment in taz
                    // K12 enrollment into k-8 (grade school) and 9-12 (highschool)
                    int stuGrdParcel = k12EnrollBase[parcelid - 1, 0];
                    int stuHghParcel = k12EnrollBase[parcelid - 1, 1];
                    int totalK12EnrolParcel = stuGrdParcel + stuHghParcel;

                    for (int stype = 0; stype < 2; stype++)
                    {

                        // increment in enrollment
                        if (k12Enroll > 0)
                        {
                            // alllocate only if there is existing enrollment
                            if (totalK12EnrolParcel > 0)
                            {

                                k12EnrollForecast[parcelid - 1, 0] = k12EnrollBase[parcelid - 1, 0] + ((double)diffK12EnrollTaz[taz - 1] * shareK12EnrollInTaz[parcelid - 1] * ((double)stuGrdParcel / (double)totalK12EnrolParcel));
                                k12EnrollForecast[parcelid - 1, 1] = k12EnrollBase[parcelid - 1, 1] + ((double)diffK12EnrollTaz[taz - 1] * shareK12EnrollInTaz[parcelid - 1] * ((double)stuHghParcel / (double)totalK12EnrolParcel));
                            }
                        }
                        else
                        {
                            // no existing base enrollment in taz - allocate according to LU description
                            // based on area - change it to by share at taz
                            int lutype = parcelLUType[parcelid - 1];

                            if (schoolFlag[taz - 1] == 1) // this is TAZ check
                            {
                                // vacant school parcels exist
                                if (lutype == 24 || lutype == 25 || lutype == 37) //only to vacant - this parcel check
                                {
                                    k12EnrollForecast[parcelid - 1, 0] = k12EnrollBase[parcelid - 1, 0] + ((double)diffK12EnrollTaz[taz - 1] * shareAreaSchoolInTaz[parcelid - 1] * fracStugrd);
                                    k12EnrollForecast[parcelid - 1, 1] = k12EnrollBase[parcelid - 1, 1] + ((double)diffK12EnrollTaz[taz - 1] * shareAreaSchoolInTaz[parcelid - 1] * (1 - fracStugrd));
                                }
                            }
                            else if (remK12Change[taz - 1] > 0)
                            {
                                // no vacant school parcels
                                numAvailParcels = numVacCommResParcels[taz - 1, stype];
                                enrolSize = avgEnrolSize[stype];

                                // if school size is higher than the remaining enrollment than allocate whatever is remaining
                                if (enrolSize > remK12Change[taz - 1]) enrolSize = remK12Change[taz - 1];

                                if (numAvailParcels > 0) // taz has vacant commerical and residential parcels
                                {
                                    if ((areaBase[parcelid - 1] <= avgEnrolAreaSize[stype] * factor) && (areaBase[parcelid - 1] >= minSchlArea[stype]))
                                    {
                                        if (stype == 0) numReqParcels = (int)Math.Ceiling((double)diffK12EnrollTaz[taz - 1] * fracStugrd / (double)avgEnrolSize[stype]);
                                        else numReqParcels = (int)Math.Ceiling((double)diffK12EnrollTaz[taz - 1] * (1 - fracStugrd) / (double)avgEnrolSize[stype]);

                                        // if available parcels are not sufficient than recalculate school (enrollment) size
                                        if (numAvailParcels < numReqParcels)
                                        {
                                            if (stype == 0) enrolSize = (double)(diffK12EnrollTaz[taz - 1] * fracStugrd) / (double)numAvailParcels;
                                            else enrolSize = (double)(diffK12EnrollTaz[taz - 1] * (1 - fracStugrd)) / (double)numAvailParcels;

                                            //minSchlArea[taz - 1, stype] = (double)enrolSize / (double)maxEnrolDensity[stype];
                                        }

                                        // calculate required vacant parcels to be filled - all vacant commercial parcels need to be filled first and then residential
                                        numReqResParcels = numReqParcels - numVacCommParcels[taz - 1, stype];

                                        if (lutype == 35)
                                        {
                                            // fill vacant commercial parcels
                                            k12EnrollForecast[parcelid - 1, stype] = k12EnrollBase[parcelid - 1, stype] + enrolSize;
                                            remK12Change[taz - 1] -= enrolSize;
                                        }
                                        else if (lutype == 38)
                                        {
                                            // fill vacant residential parcels, only if needed to be filled
                                            if (numFilledResParcels[stype] < numReqResParcels)
                                            {
                                                k12EnrollForecast[parcelid - 1, stype] = k12EnrollBase[parcelid - 1, stype] + enrolSize;
                                                numFilledResParcels[stype] += 1;
                                                remK12Change[taz - 1] -= enrolSize;
                                            }
                                        }
                                    }
                                }

                                else if (numExisCommParcels[taz - 1, stype] > 0 && LUse_T[taz - 1, stype] > 0) //taz has existing commercial parcels (from LU type) with education employment (from daysim input)
                                {
                                    // Area should be within the band
                                    if (lutype == 17 && (areaBase[parcelid - 1] <= avgEnrolAreaSize[stype] * factor) && (areaBase[parcelid - 1] >= minSchlArea[stype]))
                                    {
                                        k12EnrollForecast[parcelid - 1, stype] = k12EnrollBase[parcelid - 1, stype] + enrolSize;
                                        remK12Change[taz - 1] -= enrolSize;
                                    }

                                }

                                else // if nothing is available - assign to other parcels with area higher than the minimum
                                {
                                    //enrolSize = avgEnrolSize[stype];
                                    //if (enrolSize > remK12Change[taz - 1]) enrolSize = remK12Change[taz - 1];
                                    //int otherParcel = numOtherParcels[taz - 1, stype];
                                    //if (otherParcel > 0)
                                    //{
                                    //    if ((areaBase[parcelid - 1] >= minSchlArea[taz - 1, stype]))
                                    //    {
                                    //        k12EnrollForecast[parcelid - 1, stype] = k12EnrollBase[parcelid - 1, stype] + enrolSize;
                                    //        remK12Change[taz - 1] -= enrolSize;
                                    //        //double temp4 = remK12Change[taz - 1];
                                    //    }
                                    //}
                                    int numReqOtherParcels = 0;

                                    if (stype == 0) numReqOtherParcels = (int)Math.Ceiling((double)diffK12EnrollTaz[taz - 1] * fracStugrd / (double)avgEnrolSize[stype]);
                                    else numReqOtherParcels = (int)Math.Ceiling((double)diffK12EnrollTaz[taz - 1] * (1 - fracStugrd) / (double)avgEnrolSize[stype]);

                                    if (numOtherParcels[taz - 1, stype] < numReqOtherParcels)
                                    {
                                        if (stype == 0) enrolSize = (double)diffK12EnrollTaz[taz - 1] * fracStugrd / (double)numOtherParcels[taz - 1, stype];
                                        else enrolSize = (double)diffK12EnrollTaz[taz - 1] * (1 - fracStugrd) / (double)numOtherParcels[taz - 1, stype];

                                        //minSchlArea[taz - 1, stype] = (double)enrolSize / (double)maxEnrolDensity[stype];
                                    }

                                    if (enrolSize > remK12Change[taz - 1]) enrolSize = remK12Change[taz - 1];

                                    if (areaBase[parcelid - 1] >= minSchlArea[stype])
                                    // assign to all parcels - start from the biggest parcel
                                    {
                                        k12EnrollForecast[parcelid - 1, stype] = k12EnrollBase[parcelid - 1, stype] + enrolSize;
                                        remK12Change[taz - 1] -= enrolSize;
                                    }

                                }

                            }

                        }
                    }

                }
                else
                {
                    // reduction in enrollment or no change
                    if (k12Enroll > 0)
                    {
                        // K12 enrollment into k-8 (grade school) and 9-12 (highschool)
                        int stuGrdParcel = k12EnrollBase[parcelid - 1, 0];
                        int stuHghParcel = k12EnrollBase[parcelid - 1, 1];
                        int totalK12EnrolParcel = stuGrdParcel + stuHghParcel;

                        // if there is existing enrollment
                        if (totalK12EnrolParcel > 0)
                        {
                            k12EnrollForecast[parcelid - 1, 0] = k12EnrollBase[parcelid - 1, 0] + ((double)diffK12EnrollTaz[taz - 1] * shareK12EnrollInTaz[parcelid - 1] * ((double)stuGrdParcel / (double)totalK12EnrolParcel));
                            k12EnrollForecast[parcelid - 1, 1] = k12EnrollBase[parcelid - 1, 1] + ((double)diffK12EnrollTaz[taz - 1] * shareK12EnrollInTaz[parcelid - 1] * ((double)stuHghParcel / (double)totalK12EnrolParcel));
                        }
                    }
                    else //set to 0
                    {
                        k12EnrollForecast[parcelid - 1, 0] = 0;
                        k12EnrollForecast[parcelid - 1, 1] = 0;
                    }
                }

                // University Enrollment
                int diffHiEducEnroll = diffHiEducTaz[taz - 1];  // diff at a taz
                int hiEducEnroll = sumHiEducInTaz[taz - 1];       // enrollment at taz

                if (diffHiEducEnroll > 0) // increment in enrollment
                {
                    if (hiEducEnroll > 0) // university enrollment is available in base year
                    {
                        hiEducForecast[parcelid - 1] = hiEducBase[parcelid - 1] + ((double)diffHiEducTaz[taz - 1] * shareHiEducInTaz[parcelid - 1]);
                        double temp = shareHiEducInTaz[parcelid - 1];
                        double temp1 = hiEducBase[i];
                    }
                    else // no existing univ enrollment
                    {
                        // allocate based on LU type
                        int lutype = parcelLUType[parcelid - 1];
                        int flag = schoolFlag[taz - 1];

                        if (schoolFlag[taz - 1] == 1) // vacant school and institutional parcels
                        {
                            if (lutype == 24 || lutype == 25 || lutype == 37) // only to vacant
                            {
                                hiEducForecast[parcelid - 1] = hiEducBase[parcelid - 1] + ((double)diffHiEducTaz[taz - 1] * shareAreaSchoolInTaz[parcelid - 1]);
                                //double temp = (double)diffHiEducTaz[taz - 1] * shareAreaSchoolInTaz[parcelid - 1];
                                //double temp2 = 0;
                            }
                        }
                        else if (remHiEducChange[taz - 1] > 0) // no vacant school parcels
                        {
                            // availbale vacant commercial and residential parcels
                            numAvailParcels = numVacCommResParcels[taz - 1, 2];
                            enrolSize = avgEnrolSize[2];

                            // if school size is higher than the remaining enrollment than allocate whatever is remaining
                            if (enrolSize > remHiEducChange[taz - 1]) enrolSize = remHiEducChange[taz - 1];

                            if (numAvailParcels > 0)
                            {
                                // for stuuni
                                if (areaBase[parcelid - 1] <= avgEnrolAreaSize[2] && (areaBase[parcelid - 1] >= minSchlArea[2]))
                                {
                                    numReqParcels = (int)Math.Ceiling((double)diffHiEducTaz[taz - 1] / (double)avgEnrolSize[2]);

                                    // if available parcels are not sufficient than recalculate school (enrollment) size
                                    if (numAvailParcels < numReqParcels)
                                    {
                                        enrolSize = (double)diffHiEducTaz[taz - 1] / (double)numAvailParcels;
                                        //minSchlArea[taz - 1, 2] = (double)enrolSize / (double)maxEnrolDensity[2];
                                    }

                                    // calculate required vacant parcels to be filled - all vacant commercial parcels need to be filled first and then residential
                                    numReqResParcels = numReqParcels - numVacCommParcels[taz - 1, 2];

                                    if (lutype == 35)
                                    {
                                        // fill vacant commercial parcels
                                        hiEducForecast[parcelid - 1] = hiEducBase[parcelid - 1] + enrolSize;
                                        remHiEducChange[taz - 1] -= enrolSize;
                                    }
                                    else if (lutype == 38)
                                    {
                                        // fill vacant residential parcels, only if needed to be filled
                                        if (numFilledResParcels[2] < numReqResParcels)
                                        {
                                            hiEducForecast[parcelid - 1] = hiEducBase[parcelid - 1] + enrolSize;
                                            numFilledResParcels[2] += 1;
                                            remHiEducChange[taz - 1] -= enrolSize;
                                        }

                                    }
                                }
                            }

                            else if (numExisCommParcels[taz - 1, 2] > 0 && LUse_T[taz - 1, 0] > 0) //existing commercial parcels (from LU type) with education employment (from daysim input)
                            {
                                // Area should be within the band
                                if (lutype == 17 && (areaBase[parcelid - 1] <= avgEnrolAreaSize[2] * factor) && (areaBase[parcelid - 1] >= minSchlArea[2]))
                                {
                                    hiEducForecast[parcelid - 1] = hiEducBase[parcelid - 1] + enrolSize;
                                    remHiEducChange[taz - 1] -= enrolSize;
                                }

                            }

                            else // if nothing is available - assign to parcels within the area band
                            {
                                //int otherParcel = numOtherParcels[taz - 1, 2];
                                //if (otherParcel > 0)
                                //{
                                //    if ((areaBase[parcelid - 1] <= avgEnrolAreaSize[2] * factor) && (areaBase[parcelid - 1] >= minSchlArea[taz - 1, 2]))
                                //    {
                                //        hiEducForecast[parcelid - 1] = hiEducBase[parcelid - 1] + enrolSize;
                                //        remHiEducChange[taz - 1] -= enrolSize;
                                //    }
                                //}

                                //}

                                int numReqOtherParcels = 0;

                                numReqOtherParcels = (int)Math.Ceiling((double)diffHiEducTaz[taz - 1] / (double)avgEnrolSize[2]);

                                if (numOtherParcels[taz - 1, 2] < numReqOtherParcels)
                                {
                                    enrolSize = (double)diffHiEducTaz[taz - 1] / (double)numOtherParcels[taz - 1, 2];
                                    //minSchlArea[taz - 1, 2] = (double)enrolSize / (double)maxEnrolDensity[2]; // don't need to recalculate as it may remove some parcels from the set
                                }

                                // assign to all parcels - start from the biggest parcel
                                if (areaBase[parcelid - 1] >= minSchlArea[2])
                                {
                                    hiEducForecast[parcelid - 1] = hiEducBase[parcelid - 1] + enrolSize;
                                    remHiEducChange[taz - 1] -= enrolSize;
                                }

                            }
                        }

                    }
                }
                else
                {
                    //decrement or no change in enrollment
                    if (hiEducEnroll > 0) hiEducForecast[parcelid - 1] = hiEducBase[parcelid - 1] + ((double)diffHiEducTaz[taz - 1] * shareHiEducInTaz[parcelid - 1]);
                    else hiEducForecast[parcelid - 1] = 0;
                }

                prev_taz = taz;

                sw.Write(Environment.NewLine + i + "," + tazId[parcelid - 1] + "," + diffK12EnrollParcel[parcelid - 1, 0] + "," + diffK12EnrollParcel[parcelid - 1, 1]
                    + "," + diffHiEducParcel[parcelid - 1]);
            }

            sw.Dispose();

        }

        public static void AllocateHH(out int[] hhForecast, int[] diffHHTaz,int[] hhBase, int[] duDensity, int[] numVacantParcelsInTaz,
            int[] numVacantParcelsInDri, int[] numParcelsInTaz, long[,] parcelVacantCorres, int[] tazId, double[] shareHHInTaz, string outFolder)
        {
            int numParcel = hhBase.GetLength(0);
            int numTaz = diffHHTaz.GetLength(0);
            hhForecast = new int[numParcel];
            int[] hhForecastTaz = new int[numTaz];
            int[] remChange = new int[numTaz];
            long prevTaz = 0;
            int supplyTaz = 0;
            int hhDensity = 0;
            int numParcels = 0;

            // start with copying the diff in taz
            Array.Copy(diffHHTaz,remChange,numTaz);

            string outputFileName = outFolder + "\\temp_HHForecastTaz.csv";
            StreamWriter sw = new StreamWriter(File.Create(outputFileName));
            string header = "TAZ,HH";
            sw.Write(header);

            // IMPORTANT:
            // ParcelTaz Correspondence file: parcel id, taz id, driid, driflag, vacant flag 
            // Also the input is already sorted by: tazid, vacant, driflag

            for (int i = 0; i < numParcel; i++)
            {
                long parcelid = parcelVacantCorres[i,0];  // need to change the way it is stored while reading, it should be stored by index not by parcel id
                long taz = parcelVacantCorres[i,1];

                int changeInTaz = remChange[taz - 1];

                int vacantParcelsInTaz = numVacantParcelsInTaz[taz - 1];
                int vacantParcelsInDri = numVacantParcelsInDri[taz - 1];

                if (taz != prevTaz)
                {
                    hhDensity = duDensity[taz - 1];
                    supplyTaz = hhDensity * vacantParcelsInTaz;
                    numParcels = numParcelsInTaz[taz - 1];
                }

                if (changeInTaz < 0)
                {
                    // redution in housing units
                    // decrement from all parcels (except vacant) - not checked, assumed that vacant parcel have no DU
                    // based on share

                    int diffHH = diffHHTaz[taz - 1];
                    double share = shareHHInTaz[parcelid - 1];

                    int decrement=0;
                    if (share > 0) decrement = ((int)Math.Ceiling(diffHH * share)) - 1;
                    
                    //if (hhBase[parcelid - 1] < Math.Abs(decrement)) decrement = -hhBase[parcelid - 1];

                    //numParcels = numParcelsInTaz[taz - 1];

                    //int decrement = ((int)Math.Ceiling((double)changeInTaz / (double)numParcels)) - 1; // ceiling will always take higher no. so deduct one
                    //if (Math.Abs(decrement) < 1) decrement = -1;
                    //if ((remChange[taz - 1] - decrement) > 0) decrement = remChange[taz - 1];
                    //if (hhBase[parcelid - 1] < Math.Abs(decrement)) decrement = -hhBase[parcelid - 1];

                    if ((remChange[taz - 1] - decrement) > 0) decrement = remChange[taz - 1];

                    if (hhBase[parcelid - 1] < Math.Abs(decrement)) decrement = -hhBase[parcelid - 1];

                    hhForecast[parcelid - 1] = hhBase[parcelid - 1] + decrement;

                    remChange[taz - 1] = remChange[taz - 1] - decrement;
                    numParcels = numParcels - 1;

                }
                else if (changeInTaz > 0)
                {
                    // increase in housing units
                    if (supplyTaz >= changeInTaz)
                    {
                        int forecast = 0;
                        int remDelta = 0;

                        AllocateIncrement(out forecast, out remDelta, hhBase[parcelid - 1], hhDensity, remChange[taz - 1]);

                        remChange[taz - 1] = remDelta;
                        hhForecast[parcelid - 1] = forecast;

                    }
                    else
                    {
                        // less parcels to allocate in taz than the increase in houseing units
                        // recalculate DU density
                        int diffHH = diffHHTaz[taz - 1];
                        int newhhDensity = hhDensity;
                        if (vacantParcelsInTaz > 0) newhhDensity = (int)Math.Ceiling((double)diffHH / (double)vacantParcelsInTaz); // this should be kept constant, so use initial difference
                        else
                        {
                            int numParcels1 = numParcelsInTaz[taz - 1];
                            newhhDensity = (int)Math.Ceiling((double)diffHH / (double)numParcels1);

                            // if new density is very small, close 0, set it to 1 (minimum)
                            //if (newhhDensity < 1) newhhDensity = 1; // not neccessary, redundant - commented out during documentation
                        }

                        hhDensity = newhhDensity;
                        supplyTaz = hhDensity * vacantParcelsInTaz;

                        // now allocate as supply >= changeInTaz
                        int forecast = 0;
                        int remDelta = 0;

                        AllocateIncrement(out forecast, out remDelta, hhBase[parcelid - 1], newhhDensity, remChange[taz - 1]);

                        remChange[taz - 1] = remDelta;
                        hhForecast[parcelid - 1] = forecast;

                    }

                }

                else hhForecast[parcelid - 1] = hhBase[parcelid - 1]; // no change

                hhForecastTaz[taz - 1] += hhForecast[parcelid - 1];
                prevTaz = taz;

            }

            for (int i = 0; i < 3000; i++)
            {
                sw.Write(Environment.NewLine + (i + 1) + "," + hhForecastTaz[i]);
            }
            sw.Dispose();

        }

        private static void AllocateIncrement(out int hhParcelForecast, out int remDeltaInTaz, int hhParcelBase, int density, int deltaInTaz)
        {
            hhParcelForecast = 0;
            remDeltaInTaz = 0;

            int increment = density;
            int remaining = deltaInTaz - increment;

            if (remaining < 0)
            {
                increment = deltaInTaz;
                remDeltaInTaz = 0;
            }
            else remDeltaInTaz = remaining;

            // more parcels to allocate in Taz than the increase in housing units
            // the way correspondence file is sorted, vacant parcels in DRi are allocated first and then the ones outside

            hhParcelForecast = hhParcelBase + increment; 

        }
    }
}
