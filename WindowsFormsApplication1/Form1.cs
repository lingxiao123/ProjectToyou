using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinfrmToYou.AppCode;
namespace WindowsFormsApplication1
{
    public partial class FormMain : Form
    {
        //public delegate void ThresholdReachedEventHandler(ThresholdReachedEventArgs e);
        public FormMain()
        {
            InitializeComponent();
        }
        private void CloseOtherCOm()
        {
            string[] str = SerialPort.GetPortNames();
            for (int i = 0; i < str.Length; i++)
            {
                SerialPort ser = new SerialPort();
                ser.PortName = str[i];
                ser.Close();
                ser.Dispose();
            }
        }
        private void BindDataSource()
        {
            try
            {
                //绑定序(批)号
                txtSNo.Text = GetNewSNo();
                txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                //绑定串口数据
                string[] str = SerialPort.GetPortNames();
                cboSerialName.DataSource = str;
                cboSerialName.SelectedIndex = cboSerialName.Items.Count > 1 ? 1 : 0;
                //绑定业务类型
                string sql4 = string.Empty;
                sql4 = sql4 + "select code,name from AA_BusiType where idrdStyleIn='21'";
                DataTable dt4 = DBAccess.QueryDataTable(sql4);
                if (dt4.Rows.Count > 0)
                {
                    ddlBusType.DataSource = dt4;
                    ddlBusType.DisplayMember = "name";
                    ddlBusType.ValueMember = "code";
                }
                //绑定存货基础数据
                string sql1 = string.Empty;
                sql1 = sql1 + "SELECT [code],[name],[specification] FROM [AA_Inventory]";
                DataTable dt1 = DBAccess.QueryDataTable(sql1);
                if (dt1.Rows.Count > 0)
                {
                    ddlInventory.DataSource = GetInventoryListFromDataTable(dt1);
                    ddlInventory.DisplayMember = "name";
                    ddlInventory.ValueMember = "code";
                }
                //绑定仓库信息
                string sql2 = string.Empty;
                sql2 = sql2 + "select code,name from AA_Warehouse";
                DataTable dt2 = DBAccess.QueryDataTable(sql2);
                if (dt2.Rows.Count > 0)
                {
                    ddlStore.DataSource = dt2;
                    ddlStore.DisplayMember = "name";
                    ddlStore.ValueMember = "code";
                }
                //绑定生产车间信息
                string sql3 = string.Empty;
                sql3 = sql3 + "select code,name from AA_Department";
                DataTable dt3 = DBAccess.QueryDataTable(sql3);
                if (dt3.Rows.Count > 0)
                {
                    ddlDep.DataSource = dt3;
                    ddlDep.DisplayMember = "name";
                    ddlDep.ValueMember = "code";
                }
                BindProductListDataSource();
            }
            catch (Exception ex)
            { }
        }
        private DataTable GetInventoryListFromDataTable(DataTable dt)
        {
            DataTable ht = new DataTable();
            ht.Columns.Add(new DataColumn("code"));
            ht.Columns.Add(new DataColumn("name"));
            foreach (DataRow rows in dt.Rows)
            {
                ht.Rows.Add(rows["code"], rows["name"] + "[规格型号：" + rows["specification"] + "]");
            }
            return ht;
        }
        /// <summary>
        /// 绑定暂存在数据库中的库存信息到列表
        /// </summary>
        private void BindProductListDataSource()
        {
            string sql = string.Empty;
            sql = "select * from ProductInfo where Pro_isInsert = '0' ";
            DataTable dt = DBAccessPrivate.QueryDataTable(sql);
            dgrProjectList.DataSource = dt;
            dgrProjectList.Columns[0].HeaderText = "数据库编号";
            dgrProjectList.Columns[1].HeaderText = "产品名称";
            dgrProjectList.Columns[2].HeaderText = "产品编码";
            dgrProjectList.Columns[3].HeaderText = "仓库编码";
            dgrProjectList.Columns[4].HeaderText = "计量单位";
            dgrProjectList.Columns[5].HeaderText = "重量";
            dgrProjectList.Columns[6].HeaderText = "生产车间";
            dgrProjectList.Columns[7].HeaderText = "生产日期";
            dgrProjectList.Columns[8].HeaderText = "业务类型";
            dgrProjectList.Columns[9].HeaderText = "序(批)号";
            dgrProjectList.Columns[10].HeaderText = "是否已导入T+";
        }
        private void GetSerPort()
        {
            try
            {
                if (SerPort.IsOpen)
                {
                    SerPort.Dispose();
                    SerPort.Close();
                }
                SerPort.PortName = cboSerialName.SelectedValue.ToString();
                SerPort.BaudRate = 9600;
                SerPort.Parity = Parity.None;
                SerPort.StopBits = StopBits.One;
                SerPort.DataBits = 8;
                SerPort.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                SerPort.ReceivedBytesThreshold = 1;
                if (SerPort.IsOpen)
                {
                    SerPort.Dispose();
                    SerPort.Close();
                    SerPort.Open();
                }
                else
                {
                    SerPort.Open();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            // (sender, e);
            // (object sender, System.IO.Ports.SerialDataReceivedEventArgs e); 
            //new serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e);
        }

        #region 获取部分T+数据方法

        /// <summary>
        /// 将获取到的电子秤数据字符串进行再处理
        /// 电子秤获取到的值是倒过来的，需要把它调换顺序
        /// 如：电子秤实际显示23.06公斤，但获取到的数据为：60.3200。
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        private string getRightStr(string temp)
        {
            string model = string.Empty;
            for (int i = temp.Length - 1; i < temp.Length; i--)
            {
                if (i > -1)
                {
                    model = model + temp.Substring(i, 1);
                }
                if (i < 0)
                {
                    break;
                }
            }
            //txtLog.Text = txtLog.Text + "\n" + "数据处理：重新整理完成！";
            return model;
        }
        /// <summary>
        /// 获取登录名的用户名
        /// </summary>
        /// <param name="UserCode"></param>
        /// <returns></returns>
        private string GetUserName(string UserCode)
        {
            try
            {
                string sql = "select PersonName from EAP_User where name='" + UserCode + "'";
                string str = DBAccess.QueryValue(sql).ToString();
                return str;
            }
            catch
            {
                return string.Empty;
            }
        }

        private string GetST_RDRecordID_By_Code(string code)
        {
            string sql = "select id from ST_RDRecord where code in ('" + code + "')";
            string str = DBAccess.QueryValue(sql).ToString();
            return str;
        }
        /// <summary>
        /// 获取新的入库单条目ID
        /// </summary>
        /// <returns></returns>
        private string GetNewST_RDRecordID()
        {
            string sql = string.Empty;
            sql = sql + "select top 1 id from ST_RDRecord where code like '%" + DateTime.Now.Year + "%'";
            sql = sql + " and code like '%" + DateTime.Now.Month + "%' order by code desc ";
            string str = DBAccess.QueryValue(sql).ToString();
            return str;
        }



        /// <summary>
        /// 获取计量单位ID
        /// </summary>
        /// <param name="InventoryCode"></param>
        /// <returns></returns>
        private string GetUintID(string InventoryCode)
        {
            string sql = "select idunit from AA_Inventory where code='" + InventoryCode + "'";
            string str = DBAccess.QueryValue(sql).ToString();
            return str;
        }



        /// <summary>
        /// 获取存货的规格型号
        /// </summary>
        /// <param name="InventoryCode"></param>
        /// <returns></returns>
        private string GetProductType(string InventoryCode)
        {
            string sql = "select specification from AA_Inventory where code='" + InventoryCode + "'";
            string str = DBAccess.QueryValue(sql).ToString();
            return str;
        }



        /// <summary>
        /// 获取存货ID
        /// </summary>
        /// <param name="InventoryCode"></param>
        /// <returns></returns>
        private string GetInventoryID(string InventoryCode)
        {
            string sql = "select ID from AA_Inventory where code='" + InventoryCode + "'";
            string str = DBAccess.QueryValue(sql).ToString();
            return str;
        }

        /// <summary>
        /// 获取存货Name
        /// </summary>
        /// <param name="InventoryCode"></param>
        /// <returns></returns>
        private string GetInventoryName(string InventoryCode)
        {
            string sql = "select name from AA_Inventory where code='" + InventoryCode + "'";
            string str = DBAccess.QueryValue(sql).ToString();
            return str;
        }

        /// <summary>
        /// 获取ST_RDRecord的新Code（同T+页面中的单据编号）
        /// </summary>
        /// <returns></returns>
        private string GetST_RDRecord_Code()
        {
            string sql = string.Empty;
            sql = sql + "select top 1 code from ST_RDRecord where code like '%" + DateTime.Now.ToString("yyyy-MM") + "%' order by code desc ";
            object ob = DBAccess.QueryValue(sql);
            string str = string.Empty;
            if (ob == null)
                str = "00000";
            else
                str = ob.ToString();
            str = string.IsNullOrEmpty(str) ? "00000" : str.Substring(str.Length - 4);
            int i = Convert.ToInt32(str);
            i++;
            return "MC-" + DateTime.Now.ToString("yyyy-MM") + "-" + i.ToString("0000");
        }
        private string GetNewST_RDRecord_B_Code()
        {
            string sql = "select count(*) ";
            return string.Empty;
        }


        /// <summary>
        /// 获取制单人ID
        /// </summary>
        /// <returns></returns>
        private string GetUserID(string UserName)
        {
            try
            {
                string sql = "select UserID from EAP_User where Name='" + UserName + "'";
                string str = DBAccessSystem.QueryValue(sql).ToString();
                return str;
            }
            catch
            {
                return string.Empty;
            }
        }



        /// <summary>
        /// 获取部门ID
        /// </summary>
        /// <param name="DepCode"></param>
        /// <returns></returns>
        private string getDepID(string DepCode)
        {
            string sql = "select ID from AA_Department where code='" + DepCode + "'";
            string str = DBAccess.QueryValue(sql).ToString();
            return str;
        }



        /// <summary>
        /// 获取仓库ID
        /// </summary>
        /// <param name="StoreCode"></param>
        /// <returns></returns>
        private string getStoreID(string StoreCode)
        {
            string sql = "select ID from AA_Warehouse where code='" + StoreCode + "'";
            string str = DBAccess.QueryValue(sql).ToString();
            return str;
        }



        /// <summary>
        /// 获取序号ID
        /// </summary>
        /// <returns></returns>
        private string GetNewSNo()
        {
            string sql = "select COUNT(*) from ProductInfo where Pro_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            string str = DBAccessPrivate.QueryValue(sql).ToString();
            string temp = DateTime.Now.ToString("yyMMdd") + "-" + (Convert.ToInt32(str) + 1).ToString("000");
            //Session["PSno"] = temp;
            return temp;
        }
        #endregion
        /// <summary>
        /// 检查必填项
        /// </summary>
        /// <returns></returns>
        private bool CheckDate()
        {
            if (txtSNo.Text == string.Empty)
            {
                //txtLog.Text = txtLog.Text + "\n" + "必填项检查结果：'" + txtSNo.Label + "'未填写！";
                return false;
            }
            if (txtUserID.Text == string.Empty)
            {
                //txtLog.Text = txtLog.Text + "\n" + "必填项检查结果：'" + txtUserID.Label + "'未填写！";
                return false;
            }
            //txtLog.Text = txtLog.Text + "\n" + "必填项检查结果：成功！";
            return true;
        }
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs serialDataReceivedEventArgs)
        {
            try
            {
                SerialPort sp = (SerialPort)sender;
                string s = "";
                int count = sp.BytesToRead;
                byte[] data = new byte[count];
                sp.Read(data, 0, count);
                foreach (byte item in data)
                {
                    s += Convert.ToChar(item);
                }
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        this.lblWeight.Text = Convert.ToDecimal(getRightStr(s.Substring(s.LastIndexOf("=") + 1))).ToString("00.00");
                        //this.txtJWeight.Text = (Convert.ToDecimal(lblWeight.Text.Equals(string.Empty) ? "0" : lblWeight.Text) - Convert.ToDecimal(txtTWeight.Text.Equals(string.Empty) ? "0" : txtTWeight.Text)).ToString();
                    }));
                }
                else
                {
                    this.lblWeight.Text = s;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void FormMain_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“privateDataBaseDataSet.ProductInfo”中。您可以根据需要移动或删除它。
            this.productInfoTableAdapter.Fill(this.privateDataBaseDataSet.ProductInfo);
            BindDataSource();
        }

