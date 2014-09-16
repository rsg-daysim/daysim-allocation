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
    public partial class FrmSic : Form
    {
        private static FrmSic sicForm;
        private static int nEmplCats;
        public static FrmSic GetInstance()
        {
            if (sicForm == null) return sicForm = new FrmSic();
            else return FrmSic.sicForm;

        }
        public FrmSic()
        {
            InitializeComponent();
            ShowToolTip();
        }

        private void ShowToolTip()
        {
            // set tool tip to SIC codes
            ToolTip tp = new ToolTip();

            tp.SetToolTip(lbl11, "Agriculture, Forestry & Fishing");
            tp.SetToolTip(lbl21, "Mining");
            tp.SetToolTip(lbl22, "Construction");
            tp.SetToolTip(lbl23, "Manufacturing");
            tp.SetToolTip(lbl3133, "Transportation, Communications & Public Utilities");
            tp.SetToolTip(lbl42, "Wholesale Trade");
            tp.SetToolTip(lbl4445, "Retail Trade");
            tp.SetToolTip(lbl4849, "Finance, Insurance & Real Estate");
            tp.SetToolTip(lbl51, "Information");
            tp.SetToolTip(lbl52, "Services");
        }

        public int[,] GetSicCodes(int numEmplCats)
        {
            int[,] sicCodes = new int[numEmplCats, 10];

            int j = 1;

            foreach (var checkedlistbox in this.Controls.OfType<CheckedListBox>())
            {
                
                int tabindex = checkedlistbox.TabIndex;

                if (tabindex >= 11 && tabindex <= 10 + numEmplCats)
                {
                    for (int code = 0; code < 10; code++)
                    {
                        if (checkedlistbox.GetItemChecked(code)) sicCodes[numEmplCats - j, code] = 1;
                    }

                    j++;
                }
            }

            return sicCodes;
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

            // checked list boxes
            foreach (var checkedlistbox in this.Controls.OfType<CheckedListBox>())
            {
                int tabindex = checkedlistbox.TabIndex;

                // text box index: 11 to 20
                if (tabindex >= 11 && tabindex <= 10 + numEmplCats) checkedlistbox.Visible = true;

            }

            nEmplCats = numEmplCats;

        }

        public void ShowXmlInputs(int numEmplCats, string[] emplCats, int[,] sicCodes)
        {

            foreach (var checkedlistbox in this.Controls.OfType<CheckedListBox>())
            {
                int tabindex = checkedlistbox.TabIndex;

                // checked list box index: 11 to 20
                if (tabindex >= 11 && tabindex <= 10 + numEmplCats)
                {
                    checkedlistbox.Visible = true;
                    for (int code = 0; code < 10; code++)
                    {
                        if (sicCodes[tabindex - 11, code] == 1) checkedlistbox.SetItemChecked(code, true);
                    }
                }

            }

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // check SIC Codes
            int[,] emplCodes = new int[nEmplCats, 10];
            emplCodes = GetSicCodes(nEmplCats);

            string Error = ValidateEmploymentCodes.Validate(nEmplCats, emplCodes, "SIC");

            if (Error.Length != 0)
            {
                MessageBox.Show(Error);
                return;
            }

            this.Hide();
            
        }

    }
}
