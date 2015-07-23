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
    public partial class SelectSection : CCSkinMain
    {
        public SelectSection()
        {
            InitializeComponent();
        }

        private void SelectSection_Load(object sender, EventArgs e)
        {
            string sqlString = "select * from ExamSection";
            DataTable dt = new Tool().ExecuteSqlQuery(sqlString);
            cbSection.DataSource = dt;
            cbSection.ValueMember = "id";
            cbSection.DisplayMember = "section";
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            if (new Tool().ExecuteSqlQuery("select *from ExamSubject where section='" + cbSection.SelectedValue.ToString() + "'").Rows.Count<1)
            {
                MessageBoxEx.Show("该章节没有任何题目,请添加题目");
                return;
            }
            new Practice("章节练习", "select *from ExamSubject where section='" + cbSection.SelectedValue.ToString() + "'").Show();
            Close();
        }
    }
}
