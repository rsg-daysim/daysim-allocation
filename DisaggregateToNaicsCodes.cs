using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DisaggregationTool
{
    class DisaggregateToNaicsCodes
    {
        public static double[,] Calculate(int numTaz, long[] tazId_T, double[,] lUse_T, int numEmplCats,
            int[,] naicsCodes, double[,] lUseSum_T, int numLUseVars_B, int numLUseVars_T, string outfolder)
        {
            //out double[,] lUseCode_T
            double[,] lUseCode_T = new double[numTaz, numLUseVars_B];

            // loop 3 - to verify calculations
            string test_naics = Path.Combine(outfolder,"test_naics.csv");
            StreamWriter sw1 = new StreamWriter(File.Create(test_naics));
            sw1.Write("TAZID,HH,CNS01,CNS02,CNS03,CNS04,CNS05,CNS06,CNS07,CNS08,CNS09,CNS10,CNS11,CNS12,CNS13,CNS14,CNS15,CNS16,CNS17,CNS18,CNS19,CNS20,C000");

            // 3. loop tazs - disaggregate categories into naics codes
            for (int taz_ind = 0; taz_ind < numTaz; taz_ind++)
            {
                sw1.Write(Environment.NewLine + tazId_T[taz_ind]);

                // NAICS code calculations
                lUseCode_T[taz_ind, 0] = lUse_T[taz_ind, 0]; //HHs column

                // Calculations are TAZ level
                for (int cat = 0; cat < numEmplCats; cat++)
                {
                    double catEmpl = lUse_T[taz_ind, 1 + cat]; //first index (0) is HH, so start with index = 1
                    double empl_sum = 0;
                    int nCodes = 0;

                    for (int code = 0; code < 20; code++)
                    {
                        if (naicsCodes[cat, code] == 1)
                        {
                            // set equal to the block data for the associated naics codes
                            lUseCode_T[taz_ind, code + 1] = (naicsCodes[cat, code] * lUseSum_T[taz_ind, code + 1]);

                            // total employment for the category
                            empl_sum = empl_sum + lUseCode_T[taz_ind, code + 1];
                            nCodes = nCodes + 1;
                        }

                    }

                    // factor-in empl data for the category
                    for (int code = 0; code < 20; code++)
                    {
                        if (naicsCodes[cat, code] == 1)
                        {
                            if (empl_sum > 0) lUseCode_T[taz_ind, code + 1] = lUseCode_T[taz_ind, code + 1] * (catEmpl / empl_sum); // factor in based on the share

                            else if (catEmpl > 0) // if block data for NAICS codes is zero but TAZ data is nonzero
                            {
                                // divide equally
                                lUseCode_T[taz_ind, code + 1] = catEmpl / nCodes;
                            }
                        }

                    }

                    // as the categories have different NAICS codes, values for all codes will be calculated by the end of the loop
                }

                lUseCode_T[taz_ind, numLUseVars_B - 1] = lUse_T[taz_ind, numLUseVars_T - 1]; //Total Empl Column

                // write to a text file
                for (int i = 0; i < numLUseVars_B; i++)
                {
                    sw1.Write("," + Convert.ToInt32(lUseCode_T[taz_ind, i]));
                }
            }

            sw1.Dispose();

            return lUseCode_T;
        }

    }
}
