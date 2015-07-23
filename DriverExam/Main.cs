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
    public partial class Main :CCSkinMain
    {
        private string name;
        public Main()
        {
            InitializeComponent();
        }

        public Main(string username)
        {
            InitializeComponent();
            this.name = username;
        }

        private void 用户管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new UserManager().Show();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.系统设置ToolStripMenuItem.Visible = this.name == "admin";
        }

        private void 系统数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SectionManage().Show();
        }

        private void toolStripButtonSection_Click(object sender, EventArgs e)
        {
            new SelectSection().ShowDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            new Practice("顺序练习", "select distinct * from ExamSubject").Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            new Practice("随机练习", "select distinct * from ExamSubject","随机练习").Show();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            new Practice("单项练习", "select distinct * from ExamSubject where type='单项选择题'").Show();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            new Practice("判断题", "select distinct * from ExamSubject where type='判断题'").Show();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            new Practice("多项练习", "select distinct * from ExamSubject where type='多项选择题'").Show();
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            if (new Tool().ExecuteSqlQuery("select a.* from ExamSubject a left join ExamErrorSubject b on a.id= b.subject_id where b.user_id = '" + Login.USERID + "'").Rows.Count < 1)
            {
                MessageBoxEx.Show("您还没有错题");
                return;
            }
            new Practice("我的错题", "select a.* from ExamSubject a left join ExamErrorSubject b on a.id= b.subject_id where b.user_id = '"+Login.USERID+"'").Show();
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            new PracticeTest().Show();
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            if (new Tool().ExecuteSqlQuery("select a.* from ExamSubject a left join ExamCollect b on a.id= b.subject_id where b.user_id = '" + Login.USERID + "'").Rows.Count < 1)
            {
                MessageBoxEx.Show("您还没有收藏的题目");
                return;
            }
            new Practice("我的收藏", "select a.* from ExamSubject a left join ExamCollect b on a.id= b.subject_id where b.user_id = '" + Login.USERID + "'").Show();
        }

        private void excel导入题库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Tool().importExcel();
        }

        private void 手动添加题库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AddSubject().Show();
        }

        private void toolStripButton6_Click_1(object sender, EventArgs e)
        {
            new Practice("图片题练习", "select distinct * from ExamSubject where not picture_name  is null and picture_name <> ''").Show();
        }
    }
}
