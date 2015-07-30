using CCWin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DriverExam
{
    public partial class Skills : CCSkinMain
    {
        private DataTable dt;
        public Skills()
        {
            InitializeComponent();
        }

        private void cbSkillName_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblquestion.Text = dt.Rows[cbSkillName.SelectedIndex]["details"].ToString();
        }

        private void Skills_Load(object sender, EventArgs e)
        {
            LoadDefault();
        }

        private void LoadDefault()
        {
            string sqlString = "select * from ExamSkill";
            dt = new Tool().ExecuteSqlQuery(sqlString);
            cbSkillName.DataSource = dt;
            cbSkillName.ValueMember = "id";
            cbSkillName.DisplayMember = "name";
        }
    }
}
