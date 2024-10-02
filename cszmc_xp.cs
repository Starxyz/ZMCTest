using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using cszmcaux;

namespace cszmc_xp
{
    public class cszmc_xp
    {

        public static GCHandle ght;


        /// <summary>
        /// 描述：设置相机触发路数
        /// </summary>
        /// <param name="ID_CON">控制器句柄</param>
        /// <param name="num">相机路数</param>
        /// <returns></returns>

        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_CAM_NUM", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_SET_CAM_NUM(IntPtr ID_CON, int num);

        

        /// <summary>
        /// 设置排料触发路数
        /// </summary>
        /// <param name="ID_CON">控制器句柄</param>
        /// <param name="num">排料路数</param>
        /// <returns></returns>
        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_OP_NUM", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32  PC_SET_OP_NUM(IntPtr ID_CON, int num);


        
        /// <summary>
        /// 停止运行
        /// </summary>
        /// <param name="ID_CON">控制器句柄</param>
        /// <returns></returns>
        [DllImport("zmc_xp.dll", EntryPoint = "PC_CAM_STOP_TASK", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32  PC_CAM_STOP_TASK(IntPtr ID_CON);


        
        /// <summary>
        /// 启动运行
        /// </summary>
        /// <param name="ID_CON">控制器句柄</param>
        /// <returns></returns>
        [DllImport("zmc_xp.dll", EntryPoint = "PC_CAM_RUN_TASK", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32  PC_CAM_RUN_TASK(IntPtr ID_CON);


        
        /// <summary>
        /// 转盘回零
        /// </summary>
        /// <param name="ID_CON">控制器句柄</param>
        /// <returns></returns>
        [DllImport("zmc_xp.dll", EntryPoint = "PC_datum_task", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32  PC_datum_task(IntPtr ID_CON);


        /******************************
            描述：转盘参数设置
            ID_CON:控制器句柄
            axis_Param : 转盘轴参数，依次是轴类型，驱动器一圈脉冲数（如有减速机，需要*减速比），旋转行程或角度（360°），速度，加速度，减速度，方向，按照顺序依次写入
        *******************************/
        /// <summary>
        /// 描述：转盘参数设置
        ///ID_CON:控制器句柄
       ///axis_Param : 转盘轴参数，依次是轴类型，驱动器一圈脉冲数（如有减速机，需要* 减速比），旋转行程或角度（360°），速度，加速度，减速度，方向，按照顺序依次写入
        /// </summary>
        /// <param name="ID_CON"></param>
        /// <param name="axis_Param"></param>
        /// <returns></returns>
        [DllImport("zmc_xp.dll", EntryPoint = "PC_SAVE_PARAM", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32  PC_SAVE_PARAM(IntPtr ID_CON, float[] axis_Param);


        /******************************
            描述：配置相机触发参数
            ID_CON:控制器句柄
            CAM_ID:相机ID，从0开始，依次向下，有最大数量限制
            CAM_POS:相机位置
            CAM_IO：相机触发IO
            CAM_TIME：相机触发电平有效时间(US)
            CAM_REG:相机触发信号反馈输入口编号，默认-1为无
        *******************************/
        /// <summary>
        ///  描述：配置相机触发参数
        /// </summary>
        /// <param name="ID_CON">控制器句柄</param>
        /// <param name="CAM_ID">相机ID，从0开始，依次向下，有最大数量限制</param>
        /// <param name="CAM_POS">相机位置</param>
        /// <param name="CAM_IO">相机触发IO</param>
        /// <param name="CAM_TIME">相机触发电平有效时间(US)</param>
        /// <param name="CAM_REG">相机触发信号反馈输入口编号，默认-1为无</param>
        /// <returns></returns>
        [DllImport("zmc_xp.dll", EntryPoint = "PC_SAVE_POS", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32  PC_SAVE_POS(IntPtr ID_CON, int CAM_ID, float CAM_POS, int CAM_IO, float CAM_TIME, float CAM_REG);


        /******************************
            描述：测试相机触发
            ID_CON:控制器句柄
            CAM_ID:相机ID，从0开始，依次向下，有最大数量限制
            CAM_IO：相机触发IO
            CAM_TIME：相机触发电平有效时间(US)
        *******************************/
        /// <summary>
        /// 描述：测试相机触发   ID_CON:控制器句柄  CAM_ID:相机ID，从0开始，依次向下，有最大数量限制   CAM_IO：相机触发IO   CAM_TIME：相机触发电平有效时间(US)
        /// </summary>
        /// <param name="ID_CON"></param>
        /// <param name="CAM_ID"></param>
        /// <param name="CAM_IO"></param>
        /// <param name="CAM_TIME"></param>
        /// <returns></returns>
        [DllImport("zmc_xp.dll", EntryPoint = "PC_CAM_TEST", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32  PC_CAM_TEST(IntPtr ID_CON, int CAM_ID, int CAM_IO, float CAM_TIME);


        /******************************
            描述：配置排料触发参数
            ID_CON:控制器句柄
            OP_ID:排料口ID，从0开始，依次向下，有最大数量限制
            OP_POS:排料位置
            OP_IO：排料触发IO
            OP_TIME：排料触发电平有效时间(US)
            OP_IO_STATU:排料口输出状态   0- 关  1 - 开
            OP_OUTPUT_FALG : 是否参与模拟排料 0-不参与  1- 参与
        *******************************/
        /// <summary>
        ///  描述：配置排料触发参数
        /// </summary>
        /// <param name="ID_CON">ID_CON:控制器句柄</param>
        /// <param name="OP_ID">OP_ID:排料口ID，从0开始，依次向下，有最大数量限制</param>
        /// <param name="OP_POS">OP_POS:排料位置</param>
        /// <param name="OP_IO">OP_IO：排料触发IO</param>
        /// <param name="OP_TIME">OP_TIME：排料触发电平有效时间(US)</param>
        /// <param name="OP_IO_STATU">OP_IO_STATU:排料口输出状态   0- 关  1 - 开</param>
        /// <param name="OP_OUTPUT_FALG">OP_OUTPUT_FALG : 是否参与模拟排料 0-不参与  1- 参与</param>
        /// <returns></returns>
        [DllImport("zmc_xp.dll", EntryPoint = "PC_SAVE_POSNG", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32  PC_SAVE_POSNG(IntPtr ID_CON, int OP_ID, float OP_POS, int OP_IO, float OP_TIME, int OP_IO_STATU, int OP_OUTPUT_FALG);


        /******************************
            描述：测试排料触发
            ID_CON:控制器句柄
            OP_ID:排料口ID，从0开始，依次向下，有最大数量限制
            OP_IO：排料触发IO
            OP_TIME：排料触发电平有效时间(US)
        *******************************/
        /// <summary>
        ///  描述：测试排料触发
        /// </summary>
        /// <param name="ID_CON"> ID_CON:控制器句柄</param>
        /// <param name="OP_ID">OP_ID:排料口ID，从0开始，依次向下，有最大数量限制</param>
        /// <param name="OP_IO">OP_IO：排料触发IO</param>
        /// <param name="OP_TIME">OP_TIME：排料触发电平有效时间(US)</param>
        /// <returns></returns>
        [DllImport("zmc_xp.dll", EntryPoint = "PC_OP_TEST", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32  PC_OP_TEST(IntPtr ID_CON, int OP_ID, int OP_IO, int OP_TIME);


        /*******************************
            描述：设置外部启动按钮
            ID_CON:控制器句柄
            IN_NUM :输入口  -1 表示没有
            IN_TYPE:输入类型  1 - 常闭， 0 - 常开
        ********************************/
        /// <summary>
        /// 描述：设置外部启动按钮
        /// </summary>
        /// <param name="ID_CON">ID_CON:控制器句柄</param>
        /// <param name="IN_NUM">IN_NUM :输入口  -1 表示没有</param>
        /// <param name="IN_TYPE"> IN_TYPE:输入类型  1 - 常闭， 0 - 常开</param>
        /// <returns></returns>
        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_STAR", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32  PC_SET_STAR(IntPtr ID_CON, int IN_NUM, int IN_TYPE);


        /*******************************
            描述：设置外部停止按钮
            ID_CON:控制器句柄
            IN_NUM :输入口  -1 表示没有
            IN_TYPE:输入类型  1 - 常闭， 0 - 常开
        ********************************/
        /// <summary>
        /// 描述：设置外部停止按钮
        /// </summary>
        /// <param name="ID_CON">ID_CON:控制器句柄</param>
        /// <param name="IN_NUM">IN_NUM :输入口  -1 表示没有</param>
        /// <param name="IN_TYPE">IN_TYPE:输入类型  1 - 常闭， 0 - 常开</param>
        /// <returns></returns>
        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_STOP", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32  PC_SET_STOP(IntPtr ID_CON, int IN_NUM, int IN_TYPE);






        /*******************************
            描述：设置外部回零按钮
            ID_CON:控制器句柄
            IN_NUM :输入口  -1 表示没有
            IN_TYPE:输入类型  1 - 常闭， 0 - 常开
        ********************************/
        /// <summary>
        /// 描述：设置外部回零按钮
        /// </summary>
        /// <param name="ID_CON">ID_CON:控制器句柄</param>
        /// <param name="IN_NUM">IN_NUM :输入口  -1 表示没有</param>
        /// <param name="IN_TYPE">IN_TYPE:输入类型  1 - 常闭， 0 - 常开</param>
        /// <returns></returns>
        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_DATUM", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32  PC_SET_DATUM(IntPtr ID_CON, int IN_NUM, int IN_TYPE);


        /*******************************
            描述：设置无料检测
            ID_CON:控制器句柄
            IN_NUM :输入口  -1 表示没有
            IN_TYPE:输入类型  1 - 常闭， 0 - 常开
            IN_TIME:无料检测时间(S)
        ********************************/
        /// <summary>
        ///  描述：设置无料检测
        /// </summary>
        /// <param name="ID_CON">ID_CON:控制器句柄</param>
        /// <param name="IN_NUM">IN_NUM :输入口  -1 表示没有</param>
        /// <param name="IN_TYPE">IN_TYPE:输入类型  1 - 常闭， 0 - 常开</param>
        /// <param name="IN_TIME">IN_TIME:无料检测时间(S)</param>
        /// <returns></returns>
        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_CHECK", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32  PC_SET_CHECK(IntPtr ID_CON, int IN_NUM, int IN_TYPE, float IN_TIME);


        /*******************************
            描述：设置外部输出，三色灯，振动盘
            ID_CON:控制器句柄
            FUN_ATYPE：功能选择， 0 - 红灯， 1 - 绿灯， 2 - 振动盘
            OUT_NUM :输出口  -1 表示没有
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_LED_SHO", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32  PC_SET_LED_SHO(IntPtr ID_CON, int FUN_ATYPE, int OUT_NUM);


        /*******************************
            描述：设置视觉处理结果通讯方式
            ID_CON:控制器句柄
            MODE：通讯模式，0 - IO  ， 1 - 串口MODBUS_RTU    2 - 网口 MODBUS_TCP， 无实际串口或网口通讯时，直接操作MODBUS寄存器，选择串口或网口模式都可以
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_MODE", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32  PC_SET_MODE(IntPtr ID_CON, int mode);


        /*******************************
            描述：设置IO通讯起始IO编号
            ID_CON:控制器句柄
            IO_NUM：IO通讯起始IO编号  最小值0
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_IO_MODE", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32  PC_IO_MODE(IntPtr ID_CON, int IO_NUM);


        /*******************************
            描述：设置串口通讯参数
            ID_CON:控制器句柄
            COM_BUAD：波特率   一般用38400
            COM_DBIT：数据位   8位
            COM_CHECK：校验    0 -无检验  1 - 奇校验   2 - 偶校验
            COM_SBIT：停止位   0/1/2
            COM_ID：串口号     0/1/2  默认用 0 - 232
            COM_NUM：modbus_4X寄存器编号  默认0
            NUM_STAR：MODBUS_4X寄存器起始数值  一般设置 0
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_COM_MODE", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_COM_MODE(IntPtr ID_CON, int COM_BUAD, int COM_DBIT, int COM_CHECK, int COM_SBIT, int COM_ID, int COM_NUM, int NUM_STAR);


        /*******************************
            描述：设置网口通讯参数
            ID_CON:控制器句柄
            TCP_NUM：modbus_4X寄存器编号  默认0
            NUM_STAR：MODBUS_4X寄存器起始数值  一般设置 0
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_TCP_MODE", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_TCP_MODE(IntPtr ID_CON, int TCP_NUM, int NUM_STAR);


        /*******************************
            描述：读取相机触发路数
            ID_CON:控制器句柄
            num :相机路数
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_CAM_NUM", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_READ_CAM_NUM(IntPtr ID_CON, ref float num);


        /******************************
            描述：读取排料触发路数
            ID_CON:控制器句柄
            num :排料路数
        *******************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_OP_NUM", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_READ_OP_NUM(IntPtr ID_CON, ref float num);


        /******************************
            描述：转盘参数读取
            ID_CON:控制器句柄
            axis_Param : 转盘轴参数，依次是轴类型，驱动器一圈脉冲数（如有减速机，需要*减速比），旋转行程或角度（360°），速度，加速度，减速度，方向，按照顺序依次写入
        *******************************/
        /// <summary>
        ///  描述：转盘参数读取   
        /// </summary>
        /// <param name="ID_CON">ID_CON:控制器句柄</param>
        /// <param name="axis_Param"> axis_Param : 转盘轴参数，依次是轴类型，驱动器一圈脉冲数（如有减速机，需要*减速比），旋转行程或角度（360°），速度，加速度，减速度，方向，按照顺序依次写入</param>
        /// <returns></returns>
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_PARAM", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_READ_PARAM(IntPtr ID_CON, ref float[] axis_Param);


        /******************************
            描述：读取相机触发参数
            ID_CON:控制器句柄
            CAM_ID:相机ID，从0开始，依次向下，有最大数量限制
            CAM_POS:相机位置
            CAM_IO：相机触发IO
            CAM_TIME：相机触发电平有效时间(US)
            CAM_REG:相机触发信号反馈输入口编号，默认-1为无
        *******************************/
        /// <summary>
        ///  描述：读取相机触发参数
        /// </summary>
        /// <param name="ID_CON">ID_CON:控制器句柄</param>
        /// <param name="CAM_ID">CAM_ID:相机ID，从0开始，依次向下，有最大数量限制</param>
        /// <param name="CAM_POS">CAM_POS:相机位置</param>
        /// <param name="CAM_IO">CAM_IO：相机触发IO</param>
        /// <param name="CAM_TIME">CAM_TIME：相机触发电平有效时间(US)</param>
        /// <param name="CAM_REG">CAM_REG:相机触发信号反馈输入口编号，默认-1为无</param>
        /// <returns></returns>
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_POS", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32  PC_READ_POS(IntPtr ID_CON, int CAM_ID, ref float CAM_POS, ref float  CAM_IO, ref float CAM_TIME, ref float CAM_REG);


        /******************************
            描述：读取排料触发参数
            ID_CON:控制器句柄
            OP_ID:排料口ID，从0开始，依次向下，有最大数量限制
            OP_POS:排料位置
            OP_IO：排料触发IO
            OP_TIME：排料触发电平有效时间(US)
            IO_STATUS:操作状态
	        IO_OPFLAG：是否开启模拟排料 0- 否  1- 是
        *******************************/
        /// <summary>
        /// 描述：读取排料触发参数
        /// </summary>
        /// <param name="ID_CON">ID_CON:控制器句柄</param>
        /// <param name="OP_ID">OP_ID:排料口ID，从0开始，依次向下，有最大数量限制</param>
        /// <param name="OP_POS">排料位置</param>
        /// <param name="OP_IO">排料触发IO</param>
        /// <param name="OP_TIME">排料触发电平有效时间(US)</param>
        /// <param name="IO_STATUS">操作状态</param>
        /// <param name="IO_OP_FLAG">是否开启模拟排料  0- 否  1- 是</param>
        /// <returns></returns>
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_POSNG", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32  PC_READ_POSNG(IntPtr ID_CON, int OP_ID, ref float OP_POS, ref float OP_IO, ref float OP_TIME, ref float IO_STATUS, ref float IO_OP_FLAG);


        /*******************************
            描述：读取外部启动按钮
            ID_CON:控制器句柄
            IN_NUM :输入口  -1 表示没有
            //IN_TYPE:输入类型  1 - 常闭， 0 - 常开
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_STAR", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_READ_STAR(IntPtr ID_CON, ref int IN_NUM);


        /*******************************
            描述：读取外部停止按钮
            ID_CON:控制器句柄
            IN_NUM :输入口  -1 表示没有
            //IN_TYPE:输入类型  1 - 常闭， 0 - 常开
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_STOP", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_READ_STOP(IntPtr ID_CON, ref int IN_NUM);




        



        /*******************************
            描述：读取外部回零按钮
            ID_CON:控制器句柄
            IN_NUM :输入口  -1 表示没有
            //IN_TYPE:输入类型  1 - 常闭， 0 - 常开
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_DATUM", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_READ_DATUM(IntPtr ID_CON, ref int IN_NUM);


        /*******************************
            描述：读取无料检测
            ID_CON:控制器句柄
            IN_NUM :输入口  -1 表示没有
            //IN_TYPE:输入类型  1 - 常闭， 0 - 常开
            IN_TIME:无料检测时间(S)
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_CHECK", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_READ_CHECK(IntPtr ID_CON, ref int IN_NUM, ref float IN_TIME);



        /*******************************
            描述：读取外部输出，三色灯，振动盘
            ID_CON:控制器句柄
            FUN_ATYPE：功能选择， 0 - 红灯， 1 - 绿灯， 2 - 振动盘
            OUT_NUM :输出口  -1 表示没有
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_LED_SHO", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_READ_LED_SHO(IntPtr ID_CON, int FUN_ATYPE, ref int OUT_NUM);


        /*******************************
            描述：读取视觉处理结果通讯方式
            ID_CON:控制器句柄
            MODE：通讯模式，0 - IO  ， 1 - 串口MODBUS_RTU    2 - 网口 MODBUS_TCP， 无实际串口或网口通讯时，直接操作MODBUS寄存器，选择串口或网口模式都可以
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_MODE", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_READ_MODE(IntPtr ID_CON, ref int mode);


        /*******************************
            描述：读取IO通讯起始IO编号
            ID_CON:控制器句柄
            IO_NUM：IO通讯起始IO编号  最小值0
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_IO_MODE", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_READ_IO_MODE(IntPtr ID_CON, ref int IO_NUM);


        /*******************************
            描述：读取串口通讯参数
            ID_CON:控制器句柄
            COM_BUAD：波特率   一般用38400
            COM_DBIT：数据位   8位
            COM_CHECK：校验    0 -无检验  1 - 奇校验   2 - 偶校验
            COM_SBIT：停止位   0/1/2
            COM_ID：串口号     0/1/2  默认用 0 - 232
            COM_NUM：modbus_4X寄存器编号  默认0
            NUM_STAR：MODBUS_4X寄存器起始数值  一般设置 0
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_COM_MODE", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_READ_COM_MODE(IntPtr ID_CON, ref int COM_BUAD, ref int COM_DBIT, ref int COM_CHECK, ref int COM_SBIT, ref int COM_ID, ref int COM_NUM, ref int NUM_STAR);


        /*******************************
            描述：读取网口通讯参数
            ID_CON:控制器句柄
            TCP_NUM：modbus_4X寄存器编号  默认0
            NUM_STAR：MODBUS_4X寄存器起始数值  一般设置 0
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_TCP_MODE", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_READ_TCP_MODE(IntPtr ID_CON, ref int TCP_NUM, ref int NUM_STAR);


        /*******************************
            描述：读取轴当前位置
            ID_CON:控制器句柄
            AXIS_NUM：轴号
            AXIS_MPOS：轴反馈位置
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_MPOS", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_READ_MPOS(IntPtr ID_CON, int AXIS_NUM, ref float AXIS_MPOS);


        /*******************************
            描述：读取当前感应计数
            ID_CON:控制器句柄
            REG_COUNT：感应计数
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_REG", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_READ_REG(IntPtr ID_CON, ref int REG_COUNT);


        /*******************************
            描述：读取当前相机执行计数
            ID_CON:控制器句柄
            CAM_RUN_COUNT：执行计数
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_CAM_RUN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_READ_CAM_RUN(IntPtr ID_CON, ref int CAM_RUN_COUNT);


        /*******************************
            描述：读取当前相机跳过计数
            ID_CON:控制器句柄
            CAM_SKI_COUNT：跳过计数
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_CAM_SKI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_READ_CAM_SKI(IntPtr ID_CON, ref int CAM_SKI_COUNT);


        /*******************************
            描述：读取当前排料感应计数
            ID_CON:控制器句柄
            OP_REG_COUNT：感应计数
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_OP_REG", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_READ_OP_REG(IntPtr ID_CON, ref int OP_REG_COUNT);


        /*******************************
            描述：读取当前排料触发计数
            ID_CON:控制器句柄
            OP_RUN_COUNT：跳过计数
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_OP_RUN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_READ_OP_RUN(IntPtr ID_CON, ref int OP_RUN_COUNT);


        /*******************************
            描述：读取当前排料跳过计数
            ID_CON:控制器句柄
            OP_SKI_COUNT：跳过计数
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_OP_SKI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_READ_OP_SKI(IntPtr ID_CON, ref int OP_SKI_COUNT);


        /*******************************
            描述：筛选结果下发控制器，仅限于走串口或MODBUS_TCP方式
            ID_CON:控制器句柄
            RESULT：结果， 和设定的寄存器初始值有关系，如，modbus寄存器为0，初始值是8，那么第一个排料口对应的数值是8，第二个对应的9，以此类推
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_MODBUS_SET", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_MODBUS_SET(IntPtr ID_CON, int RESULT);


        /******************************************
        加载写入文件，ZMOTION.zar
        uResourceId: 文件名/ID
        szResourceType：文件后缀，类型
        szFileName：重新加载名字
        *******************************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_FILE", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_READ_FILE(UInt32 uResourceId, string szResourceType,string szFileName);


        /******************************************
            描述：初始化控制卡
            ID_CON:控制器句柄
        *******************************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_INT_CARD", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_INT_CARD(IntPtr ID_CON);


        /******************************************
        加载写入文件，ZMOTION.zar
        *******************************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_STAR_TRY", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_STAR_TRY();


        /*******************************
        描述：设置转盘使能输出口
        ID_CON:控制器句柄
        OP_NUM :输出口  -1 表示没有
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_ENABLE", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_SET_ENABLE(IntPtr ID_CON, int OP_NUM);


        /*******************************
        描述：设置转盘使能输出口状态
        ID_CON:控制器句柄
        OP_STATUS :输入口状态  0 - 关    1 - 开
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_SERVO_ENABLE", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_SERVO_ENABLE(IntPtr ID_CON, int OP_STATUS);


        /*******************************
        描述：设置排料常开输出口
        ID_CON:控制器句柄
        OP_NUM :输出口  -1 表示没有
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_OPEN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_SET_OPEN(IntPtr ID_CON, int OP_NUM);


        /*******************************
        描述：设置排料常开输出口状态
        ID_CON:控制器句柄
        OP_STATUS :输入口状态  0 - 关    1 - 开
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_OPEN_ENABLE", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_OPEN_ENABLE(IntPtr ID_CON, int OP_STATUS);


        /*******************************
        描述：读取转盘使能输出口
        ID_CON:控制器句柄
        OP_NUM :输出口  -1 表示没有
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_SERVO_ENABLE", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_READ_SERVO_ENABLE(IntPtr ID_CON, ref int OP_NUM);


        /*******************************
        描述：读取排料常开输出口
        ID_CON:控制器句柄
        OP_NUM :输出口  -1 表示没有
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_OPEN_ENABLE", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32  PC_READ_OPEN_ENABLE(IntPtr ID_CON, ref int OP_NUM);


        /*******************************
	    描述：读取指定编号相机执行计数
	    ID_CON:控制器句柄
	    CAM_ID：相机编号
	    CAM_RUN_COUNT：执行计数
	    ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_CURCAM_RUN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_READ_CURCAM_RUN(IntPtr ID_CON, int CAM_ID,  ref int CAM_RUN_COUNT);


        /*******************************
      描述：读取指定编号相机跳过计数
      ID_CON:控制器句柄
      CAM_ID：相机编号
      CAM_RUN_COUNT：跳过计数
      ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_CURCAM_SKI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_READ_CURCAM_SKI(IntPtr ID_CON, int CAM_ID, ref int CAM_RUN_COUNT);


        /*******************************
	描述：读取指定编号排料执行计数
	ID_CON:控制器句柄
	OP_ID：排料编号
	OP_RUN_COUNT：执行计数
	********************************/

        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_CUROP_RUN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_READ_CUROP_RUN(IntPtr ID_CON, int OP_ID, ref int OP_RUN_COUNT);


        /*******************************
	描述：读取指定编号排料跳过计数
	ID_CON:控制器句柄
	OP_ID：排料编号
	OP_RUN_COUNT：跳过计数
	********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_CUROP_SKI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_READ_CUROP_SKI(IntPtr ID_CON, int OP_ID, ref int OP_RUN_COUNT);



        /*******************************
	描述：读取指定编号相机已执行计数
	ID_CON:控制器句柄
	CAM_ID：相机编号
	CAM_RUN_COUNT：已执行计数
	********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_CURCAM_PS2", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_READ_CURCAM_PS2(IntPtr ID_CON, int CAM_ID, ref int CAM_RUN_COUNT);


        /*******************************
        描述：读取指定编号排料已执行计数
        ID_CON:控制器句柄
        OP_ID：排料编号
        OP_RUN_COUNT：已执行计数
        ********************************/

        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_CUROP_PS2", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_READ_CUROP_PS2(IntPtr ID_CON, int OP_ID, ref int OP_RUN_COUNT);




        /*******************************
	描述：设置过滤最大值、最小值
	ID_CON:控制器句柄
	MAX_SIZE :最大值
	MIN_SIZE:最小值
********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_SIZE", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_SET_SIZE(IntPtr ID_CON, float MAX_SIZE, float MIN_SIZE);

        /*******************************
      描述：设置过滤使能
      ID_CON:控制器句柄
      enable_status :0 - 关  1- 开
  ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_SIZE_enable", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_SET_SIZE_enable(IntPtr ID_CON, int enable_status);
  

        /*******************************
      描述：执行尺寸测量
      ID_CON:控制器句柄
      enable_status :0 - 关  1- 开
  ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_RUN_SIZE_MESUR", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_RUN_SIZE_MESUR(IntPtr ID_CON);

        /*******************************
      描述：读取尺寸过滤最大值、最小值
      ID_CON:控制器句柄
      max_size :最大值
      min_size：最小值
  ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_SIZE", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_READ_SIZE(IntPtr ID_CON, ref float max_size, ref float min_size);

        /*******************************
      描述：读取尺寸过滤使能状态
      ID_CON:控制器句柄
      status:使能状态
  ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_SIZE_enable", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_READ_SIZE_enable(IntPtr ID_CON, ref int status);

        /*******************************
      描述：读取测量尺寸
      ID_CON:控制器句柄
      mersuer_size：测量尺寸
  ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_MESURE_SIZE", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_READ_MESURE_SIZE(IntPtr ID_CON, ref float mersuer_size);



        /*******************************
	描述：读取料仓IO数组寄存器
	ID_CON:控制器句柄
	read_num ; 读取数量，最多20个
	product_lib_list：料仓顺序IO对应寄存器数组
    ********************************************/

        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_PRODUCT_LIB_LIST", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_READ_PRODUCT_LIB_LIST(IntPtr ID_CON, int read_num, ref float[] product_lib_list);

        /*******************************
      描述：写入料仓IO数组寄存器
      ID_CON:控制器句柄
      write_num ; 读取数量，最多20个
      product_lib_list：料仓顺序IO对应寄存器数组
  ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_PRODUCT_LIB_LIST", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_SET_PRODUCT_LIB_LIST(IntPtr ID_CON, int read_num, ref float [] product_lib_list);

        /******************************
        描述：配置运行、停止保护时间，先振动盘操作，再轴操作
        ID_CON:控制器句柄
        protect_time:保护时间,单位时间S,最小值0
    ********************************/

        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_PROTEC", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_SET_PROTEC(IntPtr ID_CON, float protect_time);

        /*******************************
      描述：读取运行、停止保护时间
      ID_CON:控制器句柄
      protect_time:保护时间,单位时间S,最小值0
  ********************************/

        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_PROTEC", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_READ_PROTEC(IntPtr ID_CON,ref float protect_time);

        /******************************
      描述：配置延时取样时间
      ID_CON : 控制器句柄
      protect_time : 延时取样时间, 单位时间US, 最小值0
  *******************************/

        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_PRORT", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_SET_PRORT(IntPtr ID_CON, float protect_time);

        /*******************************
      描述：读取延时取样时间
      ID_CON : 控制器句柄
      protect_time : 延时取样时间, 单位时间US, 最小值0
  ********************************/

        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_PRORT", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_READ_PRORT(IntPtr ID_CON,ref float protect_time);

        /******************************
      描述：配置延时取样时间开关状态
      ID_CON : 控制器句柄
      protect_time_flag : 0 -close 1 -open
  *******************************/

        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_PRORTFLAG", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_SET_PRORTFLAG(IntPtr ID_CON, int protect_time_flag);

        /*******************************
      描述：读取延时取样时间开关状态
      ID_CON : 控制器句柄
      protect_time_flag : 0 -close 1 -open
  ********************************/

        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_PROTECFLAG", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_READ_PROTECFLAG(IntPtr ID_CON, ref int protect_time_flag);



        /******************************
   描述：单步自动运行
   ID_CON : 控制器句柄
*******************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_SINGLE_RUN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_SINGLE_RUN(IntPtr ID_CON);



        /******************************
      描述：配置料仓IO数据
      ID_CON:控制器句柄
      lib_id:料仓编号
      lib_in_num:料仓检查输入口
      lib_tip_in:料仓状态提示口
      lib_alm_in:料仓告警口
  *******************************/

        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_PRO_LIB", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_SET_PRO_LIB(IntPtr ID_CON, int lib_id, int lib_in_num, int lib_tip_in, int lib_alm_in);

        /******************************
      描述：读取料仓IO数据
      ID_CON:控制器句柄
      lib_id:料仓编号
      lib_in_num:料仓检查输入口
      lib_tip_in:料仓状态提示口
      lib_alm_in:料仓告警口
  *******************************/

        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_PRO_LIB", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_READ_PRO_LIB(IntPtr ID_CON, int lib_id, ref int lib_in_num, ref int lib_tip_in, ref int lib_alm_in);

        /******************************
      描述：读取料仓IO状态
      ID_CON:控制器句柄
      lib_id:料仓编号
      lib_in_num:料仓检查输入口状态
  *******************************/

        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_PRO_LIB_STATUS", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_READ_PRO_LIB_STATUS(IntPtr ID_CON, int lib_id, ref uint lib_in_num);


        /******************************
      描述：操作料仓提示IO
      ID_CON:控制器句柄
      lib_id:料仓编号
      lib_tip_status:料仓状态 0 - 关闭， 1 - 常开 2 - 闪烁
      lib_tip_timems:闪烁时间(MS)
  *******************************/

        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_PRO_TIP", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_SET_PRO_TIP(IntPtr ID_CON, int lib_id, int lib_tip_status, int lib_tip_timems);

        /******************************
      描述：操作料仓告警IO
      ID_CON:控制器句柄
      lib_id:料仓编号
      lib_alm_status:料仓状态 0 - 关闭， 1 - 常开 2 - 闪烁
      lib_alm_timems:闪烁时间(MS)
  *******************************/

        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_PRO_ALM", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_SET_PRO_ALM(IntPtr ID_CON, int lib_id, int lib_alm_status, int lib_alm_timems);




        /************** 2023 / 07 / 27 新加 心跳检测 \ 窗口操作功能、 提示信息写入功能 和  紧急停止功能s******************/




        /******************************
     描述：设置心跳检查开关状态
     ID_CON : 控制器句柄
     STATUS : 0 -close 1 -open
 *******************************/

        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_HEARTBEAT", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_SET_HEARTBEAT(IntPtr ID_CON, int STATUS);


        /******************************
    描述：读取心跳检查开关状态
    ID_CON : 控制器句柄
    STATUS : 0 -close 1 -open
*******************************/

        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_HEARTBEAT", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_READ_HEARTBEAT(IntPtr ID_CON, ref int STATUS);



        /******************************
     描述：设置心跳检测时间
     ID_CON : 控制器句柄
     TIME : 检测有效时间, 单位时间US, 最小值0
 *******************************/

        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_HEARTBEAT_TIME", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_SET_HEARTBEAT_TIME(IntPtr ID_CON, float TIME);



        /******************************
     描述：读取心跳检测时间
     ID_CON : 控制器句柄
     TIME : 检测有效时间, 单位时间US, 最小值0
 *******************************/

        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_HEARTBEAT_TIME", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_READ_HEARTBEAT_TIME(IntPtr ID_CON, ref float TIME);





        /******************************
      描述：下发心跳交互值
      ID_CON : 控制器句柄
      Heart_Beat_Num : 心跳交互值，默认值为0， 非零为心跳有效值,规定时间内，若检查到心跳交互值有效（非零），心跳交互值自动复位，若检查时间到，心跳交互值仍为0，则触发停机保护
  *******************************/

        [DllImport("zmc_xp.dll", EntryPoint = "PC_HEARTBEAT_SEND", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_HEARTBEAT_SEND(IntPtr ID_CON, float Heart_Beat_Num);




        /******************************
      描述：读取心跳交互值
      ID_CON : 控制器句柄
      Heart_Beat_Num : 心跳交互值，默认值为0， 非零为心跳有效值,规定时间内，若检查到心跳交互值有效（非零），心跳交互值自动复位，若检查时间到，心跳交互值仍为0，则触发停机保护
  *******************************/

        [DllImport("zmc_xp.dll", EntryPoint = "PC_HEARTBEAT_READ", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_HEARTBEAT_READ(IntPtr ID_CON, ref float Heart_Beat_Num);





        /******************************
     描述：操作提示窗口显示内容
     ID_CON:控制器句柄
      TIPS ： 提示内容 ，字符串形式
 *******************************/

        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_TIPSSTRING", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_SET_TIPSSTRING(IntPtr ID_CON, string TIPS);




        /******************************
     描述：打开提示窗口
     ID_CON:控制器句柄
 *******************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_OPEN_TIPSWINDOWS", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_OPEN_TIPSWINDOWS(IntPtr ID_CON);






        /*******************************
            描述：设置外部急停停止按钮
            ID_CON:控制器句柄
            IN_NUM :输入口  -1 表示没有
            IN_TYPE:输入类型  1 - 常闭， 0 - 常开
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_EMERCYSTOP", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_SET_EMERCYSTOP(IntPtr ID_CON, int IN_NUM, int IN_TYPE);



        /*******************************
            描述：读取外部紧急停止按钮
            ID_CON:控制器句柄
            IN_NUM :输入口  -1 表示没有
            //IN_TYPE:输入类型  1 - 常闭， 0 - 常开
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_EMERCYSTOP", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32 PC_READ_EMERCYSTOP(IntPtr ID_CON, ref int IN_NUM);


        /*******************************
     描述：执行消警
     ID_CON:控制器句柄
 ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_CLEAR_ALM", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_CLEAR_ALM(IntPtr ID_CON);




        /*******************************
            描述：筛选结果下发控制器，仅限于走串口或MODBUS_TCP方式
            ID_CON:控制器句柄
            RESULT：结果,排料口号， 和设定的寄存器初始值有关系，如，modbus寄存器为0，初始值是8，那么第一个排料口对应的数值是8，第二个对应的9，以此类推
            data_num: 排料的料编号
        ********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_MODBUS_SET2", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32 PC_MODBUS_SET2(IntPtr ID_CON, int RESULT, int data_num);


        /*******************************
	描述：设置排料信息下发模式，仅限于走串口或MODBUS_TCP方式
	ID_CON:控制器句柄
	RESULT：模式  0 - PC_MODBUS_SET    1 - PC_MODBUS_SET2

********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_MODBUS_SET_MODELR", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32 PC_MODBUS_SET_MODELR(IntPtr ID_CON, int RESULT);


        /******************************
         描述：设置前置排料位编号
         ID_CON : 控制器句柄
         STATUS : 编号
     *******************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_FORCE", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32 PC_SET_FORCE(IntPtr ID_CON, int STATUS);

        /******************************
	描述：读取前置排料位编号
	ID_CON : 控制器句柄
	STATUS : 编号
*******************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_FORCE", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32 PC_READ_FORCE(IntPtr ID_CON, ref  int STATUS);




        /*******************************
	描述：读取指定编号相机已执行下降沿计数
	ID_CON:控制器句柄
	CAM_ID：相机编号
	CAM_RUN_COUNT：已执行下降沿计数
	********************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_CURCAM_PS2_DOWN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_READ_CURCAM_PS2_DOWN(IntPtr ID_CON, int CAM_ID, ref int CAM_RUN_COUNT);



        /*******************************
     描述：设置光纤偏移百分比比例
     ID_CON:控制器句柄
     num：比例值  范围 0 - 100
 ********************************/

        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_SIZE_PUL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_SET_SIZE_PUL(IntPtr ID_CON, float num);


        /******************************
	描述：读取取样间隔功能的模式：延时模式或非延时模式
	ID_CON : 控制器句柄
	MODE : 取样间隔模式 0 - 非延时 1 - 延时模式,(注：非延时模式下，当在设定时间周期内，如果出现一次以上的光电触发信号，则在这个周期中触发的光电信号)
*******************************/
        /// <summary>
        ///  描述：读取取样间隔功能的模式：延时模式或非延时模式
        /// </summary>
        /// <param name="ID_CON">ID_CON:控制器句柄</param>
        /// <param name="MODE"> MODE : 取样间隔模式 0 - 非延时 1 - 延时模式,(注：非延时模式下，当在设定时间周期内，如果出现一次以上的光电触发信号，则在这个周期中触发的光电信号)</param>
        /// <returns></returns>
        [DllImport("zmc_xp.dll", EntryPoint = "PC_GET_Time_Mode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32 PC_GET_Time_Mode(IntPtr ID_CON, ref int MODE);

        /******************************
   描述：设置取样间隔功能的模式：延时模式或非延时模式
   ID_CON : 控制器句柄
   MODE : 取样间隔模式 0 - 非延时 1 - 延时模式,(注：非延时模式下，当在设定时间周期内，如果出现一次以上的光电触发信号，则在这个周期中触发的光电信号)
*******************************/
        /// <summary>
        ///  描述：设置取样间隔功能的模式：延时模式或非延时模式
        /// </summary>
        /// <param name="ID_CON">ID_CON:控制器句柄</param>
        /// <param name="MODE"> MODE : 取样间隔模式 0 - 非延时 1 - 延时模式,(注：非延时模式下，当在设定时间周期内，如果出现一次以上的光电触发信号，则在这个周期中触发的光电信号)</param>
        /// <returns></returns>
        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_Time_Mode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

        public static extern Int32 PC_SET_Time_Mode(IntPtr ID_CON, int MODE);

        #region [自动结果综合]

        /*********************************自动结果综合******************************************/


        /******************************
	    描述：设置自动综合结果模式
	    ID_CON : 控制器句柄
	    MODE :自动综合模式   0 - 上位机综合下发，需要上位机配合使用主动上报函数，在主动上报函数中根据上报的数据进行分析综合载调用PC_MODBUS_SET或者PC_MODBUS_SET2指令下发结果

						     1 - 下位机综合预设, 需要提前设置好每个料仓所接受的相机结果，控制器会在相机执行完之后自动按照预设的结果进行比对，比对符合后自动触发对应排料口动作
        *******************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_Synth_Mode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_SET_Synth_Mode(IntPtr ID_CON, int MODE);


        /******************************
	        描述：读取自动综合结果模式
	        ID_CON : 控制器句柄
	        MODE :自动综合模式   0 - 上位机综合下发，需要上位机配合使用主动上报函数，在主动上报函数中根据上报的数据进行分析综合载调用PC_MODBUS_SET或者PC_MODBUS_SET2指令下发结果

						         1 - 下位机综合预设, 需要提前设置好每个料仓所接受的相机结果，控制器会在相机执行完之后自动按照预设的结果进行比对，比对符合后自动触发对应排料口动作
        *******************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_Synth_Mode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_READ_Synth_Mode(IntPtr ID_CON, ref int MODE);

        /******************************
	        描述：设置自动综合IO起始点
	        ID_CON : 控制器句柄
	        num : 输入口编号，相机的结果反馈IO基于此值顺延，即当 num = 10时，相机0对应IN10，相机1对应IN11，依次类推
	    *******************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_Synth_IO_Start", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_SET_Synth_IO_Start(IntPtr ID_CON, int num);


        /******************************
             描述：读取自动综合IO起始点
             ID_CON : 控制器句柄
             num : 输入口编号，相机的结果反馈IO基于此值顺延，即当 num = 10时，相机0对应IN10，相机1对应IN11，依次类推
         *******************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_Synth_IO_Start", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_READ_Synth_IO_Start(IntPtr ID_CON, ref int num);


        /******************************
	        描述：设置自动综合TCP寄存器起始点
	        ID_CON : 控制器句柄
	        num : 寄存器编号，相机的结果反馈modbus寄存器基于此值顺延，即当 num = 10时，相机0对应modbus 4X 的 10号寄存器，相机1对应modbus 4X 的 11号寄存器，依次类推
        *******************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_Synth_TCP_Start", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_SET_Synth_TCP_Start(IntPtr ID_CON, int num);


        /******************************
	        描述：读取自动综合TCP寄存器起始点
	        ID_CON : 控制器句柄
	        num : 寄存器编号，相机的结果反馈modbus寄存器基于此值顺延，即当 num = 10时，相机0对应modbus 4X 的 10号寄存器，相机1对应modbus 4X 的 11号寄存器，依次类推
        *******************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_Synth_TCP_Start", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_READ_Synth_TCP_Start(IntPtr ID_CON, ref int num);


        /******************************
	        描述：设置自动综合串口寄存器起始点
	        ID_CON : 控制器句柄
	        num : 寄存器编号，相机的结果反馈modbus寄存器基于此值顺延，即当 num = 10时，相机0对应modbus 4X 的 10号寄存器，相机1对应modbus 4X 的 11号寄存器，依次类推
        *******************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_Synth_COM_Start", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_SET_Synth_COM_Start(IntPtr ID_CON, int num);


        /******************************
	        描述：读取自动综合串口寄存器起始点
	        ID_CON : 控制器句柄
	        num : 寄存器编号，相机的结果反馈modbus寄存器基于此值顺延，即当 num = 10时，相机0对应modbus 4X 的 10号寄存器，相机1对应modbus 4X 的 11号寄存器，依次类推
        *******************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_Synth_TCP_Start", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_READ_Synth_COM_Start(IntPtr ID_CON, ref int num);


        /******************************
		    描述：设置自动综合模式下相机结果检出最小时间
		    ID_CON : 控制器句柄
		    num : 自动综合模式下相机结果检出最小时间，单位US,最小值 1
	    *******************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_Synth_CamOut_MinTime", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_SET_Synth_CamOut_MinTime(IntPtr ID_CON, int num);


        /******************************
	        描述：读取自动综合模式下相机结果检出最小时间
	        ID_CON : 控制器句柄
	        num : 自动综合模式下相机结果检出最小时间，单位US,最小值 1
        *******************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_Synth_CamOut_MinTime", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_READ_Synth_CamOut_MinTime(IntPtr ID_CON, ref int num);


        /******************************
	        描述：设置自动综合模式下相机结果检出最大时间
	        ID_CON : 控制器句柄
	        num : 自动综合模式下相机结果检出最大时间，单位US,最小值 1
        *******************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_Synth_CamOut_MaxTime", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_SET_Synth_CamOut_MaxTime(IntPtr ID_CON, int num);


        /******************************
	        描述：读取自动综合模式下相机结果检出最大时间
	        ID_CON : 控制器句柄
	        num : 自动综合模式下相机结果检出最大时间，单位US,最小值 1
        *******************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_Synth_CamOut_MaxTime", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_READ_Synth_CamOut_MaxTime(IntPtr ID_CON, ref int num);



        /******************************
	        描述：设置指定编号料仓的预设结果
	        ID_CON : 控制器句柄
	        op_num : 设置的料仓编号
	        pep_result : 预设结果字符串，必须严格按照相机个数和下发数据值设置  格式 结果码0-结果码1-结果码2，如 2500-2500-2500-2500-2500
        *******************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_Synth_Result", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_SET_Synth_Result(IntPtr ID_CON, int op_num, string result);


        /******************************
	        描述：读取指定编号料仓的预设结果
	        ID_CON : 控制器句柄
	        op_num : 设置的料仓编号
	        pep_result : 预设结果字符串，必须严格按照相机个数和下发数据值设置  格式 结果码0-结果码1-结果码2，如 2500-2500-2500-2500-2500
        *******************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_READ_Synth_Result", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_READ_Synth_Result(IntPtr ID_CON, int op_num, ref string result);


        /*************************************************************
		    描述: 主动上报的回调函数格式（主动上报，PC综合结果排料）
		    ID_CON : 控制器句柄
		    itypecode ： 上报的当前处理的料的编号
		    idatalength ：上报字符信息长度
		    pdata : 上报的数据，实际是字符串，需要转换成 char * 类型，再对数据进行解析，
				     数据格式如下所示，每个相机的结果之间使用字符 “ - ”来链接： 相机0结果-相机1结果-相机2结果-相机3结果
				 
				    （注） 解析数据判断当前料排放口后，调用zmc_xp的库的PC_MODBUS/PC_MODBUS2指令按照参数格式下发进行排料
	    *************************************************************/
        public delegate void ZAuxCallBack(IntPtr handle, Int32 itypecode, Int32 idatalength, [MarshalAs(UnmanagedType.LPArray, SizeConst = 2048)]byte[] pdata);

        /******************************
		    描述：设置相机综合结果主动上报函数
		    ID_CON : 控制器句柄
		    pcallback : 主动上报回调函数（主动上报 触发后的执行函数，用于做结果综合逻辑）
	    *******************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_AUTOUP", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_SET_AUTOUP(IntPtr ID_CON, zmcaux.ZAuxCallBack pcallback);


        /******************************
		    描述：设置相机综合结果主动上报函数2,采用GCHandle.Alloc
		    ID_CON : 控制器句柄
		    pcallback : 主动上报回调函数（主动上报 触发后的执行函数，用于做结果综合逻辑）
	    *******************************/
        public static Int32 PC_SET_AUTOUP2(IntPtr ID_CON, zmcaux.ZAuxCallBack pcallback)
        {
            ght = GCHandle.Alloc(pcallback);

            int ret =   zmcaux.ZAux_SetAutoUpCallBack(ID_CON, (zmcaux.ZAuxCallBack)ght.Target);

            return ret;

        }


        /******************************
	        描述：下发 自动综合模式下 指定编号相机结果,仅限于自动综合串口（COM）或 自动综合TCP模式下使用
	        ID_CON : 控制器句柄
	        cam_num : 相机编号
	        send_data : 下发的结果
        *******************************/
        [DllImport("zmc_xp.dll", EntryPoint = "PC_SET_Synth_CurCam_result", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PC_SET_Synth_CurCam_result(IntPtr ID_CON, int cam_num, int send_data);


        #endregion

    }
}
