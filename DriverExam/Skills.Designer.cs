namespace DriverExam
{
    partial class Skills
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
            this.skinLabel1 = new CCWin.SkinControl.SkinLabel();
            this.cbSkillName = new CCWin.SkinControl.SkinComboBox();
            this.skinPanel1 = new CCWin.SkinControl.SkinPanel();
            this.lblquestion = new System.Windows.Forms.Label();
            this.skinLabel2 = new CCWin.SkinControl.SkinLabel();
            this.skinPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // skinLabel1
            // 
            this.skinLabel1.AutoSize = true;
            this.skinLabel1.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel1.BorderColor = System.Drawing.Color.White;
            this.skinLabel1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel1.Location = new System.Drawing.Point(8, 32);
            this.skinLabel1.Name = "skinLabel1";
            this.skinLabel1.Size = new System.Drawing.Size(56, 17);
            this.skinLabel1.TabIndex = 0;
            this.skinLabel1.Text = "请选择：";
            // 
            // cbSkillName
            // 
            this.cbSkillName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbSkillName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSkillName.FormattingEnabled = true;
            this.cbSkillName.Location = new System.Drawing.Point(70, 32);
            this.cbSkillName.Name = "cbSkillName";
            this.cbSkillName.Size = new System.Drawing.Size(121, 22);
            this.cbSkillName.TabIndex = 11;
            this.cbSkillName.WaterText = "";
            this.cbSkillName.SelectedIndexChanged += new System.EventHandler(this.cbSkillName_SelectedIndexChanged);
            // 
            // skinPanel1
            // 
            this.skinPanel1.BackColor = System.Drawing.Color.Transparent;
            this.skinPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.skinPanel1.Controls.Add(this.lblquestion);
            this.skinPanel1.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.skinPanel1.DownBack = null;
            this.skinPanel1.Location = new System.Drawing.Point(4, 98);
            this.skinPanel1.MouseBack = null;
            this.skinPanel1.Name = "skinPanel1";
            this.skinPanel1.NormlBack = null;
            this.skinPanel1.Size = new System.Drawing.Size(567, 301);
            this.skinPanel1.TabIndex = 12;
            // 
            // lblquestion
            // 
            this.lblquestion.BackColor = System.Drawing.Color.White;
            this.lblquestion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblquestion.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Bold);
            this.lblquestion.Location = new System.Drawing.Point(0, 0);
            this.lblquestion.Name = "lblquestion";
            this.lblquestion.Size = new System.Drawing.Size(565, 299);
            this.lblquestion.TabIndex = 4;
            this.lblquestion.Text = "A";
            // 
            // skinLabel2
            // 
            this.skinLabel2.AutoSize = true;
            this.skinLabel2.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel2.BorderColor = System.Drawing.Color.White;
            this.skinLabel2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel2.Location = new System.Drawing.Point(8, 75);
            this.skinLabel2.Name = "skinLabel2";
            this.skinLabel2.Size = new System.Drawing.Size(68, 17);
            this.skinLabel2.TabIndex = 13;
            this.skinLabel2.Text = "秘籍内容：";
            // 
            // Skills
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(208)))), ((int)(((byte)(255)))));
            this.CanResize = false;
            this.ClientSize = new System.Drawing.Size(575, 403);
            this.Controls.Add(this.skinLabel2);
            this.Controls.Add(this.skinPanel1);
            this.Controls.Add(this.cbSkillName);
            this.Controls.Add(this.skinLabel1);
            this.MaximizeBox = false;
            this.Name = "Skills";
            this.Text = "过关秘籍";
            this.Load += new System.EventHandler(this.Skills_Load);
            this.skinPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CCWin.SkinControl.SkinLabel skinLabel1;
        private CCWin.SkinControl.SkinComboBox cbSkillName;
        private CCWin.SkinControl.SkinPanel skinPanel1;
        private System.Windows.Forms.Label lblquestion;
        private CCWin.SkinControl.SkinLabel skinLabel2;
    }
}