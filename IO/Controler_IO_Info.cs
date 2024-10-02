using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using cszmcaux;
using 筛选机指令测试工具;

namespace CONTROLER_IO
{
    public partial class Controler_IO_Info : Form
    {
        public int num;

        public int select_num;

        public Controler_IO_Info()
        {
            InitializeComponent();

        }


        private void Controler_IO_Info_Load(object sender, EventArgs e)
        {
            add_in_num();

            invert_add();

            out_select_add();

            out_label_add();

            In_Timer.Enabled = true;

            Op_Timer.Enabled = false;

        }


        //给IN口的添加子集合
        public void add_in_num()
        {
            int add_num;

            for (add_num = 0; add_num < 256; add_num++)
            {
                String in_for = (add_num * 16).ToString();

                String in_back = ((add_num * 16) + 15).ToString();

                String connet = "-";

                String name = String.Concat(in_for, connet, in_back);

                in_num.Items.Add(name);



            }

        }

        //翻转口添加子集合
        public void invert_add()
        {
            int i;

            invert_num.Items.Clear();

            invert_num.Text = "";

            for (i = 0; i < 16; i++)
            {
                String invert_text = ((num * 16) + i).ToString();
                invert_num.Items.Add(invert_text);
            }
        }



        //输出口选择批量添加集合
        public void out_select_add()
        {
            int i;

            for (i = 0; i < 256; i++)
            {
                String in_for = (i * 16).ToString();

                String in_back = ((i * 16) + 15).ToString();

                String connet = "-";

                String name = String.Concat(in_for, connet, in_back);

                out_num.Items.Add(name);
            }
        }


        //输出口标识符label新建添加

        public void out_label_add()
        {
            int i;
            for (i = 0; i < 16; i++)
            {
                Label label = new Label();

                label.Name = "out" + i.ToString();

                label.Text = "out" + i.ToString();

                label.Size = new Size(100, 12);

                label.Location = new Point(120 + 145 * (i % 4), 24 + 100 * (i / 4));

                this.panel6.Controls.Add(label);



            }
        }

        /// <summary>
        /// 输入口选择序号改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void in_num_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 15;


            num = Convert.ToInt32(in_num.SelectedIndex);

            foreach (Control c in this.groupBox4.Controls)
            {
                if (c is Label && c.Name.StartsWith("in"))
                {
                    c.Text = "in" + ((num * 16) + i).ToString();

                    --i;

                }
            }

            invert_add();   //刷新翻转输入combox 信息
        }


        /// <summary>
        /// 输出口选择序号改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void out_num_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;

            select_num = Convert.ToInt32(out_num.SelectedIndex);

