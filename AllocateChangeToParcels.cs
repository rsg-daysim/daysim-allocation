using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DisaggregationTool
{
    class AllocateChangeToParcels
    {
        public static void Allocate(out double[] gqForecast,  out double[,] lUseForecast, out double[,] k12EnrollForecast, out double[] hiEducForecast, 
            double[] gqBase, double[,] lUseBase, 
            int[,] k12EnrollBase, int[] hiEducBase, int[] k12EnrollTazForecast, int[] hiEducTazForecast, double[] diffGQParcel, double[,] diffLUseParcel, 
            double[,] diffK12EnrollParcel, double[] diffHiEducParcel, int numLUseVars, double[] shareK12EnrollInTaz, double[] shareHiEducInTaz,
            int[] tazId)
        {
            int numParcel = lUseBase.GetLength(0);
            gqForecast = new double[numParcel];
            lUseForecast = new double[numParcel, numLUseVars];
            k12EnrollForecast = new double[numParcel, 2];
            hiEducForecast = new double[numParcel];

            double[] gqTaz = new double[3000];
            double[,] k12EnrollTaz = new double[3000, 2];
            double[] hiEducTaz = new double[3000];

            string outputFileName = "C:\\Projects\\FDOT Allocation Tool\\inputs\\Input Data\\ForecastTaz.csv";
            StreamWriter sw = new StreamWriter(File.Create(outputFileName));
            string header = "TAZ,GQ,stugrd,stuhgh,stuuni";
            sw.Write(header);

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

                k12EnrollForecast[i, 0] = k12EnrollBase[i, 0] + diffK12EnrollParcel[i, 0];
                k12EnrollForecast[i, 1] = k12EnrollBase[i, 1] + diffK12EnrollParcel[i, 1];

                hiEducForecast[i] = hiEducBase[i] + diffHiEducParcel[i];

                gqTaz[taz - 1] += gqForecast[i];
                k12EnrollTaz[taz - 1, 0] += k12EnrollForecast[i, 0];
                k12EnrollTaz[taz - 1, 1] += k12EnrollForecast[i, 1];
                hiEducTaz[taz - 1] += hiEducForecast[i];
                
            }

            for (int i = 0; i < 3000; i++)
            {
                sw.Write(Environment.NewLine + (i + 1) + "," + gqTaz[i] + "," + k12EnrollTaz[i, 0] + "," + k12EnrollTaz[i, 1] + "," + hiEducTaz[i]);
            }
            sw.Dispose();

        }

        public static void AllocateHH(out int[] hhForecast, int[] diffHHTaz,int[] hhBase, int[] duDensity, int[] numVacantParcelsInTaz, 
            int[] numVacantParcelsInDri, int[] numParcelsInTaz, long[,] parcelDriCorres, int[] tazId, double[] shareHHInTaz)
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

            string outputFileName = "C:\\Projects\\FDOT Allocation Tool\\inputs\\Input Data\\HHForecastTaz.csv";
            StreamWriter sw = new StreamWriter(File.Create(outputFileName));
            string header = "TAZ,HH";
            sw.Write(header);

            // IMPORTANT:
            // ParcelTaz Correspondence file: parcel id, taz id, driid, driflag, vacant flag 
            // Also the input is already sorted by: tazid, vacant, driflag

            for (int i = 0; i < numParcel; i++)
            {
                long parcelid = parcelDriCorres[i,0];  // need to change the way it is stored while reading, it should be stored by index not by parcel id
                long taz = parcelDriCorres[i,1];

                int changeInTaz = remChange[taz - 1];

                int vacantParcelsInTaz = numVacantParcelsInTaz[taz - 1];
                int vacantParcelsInDri = numVacantParcelsInDri[taz - 1];

                if (taz != prevTaz)
                {
                    hhDensity = duDensity[taz - 1];
                    supplyTaz = hhDensity * vacantParcelsInTaz;
                    numParcels = numParcelsInTaz[taz - 1];
                }

                //int supplyDri = hhDensity * vacantParcelsInDri;

                if (taz==1763)
                {
                    int test=9999;
                    if (parcelid == 496130)
                    {
                        int test1 = 9999;
                    }
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
                            if (newhhDensity < 1) newhhDensity = 1;
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
