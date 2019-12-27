namespace MapAssist
{
    partial class MapAssist
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button_AddFile = new System.Windows.Forms.Button();
            this.button_Start = new System.Windows.Forms.Button();
            this.listBox_files = new System.Windows.Forms.ListBox();
            this.button_Delfile = new System.Windows.Forms.Button();
            this.dataGridView_FileModule = new System.Windows.Forms.DataGridView();
            this.button_SaveModuleCfg = new System.Windows.Forms.Button();
            this.button_ReadModuleCfg = new System.Windows.Forms.Button();
            this.RichTxtBox_Log = new System.Windows.Forms.RichTextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.checkedListBox_module = new System.Windows.Forms.CheckedListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkBox_ModuleAll = new System.Windows.Forms.CheckBox();
            this.button_AnaModule = new System.Windows.Forms.Button();
            this.dataGridView_detail = new System.Windows.Forms.DataGridView();
            this.checkedListBox_Resource = new System.Windows.Forms.CheckedListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_FileModule)).BeginInit();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_detail)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_AddFile
            // 
            this.button_AddFile.Location = new System.Drawing.Point(5, 23);
            this.button_AddFile.Name = "button_AddFile";
            this.button_AddFile.Size = new System.Drawing.Size(75, 23);
            this.button_AddFile.TabIndex = 0;
            this.button_AddFile.Text = "AddFile";
            this.button_AddFile.UseVisualStyleBackColor = true;
            this.button_AddFile.Click += new System.EventHandler(this.button_AddFile_Click);
            // 
            // button_Start
            // 
            this.button_Start.Location = new System.Drawing.Point(3, 158);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(75, 23);
            this.button_Start.TabIndex = 1;
            this.button_Start.Text = "Start";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.button_Start_Click);
            // 
            // listBox_files
            // 
            this.listBox_files.AllowDrop = true;
            this.listBox_files.FormattingEnabled = true;
            this.listBox_files.HorizontalScrollbar = true;
            this.listBox_files.ItemHeight = 12;
            this.listBox_files.Location = new System.Drawing.Point(3, 52);
            this.listBox_files.Name = "listBox_files";
            this.listBox_files.Size = new System.Drawing.Size(261, 100);
            this.listBox_files.TabIndex = 2;
            // 
            // button_Delfile
            // 
            this.button_Delfile.Location = new System.Drawing.Point(84, 23);
            this.button_Delfile.Name = "button_Delfile";
            this.button_Delfile.Size = new System.Drawing.Size(75, 23);
            this.button_Delfile.TabIndex = 3;
            this.button_Delfile.Text = "Delfile";
            this.button_Delfile.UseVisualStyleBackColor = true;
            this.button_Delfile.Click += new System.EventHandler(this.button_Delfile_Click);
            // 
            // dataGridView_FileModule
            // 
            this.dataGridView_FileModule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_FileModule.Location = new System.Drawing.Point(1013, 31);
            this.dataGridView_FileModule.Name = "dataGridView_FileModule";
            this.dataGridView_FileModule.RowTemplate.Height = 23;
            this.dataGridView_FileModule.Size = new System.Drawing.Size(247, 356);
            this.dataGridView_FileModule.TabIndex = 5;
            this.dataGridView_FileModule.Tag = "";
            // 
            // button_SaveModuleCfg
            // 
            this.button_SaveModuleCfg.Location = new System.Drawing.Point(1266, 31);
            this.button_SaveModuleCfg.Name = "button_SaveModuleCfg";
            this.button_SaveModuleCfg.Size = new System.Drawing.Size(92, 32);
            this.button_SaveModuleCfg.TabIndex = 6;
            this.button_SaveModuleCfg.Text = "SaveModuleCfg";
            this.button_SaveModuleCfg.UseVisualStyleBackColor = true;
            this.button_SaveModuleCfg.Click += new System.EventHandler(this.button_SaveModuleCfg_Click);
            // 
            // button_ReadModuleCfg
            // 
            this.button_ReadModuleCfg.Location = new System.Drawing.Point(1266, 69);
            this.button_ReadModuleCfg.Name = "button_ReadModuleCfg";
            this.button_ReadModuleCfg.Size = new System.Drawing.Size(92, 33);
            this.button_ReadModuleCfg.TabIndex = 7;
            this.button_ReadModuleCfg.Text = "ReadModuleCfg";
            this.button_ReadModuleCfg.UseVisualStyleBackColor = true;
            this.button_ReadModuleCfg.Click += new System.EventHandler(this.button_ReadModuleCfg_Click);
            // 
            // RichTxtBox_Log
            // 
            this.RichTxtBox_Log.Location = new System.Drawing.Point(289, 28);
            this.RichTxtBox_Log.Name = "RichTxtBox_Log";
            this.RichTxtBox_Log.Size = new System.Drawing.Size(470, 316);
            this.RichTxtBox_Log.TabIndex = 8;
            this.RichTxtBox_Log.Text = "";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // checkedListBox_module
            // 
            this.checkedListBox_module.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.checkedListBox_module.FormattingEnabled = true;
            this.checkedListBox_module.Location = new System.Drawing.Point(0, 12);
            this.checkedListBox_module.Name = "checkedListBox_module";
            this.checkedListBox_module.Size = new System.Drawing.Size(200, 164);
            this.checkedListBox_module.TabIndex = 9;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_AddFile);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.listBox_files);
            this.panel1.Controls.Add(this.button_Delfile);
            this.panel1.Controls.Add(this.button_Start);
            this.panel1.Location = new System.Drawing.Point(12, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(271, 230);
            this.panel1.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "Add Map File and Ld File";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1370, 25);
            this.menuStrip1.TabIndex = 14;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(39, 21);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(47, 21);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // checkBox_ModuleAll
            // 
            this.checkBox_ModuleAll.AutoSize = true;
            this.checkBox_ModuleAll.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.checkBox_ModuleAll.Location = new System.Drawing.Point(0, 176);
            this.checkBox_ModuleAll.Name = "checkBox_ModuleAll";
            this.checkBox_ModuleAll.Size = new System.Drawing.Size(200, 16);
            this.checkBox_ModuleAll.TabIndex = 15;
            this.checkBox_ModuleAll.Text = "ALL Moudle";
            this.checkBox_ModuleAll.UseVisualStyleBackColor = true;
            this.checkBox_ModuleAll.CheckedChanged += new System.EventHandler(this.checkBox_ModuleAll_CheckedChanged);
            // 
            // button_AnaModule
            // 
            this.button_AnaModule.Location = new System.Drawing.Point(781, 446);
            this.button_AnaModule.Name = "button_AnaModule";
            this.button_AnaModule.Size = new System.Drawing.Size(177, 54);
            this.button_AnaModule.TabIndex = 16;
            this.button_AnaModule.Text = "Analysis Module";
            this.button_AnaModule.UseVisualStyleBackColor = true;
            this.button_AnaModule.Click += new System.EventHandler(this.button_AnalysisFilterModule_Click);
            // 
            // dataGridView_detail
            // 
            this.dataGridView_detail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_detail.Location = new System.Drawing.Point(289, 350);
            this.dataGridView_detail.Name = "dataGridView_detail";
            this.dataGridView_detail.RowTemplate.Height = 23;
            this.dataGridView_detail.Size = new System.Drawing.Size(470, 299);
            this.dataGridView_detail.TabIndex = 18;
            // 
            // checkedListBox_Resource
            // 
            this.checkedListBox_Resource.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.checkedListBox_Resource.FormattingEnabled = true;
            this.checkedListBox_Resource.Location = new System.Drawing.Point(0, 16);
            this.checkedListBox_Resource.Name = "checkedListBox_Resource";
            this.checkedListBox_Resource.Size = new System.Drawing.Size(183, 84);
            this.checkedListBox_Resource.TabIndex = 11;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.checkedListBox_module);
            this.panel2.Controls.Add(this.checkBox_ModuleAll);
            this.panel2.Location = new System.Drawing.Point(773, 31);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 192);
            this.panel2.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 16;
            this.label2.Text = "Select Module";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.checkedListBox_Resource);
            this.panel3.Location = new System.Drawing.Point(775, 286);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(183, 100);
            this.panel3.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(773, 271);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "Select Resource";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(775, 392);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(96, 16);
            this.checkBox1.TabIndex = 9;
            this.checkBox1.Text = "ALL Resource";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // MapAssist
            // 
            this.ClientSize = new System.Drawing.Size(1370, 750);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.dataGridView_detail);
            this.Controls.Add(this.button_AnaModule);
            this.Controls.Add(this.button_ReadModuleCfg);
            this.Controls.Add(this.button_SaveModuleCfg);
            this.Controls.Add(this.dataGridView_FileModule);
            this.Controls.Add(this.RichTxtBox_Log);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MapAssist";
            this.Load += new System.EventHandler(this.MapAssist_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_FileModule)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_detail)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.Button button_AddFile;
        private System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.ListBox listBox_files;
        private System.Windows.Forms.Button button_Delfile;
        private System.Windows.Forms.DataGridView dataGridView_FileModule;
        private System.Windows.Forms.Button button_SaveModuleCfg;
        private System.Windows.Forms.Button button_ReadModuleCfg;
        private System.Windows.Forms.RichTextBox RichTxtBox_Log;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckedListBox checkedListBox_module;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBox_ModuleAll;
        private System.Windows.Forms.Button button_AnaModule;
        private System.Windows.Forms.DataGridView dataGridView_detail;
        private System.Windows.Forms.CheckedListBox checkedListBox_Resource;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}