            foreach (Control a in this.panel6.Controls)
            {
                if (a is Label && a.Name.StartsWith("out"))
                {

                    a.Text = "out" + ((select_num * 16) + i).ToString();

                    i++;

                }
            }
        }



        /// <summary>
        /// 定时器获取输入口状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void In_Timer_Tick(object sender, EventArgs e)
        {
            uint[] instatus = new uint[16];

            int[] invertstatus = new int[16];

            Graphics g = this.groupBox4.CreateGraphics();

            Pen redPen = new Pen(Color.Red, 10);
            Pen greenPen = new Pen(Color.Green, 10);


            if (Form1.g_handle != (IntPtr)0)
            {

                int i;

                for (i = (num * 16); i <= ((num * 16) + 15); i++)
                {

                    zmcaux.ZAux_Direct_GetIn(Form1.g_handle,i, ref instatus[i - (num * 16)]);

                    zmcaux.ZAux_Direct_GetInvertIn(Form1.g_handle,i, ref invertstatus[i - (num * 16)]);

                }




                g = this.groupBox4.CreateGraphics();


                for (i = 0; i < 16; i++)
                {
                    if (instatus[i] == 0)
                    {
                        g.DrawEllipse(redPen, new Rectangle(120 + 145 * (i % 4), 59 + 100 * (i / 4), 10, 10));   //红色，无信号
                    }
                    else
                    {
                        g.DrawEllipse(greenPen, new Rectangle(120 + 145 * (i % 4), 59 + 100 * (i / 4), 10, 10));   //绿色
                    }

                    if (invertstatus[i] == 0)
                    {
                        g.DrawEllipse(redPen, new Rectangle(120 + 145 * (i % 4), 97 + 100 * (i / 4), 10, 10));   //红色，无信号
                    }
                    else
                    {
                        g.DrawEllipse(greenPen, new Rectangle(120 + 145 * (i % 4), 97 + 100 * (i / 4), 10, 10));   //绿色
                    }



                }


            }

        }


        /// <summary>
        /// 翻转IN
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void invert_button_Click(object sender, EventArgs e)
        {
            int invert_in = 0;

            if (!string.IsNullOrWhiteSpace(invert_num.Text))
            {
                zmcaux.ZAux_Direct_GetInvertIn(Form1.g_handle, Convert.ToInt32(invert_num.Text), ref invert_in);

                if (invert_in == 0)
                {
                    zmcaux.ZAux_Direct_SetInvertIn(Form1.g_handle, Convert.ToInt32(invert_num.Text), 1);
                }
                else
                {
                    zmcaux.ZAux_Direct_SetInvertIn(Form1.g_handle, Convert.ToInt32(invert_num.Text), 0);
                }


            }


        }

        private void Op_Timer_Tick(object sender, EventArgs e)
        {
            Graphics h = this.panel6.CreateGraphics();
            Pen redPen = new Pen(Color.Red, 10);
            Pen greenPen = new Pen(Color.Green, 10);

            uint[] outstate = new uint[16];


            if (Form1.g_handle != (IntPtr)0)
            {

                int i;

                h = this.panel6.CreateGraphics();

                for (i = (select_num * 16); i <= ((select_num * 16) + 15); i++)
                {

                    zmcaux.ZAux_Direct_GetOp(Form1.g_handle, i, ref outstate[i - (select_num * 16)]);

                }


                for (i = 0; i < 16; i++)
                {
                    //h = this.count_op[i].CreateGraphics();

                    foreach (Label c in panel6.Controls)
                    {
                        if (c.Name == "op" + i.ToString())
                        {
                            h = c.CreateGraphics();

                            if (outstate[i] == 0)
                            {
                                h.DrawEllipse(redPen, new Rectangle(10, 10, 10, 10));   //红色，无信号
                            }
                            else
                            {
                                h.DrawEllipse(greenPen, new Rectangle(10, 10, 10, 10));   //绿色
                            }
                        }
                    }






                }




            }
        }


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tab = (TabControl)sender;

            if (tab.SelectedIndex == 0)
            {
                In_Timer.Enabled = true;

                Op_Timer.Enabled = false;
            }
            else if (tab.SelectedIndex == 1)
            {
                In_Timer.Enabled = false;

                Op_Timer.Enabled = true;
            }
        }


        public void OP_Click(Label c)
        {
            int op = (select_num * 16) + Convert.ToInt32(c.Name.Substring(2));

            uint op_status = 0;

           zmcaux.ZAux_Direct_GetOp(Form1.g_handle, op, ref op_status);

            if (op_status == 0)
            {
                zmcaux.ZAux_Direct_SetOp(Form1.g_handle, op, 1);
            }
            else
            {
                zmcaux.ZAux_Direct_SetOp(Form1.g_handle, op, 0);
            }

        }

        private void op0_Click(object sender, EventArgs e)
        {
            Label c = (Label)sender;

            OP_Click(c);
        }

        private void op1_Click(object sender, EventArgs e)
        {
            Label c = (Label)sender;

            OP_Click(c);
        }

        private void op2_Click(object sender, EventArgs e)
        {
            Label c = (Label)sender;

            OP_Click(c);
        }

        private void op3_Click(object sender, EventArgs e)
        {
            Label c = (Label)sender;

            OP_Click(c);
        }

        private void op4_Click(object sender, EventArgs e)
        {
            Label c = (Label)sender;

            OP_Click(c);
        }

        private void op5_Click(object sender, EventArgs e)
        {
            Label c = (Label)sender;

            OP_Click(c);
        }

        private void op6_Click(object sender, EventArgs e)
        {
            Label c = (Label)sender;

            OP_Click(c);
        }

        private void op7_Click(object sender, EventArgs e)
        {
            Label c = (Label)sender;

            OP_Click(c);
        }

        private void op8_Click(object sender, EventArgs e)
        {
            Label c = (Label)sender;

            OP_Click(c);
        }

        private void op9_Click(object sender, EventArgs e)
        {
            Label c = (Label)sender;

            OP_Click(c);
        }

        private void op10_Click(object sender, EventArgs e)
        {
            Label c = (Label)sender;

            OP_Click(c);
        }

        private void op11_Click(object sender, EventArgs e)
        {
            Label c = (Label)sender;

            OP_Click(c);
        }

        private void op12_Click(object sender, EventArgs e)
        {
            Label c = (Label)sender;

            OP_Click(c);
        }

        private void op13_Click(object sender, EventArgs e)
        {
            Label c = (Label)sender;

            OP_Click(c);
        }

        private void op14_Click(object sender, EventArgs e)
        {
            Label c = (Label)sender;

            OP_Click(c);
        }

        private void op15_Click(object sender, EventArgs e)
        {
            Label c = (Label)sender;

            OP_Click(c);
        }

        private void Controler_IO_Info_FormClosing(object sender, FormClosingEventArgs e)
        {
            In_Timer.Enabled = false;

            Op_Timer.Enabled = false;
        }

        private void All_Open_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 16; i++)
            {
                zmcaux.ZAux_Direct_SetOp(Form1.g_handle,(select_num * 16) + i, 1);
            }
        }

        private void ALL_Close_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 16; i++)
            {
                zmcaux.ZAux_Direct_SetOp(Form1.g_handle,(select_num * 16) + i, 0);
            }
        }
    }
}
