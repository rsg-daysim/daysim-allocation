namespace DisaggregationTool
{
    partial class FrmInput
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmdRun = new System.Windows.Forms.Button();
            this.lblBlock = new System.Windows.Forms.Label();
            this.lblMZ = new System.Windows.Forms.Label();
            this.lblSchool = new System.Windows.Forms.Label();
            this.lblOutput = new System.Windows.Forms.Label();
            this.txtBlockFile = new System.Windows.Forms.TextBox();
            this.txtMzFile = new System.Windows.Forms.TextBox();
            this.txtSchoolFile = new System.Windows.Forms.TextBox();
            this.txtOutputFile = new System.Windows.Forms.TextBox();
            this.btnBrowseBlocks = new System.Windows.Forms.Button();
            this.btnBrowseMZ = new System.Windows.Forms.Button();
            this.btnBrowseSchool = new System.Windows.Forms.Button();
            this.btnBrowseOutput = new System.Windows.Forms.Button();
            this.lblTaz = new System.Windows.Forms.Label();
            this.txtTazFile = new System.Windows.Forms.TextBox();
            this.clbEmplCats = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowseTaz = new System.Windows.Forms.Button();
            this.gbInputs = new System.Windows.Forms.GroupBox();
            this.cbSectorCorrespondenceHeader = new System.Windows.Forms.CheckBox();
            this.btnBrowseSectorCorrespondence = new System.Windows.Forms.Button();
            this.txtSectorCorrespondenceFile = new System.Windows.Forms.TextBox();
            this.lblSectorCorrespondenceFile = new System.Windows.Forms.Label();
            this.cbCorrespondenceHeader = new System.Windows.Forms.CheckBox();
            this.btnBrowseCorrespondence = new System.Windows.Forms.Button();
            this.txtDriCorrespondenceFile = new System.Windows.Forms.TextBox();
            this.lblDriCorrespondenceFile = new System.Windows.Forms.Label();
            this.cbParcelBaseHeader = new System.Windows.Forms.CheckBox();
            this.cbTazForecastHeader = new System.Windows.Forms.CheckBox();
            this.btnBrowseParcelBase = new System.Windows.Forms.Button();
            this.btnBrowseTazForecast = new System.Windows.Forms.Button();
            this.txtParcelBaseFile = new System.Windows.Forms.TextBox();
            this.txtTazForecastFile = new System.Windows.Forms.TextBox();
            this.lblParcelBaseFile = new System.Windows.Forms.Label();
            this.lblTazForecastFile = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rdbSic = new System.Windows.Forms.RadioButton();
            this.rdbNaics = new System.Windows.Forms.RadioButton();
            this.cbTazHeader = new System.Windows.Forms.CheckBox();
            this.cbSchoolHeader = new System.Windows.Forms.CheckBox();
            this.cbMZHeader = new System.Windows.Forms.CheckBox();
            this.cbBlockHeader = new System.Windows.Forms.CheckBox();
            this.btnEmplCatsOk = new System.Windows.Forms.Button();
            this.btnReadXml = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtParcelForecastFile = new System.Windows.Forms.TextBox();
            this.lblParcelForecastFile = new System.Windows.Forms.Label();
            this.btnBrowseParcelForecast = new System.Windows.Forms.Button();
            this.btnOutputForm = new System.Windows.Forms.Button();
            this.gbReadXml = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdbMultiYears = new System.Windows.Forms.RadioButton();
            this.rdbOneYear = new System.Windows.Forms.RadioButton();
            this.cbTazForecastHeader1 = new System.Windows.Forms.CheckBox();
            this.btnBrowseTazForecast1 = new System.Windows.Forms.Button();
            this.txtTazForecastFile1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.gbInputs.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gbReadXml.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdRun
            // 
            this.cmdRun.Location = new System.Drawing.Point(331, 837);
            this.cmdRun.Name = "cmdRun";
            this.cmdRun.Size = new System.Drawing.Size(155, 42);
            this.cmdRun.TabIndex = 0;
            this.cmdRun.Text = "RUN";
            this.cmdRun.UseVisualStyleBackColor = true;
            this.cmdRun.Click += new System.EventHandler(this.cmdRun_Click);
            // 
            // lblBlock
            // 
            this.lblBlock.AutoSize = true;
            this.lblBlock.Location = new System.Drawing.Point(57, 278);
            this.lblBlock.Name = "lblBlock";
            this.lblBlock.Size = new System.Drawing.Size(53, 13);
            this.lblBlock.TabIndex = 2;
            this.lblBlock.Text = "Block File";
            // 
            // lblMZ
            // 
            this.lblMZ.AutoSize = true;
            this.lblMZ.Location = new System.Drawing.Point(57, 313);
            this.lblMZ.Name = "lblMZ";
            this.lblMZ.Size = new System.Drawing.Size(128, 13);
            this.lblMZ.TabIndex = 3;
            this.lblMZ.Text = "Micro-Zone Geomtery File";
            // 
            // lblSchool
            // 
            this.lblSchool.AutoSize = true;
            this.lblSchool.Location = new System.Drawing.Point(57, 354);
            this.lblSchool.Name = "lblSchool";
            this.lblSchool.Size = new System.Drawing.Size(59, 13);
            this.lblSchool.TabIndex = 4;
            this.lblSchool.Text = "School File";
            // 
            // lblOutput
            // 
            this.lblOutput.AutoSize = true;
            this.lblOutput.Location = new System.Drawing.Point(61, 79);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(58, 13);
            this.lblOutput.TabIndex = 5;
            this.lblOutput.Text = "Output File";
            // 
            // txtBlockFile
            // 
            this.txtBlockFile.Location = new System.Drawing.Point(218, 271);
            this.txtBlockFile.Name = "txtBlockFile";
            this.txtBlockFile.Size = new System.Drawing.Size(340, 20);
            this.txtBlockFile.TabIndex = 7;
            // 
            // txtMzFile
            // 
            this.txtMzFile.Location = new System.Drawing.Point(218, 309);
            this.txtMzFile.Name = "txtMzFile";
            this.txtMzFile.Size = new System.Drawing.Size(340, 20);
            this.txtMzFile.TabIndex = 8;
            // 
            // txtSchoolFile
            // 
            this.txtSchoolFile.Location = new System.Drawing.Point(218, 346);
            this.txtSchoolFile.Name = "txtSchoolFile";
            this.txtSchoolFile.Size = new System.Drawing.Size(340, 20);
            this.txtSchoolFile.TabIndex = 9;
            // 
            // txtOutputFile
            // 
            this.txtOutputFile.Location = new System.Drawing.Point(222, 78);
            this.txtOutputFile.Name = "txtOutputFile";
            this.txtOutputFile.Size = new System.Drawing.Size(340, 20);
            this.txtOutputFile.TabIndex = 10;
            // 
            // btnBrowseBlocks
            // 
            this.btnBrowseBlocks.Location = new System.Drawing.Point(580, 270);
            this.btnBrowseBlocks.Name = "btnBrowseBlocks";
            this.btnBrowseBlocks.Size = new System.Drawing.Size(24, 19);
            this.btnBrowseBlocks.TabIndex = 13;
            this.btnBrowseBlocks.Text = "....";
            this.btnBrowseBlocks.UseVisualStyleBackColor = true;
            this.btnBrowseBlocks.Click += new System.EventHandler(this.btnBrowseBlocks_Click);
            // 
            // btnBrowseMZ
            // 
            this.btnBrowseMZ.Location = new System.Drawing.Point(580, 309);
            this.btnBrowseMZ.Name = "btnBrowseMZ";
            this.btnBrowseMZ.Size = new System.Drawing.Size(24, 19);
            this.btnBrowseMZ.TabIndex = 14;
            this.btnBrowseMZ.Text = "....";
            this.btnBrowseMZ.UseVisualStyleBackColor = true;
            this.btnBrowseMZ.Click += new System.EventHandler(this.btnBrowseMZ_Click);
            // 
            // btnBrowseSchool
            // 
            this.btnBrowseSchool.Location = new System.Drawing.Point(580, 345);
            this.btnBrowseSchool.Name = "btnBrowseSchool";
            this.btnBrowseSchool.Size = new System.Drawing.Size(24, 19);
            this.btnBrowseSchool.TabIndex = 15;
            this.btnBrowseSchool.Text = "....";
            this.btnBrowseSchool.UseVisualStyleBackColor = true;
            this.btnBrowseSchool.Click += new System.EventHandler(this.btnBrowseSchool_Click);
            // 
            // btnBrowseOutput
            // 
            this.btnBrowseOutput.Location = new System.Drawing.Point(584, 78);
            this.btnBrowseOutput.Name = "btnBrowseOutput";
            this.btnBrowseOutput.Size = new System.Drawing.Size(24, 19);
            this.btnBrowseOutput.TabIndex = 16;
            this.btnBrowseOutput.Text = "....";
            this.btnBrowseOutput.UseVisualStyleBackColor = true;
            this.btnBrowseOutput.Click += new System.EventHandler(this.btnBrowseOutput_Click);
            // 
            // lblTaz
            // 
            this.lblTaz.AutoSize = true;
            this.lblTaz.Location = new System.Drawing.Point(144, 22);
            this.lblTaz.Name = "lblTaz";
            this.lblTaz.Size = new System.Drawing.Size(47, 13);
            this.lblTaz.TabIndex = 1;
            this.lblTaz.Text = "TAZ File";
            // 
            // txtTazFile
            // 
            this.txtTazFile.Location = new System.Drawing.Point(221, 15);
            this.txtTazFile.Name = "txtTazFile";
            this.txtTazFile.Size = new System.Drawing.Size(341, 20);
            this.txtTazFile.TabIndex = 6;
            // 
            // clbEmplCats
            // 
            this.clbEmplCats.Enabled = false;
            this.clbEmplCats.FormattingEnabled = true;
            this.clbEmplCats.Location = new System.Drawing.Point(221, 74);
            this.clbEmplCats.Name = "clbEmplCats";
            this.clbEmplCats.Size = new System.Drawing.Size(337, 139);
            this.clbEmplCats.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(144, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Select Employment Categories";
            // 
            // btnBrowseTaz
            // 
            this.btnBrowseTaz.Location = new System.Drawing.Point(583, 15);
            this.btnBrowseTaz.Name = "btnBrowseTaz";
            this.btnBrowseTaz.Size = new System.Drawing.Size(24, 19);
            this.btnBrowseTaz.TabIndex = 14;
            this.btnBrowseTaz.Text = "....";
            this.btnBrowseTaz.UseVisualStyleBackColor = true;
            this.btnBrowseTaz.Click += new System.EventHandler(this.btnBrowseTaz_Click);
            // 
            // gbInputs
            // 
            this.gbInputs.Controls.Add(this.cbSectorCorrespondenceHeader);
            this.gbInputs.Controls.Add(this.btnBrowseSectorCorrespondence);
            this.gbInputs.Controls.Add(this.txtSectorCorrespondenceFile);
            this.gbInputs.Controls.Add(this.lblSectorCorrespondenceFile);
            this.gbInputs.Controls.Add(this.cbCorrespondenceHeader);
            this.gbInputs.Controls.Add(this.btnBrowseCorrespondence);
            this.gbInputs.Controls.Add(this.txtDriCorrespondenceFile);
            this.gbInputs.Controls.Add(this.lblDriCorrespondenceFile);
            this.gbInputs.Controls.Add(this.cbParcelBaseHeader);
            this.gbInputs.Controls.Add(this.cbTazForecastHeader);
            this.gbInputs.Controls.Add(this.btnBrowseParcelBase);
            this.gbInputs.Controls.Add(this.btnBrowseTazForecast);
            this.gbInputs.Controls.Add(this.txtParcelBaseFile);
            this.gbInputs.Controls.Add(this.txtTazForecastFile);
            this.gbInputs.Controls.Add(this.lblParcelBaseFile);
            this.gbInputs.Controls.Add(this.lblTazForecastFile);
            this.gbInputs.Controls.Add(this.label2);
            this.gbInputs.Controls.Add(this.rdbSic);
            this.gbInputs.Controls.Add(this.rdbNaics);
            this.gbInputs.Controls.Add(this.cbTazHeader);
            this.gbInputs.Controls.Add(this.cbSchoolHeader);
            this.gbInputs.Controls.Add(this.cbMZHeader);
            this.gbInputs.Controls.Add(this.cbBlockHeader);
            this.gbInputs.Controls.Add(this.btnBrowseSchool);
            this.gbInputs.Controls.Add(this.btnBrowseMZ);
            this.gbInputs.Controls.Add(this.btnBrowseBlocks);
            this.gbInputs.Controls.Add(this.txtSchoolFile);
            this.gbInputs.Controls.Add(this.txtMzFile);
            this.gbInputs.Controls.Add(this.txtBlockFile);
            this.gbInputs.Controls.Add(this.lblSchool);
            this.gbInputs.Controls.Add(this.lblMZ);
            this.gbInputs.Controls.Add(this.lblBlock);
            this.gbInputs.Controls.Add(this.btnEmplCatsOk);
            this.gbInputs.Controls.Add(this.btnBrowseTaz);
            this.gbInputs.Controls.Add(this.label1);
            this.gbInputs.Controls.Add(this.clbEmplCats);
            this.gbInputs.Controls.Add(this.txtTazFile);
            this.gbInputs.Controls.Add(this.lblTaz);
            this.gbInputs.Location = new System.Drawing.Point(12, 101);
            this.gbInputs.Name = "gbInputs";
            this.gbInputs.Size = new System.Drawing.Size(792, 559);
            this.gbInputs.TabIndex = 12;
            this.gbInputs.TabStop = false;
            this.gbInputs.Text = "INPUT";
            // 
            // cbSectorCorrespondenceHeader
            // 
            this.cbSectorCorrespondenceHeader.AutoSize = true;
            this.cbSectorCorrespondenceHeader.Location = new System.Drawing.Point(633, 522);
            this.cbSectorCorrespondenceHeader.Name = "cbSectorCorrespondenceHeader";
            this.cbSectorCorrespondenceHeader.Size = new System.Drawing.Size(61, 17);
            this.cbSectorCorrespondenceHeader.TabIndex = 73;
            this.cbSectorCorrespondenceHeader.Text = "Header";
            this.cbSectorCorrespondenceHeader.UseVisualStyleBackColor = true;
            // 
            // btnBrowseSectorCorrespondence
            // 
            this.btnBrowseSectorCorrespondence.Location = new System.Drawing.Point(582, 522);
            this.btnBrowseSectorCorrespondence.Name = "btnBrowseSectorCorrespondence";
            this.btnBrowseSectorCorrespondence.Size = new System.Drawing.Size(24, 19);
            this.btnBrowseSectorCorrespondence.TabIndex = 72;
            this.btnBrowseSectorCorrespondence.Text = "....";
            this.btnBrowseSectorCorrespondence.UseVisualStyleBackColor = true;
            this.btnBrowseSectorCorrespondence.Click += new System.EventHandler(this.btnBrowseSectorCorrespondence_Click);
            // 
            // txtSectorCorrespondenceFile
            // 
            this.txtSectorCorrespondenceFile.Location = new System.Drawing.Point(220, 523);
            this.txtSectorCorrespondenceFile.Name = "txtSectorCorrespondenceFile";
            this.txtSectorCorrespondenceFile.Size = new System.Drawing.Size(340, 20);
            this.txtSectorCorrespondenceFile.TabIndex = 71;
            // 
            // lblSectorCorrespondenceFile
            // 
            this.lblSectorCorrespondenceFile.AutoSize = true;
            this.lblSectorCorrespondenceFile.Location = new System.Drawing.Point(59, 531);
            this.lblSectorCorrespondenceFile.Name = "lblSectorCorrespondenceFile";
            this.lblSectorCorrespondenceFile.Size = new System.Drawing.Size(138, 13);
            this.lblSectorCorrespondenceFile.TabIndex = 70;
            this.lblSectorCorrespondenceFile.Text = "Sector Correspondence File";
            // 
            // cbCorrespondenceHeader
            // 
            this.cbCorrespondenceHeader.AutoSize = true;
            this.cbCorrespondenceHeader.Location = new System.Drawing.Point(631, 485);
            this.cbCorrespondenceHeader.Name = "cbCorrespondenceHeader";
            this.cbCorrespondenceHeader.Size = new System.Drawing.Size(61, 17);
            this.cbCorrespondenceHeader.TabIndex = 69;
            this.cbCorrespondenceHeader.Text = "Header";
            this.cbCorrespondenceHeader.UseVisualStyleBackColor = true;
            // 
            // btnBrowseCorrespondence
            // 
            this.btnBrowseCorrespondence.Location = new System.Drawing.Point(580, 485);
            this.btnBrowseCorrespondence.Name = "btnBrowseCorrespondence";
            this.btnBrowseCorrespondence.Size = new System.Drawing.Size(24, 19);
            this.btnBrowseCorrespondence.TabIndex = 68;
            this.btnBrowseCorrespondence.Text = "....";
            this.btnBrowseCorrespondence.UseVisualStyleBackColor = true;
            this.btnBrowseCorrespondence.Click += new System.EventHandler(this.btnBrowseCorrespondence_Click);
            // 
            // txtDriCorrespondenceFile
            // 
            this.txtDriCorrespondenceFile.Location = new System.Drawing.Point(218, 486);
            this.txtDriCorrespondenceFile.Name = "txtDriCorrespondenceFile";
            this.txtDriCorrespondenceFile.Size = new System.Drawing.Size(340, 20);
            this.txtDriCorrespondenceFile.TabIndex = 67;
            // 
            // lblDriCorrespondenceFile
            // 
            this.lblDriCorrespondenceFile.AutoSize = true;
            this.lblDriCorrespondenceFile.Location = new System.Drawing.Point(57, 494);
            this.lblDriCorrespondenceFile.Name = "lblDriCorrespondenceFile";
            this.lblDriCorrespondenceFile.Size = new System.Drawing.Size(126, 13);
            this.lblDriCorrespondenceFile.TabIndex = 66;
            this.lblDriCorrespondenceFile.Text = "DRI Correspondence File";
            // 
            // cbParcelBaseHeader
            // 
            this.cbParcelBaseHeader.AutoSize = true;
            this.cbParcelBaseHeader.Location = new System.Drawing.Point(631, 446);
            this.cbParcelBaseHeader.Name = "cbParcelBaseHeader";
            this.cbParcelBaseHeader.Size = new System.Drawing.Size(61, 17);
            this.cbParcelBaseHeader.TabIndex = 65;
            this.cbParcelBaseHeader.Text = "Header";
            this.cbParcelBaseHeader.UseVisualStyleBackColor = true;
            // 
            // cbTazForecastHeader
            // 
            this.cbTazForecastHeader.AutoSize = true;
            this.cbTazForecastHeader.Location = new System.Drawing.Point(631, 380);
            this.cbTazForecastHeader.Name = "cbTazForecastHeader";
            this.cbTazForecastHeader.Size = new System.Drawing.Size(61, 17);
            this.cbTazForecastHeader.TabIndex = 64;
            this.cbTazForecastHeader.Text = "Header";
            this.cbTazForecastHeader.UseVisualStyleBackColor = true;
            // 
            // btnBrowseParcelBase
            // 
            this.btnBrowseParcelBase.Location = new System.Drawing.Point(580, 446);
            this.btnBrowseParcelBase.Name = "btnBrowseParcelBase";
            this.btnBrowseParcelBase.Size = new System.Drawing.Size(24, 19);
            this.btnBrowseParcelBase.TabIndex = 62;
            this.btnBrowseParcelBase.Text = "....";
            this.btnBrowseParcelBase.UseVisualStyleBackColor = true;
            this.btnBrowseParcelBase.Click += new System.EventHandler(this.btnBrowseParcelBase_Click);
            // 
            // btnBrowseTazForecast
            // 
            this.btnBrowseTazForecast.Location = new System.Drawing.Point(580, 380);
            this.btnBrowseTazForecast.Name = "btnBrowseTazForecast";
            this.btnBrowseTazForecast.Size = new System.Drawing.Size(24, 19);
            this.btnBrowseTazForecast.TabIndex = 61;
            this.btnBrowseTazForecast.Text = "....";
            this.btnBrowseTazForecast.UseVisualStyleBackColor = true;
            this.btnBrowseTazForecast.Click += new System.EventHandler(this.btnBrowseTazForecast_Click);
            // 
            // txtParcelBaseFile
            // 
            this.txtParcelBaseFile.Location = new System.Drawing.Point(218, 447);
            this.txtParcelBaseFile.Name = "txtParcelBaseFile";
            this.txtParcelBaseFile.Size = new System.Drawing.Size(340, 20);
            this.txtParcelBaseFile.TabIndex = 59;
            // 
            // txtTazForecastFile
            // 
            this.txtTazForecastFile.Location = new System.Drawing.Point(218, 380);
            this.txtTazForecastFile.Name = "txtTazForecastFile";
            this.txtTazForecastFile.Size = new System.Drawing.Size(340, 20);
            this.txtTazForecastFile.TabIndex = 58;
            // 
            // lblParcelBaseFile
            // 
            this.lblParcelBaseFile.AutoSize = true;
            this.lblParcelBaseFile.Location = new System.Drawing.Point(57, 455);
            this.lblParcelBaseFile.Name = "lblParcelBaseFile";
            this.lblParcelBaseFile.Size = new System.Drawing.Size(108, 13);
            this.lblParcelBaseFile.TabIndex = 56;
            this.lblParcelBaseFile.Text = "Base Year Parcel File";
            // 
            // lblTazForecastFile
            // 
            this.lblTazForecastFile.AutoSize = true;
            this.lblTazForecastFile.Location = new System.Drawing.Point(57, 384);
            this.lblTazForecastFile.Name = "lblTazForecastFile";
            this.lblTazForecastFile.Size = new System.Drawing.Size(125, 13);
            this.lblTazForecastFile.TabIndex = 55;
            this.lblTazForecastFile.Text = "Forecast Year TAZ File 2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(599, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(165, 13);
            this.label2.TabIndex = 53;
            this.label2.Text = "Employment Classification System";
            // 
            // rdbSic
            // 
            this.rdbSic.AutoSize = true;
            this.rdbSic.Location = new System.Drawing.Point(624, 157);
            this.rdbSic.Name = "rdbSic";
            this.rdbSic.Size = new System.Drawing.Size(42, 17);
            this.rdbSic.TabIndex = 52;
            this.rdbSic.TabStop = true;
            this.rdbSic.Text = "SIC";
            this.rdbSic.UseVisualStyleBackColor = true;
            // 
            // rdbNaics
            // 
            this.rdbNaics.AutoSize = true;
            this.rdbNaics.Checked = true;
            this.rdbNaics.Location = new System.Drawing.Point(624, 133);
            this.rdbNaics.Name = "rdbNaics";
            this.rdbNaics.Size = new System.Drawing.Size(57, 17);
            this.rdbNaics.TabIndex = 51;
            this.rdbNaics.TabStop = true;
            this.rdbNaics.Text = "NAICS";
            this.rdbNaics.UseVisualStyleBackColor = true;
            // 
            // cbTazHeader
            // 
            this.cbTazHeader.AutoSize = true;
            this.cbTazHeader.Location = new System.Drawing.Point(635, 17);
            this.cbTazHeader.Name = "cbTazHeader";
            this.cbTazHeader.Size = new System.Drawing.Size(61, 17);
            this.cbTazHeader.TabIndex = 50;
            this.cbTazHeader.Text = "Header";
            this.cbTazHeader.UseVisualStyleBackColor = true;
            // 
            // cbSchoolHeader
            // 
            this.cbSchoolHeader.AutoSize = true;
            this.cbSchoolHeader.Location = new System.Drawing.Point(631, 345);
            this.cbSchoolHeader.Name = "cbSchoolHeader";
            this.cbSchoolHeader.Size = new System.Drawing.Size(61, 17);
            this.cbSchoolHeader.TabIndex = 19;
            this.cbSchoolHeader.Text = "Header";
            this.cbSchoolHeader.UseVisualStyleBackColor = true;
            // 
            // cbMZHeader
            // 
            this.cbMZHeader.AutoSize = true;
            this.cbMZHeader.Location = new System.Drawing.Point(631, 309);
            this.cbMZHeader.Name = "cbMZHeader";
            this.cbMZHeader.Size = new System.Drawing.Size(61, 17);
            this.cbMZHeader.TabIndex = 18;
            this.cbMZHeader.Text = "Header";
            this.cbMZHeader.UseVisualStyleBackColor = true;
            // 
            // cbBlockHeader
            // 
            this.cbBlockHeader.AutoSize = true;
            this.cbBlockHeader.Location = new System.Drawing.Point(631, 271);
            this.cbBlockHeader.Name = "cbBlockHeader";
            this.cbBlockHeader.Size = new System.Drawing.Size(61, 17);
            this.cbBlockHeader.TabIndex = 17;
            this.cbBlockHeader.Text = "Header";
            this.cbBlockHeader.UseVisualStyleBackColor = true;
            // 
            // btnEmplCatsOk
            // 
            this.btnEmplCatsOk.Enabled = false;
            this.btnEmplCatsOk.Location = new System.Drawing.Point(362, 219);
            this.btnEmplCatsOk.Name = "btnEmplCatsOk";
            this.btnEmplCatsOk.Size = new System.Drawing.Size(70, 28);
            this.btnEmplCatsOk.TabIndex = 17;
            this.btnEmplCatsOk.Text = "OK";
            this.btnEmplCatsOk.UseVisualStyleBackColor = true;
            this.btnEmplCatsOk.Click += new System.EventHandler(this.btnEmplCatsOk_Click);
            // 
            // btnReadXml
            // 
            this.btnReadXml.Location = new System.Drawing.Point(60, 19);
            this.btnReadXml.Name = "btnReadXml";
            this.btnReadXml.Size = new System.Drawing.Size(155, 42);
            this.btnReadXml.TabIndex = 29;
            this.btnReadXml.Text = "Read XML Input File";
            this.btnReadXml.UseVisualStyleBackColor = true;
            this.btnReadXml.Click += new System.EventHandler(this.btnReadXML_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtParcelForecastFile);
            this.groupBox2.Controls.Add(this.lblParcelForecastFile);
            this.groupBox2.Controls.Add(this.btnBrowseParcelForecast);
            this.groupBox2.Controls.Add(this.btnOutputForm);
            this.groupBox2.Controls.Add(this.txtOutputFile);
            this.groupBox2.Controls.Add(this.lblOutput);
            this.groupBox2.Controls.Add(this.btnBrowseOutput);
            this.groupBox2.Location = new System.Drawing.Point(12, 672);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(792, 153);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "OUTPUT";
            // 
            // txtParcelForecastFile
            // 
            this.txtParcelForecastFile.Location = new System.Drawing.Point(223, 114);
            this.txtParcelForecastFile.Name = "txtParcelForecastFile";
            this.txtParcelForecastFile.Size = new System.Drawing.Size(340, 20);
            this.txtParcelForecastFile.TabIndex = 19;
            // 
            // lblParcelForecastFile
            // 
            this.lblParcelForecastFile.AutoSize = true;
            this.lblParcelForecastFile.Location = new System.Drawing.Point(62, 115);
            this.lblParcelForecastFile.Name = "lblParcelForecastFile";
            this.lblParcelForecastFile.Size = new System.Drawing.Size(125, 13);
            this.lblParcelForecastFile.TabIndex = 18;
            this.lblParcelForecastFile.Text = "Forecast Year Parcel File";
            // 
            // btnBrowseParcelForecast
            // 
            this.btnBrowseParcelForecast.Location = new System.Drawing.Point(585, 114);
            this.btnBrowseParcelForecast.Name = "btnBrowseParcelForecast";
            this.btnBrowseParcelForecast.Size = new System.Drawing.Size(24, 19);
            this.btnBrowseParcelForecast.TabIndex = 20;
            this.btnBrowseParcelForecast.Text = "....";
            this.btnBrowseParcelForecast.UseVisualStyleBackColor = true;
            this.btnBrowseParcelForecast.Click += new System.EventHandler(this.btnBrowseParcelForecast_Click);
            // 
            // btnOutputForm
            // 
            this.btnOutputForm.Location = new System.Drawing.Point(308, 20);
            this.btnOutputForm.Name = "btnOutputForm";
            this.btnOutputForm.Size = new System.Drawing.Size(167, 42);
            this.btnOutputForm.TabIndex = 17;
            this.btnOutputForm.Text = "Select Employment Categories";
            this.btnOutputForm.UseVisualStyleBackColor = true;
            this.btnOutputForm.Click += new System.EventHandler(this.btnOutputForm_Click);
            // 
            // gbReadXml
            // 
            this.gbReadXml.Controls.Add(this.btnReadXml);
            this.gbReadXml.Location = new System.Drawing.Point(488, 24);
            this.gbReadXml.Name = "gbReadXml";
            this.gbReadXml.Size = new System.Drawing.Size(254, 71);
            this.gbReadXml.TabIndex = 54;
            this.gbReadXml.TabStop = false;
            this.gbReadXml.Text = "Read Input From XML";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbMultiYears);
            this.groupBox1.Controls.Add(this.rdbOneYear);
            this.groupBox1.Location = new System.Drawing.Point(55, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(323, 71);
            this.groupBox1.TabIndex = 55;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Allocation Type";
            // 
            // rdbMultiYears
            // 
            this.rdbMultiYears.AutoSize = true;
            this.rdbMultiYears.Checked = true;
            this.rdbMultiYears.Location = new System.Drawing.Point(180, 34);
            this.rdbMultiYears.Name = "rdbMultiYears";
            this.rdbMultiYears.Size = new System.Drawing.Size(77, 17);
            this.rdbMultiYears.TabIndex = 1;
            this.rdbMultiYears.TabStop = true;
            this.rdbMultiYears.Text = "Multi Years";
            this.rdbMultiYears.UseVisualStyleBackColor = true;
            // 
            // rdbOneYear
            // 
            this.rdbOneYear.AutoSize = true;
            this.rdbOneYear.Location = new System.Drawing.Point(44, 34);
            this.rdbOneYear.Name = "rdbOneYear";
            this.rdbOneYear.Size = new System.Drawing.Size(70, 17);
            this.rdbOneYear.TabIndex = 0;
            this.rdbOneYear.TabStop = true;
            this.rdbOneYear.Text = "One Year";
            this.rdbOneYear.UseVisualStyleBackColor = true;
            // 
            // cbTazForecastHeader1
            // 
            this.cbTazForecastHeader1.AutoSize = true;
            this.cbTazForecastHeader1.Location = new System.Drawing.Point(643, 514);
            this.cbTazForecastHeader1.Name = "cbTazForecastHeader1";
            this.cbTazForecastHeader1.Size = new System.Drawing.Size(61, 17);
            this.cbTazForecastHeader1.TabIndex = 68;
            this.cbTazForecastHeader1.Text = "Header";
            this.cbTazForecastHeader1.UseVisualStyleBackColor = true;
            // 
            // btnBrowseTazForecast1
            // 
            this.btnBrowseTazForecast1.Location = new System.Drawing.Point(592, 514);
            this.btnBrowseTazForecast1.Name = "btnBrowseTazForecast1";
            this.btnBrowseTazForecast1.Size = new System.Drawing.Size(24, 19);
            this.btnBrowseTazForecast1.TabIndex = 67;
            this.btnBrowseTazForecast1.Text = "....";
            this.btnBrowseTazForecast1.UseVisualStyleBackColor = true;
            this.btnBrowseTazForecast1.Click += new System.EventHandler(this.btnBrowseTazForecast1_Click);
            // 
            // txtTazForecastFile1
            // 
            this.txtTazForecastFile1.Location = new System.Drawing.Point(230, 514);
            this.txtTazForecastFile1.Name = "txtTazForecastFile1";
            this.txtTazForecastFile1.Size = new System.Drawing.Size(340, 20);
            this.txtTazForecastFile1.TabIndex = 66;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(69, 518);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 13);
            this.label3.TabIndex = 65;
            this.label3.Text = "Forecast Year TAZ File 1";
            // 
            // FrmInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 890);
            this.Controls.Add(this.cbTazForecastHeader1);
            this.Controls.Add(this.btnBrowseTazForecast1);
            this.Controls.Add(this.txtTazForecastFile1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbReadXml);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gbInputs);
            this.Controls.Add(this.cmdRun);
            this.Name = "FrmInput";
            this.Text = "Microzone Disaggregation Tool";
            this.gbInputs.ResumeLayout(false);
            this.gbInputs.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.gbReadXml.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdRun;
        private System.Windows.Forms.Label lblBlock;
        private System.Windows.Forms.Label lblMZ;
        private System.Windows.Forms.Label lblSchool;
        private System.Windows.Forms.Label lblOutput;
        private System.Windows.Forms.TextBox txtBlockFile;
        private System.Windows.Forms.TextBox txtMzFile;
        private System.Windows.Forms.TextBox txtSchoolFile;
        private System.Windows.Forms.TextBox txtOutputFile;
        private System.Windows.Forms.Button btnBrowseBlocks;
        private System.Windows.Forms.Button btnBrowseMZ;
        private System.Windows.Forms.Button btnBrowseSchool;
        private System.Windows.Forms.Button btnBrowseOutput;
        private System.Windows.Forms.Label lblTaz;
        private System.Windows.Forms.TextBox txtTazFile;
        private System.Windows.Forms.CheckedListBox clbEmplCats;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowseTaz;
        private System.Windows.Forms.GroupBox gbInputs;
        private System.Windows.Forms.Button btnEmplCatsOk;
        private System.Windows.Forms.Button btnReadXml;
        private System.Windows.Forms.CheckBox cbTazHeader;
        private System.Windows.Forms.CheckBox cbBlockHeader;
        private System.Windows.Forms.CheckBox cbMZHeader;
        private System.Windows.Forms.CheckBox cbSchoolHeader;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnOutputForm;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rdbSic;
        private System.Windows.Forms.RadioButton rdbNaics;
        private System.Windows.Forms.GroupBox gbReadXml;
        private System.Windows.Forms.CheckBox cbParcelBaseHeader;
        private System.Windows.Forms.CheckBox cbTazForecastHeader;
        private System.Windows.Forms.Button btnBrowseParcelBase;
        private System.Windows.Forms.Button btnBrowseTazForecast;
        private System.Windows.Forms.TextBox txtParcelBaseFile;
        private System.Windows.Forms.TextBox txtTazForecastFile;
        private System.Windows.Forms.Label lblParcelBaseFile;
        private System.Windows.Forms.Label lblTazForecastFile;
        private System.Windows.Forms.TextBox txtParcelForecastFile;
        private System.Windows.Forms.Label lblParcelForecastFile;
        private System.Windows.Forms.Button btnBrowseParcelForecast;
        private System.Windows.Forms.CheckBox cbCorrespondenceHeader;
        private System.Windows.Forms.Button btnBrowseCorrespondence;
        private System.Windows.Forms.TextBox txtDriCorrespondenceFile;
        private System.Windows.Forms.Label lblDriCorrespondenceFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdbMultiYears;
        private System.Windows.Forms.RadioButton rdbOneYear;
        private System.Windows.Forms.CheckBox cbSectorCorrespondenceHeader;
        private System.Windows.Forms.Button btnBrowseSectorCorrespondence;
        private System.Windows.Forms.TextBox txtSectorCorrespondenceFile;
        private System.Windows.Forms.Label lblSectorCorrespondenceFile;
        private System.Windows.Forms.CheckBox cbTazForecastHeader1;
        private System.Windows.Forms.Button btnBrowseTazForecast1;
        private System.Windows.Forms.TextBox txtTazForecastFile1;
        private System.Windows.Forms.Label label3;

    }
}

