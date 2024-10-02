using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Dynamic;
using System.Net;
using System.Threading;
using System.Security;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

using Box;
using Microsoft.Win32;
using Microsoft.VisualBasic;
using System.Security.AccessControl;



namespace Box
{

    
    public enum Rty
    {
        sex = 1,
        rt = 2,
    }

    /// <summary>
    /// MESSAGEBOX
    /// </summary>
   public class MessageBoxMidle
    {
        private static IWin32Window _owner;
        private static HookProc _hookProc;
        private static IntPtr _hHook;
        public static DialogResult Show(string text)
        {
            Initialize();
            return MessageBox.Show(text);
        }
        public static DialogResult Show(string text, string caption)
        {
            Initialize();
            return MessageBox.Show(text, caption);
        }
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
        {
            Initialize();
            return MessageBox.Show(text, caption, buttons);
        }
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            Initialize();
            return MessageBox.Show(text, caption, buttons, icon);
        }
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton)
        {
            Initialize();
            return MessageBox.Show(text, caption, buttons, icon, defButton);
        }
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton, MessageBoxOptions options)
        {
            Initialize();
            return MessageBox.Show(text, caption, buttons, icon, defButton, options);
        }
        public static DialogResult Show(IWin32Window owner, string text)
        {
            _owner = owner; Initialize();
            return MessageBox.Show(owner, text);
        }
        public static DialogResult Show(IWin32Window owner, string text, string caption)
        {
            _owner = owner;
            Initialize();
            return MessageBox.Show(owner, text, caption);
        }
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
        {
            _owner = owner;
            Initialize();
            return MessageBox.Show(owner, text, caption, buttons);
        }
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            _owner = owner;
            Initialize();
            return MessageBox.Show(owner, text, caption, buttons, icon);
        }
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton)
        {
            _owner = owner;
            Initialize();
            return MessageBox.Show(owner, text, caption, buttons, icon, defButton);
        }
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton, MessageBoxOptions options)
        {
            _owner = owner;
            Initialize();
            return MessageBox.Show(owner, text, caption, buttons, icon, defButton, options);
        }
        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        public delegate void TimerProc(IntPtr hWnd, uint uMsg, UIntPtr nIDEvent, uint dwTime);
        public const int WH_CALLWNDPROCRET = 12;
        public enum CbtHookAction :
        int
        {
            HCBT_MOVESIZE = 0,
            HCBT_MINMAX = 1,
            HCBT_QS = 2,
            HCBT_CREATEWND = 3,
            HCBT_DESTROYWND = 4,
            HCBT_ACTIVATE = 5,
            HCBT_CLICKSKIPPED = 6,
            HCBT_KEYSKIPPED = 7,
            HCBT_SYSCOMMAND = 8,
            HCBT_SETFOCUS = 9
        }

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle lpRect);
        [DllImport("user32.dll")]
        private static extern int MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("User32.dll")]
        public static extern UIntPtr SetTimer(IntPtr hWnd, UIntPtr nIDEvent, uint uElapse, TimerProc lpTimerFunc);
        [DllImport("User32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
        [DllImport("user32.dll")]
        public static extern int UnhookWindowsHookEx(IntPtr idHook);
        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern int GetWindowTextLength(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int maxLength);
        [DllImport("user32.dll")]
        public static extern int EndDialog(IntPtr hDlg, IntPtr nResult);
        [StructLayout(LayoutKind.Sequential)]
        public struct CWPRETSTRUCT
        {
            public IntPtr lResult;
            public IntPtr lParam;
            public IntPtr wParam;
            public uint message;
            public IntPtr hwnd;
        };
        static MessageBoxMidle()
        {
            _hookProc = new HookProc(MessageBoxHookProc);
            _hHook = IntPtr.Zero;
        }
        private static void Initialize()
        {
            if (_hHook != IntPtr.Zero)
            {
                throw new NotSupportedException("multiple calls are not supported");
            }
            if (_owner != null)
            {
                _hHook = SetWindowsHookEx(WH_CALLWNDPROCRET, _hookProc, IntPtr.Zero, AppDomain.GetCurrentThreadId());
            }
        }
        private static IntPtr MessageBoxHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
            {
                return CallNextHookEx(_hHook, nCode, wParam, lParam);
            }
            CWPRETSTRUCT msg = (CWPRETSTRUCT)Marshal.PtrToStructure(lParam, typeof(CWPRETSTRUCT));
            IntPtr hook = _hHook;
            if (msg.message == (int)CbtHookAction.HCBT_ACTIVATE)
            {
                try
                {
                    CenterWindow(msg.hwnd);
                }
                finally
                {
                    UnhookWindowsHookEx(_hHook); _hHook = IntPtr.Zero;
                }
            }
            return CallNextHookEx(hook, nCode, wParam, lParam);
        }
        private static void CenterWindow(IntPtr hChildWnd)
        {
            Rectangle recChild = new Rectangle(0, 0, 0, 0);
            bool success = GetWindowRect(hChildWnd, ref recChild);
            int width = recChild.Width - recChild.X;
            int height = recChild.Height - recChild.Y;
            Rectangle recParent = new Rectangle(0, 0, 0, 0);
            success = GetWindowRect(_owner.Handle, ref recParent);
            Point ptCenter = new Point(0, 0);
            ptCenter.X = recParent.X + ((recParent.Width - recParent.X) / 2);
            ptCenter.Y = recParent.Y + ((recParent.Height - recParent.Y) / 2);
            Point ptStart = new Point(0, 0);
            ptStart.X = (ptCenter.X - (width / 2));
            ptStart.Y = (ptCenter.Y - (height / 2));
            ptStart.X = (ptStart.X < 0) ? 0 : ptStart.X;
            ptStart.Y = (ptStart.Y < 0) ? 0 : ptStart.Y;
            int result = MoveWindow(hChildWnd, ptStart.X, ptStart.Y, width, height, false);
        }
    }//Class_end



    /// <summary>
    /// 控件跟随窗体大小而变化
    /// </summary>
    public class Size_set
    {

        public static float X;//当前窗体的宽度
        public static float Y;//当前窗体的高度

        /// <summary>
        /// 将控件的宽，高，左边距，顶边距和字体大小暂存到tag属性中
        /// </summary>
        /// <param name="cons">递归控件中的控件</param>
        public void setTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                if (con.Controls.Count > 0)
                    setTag(con);
            }
        }


        //根据窗体大小调整控件大小
        public void setControls(float newx, float newy, Control cons)
        {
            //遍历窗体中的控件，重新设置控件的值
            foreach (Control con in cons.Controls)
            {

                string[] mytag = con.Tag.ToString().Split(new char[] { ':' });//获取控件的Tag属性值，并分割后存储字符串数组
                float a = System.Convert.ToSingle(mytag[0]) * newx;//根据窗体缩放比例确定控件的值，宽度
                con.Width = (int)a;//宽度
                a = System.Convert.ToSingle(mytag[1]) * newy;//高度
                con.Height = (int)(a);
                a = System.Convert.ToSingle(mytag[2]) * newx;//左边距离
                con.Left = (int)(a);
                a = System.Convert.ToSingle(mytag[3]) * newy;//上边缘距离
                con.Top = (int)(a);
                Single currentSize = System.Convert.ToSingle(mytag[4]) * newy;//字体大小
                con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                if (con.Controls.Count > 0)
                {
                    setControls(newx, newy, con);
                }
            }
        }

    }



    /// <summary>
    /// 数据转换
    /// </summary>
    public class Parse_convert
    {
        /// <summary>
        /// 将数字转成中文大写
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public string NumToChinese(string x)
        {
            //数字转换为中文后的数组 
            string[] P_array_num = new string[] { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };
            //为数字位数建立一个位数组  
            string[] P_array_digit = new string[] { "", "拾", "佰", "仟" };
            //为数字单位建立一个单位数组  
            string[] P_array_units = new string[] { "", "万", "亿", "万亿" };
            string P_str_returnValue = ""; //返回值  
            int finger = 0; //字符位置指针  
            int P_int_m = x.Length % 4; //取模  
            int P_int_k = 0;
            if (P_int_m > 0)
                P_int_k = x.Length / 4 + 1;
            else
                P_int_k = x.Length / 4;
            //外层循环,四位一组,每组最后加上单位: ",万亿,",",亿,",",万,"  
            for (int i = P_int_k; i > 0; i--)
            {
                int P_int_L = 4;
                if (i == P_int_k && P_int_m != 0)
                    P_int_L = P_int_m;
                //得到一组四位数  
                string four = x.Substring(finger, P_int_L);
                int P_int_l = four.Length;
                //内层循环在该组中的每一位数上循环  
                for (int j = 0; j < P_int_l; j++)
                {
                    //处理组中的每一位数加上所在的位  
                    int n = Convert.ToInt32(four.Substring(j, 1));
                    if (n == 0)
                    {
                        if (j < P_int_l - 1 && Convert.ToInt32(four.Substring(j + 1, 1)) > 0 && !P_str_returnValue.EndsWith(P_array_num[n]))
                            P_str_returnValue += P_array_num[n];
                    }
                    else
                    {
                        if (!(n == 1 && (P_str_returnValue.EndsWith(P_array_num[0]) | P_str_returnValue.Length == 0) && j == P_int_l - 2))
                            P_str_returnValue += P_array_num[n];
                        P_str_returnValue += P_array_digit[P_int_l - j - 1];
                    }
                }
                finger += P_int_L;
                //每组最后加上一个单位:",万,",",亿," 等  
                if (i < P_int_k) //如果不是最高位的一组  
                {
                    if (Convert.ToInt32(four) != 0)
                        //如果所有4位不全是0则加上单位",万,",",亿,"等  
                        P_str_returnValue += P_array_units[i - 1];
                }
                else
                {
                    //处理最高位的一组,最后必须加上单位  
                    P_str_returnValue += P_array_units[i - 1];
                }
            }

            return P_str_returnValue;
        }


        /// <summary> 
        /// 将中文数字转换成阿拉伯数字 
        /// </summary> 
        /// <param name="cnNumber"></param> 
        /// <returns></returns> 
        public int ConverToDigit(string cnNumber)
        {

            int result = 0;

            int temp = 0;

            foreach (char c in cnNumber)
            {

                int temp1 = ToDigit(c);

                if (temp1 == 10000)

                {

                    result += temp;

                    result *= 10000;

                    temp = 0;

                }

                else if (temp1 > 9)

                {

                    if (temp1 == 10 && temp == 0) temp = 1;

                    result += temp * temp1;

                    temp = 0;

                }

                else temp = temp1;

            }

            result += temp;

            return result;

        }

        /// <summary> 
        /// 将中文数字转换成阿拉伯数字 
        /// </summary> 
        /// <param name="cn"></param> 
        /// <returns></returns> 
        public int ToDigit(char cn)
        {

            int number = 0;

            switch (cn)
            {

                case '壹':

                case '一':

                    number = 1;

                    break;

                case '两':

                case '贰':

                case '二':

                    number = 2;

                    break;

                case '叁':

                case '三':

                    number = 3;

                    break;

                case '肆':

                case '四':

                    number = 4;

                    break;

                case '伍':

                case '五':

                    number = 5;

                    break;

                case '陆':

                case '六':

                    number = 6;

                    break;

                case '柒':

                case '七':

                    number = 7;

                    break;

                case '捌':

                case '八':

                    number = 8;

                    break;

                case '玖':

                case '九':

                    number = 9;

                    break;

                case '拾':

                case '十':

                    number = 10;

                    break;

                case '佰':

                case '百':

                    number = 100;

                    break;

                case '仟':

                case '千':

                    number = 1000;

                    break;

                case '萬':

                case '万':

                    number = 10000;

                    break;

                case '零':

                default:

                    number = 0;

                    break;

            }

            return number;

        }


        /// <summary> 
        /// 将中文数字转换成阿拉伯数字 
        /// </summary> 
        /// <param name="cnDigit"></param> 
        /// <returns></returns> 
        public long ToLong(string cnDigit)
        {

            long result = 0;

            string[] str = cnDigit.Split('亿');

            result = ConverToDigit(str[0]);

            if (str.Length > 1)

            {

                result *= 100000000;

                result += ConverToDigit(str[1]);

            }

            return result;

        }


        /// <summary>
        /// 设置对象小数位数
        /// </summary>
        /// <param name="GN"></param>
        /// <param name="num"></param>
        /// <param name="GN2"></param>
        public void Set_DigitBit(System.Globalization.NumberFormatInfo GN, int num, ref System.Globalization.NumberFormatInfo GN2)
        {
            GN = new System.Globalization.CultureInfo("zh-CN", false).NumberFormat;

            GN.CurrencyDecimalDigits = num;

            GN2 = GN;

        }


        /// <summary>
        /// 设转货币形式
        /// </summary>
        /// <param name="GN"></param>
        /// <param name="num"></param>
        /// <param name="GN2"></param>
        public string Set_DigitFram(string x)
        {
            string tsr = "";

            double sgsg;


            if (double.TryParse(x, out sgsg))
            {
                System.Globalization.NumberFormatInfo GN = new System.Globalization.CultureInfo("zh-CN", false).NumberFormat;

                GN.CurrencyGroupSeparator = ".";

                tsr = sgsg.ToString("C", GN);

            }

            return tsr;

        }


        /// <summary>
        /// 字符串转ASCLL
        /// </summary>
        /// <param name="str"></param>
        /// <param name="ascll"></param>
        public void String_2_Ascll(string str, ref byte[] ascll)
        {
            string str1 = str.Trim();   // 去掉字符串首尾处的空格

            char[] charBuf = str.ToArray();    // 将字符串转换为字符数组

            ASCIIEncoding charToASCII = new ASCIIEncoding();

            ascll = new byte[charBuf.Length];  // 定义发送缓冲区；

            ascll = charToASCII.GetBytes(charBuf);   // 转换为各字符对应的ASCII

        }


        /// <summary>
        /// ASCLL转字符串
        /// </summary>
        /// <param name="ascll"></param>
        /// <param name="str"></param>
        public void Ascll_2_String(byte[] ascll, ref string str)
        {

            if (ascll.Length != 0)
            {
                //for (int i = 0; i < ascll.Length; i++)
                // {


                //ASCIIEncoding ASCIITochar = new ASCIIEncoding();
                // char[] ascii = ASCIITochar.GetChars(ascll);      // 将接收字节解码为ASCII字符数组
                // str += ascii[i];


                // }

                str = ASCIIEncoding.Default.GetString(ascll);

            }
            
        }

        /// <summary>
        /// ASCLL转字符串
        /// </summary>
        /// <param name="ascll"></param>
        /// <param name="str"></param>
        public void Ascll_2_String(ref string str, byte[] ascll)
        {

            if (ascll.Length != 0)
            {
                str = Encoding.Default.GetString(ascll);
            }

        }



        /// <summary>
        /// 表格数据输出成Excel
        /// </summary>
        /// <param name="dataGridView1"></param>
        public static void DataToExcel(DataGridView dataGridView1)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Execl files (*.xls)|*.xls";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.CreatePrompt = true;
            saveFileDialog.Title = "Export Excel File";
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName == "")
            {
                return;
            }
            Stream myStream;
            myStream = saveFileDialog.OpenFile();
            StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(-0));

            string str = "";

            try
            {
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    if (i > 0)
                    {
                        str += "\t";
                    }
                    str += dataGridView1.Columns[i].HeaderText;
                }
                sw.WriteLine(str);
                for (int j = 0; j < dataGridView1.Rows.Count; j++)
                {
                    string tempStr = "";
                    for (int k = 0; k < dataGridView1.Columns.Count; k++)
                    {
                        if (k > 0)
                        {
                            tempStr += "\t";
                        }

                        if (dataGridView1.Rows[j].Cells[k].Value != null)
                        {
                            tempStr += dataGridView1.Rows[j].Cells[k].Value.ToString();
                        }

                    }
                    sw.WriteLine(tempStr);
                }
                sw.Close();
                myStream.Close();

                MessageBox.Show("导出表格成功");

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sw.Close();
                myStream.Close();

            }
        }



        /// <summary>
        /// 数据导出成TXT
        /// </summary>
        /// <param name="dataGridView1"></param>
        public static void DataToTxT(DataGridView dataGridView1)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "DOC文档(.doc)|*.doc|DOCX文档(.docx)|*.docx|TXT文档(.txt)|*.txt";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.CreatePrompt = true;
            saveFileDialog.Title = "Export DOC File";
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName == "")
            {
                return;
            }
            Stream myStream;
            myStream = saveFileDialog.OpenFile();
            StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(-0));

            string str = "";

            try
            {
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    if (i > 0)
                    {
                        str += "\t";
                    }
                    str += dataGridView1.Columns[i].HeaderText;
                }
                sw.WriteLine(str);
                for (int j = 0; j < dataGridView1.Rows.Count; j++)
                {
                    string tempStr = "";
                    for (int k = 0; k < dataGridView1.Columns.Count; k++)
                    {
                        if (k > 0)
                        {
                            tempStr += "\t";
                        }

                        if (dataGridView1.Rows[j].Cells[k].Value != null)
                        {
                            tempStr += dataGridView1.Rows[j].Cells[k].Value.ToString();
                        }

                    }
                    sw.WriteLine(tempStr);
                }
                sw.Close();
                myStream.Close();

                MessageBox.Show("导出文档成功");

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sw.Close();
                myStream.Close();

            }
        }



    }



    /// <summary>
    /// 定时win32的SystemTime的内部数据，顺序不能改变
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SystemTime
    {
        /// <summary>
        /// 年份
        /// </summary>
        public ushort wYear;

        /// <summary>
        /// 月份
        /// </summary>
        public ushort wMonth;

        /// <summary>
        /// 一周的第几天
        /// </summary>
        public ushort wDayOfWeek;

        /// <summary>
        /// 日期
        /// </summary>
        public ushort wDay;

        /// <summary>
        /// 时间
        /// </summary>
        public ushort wHour;

        /// <summary>
        /// 分钟
        /// </summary>
        public ushort wMinute;

        /// <summary>
        /// 秒
        /// </summary>
        public ushort wSecond;

        /// <summary>
        /// MS
        /// </summary>
        public ushort wMiliseconds;
    }




    /// <summary>
    /// WIN 32 外部引用
    /// </summary>
    public class Win32
    {

        /// <summary>
        /// 设置系统时间，美国的时间
        /// </summary>
        /// <param name="sysTime"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll")]
        public static extern bool SetSystemTime(ref SystemTime sysTime);

        /// <summary>
        /// 设置本地时间
        /// </summary>
        /// <param name="sysTime"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll")]
        public static extern bool SetLocalTime(ref SystemTime sysTime);

        /// <summary>
        /// 获取系统时间，取得的是美国时间
        /// </summary>
        /// <param name="sysTime"></param>
        [DllImport("Kernel32.dll")]
        public static extern void GetSystemTime(ref SystemTime sysTime);

        /// <summary>
        /// 获取本地时间
        /// </summary>
        /// <param name="sysTime"></param>
        [DllImport("Kernel32.dll")]
        public static extern void GetLocalTime(ref SystemTime sysTime);
    }


    /// <summary>
    /// 排序算法集合
    /// </summary>
    public class Param_Swap
    {

        /// <summary>
        /// 快速排序算法
        /// </summary>
        /// <param name="array"></param>
        public  void QuickSort(int[] array)
        {
            Sort(array, 0, array.Length - 1);
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="array">需要排列的数组</param>
        /// <param name="low">低位,开始位置</param>
        /// <param name="high">高位，结束位置</param>
        public void Sort(int[] array, int low, int high)
        {
            if (low >= high)
            {
                return;
            }
            // 基准值 选取当前数组第一个值
            int index = array[low];
            // 低位i从左向右扫描
            int i = low;
            // 高位j从右向左扫描
            int j = high;
            while (i < j)
            {
                while (i < j && array[j] >= index)
                {
                    j--;
                }
                if (i < j)
                {
                    array[i] = array[j];
                    i++;
                }
                while (i < j && array[i] < index)
                {
                    i++;
                }
                if (i < j)
                {
                    array[j] = array[i];
                    j--;
                }
            }
            array[i] = index;
            // 左边的继续递归排序
            Sort(array, low, i - 1);
            // 右边的继续递归排序
            Sort(array, i + 1, high);
        }



        //冒泡排序（从数组的起始位置开始遍历，以大数为基准：大的数向下沉一位）
        public void BubbleSortFunction(int[] array)
        {
            try
            {
                int length = array.Length;
                int temp;
                bool hasExchangeAction; //记录此次大循环中相邻的两个数是否发生过互换（如果没有互换，则数组已经是有序的）

                for (int i = 0; i < length - 1; i++)    //数组有N个数，那么用N-1次大循环就可以排完
                {
                    hasExchangeAction = false;  //每次大循环都假设数组有序

                    for (int j = 0; j < length - i - 1; j++)    //从数组下标0处开始遍历，（length - i - 1 是刨除已经排好的大数）
                    {
                        if (array[j] > array[j + 1])    //相邻两个数进行比较，如果前面的数大于后面的数，则将这相邻的两个数进行互换
                        {
                            temp = array[j];
                            array[j] = array[j + 1];
                            array[j + 1] = temp;
                            hasExchangeAction = true;   //发生过互换
                        }
                    }

                    if (!hasExchangeAction) //如果没有发生过互换，则数组已经是有序的了，跳出循环
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        public void ZhiJieChaRuPaiXu(int[] data, int n)
        {

            int i, j;
            for (i = 1; i < n; i++)
            {
                //最外层

                if (data[i] < data[i - 1])
                {
                    //若第2个元素小于前一个元素，则需要插入和挪位置
                    int tmp = data[i];
                    data[i] = data[i - 1];

                    for (j = i - 2; j >= 0 && data[j] > tmp; j--)
                    {
                        //将这些元素统统后移
                        data[j + 1] = data[j];
                    }
                    data[j + 1] = tmp;


                }


            }
        }

    }





    public struct ID
    {
        private double width { get; set; }
        public double height { get; set; }

        public double Area(double x,double y)
        {
            return x * y;
        }
    }


    /// <summary>
    /// 窗体调用方法
    /// </summary>
    public class Form_Operate
    {

        /// <summary>
        /// 关闭窗口时提醒
        /// </summary>
        /// <param name="e"></param>
        public void Closing_Form(FormClosingEventArgs e)
        {
            if (MessageBox.Show("确认关闭窗口", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 打开窗体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="form_num"></param>
        public void Form_Open<T>(Form form_num)
        {

            form_num.ShowDialog();


        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="form_num"></param>
        public void Form_Close<T>(Form form_num)
        {

            form_num.Close();

        }

        /// <summary>
        /// 弹出提示窗体
        /// </summary>
        /// <param name="str"></param>
        /// <param name="title"></param>
        public void Tip_Window_Open(string str,string title)
        {
            MessageBox.Show(str.ToString(), title.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
           
        }


        /// <summary>
        /// 对窗体进行闪烁，handle可以是窗体的handle属性，也可以是控件的
        /// </summary>
        /// <param name="Handle"></param>
        /// <param name="Invert"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool FlashWindow(IntPtr Handle, bool Invert);

        public const UInt32 FLASHW_STOP = 0;
        public const UInt32 FLASHW_CAPTION = 1;
        public const UInt32 FLASHW_TRAY = 2;
        public const UInt32 FLASHW_ALL = 3;
        public const UInt32 FLASHW_TIMER = 4;
        public const UInt32 FLASHW_TIMERNOFG = 12;


       public enum AnimateWindowFlags
       {
            /// <summary>
            /// 从左到右打开窗口
            /// </summary>
            AW_HOR_POSITIVE = 0x00000001, // 从左到右打开窗口
            /// <summary>
            /// 从右到左打开窗口
            /// </summary>
            AW_HOR_NEGATIVE = 0x00000002, // 从右到左打开窗口
            /// <summary>
            /// 从上到下打开窗口
            /// </summary>
            AW_VER_POSITIVE = 0x00000004, // 从上到下打开窗口
            /// <summary>
            /// 从下到上打开窗口
            /// </summary>
            AW_VER_NEGATIVE = 0x00000008, // 从下到上打开窗口
            /// <summary>
            /// 若使用了AW_HIDE标志，则使窗口向内重叠；若未使用AW_HIDE标志，则使窗口向外扩展
            /// </summary>
            AW_CENTER = 0x00000010, //若使用了AW_HIDE标志，则使窗口向内重叠；若未使用AW_HIDE标志，则使窗口向外扩展。
            /// <summary>
            /// 隐藏窗口，缺省则显示窗口。
            /// </summary>
            AW_HIDE = 0x00010000, //隐藏窗口，缺省则显示窗口。
            /// <summary>
            /// 激活窗口。在使用了AW_HIDE标志后不要使用这个标志。
            /// </summary>
            AW_ACTIVATE = 0x00020000, //激活窗口。在使用了AW_HIDE标志后不要使用这个标志。
            /// <summary>
            /// 使用滑动类型。缺省则为滚动动画类型。当使用AW_CENTER标志时，这个标志就被忽略。
            /// </summary>
            AW_SLIDE = 0x00040000, //使用滑动类型。缺省则为滚动动画类型。当使用AW_CENTER标志时，这个标志就被忽略。
            /// <summary>
            /// 使用淡出效果。只有当hWnd为顶层窗口的时候才可以使用此标志。
            /// </summary>
            AW_BLEND = 0x00080000, //使用淡出效果。只有当hWnd为顶层窗口的时候才可以使用此标志。

       }

        /// <summary>
        /// 窗口动画显示
        /// </summary>
        /// <param name="hWnd">窗体或控件句柄句柄</param>
        /// <param name="time">动画持续时间US单位,标准时间200US</param>
        /// <param name="flags">动画形式标志</param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool AnimateWindow(IntPtr hWnd, int time, AnimateWindowFlags flags);



        /// <summary>
        /// 改变控件字体大小
        /// </summary>
        /// <param name="control"></param>
        /// <param name="size"></param>
        /// <param name="mode"></param>
        public static void Riting(Control control, float size, int mode)
        {
            if (mode == 0)
            {

                control.Font = new Font("宋体", size, FontStyle.Regular);
            }
            else if (mode == 1)
            {
                control.Font = new Font("宋体", size, FontStyle.Bold);
            }

        }


    }


    /// <summary>
    /// INI 文件读写
    /// </summary>
    public class IniFile
    {
        [DllImport("kernel32")]
        private static extern long GetPrivateProfileString(string section, string key, string defVal, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        /// <summary>    
        /// 读取INI文件    
        /// </summary>    
        /// <param name="section">项目名称(如 [section] )</param>    
        /// <param name="skey">键</param>   
        /// <param name="path">路径</param> 
        public static string IniReadValue(string section, string skey, string path)
        {
            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(section, skey, "", temp, 1024, path);
            return temp.ToString();
        }

        /// <summary>
        /// 写入ini文件
        /// </summary>
        /// <param name="section">项目名称</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="path">路径</param>
        public static void IniWrite(string section, string key, string value, string path)
        {
            WritePrivateProfileString(section, key, value, path);
        }

    }




    /// <summary>
    /// 日志文件
    /// </summary>
    public class LogFile
    {


        /// <summary>
        /// 日志文件写入
        /// </summary>
        /// <param name="input">写入的信息</param>
        /// <param name="folderpath">当前文件夹下日志文件夹名称</param>
        /// <param name="filename">当前文件夹下日志文件夹中的日志名称</param>
        public static void WriteLogFile(string input, string folderpath, string filename)
        {
            string logPath = Directory.GetCurrentDirectory() + "\\" + folderpath;

            //判断该路径下文件夹是否存在，不存在的情况下新建文件夹
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
            //指定日志文件的目录
            string fname = logPath + "\\" + filename;

            //定义文件信息对象
            FileInfo finfo = new FileInfo(fname);

            if (!finfo.Exists)
            {
                FileStream fs;
                fs = File.Create(fname);
                fs.Close();
                finfo = new FileInfo(fname);
            }

            //判断文件是否存在以及是否大于2K
            if (finfo.Length > 1024 * 1024 * 10)
            {
                //文件超过10MB则重命名
                File.Move(Directory.GetCurrentDirectory() + "\\" + filename, Directory.GetCurrentDirectory() + DateTime.Now.TimeOfDay + "\\" + filename);
            }
            //创建只写文件流
            using (FileStream fs = finfo.OpenWrite())
            {
                //根据上面创建的文件流创建写数据流
                StreamWriter w = new StreamWriter(fs);

                //设置写数据流的起始位置为文件流的末尾
                w.BaseStream.Seek(0, SeekOrigin.End);

                //写入“Log Entry : ”
                w.Write("\n\rLog Entry : ");

                //写入当前系统时间并换行
                w.Write("{0} {1} \n\r", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());

                //写入日志内容并换行
                w.Write(input + "\n\r");

                //写入------------------------------------“并换行
                w.Write("------------------------------------\n\r");

                //清空缓冲区内容，并把缓冲区内容写入基础流
                w.Flush();

                //关闭写数据流
                w.Close();
            }
        }
    }


    /// <summary>
    /// 伪装文件夹
    /// </summary>
    public class DiSope
    {

        /// <summary>
        /// 伪装文件夹识别码枚举类型
        /// </summary>
        public enum 文件标识符
        {
            我的电脑 = 0,

            我的文档 = 1,

            拨号网络 = 2,

            控制面板 = 3,

            计划任务 = 4,

            打印机 = 5,

            网络邻居 = 6,

            回收站 = 7,

            公文包 = 8,

            字体 = 9, 

            Web文件夹 = 10,

            安全文件 = 11,

        }

        /// <summary>
        /// 选择伪装文件夹的伪装识别码字符
        /// </summary>
        /// <param name="num">识别码类型</param>
        /// <returns>返回识别码字符</returns>
        public string GetFolType(文件标识符 num)
        {
            int Tid = Convert.ToInt32(num);
            switch (Tid)
            {
                case 0: return @"{20D04FE0-3AEA-1069-A2D8-08002B30309D}";
                case 1: return @"{450D8FBA-AD25-11D0-98A8-0800361B1103}";
                case 2: return @"{992CFFA0-F557-101A-88EC-00DD010CCC48}";
                case 3: return @"{21EC2020-3AEA-1069-A2DD-08002B30309D}";
                case 4: return @"{D6277990-4C6A-11CF-8D87-00AA0060F5BF}";
                case 5: return @"{2227A280-3AEA-1069-A2DE-08002B30309D}";
                case 6: return @"{208D2C60-3AEA-1069-A2D7-08002B30309D}";
                case 7: return @"{645FF040-5081-101B-9F08-00AA002F954E}";
                case 8: return @"{85BBD920-42A0-1069-A2E4-08002B30309D}";
                case 9: return @"{BD84B380-8CA2-1069-AB1D-08000948F534}";
                case 10: return @"{BDEADF00-C265-11d0-BCED-00A0C90AB50F}";
                case 11: return @"{2559a1f2-21d7-11d4-bdaf-00c04f60b9f0}";
            }
            return @"{20D04FE0-3AEA-1069-A2D8-08002B30309D}";
        }

        /// <summary>
        /// 伪装文件夹
        /// </summary>
        /// <param name="folderpath">伪装文件夹路径</param>
        /// <param name="foldertype_str">伪装类型识别码字符</param>
        public void Camouflage(string folderpath,string foldertype_str)
        {
            StreamWriter sw = File.CreateText(folderpath.Trim() + @"\desktop.ini");
            sw.WriteLine(@"[.ShellClassInfo]");
            sw.WriteLine("CLSID=" + foldertype_str);
            sw.Close();
            File.SetAttributes(folderpath.Trim() + @"\desktop.ini", FileAttributes.Hidden);
            File.SetAttributes(folderpath.Trim(), FileAttributes.System);
            MessageBox.Show("伪装成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        /// <summary>
        /// 还原伪装
        /// </summary>
        /// <param name="folderpath">还原的伪装文件夹路径</param>
        public void Cancelflage(string folderpath)
        {
            if (folderpath == "")
            {
                MessageBox.Show("请选择加密过的文件夹！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    FileInfo fi = new FileInfo(folderpath.Trim() + @"\desktop.ini");
                    if (!fi.Exists)
                    {
                        MessageBox.Show("该文件夹没有被伪装！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(1000);
                        File.Delete(folderpath + @"\desktop.ini");
                        File.SetAttributes(folderpath.Trim(), FileAttributes.System);
                        MessageBox.Show("还原成功", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch
                {
                    MessageBox.Show("不要多次还原！");
                }
            }
        }

    }


    /// <summary>
    /// 文件处理函数
    /// </summary>
    public class File_Class
    {
        /// <summary>
        /// 获取文件夹大小
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static long GetDirectorySize(string path)
        {
            long size = 0;
            DirectoryInfo dir = new DirectoryInfo(path);

            foreach (FileInfo file in dir.GetFiles())
            {
                size += file.Length;
            }

            foreach (DirectoryInfo subDir in dir.GetDirectories())
            {
                size += GetDirectorySize(subDir.FullName);
            }

            return size;
        }


        // <summary>  
        /// 递归搜索文件方法  
        /// </summary>  
        /// <param name="path">搜索的目录</param>  
        /// <param name="name">搜索的文件名</param>  
        public void GetDir(string path, string name)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            DirectorySecurity s = new DirectorySecurity(path, AccessControlSections.Access);

            //判断目录是否 可以访问  
            if (!s.AreAccessRulesProtected)
            {
                foreach (DirectoryInfo d in di.GetDirectories())
                {
                    foreach (FileInfo fi in di.GetFiles())
                    {
                        if (fi.Name.Contains(name))
                        {
                            //txtInfo.AppendText("文件名：" + fi.Name + " 路径：" + fi.FullName + " \n");

                            //添加处理过程
                        }
                    }

                    GetDir(d.FullName, name);
                }
            }
        }

    }


 
    


}



/// <summary>
/// 文件加密算法
/// </summary>
namespace DESFile
{
    /// <summary>
    /// 异常处理类
    /// </summary>
    public class CryptoHelpException : ApplicationException
    {
        public CryptoHelpException(string msg) : base(msg) { }
    }

    /// <summary>
    /// CryptHelp
    /// </summary>
    public class DESFileClass
    {
        private const ulong FC_TAG = 0xFC010203040506CF;

        private const int BUFFER_SIZE = 128 * 1024;

        /// <summary>
        /// 检验两个Byte数组是否相同
        /// </summary>
        /// <param name="b1">Byte数组
        /// <param name="b2">Byte数组
        /// <returns>true－相等</returns>
        private static bool CheckByteArrays(byte[] b1, byte[] b2)
        {
            if (b1.Length == b2.Length)
            {
                for (int i = 0; i < b1.Length; ++i)
                {
                    if (b1[i] != b2[i])
                        return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 创建DebugLZQ ,http://www.cnblogs.com/DebugLZQ
        /// </summary>
        /// <param name="password">密码
        /// <param name="salt">
        /// <returns>加密对象</returns>
        private static SymmetricAlgorithm CreateRijndael(string password, byte[] salt)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, salt, "SHA256", 1000);

            SymmetricAlgorithm sma = Rijndael.Create();
            sma.KeySize = 256;
            sma.Key = pdb.GetBytes(32);
            sma.Padding = PaddingMode.PKCS7;
            return sma;
        }

        /// <summary>
        /// 加密文件随机数生成
        /// </summary>
        private static RandomNumberGenerator rand = new RNGCryptoServiceProvider();

        /// <summary>
        /// 生成指定长度的随机Byte数组
        /// </summary>
        /// <param name="count">Byte数组长度
        /// <returns>随机Byte数组</returns>
        private static byte[] GenerateRandomBytes(int count)
        {
            byte[] bytes = new byte[count];
            rand.GetBytes(bytes);
            return bytes;
        }

        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="inFile">待加密文件
        /// <param name="outFile">加密后输入文件
        /// <param name="password">加密密码
        public static void EncryptFile(string inFile, string outFile, string password)
        {
            using (FileStream fin = File.OpenRead(inFile),
              fout = File.OpenWrite(outFile))
            {
                long lSize = fin.Length; // 输入文件长度
                int size = (int)lSize;
                byte[] bytes = new byte[BUFFER_SIZE]; // 缓存
                int read = -1; // 输入文件读取数量
                int value = 0;

                // 获取IV和salt
                byte[] IV = GenerateRandomBytes(16);
                byte[] salt = GenerateRandomBytes(16);

                // 创建加密对象
                SymmetricAlgorithm sma = DESFileClass.CreateRijndael(password, salt);
                sma.IV = IV;

                // 在输出文件开始部分写入IV和salt
                fout.Write(IV, 0, IV.Length);
                fout.Write(salt, 0, salt.Length);

                // 创建散列加密
                HashAlgorithm hasher = SHA256.Create();
                using (CryptoStream cout = new CryptoStream(fout, sma.CreateEncryptor(), CryptoStreamMode.Write),
                  chash = new CryptoStream(Stream.Null, hasher, CryptoStreamMode.Write))
                {
                    BinaryWriter bw = new BinaryWriter(cout);
                    bw.Write(lSize);

                    bw.Write(FC_TAG);

                    // 读写字节块到加密流缓冲区
                    while ((read = fin.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        cout.Write(bytes, 0, read);
                        chash.Write(bytes, 0, read);
                        value += read;
                    }
                    // 关闭加密流
                    chash.Flush();
                    chash.Close();

                    // 读取散列
                    byte[] hash = hasher.Hash;

                    // 输入文件写入散列
                    cout.Write(hash, 0, hash.Length);

                    // 关闭文件流
                    cout.Flush();
                    cout.Close();
                }
            }
        }

        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="inFile">待解密文件
        /// <param name="outFile">解密后输出文件
        /// <param name="password">解密密码
        public static void DecryptFile(string inFile, string outFile, string password)
        {
            // 创建打开文件流
            using (FileStream fin = File.OpenRead(inFile),
              fout = File.OpenWrite(outFile))
            {
                int size = (int)fin.Length;
                byte[] bytes = new byte[BUFFER_SIZE];
                int read = -1;
                int value = 0;
                int outValue = 0;

                byte[] IV = new byte[16];
                fin.Read(IV, 0, 16);
                byte[] salt = new byte[16];
                fin.Read(salt, 0, 16);

                SymmetricAlgorithm sma = DESFileClass.CreateRijndael(password, salt);
                sma.IV = IV;

                value = 32;
                long lSize = -1;

                // 创建散列对象, 校验文件
                HashAlgorithm hasher = SHA256.Create();

                using (CryptoStream cin = new CryptoStream(fin, sma.CreateDecryptor(), CryptoStreamMode.Read),
                  chash = new CryptoStream(Stream.Null, hasher, CryptoStreamMode.Write))
                {
                    // 读取文件长度
                    BinaryReader br = new BinaryReader(cin);
                    lSize = br.ReadInt64();
                    ulong tag = br.ReadUInt64();

                    if (FC_TAG != tag)
                        throw new CryptoHelpException("文件被破坏");

                    long numReads = lSize / BUFFER_SIZE;

                    long slack = (long)lSize % BUFFER_SIZE;

                    for (int i = 0; i < numReads; ++i)
                    {
                        read = cin.Read(bytes, 0, bytes.Length);
                        fout.Write(bytes, 0, read);
                        chash.Write(bytes, 0, read);
                        value += read;
                        outValue += read;
                    }

                    if (slack > 0)
                    {
                        read = cin.Read(bytes, 0, (int)slack);
                        fout.Write(bytes, 0, read);
                        chash.Write(bytes, 0, read);
                        value += read;
                        outValue += read;
                    }

                    chash.Flush();
                    chash.Close();

                    fout.Flush();
                    fout.Close();

                    byte[] curHash = hasher.Hash;

                    // 获取比较和旧的散列对象
                    byte[] oldHash = new byte[hasher.HashSize / 8];
                    read = cin.Read(oldHash, 0, oldHash.Length);
                    if ((oldHash.Length != read) || (!CheckByteArrays(oldHash, curHash)))
                        throw new CryptoHelpException("文件被破坏");
                }

                if (outValue != lSize)
                    throw new CryptoHelpException("文件大小不匹配");
            }
        }
    }
}



