using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace DisaggregationTool
{
    public partial class FrmInput : Form
    {
        // Inputs from userform
        private string _tazFileName;
        private string _blockFileName;
        private string _mzFileName;
        private string _schoolFileName;
        private string _outputFileName;
        private bool _catsSelected;
        private bool _codesSelected;
        private static string _emplClassType;
        private static int[,] _sicToNaicsConversion;

        // ------------------------------- FDOT Allocation Tool -----------------------

        // FDOT allocation tool
        private static string _analysisType;
        private string _tazForecastFileName;
        private string _tazForecastFileName1;
        private string _parcelBaseFileName;
        private string _parcelForecastFileName;
        private string _driCorrespondenceFileName;
        private string _sectorCorrespondenceFileName;
        //private static Dictionary<int, int> _tazIndBaseDictionary;
        private static Dictionary<int, int> _tazIndForecastDictionary;
        private static int _numParcel;
        const int NumDaySimSector = 9;

        private static Dictionary<int, int> _sectorCorrespondenceDictionary;

        //Base year TAZ-level
        //private static double[,] _luse_t_base; // hh, sector wise empl, totempl
        //private static double[] _area_t_base;
        //private static int[] _tazid_t_base;
        //private static int _numLUseVars_t_base;

        //Forecast year TAZ-level
        private static double[,] _luse_t_forecast; // sector wise empl, totempl
        private static int[] _k12Enroll_t_forecast;
        private static int[] _hiEduc_t_forecast;
        private static double[] _area_t_forecast;
        private static int[] _tazid_t_forecast;
        private static int _numLUseVars_t_forecast;
        int[,] _dwellUnits_t_forecast;
        double[] _gqPop_t_forecast;

        //Base year parcel level
        private static double[,] _luse_p_base;  // hh, sector wise empl, totempl
        private static double[] _area_p_base;
        private static long[] _parcelid_p_base;
        private static int[] _tazid_p_base;
        private static int _numLUseVars_p_base;
        private static double[] _xcoord_p_base;
        private static double[] _ycoord_p_base;
        private static int[] _hh_p_base;
        private static double[,] _enroll_p_base;
        private static int[,] _k12Enroll_p_base;
        private static int[] _hieduc_p_base;


        //Forecast year parcel level - output
        private static double[,] _luse_p_forecast; // hh, sector wise empl, totempl
        private static double[] _area_p_forecast;
        private static long[] _parcelid_p_forecast;
        private static int _numLUseVars_p_forecast;
        //private static int[,] _k12Enroll_p_forecast;
        //private static int[] _hieduc_p_forecast;


        // Parcel-DRI-TAZ correspondence
        private static int[,] _parcelDriCorrespondence;
        private static long[,] _parcelVacantCorrespondence;
        private static double[] _gqPop_p_base;

        private static Dictionary<long, int> _tazParcelDictionary;
        private static Dictionary<long, int> _parcelDriDictionary;

        // ---------------------------------------------------------------------------

        // TAZ Inputs
        private static int _numTazFields;
        private static int _numEmplCats;
        private static string[] _emplCats;
        private static int _numTaz;
        private static long[] _tazid_t;
        private static Dictionary<long, int> _tazIndDictionary;
        private static double[] _xcoord_t;
        private static double[] _ycoord_t;
        private static double[] _area_t;
        private static double[,] _luse_t;
        private static double[,] _lusecode_t;
        private static int _numLUseVars_T;
        private static int[,] _naicsCodes;
        private static int[,] _sicCodes;

        // BLOCK Inputs
        const int NumLUseVars_B = 22; // HH, 20 NAICS, and total empl
        const int TotEmplIndex_B = 21;
        const int HHIndex_B = 0;
        private static int _numBlocks;
        private static long[] _blockId_b;
        private static Dictionary<long, int> _blockIndDictionary;
        private static double[] _xcoord_b;
        private static double[] _ycoord_b;
        private static double[] _area_b;
        private static double[,] _luse_b;

        // MZ Inputs
        private static int _numMZ;
        private static int[] _mzid_m;
        private static Dictionary<long, int> _mzIndDictionary;
        private static double[] _xcoord_m;
        private static double[] _ycoord_m;
        private static double[] _area_m;
        private static long[] _blockid_m;
        private static long[] _tazid_m;

        // School Inputs
        const int NumSchVars = 3; //k-8, 9-12, and University
        private static int[] _schid;
        private static double[] _xcoord_s;
        private static double[] _ycoord_s;
        private static int[] _mzid_s;
        private static int[,] _schenrl_s;
        private static int _numSchools; //totalSchools

        // Inputs for Output
        FrmOutput Input;
        FrmNaics NaicsForm;
        FrmSic SicForm;
        private static int _outNumEmplCats;
        private static string[] _outEmplCats;
        private static int[,] _outEmplCatsNaics;
        private static bool _readFromXml;

        public FrmInput()
        {
            InitializeComponent();

            cbTazHeader.Enabled = false;
            CatsSelected = false;
            CodesSelected = false;

            NaicsForm = FrmNaics.GetInstance();
            SicForm = FrmSic.GetInstance();
            Input = FrmOutput.GetInstance();

            // TAZ file: *.txt files (comma seperated)
            // Block file: *.dat file (w/o header) (space seperated)
            // MZ file: *.txt file (comma seperated)
            // school file: *.txt file (comma seperated)
            // what about ASCII format
        }

        private void cmdRun_Click(object sender, EventArgs e)
        {
            try
            {
                GetAnalysisType();

                if (AnalysisType == Constants.OneYear)
                {
                    // Validate input files
                    if (!ValidateInputs()) return;

                    // Read input data
                    ReadTazFile(TazFileName);

                    //SaveXmlFile();

                    // Get input Naics Codes
                    if (!ReadFromXml) GetNaicsCodes(); // if not read from xml - find a better way to identify

                    // check if NAICS codes are selected correctly - for the case when the user might dispose the form
                    string inputError = ValidateEmploymentCodes.Validate(NumEmplCats, NaicsCodes, EmplClassType);

                    if (inputError.Length != 0)
                    {
                        MessageBox.Show(inputError);
                        return;
                    }

                    // Get DaySim employment categories
                    if (OutNumEmplCats == 0) GetInputsForOutput(); // if not read from xml

                    SaveXmlFile();

                    ReadBlockFile(BlockFileName);
                    ReadMZFile(MzFileName);
                    ReadSchoolFile(SchoolFileName);


                    // check if NAICS codes are selected correctly - for the case when the user might dispose the form
                    string outputError = ValidateEmploymentCodes.Validate(OutNumEmplCats, OutEmplCatsNaics, "NAICS");

                    if (outputError.Length != 0)
                    {
                        MessageBox.Show(outputError);
                        return;
                    }

                    //SaveXmlFile();

                    // Perform calculations
                    PerformOneYearCalculations();
                }
                else
                {
                    //disable other inputs
                    btnBrowseBlocks.Enabled = false;
                    btnBrowseMZ.Enabled = false;
                    btnBrowseTaz.Enabled = false;
                    btnBrowseSchool.Enabled = false;
                    btnBrowseOutput.Enabled = false;

                    //TAZ allocation tool
                    ReadSectorCorrespondenceFile(SectorCorrespondenceFileName);
                    ReadForecastTazFile(TazForecastFileName);
                    ReadForecastTazHHFile(TazForecastFileName1);
                    ReadBaseParcelFile(ParcelBaseFileName);
                    ReadDriCorrespondenceFile(DriCorrespondenceFileName);

                    // Do the calculations
                    PerformMultiYearCalculations();

                }

                MessageBox.Show("Finished!");

                Application.Exit();
                
            }

            catch (System.IO.IOException error)
            {
                MessageBox.Show("Error: " + error.Message);
                MessageBox.Show("Unsuccessful!");
                return;
            }

        }

        private bool ValidateInputs()
        {
            if (txtTazFile.Text.Length != 0 && txtBlockFile.Text.Length != 0 && txtMzFile.Text.Length != 0 &&
                txtSchoolFile.Text.Length != 0 && txtOutputFile.Text.Length != 0) return true;

            else
            {
                MessageBox.Show("Inputs/ outputs are missing");
                return false;
            }

        }

        private void ReadSectorCorrespondenceFile(string fileName)
        {
            FileInfo fileFormat = new FileInfo(fileName);
            string extension = fileFormat.Extension;

            SectorCorrespondenceDictionary = new Dictionary<int, int>();

            int tazSectorIndex = 0;
            int daySimSectorIndex = 0;

            // use streamreader to read the file
            StreamReader file = new StreamReader(fileName);

            // read line by line
            string line = file.ReadLine();

            int i = 0;
            while (line != null)
            {
                string[] split = System.Text.RegularExpressions.Regex.Split(line, "\t");

                int j = 0;

                foreach (string column in split)
                {
                    if (column == "SECTOR")
                    {
                        break;
                    }
                    else
                    {
                        if (j == 1) tazSectorIndex = Convert.ToInt16(column);
                        else if (j == 2) daySimSectorIndex = Convert.ToInt16(column);

                        j++;
                    }

                }

                if (i>0) SectorCorrespondenceDictionary[tazSectorIndex] = daySimSectorIndex;

                line = file.ReadLine();
                i++;

            }

            file.Close();
            return;

        }

        private void ReadForecastTazFile(string fileName)
        {
            NumTaz = File.ReadAllLines(fileName).Length - 1;
            NumLUseVars_T_Forecast = NumDaySimSector + 1; // sectors, totempl
            Area_T_Forecast = new double[NumTaz];
            LUse_T_Forecast = new double[NumTaz, NumLUseVars_T_Forecast];
            K12Enroll_T_Forecast = new int[NumTaz];
            HiEduc_T_Forecast = new int[NumTaz];
            TazId_T_Forecast = new int[NumTaz];
            TazIndForecastDictionary = new Dictionary<int, int>();
            int landUseStartIndex = 4;
            int taz = 0;
            
            FileInfo fileFormat = new FileInfo(fileName);
            string extension = fileFormat.Extension;

            // use streamreader to read the file
            StreamReader file = new StreamReader(fileName);

            // read line by line
            string line = file.ReadLine();
            int i = 0;

            while (line != null)
            {
                //string[] split = System.Text.RegularExpressions.Regex.Split(line, "\\s+");
                string[] split = System.Text.RegularExpressions.Regex.Split(line, ",");


                int j = 0;

                foreach (string column in split)
                {
                    if (i == 0)
                    {
                        break;
                    }
                    else
                    {
                        // make index
                        if (j == 3)
                        {

                            if (column.Contains('.')) taz = (int)Convert.ToDouble(column);
                            else taz = Convert.ToInt16(column);

                            TazId_T_Forecast[i - 1] = taz;
                            TazIndForecastDictionary[i - 1] = TazId_T_Forecast[i - 1]; // key = index; value = taz
                        }
                        else if (j >= landUseStartIndex && j <= landUseStartIndex+9)
                        {
                            int sectorIndex = SectorCorrespondenceDictionary[j - landUseStartIndex];
                            LUse_T_Forecast[taz - 1, sectorIndex] = Convert.ToDouble(column);
                        }

                        if (j == 14) K12Enroll_T_Forecast[taz - 1] = Convert.ToInt16(Convert.ToDouble(column));
                        if (j == 15) HiEduc_T_Forecast[taz - 1] = Convert.ToInt16(Convert.ToDouble(column));

                        j++;
                    }

                }

                line = file.ReadLine();
                i++;

            }

            file.Close();
            return;

        }

        private void ReadForecastTazHHFile(string fileName)
        {
            NumTaz = File.ReadAllLines(fileName).Length - 1;
            DwellUnits_T_Forecast = new int[NumTaz, 3];
            GQPop_T_Forecast = new double[NumTaz];

            int TotalDU = 0;
            double pctvnp = 0; // % vacant or seasonal
            double pctvac = 0; // % vacant

            FileInfo fileFormat = new FileInfo(fileName);
            string extension = fileFormat.Extension;

            // use streamreader to read the file
            StreamReader file = new StreamReader(fileName);

            // read line by line
            string line = file.ReadLine();
            int i = 0;

            while (line != null)
            {
                //string[] split = System.Text.RegularExpressions.Regex.Split(line, "\\s+");
                string[] split = System.Text.RegularExpressions.Regex.Split(line, ",");

                int j = 0;

                foreach (string column in split)
                {
                    if (i == 0)
                    {
                        break;
                    }
                    else
                    {
                        // make index - assuming same TAZ index as ZDATA2
                        if (j == 4) TotalDU = Convert.ToInt16(column);
                        if (j == 5) pctvnp = Convert.ToDouble(column);
                        if (j == 6) pctvac = Convert.ToDouble(column);

                        DwellUnits_T_Forecast[i-1, 0] = TotalDU; // total dwelling units
                        DwellUnits_T_Forecast[i-1, 1] = Convert.ToInt16(TotalDU * (100 - pctvnp) * 0.01);  // permanent residents dwelling units
                        DwellUnits_T_Forecast[i-1, 2] = Convert.ToInt16(TotalDU * (pctvnp - pctvac) * 0.01);  // seasonal residents dwelling units

                        if (j == 23) GQPop_T_Forecast[i-1] = Convert.ToDouble(column);

                        j++;
                    }

                }
                line = file.ReadLine();
                i++;
            }

            file.Close();
            return;
        }     

        private void ReadBaseParcelFile(string fileName)
        {
            NumParcel = File.ReadAllLines(fileName).Length - 1;

            NumLUseVars_P_Base = NumDaySimSector + 1; // empedu,empfoo,empgov,empind,empmed,empofc,empret,empsvc,empoth,emptot=10
            Area_P_Base = new double[NumParcel];
            LUse_P_Base = new double[NumParcel, NumLUseVars_P_Base];
            ParcelId_P_Base = new long[NumParcel];
            TazId_P_Base = new int[NumParcel];
            XCoord_P_Base = new double[NumParcel];
            YCoord_P_Base = new double[NumParcel];
            HH_P_Base = new int[NumParcel];
            K12Enroll_P_Base = new int[NumParcel, 2];
            HiEduc_P_Base = new int[NumParcel];

            FileInfo fileFormat = new FileInfo(fileName);
            string extension = fileFormat.Extension;

            // use streamreader to read the file
            StreamReader file = new StreamReader(fileName);

            // read line by line
            string line = file.ReadLine();
            int i = 0;

            while (line != null)
            {
                //string[] split = System.Text.RegularExpressions.Regex.Split(line, "\\s+");
                string[] split = System.Text.RegularExpressions.Regex.Split(line, " ");
                //string[] split = System.Text.RegularExpressions.Regex.Split(line, ",");

                int j = 0;

                foreach (string column in split)
                {
                    if (i == 0)
                    {
                        break;
                    }
                    else
                    {
                        if (j == 0) ParcelId_P_Base[i-1] = Convert.ToInt64(column);
                        if (j == 1) XCoord_P_Base[i - 1] = Convert.ToDouble(column);
                        if (j == 2) YCoord_P_Base[i - 1] = Convert.ToDouble(column);
                        else if (j == 3) Area_P_Base[i - 1] = Convert.ToDouble(column);
                        else if (j == 4) TazId_P_Base[i - 1] = Convert.ToInt32(column);
                        else if (j == 6) HH_P_Base[i - 1] = (int)Convert.ToDouble(column);
                        else if (j >= 7 && j <= 8) K12Enroll_P_Base[i-1,j-7] = Convert.ToInt16(Convert.ToDouble(column));
                        else if (j == 9) HiEduc_P_Base[i - 1] = Convert.ToInt16(Convert.ToDouble(column));
                        else if (j >= 10 && j <= 19) LUse_P_Base[i - 1, j - 10] = Convert.ToDouble(column);

                        j++;
                    }

                }

                line = file.ReadLine();
                i++;

            }

            file.Close();
            return;

        }

        private void ReadDriCorrespondenceFile(string fileName)
        {
            // Stored based on the parcel id

            NumParcel = File.ReadAllLines(fileName).Length - 1;

            FileInfo fileFormat = new FileInfo(fileName);
            string extension = fileFormat.Extension;

            ParcelDriCorrespondence = new int[NumParcel, 4];
            ParcelVacantCorrespondence = new Int64[NumParcel, 5];
            GQPop_P_Base = new double[NumParcel];

            // use streamreader to read the file
            StreamReader file = new StreamReader(fileName);

            // read line by line
            string line = file.ReadLine();
            int i = 0;
            long parcelid = 0;

            while (line != null)
            {
                string[] split = System.Text.RegularExpressions.Regex.Split(line, ",");

                //int j = 0;
                if (i > 0)
                {
                    parcelid = Convert.ToInt32(split[3]); // new_pclid

                    ParcelDriCorrespondence[parcelid - 1, 0] = Convert.ToInt32(split[0]); //taz2010
                    ParcelDriCorrespondence[parcelid - 1, 1] = Convert.ToInt32(split[1]); //dri
                    ParcelDriCorrespondence[parcelid - 1, 2] = Convert.ToInt32(split[2]); //dri_flag
                    ParcelDriCorrespondence[parcelid - 1, 3] = Convert.ToInt32(split[4]); //dor_desc
                    //ParcelDriCorrespondence[parcelid - 1, 4] = Convert.ToInt32(split[6]); //hh_1 - double: don't need to save it, it is used for sorting (done beforehand as of now)

                    // for HH allocation - in sorted order, by: taz (ascending), vacant (desending), dri (descending), hh_1(descending)
                    ParcelVacantCorrespondence[i - 1, 0] = parcelid;
                    ParcelVacantCorrespondence[i - 1, 1] = Convert.ToInt32(split[0]); //taz2010
                    ParcelVacantCorrespondence[i - 1, 2] = Convert.ToInt32(split[1]); //dri
                    ParcelVacantCorrespondence[i - 1, 3] = Convert.ToInt32(split[2]); //dri_flag
                    ParcelVacantCorrespondence[i - 1, 4] = Convert.ToInt32(split[4]); //dor_desc

                    GQPop_P_Base[parcelid - 1] = Convert.ToDouble(split[8]); //gq
                }

                //foreach (string column in split)
                //{
                //    if (i == 0)
                //    {
                //        break;
                //    }
                //    else
                //    {
                //        if (j == 0)
                //        {
                //            parcelid = Convert.ToInt32(column);
                //            ParcelVacantCorrespondence[i - 1, j] = parcelid;
                //        }
                //        else if (j == 8 && column != null) GQPop_P_Base[parcelid - 1] = Convert.ToDouble(column);
                //        else
                //        {
                //            ParcelDriCorrespondence[parcelid - 1, j - 1] = Convert.ToInt32(column); //last index is for vacant parcel flag
                //            ParcelVacantCorrespondence[i - 1, j] = Convert.ToInt32(column);
                //        }

                //        j++;
                //    }

                //}

                line = file.ReadLine();
                i++;

            }

            file.Close();
            return;

        }


        private void ReadTazFile(string fileName)
        {
            int nFields = clbEmplCats.Items.Count;
            NumLUseVars_T = nFields - 4; // taz, xcoord, ycoord, and area
            
            // Total number of tazs
            NumTaz = File.ReadAllLines(fileName).Length - 1;

            TazId_T = new long[NumTaz];
            TazIndDictionary = new Dictionary<long, int>();
            XCoord_T = new double[NumTaz];
            YCoord_T = new double[NumTaz];
            Area_T = new double[NumTaz];
            LUse_T = new double[NumTaz, NumLUseVars_T];

            FileInfo fileFormat = new FileInfo(fileName);
            string extension = fileFormat.Extension;
            
            switch (extension)
            {
                case ".txt":
                    {
                        // use streamreader to read the file
                        StreamReader file = new StreamReader(fileName);

                        // read line by line
                        string line = file.ReadLine();
                        int i = 0;

                        while (line != null)
                        {
                            string[] split = System.Text.RegularExpressions.Regex.Split(line, "\\s+");
                            //string[] split = System.Text.RegularExpressions.Regex.Split(line, ",");

                            int j = 0;

                            foreach (string column in split)
                            {
                                if (column == "TAZ")
                                {
                                    break;
                                }
                                else
                                {
                                    // make index
                                    if (j == 0)
                                    {
                                        //TazId_T[i-1] = (int) Convert.ToDouble(column);
                                        //TazId_T[i - 1] = Convert.ToInt64(column);

                                        if (column.Contains('.')) TazId_T[i - 1] = (int)Convert.ToDouble(column);
                                        else TazId_T[i - 1] = Convert.ToInt64(column);

                                        TazIndDictionary[TazId_T[i - 1]] = i-1; 
                                    }

                                    else if (j==1) XCoord_T[i-1] = Convert.ToDouble(column);
                                    else if (j==2) YCoord_T[i-1] = Convert.ToDouble(column);
                                    else if (j==3) Area_T[i-1] = Convert.ToDouble(column);
                                    else LUse_T[i - 1, j - 4] = Convert.ToDouble(column);

                                    j++;
                                }

                            }

                            line = file.ReadLine();
                            i++;

                        }

                        file.Close();
                        return;
                    }

                case ".dbf":
                    {
                        return;
                    }

            }
            
 
        }

        private void ReadBlockFile(string fileName)
        {
            // Ask for header - if it is there than deduct one from the total Blocks. As of now, header is there so deduct one.
            bool header = cbBlockHeader.Checked;

            // Total number of blocks in the file
            NumBlocks = File.ReadAllLines(fileName).Length - 1;

            BlockId_B = new long[NumBlocks];
            BlockIndDictionary = new Dictionary<long,int>();
            XCoord_B = new double[NumBlocks];
            YCoord_B = new double[NumBlocks];
            Area_B = new double[NumBlocks];
            LUse_B = new double[NumBlocks, NumLUseVars_B];

            // use streamreader to read the file
            StreamReader file = new StreamReader(fileName);

            // read line by line
            string line = file.ReadLine();

            int i = 0;

            if (!header) i = 1;

            while (line != null)
            {
                string[] split = System.Text.RegularExpressions.Regex.Split(line, "\\s+");

                int j = 0;

                foreach (string column in split)
                {
                    if (column == "blockid10")
                    {
                        break;
                    }
                    else
                    {
                        if (j == 0) 
                        {
                            BlockId_B[i-1] = Convert.ToInt64(column);
                            BlockIndDictionary[BlockId_B[i-1]] = i-1; 
                        }

                        else if (j==1) XCoord_B[i-1] = Convert.ToDouble(column);
                        else if (j==2) YCoord_B[i-1] = Convert.ToDouble(column);
                        else if (j == 3) Area_B[i - 1] = Convert.ToDouble(column);
                        else
                        {
                            if (column.Length > 0) LUse_B[i - 1, j - 4] = Convert.ToDouble(column);
                        }

                        j++;
                    }
                    
                }

                line = file.ReadLine();
                i++;

            }

            file.Close();

        }

        private void ReadMZFile(string fileName)
        {
            // Ask for header - if it is there than deduct one from the total Blocks. As of now, header is there so deduct one.
            bool header = cbMZHeader.Checked;

            // Total number of blocks in the file
            NumMZ = File.ReadAllLines(fileName).Length - 1;

            //mzData = new double[NumMZ - 1, 14];

            // use streamreader to read the file
            StreamReader file = new StreamReader(fileName);

            MZId_M = new int[NumMZ];
            MZIndDictionary = new Dictionary<long, int>();
            XCoord_M = new double[NumMZ];
            YCoord_M = new double[NumMZ];
            Area_M = new double[NumMZ];
            TazId_M = new long[NumMZ];
            BlockId_M = new long[NumMZ];           

            // read line by line
            string line = file.ReadLine();
            int i = 0;

            if (!header) i = 1;

            while (line != null)
            {
                string[] split = System.Text.RegularExpressions.Regex.Split(line, "\\s+");
                //string[] split = System.Text.RegularExpressions.Regex.Split(line, ",");

                int j = 0;

                foreach (string column in split)
                {
                    if (column == "ID")
                    {
                        break;
                    }
                    else
                    {
                        if (j == 0)
                        {
                            // MZId_M[i - 1] = (int) Convert.ToInt64(column);
                            MZId_M[i - 1] = (int)Convert.ToInt32(column);
                            MZIndDictionary[MZId_M[i - 1]] = i - 1;
                            //int test = (int)Convert.ToInt64(column);
                        }

                        else if (j == 1) XCoord_M[i - 1] = Convert.ToDouble(column);
                        else if (j == 2) YCoord_M[i - 1] = Convert.ToDouble(column);
                        else if (j == 3) Area_M[i - 1] = Convert.ToDouble(column);
                        else if (j == 4) TazId_M[i - 1] = Convert.ToInt64(column);
                        else if (j == 5) BlockId_M[i - 1] = Convert.ToInt64(column);

                        j++;
                    }

                }

                line = file.ReadLine();
                i++;

            }

            file.Close();

        }

        private void ReadSchoolFile(string fileName)
        {
            // FILE FORMAT: OBJECTID,XCOORD,YCOORD,STUGRD,STUHGH,STUUNI,SCHID,MZID

            // Ask for header - if it is there than deduct one from the total Blocks. As of now, header is there so deduct one.
            bool header = cbSchoolHeader.Checked;

            // Total number of blocks in the file 
            NumSchools = File.ReadAllLines(fileName).Length - 1;

            SchId = new int[NumSchools];
            MZId_S = new int[NumSchools];
            XCoord_S = new double[NumSchools];
            YCoord_S = new double[NumSchools];  
            SchEnrl_S = new int[NumSchools, NumSchVars];

            // use streamreader to read the file
            StreamReader file = new StreamReader(fileName);

            // read line by line
            string line = file.ReadLine();
            int i = 0;

            if (!header) i = 1;

            while (line != null)
            {
                string[] split = System.Text.RegularExpressions.Regex.Split(line, "\\s+");
                //string[] split = System.Text.RegularExpressions.Regex.Split(line, ",");

                int j = 0;

                foreach (string column in split)
                {
                    if (column == "SCHID")
                    {
                        break;
                    }
                    else
                    {
                        if (j == 0)
                        {
                            SchId[i - 1] = Convert.ToInt16(column);
                        }

                        else if (j == 1) MZId_S[i - 1] = (int)Convert.ToInt64(column);
                        else if (j == 2) XCoord_S[i - 1] = Convert.ToDouble(column);
                        else if (j == 3) YCoord_S[i - 1] = Convert.ToDouble(column);
                        else if (j > 3 && j < 7) SchEnrl_S[i - 1, j - 4] = Convert.ToInt32(column);

                        j++;
                    }

                }

                line = file.ReadLine();
                i++;

            }

            file.Close();

        }

        private void GetInputsForOutput()
        {
            OutNumEmplCats = Input.numDaySimCats;
            OutEmplCats = Input.GetDaySimEmplCatsNames();
            OutEmplCatsNaics = Input.GetDaySimEmplCatsNaics();
        }

        private void DisplayTazFields(string fileName)
        {
            clbEmplCats.Enabled = true;
            clbEmplCats.Items.Clear();

            FileInfo fileFormat = new FileInfo(fileName);
            string extension = fileFormat.Extension;

            switch (extension)
            {
                case ".dat":
                    {
                        // for now this format is assumed

                        StreamReader file = new StreamReader(fileName);

                        string line = file.ReadLine();

                        string[] split = System.Text.RegularExpressions.Regex.Split(line, "\\s+");
                        NumTazFields = split.Length;
                        foreach (string s in split)
                        {
                            string a = s;

                            // display the fields in the list box
                            clbEmplCats.Items.Add(a);
                        }

                        file.Close();
                        break;
                    }
                case ".txt":
                    {
                        StreamReader file = new StreamReader(fileName);

                        string line = file.ReadLine();
                        string[] split = System.Text.RegularExpressions.Regex.Split(line, "\\s+");
                        //string[] split = System.Text.RegularExpressions.Regex.Split(line, ",");

                        NumTazFields = split.Length;

                        // need to modify
                        foreach (string s in split)
                        {
                            string a = s;

                            // display the fields in the list box

                            clbEmplCats.Items.Add(a);
                        }

                        file.Close();

                        break;
                    }
                case ".dbf":
                    {
                        break;
                    }
            }

            btnEmplCatsOk.Enabled = true;

        }

        private void PerformOneYearCalculations()
        {
            // declare local variables
            double[] AreaSum_B;
            double[,] LUse_M;
            double[] AreaSum_T;
            double[,] LUseSum_T;
            double[,] LUseInNaics_T;
            double[,] Empl_M;
            double[] HH_M;
            int[,] SchEnrl_M;

            string OutFolder = Path.GetDirectoryName(OutputFileName);

            // 1. loop micro-zones - sum area by block
            AreaSum_B = SumAreaByBlock.Calculate(NumLUseVars_T, NumLUseVars_B, NumTaz, NumBlocks, NumMZ, Area_M, BlockIndDictionary,
                BlockId_M, BlockId_B, OutFolder);

            // 2. loop micro-zones - factor block data by fraction of block area and sum all by TAZ
            FactorBlockData.Calculate(out LUse_M, out AreaSum_T, out LUseSum_T, NumMZ, NumLUseVars_B, NumSchVars, MZId_M, TazId_M, BlockIndDictionary,
                BlockId_M, TazIndDictionary, LUse_B, Area_M, AreaSum_B, TazId_T, NumTaz, OutFolder);

            // 3. loop tazs - disaggregate categories into naics codes
            LUseInNaics_T = DisaggregateToNaicsCodes.Calculate(NumTaz, TazId_T, LUse_T, NumEmplCats, NaicsCodes, LUseSum_T, NumLUseVars_B, NumLUseVars_T, OutFolder);

            // 4. loop micro-zones - factor micro-zone data to match TAZ totals
            LUse_M = FactorToMatchTazTotals.Calculate(LUse_M, OutFolder, NumMZ, MZId_M, XCoord_M, YCoord_M, Area_M, TazId_M, BlockId_M,
                BlockIndDictionary, TazIndDictionary, TotEmplIndex_B, HHIndex_B, NumLUseVars_B, LUseInNaics_T, LUseSum_T, AreaSum_T);

            // 5. Aggregate to DaySim categories
            AggregateToDaySimCats.Calculate(out Empl_M, out HH_M, NumMZ, OutNumEmplCats, OutEmplCatsNaics, LUse_M);

            // 6. Get school enrollments at MZ level
            SchEnrl_M = AddSchoolData.Calculate(LUse_M, NumSchools, MZIndDictionary, MZId_S, NumSchVars, NumLUseVars_B, SchEnrl_S, NumMZ);

            // 7. Write output
            WriteToOutput.Write(OutputFileName, OutNumEmplCats, OutEmplCats, NumMZ, MZId_M, XCoord_M, YCoord_M, Area_M, TazId_M,
                BlockId_M, Empl_M, HH_M, SchEnrl_M, NumSchVars);
        }

        private void PerformMultiYearCalculations()
        {

            double[,] LUse_T_Base; // hh, sector wise empl, totempl
            int[,] K12Enroll_T_Base;
            int[] HiEduc_T_Base; 
            double[] Area_T_Base;
            Dictionary<int, int> TazIndBaseDictionary;
            double[,] DiffLUseTaz;
            int[] DiffK12EnrollTaz;
            int[] DiffHiEducTaz;
            double[,] SumLUseDri;
            double[] SumAreaDri;
            double[,] ShareLUseDri;
            double[] ShareAreaDri;
            double[] ShareHHTaz;
            double[,] ShareLUseTaz;
            double[] ShareK12EnrollTaz;
            double[] ShareHiEducTaz;
            double[] ShareAreaTaz;
            double[,] DiffLUseParcel;
            double[,] DiffK12EnrollParcel;
            double[] DiffHiEducParcel;
            double[,] LUse_P_Forecast;
            double[,] K12Enroll_P_Forecast;
            double[] HiEduc_P_Forecast;
            int[] DUDensity_T_Base;
            int[] HH_T_Base;
            int[] DiffHHTaz;
            int[] NumVacantParcelsInTaz;
            int[] NumVacantParcelsInDri;
            int[] HH_P_Forecast;
            int[] NumParcelsInTaz;
            double[] GQPop_T_Base;
            double[] ShareGQTaz;
            double[] DiffGQParcel;
            double[] GQPop_P_Forecast;
            double[] DiffGQTaz;
            int[] HH_P_Forecast_Round;
            int[,] LUse_P_Forecast_Round;
            int[,] K12Enroll_P_Forecast_Round;
            int[] HiEduc_P_Forecast_Round;
            
            string OutFolder = Path.GetDirectoryName(ParcelForecastFileName);

            //1. Aggregate base parcel data to Taz
            AggregateToTazBase.Aggregate(out HH_T_Base, out GQPop_T_Base, out LUse_T_Base, out K12Enroll_T_Base, out HiEduc_T_Base, out Area_T_Base, out TazIndBaseDictionary,
                out DUDensity_T_Base, out NumVacantParcelsInTaz, out NumVacantParcelsInDri, out NumParcelsInTaz, ParcelId_P_Base, TazId_P_Base, HH_P_Base, GQPop_P_Base,
                LUse_P_Base, K12Enroll_P_Base, HiEduc_P_Base, Area_P_Base, NumLUseVars_P_Base, NumTaz, ParcelDriCorrespondence);

            //2. Calculate TAZ diff by sector
            CalculateTazDiff.Calculate(out DiffHHTaz, out DiffGQTaz, out DiffLUseTaz, out DiffK12EnrollTaz, out DiffHiEducTaz, HH_T_Base, GQPop_T_Base, LUse_T_Base, K12Enroll_T_Base, HiEduc_T_Base,
                DwellUnits_T_Forecast, GQPop_T_Forecast, LUse_T_Forecast, K12Enroll_T_Forecast, HiEduc_T_Forecast, TazIndBaseDictionary, TazIndForecastDictionary, NumLUseVars_P_Base, NumTaz);

            //3. Calculate sum at DRI level
            CalculateSum.Calculate(out SumLUseDri, out SumAreaDri, ParcelDriCorrespondence, NumTaz, NumLUseVars_P_Base, 
                LUse_P_Base, Area_P_Base, TazId_P_Base, TazIndBaseDictionary);

            //4. Calculate share for parcels in DRI
            SharesForParcelsInDri.Calculate(out ShareLUseDri, out ShareAreaDri, ParcelDriCorrespondence, NumTaz, NumLUseVars_P_Base,
                LUse_P_Base, Area_P_Base, TazId_P_Base, TazIndBaseDictionary, SumLUseDri, SumAreaDri);

            //5. Calculate shares for parcels in TAZ
            SharesForParcelsInTaz.Calculate(out ShareHHTaz, out ShareGQTaz, out ShareLUseTaz, out ShareK12EnrollTaz, out ShareHiEducTaz, out ShareAreaTaz, NumTaz, NumLUseVars_P_Base,
                HH_P_Base, GQPop_P_Base, LUse_P_Base, K12Enroll_P_Base, HiEduc_P_Base, Area_P_Base, TazId_P_Base, TazIndBaseDictionary, HH_T_Base, GQPop_T_Base, LUse_T_Base, 
                K12Enroll_T_Base, HiEduc_T_Base, Area_T_Base);

            //6. Calculate change for parcels
            CalculateParcelDiff.Calculate(out DiffGQParcel, out DiffLUseParcel, out DiffK12EnrollParcel, out DiffHiEducParcel, GQPop_P_Base, LUse_P_Base, K12Enroll_P_Base, HiEduc_P_Base, 
                NumLUseVars_P_Base, TazId_P_Base, TazIndForecastDictionary, SumLUseDri, SumAreaDri, GQPop_T_Base,
                LUse_T_Base, K12Enroll_T_Base, HiEduc_T_Base,  Area_T_Base, ShareLUseDri, ShareAreaDri, ShareGQTaz, ShareLUseTaz, ShareK12EnrollTaz, ShareHiEducTaz,
                ShareAreaTaz, DiffGQTaz, DiffLUseTaz, DiffK12EnrollTaz, DiffHiEducTaz);

            //7. Allocate change to parcels
            AllocateChangeToParcels.Allocate(out GQPop_P_Forecast, out LUse_P_Forecast, out K12Enroll_P_Forecast, out HiEduc_P_Forecast, GQPop_P_Base, LUse_P_Base, K12Enroll_P_Base, 
                HiEduc_P_Base, K12Enroll_T_Forecast, HiEduc_T_Forecast, DiffGQParcel, DiffLUseParcel, DiffK12EnrollParcel,
                DiffHiEducParcel, NumLUseVars_P_Base, ShareK12EnrollTaz, ShareHiEducTaz, TazId_P_Base);

            //7.1 Allocate HH change to parcels
            AllocateChangeToParcels.AllocateHH(out HH_P_Forecast,DiffHHTaz, HH_P_Base, DUDensity_T_Base, NumVacantParcelsInTaz, NumVacantParcelsInDri,
                NumParcelsInTaz, ParcelVacantCorrespondence, TazId_P_Base,ShareHHTaz);

            // 8.0 Intergerize values
            IntegeriseValues.Calculate(out LUse_P_Forecast_Round, out K12Enroll_P_Forecast_Round, out HiEduc_P_Forecast_Round, HH_P_Forecast, GQPop_P_Forecast, LUse_P_Forecast,
                K12Enroll_P_Forecast, HiEduc_P_Forecast, ParcelVacantCorrespondence, NumLUseVars_P_Base);

            //8. Write output
            WriteMultiYearOutput.Write(HH_P_Forecast, GQPop_P_Forecast, LUse_P_Forecast_Round, K12Enroll_P_Forecast_Round, HiEduc_P_Forecast_Round, ParcelId_P_Base, XCoord_P_Base, YCoord_P_Base, 
                Area_P_Base, TazId_P_Base, ParcelForecastFileName, ParcelDriCorrespondence);

        }

        private void GetAnalysisType()
        {
            if (rdbOneYear.Checked == true) AnalysisType = Constants.OneYear;
            else if (rdbMultiYears.Checked == true) AnalysisType = Constants.MultiYear;
        }

        private void GetEmploymentClassificationType()
        {
            if (rdbNaics.Checked == true) EmplClassType = Constants.Naics;
            else if (rdbSic.Checked == true) EmplClassType = Constants.Sic;
        }

        private void GetEmploymentCategories()
        {
            CatsSelected = true;

            NumEmplCats = clbEmplCats.CheckedItems.Count;

            EmplCatsName = new string[NumEmplCats];

            // Get categories
            for (int cat = 0; cat < NumEmplCats; cat++)
            {
                EmplCatsName[cat] = clbEmplCats.CheckedItems[cat].ToString();

            }

        }

        private void GetNaicsCodes()
        {
            // Get codes
            switch (EmplClassType)
            {
                case Constants.Naics:
                    {
                        NaicsCodes = NaicsForm.GetNaicsCodes(NumEmplCats);
                        break;
                    }

                case Constants.Sic:
                    {
                        SicCodes = SicForm.GetSicCodes(NumEmplCats);

                        // convert to NAICS codes
                        NaicsCodes = SicToNaics.Convert(SicCodes, NumEmplCats);

                        //GetNaicsFromSic();

                        break;
                    }

            }

        }

        //private void GetNaicsFromSic()
        //{
        //    // move to a different class - done
        //    GetSicToNaicsRelation();
            
        //    for (int cat = 0; cat < NumEmplCats; cat++)
        //    {
        //        for (int sic = 0; sic < 10; sic++)
        //        {
        //            if (SicCodes[cat, sic] == 1)
        //            {
        //                for (int i = 0; i < 20; i++) NaicsCodes[cat, i] = SicToNaicsConversion[sic, i];
        //            }
        //        }

        //    }

        //}

        //private void GetSicToNaicsRelation()
        //{
        //    // move to a different class - done
        //    SicToNaicsConversion = new int[10, 20];

        //    for (int sic = 0; sic < 10; sic++)
        //    {
        //        for (int naics = 0; naics < 20; naics++)
        //        {
        //            if (sic == 1 && naics == 0) SicToNaicsConversion[sic, naics] = 1;
        //            if (sic == 2 && naics == 1) SicToNaicsConversion[sic, naics] = 1;
        //            if (sic == 3 && naics == 2) SicToNaicsConversion[sic, naics] = 1;
        //            if (sic == 4 && naics == 3) SicToNaicsConversion[sic, naics] = 1;
        //            if (sic == 5 && naics >= 4 && naics < 6) SicToNaicsConversion[sic, naics] = 1;
        //            if (sic == 6 && naics == 6) SicToNaicsConversion[sic, naics] = 1;
        //            if (sic == 7 && naics >= 7 && naics < 9) SicToNaicsConversion[sic, naics] = 1;
        //            if (sic == 8 && naics >= 9 && naics < 11) SicToNaicsConversion[sic, naics] = 1;
        //            if (sic == 9 && naics >= 11 && naics < 18) SicToNaicsConversion[sic, naics] = 1;
        //            if (sic == 10 && naics >= 18 && naics < 19) SicToNaicsConversion[sic, naics] = 1;

        //        }
        //    }


        //}

 
        private void SaveXmlFile()
        {
            // Make a seperate class
            XmlTextWriter xWriter = new XmlTextWriter("C:\\Projects\\STEP Transferability\\Tools\\DisaggregationTool\\Run\\inputs.xml", Encoding.UTF8);
            xWriter.Formatting = Formatting.Indented;

            xWriter.WriteStartElement("DisaggregationTool"); // <Disaggregation Tool>

            xWriter.WriteStartElement("Inputs"); // <Inputs>

            xWriter.WriteStartElement("TAZ"); // <TAZ>

            xWriter.WriteStartElement("InputFile");
            xWriter.WriteString(txtTazFile.Text);
            xWriter.WriteEndElement();

            xWriter.WriteStartElement("Header");
            xWriter.WriteString(cbTazHeader.Checked.ToString());
            xWriter.WriteEndElement();

            xWriter.WriteStartElement("EmplCats");

            // write categories and their naics codes
            for (int cat = 0; cat < NumEmplCats; cat++)
            {
                xWriter.WriteStartElement(EmplCatsName[cat]);
                xWriter.WriteString(clbEmplCats.CheckedIndices[cat].ToString());
                xWriter.WriteEndElement();
            }
            
            xWriter.WriteEndElement(); //</EmplCats>

            xWriter.WriteStartElement("NAICS_Codes");

            for (int cat = 0; cat < NumEmplCats; cat++)
            {
                // write employment categories
                xWriter.WriteStartElement(EmplCatsName[cat]);

                // write naics codes
                for(int code=0;code<20;code++)
                {
                    xWriter.WriteStartElement("CNS" + (code + 1).ToString());
                    xWriter.WriteString(NaicsCodes[cat, code].ToString());
                    xWriter.WriteEndElement();

                }
                xWriter.WriteEndElement();
            }

            xWriter.WriteEndElement(); // </NAICS_Codes>

            xWriter.WriteEndElement(); // </TAZ>

            xWriter.WriteStartElement("BLOCK");
            xWriter.WriteStartElement("InputFile");
            xWriter.WriteString(txtBlockFile.Text);
            xWriter.WriteEndElement();

            xWriter.WriteStartElement("Header");
            xWriter.WriteString(cbBlockHeader.Checked.ToString());
            xWriter.WriteEndElement();

            xWriter.WriteEndElement(); // </BLOCK>

            xWriter.WriteStartElement("MZ");
            xWriter.WriteStartElement("InputFile");
            xWriter.WriteString(txtMzFile.Text);
            xWriter.WriteEndElement();

            xWriter.WriteStartElement("Header");
            xWriter.WriteString(cbMZHeader.Checked.ToString());
            xWriter.WriteEndElement();

            xWriter.WriteEndElement(); // </MZ>

            xWriter.WriteStartElement("SCHOOL");
            xWriter.WriteStartElement("InputFile");
            xWriter.WriteString(txtSchoolFile.Text);
            xWriter.WriteEndElement();

            xWriter.WriteStartElement("Header");
            xWriter.WriteString(cbSchoolHeader.Checked.ToString());
            xWriter.WriteEndElement();

            xWriter.WriteEndElement(); // </SCHOOL>

            xWriter.WriteEndElement(); // </Inputs>

            xWriter.WriteStartElement("Output");
            xWriter.WriteStartElement("OutputFile");
            xWriter.WriteString(txtOutputFile.Text);
            xWriter.WriteEndElement();

            xWriter.WriteStartElement("EmplCats");

            // write categories and their naics codes
            for (int cat = 0; cat < OutNumEmplCats; cat++)
            {
                xWriter.WriteStartElement("Cat" + cat.ToString());
                xWriter.WriteString(OutEmplCats[cat]);
                xWriter.WriteEndElement();
            }

            xWriter.WriteEndElement(); //</EmplCats>

            xWriter.WriteStartElement("NAICS_Codes");

            for (int cat = 0; cat < OutNumEmplCats; cat++)
            {
                // write the employment category
                xWriter.WriteStartElement(OutEmplCats[cat]);

                // write naics codes
                for (int code = 0; code < 20; code++)
                {
                    xWriter.WriteStartElement("CNS" + (code + 1).ToString());
                    xWriter.WriteString(OutEmplCatsNaics[cat,code].ToString());
                    xWriter.WriteEndElement();
                }

                xWriter.WriteEndElement();
            }

            xWriter.WriteEndElement(); // </NAICS_Codes>
            
            xWriter.WriteEndElement(); // </OUTPUT>

            xWriter.WriteEndElement(); // </Disaggregation Tool>

            xWriter.Close();
        }

        private void ReadXmlFile(string filename)
        {
            try
            {
                XmlDocument xDocReader = new XmlDocument();
                xDocReader.Load(filename);

                txtTazFile.Text = xDocReader.SelectSingleNode("DisaggregationTool/Inputs/TAZ/InputFile").InnerText;
                txtBlockFile.Text = xDocReader.SelectSingleNode("DisaggregationTool/Inputs/BLOCK/InputFile").InnerText;
                txtMzFile.Text = xDocReader.SelectSingleNode("DisaggregationTool/Inputs/MZ/InputFile").InnerText;
                txtSchoolFile.Text = xDocReader.SelectSingleNode("DisaggregationTool/Inputs/SCHOOL/InputFile").InnerText;
                txtOutputFile.Text = xDocReader.SelectSingleNode("DisaggregationTool/Output/OutputFile").InnerText;

                cbTazHeader.Checked = Convert.ToBoolean(xDocReader.SelectSingleNode("DisaggregationTool/Inputs/TAZ/Header").InnerText);
                cbBlockHeader.Checked = Convert.ToBoolean(xDocReader.SelectSingleNode("DisaggregationTool/Inputs/BLOCK/Header").InnerText);
                cbMZHeader.Checked = Convert.ToBoolean(xDocReader.SelectSingleNode("DisaggregationTool/Inputs/MZ/Header").InnerText);
                cbSchoolHeader.Checked = Convert.ToBoolean(xDocReader.SelectSingleNode("DisaggregationTool/Inputs/SCHOOL/Header").InnerText);

                TazFileName = txtTazFile.Text;
                BlockFileName = txtBlockFile.Text;
                MzFileName = txtMzFile.Text;
                SchoolFileName = txtSchoolFile.Text;
                OutputFileName = txtOutputFile.Text;

                // Display TAZ fields in the list box
                DisplayTazFields(TazFileName);

                // Check employment Categories
                XmlNode emplNodes = xDocReader.SelectSingleNode("DisaggregationTool/Inputs/TAZ/EmplCats");
                NumEmplCats = emplNodes.ChildNodes.Count;

                EmplCatsName = new string[NumEmplCats];
                // first get and populate the fields
                for (int cat = 0; cat < NumEmplCats; cat++)
                {
                    EmplCatsName[cat] = emplNodes.ChildNodes[cat].LocalName;
                    int fieldIndex = Convert.ToInt16(emplNodes.ChildNodes[cat].InnerText);
                    clbEmplCats.SetItemChecked(fieldIndex, true);

                }

                CatsSelected = true;

                // NAICS Codes
                XmlNode naicsNode = emplNodes.NextSibling; // go to <NAICS_Code>
                XmlNode firstEmplCat = naicsNode.FirstChild; // Access first element

                NaicsCodes = new int[NumEmplCats, 20];
                for (int cat = 0; cat < NumEmplCats; cat++)
                {
                    for (int code = 0; code < 20; code++)
                    {

                        NaicsCodes[cat, code] = Convert.ToInt16(firstEmplCat.ChildNodes[code].InnerText); // 1 = checked; 0 - not checked

                    }

                    // go to next employment category
                    firstEmplCat = firstEmplCat.NextSibling;
                }
                // SIC Codes
                SicCodes = new int[NumEmplCats, 10];

                // DaySim Employment Categories
                XmlNode daySimNodes = xDocReader.SelectSingleNode("DisaggregationTool/Output/EmplCats");

                OutNumEmplCats = daySimNodes.ChildNodes.Count;

                OutEmplCats = new string[OutNumEmplCats];
                OutEmplCatsNaics = new int[OutNumEmplCats, 20];

                // Read Categories
                for (int cat = 0; cat < OutNumEmplCats; cat++)
                {
                    OutEmplCats[cat] = daySimNodes.ChildNodes[cat].InnerText;
                }

                // Read NAICS Codes
                XmlNode daySimNaicsNode = daySimNodes.NextSibling; // go to <NAICS_Code>
                XmlNode firstDaySimCat = daySimNaicsNode.FirstChild; // Access first element

                for (int cat = 0; cat < OutNumEmplCats; cat++)
                {
                    for (int code = 0; code < 20; code++)
                    {
                        OutEmplCatsNaics[cat, code] = Convert.ToInt16(firstDaySimCat.ChildNodes[code].InnerText); // 1 = checked; 0 - not checked
                    }

                    // go to next employment category
                    firstDaySimCat = firstDaySimCat.NextSibling;
                }
            }

            catch (System.IO.IOException e)
            {
                MessageBox.Show("Error: " + e.Message);
                return;
            }

        }

        private void btnBrowseTaz_Click(object sender, EventArgs e)
        {
            OpenFileDialog openTazInputFileDialog = new OpenFileDialog();

            openTazInputFileDialog.InitialDirectory = "C:\\Projects\\STEP Transferability\\Tools\\DisaggregationTool\\Run\\";
            openTazInputFileDialog.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
            //openTazInputFileDialog.FileName = "TAZ_rochester.txt";
            openTazInputFileDialog.Title = "Open TAZ File";
            openTazInputFileDialog.FilterIndex = 2;
            openTazInputFileDialog.RestoreDirectory = true;

            if (openTazInputFileDialog.ShowDialog() == DialogResult.OK)
            {
                // get the name of the taz file
                TazFileName = openTazInputFileDialog.FileName;
                txtTazFile.Text = TazFileName;
                cbTazHeader.Checked = true;

                // display fields in the check box
                DisplayTazFields(TazFileName);
            }

        }

        private void btnBrowseBlocks_Click(object sender, EventArgs e)
        {
            OpenFileDialog openBlockInputFileDialog = new OpenFileDialog();

            openBlockInputFileDialog.InitialDirectory = "C:\\Projects\\STEP Transferability\\Tools\\DisaggregationTool\\Run\\";
            openBlockInputFileDialog.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
            //openBlockInputFileDialog.FileName = "BLOCKS_SE_rochester.dat";
            openBlockInputFileDialog.FilterIndex = 2;
            openBlockInputFileDialog.RestoreDirectory = true;

            if (openBlockInputFileDialog.ShowDialog() == DialogResult.OK)
            {
                BlockFileName = openBlockInputFileDialog.FileName;
                txtBlockFile.Text = BlockFileName;
                cbBlockHeader.Checked = true;
            }
        }

        private void btnBrowseMZ_Click(object sender, EventArgs e)
        {
            OpenFileDialog openMZInputFileDialog = new OpenFileDialog();

            openMZInputFileDialog.InitialDirectory = "C:\\Projects\\STEP Transferability\\Tools\\DisaggregationTool\\Run\\";
            openMZInputFileDialog.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
            //openMZInputFileDialog.FileName = "MZ_rochester.txt";
            openMZInputFileDialog.FilterIndex = 2;
            openMZInputFileDialog.RestoreDirectory = true;

            if (openMZInputFileDialog.ShowDialog() == DialogResult.OK)
            {
                MzFileName = openMZInputFileDialog.FileName;
                txtMzFile.Text = MzFileName;
                cbMZHeader.Checked = true;
            }
        }

        private void btnBrowseSchool_Click(object sender, EventArgs e)
        {
            OpenFileDialog openSchoolInputFileDialog = new OpenFileDialog();

            openSchoolInputFileDialog.InitialDirectory = "C:\\Projects\\STEP Transferability\\Tools\\DisaggregationTool\\Run\\";
            openSchoolInputFileDialog.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
            //openSchoolInputFileDialog.FileName = "Schools_rochester.txt";
            openSchoolInputFileDialog.FilterIndex = 2;
            openSchoolInputFileDialog.RestoreDirectory = true;

            if (openSchoolInputFileDialog.ShowDialog() == DialogResult.OK)
            {
                SchoolFileName = openSchoolInputFileDialog.FileName;
                txtSchoolFile.Text = SchoolFileName;
                cbSchoolHeader.Checked = true;
            }
        }

        private void btnBrowseTazForecast_Click(object sender, EventArgs e)
        {
            OpenFileDialog openTazForecastInputFileDialog = new OpenFileDialog();

            openTazForecastInputFileDialog.InitialDirectory = "C:\\Projects\\FDOT Allocation Tool\\inputs\\InputData\\";
            openTazForecastInputFileDialog.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
            openTazForecastInputFileDialog.FileName = "ZDATA2_9E_40A.csv";
            openTazForecastInputFileDialog.FilterIndex = 2;
            openTazForecastInputFileDialog.RestoreDirectory = true;

            if (openTazForecastInputFileDialog.ShowDialog() == DialogResult.OK)
            {
                TazForecastFileName = openTazForecastInputFileDialog.FileName;
                txtTazForecastFile.Text = TazForecastFileName;
                cbTazForecastHeader.Checked = true;
            }

        }

        private void btnBrowseTazForecast1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openTazForecastInputFile1Dialog = new OpenFileDialog();

            openTazForecastInputFile1Dialog.InitialDirectory = "C:\\Projects\\FDOT Allocation Tool\\inputs\\InputData\\";
            openTazForecastInputFile1Dialog.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
            openTazForecastInputFile1Dialog.FileName = "ZDATA1_40A.csv";
            openTazForecastInputFile1Dialog.FilterIndex = 2;
            openTazForecastInputFile1Dialog.RestoreDirectory = true;

            if (openTazForecastInputFile1Dialog.ShowDialog() == DialogResult.OK)
            {
                TazForecastFileName1 = openTazForecastInputFile1Dialog.FileName;
                txtTazForecastFile1.Text = TazForecastFileName1;
                cbTazForecastHeader1.Checked = true;
            }

        }

        private void btnBrowseParcelBase_Click(object sender, EventArgs e)
        {
            OpenFileDialog openParcelBaseInputFileDialog = new OpenFileDialog();

            openParcelBaseInputFileDialog.InitialDirectory = "C:\\Projects\\FDOT Allocation Tool\\inputs\\InputData\\";
            openParcelBaseInputFileDialog.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
            openParcelBaseInputFileDialog.FileName = "Tampa_buffparcel_logdecay_allstreets.dat";
            openParcelBaseInputFileDialog.FilterIndex = 2;
            openParcelBaseInputFileDialog.RestoreDirectory = true;

            if (openParcelBaseInputFileDialog.ShowDialog() == DialogResult.OK)
            {
                ParcelBaseFileName = openParcelBaseInputFileDialog.FileName;
                txtParcelBaseFile.Text = ParcelBaseFileName;
                cbParcelBaseHeader.Checked = true;
            }
        }

        private void btnBrowseCorrespondence_Click(object sender, EventArgs e)
        {
            OpenFileDialog openCorrespondenceInputFileDialog = new OpenFileDialog();

            openCorrespondenceInputFileDialog.InitialDirectory = "C:\\Projects\\FDOT Allocation Tool\\inputs\\Input Data\\";
            openCorrespondenceInputFileDialog.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
            openCorrespondenceInputFileDialog.FileName = "Parcel_DRI_TAZ_DESC_HH1_GQ_Sorted.csv";
            openCorrespondenceInputFileDialog.FilterIndex = 2;
            openCorrespondenceInputFileDialog.RestoreDirectory = true;

            if (openCorrespondenceInputFileDialog.ShowDialog() == DialogResult.OK)
            {
                DriCorrespondenceFileName = openCorrespondenceInputFileDialog.FileName;
                txtDriCorrespondenceFile.Text = DriCorrespondenceFileName;
                cbCorrespondenceHeader.Checked = true;
            }
        }

        private void btnBrowseSectorCorrespondence_Click(object sender, EventArgs e)
        {
            OpenFileDialog openSetctorCorrespondenceInputFileDialog = new OpenFileDialog();

            openSetctorCorrespondenceInputFileDialog.InitialDirectory = "C:\\Projects\\FDOT Allocation Tool\\inputs\\Input Data\\";
            openSetctorCorrespondenceInputFileDialog.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
            openSetctorCorrespondenceInputFileDialog.FileName = "TazSectorToDaysimSectorCorrespondence.csv";
            openSetctorCorrespondenceInputFileDialog.FilterIndex = 2;
            openSetctorCorrespondenceInputFileDialog.RestoreDirectory = true;

            if (openSetctorCorrespondenceInputFileDialog.ShowDialog() == DialogResult.OK)
            {
                SectorCorrespondenceFileName = openSetctorCorrespondenceInputFileDialog.FileName;
                txtSectorCorrespondenceFile.Text = SectorCorrespondenceFileName;
                cbSectorCorrespondenceHeader.Checked = true;
            }
        }


        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            //OpenFileDialog openOutputFileDialog = new OpenFileDialog();

            SaveFileDialog saveOutputFileDialog = new SaveFileDialog();

            saveOutputFileDialog.InitialDirectory = "C:\\Projects\\STEP Transferability\\Tools\\DisaggregationTool\\Run\\";
            saveOutputFileDialog.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            //saveOutputFileDialog.FileName = "mzOutput";
            saveOutputFileDialog.Title = "Save Micro Zone Output";
            saveOutputFileDialog.RestoreDirectory = true;

            if (saveOutputFileDialog.ShowDialog() == DialogResult.OK)
            {
                OutputFileName = saveOutputFileDialog.FileName;
                txtOutputFile.Text = OutputFileName;
            }
        }

        private void btnBrowseParcelForecast_Click(object sender, EventArgs e)
        {

            SaveFileDialog saveOutputFileDialog = new SaveFileDialog();

            saveOutputFileDialog.InitialDirectory = "C:\\Projects\\FDOT Allocation Tool\\inputs\\Input Data\\";
            saveOutputFileDialog.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            saveOutputFileDialog.FileName = "parcelOutput_enroll";
            saveOutputFileDialog.Title = "Save Future Year Parcel Output";
            saveOutputFileDialog.RestoreDirectory = true;

            if (saveOutputFileDialog.ShowDialog() == DialogResult.OK)
            {
                ParcelForecastFileName = saveOutputFileDialog.FileName;
                txtParcelForecastFile.Text = ParcelForecastFileName;
            }

        }


        private void btnEmplCatsOk_Click(object sender, EventArgs e)
        {
            try
            {
                // Get empl classification: NAICS or SIC
                GetEmploymentClassificationType();

                // Get selected employment categories
                GetEmploymentCategories();

                // show form for codes input
                if (EmplClassType == Constants.Naics)
                {
                    if (NaicsForm.IsDisposed) NaicsForm = new FrmNaics();
                    NaicsForm.Show();

                    NaicsForm.ShowEmplFields(NumEmplCats, EmplCatsName);
                    if (ReadFromXml) NaicsForm.ShowXmlInputs(NumEmplCats, EmplCatsName, NaicsCodes);
                }

                else if (EmplClassType == Constants.Sic)
                {
                    if (SicForm.IsDisposed) SicForm = new FrmSic();
                    SicForm.Show();

                    SicForm.ShowEmplFields(NumEmplCats, EmplCatsName);
                    if (ReadFromXml) SicForm.ShowXmlInputs(NumEmplCats, EmplCatsName, SicCodes);
                }
            }

            catch (System.IO.IOException error)
            {
                MessageBox.Show("Error: " + error.Message);
                if (NaicsForm != null) this.Close();
                if (SicForm != null) this.Close();
            }

        }


        private void btnReadXML_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openXMLFileDialog = new OpenFileDialog();
                openXMLFileDialog.InitialDirectory = "C:\\Projects\\STEP Transferability\\Tools\\DisaggregationTool\\Run\\";
                openXMLFileDialog.Filter = "XML files (*.xml)|*.xml";
                //openXMLFileDialog.FileName = "inputs_rochester";
                openXMLFileDialog.RestoreDirectory = true;
                string xmlInputFile = openXMLFileDialog.FileName; // default

                if (openXMLFileDialog.ShowDialog() == DialogResult.OK) xmlInputFile = openXMLFileDialog.FileName;
                else
                {
                    MessageBox.Show("No file selected");
                    return;
                }

                ReadFromXml = true;

                ReadXmlFile(xmlInputFile);

                //XmlFile.Read(,,out NumEmplCats,out EmplCatsName, out NaicsCodes, out SicCodes,out OutNumEmplCats, 
                //    out OutEmplCats, out OutEmplCatsNaics,);

            }

            catch (System.IO.IOException error)
            {
                MessageBox.Show("Error: " + error.Message);
                return;
            }
            
        }

        private void btnOutputForm_Click(object sender, EventArgs e)
        {
            try
            {
                if (Input.IsDisposed) Input = new FrmOutput();
                Input.Show();

                // if read data from XML file
                if (ReadFromXml) Input.ShowXmlInputs(OutNumEmplCats, OutEmplCats, OutEmplCatsNaics);
            }

            catch (System.IO.IOException error)
            {
                MessageBox.Show("Error: " + error.Message);
                if (Input != null) Input.Close();
            }

        }

        public int NumTazFields
        {
            get { return _numTazFields; }
            set { _numTazFields = value; }
        }

        public string EmplClassType
        {
            get { return _emplClassType; }
            set { _emplClassType = value; }
        }

        public int NumEmplCats
        {
            get { return _numEmplCats; }
            set { _numEmplCats = value; }
        }

        public int NumBlocks
        {
            get { return _numBlocks; }
            set { _numBlocks = value; }
        }

        public int NumMZ
        {
            get { return _numMZ; }
            set { _numMZ = value; }
        }

        public int NumTaz
        {
            get { return _numTaz; }
            set { _numTaz = value; }
        }

        public int[,] NaicsCodes
        {
            get { return _naicsCodes; }
            set { _naicsCodes = value; }
        }

        public int[,] SicCodes
        {
            get { return _sicCodes; }
            set { _sicCodes = value; }
        }

        public long[] TazId_T
        {
            get { return _tazid_t; }
            set { _tazid_t = value; }
        }

        public Dictionary<long, int> TazIndDictionary
        {
            get { return _tazIndDictionary; }
            set { _tazIndDictionary = value; }
        }

        public double[] Area_T
        {
            get { return _area_t; }
            set { _area_t = value; }
        }

        public double[,] LUse_T
        {
            get { return _luse_t; }
            set { _luse_t = value; }
        }

        public double[,] LUseCode_T
        {
            get { return _lusecode_t; }
            set { _lusecode_t = value; }
        }

        public long[] BlockId_B
        {
            get { return _blockId_b; }
            set { _blockId_b = value; }
        }
        public Dictionary<long, int> BlockIndDictionary
        {
            get { return _blockIndDictionary; }
            set { _blockIndDictionary = value; }
        }
        public double[] Area_B
        {
            get { return _area_b; }
            set { _area_b = value; }
        }

        public double[,] LUse_B
        {
            get { return _luse_b; }
            set { _luse_b = value; }
        }

        public int[] MZId_M
        {
            get { return _mzid_m; }
            set { _mzid_m = value; }
        }

        public Dictionary<long, int> MZIndDictionary
        {
            get { return _mzIndDictionary; }
            set { _mzIndDictionary = value; }
        }

        public double[] Area_M
        {
            get { return _area_m; }
            set { _area_m = value; }
        }

        public long[] BlockId_M
        {
            get { return _blockid_m; }
            set { _blockid_m = value; }
        }

        public long[] TazId_M
        {
            get { return _tazid_m; }
            set { _tazid_m = value; }
        }

        public int[] SchId
        {
            get { return _schid; }
            set { _schid = value; }
        }

        public int[] MZId_S
        {
            get { return _mzid_s; }
            set { _mzid_s = value; }
        }

        public int[,] SchEnrl_S
        {
            get { return _schenrl_s; }
            set { _schenrl_s = value; }
        }

        public int NumSchools
        {
            get { return _numSchools; }
            set { _numSchools = value; }
        }

        public double[] XCoord_T
        {
            get { return _xcoord_t; }
            set { _xcoord_t = value; }
        }

        public double[] YCoord_T
        {
            get { return _ycoord_t; }
            set { _ycoord_t = value; }
        }

        public static int NumLUseVars_T
        {
            get { return _numLUseVars_T; }
            set { _numLUseVars_T = value; }
        }

        public double[] XCoord_B
        {
            get { return _xcoord_b; }
            set { _xcoord_b = value; }
        }

        public double[] YCoord_B
        {
            get { return _ycoord_b; }
            set { _ycoord_b = value; }
        }

        public double[] XCoord_M
        {
            get { return _xcoord_m; }
            set { _xcoord_m = value; }
        }

        public double[] YCoord_M
        {
            get { return _ycoord_m; }
            set { _ycoord_m = value; }
        }

        public double[] XCoord_S
        {
            get { return _xcoord_s; }
            set { _xcoord_s = value; }
        }

        public double[] YCoord_S
        {
            get { return _ycoord_s; }
            set { _ycoord_s = value; }
        }

        private bool CatsSelected
        {
            get { return _catsSelected; }
            set { _catsSelected = value; }
        }

        private bool CodesSelected
        {
            get { return _codesSelected; }
            set { _codesSelected = value; }
        }

        private string TazFileName
        {
            get { return _tazFileName; }
            set { _tazFileName = value; }
        }

        private string BlockFileName
        {
            get { return _blockFileName; }
            set { _blockFileName = value; }
        }

        private string MzFileName
        {
            get { return _mzFileName; }
            set { _mzFileName = value; }
        }

        private string SchoolFileName
        {
            get { return _schoolFileName; }
            set { _schoolFileName = value; }
        }

        private string OutputFileName
        {
            get { return _outputFileName; }
            set { _outputFileName = value; }
        }

        public int OutNumEmplCats
        {
            get { return _outNumEmplCats; }
            set { _outNumEmplCats = value; }
        }

        public string[] OutEmplCats
        {
            get { return _outEmplCats; }
            set { _outEmplCats = value; }
        }

        public int[,] OutEmplCatsNaics
        {
            get { return _outEmplCatsNaics; }
            set { _outEmplCatsNaics = value; }
        }

        public bool ReadFromXml
        {
            get { return _readFromXml; }
            set { _readFromXml = value; }
        }

        private string[] EmplCatsName
        {
            get { return _emplCats; }
            set { _emplCats = value; }
        }

        private int[,] SicToNaicsConversion
        {
            get { return _sicToNaicsConversion; }
            set { _sicToNaicsConversion = value; }
        }

        // FDOT allocation tool

        private string TazForecastFileName
        {
            get { return _tazForecastFileName; }
            set { _tazForecastFileName = value; }
        }

        private string TazForecastFileName1
        {
            get { return _tazForecastFileName1; }
            set { _tazForecastFileName1 = value; }
        }


        private string ParcelBaseFileName
        {
            get { return _parcelBaseFileName; }
            set { _parcelBaseFileName = value; }
        }

        private string ParcelForecastFileName
        {
            get { return _parcelForecastFileName; }
            set { _parcelForecastFileName = value; }
        }

        private string DriCorrespondenceFileName
        {
            get { return _driCorrespondenceFileName; }
            set { _driCorrespondenceFileName = value; }
        }

        private string SectorCorrespondenceFileName
        {
            get { return _sectorCorrespondenceFileName; }
            set { _sectorCorrespondenceFileName = value; }
        }


        public double[,] LUse_T_Forecast
        {
            get { return _luse_t_forecast; }
            set { _luse_t_forecast = value; }
        }

        public double[] Area_T_Forecast
        {
            get { return _area_t_forecast; }
            set { _area_t_forecast = value; }
        }

        public double[,] LUse_P_Base
        {
            get { return _luse_p_base; }
            set { _luse_p_base = value; }
        }

        public double[] Area_P_Base
        {
            get { return _area_p_base; }
            set { _area_p_base = value; }
        }
        
        public double[,] LUse_P_Forecast
        {
            get { return _luse_p_forecast; }
            set { _luse_p_forecast = value; }
        }

        public double[] Area_P_Forecast
        {
            get { return _area_p_forecast; }
            set { _area_p_forecast = value; }
        }

        public Dictionary<long, int> TazParcelDictionary
        {
            get { return _tazParcelDictionary; }
            set { _tazParcelDictionary = value; }
        }

        public Dictionary<long, int> ParcelDriDictionary
        {
            get { return _parcelDriDictionary; }
            set { _parcelDriDictionary = value; }
        }

        public int[] TazId_T_Forecast
        {
            get { return _tazid_t_forecast; }
            set { _tazid_t_forecast = value; }
        }

        public long[] ParcelId_P_Base
        {
            get { return _parcelid_p_base; }
            set { _parcelid_p_base = value; }
        }

        public long[] ParcelId_P_Forecast
        {
            get { return _parcelid_p_forecast; }
            set { _parcelid_p_forecast = value; }
        }


        public Dictionary<int, int> TazIndForecastDictionary
        {
            get { return _tazIndForecastDictionary; }
            set { _tazIndForecastDictionary = value; }
        }


        public static int NumLUseVars_T_Forecast
        {
            get { return _numLUseVars_t_forecast; }
            set { _numLUseVars_t_forecast = value; }
        }

        public static int NumLUseVars_P_Base
        {
            get { return _numLUseVars_p_base; }
            set { _numLUseVars_p_base = value; }
        }

        public static int NumLUseVars_P_Forecast
        {
            get { return _numLUseVars_p_forecast; }
            set { _numLUseVars_p_forecast = value; }
        }

        public int NumParcel
        {
            get { return _numParcel; }
            set { _numParcel = value; }
        }

        public int[] TazId_P_Base
        {
            get { return _tazid_p_base; }
            set { _tazid_p_base = value; }
        }

        public Dictionary<int, int> SectorCorrespondenceDictionary
        {
            get { return _sectorCorrespondenceDictionary; }
            set { _sectorCorrespondenceDictionary = value; }
        }

        public string AnalysisType
        {
            get { return _analysisType; }
            set { _analysisType = value; }
        }

        public int[,] ParcelDriCorrespondence
        {
            get { return _parcelDriCorrespondence; }
            set { _parcelDriCorrespondence = value; }
        }

        public long[,] ParcelVacantCorrespondence
        {
            get { return _parcelVacantCorrespondence; }
            set { _parcelVacantCorrespondence = value; }
        }

        public double[] XCoord_P_Base
        {
            get { return _xcoord_p_base; }
            set { _xcoord_p_base = value; }
        }

        public double[] YCoord_P_Base
        {
            get { return _ycoord_p_base; }
            set { _ycoord_p_base = value; }
        }

        public int[] HH_P_Base
        {
            get { return _hh_p_base; }
            set { _hh_p_base = value; }
        }

        public double[,] Enroll_P_Base
        {
            get { return _enroll_p_base; }
            set { _enroll_p_base = value; }
        }

        public int[,] K12Enroll_P_Base
        {
            get { return _k12Enroll_p_base; }
            set { _k12Enroll_p_base = value; }
        }

        public int[] HiEduc_P_Base
        {
            get { return _hieduc_p_base; }
            set { _hieduc_p_base = value; }
        }

        public int[] K12Enroll_T_Forecast
        {
            get { return _k12Enroll_t_forecast; }
            set { _k12Enroll_t_forecast = value; }
        }
        
        public int[] HiEduc_T_Forecast
        {
            get { return _hiEduc_t_forecast; }
            set { _hiEduc_t_forecast = value; }
        }

        public int[,] DwellUnits_T_Forecast
        {
            get { return _dwellUnits_t_forecast; }
            set { _dwellUnits_t_forecast = value; }
        }

        public double[] GQPop_T_Forecast
        {
            get { return _gqPop_t_forecast; }
            set { _gqPop_t_forecast = value; }
        }
        
        public double[] GQPop_P_Base
        {
            get { return _gqPop_p_base; }
            set { _gqPop_p_base = value; }
        }


    }
}
