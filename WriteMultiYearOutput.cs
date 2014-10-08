using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DisaggregationTool
{
    class WriteMultiYearOutput
    {
        public static void Write(int[] hh_P, double[] gq_P, double[,] lUse_P, double[,] k12Enroll_P, double[] hiEduc_P, long[] parcelId, double[] xCoord, double[] yCoord, 
            double[] area, int[] tazId, string outputFileName, int[,] driCorrespondence)
        {

            //int numSchVars = 3;
            int numParcel = lUse_P.GetLength(0);
            double zeroValue = 0;

            StreamWriter sw = new StreamWriter(File.Create(outputFileName));

            string header = "parcelid,xcoord_p,ycoord_p,sqft_p,taz_p,lutype_p,hh_p,stugrd_p,stuhgh_p,stuuni_p,empedu_p,empfoo_p,empgov_p,empind_p,empmed_p,"
                + "empofc_p,empret_p,empsvc_p,empoth_p,emptot_p,parkdy_p,parkhr_p,ppricdyp,pprichrp, GQ, driid, driflag";

            // write header
            sw.Write(header);

            for (int p = 0; p < numParcel; p++)
            {
                // Write basic MZ information
                sw.Write(Environment.NewLine + parcelId[p] + "," + xCoord[p] + "," + yCoord[p] + "," + area[p] + "," + tazId[p] + ","
                    + zeroValue);

                // Write HH data
                sw.Write("," + hh_P[p].ToString());
                
                // write K12 school data
                for (int enrl = 0; enrl <2; enrl++)
                {
                    sw.Write("," + Math.Round(k12Enroll_P[p, enrl]).ToString());
                }

                // write university enrollment
                sw.Write("," +  Math.Round(hiEduc_P[p]).ToString());

                // write employment data in the required format
                for (int sector = 0; sector < 10; sector++)
                {
                    sw.Write("," +  Math.Round(lUse_P[p,sector]).ToString());
                }

                // write 0 for next 4 columns - for parking columns
                
                for (int cat = 0; cat < 4; cat++)
                {
                    sw.Write("," + zeroValue.ToString("0.##"));
                }

                // GQ pop
                sw.Write("," + gq_P[p].ToString("0.##"));

                //write dri flag
                sw.Write("," + driCorrespondence[p,1].ToString());
                sw.Write("," + driCorrespondence[p,2].ToString());

            }

            sw.Dispose();

        }

    }
}
