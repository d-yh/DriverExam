namespace DriverExam
{
    partial class SelectSection
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
            this.cbSection = new CCWin.SkinControl.SkinComboBox();
            this.skinLabel1 = new CCWin.SkinControl.SkinLabel();
            this.btnSure = new CCWin.SkinControl.SkinButton();
            this.SuspendLayout();
            // 
            // cbSection
            // 
            this.cbSection.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbSection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSection.FormattingEnabled = true;
            this.cbSection.Location = new System.Drawing.Point(89, 42);
            this.cbSection.Name = "cbSection";
            this.cbSection.Size = new System.Drawing.Size(175, 22);
            this.cbSection.TabIndex = 0;
            this.cbSection.WaterText = "";
            // 
            // skinLabel1
            // 
            this.skinLabel1.AutoSize = true;
            this.skinLabel1.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel1.BorderColor = System.Drawing.Color.White;
            this.skinLabel1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel1.Location = new System.Drawing.Point(7, 42);
            this.skinLabel1.Name = "skinLabel1";
            this.skinLabel1.Size = new System.Drawing.Size(68, 17);
            this.skinLabel1.TabIndex = 1;
            this.skinLabel1.Text = "章节类别：";
            // 
            // btnSure
            // 
            this.btnSure.BackColor = System.Drawing.Color.Transparent;
            this.btnSure.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnSure.DownBack = null;
            this.btnSure.Location = new System.Drawing.Point(207, 74);
            this.btnSure.MouseBack = null;
            this.btnSure.Name = "btnSure";
            this.btnSure.NormlBack = null;
            this.btnSure.Size = new System.Drawing.Size(57, 23);
            this.btnSure.TabIndex = 2;
            this.btnSure.Text = "确定";
            this.btnSure.UseVisualStyleBackColor = false;
            this.btnSure.Click += new System.EventHandler(this.btnSure_Click);
            // 
            // SelectSection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CanResize = false;
            this.ClientSize = new System.Drawing.Size(284, 104);
            this.Controls.Add(this.btnSure);
            this.Controls.Add(this.skinLabel1);
            this.Controls.Add(this.cbSection);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectSection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "章节选择";
            this.Load += new System.EventHandler(this.SelectSection_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CCWin.SkinControl.SkinComboBox cbSection;
        private CCWin.SkinControl.SkinLabel skinLabel1;
        private CCWin.SkinControl.SkinButton btnSure;
    }
}