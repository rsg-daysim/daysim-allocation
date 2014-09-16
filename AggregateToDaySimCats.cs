using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DisaggregationTool
{
    class AggregateToDaySimCats
    {
        public static void Calculate(out double[,] empl_M, out double[] hh_M, int numMZ, int outNumEmplCats, int[,] outEmplCatsNaics, double[,] lUse_M)
        {
            empl_M = new double[numMZ, outNumEmplCats + 1]; // employment categories, and total employment
            hh_M = new double[numMZ]; // households

            // Aggregate employment into DaySim categories
            for (int mz = 0; mz < numMZ; mz++)
            {
                double totalEmployment = 0;
                for (int cat = 0; cat < outNumEmplCats; cat++)
                {
                    hh_M[mz] = lUse_M[mz, 0];

                    for (int j = 0; j < 20; j++)
                    {
                        double employment = outEmplCatsNaics[cat, j] * lUse_M[mz, j + 1];
                        empl_M[mz, cat] = (empl_M[mz, cat] + employment);
                        totalEmployment += employment;
                    }

                }

                empl_M[mz, outNumEmplCats] = totalEmployment;

            }

        }

    }
}
