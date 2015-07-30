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
    public partial class ExamSkillManage : CCSkinMain
    {
        public ExamSkillManage()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            new EditExamSkillManage().ShowDialog();
            LoadDefault();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                new EditExamSkillManage(dgRuleTable.CurrentRow.Cells["id"].Value.ToString()).ShowDialog();
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
                string RuleNumber = dgRuleTable.CurrentRow.Cells["id"].Value.ToString();
                string userName = dgRuleTable.CurrentRow.Cells["秘籍名称"].Value.ToString();

                if (MessageBox.Show("你确定要删除:" + userName + "这个秘籍吗?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string SqlString = "delete ExamSkill where id = '" + RuleNumber + "'";
                    new Tool().ExecNonSQLQuery(SqlString);
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
            string sqlString = "select id,name as 秘籍名称,details as 秘籍内容 from ExamSkill where name like '%<name>%'order by name";
            sqlString = sqlString.Replace("<name>", txtSearch.Text.Trim());
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

        private void ExamSkillManage_Load(object sender, EventArgs e)
        {
            LoadDefault();
        }
    }
}