        private void btnOpenPort_Click(object sender, EventArgs e)
        {
            GetSerPort();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SerPort.Dispose();
            SerPort.Close();
        }

        private void btnClosePort_Click(object sender, EventArgs e)
        {
            SerPort.Dispose();
            SerPort.Close();
        }

        private void ddlInventory_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable table1 = this.ddlInventory.DataSource as DataTable;

            txtName.Text = table1.Rows[this.ddlInventory.SelectedIndex]["name"].ToString();

            txtSpecification.Text = GetProductType(table1.Rows[this.ddlInventory.SelectedIndex]["code"].ToString());
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //实例化打印对象
            PrintDocument printDocument1 = new PrintDocument();
            //设置打印用的纸张,当设置为Custom的时候，可以自定义纸张的大小
            printDocument1.DefaultPageSettings.PaperSize = new PaperSize("Custum", 500, 500);
            //注册PrintPage事件，打印每一页时会触发该事件
            printDocument1.PrintPage += new PrintPageEventHandler(this.PrintDocument_PrintPage);
            try
            {
                printDocument1.Print();//开始打印
                Save();
                //txtLog.Text = txtLog.Text + "\n" + "打印结果：打印成功！";
                //PrintPage();
            }
            catch (Exception ex)
            {
                //txtLog.Text = txtLog.Text + "\n" + "打印出现错误：" + ex.Message;
            }
            finally
            {
                printDocument1.Dispose();
            }
        }
        /// <summary>
        /// 设置打印内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            string inventoryStr = string.Empty;
            DataTable table1 = this.ddlInventory.DataSource as DataTable;
            inventoryStr = table1.Rows[this.ddlInventory.SelectedIndex]["code"].ToString();
            string inventoryName = GetInventoryName(inventoryStr);
            decimal jW = GetJweight();
            int FontSize = Convert.ToInt32(ConfigurationManager.AppSettings["PrintFontSize"].ToString());
            //设置打印内容及其字体，颜色和位置
            e.Graphics.DrawString(inventoryName, new Font(new FontFamily("黑体"), FontSize), System.Drawing.Brushes.Red, Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow1X"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow1Y"].ToString()));
            e.Graphics.DrawString(txtSpecification.Text, new Font(new FontFamily("黑体"), FontSize), System.Drawing.Brushes.Red, Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow2X"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow2Y"].ToString()));
            e.Graphics.DrawString(lblWeight.Text, new Font(new FontFamily("黑体"), FontSize), System.Drawing.Brushes.Red, Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow3X"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow3Y"].ToString()));
            e.Graphics.DrawString(txtTWeight.Text, new Font(new FontFamily("黑体"), FontSize), System.Drawing.Brushes.Red, Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow4X"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow4Y"].ToString()));
            e.Graphics.DrawString(jW.ToString(), new Font(new FontFamily("黑体"), FontSize), System.Drawing.Brushes.Red, Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow5X"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow5Y"].ToString()));
            e.Graphics.DrawString(txtDate.Text, new Font(new FontFamily("黑体"), FontSize), System.Drawing.Brushes.Red, Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow6X"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow6Y"].ToString()));
            e.Graphics.DrawString(GetNewSNo(), new Font(new FontFamily("黑体"), FontSize), System.Drawing.Brushes.Red, Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow7X"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow7Y"].ToString()));
            //txtLog.Text = txtLog.Text + "\n" + "打印内容设置结果：已完成！";
        }
        private void Save()
        {
            if (!CheckDate())
            {
                return;
            }
            try
            {
                string inventoryStr = string.Empty;
                DataTable table1 = this.ddlInventory.DataSource as DataTable;
                inventoryStr = table1.Rows[this.ddlInventory.SelectedIndex]["code"].ToString();
                string storeStr = string.Empty;
                DataTable table2 = this.ddlStore.DataSource as DataTable;
                storeStr = table2.Rows[this.ddlStore.SelectedIndex]["code"].ToString();
                string depStr = string.Empty;
                DataTable table3 = this.ddlDep.DataSource as DataTable;
                depStr = table3.Rows[this.ddlDep.SelectedIndex]["code"].ToString();
                string bustypeStr = string.Empty;
                DataTable table4 = this.ddlBusType.DataSource as DataTable;
                bustypeStr = table4.Rows[this.ddlBusType.SelectedIndex]["code"].ToString();
                decimal jW = GetJweight();
                string sql = "insert into ProductInfo values('" + txtName.Text + "','" + inventoryStr + "','" + storeStr + "','" + "KG" + "','" + jW.ToString() + "','" + depStr + "','" + DateTime.Now.ToString("yyyy/MM/dd") + "','" + bustypeStr + "','" + txtSNo.Text + "','0')";
                DBAccessPrivate.ExecSql(sql);
                //txtLog.Text = txtLog.Text + "\n" + "数据暂存结果：成功！";
            }
            catch
            {
                //txtLog.Text = txtLog.Text + "\n" + "数据暂存结果：失败，请检查数据类型！";
            }
            finally
            {
                BindProductListDataSource();
                //ReleaseControl();
                txtSNo.Text = GetNewSNo();
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!CheckDate())
            {
                return;
            }
            try
            {
                string inventoryStr = string.Empty;
                DataTable table1 = this.ddlInventory.DataSource as DataTable;
                inventoryStr = table1.Rows[this.ddlInventory.SelectedIndex]["code"].ToString();
                string storeStr = string.Empty;
                DataTable table2 = this.ddlStore.DataSource as DataTable;
                storeStr = table2.Rows[this.ddlStore.SelectedIndex]["code"].ToString();
                string depStr = string.Empty;
                DataTable table3 = this.ddlDep.DataSource as DataTable;
                depStr = table3.Rows[this.ddlDep.SelectedIndex]["code"].ToString();
                string bustypeStr = string.Empty;
                DataTable table4 = this.ddlBusType.DataSource as DataTable;
                bustypeStr = table4.Rows[this.ddlBusType.SelectedIndex]["code"].ToString();
                decimal jW = GetJweight();
                string sql = "insert into ProductInfo values('" + txtName.Text + "','" + inventoryStr + "','" + storeStr + "','" + "KG" + "','" + jW.ToString() + "','" + depStr + "','" + DateTime.Now.ToString("yyyy/MM/dd") + "','" + bustypeStr + "','" + txtSNo.Text + "','0')";
                DBAccessPrivate.ExecSql(sql);
                //txtLog.Text = txtLog.Text + "\n" + "数据暂存结果：成功！";
            }
            catch
            {
                //txtLog.Text = txtLog.Text + "\n" + "数据暂存结果：失败，请检查数据类型！";
            }
            finally
            {
                BindProductListDataSource();
                //ReleaseControl();
                txtSNo.Text = GetNewSNo();
            }
        }
        private decimal GetJweight()
        {
            decimal model = 0;
            decimal tW = 0;
            decimal zW = 0;
            try
            {
                tW = Convert.ToDecimal(string.IsNullOrEmpty(txtTWeight.Text) ? "0" : txtTWeight.Text);
            }
            catch (Exception ex)
            {
                tW = 0;
            }
            try
            {
                zW = Convert.ToDecimal(string.IsNullOrEmpty(lblWeight.Text) ? "0" : lblWeight.Text);
            }
            catch (Exception ex)
            {
                zW = 0;
            }
            model = zW - tW;
            return model;
        }
        /// <summary>
        /// 导入T+
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgrProjectList.Rows.Count > 0)
                {
                    string inventoryStr = string.Empty;
                    DataTable table1 = this.ddlInventory.DataSource as DataTable;
                    inventoryStr = table1.Rows[this.ddlInventory.SelectedIndex]["code"].ToString();
                    string storeStr = string.Empty;
                    DataTable table2 = this.ddlStore.DataSource as DataTable;
                    storeStr = table2.Rows[this.ddlStore.SelectedIndex]["code"].ToString();
                    string depStr = string.Empty;
                    DataTable table3 = this.ddlDep.DataSource as DataTable;
                    depStr = table3.Rows[this.ddlDep.SelectedIndex]["code"].ToString();
                    string bustypeStr = string.Empty;
                    DataTable table4 = this.ddlBusType.DataSource as DataTable;
                    bustypeStr = table4.Rows[this.ddlBusType.SelectedIndex]["code"].ToString();
                    string ST_RDRecord_Code = GetST_RDRecord_Code();
                    //新增一个main入库单
                    string MainSql = string.Empty;
                    MainSql = MainSql + "insert into ST_RDRecord(code,printTime,amount,rdDirectionFlag,isCostAccount,";
                    MainSql = MainSql + "isMergedFlow,isAutoGenerate,iscarriedforwardin,iscarriedforwardout,ismodifiedcode,";
                    MainSql = MainSql + "accountingperiod,accountingyear,updatedBy,VoucherYear,VoucherPeriod,PrintCount,";
                    MainSql = MainSql + "idbusitype,iddepartment,IdMarketingOrgan,idrdstyle,idwarehouse,accountState,voucherState,";
                    MainSql = MainSql + "makerid,idvouchertype,voucherdate,madedate,createdtime,updated,maker)";
                    MainSql = MainSql + " values('" + ST_RDRecord_Code + "','0', '0', '1', '0',";
                    MainSql = MainSql + "'0', '0', '0', '0', '0',";
                    MainSql = MainSql + "'" + DateTime.Now.Month + "', '" + DateTime.Now.Year + "', '" + txtUserID.Text + "', '" + DateTime.Now.Year + "', '" + DateTime.Now.Month + "', '0',";
                    MainSql = MainSql + "'3', '" + getDepID(depStr) + "', '1', '21', '" + getStoreID(storeStr) + "', '338', '181',";
                    MainSql = MainSql + "'" + GetUserID(txtUserID.Text) + "', '15', '" + DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "', ";
                    MainSql = MainSql + "'" + DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "', '" + DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "', ";
                    MainSql = MainSql + "'" + DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "','" + GetUserName(txtUserID.Text) + "')";
                    if (DBAccess.ExecSql(MainSql) != -1)
                    {
                        string idRDRecordDTO = GetNewST_RDRecordID();
                        string sql = string.Empty;
                        sql = "select * from ProductInfo where Pro_isInsert not in ('1') ";
                        DataTable dt = DBAccessPrivate.QueryDataTable(sql);
                        int i = 0;
                        foreach (DataRow trow in dt.Rows)
                        {
                            string ChildSql = string.Empty;
                            ChildSql = ChildSql + "insert into ST_RDRecord_b(code,quantity, compositionQuantity, baseQuantity,";
                            ChildSql = ChildSql + "price, basePrice, amount, batch, isManualCost, isCostAccounted, taxFlag,SourceVoucherCodeByMergedFlow,SourceVoucherCode,";
                            ChildSql = ChildSql + "updatedBy, IsPresent, IsPromotionPresent, idbusiTypeByMergedFlow, idinventory,";
                            ChildSql = ChildSql + "idbaseunit, idunit, idwarehouse, idRDRecordDTO, createdtime, updated)";
                            ChildSql = ChildSql + "values('"+i.ToString("0000")+"','" + trow["Pro_count"].ToString() + "', '" + trow["Pro_count"].ToString() + trow["Pro_unit"].ToString() + "', '" + trow["Pro_count"].ToString() + "', ";
                            ChildSql = ChildSql + "'0', '0', '0', '" + trow["Pro_sno"].ToString() + "', '0', '0', '0', '','',";
                            ChildSql = ChildSql + "'" + txtUserID.Text + "', '0', '0', '3', '" + GetInventoryID(trow["Pro_code"].ToString()) + "', ";
                            ChildSql = ChildSql + "'" + GetUintID(trow["Pro_code"].ToString()) + "', '" + GetUintID(trow["Pro_code"].ToString()) + "', '" + getStoreID(trow["Pro_store"].ToString()) + "', '" + idRDRecordDTO + "', '" + DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "', '" + DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "')";
                            if (DBAccess.ExecSql(ChildSql) != -1)
                            {
                                string StockSql = string.Empty;
                                StockSql = StockSql + "insert into ST_CurrentStock";
                                StockSql = StockSql + "(";
                                StockSql = StockSql + "batch,";
                                StockSql = StockSql + "productForReceiveBaseQuantity,";
                                StockSql = StockSql + "isCarriedForwardOut,";
                                StockSql = StockSql + "isCarriedForwardIn,";
                                StockSql = StockSql + "updatedBy,";
                                StockSql = StockSql + "idinventory,";
                                StockSql = StockSql + "IdMarketingOrgan,";
                                StockSql = StockSql + "idbaseunit,";
                                StockSql = StockSql + "idwarehouse,";
                                StockSql = StockSql + "createdtime,";
                                StockSql = StockSql + "updated";
                                StockSql = StockSql + ")";
                                StockSql = StockSql + "values";
                                StockSql = StockSql + "(";
                                StockSql = StockSql + " '" + trow["Pro_sno"].ToString() + "',";
                                StockSql = StockSql + " '" + trow["Pro_count"].ToString() + "',";
                                StockSql = StockSql + "0,";
                                StockSql = StockSql + "0,";
                                StockSql = StockSql + " '" + txtUserID.Text + "',";
                                StockSql = StockSql + " '" + GetInventoryID(trow["Pro_code"].ToString()) + "',";
                                StockSql = StockSql + " 1,";
                                StockSql = StockSql + " 1,";
                                StockSql = StockSql + " '" + getStoreID(trow["Pro_store"].ToString()) + "',";
                                StockSql = StockSql + " '" + DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "',";
                                StockSql = StockSql + "'" + DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "'";
                                StockSql = StockSql + ")";
                                DBAccess.ExecSql(StockSql);
                                string MpSQL = "update ProductInfo set Pro_isInsert=1 where id=" + trow["id"];
                                DBAccessPrivate.ExecSql(MpSQL);
                            }
                            i++;
                        }
                    }
                }
                //txtLog.Text = txtLog.Text + "\n" + "T+导入结果：已完成！";
            }
            catch (Exception ew)
            {
                //txtLog.Text = txtLog.Text + "\n" + "T+导入结果：失败！" + ew.Message + ew.TargetSite.Name;
            }
            finally
            {
                BindProductListDataSource();
            }
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            SerPort.Dispose();
            SerPort.Close();
        }
    }
}
