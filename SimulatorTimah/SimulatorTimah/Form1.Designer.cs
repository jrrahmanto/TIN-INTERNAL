namespace SimulatorTimah
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.cboSimulator = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtJumlahBst = new System.Windows.Forms.TextBox();
            this.txtProduct = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBrand = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtSellerName = new System.Windows.Forms.ComboBox();
            this.clearingMemberBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tIN_KBIDataSet = new SimulatorTimah.TIN_KBIDataSet();
            this.label10 = new System.Windows.Forms.Label();
            this.txtSequence = new System.Windows.Forms.TextBox();
            this.txtBusinessDate = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnSimulateCTD = new System.Windows.Forms.Button();
            this.txtFormatCoa = new System.Windows.Forms.TextBox();
            this.txtFormatBst = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cboWarehouse = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtConsole = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSimulatorFD = new System.Windows.Forms.Button();
            this.txtFullDelivery = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtSeqExReff = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.btnSimulateTF = new System.Windows.Forms.Button();
            this.txtCIF = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.txtSeqBST = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.txtFormatBSTTrade = new System.Windows.Forms.TextBox();
            this.txtBuyerCode = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtSellerCode = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.txtFormatExRef = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtProductCode = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtTradeTime = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtStartSeq = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtJumlahTrade = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnFileBST = new System.Windows.Forms.Button();
            this.clearingMemberTableAdapter = new SimulatorTimah.TIN_KBIDataSetTableAdapters.ClearingMemberTableAdapter();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clearingMemberBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tIN_KBIDataSet)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboSimulator
            // 
            this.cboSimulator.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cboSimulator.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSimulator.FormattingEnabled = true;
            this.cboSimulator.Items.AddRange(new object[] {
            "Simulate CTDDaily",
            "Simulate Raw Trade Feed",
            "Simulate Full Delivery"});
            this.cboSimulator.Location = new System.Drawing.Point(98, 12);
            this.cboSimulator.Name = "cboSimulator";
            this.cboSimulator.Size = new System.Drawing.Size(314, 21);
            this.cboSimulator.TabIndex = 0;
            this.cboSimulator.SelectionChangeCommitted += new System.EventHandler(this.comboBox1_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Simulator";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Jumlah BST";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Seller Name";
            // 
            // txtJumlahBst
            // 
            this.txtJumlahBst.Location = new System.Drawing.Point(86, 19);
            this.txtJumlahBst.Name = "txtJumlahBst";
            this.txtJumlahBst.Size = new System.Drawing.Size(61, 20);
            this.txtJumlahBst.TabIndex = 4;
            // 
            // txtProduct
            // 
            this.txtProduct.Location = new System.Drawing.Point(86, 67);
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.Size = new System.Drawing.Size(100, 20);
            this.txtProduct.TabIndex = 6;
            this.txtProduct.Text = "TINPB300";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Product";
            // 
            // txtBrand
            // 
            this.txtBrand.Location = new System.Drawing.Point(86, 91);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(100, 20);
            this.txtBrand.TabIndex = 8;
            this.txtBrand.Text = "BANKA";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Brand";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtSellerName);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtSequence);
            this.groupBox1.Controls.Add(this.txtBusinessDate);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.btnSimulateCTD);
            this.groupBox1.Controls.Add(this.txtFormatCoa);
            this.groupBox1.Controls.Add(this.txtFormatBst);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cboWarehouse);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtBrand);
            this.groupBox1.Controls.Add(this.txtJumlahBst);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtProduct);
            this.groupBox1.Location = new System.Drawing.Point(12, 49);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(656, 156);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Simulator CTDDaily";
            // 
            // txtSellerName
            // 
            this.txtSellerName.DataSource = this.clearingMemberBindingSource;
            this.txtSellerName.DisplayMember = "Name";
            this.txtSellerName.FormattingEnabled = true;
            this.txtSellerName.Location = new System.Drawing.Point(86, 43);
            this.txtSellerName.Name = "txtSellerName";
            this.txtSellerName.Size = new System.Drawing.Size(217, 21);
            this.txtSellerName.TabIndex = 21;
            this.txtSellerName.ValueMember = "Name";
            // 
            // clearingMemberBindingSource
            // 
            this.clearingMemberBindingSource.DataMember = "ClearingMember";
            this.clearingMemberBindingSource.DataSource = this.tIN_KBIDataSet;
            // 
            // tIN_KBIDataSet
            // 
            this.tIN_KBIDataSet.DataSetName = "TIN_KBIDataSet";
            this.tIN_KBIDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(322, 94);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(51, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "Start Seq";
            // 
            // txtSequence
            // 
            this.txtSequence.Location = new System.Drawing.Point(400, 91);
            this.txtSequence.Name = "txtSequence";
            this.txtSequence.Size = new System.Drawing.Size(100, 20);
            this.txtSequence.TabIndex = 19;
            this.txtSequence.Text = "1";
            // 
            // txtBusinessDate
            // 
            this.txtBusinessDate.Location = new System.Drawing.Point(400, 67);
            this.txtBusinessDate.Name = "txtBusinessDate";
            this.txtBusinessDate.Size = new System.Drawing.Size(138, 20);
            this.txtBusinessDate.TabIndex = 18;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(322, 70);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "Business Date";
            // 
            // btnSimulateCTD
            // 
            this.btnSimulateCTD.Location = new System.Drawing.Point(325, 113);
            this.btnSimulateCTD.Name = "btnSimulateCTD";
            this.btnSimulateCTD.Size = new System.Drawing.Size(75, 23);
            this.btnSimulateCTD.TabIndex = 16;
            this.btnSimulateCTD.Text = "Create Simulation Data";
            this.btnSimulateCTD.UseVisualStyleBackColor = true;
            this.btnSimulateCTD.Click += new System.EventHandler(this.btnSimulateCTD_Click);
            // 
            // txtFormatCoa
            // 
            this.txtFormatCoa.Location = new System.Drawing.Point(400, 43);
            this.txtFormatCoa.Name = "txtFormatCoa";
            this.txtFormatCoa.Size = new System.Drawing.Size(230, 20);
            this.txtFormatCoa.TabIndex = 15;
            this.txtFormatCoa.Text = "{sequence}/ALARAL";
            // 
            // txtFormatBst
            // 
            this.txtFormatBst.Location = new System.Drawing.Point(400, 19);
            this.txtFormatBst.Name = "txtFormatBst";
            this.txtFormatBst.Size = new System.Drawing.Size(230, 20);
            this.txtFormatBst.TabIndex = 14;
            this.txtFormatBst.Text = "BST/{tahun}/{bulan}/I/{sequence}";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(322, 46);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Format COA";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(322, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Format BST";
            // 
            // cboWarehouse
            // 
            this.cboWarehouse.FormattingEnabled = true;
            this.cboWarehouse.Items.AddRange(new object[] {
            "GD. RFE MUNTOK",
            "GD. KUNDUR",
            "GD. PANGKAL BALAM"});
            this.cboWarehouse.Location = new System.Drawing.Point(86, 115);
            this.cboWarehouse.Name = "cboWarehouse";
            this.cboWarehouse.Size = new System.Drawing.Size(121, 21);
            this.cboWarehouse.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 118);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Warehouse";
            // 
            // txtConsole
            // 
            this.txtConsole.Location = new System.Drawing.Point(683, 16);
            this.txtConsole.Multiline = true;
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.ReadOnly = true;
            this.txtConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtConsole.Size = new System.Drawing.Size(502, 516);
            this.txtConsole.TabIndex = 11;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSimulatorFD);
            this.groupBox2.Controls.Add(this.txtFullDelivery);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Location = new System.Drawing.Point(12, 202);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(656, 52);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Simulator Full Delivery";
            // 
            // btnSimulatorFD
            // 
            this.btnSimulatorFD.Location = new System.Drawing.Point(325, 15);
            this.btnSimulatorFD.Name = "btnSimulatorFD";
            this.btnSimulatorFD.Size = new System.Drawing.Size(75, 23);
            this.btnSimulatorFD.TabIndex = 2;
            this.btnSimulatorFD.Text = "Create";
            this.btnSimulatorFD.UseVisualStyleBackColor = true;
            this.btnSimulatorFD.Click += new System.EventHandler(this.btnSimulatorFD_Click);
            // 
            // txtFullDelivery
            // 
            this.txtFullDelivery.Location = new System.Drawing.Point(86, 17);
            this.txtFullDelivery.Name = "txtFullDelivery";
            this.txtFullDelivery.Size = new System.Drawing.Size(217, 20);
            this.txtFullDelivery.TabIndex = 1;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(7, 20);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(45, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "No BST";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtSeqExReff);
            this.groupBox3.Controls.Add(this.label22);
            this.groupBox3.Controls.Add(this.btnSimulateTF);
            this.groupBox3.Controls.Add(this.txtCIF);
            this.groupBox3.Controls.Add(this.label21);
            this.groupBox3.Controls.Add(this.txtSeqBST);
            this.groupBox3.Controls.Add(this.label20);
            this.groupBox3.Controls.Add(this.txtFormatBSTTrade);
            this.groupBox3.Controls.Add(this.txtBuyerCode);
            this.groupBox3.Controls.Add(this.label19);
            this.groupBox3.Controls.Add(this.txtSellerCode);
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Controls.Add(this.txtFormatExRef);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.txtProductCode);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.txtTradeTime);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.txtStartSeq);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.txtJumlahTrade);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Location = new System.Drawing.Point(12, 260);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(656, 167);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Simulator TradeFeed";
            // 
            // txtSeqExReff
            // 
            this.txtSeqExReff.Location = new System.Drawing.Point(86, 132);
            this.txtSeqExReff.Name = "txtSeqExReff";
            this.txtSeqExReff.Size = new System.Drawing.Size(100, 20);
            this.txtSeqExReff.TabIndex = 29;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(7, 135);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(61, 13);
            this.label22.TabIndex = 28;
            this.label22.Text = "Seq ExReff";
            // 
            // btnSimulateTF
            // 
            this.btnSimulateTF.Location = new System.Drawing.Point(325, 135);
            this.btnSimulateTF.Name = "btnSimulateTF";
            this.btnSimulateTF.Size = new System.Drawing.Size(75, 23);
            this.btnSimulateTF.TabIndex = 27;
            this.btnSimulateTF.Text = "Create";
            this.btnSimulateTF.UseVisualStyleBackColor = true;
            this.btnSimulateTF.Click += new System.EventHandler(this.btnSimulateTF_Click);
            // 
            // txtCIF
            // 
            this.txtCIF.Location = new System.Drawing.Point(400, 109);
            this.txtCIF.Name = "txtCIF";
            this.txtCIF.Size = new System.Drawing.Size(100, 20);
            this.txtCIF.TabIndex = 26;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(322, 112);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(23, 13);
            this.label21.TabIndex = 25;
            this.label21.Text = "CIF";
            // 
            // txtSeqBST
            // 
            this.txtSeqBST.Location = new System.Drawing.Point(86, 109);
            this.txtSeqBST.Name = "txtSeqBST";
            this.txtSeqBST.Size = new System.Drawing.Size(61, 20);
            this.txtSeqBST.TabIndex = 24;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(7, 112);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(50, 13);
            this.label20.TabIndex = 23;
            this.label20.Text = "Seq BST";
            // 
            // txtFormatBSTTrade
            // 
            this.txtFormatBSTTrade.Location = new System.Drawing.Point(400, 86);
            this.txtFormatBSTTrade.Name = "txtFormatBSTTrade";
            this.txtFormatBSTTrade.Size = new System.Drawing.Size(178, 20);
            this.txtFormatBSTTrade.TabIndex = 22;
            this.txtFormatBSTTrade.Text = "BST/{tahun}/{bulan}/XIII/KBI/{sequence}";
            // 
            // txtBuyerCode
            // 
            this.txtBuyerCode.Location = new System.Drawing.Point(400, 63);
            this.txtBuyerCode.Name = "txtBuyerCode";
            this.txtBuyerCode.Size = new System.Drawing.Size(78, 20);
            this.txtBuyerCode.TabIndex = 13;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(322, 89);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(63, 13);
            this.label19.TabIndex = 21;
            this.label19.Text = "Format BST";
            // 
            // txtSellerCode
            // 
            this.txtSellerCode.Location = new System.Drawing.Point(86, 86);
            this.txtSellerCode.Name = "txtSellerCode";
            this.txtSellerCode.Size = new System.Drawing.Size(78, 20);
            this.txtSellerCode.TabIndex = 12;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(322, 66);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(62, 13);
            this.label18.TabIndex = 11;
            this.label18.Text = "Buyer Code";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(7, 89);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(61, 13);
            this.label17.TabIndex = 10;
            this.label17.Text = "Seller Code";
            // 
            // txtFormatExRef
            // 
            this.txtFormatExRef.Location = new System.Drawing.Point(86, 63);
            this.txtFormatExRef.Name = "txtFormatExRef";
            this.txtFormatExRef.Size = new System.Drawing.Size(161, 20);
            this.txtFormatExRef.TabIndex = 9;
            this.txtFormatExRef.Text = "19G{tanggal}{sequence}";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(7, 66);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(74, 13);
            this.label16.TabIndex = 8;
            this.label16.Text = "Format ExReff";
            // 
            // txtProductCode
            // 
            this.txtProductCode.Location = new System.Drawing.Point(400, 40);
            this.txtProductCode.Name = "txtProductCode";
            this.txtProductCode.Size = new System.Drawing.Size(100, 20);
            this.txtProductCode.TabIndex = 7;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(322, 43);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(72, 13);
            this.label15.TabIndex = 6;
            this.label15.Text = "Product Code";
            // 
            // txtTradeTime
            // 
            this.txtTradeTime.Location = new System.Drawing.Point(86, 40);
            this.txtTradeTime.Name = "txtTradeTime";
            this.txtTradeTime.Size = new System.Drawing.Size(135, 20);
            this.txtTradeTime.TabIndex = 5;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(7, 43);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(58, 13);
            this.label14.TabIndex = 4;
            this.label14.Text = "TradeTime";
            // 
            // txtStartSeq
            // 
            this.txtStartSeq.Location = new System.Drawing.Point(400, 17);
            this.txtStartSeq.Name = "txtStartSeq";
            this.txtStartSeq.Size = new System.Drawing.Size(100, 20);
            this.txtStartSeq.TabIndex = 3;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(322, 20);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(51, 13);
            this.label13.TabIndex = 2;
            this.label13.Text = "Start Seq";
            // 
            // txtJumlahTrade
            // 
            this.txtJumlahTrade.Location = new System.Drawing.Point(86, 17);
            this.txtJumlahTrade.Name = "txtJumlahTrade";
            this.txtJumlahTrade.Size = new System.Drawing.Size(61, 20);
            this.txtJumlahTrade.TabIndex = 1;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(7, 20);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(71, 13);
            this.label12.TabIndex = 0;
            this.label12.Text = "Jumlah Trade";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnFileBST);
            this.groupBox4.Location = new System.Drawing.Point(12, 432);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(656, 59);
            this.groupBox4.TabIndex = 14;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Simulator BST";
            // 
            // btnFileBST
            // 
            this.btnFileBST.Location = new System.Drawing.Point(10, 20);
            this.btnFileBST.Name = "btnFileBST";
            this.btnFileBST.Size = new System.Drawing.Size(75, 23);
            this.btnFileBST.TabIndex = 0;
            this.btnFileBST.Text = "Create File";
            this.btnFileBST.UseVisualStyleBackColor = true;
            this.btnFileBST.Click += new System.EventHandler(this.button1_Click);
            // 
            // clearingMemberTableAdapter
            // 
            this.clearingMemberTableAdapter.ClearBeforeFill = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1197, 544);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.txtConsole);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboSimulator);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Simulator Timah";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clearingMemberBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tIN_KBIDataSet)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboSimulator;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtJumlahBst;
        private System.Windows.Forms.TextBox txtProduct;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBrand;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtFormatCoa;
        private System.Windows.Forms.TextBox txtFormatBst;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cboWarehouse;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtBusinessDate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnSimulateCTD;
        private System.Windows.Forms.TextBox txtConsole;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtSequence;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSimulatorFD;
        private System.Windows.Forms.TextBox txtFullDelivery;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtFormatExRef;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtProductCode;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtTradeTime;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtStartSeq;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtJumlahTrade;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtBuyerCode;
        private System.Windows.Forms.TextBox txtSellerCode;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtSeqBST;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtFormatBSTTrade;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtCIF;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button btnSimulateTF;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnFileBST;
        private System.Windows.Forms.TextBox txtSeqExReff;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.ComboBox txtSellerName;
        private TIN_KBIDataSet tIN_KBIDataSet;
        private System.Windows.Forms.BindingSource clearingMemberBindingSource;
        private TIN_KBIDataSetTableAdapters.ClearingMemberTableAdapter clearingMemberTableAdapter;
    }
}

