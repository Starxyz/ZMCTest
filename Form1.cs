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
using cszmc_xp;
using Box;
using 筛选机指令测试工具;
using System.Threading;
using System.IO;
using CONTROLER_IO;


namespace 筛选机指令测试工具
{
    public partial class Form1 : Form
    {
        public static Form1 f1 = null;

        public static float wind_x, wind_y;

        public static string main_filepath = Environment.CurrentDirectory +  "/主界面参数.ini";

        public static IntPtr g_handle;

        public static int Inits_flag = 0;

        public Label[] f_r_datum_in = new Label[6];


        public int axisnum = 128;    //轴数


        public int[] axis = new int[128];

        public int[] atype = new int[128];

        public float[] units = new float[128];

        public float[] dir = new float[128];

        public float[] pul = new float[128];

        public float[] dis_angle = new float[128];

        public float[] speed = new float[128];

        public float[] accel = new float[128];

        public float[] decel = new float[128];


        public float[] fs_limit = new float[128];

        public float[] rs_limit = new float[128];

        public int[] fwd_in = new int[128];

        public int[] rev_in = new int[128];

        public int[] datum_in = new int[128];

        public uint[] axis_re_result = new uint[6];


        public float[] creep = new float[128];

        public int[] datum_modes = new int[128];

        public int re_in = 0;


        public int datum_flag = 0;


        /*    IO 状态储存数组     */


        public int[] even_state = new int[4096];

        public uint[] instate = new uint[4096];

        public uint[] outstate = new uint[4096];



        public int virtul_count_set;

        public float[] virtual_array = new float[128]; 


        public static Thread runtask = new Thread(new ThreadStart(下发排料));


        #region ////系统函数

        public Form1()
        {
            InitializeComponent();

            cszmcaux.zmcaux.ZAux_FastOpen(2, "192.168.0.236", 1000, out g_handle);

            if (g_handle != (IntPtr)0)
            {
                MessageBox.Show("控制器已连接");
            }

           int ret =  cszmc_xp.cszmc_xp.PC_INT_CARD(g_handle);

            if (ret == 0)
            {
                MessageBox.Show("筛选程序已加载");
            }

            SYS_STATUS.Text = "系统就绪，未运行";

            f1 = this;

            Thread.Sleep(1000);

            mian_timer.Enabled = true;

            wind_x = this.Width;

            wind_y = this.Height;

        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            Size_set size_Set = new Size_set();

            float newx = (this.Width) / wind_x; //窗体宽度缩放比例
            float newy = (this.Height) / wind_y;//窗体高度缩放比例
            size_Set.setControls(newx, newy, this);//随窗体改变控件大小
        }

        private void Form1_Load(object sender, EventArgs e)
        {
   
            try
            {
                Param_READ();

                for (int i = 0; i < Convert.ToInt32( CAM_NUM.Text); i++)
                {

                    listView1.Items.Add(i.ToString());

                    CCD_NUM.Items.Add(i.ToString());

                }

                for (int i = 0; i < Convert.ToInt32(OP_NUM.Text); i++)
                {
                    listView2.Items.Add(i.ToString());

                    IO_NUM.Items.Add(i.ToString());
                }

            }
            catch (Exception rt)
            {
                MessageBox.Show(rt.ToString());
            }

            HARD_CONNECT_READ();

            CONNECT_CONNECT_READ();


            Inits_flag = 1;


            f_r_datum_in[0] = cur_fwd_states;

            f_r_datum_in[1] = cur_rev_states;

            f_r_datum_in[2] = cur_datum_states;


            Size_set size_Set = new Size_set();

            size_Set.setTag(this);

        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
           // Application.Exit();
        }


        #endregion


        #region   //主界面操作函数

        /***************主界面********************/
        public void Param_Write()
        {
            

            if (!File.Exists(main_filepath))
            {
                FileStream srt = new FileStream(main_filepath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);

                srt.Close();

                Box.IniFile.IniWrite(main_filepath, "相机数", 1.ToString(), main_filepath);

                Box.IniFile.IniWrite(main_filepath, "排料数", 1.ToString(), main_filepath);

                Box.IniFile.IniWrite(main_filepath, "通讯模式", 2.ToString(), main_filepath);

                Box.IniFile.IniWrite(main_filepath, "取样间隔时间", 0.ToString(), main_filepath);

                Box.IniFile.IniWrite(main_filepath, "心跳检测时间", 0.ToString(), main_filepath);

                Box.IniFile.IniWrite(main_filepath, "模拟排料", "False", main_filepath);

                Box.IniFile.IniWrite(main_filepath, "光纤比例偏移", 0.ToString(), main_filepath);

                Box.IniFile.IniWrite(main_filepath, "延迟启动/停止时间", 0.ToString(), main_filepath);


                Box.IniFile.IniWrite(main_filepath, "通讯格式1", "False", main_filepath);

                Box.IniFile.IniWrite(main_filepath, "通讯格式2", "True", main_filepath);

            }

            Box.IniFile.IniWrite(main_filepath, "相机数", CAM_NUM.Text, main_filepath);

            Box.IniFile.IniWrite(main_filepath, "排料数", OP_NUM.Text, main_filepath);

            Box.IniFile.IniWrite(main_filepath, "通讯模式", CONNECT_MODE.SelectedIndex.ToString(), main_filepath);

            Box.IniFile.IniWrite(main_filepath, "取样间隔时间", SCOPE_TIME.Text, main_filepath);

            Box.IniFile.IniWrite(main_filepath, "心跳检测时间", HEART_TIME.Text, main_filepath);

            Box.IniFile.IniWrite(main_filepath, "模拟排料", 模拟排料.Checked.ToString(), main_filepath);

            Box.IniFile.IniWrite(main_filepath, "光纤比例偏移", Size_Pul_VR_NUM.Text.ToString(), main_filepath);

            Box.IniFile.IniWrite(main_filepath, "延迟启动/停止时间", PRTECT_TIME.Text, main_filepath);


            Box.IniFile.IniWrite(main_filepath, "通讯格式1", PC_MODBUS_SET.Checked.ToString(), main_filepath);

            Box.IniFile.IniWrite(main_filepath, "通讯格式2", PC_MODBUS_SET2.Checked.ToString(), main_filepath);

        }

