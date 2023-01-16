namespace assignment07_WinDEXT
{
    partial class InfoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfoForm));
            this.btnOk = new System.Windows.Forms.Button();
            this.grpBxInfo = new System.Windows.Forms.GroupBox();
            this.lstBxProductInfo = new System.Windows.Forms.ListBox();
            this.formsPlot = new ScottPlot.FormsPlot();
            this.toolTipInfo = new System.Windows.Forms.ToolTip(this.components);
            this.grpBxInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(373, 470);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Close";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // grpBxInfo
            // 
            this.grpBxInfo.Controls.Add(this.lstBxProductInfo);
            this.grpBxInfo.Location = new System.Drawing.Point(12, 254);
            this.grpBxInfo.Name = "grpBxInfo";
            this.grpBxInfo.Size = new System.Drawing.Size(459, 200);
            this.grpBxInfo.TabIndex = 1;
            this.grpBxInfo.TabStop = false;
            this.grpBxInfo.Text = "groupBox1";
            // 
            // lstBxProductInfo
            // 
            this.lstBxProductInfo.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lstBxProductInfo.FormattingEnabled = true;
            this.lstBxProductInfo.ItemHeight = 15;
            this.lstBxProductInfo.Location = new System.Drawing.Point(6, 22);
            this.lstBxProductInfo.Name = "lstBxProductInfo";
            this.lstBxProductInfo.Size = new System.Drawing.Size(444, 169);
            this.lstBxProductInfo.TabIndex = 0;
            // 
            // formsPlot
            // 
            this.formsPlot.Location = new System.Drawing.Point(-14, 0);
            this.formsPlot.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.formsPlot.Name = "formsPlot";
            this.formsPlot.Size = new System.Drawing.Size(502, 270);
            this.formsPlot.TabIndex = 2;
            // 
            // InfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 510);
            this.Controls.Add(this.grpBxInfo);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.formsPlot);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "InfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "InfoForm";
            this.grpBxInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnOk;
        private GroupBox grpBxInfo;
        private ListBox lstBxProductInfo;
        private ScottPlot.FormsPlot formsPlot;
        private ToolTip toolTipInfo;
    }
}