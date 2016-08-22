
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Drawing;
namespace ProjectToYou.Code
{
    /**//// <summary>
    /// 打印类
    /// </summary>
    public class PrinterDao
    {
        private DataGridView dataview;
        private PrintDocument printDoc;
        //打印有效区域的宽度
        int width;
        int height;
        int columns;
        double Rate;
        bool hasMorePage = false;
        int currRow = 0;
        int rowHeight = 20;
        //打印页数
        int PageNumber;
        //当前打印页的行数
        int pageSize = 20;
        //当前打印的页码
        int PageIndex;
        private int PageWidth; //打印纸的宽度
        private int PageHeight; //打印纸的高度
        private int LeftMargin; //有效打印区距离打印纸的左边大小
        private int TopMargin;//有效打印区距离打印纸的上面大小
        private int RightMargin;//有效打印区距离打印纸的右边大小
        private int BottomMargin;//有效打印区距离打印纸的下边大小
        int rows;
        /**//// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataview">要打印的DateGridView</param>
        /// <param name="printDoc">PrintDocument用于获取打印机的设置</param>
        public void Printer(DataGridView dataview, PrintDocument printDoc)
        {
            this.dataview = dataview;
            this.printDoc = printDoc;
            PageIndex = 0;
            //获取打印数据的具体行数
            this.rows = dataview.RowCount;
            this.columns = dataview.ColumnCount;
            //判断打印设置是否是横向打印
            if (!printDoc.DefaultPageSettings.Landscape)
            {
                PageWidth = printDoc.DefaultPageSettings.PaperSize.Width;
                PageHeight = printDoc.DefaultPageSettings.PaperSize.Height;
            }
            else
            {
                PageHeight = printDoc.DefaultPageSettings.PaperSize.Width;
                PageWidth = printDoc.DefaultPageSettings.PaperSize.Height;
            }
            LeftMargin = printDoc.DefaultPageSettings.Margins.Left;
            TopMargin = printDoc.DefaultPageSettings.Margins.Top;
            RightMargin = printDoc.DefaultPageSettings.Margins.Right;
            BottomMargin = printDoc.DefaultPageSettings.Margins.Bottom;
            height = PageHeight - TopMargin - BottomMargin - 2;
            width = PageWidth - LeftMargin - RightMargin - 2;
            double tempheight = height;
            double temprowHeight = rowHeight;
            while (true)
            {
                string temp = Convert.ToString(tempheight / Math.Round(temprowHeight, 3));
                int i = temp.IndexOf('.');
                double tt = 100;
                if (i != -1)
                {
                    tt = Math.Round(Convert.ToDouble(temp.Substring(temp.IndexOf('.'))), 3);
                }
                if (tt <= 0.01)
                {
                    rowHeight = Convert.ToInt32(temprowHeight);
                    break;
                }
                else
                {
                    temprowHeight = temprowHeight + 0.01;

                }
            }
            pageSize = height / rowHeight;
            if ((rows + 1) <= pageSize)
            {
                pageSize = rows + 1;
                PageNumber = 1;
            }
            else
            {
                PageNumber = rows / (pageSize - 1);
                if (rows % (pageSize - 1) != 0)
                {
                    PageNumber = PageNumber + 1;
                }
            }
        }
        /// <summary>
        /// 初始化打印
        /// </summary>
        private void InitPrint()
        {
            PageIndex = PageIndex + 1;
            if (PageIndex == PageNumber)
            {
                hasMorePage = false;
                if (PageIndex != 1)
                {
                    pageSize = rows % (pageSize - 1) + 1;
                }
            }
            else
            {
                hasMorePage = true;
            }
        }
        //打印头
        private void DrawHeader(Graphics g)
        {
            Font font = new Font("宋体", 12, FontStyle.Bold);
            int temptop = (rowHeight / 2) + TopMargin + 1;
            int templeft = LeftMargin + 1;
            for (int i = 0; i < this.columns; i++)
            {
                string headString = this.dataview.Columns[i].HeaderText;
                float fontHeight = g.MeasureString(headString, font).Height;
                float fontwidth = g.MeasureString(headString, font).Width;
                float temp = temptop - (fontHeight) / 3;
                g.DrawString(headString, font, Brushes.Black, new PointF(templeft, temp));
                templeft = templeft + (int)(this.dataview.Columns[i].Width / Rate) + 1;
            }
        }
        //画表格
        private void DrawTable(Graphics g)
        {
            Rectangle border = new Rectangle(LeftMargin, TopMargin, width, (pageSize) * rowHeight);
            g.DrawRectangle(new Pen(Brushes.Black, 2), border);
            for (int i = 1; i < pageSize; i++)
            {
                if (i != 1)
                {
                    g.DrawLine(new Pen(Brushes.Black, 1), new Point(LeftMargin + 1, (rowHeight * i) + TopMargin + 1), new Point(width + LeftMargin, (rowHeight * i) + TopMargin + 1));
                }
                else
                {
                    g.DrawLine(new Pen(Brushes.Black, 2), new Point(LeftMargin + 1, (rowHeight * i) + TopMargin + 1), new Point(width + LeftMargin, (rowHeight * i) + TopMargin + 1));
                }
            }
            //计算出列的总宽度和打印纸比率
            Rate = Convert.ToDouble(GetDateViewWidth()) / Convert.ToDouble(width);
            int tempLeft = LeftMargin + 1;
            int endY = (pageSize) * rowHeight + TopMargin;
            for (int i = 1; i < columns; i++)
            {
                tempLeft = tempLeft + 1 + (int)(this.dataview.Columns[i - 1].Width / Rate);
                g.DrawLine(new Pen(Brushes.Black, 1), new Point(tempLeft, TopMargin), new Point(tempLeft, endY));
            }
        }
        /**//// <summary>
        /// 获取打印的列的总宽度
        /// </summary>
        /// <returns></returns>
        private int GetDateViewWidth()
        {
            int total = 0;
            for (int i = 0; i < this.columns; i++)
            {
                total = total + this.dataview.Columns[i].Width;
            }
            return total;
        }
        //打印行数据
        private void DrawRows(Graphics g)
        {
            Font font = new Font("宋体", 12, FontStyle.Regular);
            int temptop = (rowHeight / 2) + TopMargin + 1 + rowHeight;
            for (int i = currRow; i < pageSize + currRow - 1; i++)
            {
                int templeft = LeftMargin + 1;
                for (int j = 0; j < columns; j++)
                {
                    string headString = this.dataview.Rows[i].Cells[j].Value.ToString();
                    float fontHeight = g.MeasureString(headString, font).Height;
                    float fontwidth = g.MeasureString(headString, font).Width;
                    float temp = temptop - (fontHeight) / 3;
                    while (true)
                    {
                        if (fontwidth <= (int)(this.dataview.Columns[j].Width / Rate))
                        {
                            break;
                        }
                        else
                        {
                            headString = headString.Substring(0, headString.Length - 1);
                            fontwidth = g.MeasureString(headString, font).Width;
                        }
                    }
                    g.DrawString(headString, font, Brushes.Black, new PointF(templeft, temp));
                    templeft = templeft + (int)(this.dataview.Columns[j].Width / Rate) + 1;
                }
                temptop = temptop + rowHeight;
            }
            currRow = pageSize + currRow - 1;
        }
        /// <summary>
        /// 在PrintDocument中的PrintPage方法中调用
        /// </summary>
        /// <param name="g">传入PrintPage中PrintPageEventArgs中的Graphics</param>
        /// <returns>是否还有打印页 有返回true，无则返回false</returns>
        public bool Print(Graphics g)
        {
            InitPrint();
            DrawTable(g);
            DrawHeader(g);
            DrawRows(g);
            //打印页码
            string pagestr = PageIndex + " / " + PageNumber;
            Font font = new Font("宋体", 12, FontStyle.Regular);
            g.DrawString(pagestr, font, Brushes.Black, new PointF((PageWidth / 2) - g.MeasureString(pagestr, font).Width, PageHeight - (BottomMargin / 2) - g.MeasureString(pagestr, font).Height));
            //打印查询的功能项名称
            string temp = dataview.Tag.ToString() + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            g.DrawString(temp, font, Brushes.Black, new PointF(PageWidth - 5 - g.MeasureString(temp, font).Width, PageHeight - 5 - g.MeasureString(temp, font).Height));
            return hasMorePage;
        }
        private void printContent(object sender, PrintPageEventArgs e, string StudentId)
        {
            int x, y, w, h, l, t; //坐标，宽、高、左、上
            x = e.MarginBounds.X;
            y = e.MarginBounds.Y;
            w = e.MarginBounds.Width;
            h = e.MarginBounds.Height;
            l = e.MarginBounds.Left;
            t = e.MarginBounds.Top;
            int rowSpan = Tools.IsNumeric(cbxRowSpan.Text); //行间距
            int xLeft = x + 20;      //左边距
            int xRight = 760 - 20;   //右边距
            int intRemark = 0; //该生有几科不正常成绩
            //获取学生基本信息
            StudentInfo si = new StudentInfo(StudentId);
            //设置Graphics实例
            Graphics g = e.Graphics;
            //画笔普通
            Pen pen = new Pen(Color.Black);
            //画笔重
            Pen penStrong = new Pen(Color.Black, 2);
            //字体(表头，即大标题)
            Font fontHead = new Font(new FontFamily("宋体"), 17, FontStyle.Bold);
            //字体(小标题，即姓名、班级、落款等)
            Font fontCaption = new Font(new FontFamily("宋体"), 12);
            //字体(小表头，即序号，课程名等)
            Font fontBodyTitle = new Font(new FontFamily("黑体"), 10);
            //字体(正文)
            Font fontBody = new Font(new FontFamily("宋体"), 10);
            //表头
            string strHead = "XX学院" + si.Jie.ToString() + "届毕业生成绩单";
            g.DrawString(strHead, fontHead, Brushes.Black, xLeft + (xRight - xLeft) / 2 - strHead.Length * fontHead.Height / 2 + 20, y);
            xLeft = xLeft - 30;
            y = y + Convert.ToInt32(fontHead.Height * 2.5);
            //姓名，专业班级，制表日期
            g.DrawString("姓名：" + si.StudentName, fontCaption, Brushes.Black, xLeft, y);
            //是否打印毕业证书号 ,同时考虑专业班级的位置对称
            if (chbPrintDiplomaNo.Checked)
            {
                g.DrawString("专业班级：" + si.Classroom, fontCaption, Brushes.Black, xLeft + 140, y);
                if (si.Classroom.Length < 11)
                {
                    g.DrawString("毕业证号：" + si.DiplomaNo, fontCaption, Brushes.Black, xLeft + 390, y);
                }
                else
                {
                    y = y + Convert.ToInt32(fontCaption.Height * 1.1);
                    g.DrawString("毕业证号：" + si.DiplomaNo, fontCaption, Brushes.Black, xLeft + 140, y);
                }
            }
            else
            {
                g.DrawString("专业班级：" + si.Classroom, fontCaption, Brushes.Black, xLeft + 330, y);
            }
            y = y + Convert.ToInt32(fontCaption.Height * 1.5);
            g.DrawLine(penStrong, xLeft, y, xRight, y); //加水平线
            int yStart = y;
            y = y + rowSpan;
            g.FillRectangle(Brushes.Gray, xLeft, y - 1, xRight - xLeft, fontBodyTitle.Height + 2);
            //左成绩表头
            g.DrawString("序号", fontBodyTitle, Brushes.Black, xLeft, y);
            g.DrawString("科目", fontBodyTitle, Brushes.Black, xLeft + 60, y);
            g.DrawString("成绩", fontBodyTitle, Brushes.Black, xLeft + 245, y);
            //右成绩表头
            g.DrawString("序号", fontBodyTitle, Brushes.Black, xLeft + (xRight - xLeft) / 2, y);
            g.DrawString("科目", fontBodyTitle, Brushes.Black, xLeft + (xRight - xLeft) / 2 + 60, y);
            g.DrawString("成绩", fontBodyTitle, Brushes.Black, xLeft + (xRight - xLeft) / 2 + 245, y);
            y = y + fontBodyTitle.Height + rowSpan;
            g.DrawLine(penStrong, xLeft, y, xRight, y);
            y = y + rowSpan;
            int intNo = 1; //课程序号
            for (int i = 0; i < si.TermCount; i++)
            {
                //g.FillRectangle(Brushes.LightGray, xLeft, y, xRight2, fontBody.Height);
                g.DrawString("第" + (i + 1).ToString() + "学期(" + si.arrTerm[i] + ")", fontBodyTitle, Brushes.Black, xLeft, y);
                y = y + fontBodyTitle.Height + rowSpan;
                g.DrawLine(pen, xLeft, y, xRight, y); //加水平线
                y = y + rowSpan;
                DataView dv = si.dtScore.DefaultView;
                dv.RowFilter = "Term='" + si.arrTerm[i] + "'";
                for (int j = 0; j < dv.Count; j++)
                {
                    //第一列成绩
                    if (dv[j]["Score"].ToString() == "" && dv[j]["Remark"].ToString() != "") //取非正常成绩
                    {
                        intRemark++;
                    }
                    g.DrawString((intNo++).ToString() + ".", fontBody, Brushes.Black, xLeft + 5, y);
                    g.DrawString(dv[j]["Course"].ToString() + "：", fontBody, Brushes.Black, xLeft + 25, y);
                    g.DrawString(dv[j]["Score"].ToString(), fontBody, Brushes.Black, xLeft + 250, y);
                    //第二列成绩
                    if (++j < dv.Count)
                    {
                        if (dv[j]["Score"].ToString() == "" && dv[j]["Remark"].ToString() != "")
                        {
                            intRemark++;
                        }
                        g.DrawString((intNo++).ToString() + ".", fontBody, Brushes.Black, xLeft + (xRight - xLeft) / 2 + 10, y);
                        g.DrawString(dv[j]["Course"].ToString(), fontBody, Brushes.Black, xLeft + (xRight - xLeft) / 2 + 10 + 25, y);
                        g.DrawString(dv[j]["Score"].ToString(), fontBody, Brushes.Black, xLeft + (xRight - xLeft) / 2 + 250, y);
                    }
                    y = y + fontBody.Height + rowSpan;
                }
                if (i < si.TermCount - 1)
                {
                    g.DrawLine(pen, xLeft, y, xRight, y); //加水平线
                }
                y = y + rowSpan;
            }
            g.DrawLine(penStrong, xLeft, y, xRight, y); //加水平线

            g.DrawLine(penStrong, xLeft, yStart, xLeft, y); //加左竖线
            g.DrawLine(pen, xLeft + (xRight - xLeft) / 2, yStart, xLeft + (xRight - xLeft) / 2, y); //加中竖线
            g.DrawLine(penStrong, xRight, yStart, xRight, y); //加右竖线

            //选择合适的打印页脚的位置
            if (y < (t + h)) //若剩余空行多，就在之间打印页脚
            {
                y = Convert.ToInt32(y + (t + h - y) * 0.5);
                if (intRemark > 0)
                {
                    g.FillRectangle(Brushes.Gray, xLeft + 20, y, fontCaption.Height * 2, fontCaption.Height);
                    g.DrawString(intRemark.ToString(), fontCaption, Brushes.Black, xLeft + 20, y);
                }
                g.DrawString("XX学院学生处", fontCaption, Brushes.Black, xLeft + 400, y);
                y = y + 20;
                g.DrawString("制表日期： " + System.DateTime.Now.ToShortDateString(), fontCaption, Brushes.Black, xLeft + 435, y);
            }
            else           //否则在页底打印页脚
            {
                if (intRemark > 0)
                {
                    g.FillRectangle(Brushes.Gray, xLeft + 20, y, fontCaption.Height * 2, fontCaption.Height);
                    g.DrawString(intRemark.ToString(), fontCaption, Brushes.Black, xLeft + 20, y);
                }
                g.DrawString("XX学院学生处", fontCaption, Brushes.Black, xLeft + 200, y + 5);
                g.DrawString("制表日期： " + System.DateTime.Now.ToShortDateString(), fontCaption, Brushes.Black, xLeft + 450, y + 5);
            }
        }
    }
}