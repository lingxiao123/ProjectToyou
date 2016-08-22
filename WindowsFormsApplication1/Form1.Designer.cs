namespace WindowsFormsApplication1
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnOpenPort = new System.Windows.Forms.Button();
            this.btnClosePort = new System.Windows.Forms.Button();
            this.SerPort = new System.IO.Ports.SerialPort(this.components);
            this.PanTop = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.dgrProjectList = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.txtSNo = new System.Windows.Forms.TextBox();
            this.txtDate = new System.Windows.Forms.TextBox();
            this.txtSpecification = new System.Windows.Forms.TextBox();
            this.txtTWeight = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblWeight = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ddlDep = new System.Windows.Forms.ComboBox();
            this.ddlInventory = new System.Windows.Forms.ComboBox();
            this.ddlStore = new System.Windows.Forms.ComboBox();
            this.ddlBusType = new System.Windows.Forms.ComboBox();
            this.cboSerialName = new System.Windows.Forms.ComboBox();
            this.productInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.privateDataBaseDataSet = new WindowsFormsApplication1.PrivateDataBaseDataSet();
            this.productInfoTableAdapter = new WindowsFormsApplication1.PrivateDataBaseDataSetTableAdapters.ProductInfoTableAdapter();
            this.fillByToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.PanTop.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgrProjectList)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.productInfoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.privateDataBaseDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOpenPort
            // 
            this.btnOpenPort.Location = new System.Drawing.Point(13, 13);
            this.btnOpenPort.Name = "btnOpenPort";
            this.btnOpenPort.Size = new System.Drawing.Size(75, 23);
            this.btnOpenPort.TabIndex = 0;
            this.btnOpenPort.Text = "打开连接";
            this.btnOpenPort.UseVisualStyleBackColor = true;
            this.btnOpenPort.Click += new System.EventHandler(this.btnOpenPort_Click);
            // 
            // btnClosePort
            // 
            this.btnClosePort.Location = new System.Drawing.Point(95, 12);
            this.btnClosePort.Name = "btnClosePort";
            this.btnClosePort.Size = new System.Drawing.Size(75, 23);
            this.btnClosePort.TabIndex = 1;
            this.btnClosePort.Text = "关闭连接";
            this.btnClosePort.UseVisualStyleBackColor = true;
            this.btnClosePort.Click += new System.EventHandler(this.btnClosePort_Click);
            // 
            // PanTop
            // 
            this.PanTop.AutoSize = true;
            this.PanTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanTop.Controls.Add(this.groupBox3);
            this.PanTop.Controls.Add(this.groupBox2);
            this.PanTop.Controls.Add(this.groupBox1);
            this.PanTop.Location = new System.Drawing.Point(13, 43);
            this.PanTop.Name = "PanTop";
            this.PanTop.Size = new System.Drawing.Size(1013, 536);
            this.PanTop.TabIndex = 2;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnImport);
            this.groupBox3.Controls.Add(this.dgrProjectList);
            this.groupBox3.Location = new System.Drawing.Point(3, 272);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1005, 259);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "采集结果";
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(9, 21);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(144, 23);
            this.btnImport.TabIndex = 1;
            this.btnImport.Text = "将采集结果导入T+";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // dgrProjectList
            // 
            this.dgrProjectList.AllowUserToAddRows = false;
            this.dgrProjectList.AllowUserToDeleteRows = false;
            this.dgrProjectList.AllowUserToOrderColumns = true;
            this.dgrProjectList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgrProjectList.Location = new System.Drawing.Point(7, 55);
            this.dgrProjectList.Name = "dgrProjectList";
            this.dgrProjectList.ReadOnly = true;
            this.dgrProjectList.RowTemplate.Height = 23;
            this.dgrProjectList.Size = new System.Drawing.Size(992, 184);
            this.dgrProjectList.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.Controls.Add(this.btnPrint);
            this.groupBox2.Controls.Add(this.txtSNo);
            this.groupBox2.Controls.Add(this.txtDate);
            this.groupBox2.Controls.Add(this.txtSpecification);
            this.groupBox2.Controls.Add(this.txtTWeight);
            this.groupBox2.Controls.Add(this.txtName);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.lblWeight);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Location = new System.Drawing.Point(3, 98);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1005, 168);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "选择数据";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(621, 91);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 49);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "保  存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(355, 92);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(170, 65);
            this.btnPrint.TabIndex = 3;
            this.btnPrint.Text = "打  印";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // txtSNo
            // 
            this.txtSNo.Location = new System.Drawing.Point(403, 59);
            this.txtSNo.Name = "txtSNo";
            this.txtSNo.ReadOnly = true;
            this.txtSNo.Size = new System.Drawing.Size(182, 21);
            this.txtSNo.TabIndex = 2;
            // 
            // txtDate
            // 
            this.txtDate.Location = new System.Drawing.Point(725, 57);
            this.txtDate.Name = "txtDate";
            this.txtDate.ReadOnly = true;
            this.txtDate.Size = new System.Drawing.Size(182, 21);
            this.txtDate.TabIndex = 2;
            // 
            // txtSpecification
            // 
            this.txtSpecification.Location = new System.Drawing.Point(76, 58);
            this.txtSpecification.Name = "txtSpecification";
            this.txtSpecification.ReadOnly = true;
            this.txtSpecification.Size = new System.Drawing.Size(195, 21);
            this.txtSpecification.TabIndex = 2;
            // 
            // txtTWeight
            // 
            this.txtTWeight.Location = new System.Drawing.Point(76, 16);
            this.txtTWeight.Name = "txtTWeight";
            this.txtTWeight.Size = new System.Drawing.Size(195, 21);
            this.txtTWeight.TabIndex = 2;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(403, 16);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(502, 21);
            this.txtName.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(334, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "产品名称:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(660, 62);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(59, 12);
            this.label12.TabIndex = 0;
            this.label12.Text = "生产日期:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("楷体", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(256, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 48);
            this.label3.TabIndex = 2;
            this.label3.Text = "KG";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWeight
            // 
            this.lblWeight.AutoSize = true;
            this.lblWeight.Font = new System.Drawing.Font("楷体", 60F, System.Drawing.FontStyle.Bold);
            this.lblWeight.Location = new System.Drawing.Point(8, 77);
            this.lblWeight.Name = "lblWeight";
            this.lblWeight.Size = new System.Drawing.Size(239, 80);
            this.lblWeight.TabIndex = 2;
            this.lblWeight.Text = "00.00";
            this.lblWeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(333, 62);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "序(批)号:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 62);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "规格型号:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 22);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(59, 12);
            this.label11.TabIndex = 0;
            this.label11.Text = "桶(箱)重:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtUserID);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.ddlDep);
            this.groupBox1.Controls.Add(this.ddlInventory);
            this.groupBox1.Controls.Add(this.ddlStore);
            this.groupBox1.Controls.Add(this.ddlBusType);
            this.groupBox1.Controls.Add(this.cboSerialName);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1005, 89);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择数据";
            // 
            // txtUserID
            // 
            this.txtUserID.Location = new System.Drawing.Point(787, 19);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(118, 21);
            this.txtUserID.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 60);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "生产车间:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(242, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "存货:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(733, 22);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(47, 12);
            this.label13.TabIndex = 0;
            this.label13.Text = "T+帐号:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(501, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "仓库:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(242, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "业务类型:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "串    口:";
            // 
            // ddlDep
            // 
            this.ddlDep.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDep.FormattingEnabled = true;
            this.ddlDep.Location = new System.Drawing.Point(76, 57);
            this.ddlDep.Name = "ddlDep";
            this.ddlDep.Size = new System.Drawing.Size(121, 20);
            this.ddlDep.TabIndex = 1;
            // 
            // ddlInventory
            // 
            this.ddlInventory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlInventory.FormattingEnabled = true;
            this.ddlInventory.Location = new System.Drawing.Point(303, 57);
            this.ddlInventory.Name = "ddlInventory";
            this.ddlInventory.Size = new System.Drawing.Size(360, 20);
            this.ddlInventory.TabIndex = 1;
            this.ddlInventory.SelectedIndexChanged += new System.EventHandler(this.ddlInventory_SelectedIndexChanged);
            // 
            // ddlStore
            // 
            this.ddlStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlStore.FormattingEnabled = true;
            this.ddlStore.Location = new System.Drawing.Point(542, 19);
            this.ddlStore.Name = "ddlStore";
            this.ddlStore.Size = new System.Drawing.Size(121, 20);
            this.ddlStore.TabIndex = 1;
            // 
            // ddlBusType
            // 
            this.ddlBusType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlBusType.FormattingEnabled = true;
            this.ddlBusType.Location = new System.Drawing.Point(303, 19);
            this.ddlBusType.Name = "ddlBusType";
            this.ddlBusType.Size = new System.Drawing.Size(121, 20);
            this.ddlBusType.TabIndex = 1;
            // 
            // cboSerialName
            // 
            this.cboSerialName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSerialName.FormattingEnabled = true;
            this.cboSerialName.Location = new System.Drawing.Point(76, 19);
            this.cboSerialName.Name = "cboSerialName";
            this.cboSerialName.Size = new System.Drawing.Size(121, 20);
            this.cboSerialName.TabIndex = 1;
            // 
            // productInfoBindingSource
            // 
            this.productInfoBindingSource.DataMember = "ProductInfo";
            this.productInfoBindingSource.DataSource = this.privateDataBaseDataSet;
            // 
            // privateDataBaseDataSet
            // 
            this.privateDataBaseDataSet.DataSetName = "PrivateDataBaseDataSet";
            this.privateDataBaseDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // productInfoTableAdapter
            // 
            this.productInfoTableAdapter.ClearBeforeFill = true;
            // 
            // fillByToolStripButton
            // 
            this.fillByToolStripButton.Name = "fillByToolStripButton";
            this.fillByToolStripButton.Size = new System.Drawing.Size(23, 23);
            // 
            // FormMain
            // 
            this.ClientSize = new System.Drawing.Size(1034, 589);
            this.Controls.Add(this.PanTop);
            this.Controls.Add(this.btnClosePort);
            this.Controls.Add(this.btnOpenPort);
            this.MaximumSize = new System.Drawing.Size(1200, 800);
            this.Name = "FormMain";
            this.Text = "电子秤";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.PanTop.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgrProjectList)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.productInfoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.privateDataBaseDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpenPort;
        private System.Windows.Forms.Button btnClosePort;
        private System.IO.Ports.SerialPort SerPort;
        private System.Windows.Forms.Panel PanTop;
        private System.Windows.Forms.ComboBox cboSerialName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblWeight;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ddlInventory;
        private System.Windows.Forms.ComboBox ddlStore;
        private System.Windows.Forms.ComboBox ddlBusType;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox ddlDep;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtSNo;
        private System.Windows.Forms.TextBox txtDate;
        private System.Windows.Forms.TextBox txtSpecification;
        private System.Windows.Forms.TextBox txtTWeight;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.DataGridView dgrProjectList;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnPrint;
        private PrivateDataBaseDataSet privateDataBaseDataSet;
        private System.Windows.Forms.BindingSource productInfoBindingSource;
        private PrivateDataBaseDataSetTableAdapters.ProductInfoTableAdapter productInfoTableAdapter;
        private System.Windows.Forms.ToolStripButton fillByToolStripButton;
    }
}