        public void Param_READ()
        {


            if (!File.Exists(main_filepath))
            {
                FileStream srt = new FileStream(main_filepath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);

                srt.Close();

                Box.IniFile.IniWrite(main_filepath, "相机数", 1.ToString(), main_filepath);

                Box.IniFile.IniWrite(main_filepath, "排料数", 1.ToString(), main_filepath);

                Box.IniFile.IniWrite(main_filepath, "通讯模式", 2.ToString(), main_filepath);

                Box.IniFile.IniWrite(main_filepath, "取样间隔时间", 0.ToString(), main_filepath);

                Box.IniFile.IniWrite(main_filepath, "心跳检测时间", 0.ToString(), main_filepath);

                Box.IniFile.IniWrite(main_filepath, "模拟排料", "False", main_filepath);

                Box.IniFile.IniWrite(main_filepath, "光纤比例偏移", 0.ToString(), main_filepath);

                Box.IniFile.IniWrite(main_filepath, "延迟启动/停止时间", 0.ToString(), main_filepath);


                Box.IniFile.IniWrite(main_filepath, "通讯格式1", "False", main_filepath);

                Box.IniFile.IniWrite(main_filepath, "通讯格式2", "True", main_filepath);

            }

            CAM_NUM.Text = Box.IniFile.IniReadValue(main_filepath, "相机数",  main_filepath);

            OP_NUM.Text =  Box.IniFile.IniReadValue(main_filepath, "排料数", main_filepath);

            CONNECT_MODE.SelectedIndex = Convert.ToInt32( Box.IniFile.IniReadValue(main_filepath, "通讯模式", main_filepath));

            SCOPE_TIME.Text = Box.IniFile.IniReadValue(main_filepath, "取样间隔时间", main_filepath);

            HEART_TIME.Text = Box.IniFile.IniReadValue(main_filepath, "心跳检测时间",  main_filepath);

            PRTECT_TIME.Text = Box.IniFile.IniReadValue(main_filepath, "延迟启动/停止时间", main_filepath);



            string rt =  Box.IniFile.IniReadValue(main_filepath, "模拟排料",  main_filepath);

            if (rt == "true" || rt == "True")
            {
                模拟排料.Checked = true;
            }
            else
            {
                模拟排料.Checked = false;
            }

            Size_Pul_VR_NUM.Text =  Box.IniFile.IniReadValue(main_filepath, "光纤比例偏移", main_filepath);



            string rt1 = Box.IniFile.IniReadValue(main_filepath, "通讯格式1", main_filepath);

            string rt2 = Box.IniFile.IniReadValue(main_filepath, "通讯格式2", main_filepath);

            if (rt1 == "true" || rt1 == "True")
            {
                PC_MODBUS_SET.Checked = true;

                PC_MODBUS_SET2.Checked = false;

                cszmc_xp.cszmc_xp.PC_MODBUS_SET_MODELR(g_handle, 0);

            }
            else
            {
                PC_MODBUS_SET.Checked = false;

                PC_MODBUS_SET2.Checked = true;


                cszmc_xp.cszmc_xp.PC_MODBUS_SET_MODELR(g_handle, 1);


            }



            cszmc_xp.cszmc_xp.PC_SET_CAM_NUM(g_handle, Convert.ToInt32(CAM_NUM.Text));

            cszmc_xp.cszmc_xp.PC_SET_OP_NUM(g_handle, Convert.ToInt32(OP_NUM.Text));

            cszmc_xp.cszmc_xp.PC_SET_MODE(g_handle, CONNECT_MODE.SelectedIndex);

            cszmc_xp.cszmc_xp.PC_SET_PRORT(g_handle, Convert.ToSingle(SCOPE_TIME.Text));

            if (Convert.ToSingle(SCOPE_TIME.Text) > 0)
            {
                cszmc_xp.cszmc_xp.PC_SET_PRORTFLAG(g_handle, 1);
            }
            else
            {
                cszmc_xp.cszmc_xp.PC_SET_PRORTFLAG(g_handle, 0);
            }

            cszmc_xp.cszmc_xp.PC_SET_HEARTBEAT_TIME(g_handle, Convert.ToSingle(HEART_TIME.Text));

            if (Convert.ToSingle(HEART_TIME.Text) > 0)
            {
                cszmc_xp.cszmc_xp.PC_SET_HEARTBEAT(g_handle, 1);
            }
            else
            {
                cszmc_xp.cszmc_xp.PC_SET_HEARTBEAT(g_handle, 0);
            }


            cszmc_xp.cszmc_xp.PC_SET_SIZE_PUL(g_handle, Convert.ToSingle(Size_Pul_VR_NUM.Text));


            cszmc_xp.cszmc_xp.PC_SET_PROTEC(g_handle, Convert.ToSingle(PRTECT_TIME.Text));

        }


        public void Write_and_Read(object sender)
        {
            TextBox xt = (TextBox)sender;

            if (!string.IsNullOrWhiteSpace(xt.Text) && Inits_flag ==1 && xt.Text != "-")
            {
                Param_Write();
                Param_READ();

                listView1.Items.Clear();
                CCD_NUM.Items.Clear();
                for (int i = 0; i < Convert.ToInt32(CAM_NUM.Text); i++)
                {

                    listView1.Items.Add(i.ToString());

                    CCD_NUM.Items.Add(i.ToString());

                }

                listView2.Items.Clear();
                IO_NUM.Items.Clear();
                for (int i = 0; i < Convert.ToInt32(OP_NUM.Text); i++)
                {
                    listView2.Items.Add(i.ToString());

                    IO_NUM.Items.Add(i.ToString());
                }

            }


            
        }

        private void CAM_NUM_TextChanged(object sender, EventArgs e)
        {
           
            Write_and_Read(sender);
        }

        private void OP_NUM_TextChanged(object sender, EventArgs e)
        {
            Write_and_Read(sender);
        }

        private void SCOPE_TIME_TextChanged(object sender, EventArgs e)
        {
            Write_and_Read(sender);
        }

        private void HEART_TIME_TextChanged(object sender, EventArgs e)
        {
            Write_and_Read(sender);
        }

        private void Size_Pul_VR_NUM_TextChanged(object sender, EventArgs e)
        {

            if (Convert.ToInt32(Size_Pul_VR_NUM.Text) > 100)
            {
                Size_Pul_VR_NUM.Text = 100.ToString();
            }

            if (Convert.ToInt32(Size_Pul_VR_NUM.Text) < 0)
            {
                Size_Pul_VR_NUM.Text = 0.ToString();
            }

            Size_Pul_VR.Value = Convert.ToInt32(Size_Pul_VR_NUM.Text);

            Write_and_Read(sender);
        }


        private void PRTECT_TIME_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(PRTECT_TIME.Text) > 9999)
            {
                PRTECT_TIME.Text = 9999.ToString();
            }

            if (Convert.ToInt32(PRTECT_TIME.Text) < 0)
            {
                PRTECT_TIME.Text = 0.ToString();
            }

