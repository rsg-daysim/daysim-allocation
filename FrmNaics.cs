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
    public partial class FrmNaics : Form
    {
        private static FrmNaics naicsForm;

        private static int nEmplCats;

        public static FrmNaics GetInstance()
        {
            if (naicsForm == null) return naicsForm = new FrmNaics();
            else return FrmNaics.naicsForm;   
        }

        public FrmNaics()
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
        
        public int[,] GetNaicsCodes(int numEmplCats)
        {
            int[,] naicsCodes = new int[numEmplCats, 20];

            int j = 1;
            foreach (var checkedlistbox in this.Controls.OfType<CheckedListBox>())
            {
                int tabindex = checkedlistbox.TabIndex;

                if (tabindex >= 11 && tabindex <= 10 + numEmplCats)
                {
                    for (int code = 0; code < 20; code++)
                    {
                        if (checkedlistbox.GetItemChecked(code)) naicsCodes[numEmplCats - j, code] = 1;
                    }

                    j++;
                }
            }

            //emplCodes = naicsCodes;
            return naicsCodes;
        }

        public void ShowEmplFields(int numEmplCats, string[] emplCats)
        {
            // text boxes
            foreach (var textbox in this.Controls.OfType<TextBox>())
            {
                int tabindex = textbox.TabIndex;

                // text box index: 1 to 10
                if (tabindex >= 1 && tabindex <= numEmplCats)
                {
                    textbox.Visible = true;
                    textbox.Text = emplCats[tabindex - 1].ToString();
                }

            }

            // show checked list boxes
            foreach (var checkedlistbox in this.Controls.OfType<CheckedListBox>())
            {
                int tabindex = checkedlistbox.TabIndex;

                // text box index: 11 to 20
                if (tabindex >= 11 && tabindex <= 10 + numEmplCats) checkedlistbox.Visible = true;

            }
            nEmplCats = numEmplCats;
        }

        public void ShowXmlInputs(int numEmplCats, string[] emplCats, int[,] naicsCodes)
        {

            foreach (var checkedlistbox in this.Controls.OfType<CheckedListBox>())
            {
                int tabindex = checkedlistbox.TabIndex;

                // checked list box index: 11 to 20
                if (tabindex >= 11 && tabindex <= 10 + numEmplCats)
                {
                    checkedlistbox.Visible = true;
                    for (int code = 0; code < 20; code++)
                    {
                        if (naicsCodes[tabindex-11, code] == 1) checkedlistbox.SetItemChecked(code, true);
                    }
                }

            }

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // check NAICS Codes
            int[,] emplCodes = new int[nEmplCats,20];
            emplCodes = GetNaicsCodes(nEmplCats);

            string Error = ValidateEmploymentCodes.Validate(nEmplCats, emplCodes, "NAICS");

            if (Error.Length != 0)
            {
                MessageBox.Show(Error);
                return;
            }

            this.Hide();
        }


    } // end of class

} // end of namespace
