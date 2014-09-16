using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DisaggregationTool
{
    class WriteToOutput
    {
        public static void Write(string outputFileName, int outNumEmplCats, string[] outEmplCats, int numMZ, int[] mzId_M, double[] xCoord_M, double[] yCoord_M,
            double[] area_M, long[] tazId_M, long[] blockId_M, double[,] empl_M, double[] hh_M, int[,] schEnrl_M, int numSchVars)
        {
            // Default DaySim Categories:
            // Industrial: 22, 31-33, 42, 48-49
            // Retial Trade: 44-45
            // Office: 51-56
            // Educational Services: 61
            // Health/ Medical: 62
            // Government: 92
            // Food: 72
            // Services: 71, 81
            // Other: 11, 21, 23

            StreamWriter sw = new StreamWriter(File.Create(outputFileName));

            string header = "microzoneid,xcoord_p,ycoord_p,sqft_p,taz_p,block_p,hh_p,stugrd_p,stuhgh_p,stuuni_p,empedu_p,empfoo_p,empgov_p,empind_p,empmed_p,"
                + "empofc_p,empret_p,empsvc_p,empoth_p,emptot_p,parkdy_p,parkhr_p,ppricdyp,pprichrp";

            //// Add employment category names
            //for (int cat = 0; cat < outNumEmplCats; cat++)
            //{
            //    header = header + "," + outEmplCats[cat];
            //}

            // Add school enrollment types
            //header = header + ",TOTEMPL,STUGRD,STUHGH,STUUNIV"; 

            // write header
            sw.Write(header);

            for (int mz = 0; mz < numMZ; mz++)
            {
                // Write basic MZ information
                sw.Write(Environment.NewLine + mzId_M[mz] + "," + xCoord_M[mz] + "," + yCoord_M[mz] + "," + area_M[mz] + "," + tazId_M[mz] + "," 
                    + blockId_M[mz]);

                // Write HH data
                sw.Write("," + hh_M[mz].ToString("0.##"));

                // write school data
                for (int enrl = 0; enrl < numSchVars; enrl++)
                {
                    sw.Write("," + schEnrl_M[mz, enrl]);
                }
                
                int mid = mzId_M[mz];
                long tid = tazId_M[mz];
                long bid = blockId_M[mz];

                // write employment data
                //for (int cat = 0; cat < outNumEmplCats + 1; cat++)
                //{
                //    //sw.Write("," + empl_M[mz, cat]);
                //    sw.Write("," + empl_M[mz, cat].ToString("0.##"));
                //}

                // write employment data in the required format
                sw.Write("," + empl_M[mz, 3].ToString("0.##") + "," + empl_M[mz, 6].ToString("0.##") + "," + empl_M[mz, 5].ToString("0.##")
                     + "," + empl_M[mz, 0].ToString("0.##") + "," + empl_M[mz, 4].ToString("0.##") + "," + empl_M[mz, 2].ToString("0.##")
                      + "," + empl_M[mz, 1].ToString("0.##") + "," + empl_M[mz, 7].ToString("0.##") + "," + empl_M[mz, 8].ToString("0.##")
                       + "," + empl_M[mz, 9].ToString("0.##"));

                // write 0 for next 4 columns - for parking columns
                double zeroValue = 0;
                for (int cat = 0; cat < 4; cat++)
                {
                    sw.Write("," + zeroValue.ToString("0.##"));
                }

            }

            sw.Dispose();

        }

    }
}
