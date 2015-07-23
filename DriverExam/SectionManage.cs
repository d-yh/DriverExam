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
    public partial class SectionManage : CCSkinMain
    {
        public SectionManage()
        {
            InitializeComponent();
        }

        private void SectionManage_Load(object sender, EventArgs e)
        {
            LoadDefault();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            new EditSectionManage().ShowDialog();
            LoadDefault();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                new EditSectionManage(dgRuleTable.CurrentRow.Cells["id"].Value.ToString()).ShowDialog();
                LoadDefault();
            }
            catch
            {
                MessageBoxEx.Show("请选择要修改的一行数据");
            }            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("警告,删除该章节会删除该章节所有的题目，您确认删除？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string RuleNumber = dgRuleTable.CurrentRow.Cells["id"].Value.ToString();
                    string userName = dgRuleTable.CurrentRow.Cells["章节类别"].Value.ToString();
                    string sqlString = "delete ExamSection where id='" + RuleNumber + "'";
                    new Tool().ExecNonSQLQuery(sqlString);
                    sqlString = "delete ExamSubject where section='"+RuleNumber+"'";
                    new Tool().ExecNonSQLQuery(sqlString);
                    LoadDefault();
                }
            }
            catch
            {
                MessageBoxEx.Show("请选择要删除的一行数据");
            }
            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadDefault();
        }

        private void LoadDefault()
        {
            string sqlString = "select id,section as 章节类别 from ExamSection where section like '%<section>%'order by section";
            sqlString = sqlString.Replace("<section>",txtSearch.Text.Trim());
            dgRuleTable.DataSource = new Tool().ExecuteSqlQuery(sqlString);
            dgRuleTable.Columns["id"].Visible = false;
            if (dgRuleTable.RowCount < 1)
            {
                this.btnEdit.Enabled = this.btnDelete.Enabled = false;
            }
            else
            {
                this.btnEdit.Enabled = this.btnDelete.Enabled = true;
            }
        }
    }
}
