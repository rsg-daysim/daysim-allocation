using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DisaggregationTool
{
    class SortParcels
    {
        public static void Sort(out long[] parcelsSorted, out string error, long[] parcelsByTaz, int[] parcelToTaz, double[] area, string method, string outFolder)
        {
            long numParcels = parcelsByTaz.GetLength(0);
            parcelsSorted = new long[numParcels];

            // initialize sorted parcels with the initial parcels
            Array.Copy(parcelsByTaz, parcelsSorted,numParcels);

            string outputFileName = outFolder + "\\ParcelsSortedByArea.csv";
            StreamWriter sw = new StreamWriter(File.Create(outputFileName));
            string header = "index,tazid,parcelid,area";
            sw.Write(header);

            int prev_tazid = 0;
            int startindex=0;
            error = "";

            if (method == "A") // "A" - Ascending
            {
                for (int j = 1; j < numParcels; j++)
                {
                    long parcelid = parcelsSorted[j];
                    int tazid = parcelToTaz[parcelid-1];
                    double parcelarea = area[parcelid-1];

                    // set a new starting position for new taz
                    if (tazid != prev_tazid) startindex = j - 1;

                    int i = j - 1;

                    while (i >= startindex && area[parcelsSorted[i]-1] > parcelarea)
                    {
                        parcelsSorted[i + 1] = parcelsSorted[i];
                        i = i - 1;
                    }

                    parcelsSorted[i + 1] = parcelid;
                    prev_tazid = tazid;
                }
                
            }

            else if (method == "D") // "D" - Descending
            {
                for (int j = 1; j < numParcels; j++)
                {
                    long parcelid = parcelsSorted[j];
                    int tazid = parcelToTaz[parcelid - 1];
                    double parcelarea = area[parcelid - 1];

                    // set a new starting position for new taz
                    if (tazid != prev_tazid) startindex = j - 1;

                    int i = j - 1;

                    while (i >= startindex && area[parcelsSorted[i] - 1] < parcelarea)
                    {
                        parcelsSorted[i + 1] = parcelsSorted[i];
                        i = i - 1;
                    }

                    parcelsSorted[i + 1] = parcelid;
                    prev_tazid = tazid;
                }

            }

            else
            {
                // Not a valid method. The allowed methods are "A" or "D"
                error = "Not a valid method. The allowed methods are 'A' and 'D'";
            }

            for (int p = 0; p < numParcels; p++)
            {
                long parcelid = parcelsSorted[p];
                int tazid = parcelToTaz[parcelid - 1];
                double parcelarea = area[parcelid - 1];
                sw.Write(Environment.NewLine + p + "," + tazid + "," + parcelid + "," + parcelarea);

            }

            sw.Dispose();

        }

    }
}
