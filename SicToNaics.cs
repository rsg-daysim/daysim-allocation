using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DisaggregationTool
{
    class SicToNaics
    {
        private static int[,] sicToNaicsConversion;

        public static int[,] Convert(int[,] sicCodes, int numEmplCats)
        {
            int[,] naicsCodes = new int[numEmplCats, 20];
            sicToNaicsConversion = new int[10, 20];

            // get SIC to NAICS relation
            SicToNaicsRelation();

            // convert SIC codes to NAICS codes
            for (int cat = 0; cat < numEmplCats; cat++)
            {
                for (int sic = 0; sic < 10; sic++)
                {
                    if (sicCodes[cat, sic] == 1)
                    {
                        for (int i = 0; i < 20; i++) naicsCodes[cat, i] = sicToNaicsConversion[sic, i];
                    }
                }

            }

            return naicsCodes;

        }

        private static void SicToNaicsRelation()
        {
            // move to a different class

            for (int sic = 0; sic < 10; sic++)
            {
                for (int naics = 0; naics < 20; naics++)
                {
                    if (sic == 1 && naics == 0) sicToNaicsConversion[sic, naics] = 1;
                    if (sic == 2 && naics == 1) sicToNaicsConversion[sic, naics] = 1;
                    if (sic == 3 && naics == 2) sicToNaicsConversion[sic, naics] = 1;
                    if (sic == 4 && naics == 3) sicToNaicsConversion[sic, naics] = 1;
                    if (sic == 5 && naics >= 4 && naics < 6) sicToNaicsConversion[sic, naics] = 1;
                    if (sic == 6 && naics == 6) sicToNaicsConversion[sic, naics] = 1;
                    if (sic == 7 && naics >= 7 && naics < 9) sicToNaicsConversion[sic, naics] = 1;
                    if (sic == 8 && naics >= 9 && naics < 11) sicToNaicsConversion[sic, naics] = 1;
                    if (sic == 9 && naics >= 11 && naics < 18) sicToNaicsConversion[sic, naics] = 1;
                    if (sic == 10 && naics >= 18 && naics < 19) sicToNaicsConversion[sic, naics] = 1;

                }
            }


        }


    }
}