            Write_and_Read(sender);
        }



        private void CONNECT_MODE_SelectedIndexChanged(object sender, EventArgs e)
        {
            Param_Write();
            Param_READ();
        }


        private void 模拟排料_MouseUp(object sender, MouseEventArgs e)
        {
            Param_Write();
            Param_READ();
        }

        private void mian_timer_Tick(object sender, EventArgs e)
        {
            

            int count = 0;

            int[] cam_run = new int[Convert.ToInt32(CAM_NUM.Text)];

            int[] cam_skip = new int[Convert.ToInt32(CAM_NUM.Text)];

            int[] OP_run = new int[Convert.ToInt32(OP_NUM.Text)];

            int[] OP_skip = new int[Convert.ToInt32(OP_NUM.Text)];

            float run_status = 0;

            float run_ct = 0;

            zmcaux.ZAux_Direct_GetUserVar(g_handle, "run_status", ref run_status);

            zmcaux.ZAux_Direct_GetUserVar(g_handle, "Reg_CT", ref run_ct);

            if (run_status == 1)
            {
                SYS_STATUS.Text = "系统运行中"+ "   CT： " + run_ct.ToString() + "  pics/min";
            }
            else
            {
                SYS_STATUS.Text = "系统就绪，未运行";
            }

            float mpos_dis = 0;

            cszmc_xp.cszmc_xp.PC_READ_MPOS(g_handle, 0, ref mpos_dis);

            MPOS.Text = mpos_dis.ToString();


            cszmc_xp.cszmc_xp.PC_READ_REG(g_handle, ref count);

            chuiqi_count4.Text = count.ToString();

            cszmc_xp.cszmc_xp.PC_READ_OP_REG(g_handle, ref count);

            chuiqi_count2.Text = count.ToString();


            listView1.Items.Clear();
            for (int i = 0; i < Convert.ToInt32(CAM_NUM.Text); i++)
            {
                listView1.Items.Add(i.ToString());
            }

            for (int i = 0; i < cam_run.Length; i++)
            { 
                cszmc_xp.cszmc_xp.PC_READ_CURCAM_PS2(g_handle,i, ref cam_run[i]);

                cszmc_xp.cszmc_xp.PC_READ_CURCAM_SKI(g_handle,i, ref cam_skip[i]);

                listView1.Items[i].SubItems.Add(cam_run[i].ToString());

                listView1.Items[i].SubItems.Add(cam_skip[i].ToString());

            }


            listView2.Items.Clear();
            for (int i = 0; i < Convert.ToInt32(OP_NUM.Text); i++)
            {
                listView2.Items.Add(i.ToString());
            }
            for (int i = 0; i < OP_run.Length; i++)
            {
                cszmc_xp.cszmc_xp.PC_READ_CUROP_PS2(g_handle, i, ref OP_run[i]);

                cszmc_xp.cszmc_xp.PC_READ_CUROP_SKI(g_handle, i, ref OP_skip[i]);

                listView2.Items[i].SubItems.Add(OP_run[i].ToString());

                listView2.Items[i].SubItems.Add(OP_skip[i].ToString());
            }


        }

        private void stop_Click(object sender, EventArgs e)
        {
            cszmc_xp.cszmc_xp.PC_CAM_STOP_TASK(g_handle);

            tabControl1.Enabled = true;

            if (Convert.ToInt32(SCOPE_TIME.Text) > 0)
            {
                Thread.Sleep(Convert.ToInt32(SCOPE_TIME.Text));
            }

            runtask.Abort();

        }


        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            float run_status = 0;

            zmcaux.ZAux_Direct_GetUserVar(g_handle, "run_status", ref run_status);

            if (run_status == 1)
            {
                e.Cancel = true;
            }
            
        }

        private void run_Click(object sender, EventArgs e)
        {
            cszmc_xp.cszmc_xp.PC_CAM_RUN_TASK(g_handle);

            if (模拟排料.Checked == true)
            {

                virtul_count_set = 0;

                for (int i = 0; i < Convert.ToInt32(OP_NUM.Text); i++)
                {
                    float[] CAM_PARAM = new float[32];

                    int ret = cszmc_xp.cszmc_xp.PC_READ_POSNG(g_handle, i, ref CAM_PARAM[0], ref CAM_PARAM[1], ref CAM_PARAM[2], ref CAM_PARAM[3], ref CAM_PARAM[4]);
                    if ((int)CAM_PARAM[4] == 1)
                    {
                        virtual_array[virtul_count_set] = i;
                        virtul_count_set++;
                    }
                }

                runtask.Start();
                
            }

            

        }

        public static void 下发排料()
        {

            float run_status = 0;

            zmcaux.ZAux_Direct_GetUserVar(g_handle, "run_status", ref run_status);

            float cam_num = 0;

            cszmc_xp.cszmc_xp.PC_READ_CAM_NUM(g_handle, ref cam_num);

            int cam = (int)(cam_num - 1);


            int mode = 0;


            if (f1.PC_MODBUS_SET.Checked == true)
            {
                mode = 0;
            }
            else if (f1.PC_MODBUS_SET2.Checked == true)
            {
                mode = 1;
            }

            float virtual_run_count = f1.virtul_count_set;



            float[] virtual_run = f1.virtual_array ;


            int virtual_count = 0;

            int cam_run = 0;

            while (run_status == 1)
            {
                int cam_run2 = 0;

               int ret =  cszmc_xp.cszmc_xp.PC_READ_CURCAM_PS2(g_handle, cam, ref cam_run2);

                if (ret == 0)
                {
                    if (cam_run2 > cam_run)
                    {
                        if (mode == 0)
                        {
                           int rt =  cszmc_xp.cszmc_xp.PC_MODBUS_SET(g_handle, (int)virtual_run[virtual_count]);

                            while (rt != 0)
                            {
                                rt = cszmc_xp.cszmc_xp.PC_MODBUS_SET(g_handle, (int)virtual_run[virtual_count]);
                            }
                        }
                        else if (mode == 1)
                        {
                           int cty =  cszmc_xp.cszmc_xp.PC_MODBUS_SET2(g_handle, (int)virtual_run[virtual_count], cam_run);
                            while (cty != 0)
                            {
                                cty = cszmc_xp.cszmc_xp.PC_MODBUS_SET2(g_handle, (int)virtual_run[virtual_count], cam_run);
                            }

                        }

                        cam_run = cam_run2;

                        if (virtual_count == virtual_run_count - 1)
                        {
                            virtual_count = 0;
                        }
                        else
                        {
                            virtual_count++;
                        }

                    }

                    //zmcaux.ZAux_Direct_GetUserVar(g_handle, "run_status", ref run_status);
                }
                
            }
           
        }

        private void Size_Pul_VR_Scroll(object sender, EventArgs e)
        {
            Size_Pul_VR_NUM.Text = Size_Pul_VR.Value.ToString();
        }


        private void PC_MODBUS_SET2_MouseUp(object sender, MouseEventArgs e)
        {
            PC_MODBUS_SET2.Checked = true;
            PC_MODBUS_SET.Checked = false;
            Param_Write();
            Param_READ();
        }

        private void PC_MODBUS_SET_MouseUp(object sender, MouseEventArgs e)
        {
            PC_MODBUS_SET2.Checked = false;
            PC_MODBUS_SET.Checked = true;
            Param_Write();
            Param_READ();
        }

        private void Back_to_Zero_Click(object sender, EventArgs e)
        {
            cszmc_xp.cszmc_xp.PC_datum_task(g_handle);
        }

        private void Other_timer_Tick(object sender, EventArgs e)
        {
            float mpos_dis = 0;

            cszmc_xp.cszmc_xp.PC_READ_MPOS(g_handle, 0, ref mpos_dis);

            MPOS.Text = mpos_dis.ToString();

            mposdis.Text = mpos_dis.ToString();

            mpos_axis.Text = mpos_dis.ToString();

            MPOS_RTAXIS.Text = mpos_dis.ToString();
        }


        private void IO_WATCH_Click(object sender, EventArgs e)
        {
            Controler_IO_Info rts = new Controler_IO_Info();
            rts.Show();
        }

        #endregion



        #region   ///电机参数配置


        private void axis_param_timer_Tick(object sender, EventArgs e)
        {
            int i;

            int single_axis;

            int re_in = 0;

            uint[] axis_re_result = new uint[32];

            Graphics g = this.groupBox2.CreateGraphics();
            Pen redPen = new Pen(Color.Red, 10);
            Pen greenPen = new Pen(Color.Green, 10);


            single_axis = Convert.ToInt32(axis_num.Text);

            float mpos_dis = 0;

            cszmc_xp.cszmc_xp.PC_READ_MPOS(g_handle, 0, ref mpos_dis);

            MPOS.Text = mpos_dis.ToString();

            mposdis.Text = mpos_dis.ToString();

            MPOS_RTAXIS.Text = mpos_dis.ToString();

            cszmc_xp.cszmc_xp.PC_READ_MPOS(g_handle, single_axis, ref mpos_dis);

            mpos_axis.Text  = mpos_dis.ToString();

            


            zmcaux.ZAux_Direct_GetFwdIn(g_handle, single_axis, ref re_in);

            cur_fwd_in.Text = re_in.ToString();

            if (re_in != -1)
            {
                zmcaux.ZAux_Direct_GetIn(g_handle, re_in, ref axis_re_result[0]);
            }
            else
            {
                axis_re_result[0] = 2;
            }



            zmcaux.ZAux_Direct_GetRevIn(g_handle, single_axis, ref re_in);

            cur_rev_in.Text = re_in.ToString();

            if (re_in != -1)
            {
                zmcaux.ZAux_Direct_GetIn(g_handle, re_in, ref axis_re_result[1]);
            }
            else
            {
                axis_re_result[1] = 2;
            }



            zmcaux.ZAux_Direct_GetDatumIn(g_handle, single_axis, ref re_in);

            cur_datum_in.Text = re_in.ToString();

            if (re_in != -1)
            {
                zmcaux.ZAux_Direct_GetIn(g_handle, re_in, ref axis_re_result[2]);
            }
            else
            {
                axis_re_result[2] = 2;
            }




            for (i = 0; i < 3; i++)
            {

                g = this.f_r_datum_in[i].CreateGraphics();

                if (axis_re_result[i] == 0)
                {
                    g.DrawEllipse(redPen, new Rectangle(5, 5, 10, 10));   //红色，无信号
                }
                else if (axis_re_result[i] == 1)
                {
                    g.DrawEllipse(greenPen, new Rectangle(5, 5, 10, 10));   //红色，无信号
                }

            }
        }


        private void axis_set_Click(object sender, EventArgs e)
        {
            if (g_handle != (IntPtr)0)
            {

                get_single_val();     //获取当前轴界面参数

                aixs_single_set();    //写入当前轴界面参数并执行

                get_axis_info();      //轴信息获取

                axis_re_result[0] = 2;

                axis_re_result[1] = 2;

                axis_re_result[2] = 2;

                cur_fwd_states.Invalidate();

                cur_rev_states.Invalidate();

                cur_datum_states.Invalidate();


                if (String.Compare(axis_type.Text, cur_axis_atype.Text) != 0)
                {
                    MessageBox.Show("轴类型设置失败，不支持的类型！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }

            else
            {
                MessageBox.Show("控制器未连接！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



        //获取单轴参数

        public void get_single_val()
        {
            int single_axis;

            single_axis = Convert.ToInt32(axis_num.Text);

            if (axis_type.SelectedIndex < 8)
            {
                atype[single_axis] = Convert.ToInt32(axis_type.SelectedIndex);
            }
            else if (axis_type.SelectedIndex == 8)
            {
                atype[single_axis] = 65;
            }
            else if (axis_type.SelectedIndex == 9)
            {
                atype[single_axis] = 66;
            }
            else if (axis_type.SelectedIndex == 10)
            {
                atype[single_axis] = 67;
            }




            pul[single_axis] = Convert.ToSingle(axis_PUL.Text);

            dis_angle[single_axis] = Convert.ToSingle(axis_dis_angle.Text);

            speed[single_axis] = Convert.ToSingle(axis_speed.Text);

            accel[single_axis] = Convert.ToSingle(axis_accel.Text);

            decel[single_axis] = Convert.ToSingle(axis_decel.Text);

            units[single_axis] = (pul[single_axis] / dis_angle[single_axis]);

            fs_limit[single_axis] = Convert.ToSingle(axis_fs_limit.Text);

            rs_limit[single_axis] = Convert.ToSingle(axis_rs_limit.Text);

            fwd_in[single_axis] = Convert.ToInt32(axis_fwd.Text);

            rev_in[single_axis] = Convert.ToInt32(axis_rev.Text);

            datum_in[single_axis] = Convert.ToInt32(axis_datum_in.Text);

            dir[single_axis] = Convert.ToSingle(AXIS_DIR.SelectedIndex);

            // Console.Write(atype[single_axis]);

        }


        //  修改单轴参数

        public void aixs_single_set()
        {
            int single_axis;

            single_axis = Convert.ToInt32(axis_num.Text);

            float[] vfg = { (float)atype[single_axis], pul[single_axis], dis_angle[single_axis], speed[single_axis], accel[single_axis], decel[single_axis], dir[single_axis] };

            cszmc_xp.cszmc_xp.PC_SAVE_PARAM(g_handle, vfg);


            // int result = 0;

            //zmcaux.ZAux_Direct_SetAtype(g_handle, single_axis, atype[single_axis]);

            //textBox3.Text = atype[single_axis].ToString();

            //zmcaux.ZAux_Direct_SetUnits(g_handle, single_axis, units[single_axis]);
            //zmcaux.ZAux_Direct_SetSpeed(g_handle, single_axis, speed[single_axis]);
            //zmcaux.ZAux_Direct_SetAccel(g_handle, single_axis, accel[single_axis]);
            //zmcaux.ZAux_Direct_SetDecel(g_handle, single_axis, decel[single_axis]);


            zmcaux.ZAux_Direct_SetFsLimit(g_handle, single_axis, fs_limit[single_axis]);
            zmcaux.ZAux_Direct_SetRsLimit(g_handle, single_axis, rs_limit[single_axis]);


            //正限位
            zmcaux.ZAux_Direct_SetFwdIn(g_handle, single_axis, fwd_in[single_axis]);


            if (fwd_in_invert.Checked == true)
            {
                if (fwd_in[single_axis] != -1)
                {

                    zmcaux.ZAux_Direct_SetInvertIn(g_handle, fwd_in[single_axis], 1);

                    even_state[fwd_in[single_axis]] = 1;

                }

            }
            else
            {
                if (fwd_in[single_axis] != -1)
                {

                    zmcaux.ZAux_Direct_SetInvertIn(g_handle, fwd_in[single_axis], 0);

                    even_state[fwd_in[single_axis]] = 0;

                }
            }


            //负限位

            zmcaux.ZAux_Direct_SetRevIn(g_handle, single_axis, rev_in[single_axis]);

            if (rev_in_invert.Checked == true)
            {
                if (rev_in[single_axis] != -1)
                {

                    zmcaux.ZAux_Direct_SetInvertIn(g_handle, rev_in[single_axis], 1);

                    even_state[rev_in[single_axis]] = 1;

                }

            }
            else
            {
                if (rev_in[single_axis] != -1)
                {

                    zmcaux.ZAux_Direct_SetInvertIn(g_handle, rev_in[single_axis], 0);

                    even_state[rev_in[single_axis]] = 0;

                }
            }


            //原点信号
            zmcaux.ZAux_Direct_SetDatumIn(g_handle, single_axis, datum_in[single_axis]);

            if (datum_in_invert.Checked == true)
            {
                if (datum_in[single_axis] != -1)
                {

                    zmcaux.ZAux_Direct_SetInvertIn(g_handle, datum_in[single_axis], 1);

                    even_state[datum_in[single_axis]] = 1;

                }

            }
            else
            {
                if (datum_in[single_axis] != -1)
                {

                    zmcaux.ZAux_Direct_SetInvertIn(g_handle, datum_in[single_axis], 0);

                    even_state[datum_in[single_axis]] = 0;

                }
            }



        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    mian_timer.Enabled = true;
                    axis_param_timer.Enabled = false;
                    Other_timer.Enabled = false;
                    break;
                case 1:
                    mian_timer.Enabled = false;
                    axis_param_timer.Enabled = true;
                    Other_timer.Enabled = false;
                    get_axis_info();      //轴信息获取
                    break;


                default:
                    mian_timer.Enabled = false;
                    axis_param_timer.Enabled = false;
                    Other_timer.Enabled = true;
                    break;
            }
        }

        private void axis_num_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (g_handle != (IntPtr)0)
            {
                //get_cur_axisinfo();    //  单轴切换信息读取

                get_axis_info();      //轴信息获取

                axis_re_result[0] = 2;

                axis_re_result[1] = 2;

                axis_re_result[2] = 2;

                cur_fwd_states.Invalidate();

                cur_rev_states.Invalidate();

                cur_datum_states.Invalidate();

            }
        }

        private void axis_address_int1_Click(object sender, EventArgs e)
        {
            axis_address_initssingle(axis_num);
        }


        public void axis_address_initssingle(Control tx1)
        {
            int axis_i;

            byte[] tempbuff = new byte[1024];               //接收的返回的字符

            StringBuilder controler_info = new StringBuilder(100);

            string read_info = "";

            string commander = "";

            uint readcontrol_length = 100;



            commander = "atype(" + tx1.Text + ") = 0";

            zmcaux.ZAux_Execute(g_handle, commander, controler_info, readcontrol_length); // '执行命令并接受应答

            commander = "axis_address(" + tx1.Text + ") = -1";

            zmcaux.ZAux_Execute(g_handle, commander, controler_info, readcontrol_length); // '执行命令并接受应答

            commander = "axis_address(" + tx1.Text + ") = 0";

            zmcaux.ZAux_Execute(g_handle, commander, controler_info, readcontrol_length); // '执行命令并接受应答




        }

        private void all_axis_address_Click(object sender, EventArgs e)
        {
            axis_address_inits();
        }

        public void axis_address_inits()
        {
            int axis_i;

            byte[] tempbuff = new byte[1024];               //接收的返回的字符

            StringBuilder controler_info = new StringBuilder(100);

            string read_info = "";

            string commander = "";

            uint readcontrol_length = 100;

           string rr =  Read_controler_info("?SYS_ZFEATURE(1)");

            for (axis_i = 0; axis_i < Convert.ToInt32(rr); axis_i++)
            {

                commander = "atype(" + axis_i.ToString() + ") = 0";

                zmcaux.ZAux_Execute(g_handle, commander, controler_info, readcontrol_length); // '执行命令并接受应答

                commander = "axis_address(" + axis_i.ToString() + ") = -1";

                zmcaux.ZAux_Execute(g_handle, commander, controler_info, readcontrol_length); // '执行命令并接受应答


            }

            for (axis_i = 0; axis_i < Convert.ToInt32(rr); axis_i++)
            {

                commander = "axis_address(" + axis_i.ToString() + ") = 0";

                zmcaux.ZAux_Execute(g_handle, commander, controler_info, readcontrol_length); // '执行命令并接受应答



            }

        }


        public string Read_controler_info(string str1)
        {

            byte[] tempbuff = new byte[1024];               //接收的返回的字符

            StringBuilder controler_info = new StringBuilder(100);

            string read_info = "";

            string commander = str1;

            uint readcontrol_length = 100;

            zmcaux.ZAux_Execute(g_handle, commander, controler_info, readcontrol_length); // '执行命令并接受应答

            read_info += controler_info;

            //MessageBox.Show(read_info);

            return read_info;

        }

        private void VMOVE1_MouseDown(object sender, MouseEventArgs e)
        {
            AXIS_MOVE(0, 1);
        }

        private void VMOVE1_MouseUp(object sender, MouseEventArgs e)
        {
            AXIS_CANCEL(0);
        }

        private void VMOVE0_MouseDown(object sender, MouseEventArgs e)
        {
            AXIS_MOVE(0, -1);
        }

        private void VMOVE0_MouseUp(object sender, MouseEventArgs e)
        {
            AXIS_CANCEL(0);
        }



        public void AXIS_MOVE(int axis_num ,int dir)
        {
            if (dir == 1)
            {
                zmcaux.ZAux_Direct_Single_Vmove(g_handle, axis_num, 1);
            }
            else if (dir == -1)
            {
                zmcaux.ZAux_Direct_Single_Vmove(g_handle, axis_num, -1);
            }
        }


        public void AXIS_CANCEL(int axis_num)
        {
            zmcaux.ZAux_Direct_Single_Cancel(g_handle, axis_num, 2);
        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            AXIS_MOVE(Convert.ToInt32( axis_num.Text), 1);
        }

        private void button2_MouseUp(object sender, MouseEventArgs e)
        {
            AXIS_CANCEL(Convert.ToInt32(axis_num.Text));
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            AXIS_MOVE(Convert.ToInt32(axis_num.Text), -1);
        }

        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            AXIS_CANCEL(Convert.ToInt32(axis_num.Text));
        }

        

        //轴信息获取

        public void get_axis_info()
        {
            int re_atype;

            re_atype = 0;

            float re_param;

            re_param = 0;

            int single_axis;

            int i;




            Graphics g = this.groupBox2.CreateGraphics();
            Pen redPen = new Pen(Color.Red, 10);
            Pen greenPen = new Pen(Color.Green, 10);


            single_axis = Convert.ToInt32(axis_num.Text);



            cur_axis_num.Text = axis_num.Text;



            zmcaux.ZAux_Direct_GetAtype(g_handle, single_axis, ref re_atype);

            if (re_atype == 65)
            {
                cur_axis_atype.Text = axis_type.Items[8].ToString();
            }
            else if (re_atype == 66)
            {
                cur_axis_atype.Text = axis_type.Items[9].ToString();
            }
            else if (re_atype == 67)
            {
                cur_axis_atype.Text = axis_type.Items[10].ToString();
            }
            else
            {
                cur_axis_atype.Text = axis_type.Items[re_atype].ToString();
            }


            //cur_axis_atype.Text = axis_type.Items[re_atype].ToString();



            zmcaux.ZAux_Direct_GetUnits(g_handle, single_axis, ref re_param);

            cur_pul.Text = re_param.ToString();

            int ref_step = 0;

            zmcaux.ZAux_Direct_GetInvertStep(g_handle, single_axis, ref ref_step);
            if (ref_step == 0)
            {
                dir_axis.Text = "正向";

            }
            else if (ref_step == 2)
            {
                dir_axis.Text = "负向";
            }


            zmcaux.ZAux_Direct_GetSpeed(g_handle, single_axis, ref re_param);

            cur_speed.Text = re_param.ToString();


            zmcaux.ZAux_Direct_GetAccel(g_handle, single_axis, ref re_param);

            cur_accel.Text = re_param.ToString();



            zmcaux.ZAux_Direct_GetDecel(g_handle, single_axis, ref re_param);

            cur_decel.Text = re_param.ToString();



            zmcaux.ZAux_Direct_GetFsLimit(g_handle, single_axis, ref re_param);

            cur_fs_limit.Text = Convert.ToDouble(re_param).ToString();



            zmcaux.ZAux_Direct_GetRsLimit(g_handle, single_axis, ref re_param);

            cur_rs_limit.Text = Convert.ToDouble(re_param).ToString();



            zmcaux.ZAux_Direct_GetFwdIn(g_handle, single_axis, ref re_in);

            cur_fwd_in.Text = re_in.ToString();

            if (re_in != -1)
            {
                zmcaux.ZAux_Direct_GetIn(g_handle, re_in, ref axis_re_result[0]);
            }
            else
            {
                axis_re_result[0] = 2;
            }



            zmcaux.ZAux_Direct_GetRevIn(g_handle, single_axis, ref re_in);

            cur_rev_in.Text = re_in.ToString();

            if (re_in != -1)
            {
                zmcaux.ZAux_Direct_GetIn(g_handle, re_in, ref axis_re_result[1]);
            }
            else
            {
                axis_re_result[1] = 2;
            }



            zmcaux.ZAux_Direct_GetDatumIn(g_handle, single_axis, ref re_in);

            cur_datum_in.Text = re_in.ToString();

            if (re_in != -1)
            {
                zmcaux.ZAux_Direct_GetIn(g_handle, re_in, ref axis_re_result[2]);
            }
            else
            {
                axis_re_result[2] = 2;
            }




            for (i = 0; i < 3; i++)
            {

                g = this.f_r_datum_in[i].CreateGraphics();

                if (axis_re_result[i] == 0)
                {
                    g.DrawEllipse(redPen, new Rectangle(5, 5, 10, 10));   //红色，无信号
                }
                else if (axis_re_result[i] == 1)
                {
                    g.DrawEllipse(greenPen, new Rectangle(5, 5, 10, 10));   //红色，无信号
                }

            }





        }

       


        #endregion



        #region  ///相机设置

        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            AXIS_MOVE(0, 1);
        }

        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            AXIS_MOVE(0, -1);
        }

        private void button3_MouseUp(object sender, MouseEventArgs e)
        {
            AXIS_CANCEL(0);
        }

        private void POS_INSERT_Click(object sender, EventArgs e)
        {
            float mpos_dis = 0;
            cszmc_xp.cszmc_xp.PC_READ_MPOS(g_handle, 0, ref mpos_dis);

            CAM_OP_POS.Text = mpos_dis.ToString();


        }

        private void CAM_SAVE_Click(object sender, EventArgs e)
        {
            ///int ret =  cszmc_xp.cszmc_xp.PC_SAVE_POS(g_handle, Convert.ToInt32(CCD_NUM.Text), Convert.ToSingle(CAM_OP_POS.Text), Convert.ToInt32(CAM_OP_NUM.Text), Convert.ToSingle(CAM_OP_TIMES.Text), Convert.ToSingle(CAM_REPORT_NUM.Text));

            int ret = CAM_PARAM_SET();

            if (ret != 0)
            {
                MessageBox.Show("相机参数设置失败");
            }
        }


        public int CAM_PARAM_SET()
        {
            int ret = cszmc_xp.cszmc_xp.PC_SAVE_POS(g_handle, Convert.ToInt32(CCD_NUM.Text), Convert.ToSingle(CAM_OP_POS.Text), Convert.ToInt32(CAM_OP_NUM.Text), Convert.ToSingle(CAM_OP_TIMES.Text), Convert.ToSingle(CAM_REPORT_NUM.Text));

            return ret;
        }

        public int CAM_PARAM_READ()
        {

            float[] CAM_PARAM = new float[32];

            int ret = cszmc_xp.cszmc_xp.PC_READ_POS(g_handle, Convert.ToInt32(CCD_NUM.Text),ref CAM_PARAM[0],ref CAM_PARAM[1],ref CAM_PARAM[2],ref CAM_PARAM[3]);

            CAM_OP_POS.Text = CAM_PARAM[0].ToString();
            CAM_OP_NUM.Text = CAM_PARAM[1].ToString();
            CAM_OP_TIMES.Text = CAM_PARAM[2].ToString();
            CAM_REPORT_NUM.Text = CAM_PARAM[3].ToString();

            return ret;
        }

        private void CCD_NUM_SelectedIndexChanged(object sender, EventArgs e)
        {
            CAM_PARAM_READ();
        }

        private void CAM_TEST_Click(object sender, EventArgs e)
        {
            cszmc_xp.cszmc_xp.PC_CAM_TEST(g_handle, Convert.ToInt32(CCD_NUM.Text), Convert.ToInt32(CAM_OP_NUM.Text), Convert.ToSingle(CAM_OP_TIMES.Text));
        }

        

        private void button4_MouseUp(object sender, MouseEventArgs e)
        {
            AXIS_CANCEL(0);
        }

        


        #endregion

       
        
        #region ///排料设置

        private void button9_Click(object sender, EventArgs e)
        {
            float mpos_dis = 0;
            cszmc_xp.cszmc_xp.PC_READ_MPOS(g_handle, 0, ref mpos_dis);

            IO_OP_POS.Text = mpos_dis.ToString();
        }

        private void button8_MouseUp(object sender, MouseEventArgs e)
        {
            AXIS_CANCEL(0);
        }

        private void button7_MouseDown(object sender, MouseEventArgs e)
        {
            AXIS_MOVE(0, -1);
        }

        private void button7_MouseUp(object sender, MouseEventArgs e)
        {
            AXIS_CANCEL(0);
        }

        private void button6_Click(object sender, EventArgs e)
        {
           int ret =  IO_PARAM_SET();
            if (ret != 0)
            {
                MessageBox.Show("排料参数设置失败");
            }

        }

        private void button8_MouseDown(object sender, MouseEventArgs e)
        {
            AXIS_MOVE(0, 1);
        }


        public int IO_PARAM_SET()
        {
            int ret = cszmc_xp.cszmc_xp.PC_SAVE_POSNG(g_handle, Convert.ToInt32(IO_NUM.Text), Convert.ToSingle(IO_OP_POS.Text), Convert.ToInt32(IO_OP_NUM.Text), Convert.ToSingle(IO_OP_TIMES.Text), Convert.ToInt32(IO_STATUS.Text), Convert.ToInt32(IO_VRITUL_OP.SelectedIndex.ToString()));

            return ret;
        }

        private void IO_NUM_SelectedIndexChanged(object sender, EventArgs e)
        {
            IO_PARAM_READ();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            cszmc_xp.cszmc_xp.PC_OP_TEST(g_handle, Convert.ToInt32(IO_NUM.Text), Convert.ToInt32(IO_OP_NUM.Text),(int)Convert.ToSingle(IO_OP_TIMES.Text));
        }

       

        public int IO_PARAM_READ()
        {

            float[] CAM_PARAM = new float[32];

            int ret = cszmc_xp.cszmc_xp.PC_READ_POSNG(g_handle, Convert.ToInt32(IO_NUM.Text), ref CAM_PARAM[0], ref CAM_PARAM[1], ref CAM_PARAM[2], ref CAM_PARAM[3],ref CAM_PARAM[4]);

            IO_OP_POS.Text = CAM_PARAM[0].ToString();
            IO_OP_NUM.Text = CAM_PARAM[1].ToString();
            IO_OP_TIMES.Text = CAM_PARAM[2].ToString();
            IO_STATUS.Text = CAM_PARAM[3].ToString();
            IO_VRITUL_OP.SelectedIndex = (int)CAM_PARAM[4];


            return ret;
        }

        #endregion



        #region  ///硬件连接配置

        public void HARD_CONNECT_WRITE()
        {
            string path = Environment.CurrentDirectory + "/硬件连接参数.ini";

            if (!File.Exists(path))
            {
                FileStream srt = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);

                srt.Close();

                Box.IniFile.IniWrite(path, "启动控制口", (-1* 1).ToString(), path);

                Box.IniFile.IniWrite(path, "停止控制口", (-1 * 1).ToString(), path);

                Box.IniFile.IniWrite(path, "回零控制口", (-1 * 1).ToString(), path);

                Box.IniFile.IniWrite(path, "急停控制口", (-1 * 1).ToString(), path);

                Box.IniFile.IniWrite(path, "常开吹气口", (-1 * 1).ToString(), path);

                Box.IniFile.IniWrite(path, "红灯控制口", (-1 * 1).ToString(), path);

                Box.IniFile.IniWrite(path, "绿灯控制口", (-1 * 1).ToString(), path);

                Box.IniFile.IniWrite(path, "前置排料位编号", (-1 * 1).ToString(), path);

                Box.IniFile.IniWrite(path, "无料检测输入口", (-1 * 1).ToString(), path);

                Box.IniFile.IniWrite(path, "无料检测时间", (-1 * 1).ToString(), path);

                Box.IniFile.IniWrite(path, "振动盘IO", (-1 * 1).ToString(), path);



            }


            Box.IniFile.IniWrite(path, "启动控制口", SYS_RUN.Text, path);

            Box.IniFile.IniWrite(path, "停止控制口", SYS_STOP.Text, path);

            Box.IniFile.IniWrite(path, "回零控制口", SYS_DATUM.Text, path);

            Box.IniFile.IniWrite(path, "急停控制口", SYS_EMERCY.Text, path);

            Box.IniFile.IniWrite(path, "常开吹气口", SYS_OPEN_ALWAY.Text, path);

            Box.IniFile.IniWrite(path, "红灯控制口", SYS_RED.Text, path);

            Box.IniFile.IniWrite(path, "绿灯控制口", SYS_GREEN.Text, path);

            Box.IniFile.IniWrite(path, "前置排料位编号", SYS_FORCE_OPEN.Text, path);

            Box.IniFile.IniWrite(path, "无料检测输入口", SYS_NONE_DETECT.Text, path);

            Box.IniFile.IniWrite(path, "无料检测时间", SYS_DETECT_TIME.Text, path);

            Box.IniFile.IniWrite(path, "振动盘IO", SYS_SHOCK.Text, path);

        }


        public void HARD_CONNECT_READ()
        {
               string path = Environment.CurrentDirectory + "/硬件连接参数.ini";

            if (!File.Exists(path))
            {

                FileStream srt = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);

                srt.Close();
                

                Box.IniFile.IniWrite(path, "启动控制口", (-1 * 1).ToString(), path);

                Box.IniFile.IniWrite(path, "停止控制口", (-1 * 1).ToString(), path);

                Box.IniFile.IniWrite(path, "回零控制口", (-1 * 1).ToString(), path);

                Box.IniFile.IniWrite(path, "急停控制口", (-1 * 1).ToString(), path);

                Box.IniFile.IniWrite(path, "常开吹气口", (-1 * 1).ToString(), path);

                Box.IniFile.IniWrite(path, "红灯控制口", (-1 * 1).ToString(), path);

                Box.IniFile.IniWrite(path, "绿灯控制口", (-1 * 1).ToString(), path);

                Box.IniFile.IniWrite(path, "前置排料位编号", (-1 * 1).ToString(), path);

                Box.IniFile.IniWrite(path, "无料检测输入口", (-1 * 1).ToString(), path);

                Box.IniFile.IniWrite(path, "无料检测时间", (-1 * 1).ToString(), path);

                Box.IniFile.IniWrite(path, "振动盘IO", (-1 * 1).ToString(), path);



            }

            SYS_RUN.Text =  Box.IniFile.IniReadValue(path, "启动控制口", path);

               SYS_STOP.Text =  Box.IniFile.IniReadValue(path, "停止控制口",  path);

               SYS_DATUM.Text =Box.IniFile.IniReadValue(path, "回零控制口",  path);

               SYS_EMERCY.Text = Box.IniFile.IniReadValue(path, "急停控制口",  path);

               SYS_OPEN_ALWAY.Text = Box.IniFile.IniReadValue(path, "常开吹气口",  path);

               SYS_RED.Text = Box.IniFile.IniReadValue(path, "红灯控制口",  path);

               SYS_GREEN.Text = Box.IniFile.IniReadValue(path, "绿灯控制口",  path);

               SYS_FORCE_OPEN.Text = Box.IniFile.IniReadValue(path, "前置排料位编号",  path);

               SYS_NONE_DETECT.Text =  Box.IniFile.IniReadValue(path, "无料检测输入口", path);

               SYS_DETECT_TIME.Text = Box.IniFile.IniReadValue(path, "无料检测时间",  path);

               SYS_SHOCK.Text = Box.IniFile.IniReadValue(path, "振动盘IO",  path);



            cszmc_xp.cszmc_xp.PC_SET_STAR(g_handle, Convert.ToInt32(SYS_RUN.Text), 0);

            cszmc_xp.cszmc_xp.PC_SET_STOP(g_handle, Convert.ToInt32(SYS_STOP.Text), 0);

            cszmc_xp.cszmc_xp.PC_SET_DATUM(g_handle, Convert.ToInt32(SYS_DATUM.Text), 0);

            cszmc_xp.cszmc_xp.PC_SET_EMERCYSTOP(g_handle, Convert.ToInt32(SYS_EMERCY.Text), 0);

            cszmc_xp.cszmc_xp.PC_SET_OPEN(g_handle, Convert.ToInt32(SYS_OPEN_ALWAY.Text));

            cszmc_xp.cszmc_xp.PC_SET_LED_SHO(g_handle, 0, Convert.ToInt32(SYS_RED.Text));

            cszmc_xp.cszmc_xp.PC_SET_LED_SHO(g_handle, 1, Convert.ToInt32(SYS_GREEN.Text));

            cszmc_xp.cszmc_xp.PC_SET_LED_SHO(g_handle, 2, Convert.ToInt32(SYS_SHOCK.Text));

            cszmc_xp.cszmc_xp.PC_SET_CHECK(g_handle,Convert.ToInt32(SYS_NONE_DETECT.Text),0, Convert.ToSingle(SYS_DETECT_TIME.Text));


        }

        private void SYS_RUN_TextChanged(object sender, EventArgs e)
        {
            WR_RE(sender);
        }

        private void SYS_STOP_TextChanged(object sender, EventArgs e)
        {
            WR_RE(sender);
        }

        private void SYS_DATUM_TextChanged(object sender, EventArgs e)
        {
            WR_RE(sender);
        }

        private void SYS_EMERCY_TextChanged(object sender, EventArgs e)
        {
            WR_RE(sender);
        }

        private void SYS_OPEN_ALWAY_TextChanged(object sender, EventArgs e)
        {
            WR_RE(sender);
        }

        private void SYS_RED_TextChanged(object sender, EventArgs e)
        {
            WR_RE(sender);
        }

        private void SYS_GREEN_TextChanged(object sender, EventArgs e)
        {
            WR_RE(sender);
        }

        private void SYS_FORCE_OPEN_TextChanged(object sender, EventArgs e)
        {
            WR_RE(sender);
        }

        private void SYS_NONE_DETECT_TextChanged(object sender, EventArgs e)
        {
            WR_RE(sender);
        }

        private void SYS_DETECT_TIME_TextChanged(object sender, EventArgs e)
        {
            WR_RE(sender);
        }

        private void SYS_SHOCK_TextChanged(object sender, EventArgs e)
        {
            WR_RE(sender);
        }

        public void WR_RE(object sender)
        {
            TextBox rt = (TextBox)sender;

            if (!string.IsNullOrWhiteSpace(rt.Text) && Inits_flag == 1 && rt.Text !="-")
            {

                HARD_CONNECT_WRITE();

                HARD_CONNECT_READ();

            }
        }


        #endregion


        #region ///通讯配置



        public void CONNECT_CONNECT_WRITE()
        {
            string path = Environment.CurrentDirectory + "/通讯配置参数.ini";

            if (!File.Exists(path))
            {
                FileStream srt = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);

                srt.Close();

                Box.IniFile.IniWrite(path, "IO起始点", (0).ToString(), path);

                Box.IniFile.IniWrite(path, "波特率", (38400).ToString(), path);

                Box.IniFile.IniWrite(path, "数据位", (8).ToString(), path);

                Box.IniFile.IniWrite(path, "检验位", (0).ToString(), path);

                Box.IniFile.IniWrite(path, "停止位", (1).ToString(), path);

                Box.IniFile.IniWrite(path, "串口号", (0).ToString(), path);

                Box.IniFile.IniWrite(path, "COM寄存器", (0).ToString(), path);

                Box.IniFile.IniWrite(path, "COM起始值", (0).ToString(), path);

                Box.IniFile.IniWrite(path, "TCP寄存器", (0).ToString(), path);

                Box.IniFile.IniWrite(path, "TCP起始值", (0).ToString(), path);


            }


            Box.IniFile.IniWrite(path, "IO起始点", CONNCET_IO.Text, path);

            Box.IniFile.IniWrite(path, "波特率", BAUDER.Text, path);

            Box.IniFile.IniWrite(path, "数据位", DATABIT.Text, path);

            Box.IniFile.IniWrite(path, "检验位", JIAOYANBIT.Text, path);

            Box.IniFile.IniWrite(path, "停止位", STOPBIT.Text, path);

            Box.IniFile.IniWrite(path, "串口号", COM_NUM.Text, path);

            Box.IniFile.IniWrite(path, "COM寄存器", REG_BIT.Text, path);

            Box.IniFile.IniWrite(path, "COM起始值", STARTBIT.Text, path);

            Box.IniFile.IniWrite(path, "TCP寄存器", TCP_REG.Text, path);

            Box.IniFile.IniWrite(path, "TCP起始值", TCP_STARTBIT.Text, path);

        }


        public void CONNECT_CONNECT_READ()
        {
            string path = Environment.CurrentDirectory + "/通讯配置参数.ini";

            if (!File.Exists(path))
            {
                FileStream srt = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);

                srt.Close();

                Box.IniFile.IniWrite(path, "IO起始点", (0).ToString(), path);

                Box.IniFile.IniWrite(path, "波特率", (38400).ToString(), path);

                Box.IniFile.IniWrite(path, "数据位", (8).ToString(), path);

                Box.IniFile.IniWrite(path, "检验位", (0).ToString(), path);

                Box.IniFile.IniWrite(path, "停止位", (1).ToString(), path);

                Box.IniFile.IniWrite(path, "串口号", (0).ToString(), path);

                Box.IniFile.IniWrite(path, "COM寄存器", (0).ToString(), path);

                Box.IniFile.IniWrite(path, "COM起始值", (0).ToString(), path);

                Box.IniFile.IniWrite(path, "TCP寄存器", (0).ToString(), path);

                Box.IniFile.IniWrite(path, "TCP起始值", (0).ToString(), path);


            }

            CONNCET_IO.Text =  Box.IniFile.IniReadValue(path, "IO起始点",  path);

            BAUDER.Text = Box.IniFile.IniReadValue(path, "波特率",  path);

            DATABIT.Text = Box.IniFile.IniReadValue(path, "数据位", path);

            JIAOYANBIT.Text = Box.IniFile.IniReadValue(path, "检验位", path);

            STOPBIT.Text =Box.IniFile.IniReadValue(path, "停止位",  path);

            COM_NUM.Text = Box.IniFile.IniReadValue(path, "串口号",  path);

            REG_BIT.Text = Box.IniFile.IniReadValue(path, "COM寄存器",  path);
             
            STARTBIT.Text = Box.IniFile.IniReadValue(path, "COM起始值",  path);

            TCP_REG.Text = Box.IniFile.IniReadValue(path, "TCP寄存器",  path);

            TCP_STARTBIT.Text = Box.IniFile.IniReadValue(path, "TCP起始值",  path);



            cszmc_xp.cszmc_xp.PC_COM_MODE(g_handle, Convert.ToInt32(BAUDER.Text), Convert.ToInt32(DATABIT.Text), Convert.ToInt32(JIAOYANBIT.Text), Convert.ToInt32(STOPBIT.Text), Convert.ToInt32(COM_NUM.Text), Convert.ToInt32(REG_BIT.Text), Convert.ToInt32(STARTBIT.Text));

            cszmc_xp.cszmc_xp.PC_IO_MODE(g_handle, Convert.ToInt32(CONNCET_IO.Text));

            cszmc_xp.cszmc_xp.PC_TCP_MODE(g_handle, Convert.ToInt32(TCP_REG.Text), Convert.ToInt32(TCP_STARTBIT.Text));

        }

        private void CONNCET_IO_TextChanged(object sender, EventArgs e)
        {
            WR_COM_TCP(sender);
        }

        private void BAUDER_TextChanged(object sender, EventArgs e)
        {
            WR_COM_TCP(sender);
        }

        private void DATABIT_TextChanged(object sender, EventArgs e)
        {
            WR_COM_TCP(sender);
        }

        private void JIAOYANBIT_TextChanged(object sender, EventArgs e)
        {
            WR_COM_TCP(sender);
        }

        private void STOPBIT_TextChanged(object sender, EventArgs e)
        {
            WR_COM_TCP(sender);
        }

        private void COM_NUM_TextChanged(object sender, EventArgs e)
        {
            WR_COM_TCP(sender);
        }

        private void REG_BIT_TextChanged(object sender, EventArgs e)
        {
            WR_COM_TCP(sender);
        }

        private void STARTBIT_TextChanged(object sender, EventArgs e)
        {
            WR_COM_TCP(sender);
        }

        private void TCP_REG_TextChanged(object sender, EventArgs e)
        {
            WR_COM_TCP(sender);
        }

        private void TCP_STARTBIT_TextChanged(object sender, EventArgs e)
        {
            WR_COM_TCP(sender);
        }

        

        public void WR_COM_TCP(object sender)
        {
            TextBox rt = (TextBox)sender;

            if (!string.IsNullOrWhiteSpace(rt.Text) && Inits_flag == 1 && rt.Text != "-")
            {

                CONNECT_CONNECT_WRITE();

                CONNECT_CONNECT_READ();

            }
        }


        #endregion

    }






}
