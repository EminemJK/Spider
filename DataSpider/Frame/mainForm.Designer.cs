namespace DataSpider
{
    partial class mainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("任务区");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("运行中");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("挂起中");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("已停止");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("任务运行状态", new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode3,
            treeNode4});
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSplitBtnSys = new System.Windows.Forms.ToolStripSplitButton();
            this.btnSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.tbNewTask = new System.Windows.Forms.ToolStripMenuItem();
            this.taskTreeView = new System.Windows.Forms.TreeView();
            this.taskDataView = new System.Windows.Forms.DataGridView();
            this.contextMenuStripTaskGridView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnRun = new System.Windows.Forms.ToolStripMenuItem();
            this.btnConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.btnTaskTurnOn = new System.Windows.Forms.ToolStripMenuItem();
            this.btnTaskOff = new System.Windows.Forms.ToolStripMenuItem();
            this.btnTaskDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tbLog = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbSysLog = new System.Windows.Forms.RichTextBox();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStripNotify = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.NotifyExit = new System.Windows.Forms.ToolStripMenuItem();
            this.timerAuto = new System.Windows.Forms.Timer(this.components);
            this.btnRemoveError = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.taskDataView)).BeginInit();
            this.contextMenuStripTaskGridView.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.contextMenuStripNotify.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitBtnSys,
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(969, 25);
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStripSplitBtnSys
            // 
            this.toolStripSplitBtnSys.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSetting});
            this.toolStripSplitBtnSys.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitBtnSys.Image")));
            this.toolStripSplitBtnSys.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitBtnSys.Name = "toolStripSplitBtnSys";
            this.toolStripSplitBtnSys.Size = new System.Drawing.Size(64, 22);
            this.toolStripSplitBtnSys.Text = "工具";
            this.toolStripSplitBtnSys.ToolTipText = "设置";
            // 
            // btnSetting
            // 
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(100, 22);
            this.btnSetting.Text = "设置";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbNewTask});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(85, 22);
            this.toolStripDropDownButton1.Text = "新建任务";
            // 
            // tbNewTask
            // 
            this.tbNewTask.Name = "tbNewTask";
            this.tbNewTask.Size = new System.Drawing.Size(112, 22);
            this.tbNewTask.Text = "新任务";
            this.tbNewTask.Click += new System.EventHandler(this.tbNewTask_Click);
            // 
            // taskTreeView
            // 
            this.taskTreeView.Location = new System.Drawing.Point(0, 28);
            this.taskTreeView.Name = "taskTreeView";
            treeNode1.Name = "taskList";
            treeNode1.Text = "任务区";
            treeNode2.Name = "节点3";
            treeNode2.Text = "运行中";
            treeNode3.Name = "节点0";
            treeNode3.Text = "挂起中";
            treeNode4.Name = "节点4";
            treeNode4.Text = "已停止";
            treeNode5.Name = "taskState";
            treeNode5.Text = "任务运行状态";
            this.taskTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode5});
            this.taskTreeView.Size = new System.Drawing.Size(149, 229);
            this.taskTreeView.TabIndex = 1;
            this.taskTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.taskTreeView_AfterSelect);
            // 
            // taskDataView
            // 
            this.taskDataView.AllowUserToAddRows = false;
            this.taskDataView.AllowUserToDeleteRows = false;
            this.taskDataView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.taskDataView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.taskDataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.taskDataView.ContextMenuStrip = this.contextMenuStripTaskGridView;
            this.taskDataView.Location = new System.Drawing.Point(155, 28);
            this.taskDataView.Name = "taskDataView";
            this.taskDataView.ReadOnly = true;
            this.taskDataView.RowTemplate.Height = 23;
            this.taskDataView.Size = new System.Drawing.Size(814, 409);
            this.taskDataView.TabIndex = 2;
            this.taskDataView.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.taskDataView_CellContentDoubleClick);
            // 
            // contextMenuStripTaskGridView
            // 
            this.contextMenuStripTaskGridView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRun,
            this.btnConfig,
            this.btnTaskTurnOn,
            this.btnTaskOff,
            this.btnTaskDelete,
            this.btnRemoveError});
            this.contextMenuStripTaskGridView.Name = "contextMenuStripTaskGridView";
            this.contextMenuStripTaskGridView.Size = new System.Drawing.Size(181, 158);
            // 
            // btnRun
            // 
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(180, 22);
            this.btnRun.Text = "立即运行";
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnConfig
            // 
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(180, 22);
            this.btnConfig.Text = "查看配置";
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // btnTaskTurnOn
            // 
            this.btnTaskTurnOn.Name = "btnTaskTurnOn";
            this.btnTaskTurnOn.Size = new System.Drawing.Size(180, 22);
            this.btnTaskTurnOn.Text = "启用";
            this.btnTaskTurnOn.Click += new System.EventHandler(this.btnTaskTurnOn_Click);
            // 
            // btnTaskOff
            // 
            this.btnTaskOff.Name = "btnTaskOff";
            this.btnTaskOff.Size = new System.Drawing.Size(180, 22);
            this.btnTaskOff.Text = "禁用";
            this.btnTaskOff.Click += new System.EventHandler(this.btnTaskOff_Click);
            // 
            // btnTaskDelete
            // 
            this.btnTaskDelete.Name = "btnTaskDelete";
            this.btnTaskDelete.Size = new System.Drawing.Size(180, 22);
            this.btnTaskDelete.Text = "删除";
            this.btnTaskDelete.Click += new System.EventHandler(this.btnTaskDelete_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(153, 440);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(816, 176);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tbLog);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(808, 147);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "运行日志";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tbLog
            // 
            this.tbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLog.Location = new System.Drawing.Point(3, 3);
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.Size = new System.Drawing.Size(802, 141);
            this.tbLog.TabIndex = 1;
            this.tbLog.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(2, 260);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 14);
            this.label1.TabIndex = 4;
            this.label1.Text = "系统公告";
            // 
            // tbSysLog
            // 
            this.tbSysLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tbSysLog.Location = new System.Drawing.Point(2, 277);
            this.tbSysLog.Name = "tbSysLog";
            this.tbSysLog.Size = new System.Drawing.Size(147, 335);
            this.tbSysLog.TabIndex = 5;
            this.tbSysLog.Text = "";
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenuStripNotify;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "项目申报爬虫App";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            // 
            // contextMenuStripNotify
            // 
            this.contextMenuStripNotify.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NotifyExit});
            this.contextMenuStripNotify.Name = "contextMenuStripNotify";
            this.contextMenuStripNotify.Size = new System.Drawing.Size(101, 26);
            // 
            // NotifyExit
            // 
            this.NotifyExit.Name = "NotifyExit";
            this.NotifyExit.Size = new System.Drawing.Size(100, 22);
            this.NotifyExit.Text = "退出";
            this.NotifyExit.Click += new System.EventHandler(this.NotifyExit_Click);
            // 
            // timerAuto
            // 
            this.timerAuto.Interval = 5000;
            this.timerAuto.Tick += new System.EventHandler(this.timerAuto_Tick);
            // 
            // btnRemoveError
            // 
            this.btnRemoveError.Name = "btnRemoveError";
            this.btnRemoveError.Size = new System.Drawing.Size(180, 22);
            this.btnRemoveError.Text = "报警移除";
            this.btnRemoveError.Click += new System.EventHandler(this.btnRemoveError_Click);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(969, 618);
            this.Controls.Add(this.tbSysLog);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.taskDataView);
            this.Controls.Add(this.taskTreeView);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(738, 477);
            this.Name = "mainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "项目申报信息爬虫App v1.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.taskDataView)).EndInit();
            this.contextMenuStripTaskGridView.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.contextMenuStripNotify.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem tbNewTask;
        private System.Windows.Forms.TreeView taskTreeView;
        private System.Windows.Forms.DataGridView taskDataView;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.RichTextBox tbLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox tbSysLog;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripNotify;
        private System.Windows.Forms.ToolStripMenuItem NotifyExit;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTaskGridView;
        private System.Windows.Forms.ToolStripMenuItem btnTaskTurnOn;
        private System.Windows.Forms.ToolStripMenuItem btnTaskOff;
        private System.Windows.Forms.ToolStripMenuItem btnTaskDelete;
        private System.Windows.Forms.Timer timerAuto;
        private System.Windows.Forms.ToolStripMenuItem btnRun;
        private System.Windows.Forms.ToolStripMenuItem btnConfig;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitBtnSys;
        private System.Windows.Forms.ToolStripMenuItem btnSetting;
        private System.Windows.Forms.ToolStripMenuItem btnRemoveError;
    }
}

