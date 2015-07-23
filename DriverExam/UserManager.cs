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
    public partial class UserManager : CCSkinMain
    {
        public UserManager()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            new EditUserManager().ShowDialog();
            LoadDefault();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                new EditUserManager(dgRuleTable.CurrentRow.Cells["id"].Value.ToString()).ShowDialog();
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
                string userName = dgRuleTable.CurrentRow.Cells["用户名"].Value.ToString();

                if (MessageBox.Show("你确定要删除:" + userName + "这个用户吗?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string SqlString = "delete ExamUser where id = '" + RuleNumber + "'";
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

        private void UserManager_Load(object sender, EventArgs e)
        {
            LoadDefault();
        }

        private void LoadDefault()
        {
            string sqlString = "select id,name as 用户名,real_name as 用户姓名 from ExamUser where real_name like '%<real_name>%'order by name";
            sqlString = sqlString.Replace("<real_name>", txtSearch.Text.Trim());
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
