using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DisaggregationTool
{
    public partial class FrmOutput : Form
    {
        private static FrmOutput outputForm;
        public int numDaySimCats;
        public string[] daySimCats;
        public int[,] daySimCatsNaics;

        public static FrmOutput GetInstance()
        {
            if(outputForm==null) return outputForm = new FrmOutput();
            else return FrmOutput.outputForm;   
        }

        public FrmOutput()
        {
            InitializeComponent();
            ShowToolTip();
            
        }

        private void ShowToolTip()
        {
            // set tool tip to NAICS codes
            ToolTip tp = new ToolTip();

            tp.SetToolTip(lbl11, "Agriculture, Forestry, Fishing and Hunting");
            tp.SetToolTip(lbl21, "Mining, Quarrying, and Oil and Gas Extraction");
            tp.SetToolTip(lbl22, "Utilities");
            tp.SetToolTip(lbl23, "Construction");
            tp.SetToolTip(lbl3133, "Manufacturing");
            tp.SetToolTip(lbl42, "Wholesale Trade");
            tp.SetToolTip(lbl4445, "Retail Trade");
            tp.SetToolTip(lbl4849, "Transportation and Warehousing");
            tp.SetToolTip(lbl51, "Information");
            tp.SetToolTip(lbl52, "Finance and Insurance");
            tp.SetToolTip(lbl53, "Real Estate and Rental and Leasing");
            tp.SetToolTip(lbl54, "Professional, Scientific, and Technical Services");
            tp.SetToolTip(lbl55, "Management of Companies and Enterprises");
            tp.SetToolTip(lbl56, "Administrative and Support and Waste Management and Remediation Services");
            tp.SetToolTip(lbl61, "Educational Services");
            tp.SetToolTip(lbl62, "Health Care and Social Assistance");
            tp.SetToolTip(lbl71, "Arts, Entertainment, and Recreation");
            tp.SetToolTip(lbl72, "Accommodation and Food Services");
            tp.SetToolTip(lbl81, "Other Services [except Public Administration]");
            tp.SetToolTip(lbl92, "Public Administration");

        }

        public string[] GetDaySimEmplCatsNames()
        {
            daySimCats = new string[numDaySimCats];

            int i = 1;
            // iterates from the last item
            foreach (var textbox in gbEmplCats.Controls.OfType<TextBox>())
            {

                int tabindex = textbox.TabIndex;

                if (tabindex >= 1 && tabindex <= numDaySimCats)
                {
                    daySimCats[numDaySimCats - i] = textbox.Text;
                    i++;
                }

            }

            return daySimCats;
        }

        public int[,] GetDaySimEmplCatsNaics()
        {
            daySimCatsNaics = new int[numDaySimCats, 20];

            int j = 1;

            foreach (var checkedlistbox in gbEmplCats.Controls.OfType<CheckedListBox>())
            {
                int tabindex = checkedlistbox.TabIndex;

                if (tabindex >= 11 && tabindex <= 10 + numDaySimCats)
                {
                    for (int code = 0; code < 20; code++)
                    {
                        if (checkedlistbox.GetItemChecked(code)) daySimCatsNaics[numDaySimCats - j, code] = 1;
                    }

                    j++;
                }

            }

            return daySimCatsNaics;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            numDaySimCats = (int)udNumEmplCats.Value;

            foreach (var textbox in gbEmplCats.Controls.OfType<TextBox>())
            {
                int tabindex = textbox.TabIndex;

                // text box index: 1 to 10
                if (tabindex >= 1 && tabindex <= numDaySimCats) textbox.Visible = true;

            }

            foreach (var checkedlistbox in gbEmplCats.Controls.OfType<CheckedListBox>())
            {
                int tabindex = checkedlistbox.TabIndex;

                // checked list box index: 11 to 20
                if (tabindex >= 11 && tabindex <= 10+numDaySimCats) checkedlistbox.Visible = true;

            }
        
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // check NAICS Codes
            numDaySimCats = (int)udNumEmplCats.Value;
            daySimCatsNaics = GetDaySimEmplCatsNaics();

            string Error = ValidateEmploymentCodes.Validate(numDaySimCats, daySimCatsNaics, "NAICS");

            if (Error.Length != 0)
            {
                MessageBox.Show(Error);
                return;
            }

            this.Hide();
        }

        public void ShowXmlInputs(int numEmplCats, string[] daySimCats, int[,] daySimCatsNaics)
        {
            udNumEmplCats.Value = numEmplCats;
            for (int cat = 0; cat < numEmplCats; cat++)
            {
                foreach (var textbox in gbEmplCats.Controls.OfType<TextBox>())
                {
                    int tabindex = textbox.TabIndex;

                    // text box index: 1 to 10
                    if (tabindex >= 1 && tabindex <= numEmplCats)
                    {
                        textbox.Visible = true;
                        textbox.Text = daySimCats[tabindex - 1];
                    }

                }

                foreach (var checkedlistbox in gbEmplCats.Controls.OfType<CheckedListBox>())
                {
                    int tabindex = checkedlistbox.TabIndex;

                    // checked list box index: 11 to 20
                    if (tabindex >= 11 && tabindex <= 10+numEmplCats)
                    {
                        checkedlistbox.Visible = true;
                        for (int code = 0; code < 20; code++)
                        {
                            if (daySimCatsNaics[tabindex - 11, code] == 1) checkedlistbox.SetItemChecked(code, true); // deduct 11 (10+1) because tabindex starts from 11. 
                        }

                    }
                }

            }

        }

    }
}
