using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaymentsCalc
{
    public partial class Calc : Form
    {
        public Calc()
        {
            InitializeComponent();
        }
        //银行账单
        List<int> bankTransfer;
        //付款记录
        List<int> duePayments;
        //计算结果
        List<int> result;
        //计时器
        DateTime beginTime;
        int bankValue;

        public delegate void drawLbItemsBack(int times, int type, Stack<int> result1,List<int> result2);
        #region 初始化操作
        /// <summary>
        /// 考虑到精度及计算量问题，直接将小数转为int
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int convertToInt(string item)
        {
            if (string.IsNullOrEmpty(item))
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(Convert.ToDecimal(item) * 100);
            }
        }
        /// <summary>
        /// 将需要显示的int转换为两位小数的decimal
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        decimal convertToDecimal(int item)
        {
            if (item <= 0)
            {
                return 0;
            }
            else
            {
                return Convert.ToDecimal(item) / 100;
            }
        }

        private void fileSelectbtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            //考虑到其它excel形式多样，限制后缀
            openFile.Filter = "excel|*.csv;";
            openFile.FilterIndex = 1;
            //获取桌面文件夹路径为初始地址  
            //openPic.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);                                  
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                duePayments = openCSV(openFile.FileName, out bankTransfer);
                lbPay.Items.Clear();
                lbImportData(duePayments);
                clbBank.Items.Clear();
                clbImportData(bankTransfer,true);
            }
        }
        /// <summary>
        /// 绑定付款记录
        /// </summary>
        /// <param name="duePayments"></param>
        void lbImportData(List<int> duePayments)
        {
            foreach (int item in duePayments)
            {
                lbPay.Items.Add(convertToDecimal(item));
            }
        }
        /// <summary>
        /// 绑定银行账单
        /// </summary>
        /// <param name="bankTransfer"></param>
        void clbImportData(List<int> bankTransfer,bool title)
        {
            if (title) {
                clbBank.Items.Add("请选择");
                clbBank.SelectedIndex = 0;
            }
           
            foreach (int item in bankTransfer)
            {
                clbBank.Items.Add(convertToDecimal(item));
            }
        }
        /// <summary>
        /// 输出日志
        /// </summary>
        void drawLbItems(int times, int type, Stack<int> result1,List<int> result2)
        {
            lbResult.Items.Add(string.Format("-----------耗时{0}毫秒-----------", times));
            if (type == 1)
            {
                foreach (int item in result1)
                {
                    lbResult.Items.Add(convertToDecimal( item));
                }
            } else {
                foreach (int item in result2)
                {
                    lbResult.Items.Add(convertToDecimal(item));
                }
            }
           


        }
        #endregion

        #region 导入方法
        /// <summary>
        /// 导入方法
        /// </summary>
        /// <param name="fullFileName">路径</param>
        /// <param name="bankTransfer">返回银行账单账单</param>
        /// <returns>返回付款记录</returns>
        public List<int> openCSV(string fullFileName, out List<int> bankTransfer)
        {
            List<int> duePayments = new List<int>();
            bankTransfer = new List<int>();
            FileStream fs = new FileStream(fullFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine;
            //列分割符号
            string[] separators = { "," };
            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
            {
                strLine = strLine.Trim();
                aryLine = strLine.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
                //两列则有账单，否则只有付款记录
                if (aryLine.Count() > 1)
                {
                    bankTransfer.Add(convertToInt(aryLine[1]));
                }
                duePayments.Add(convertToInt(aryLine[0]));
            }
            sr.Close();
            fs.Close();
            return duePayments;
        }
       
        #endregion

      
        void calcByPruning(int bankValue, List<int> duePayment)
        {
            duePayment = duePayment.OrderByDescending(o => o).ToList();
            int count = duePayment.Count();
            result = new List<int>();
            using (BackgroundWorker bw = new BackgroundWorker())
            {
                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(runWorkerCompleted);
                bw.DoWork += new DoWorkEventHandler(bw_DoWork);
                bw.RunWorkerAsync();
            }
        }
        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            //筛掉比总值大的数据，去除重复数据,倒叙排列
            duePayments = duePayments.Where(o=>o< bankValue).Distinct().OrderByDescending(o => o).ToList();
            int count = duePayments.Count();
            result = new List<int>();
            for (int j = 0; j < count; j++)
            {
                //以每个数字为主计算
                result.Add(duePayments[j]);
                calcPruning(bankValue, duePayments, j);
                //计算完毕初始化结果
                result = new List<int>();
            }
        }
        void runWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //这时后台线程已经完成，并返回了主线程，所以可以直接使用UI控件了 
            // MessageBox.Show(string.Format("计算完毕，总共耗时{0}毫秒",(DateTime.Now-beginTime).Milliseconds));
            this.label3.Text = string.Format("计算完毕，总共耗时{0}秒", (DateTime.Now - beginTime).TotalSeconds);
            clbImportData(bankTransfer, true);
        }
        /// <summary>
        /// 剪枝操作跳转，减少大量无需计算的数字
        /// </summary>
        /// <param name="bankValue">总值</param>
        /// <param name="duePayment">全部数据</param>
        /// <param name="j">当前数字</param>
        void calcPruning(int bankValue, List<int> duePayment, int j)
        {
            //全部数据的最大长度
            int count = duePayment.Count;
            for (int i = j + 1; i < count; i++)
            {
                //汇总 获取当前计算值中的数据总值
                int nowReuslt = result.Sum();
                //差值=总值-当前数据总值
                int temp = bankValue - nowReuslt;
                //如果差值比当前需要计算的数字大，则直接加入计算
                if (temp > duePayment[i])
                {
                    //获取所有小于当前数值的数进行汇总，如果总值小于差值，则向上回溯
                    var max = duePayment.Where(o => o < duePayment[i]);
                    if (max.Sum() < temp)
                    {
                        result.Add(duePayment[i]);
                        //直接回溯
                        i = count - 1;
                    }
                    //否则继续计算
                    else
                    {
                        result.Add(duePayment[i]);
                    }


                }
                //如果差值等于当前的数字，找到一个正确解
                else if (temp == duePayment[i])
                {
                    result.Add(duePayment[i]);
                    drawLbItemsBack drawItem = drawLbItems;
                    lbResult.Invoke(drawItem, (DateTime.Now - beginTime).Milliseconds, 3, null,result);
                    break;
                }
                //如果差值小于了当前数字，则做剪枝操作，向下跳转
                else
                {
                    //获取当前数组中小于temp的所有数字
                    var last = duePayment.Where(o => o <= temp);
                    if (last.Count() > 0)
                    {
                        //获取最接近temp的数组下标
                        i = duePayment.IndexOf(duePayment.Where(o => o <= temp).OrderByDescending(o => o).First()) - 1;
                    }
                    else
                    {
                        i = count - 1;
                    }
                }
                //如果一个循环周期未找到正确数字，待计算数组向上回溯
                if (i == count - 1)
                {
                    if (result.Count > 0) {
                        i = duePayment.IndexOf(result[result.Count - 1]);
                        //移除最小值向上
                        result.RemoveAt(result.Count - 1);
                    }
                    //获取当前大数组中当前计算的最小的值，为i指定下标跳过
                    
                    if (i == count - 1&&result.Count>0) {
                        i = duePayment.IndexOf(result[result.Count - 1]);
                        //移除最小值再次向上回溯
                        result.RemoveAt(result.Count - 1);
                        if (result.Count == 0) {
                            break;
                        }
                    }
                   
                }


            }

        }
        #region 计算第一稿 计算繁复无法满足功能
        /// <summary>
        /// 栈解决  伪多项式量级，考虑优化
        /// </summary>
        /// <param name="bankValue"></param>
        /// <param name="duePayment"></param>
        //void calcByStack(int bankValue, List<int> duePayment)
        //{
        //    DateTime dtbegin = DateTime.Now;
        //    //倒叙排列可以快速得到值 但是数字越小越难计算 
        //    duePayment = duePayment.OrderByDescending(o => o).ToList();
        //    //duePayment = duePayment.OrderBy(o => o).ToList();
        //    int count = duePayment.Count();
        //    Stack<int> result = new Stack<int>();
        //    int temp = 0;
        //    for (int i = 0; i < count; i++)
        //    {
        //        temp++;
        //        //排除单数字大于结果的数字
        //        if (duePayment[i] > bankValue)
        //        {
        //            continue;
        //        }
        //        //栈加入
        //        result.Push(duePayment[i]);
        //        //获取当前栈
        //        int nowSum = result.Sum();
        //        if (nowSum > bankValue)
        //        {
        //            result.Pop();
        //        }
        //        else if (nowSum == bankValue)
        //        {
        //            int times = (DateTime.Now - dtbegin).Milliseconds;
        //            dtbegin = DateTime.Now;
        //            drawLbItems(times, 1, result,null);
        //            result.Pop();
        //        }

        //        if (i == count - 1)
        //        {
        //            if (nowSum < bankValue)
        //            {
        //                result.Pop();
        //            }
        //            if (result.Count() > 0)
        //            {
        //                i = duePayment.IndexOf(result.Peek());
        //                result.Pop();
        //            }

        //        }
        //    }
        //}
        #endregion
        private void clbBank_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string selectStr = clbBank.SelectedItem.ToString();

            if (selectStr == "请选择")
            {
                return;
            }
            bankValue = convertToInt(selectStr);
            if (bankTransfer.Contains(bankValue))
            {
                clbBank.Items.Clear();
                beginTime = DateTime.Now;
                //锁定，无法继续选择
                clbImportData(bankTransfer.Where(o => o <0).ToList(),false);
                this.label3.Text = string.Format("计算{0}数字中，大约需要60秒钟请稍等",convertToDecimal( bankValue));
                lbResult.Items.Clear();
                //calcByStack(bankValue, duePayments);
                calcByPruning(bankValue, duePayments);
                this.clbBank.Enabled = true;
                
            }
            else
            {
                MessageBox.Show("数据非法，在文件中无法找到所选数字，请确认且重新导入");
            }
        }
    }
}
