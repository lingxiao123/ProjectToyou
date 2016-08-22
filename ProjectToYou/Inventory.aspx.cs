using System;
using System.Linq;
using System.Data;
using ProjectToYou.Code;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Web.Providers.Entities;
using System.Drawing.Printing;
using System.Drawing;
using System.Windows.Forms;
using FineUI;
using System.Configuration;
namespace ProjectToYou
{
    public partial class _Inventory : PageBase
    {
        SerialPort serialPort1 = new SerialPort();
        Session WeightValue = new Session();
        System.IO.Stream ste;
        Session PName = new Session();//打印名称
        Session PSize = new Session();//打印规格
        Session PGW = new Session();//打印总重
        Session PBW = new Session();//打印桶（箱）重
        Session PNW = new Session();//打印净重
        Session PDate = new Session();//打印生产日期
        Session PSno = new Session();//打印序号
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSessionValue();
                //绑定基础数据
                BindBaseDataSource();
                //绑定页面控件是否可用
                BindControlEnable();
            }
        }



        /// <summary>
        /// 绑定页面控件可用性
        /// </summary>
        private void BindControlEnable()
        {
            if (ddlBusType.SelectedValue == "03")
            {
                ddlInventory.Enabled = true;
            }
        }
        /// <summary>
        /// 绑定部分Session初始值，主要用于打印
        /// </summary>
        private void BindSessionValue()
        {
            Session["PName"] = "0";
            Session["PSize"] = "0";
            Session["PGW"] = "0";
            Session["PBW"] = "0";
            Session["PNW"] = "0";
            Session["PDate"] = "0";
            Session["PSno"] = "0";
            txtSNo.Text = GetNewSNo();
        }

        /// <summary>
        /// 绑定基础数据（通过访问T+数据库基础数据表）
        /// </summary>
        private void BindBaseDataSource()
        {
            txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            Session["PDate"] = txtDate.Text;
            //绑定串口数据
            string[] str = SerialPort.GetPortNames();
            ddlSerialName.DataSource = str;
            ddlSerialName.DataBind();
            //绑定业务类型
            string sql4 = string.Empty;
            sql4 = sql4 + "select code,name from AA_BusiType where idrdStyleIn='21'";
            DataTable dt4 = DBAccess.QueryDataTable(sql4);
            if (dt4.Rows.Count > 0)
            {
                foreach (DataRow row in dt4.Rows)
                {
                    ddlBusType.Items.Add(row["name"].ToString(), row["code"].ToString());
                }
            }
            //绑定存货基础数据
            string sql1 = string.Empty;
            sql1 = sql1 + "SELECT [code],[name],[shorthand],[isBatch] ,[isQualityPeriod]  ,[isSale],[isMadeSelf],[isPurchase] ,[isMaterial],[disabled],[isMadeRequest],[isSingleUnit],[ts],[updatedBy]";
            sql1 = sql1 + " ,[Userfreeitem7],[Userfreeitem6],[Userfreeitem2],[Userfreeitem1] ,[Userfreeitem9],[Userfreeitem0],[Userfreeitem8] ,[Userfreeitem5],[Userfreeitem4],[Userfreeitem3],[MustInputFreeitem7],[MustInputFreeitem2],[MustInputFreeitem6],[MustInputFreeitem3]";
            sql1 = sql1 + ",[MustInputFreeitem5],[MustInputFreeitem4],[MustInputFreeitem9],[MustInputFreeitem1],[MustInputFreeitem8],[MustInputFreeitem0],[IsLaborCost],[IsNew],[MadeRecordDate] ,[IsSuite]";
            sql1 = sql1 + " ,[id],[idinventoryclass] ,[idMarketingOrgan],[idunit] ,[idunitbymanufacture],[idUnitByPurchase],[idUnitByRetail],[idUnitBySale],[idUnitByStock],[idunitgroup]";
            sql1 = sql1 + ",[idSubUnitByReport],[taxRate],[unittype] ,[valueType],[madeDate],[updated],[createdTime],[Creater] FROM [AA_Inventory]";
            DataTable dt1 = DBAccess.QueryDataTable(sql1);
            if (dt1.Rows.Count > 0)
            {
                ddlInventory.Items.Add("", "Code001");
                foreach (DataRow row in dt1.Rows)
                {
                    ddlInventory.Items.Add(row["name"].ToString(), row["code"].ToString());
                }
            }
            //绑定仓库信息
            string sql2 = string.Empty;
            sql2 = sql2 + "select code,name from AA_Warehouse";
            DataTable dt2 = DBAccess.QueryDataTable(sql2);
            if (dt2.Rows.Count > 0)
            {
                foreach (DataRow row in dt2.Rows)
                {
                    ddlStore.Items.Add(row["name"].ToString(), row["code"].ToString());
                }
            }
            //绑定生产车间信息
            string sql3 = string.Empty;
            sql3 = sql3 + "select code,name from AA_Department";
            DataTable dt3 = DBAccess.QueryDataTable(sql3);
            if (dt3.Rows.Count > 0)
            {
                foreach (DataRow row in dt3.Rows)
                {
                    ddlDep.Items.Add(row["name"].ToString(), row["code"].ToString());
                }
            }
            BindProductListDataSource();
        }


        /// <summary>
        /// 绑定暂存在数据库中的库存信息到列表
        /// </summary>
        private void BindProductListDataSource()
        {
            string sql = string.Empty;
            sql = "select * from ProductInfo where Pro_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and Pro_isInsert = '0' ";
            DataTable dt = DBAccessPrivate.QueryDataTable(sql);
            grdProductList.DataSource = dt;
            grdProductList.DataBind();
        }



        /// <summary>
        /// 通过接口获取电子秤数据
        /// </summary>
        private string GetWeightValue()
        {
            string aa = string.Empty;
            try
            {
                serialPort1.PortName = ddlSerialName.SelectedItem.Value;
                serialPort1.BaudRate = 9600;
                serialPort1.Parity = 0;
                serialPort1.DataBits = 8;
                serialPort1.DataReceived += (object sender, System.IO.Ports.SerialDataReceivedEventArgs e) =>
                {
                    aa = serialPort1_DataReceived();
                };
                if (serialPort1.IsOpen == false)
                {
                    serialPort1.Open();
                    int bytes = serialPort1.BytesToRead;
                    byte[] buffer = new byte[bytes];
                    serialPort1.Read(buffer, 0, bytes);
                }
                Session["WeightValue"] = aa;
                Session["PGW"] = aa;
            }
            catch (Exception ex)
            {
                Alert.ShowInTop("请检查串口是否正确。");
            }
            finally { }
            return aa;
        }



        /// <summary>
        /// 设置串口读取数据的方法
        /// </summary>
        /// <returns></returns>
        private string serialPort1_DataReceived()
        {
            string rightInfo = string.Empty;
            try
            {
                Thread.Sleep(100);
                int bytes = serialPort1.BytesToRead;
                byte[] buffer = new byte[bytes];
                serialPort1.Read(buffer, 0, bytes);
                txtWeight.Text = serialPort1.ReadLine();
                string strbuffer = Encoding.ASCII.GetString(buffer);
                string romovestartChar = strbuffer.Substring(1);
                string[] tempInfo = romovestartChar.Split('=');
                if (GetWeightBool(tempInfo))
                {
                    rightInfo = getRightStr(tempInfo[tempInfo.Count() - 1]);
                    //if (this.InvokeRequired)
                    //{
                    //    this.Invoke(new MethodInvoker(delegate { this.lblWeight.Text = Convert.ToDecimal(rightInfo).ToString("F2"); }));
                    //}
                    Session["WeightValue"] = Convert.ToDecimal(rightInfo).ToString("00.00");
                    Session["PGW"] = rightInfo;
                }
            }
            catch (Exception ex)
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Close();
                }
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Close();
                }
            }
            txtLog.Text = txtLog.Text + "\n" + "打开'" + serialPort1.PortName + "'成功！";
            return Convert.ToDecimal(rightInfo.Equals(string.Empty) ? "0" : rightInfo).ToString();
        }



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
            txtLog.Text = txtLog.Text + "\n" + "数据处理：重新整理完成！";
            return model;
        }



        /// <summary>
        /// 检查从电子秤串口中获取到的数据是否完整。
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        private bool GetWeightBool(string[] temp)
        {
            bool bl = true;
            for (int i = 0; i < temp.Count(); i++)
            {
                if (i != 0)
                {
                    if (!temp[i].Equals(temp[i - 1]))
                    {
                        bl = false;
                        break;
                    }
                }
            }
            if (bl)
            {
                txtLog.Text = txtLog.Text + "\n" + "数据检查结果：完整！";
            }
            else
            {
                txtLog.Text = txtLog.Text + "\n" + "数据检查结果：缺省！请重新打开串口！";
            }
            return bl;
        }



        /// <summary>
        /// 获取串口传输过来的数据
        /// 该串口读取数据是异步进行，对异步的方法不太了解。
        /// 采取的是将异步方法获取到的数据暂存在Session中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGetWeight_Click(object sender, EventArgs e)
        {
            txtWeight.Text = GetWeightValue();
        }



        /// <summary>
        /// 选择存货时,将存货的信息赋值到文本框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlInventory_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtName.Text = ddlInventory.SelectedText;
            txtSpecification.Text = GetProductType(ddlInventory.SelectedValue);
            Session["PName"] = txtName.Text;
            Session["PSize"] = txtSpecification.Text;
            txtLog.Text = txtLog.Text + "\n" + "存货信息检查结果：成功！";
        }



        /// <summary>
        /// 将获取到的数据暂存到数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddInfo_Click(object sender, EventArgs e)
        {
            if (!CheckDate())
            {
                return;
            }
            try
            {
                string sql = "insert into ProductInfo values('" + txtName.Text + "','" + ddlInventory.SelectedValue + "','" + ddlStore.SelectedValue + "','" + "KG" + "','" + txtJWeight.Text + "','" + ddlDep.SelectedValue + "','" + DateTime.Now.ToString("yyyy/MM/dd") + "','" + ddlBusType.SelectedValue + "','" + txtSNo.Text + "','0')";
                DBAccessPrivate.ExecSql(sql);
                txtLog.Text = txtLog.Text + "\n" + "数据暂存结果：成功！";
            }
            catch
            {
                txtLog.Text = txtLog.Text + "\n" + "数据暂存结果：失败，请检查数据类型！";
            }
            finally
            {
                BindProductListDataSource();
                ReleaseControl();
                txtSNo.Text = GetNewSNo();
            }
        }



        /// <summary>
        /// 导入T+
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdProductList.Rows.Count > 0)
                {
                    //新增一个main入库单
                    string MainSql = string.Empty;
                    MainSql = MainSql + "insert into ST_RDRecord(code,printTime,amount,rdDirectionFlag,isCostAccount,";
                    MainSql = MainSql + "isMergedFlow,isAutoGenerate,iscarriedforwardin,iscarriedforwardout,ismodifiedcode,";
                    MainSql = MainSql + "accountingperiod,accountingyear,updatedBy,VoucherYear,VoucherPeriod,PrintCount,";
                    MainSql = MainSql + "idbusitype,iddepartment,IdMarketingOrgan,idrdstyle,idwarehouse,accountState,voucherState,";
                    MainSql = MainSql + "makerid,idvouchertype,voucherdate,madedate,createdtime,updated,maker)";
                    MainSql = MainSql + " values('" + GetST_RDRecord_Code() + "','0', '0', '1', '0',";
                    MainSql = MainSql + "'0', '0', '0', '0', '0',";
                    MainSql = MainSql + "'" + DateTime.Now.Month + "', '" + DateTime.Now.Year + "', '" + txtUserID.Text + "', '" + DateTime.Now.Year + "', '" + DateTime.Now.Month + "', '0',";
                    MainSql = MainSql + "'3', '" + getDepID(ddlDep.SelectedValue) + "', '1', '21', '" + getStoreID(ddlStore.SelectedValue) + "', '338', '181',";
                    MainSql = MainSql + "'" + GetUserID(txtUserID.Text) + "', '15', '" + DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "', ";
                    MainSql = MainSql + "'" + DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "', '" + DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "', ";
                    MainSql = MainSql + "'" + DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "','" + GetUserName(txtUserID.Text) + "')";
                    if (DBAccess.ExecSql(MainSql) != -1)
                    {
                        string idRDRecordDTO = GetNewST_RDRecordID();
                        string sql = string.Empty;
                        sql = "select * from ProductInfo where Pro_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and Pro_isInsert not in ('1') ";
                        DataTable dt = DBAccessPrivate.QueryDataTable(sql);
                        foreach (DataRow trow in dt.Rows)
                        {
                            string ChildSql = string.Empty;
                            ChildSql = ChildSql + "insert into ST_RDRecord_b( quantity, compositionQuantity, baseQuantity,";
                            ChildSql = ChildSql + "price, basePrice, amount, batch, isManualCost, isCostAccounted, taxFlag,";
                            ChildSql = ChildSql + "updatedBy, IsPresent, IsPromotionPresent, idbusiTypeByMergedFlow, idinventory,";
                            ChildSql = ChildSql + "idbaseunit, idunit, idwarehouse, idRDRecordDTO, createdtime, updated)";
                            ChildSql = ChildSql + "values('" + trow["Pro_count"].ToString() + "', '" + trow["Pro_count"].ToString() + trow["Pro_unit"].ToString() + "', '" + trow["Pro_count"].ToString() + "', ";
                            ChildSql = ChildSql + "'0', '0', '0', '" + trow["Pro_sno"].ToString() + "', '0', '0', '0', ";
                            ChildSql = ChildSql + "'" + txtUserID.Text + "', '0', '0', '3', '" + GetInventoryID(trow["Pro_code"].ToString()) + "', ";
                            ChildSql = ChildSql + "'" + GetUintID(trow["Pro_code"].ToString()) + "', '" + GetUintID(trow["Pro_code"].ToString()) + "', '" + getStoreID(trow["Pro_store"].ToString()) + "', '" + idRDRecordDTO + "', '" + DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "', '" + DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "')";
                            if (DBAccess.ExecSql(ChildSql) != -1)
                            {
                                string MpSQL = "update ProductInfo set Pro_isInsert=1 where id=" + trow["id"];
                                DBAccessPrivate.ExecSql(MpSQL);
                            }
                        }
                    }
                }
                txtLog.Text = txtLog.Text + "\n" + "T+导入结果：已完成！";
            }
            catch (Exception ew)
            {
                txtLog.Text = txtLog.Text + "\n" + "T+导入结果：失败！" + ew.Message + ew.TargetSite.Name;
            }
            finally
            {
                BindProductListDataSource();
            }
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


        /// <summary>
        /// 获取新的入库单条目Code
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
            return "MC-" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + i.ToString("0000");
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
            Session["PSno"] = temp;
            return temp;
        }



        /// <summary>
        /// 清空页面数据
        /// </summary>
        private void ReleaseControl()
        {
            txtSNo.Text = string.Empty;
            txtWeight.Text = string.Empty;
        }



        /// <summary>
        /// 检查必填项
        /// </summary>
        /// <returns></returns>
        private bool CheckDate()
        {
            if (txtSNo.Text == string.Empty)
            {
                txtLog.Text = txtLog.Text + "\n" + "必填项检查结果：'" + txtSNo.Label + "'未填写！";
                return false;
            }
            if (txtWeight.Text == string.Empty)
            {
                txtLog.Text = txtLog.Text + "\n" + "必填项检查结果：'" + txtWeight.Label + "'未填写！";
                return false;
            }
            if (txtUserID.Text == string.Empty)
            {
                txtLog.Text = txtLog.Text + "\n" + "必填项检查结果：'" + txtUserID.Label + "'未填写！";
                return false;
            }
            txtLog.Text = txtLog.Text + "\n" + "必填项检查结果：成功！";
            return true;
        }



        /// <summary>
        /// 从Session中读取串口读取并暂存的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGetWeight2_Click(object sender, EventArgs e)
        {
            try
            {
                txtWeight.Text = Session["WeightValue"].ToString();
                txtJWeight.Text = (Convert.ToDecimal(txtWeight.Text.Equals(string.Empty) ? "0" : txtWeight.Text) - Convert.ToDecimal(txtTWeight.Text.Equals(string.Empty) ? "0" : txtTWeight.Text)).ToString();
                Session["PNW"] = txtJWeight.Text;
                Session["PBW"] = txtTWeight.Text;
                Session["PSno"] = GetNewSNo();
                Session["PName"] = txtName.Text;
                Session["PGW"] = txtWeight.Text;
                Session["PSize"] = txtSpecification.Text;
            }
            catch (Exception ex)
            {
                txtLog.Text = txtLog.Text + "\n" + "净重计算错误：" + ex.Message;
            }
        }


        #region 已弃用的代码

        /// <summary>
        /// 打印(已弃用)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrint_Click(object sender, EventArgs e)
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
                txtLog.Text = txtLog.Text + "\n" + "打印结果：打印成功！";
                PrintPage();
            }
            catch (Exception ex)
            {
                txtLog.Text = txtLog.Text + "\n" + "打印出现错误：" + ex.Message;
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

            //设置打印内容及其字体，颜色和位置
            e.Graphics.DrawString(txtName.Text, new Font(new FontFamily("黑体"), 24), System.Drawing.Brushes.Red, Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow1X"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow1Y"].ToString()));
            e.Graphics.DrawString(txtSpecification.Text, new Font(new FontFamily("黑体"), 24), System.Drawing.Brushes.Red, Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow2X"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow2Y"].ToString()));
            e.Graphics.DrawString(txtWeight.Text, new Font(new FontFamily("黑体"), 24), System.Drawing.Brushes.Red, Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow3X"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow3Y"].ToString()));
            e.Graphics.DrawString(txtTWeight.Text, new Font(new FontFamily("黑体"), 24), System.Drawing.Brushes.Red, Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow4X"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow4Y"].ToString()));
            e.Graphics.DrawString(txtJWeight.Text, new Font(new FontFamily("黑体"), 24), System.Drawing.Brushes.Red, Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow5X"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow5Y"].ToString()));
            e.Graphics.DrawString(txtDate.Text, new Font(new FontFamily("黑体"), 24), System.Drawing.Brushes.Red, Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow6X"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow6Y"].ToString()));
            e.Graphics.DrawString(GetNewSNo(), new Font(new FontFamily("黑体"), 24), System.Drawing.Brushes.Red, Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow7X"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow7Y"].ToString()));
            txtLog.Text = txtLog.Text + "\n" + "打印内容设置结果：已完成！";

        }

        private void PrintPage()
        {
            int str = Convert.ToInt32(string.Empty.Equals(ConfigurationManager.AppSettings["PrintFontSize"].ToString()) ? "20" : ConfigurationManager.AppSettings["PrintFontSize"].ToString());
            System.Drawing.Image tempImg = new Bitmap(280, 293);
            Graphics g = Graphics.FromImage(tempImg);
            g.DrawString(txtName.Text, new Font(new FontFamily("黑体"), str), System.Drawing.Brushes.Red, Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow1X"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow1Y"].ToString()));
            g.DrawString(txtSpecification.Text, new Font(new FontFamily("黑体"), str), System.Drawing.Brushes.Red, Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow2X"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow2Y"].ToString()));
            g.DrawString(txtWeight.Text, new Font(new FontFamily("黑体"), str), System.Drawing.Brushes.Red, Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow3X"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow3Y"].ToString()));
            g.DrawString(txtTWeight.Text, new Font(new FontFamily("黑体"), str), System.Drawing.Brushes.Red, Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow4X"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow4Y"].ToString()));
            g.DrawString(txtJWeight.Text, new Font(new FontFamily("黑体"), str), System.Drawing.Brushes.Red, Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow5X"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow5Y"].ToString()));
            g.DrawString(txtDate.Text, new Font(new FontFamily("黑体"), str), System.Drawing.Brushes.Red, Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow6X"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow6Y"].ToString()));
            g.DrawString(GetNewSNo(), new Font(new FontFamily("黑体"), str), System.Drawing.Brushes.Red, Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow7X"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow7Y"].ToString()));
            txtLog.Text = txtLog.Text + "\n" + "打印内容设置结果：已完成！";
            tempImg.Save(ste, System.Drawing.Imaging.ImageFormat.Png);

        }

        #endregion

        /// <summary>
        /// 自动计算净重
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtWeight_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtJWeight.Text = (Convert.ToDecimal(txtWeight.Text.Equals(string.Empty) ? "0" : txtWeight.Text) - Convert.ToDecimal(txtTWeight.Text.Equals(string.Empty) ? "0" : txtTWeight.Text)).ToString();
                Session["PNW"] = txtJWeight.Text;
                Session["PBW"] = txtTWeight.Text;
                Session["PSno"] = GetNewSNo();
                Session["PName"] = txtName.Text;
                Session["PGW"] = txtWeight.Text;
                Session["PSize"] = txtSpecification.Text;

            }
            catch (Exception ex)
            {
                txtLog.Text = txtLog.Text + "\n" + "净重计算错误：" + ex.Message;
            }
        }
        int i = 0;
        protected void timerGet_Tick(object sender, EventArgs e)
        {
            i++;
            lblWeight.Text = i.ToString() + "&nbsp;&nbsp;KG";
            //lblWeight.Text = GetWeightValue() + "&nbsp;&nbsp;KG";
        }
    }
}