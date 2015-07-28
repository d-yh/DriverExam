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
    public partial class AddSubject : CCSkinMain
    {
        public AddSubject()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            new EditSubject().ShowDialog();
            LoadDefault();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                new EditSubject(dgRuleTable.CurrentRow.Cells["id"].Value.ToString()).ShowDialog();
                LoadDefault();
            }
            catch
            {
                MessageBox.Show("请选择一行");
            }
           
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("警告,您确认删除该题目，您确认删除？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string RuleNumber = dgRuleTable.CurrentRow.Cells["id"].Value.ToString();
                    string sqlString = "delete ExamSubject where id='" + RuleNumber + "'";
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

        private void AddSubject_Load(object sender, EventArgs e)
        {
            LoadDefault();
        }

        private void LoadDefault()
        {
            string sqlString = "select a.id,b.section as 章节类别,topic as 题目主题,type as 题目类型,question as 题目问题,picture_name as 图片名称,option_a as 选项A,option_b as 选项B,option_c as 选项C,option_d as 选项D,answer as 答案,error_detail as 题目详解 from ExamSubject a left join ExamSection b on a.section = b.id where a.question like '%" + txtSearch.Text + "%'";
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
