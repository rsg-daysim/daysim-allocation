using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DisaggregationTool
{
    class XmlFile
    {
        public static void Read(out Dictionary<string, string> infileDictionary, out bool[] headers, out int numEmplCats, out string[] emplCatsName, 
            out int[,] naicsCodes, out int[,] sicCodes, out int outNumEmplCats, out string[] outEmplCats, out int[,] outEmplCatsNaics,
            string filename)
        {
            infileDictionary = new Dictionary<string, string>();
            headers = new bool[4];

            //int numEmplCats = 0;
            //string[] emplCatsName;
            //int[,] naicsCodes;
            //int[,] sicCodes;
            //int outNumEmplCats;
            //string[] outEmplCats;
            //int[,] outEmplCatsNaics;

            XmlDocument xDocReader = new XmlDocument();
            xDocReader.Load(filename);

            infileDictionary["TAZ File"] = xDocReader.SelectSingleNode("DisaggregationTool/Inputs/TAZ/InputFile").InnerText;
            infileDictionary["Block File"] = xDocReader.SelectSingleNode("DisaggregationTool/Inputs/BLOCK/InputFile").InnerText;
            infileDictionary["MZ File"] = xDocReader.SelectSingleNode("DisaggregationTool/Inputs/MZ/InputFile").InnerText;
            infileDictionary["School File"] = xDocReader.SelectSingleNode("DisaggregationTool/Inputs/SCHOOL/InputFile").InnerText;
            infileDictionary["Output File"] = xDocReader.SelectSingleNode("DisaggregationTool/Output/OutputFile").InnerText;

            headers[0] = Convert.ToBoolean(xDocReader.SelectSingleNode("DisaggregationTool/Inputs/TAZ/Header").InnerText);
            headers[1] = Convert.ToBoolean(xDocReader.SelectSingleNode("DisaggregationTool/Inputs/BLOCK/Header").InnerText);
            headers[2] = Convert.ToBoolean(xDocReader.SelectSingleNode("DisaggregationTool/Inputs/MZ/Header").InnerText);
            headers[3] = Convert.ToBoolean(xDocReader.SelectSingleNode("DisaggregationTool/Inputs/SCHOOL/Header").InnerText);

            //TazFileName = infileDictionary["TAZ File"];
            //BlockFileName = infileDictionary["Block File"];
            //MzFileName = infileDictionary["MZ File"];
            //SchoolFileName = txtSchoolFile.Text;
            //OutputFileName = txtOutputFile.Text;

            // Display TAZ fields in the list box
            // DisplayTazFields(TazFileName); - move to Inputs.cs

            // Check employment Categories
            XmlNode emplNodes = xDocReader.SelectSingleNode("DisaggregationTool/Inputs/TAZ/EmplCats");
            numEmplCats = emplNodes.ChildNodes.Count;

            emplCatsName = new string[numEmplCats];

            // first get and populate the fields
            for (int cat = 0; cat < numEmplCats; cat++)
            {
                emplCatsName[cat] = emplNodes.ChildNodes[cat].LocalName;
                int fieldIndex = Convert.ToInt16(emplNodes.ChildNodes[cat].InnerText);

                //clbEmplCats.SetItemChecked(fieldIndex, true); Move to inputs.cs

            }

            // CatsSelected = true; move to inputs.cs

            // NAICS Codes
            XmlNode naicsNode = emplNodes.NextSibling; // go to <NAICS_Code>
            XmlNode firstEmplCat = naicsNode.FirstChild; // Access first element

            naicsCodes = new int[numEmplCats, 20];
            for (int cat = 0; cat < numEmplCats; cat++)
            {
                for (int code = 0; code < 20; code++)
                {

                    naicsCodes[cat, code] = Convert.ToInt16(firstEmplCat.ChildNodes[code].InnerText); // 1 = checked; 0 - not checked

                }

                // go to next employment category
                firstEmplCat = firstEmplCat.NextSibling;
            }
            // SIC Codes
            sicCodes = new int[numEmplCats, 10];

            // DaySim Employment Categories
            XmlNode daySimNodes = xDocReader.SelectSingleNode("DisaggregationTool/Output/EmplCats");

            outNumEmplCats = daySimNodes.ChildNodes.Count;

            outEmplCats = new string[outNumEmplCats];
            outEmplCatsNaics = new int[outNumEmplCats, 20];

            // Read Categories
            for (int cat = 0; cat < outNumEmplCats; cat++)
            {
                outEmplCats[cat] = daySimNodes.ChildNodes[cat].InnerText;
            }

            // Read NAICS Codes
            XmlNode daySimNaicsNode = daySimNodes.NextSibling; // go to <NAICS_Code>
            XmlNode firstDaySimCat = daySimNaicsNode.FirstChild; // Access first element

            for (int cat = 0; cat < outNumEmplCats; cat++)
            {
                for (int code = 0; code < 20; code++)
                {
                    outEmplCatsNaics[cat, code] = Convert.ToInt16(firstDaySimCat.ChildNodes[code].InnerText); // 1 = checked; 0 - not checked
                }

                // go to next employment category
                firstDaySimCat = firstDaySimCat.NextSibling;
            }

        }
    }
}
