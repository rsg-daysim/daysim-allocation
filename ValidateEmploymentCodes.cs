using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DisaggregationTool
{
    class ValidateEmploymentCodes
    {
        public static string Validate(int numEmplCategories, int[,] emplCodes, string emplClassType)
        {
            int numCodes = 0;
            int[] codesStatus;
            string error = "";
            bool catChecked;

            if (emplClassType == Constants.Naics) numCodes = 20;
            else if (emplClassType == Constants.Sic) numCodes = 10;

            codesStatus = new int[numCodes];

            for (int cat = 0; cat < numEmplCategories; cat++)
            {
                catChecked = false;

                for (int code = 0; code < numCodes; code++)
                {
                    codesStatus[code] = codesStatus[code] + emplCodes[cat, code];

                    if (emplCodes[cat, code] == 1) catChecked = true;
                }
                // a category should have at-least one naics code
                if (!catChecked)
                {
                    error = "ERROR: An employment category does not have a NAICS code selected.";
                    return error;
                }
            }

            // value of all indexes should be 1
            for (int code = 0; code < numCodes; code++)
            {
                if (codesStatus[code] == 0)
                {
                    error = "ERROR: All NAICS codes should be checked!";
                    return error;
                }
                else if (codesStatus[code] > 1)
                {
                    error = "ERROR: A NAICS code is checked in multiple categories!";
                    return error;
                }

            }
            return error;
        }
    }
}
