﻿using CCWin;
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
            this.系统数据ToolStripMenuItem.Visible = false;
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
            new Practice("四川省机动车教练员从业考试操作系统-顺序练习", "select distinct * from ExamSubject").Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            new Practice("四川省机动车教练员从业考试操作系统-随机练习", "select distinct * from ExamSubject", "随机练习").Show();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            new Practice("四川省机动车教练员从业考试操作系统-单项练习", "select distinct * from ExamSubject where type='单项选择题'").Show();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            new Practice("四川省机动车教练员从业考试操作系统-判断题", "select distinct * from ExamSubject where type='判断题'").Show();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            new Practice("四川省机动车教练员从业考试操作系统-多项练习", "select distinct * from ExamSubject where type='多项选择题'").Show();
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            if (new Tool().ExecuteSqlQuery("select distinct a.* from ExamSubject a left join ExamErrorSubject b on a.id= b.subject_id where b.user_id = '" + Login.USERID + "'").Rows.Count < 1)
            {
                MessageBoxEx.Show("您还没有错题");
                return;
            }
            new Practice("四川省机动车教练员从业考试操作系统-我的错题", "select distinct a.* from ExamSubject a left join ExamErrorSubject b on a.id= b.subject_id where b.user_id = '" + Login.USERID + "'").Show();
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
            new Practice("四川省机动车教练员从业考试操作系统-我的收藏", "select a.* from ExamSubject a left join ExamCollect b on a.id= b.subject_id where b.user_id = '" + Login.USERID + "'",false).Show();
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
            new Practice("四川省机动车教练员从业考试操作系统-图片题练习", "select distinct * from ExamSubject where not picture_name  is null and picture_name <> ''").Show();
        }

        private void MenuItemBtnSingle_order_Click(object sender, EventArgs e)
        {
            new Practice("四川省机动车教练员从业考试操作系统-单项练习-顺序练习", "select distinct * from ExamSubject where type='单项选择题'").Show();
        }

        private void MenuItemBtnSingle_random_Click(object sender, EventArgs e)
        {
            new Practice("四川省机动车教练员从业考试操作系统-单项练习-随机练习", "select distinct * from ExamSubject where type='单项选择题'", "随机练习").Show();
        }

        private void MenuItemBtnSingle_difficult_Click(object sender, EventArgs e)
        {

            new Practice("四川省机动车教练员从业考试操作系统-单项练习-难题强化", "select * from ExamSubject where type = '单项选择题' and problem='难题强化'").Show();
        }

        private void MenuItemBtnJudge_order_Click(object sender, EventArgs e)
        {
            new Practice("四川省机动车教练员从业考试操作系统-判断练习-顺序练习", "select distinct * from ExamSubject where type='判断题'").Show();
        }

        private void MenuItemBtnJudge_random_Click(object sender, EventArgs e)
        {
            new Practice("四川省机动车教练员从业考试操作系统-判断练习-随机练习", "select distinct * from ExamSubject where type='判断题'", "随机练习").Show();
        }

        private void MenuItemBtnJudge_difficult_Click(object sender, EventArgs e)
        {
            new Practice("四川省机动车教练员从业考试操作系统-判断练习-难题强化", "select * from ExamSubject where type = '判断题' and problem='难题强化'").Show();
        }

        private void MenuItemBtnMore_order_Click(object sender, EventArgs e)
        {
            new Practice("四川省机动车教练员从业考试操作系统-多项练习-顺序练习", "select distinct * from ExamSubject where type='多项选择题'").Show();
        }

        private void MenuItemBtnMore_random_Click(object sender, EventArgs e)
        {
            new Practice("四川省机动车教练员从业考试操作系统-多项练习-随机练习", "select distinct * from ExamSubject where type='多项选择题'", "随机练习").Show();
        }

        private void MenuItemBtnMore_difficult_Click(object sender, EventArgs e)
        {
            new Practice("四川省机动车教练员从业考试操作系统-多项练习-难题强化", "select * from ExamSubject where type = '多项选择题' and problem='难题强化'").Show();
        }

        private void toolStripButton过关秘籍_Click(object sender, EventArgs e)
        {
            new Skills().Show();
        }

        private void 过关秘籍管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ExamSkillManage().Show();
        }

        private void 清空题库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Tool().ExecNonSQLQuery("delete ExamSubject");
            MessageBoxEx.Show("题库已经清空");
        }

        private void 清除我的收藏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Tool().ExecNonSQLQuery("delete ExamCollect where user_id = '" + Login.USERID + "'");
            MessageBoxEx.Show("我的收藏已经清空");
        }

        private void 清空我的错题ToolStripMenuItem_Click(object sender, EventArgs e)
        {          
            new Tool().ExecNonSQLQuery("delete ExamErrorSubject where user_id = '" + Login.USERID + "'");
            MessageBoxEx.Show("我的错题已经清空");
        }
    }
}
