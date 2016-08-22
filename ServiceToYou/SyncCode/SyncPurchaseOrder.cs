using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using ServiceToYou.DbAccess;
using System.Data;
namespace ServiceToYou.SyncCode
{
    public class SyncPurchaseOrder
    {
        string TableName = "PU_PurchaseOrder";//表名[存放位置]
        string Conn1 = "DefaultConnection";
        string Conn2 = "DefaultConnection2";
        string Conn3 = "DefaultConnection3";
        string Conn4 = "DefaultConnection4";
        string Conn5 = "DefaultConnection5";
        Sync_All _AllSync = new Sync_All();
        Hashtable ht = new Hashtable();
        /// <summary>
        /// 执行同步
        /// </summary>
        public void DoSync()
        {
            DataTable model = GetNeedSyncDataTable();
            if (model != null && model.Rows.Count > 0)
            {
                foreach (DataRow rows in model.Rows)
                {
                    try
                    {
                        DoSyncForCommand(rows, rows["SyncTypeName"].ToString());
                        _AllSync.DoSyncEnd(rows["id"].ToString());
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }
        #region 同步数据到其他帐套，因为是单据转换，所以只有执行插入与删除没有更新操作。
        private void DoSyncForCommand(DataRow row, string Command)
        {
            DataRow temp = GetNeedSyncRow(row["SyncCode"].ToString());
            if (temp == null)
                return;
            switch (Command)
            {
                case "Insert":

                    break;
                case "Update":
                    //DoUpdateToOtherBook(temp);
                    DoInsertToOtherBook(temp);
                    break;
                case "Delete":
                    //DoDeleteToOtherBook(temp);
                    break;
                default: break;
            }
        }
        /// <summary>
        /// 执行Insert采购汇总表
        /// </summary>
        /// <param name="row">销货单汇总表记录</param>
        private void DoInsertToOtherBook(DataRow row)
        {
            try
            {
                string conn = string.Empty;
                string idpartner = string.Empty;
                string sql;//声明SQL变量，以后每个帐套的变量都重新清空在赋值执行。
                SetWhichBooks(ref conn, ref idpartner, row);
                string PoCode = GetNewPoCode(conn, TableName);
                string idcurrency = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idcurrency"].ToString(), "AA_Currency", Conn1), conn, "AA_Currency");
                string idmarketingOrgan = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idmarketingOrgan"].ToString(), "AA_Currency", Conn1), conn, "AA_Currency");
                //string idpartner = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idparent"].ToString(), "AA_Currency", Conn1), conn, "AA_Currency");
                string payType = "457";
                string voucherState = "181";
                //string auditorid = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idparent"].ToString(), "AA_Currency", Conn1), conn, "AA_Currency");
                //string makerid = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idparent"].ToString(), "AA_Currency", Conn1), conn, "AA_Currency");

                #region Insert到具体的子帐套
                sql = string.Empty;
                sql = sql + "insert into PU_PurchaseOrder (code,discountRate,exchangeRate,earnestMoney,origTotalAmount,";
                sql = sql + "totalAmount,origTotalTaxAmount,totalTaxAmount,origEarnestMoney,maker,auditor,reviser,iscarriedforwardout,";
                sql = sql + "iscarriedforwardin,ismodifiedcode,ts,updatedBy,PrintCount,prePaymentAmount,origPrePaymentAmount,";
                sql = sql + "idbusinesstype,idcurrency,idmarketingOrgan,idpartner,payType,voucherState,auditorid,makerid,";
                sql = sql + "voucherdate,madedate,auditeddate,createdtime,updated) values('" + PoCode + "', " + row["discountRate"].ToString() + ", " + row["exchangeRate"].ToString() + ",";
                sql = sql + "" + row["origAmount"].ToString() + ", " + row["origAmount"].ToString() + ", " + row["Amount"].ToString() + ", " + row["origTaxAmount"].ToString() + ", " + row["TaxAmount"].ToString() + ", " + row["Amount"].ToString() + ",";
                sql = sql + "'" + row["maker"].ToString() + "', '" + row["auditor"].ToString() + "', '" + row["reviser"].ToString() + "', " + row["iscarriedforwardout"].ToString() + ", " + row["iscarriedforwardin"].ToString() + ", " + row["ismodifiedcode"].ToString() + ",DEFAULT,";
                sql = sql + "'" + row["updatedBy"].ToString() + "', '0', '" + row["Amount"].ToString() + "', '" + row["Amount"].ToString() + "', '1', " + idcurrency + ",";
                sql = sql + "" + idmarketingOrgan + ", " + idpartner + ", " + payType + ", " + voucherState + ", " + row["auditorid"].ToString() + ", " + row["makerid"].ToString() + ", '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "', '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',";
                sql = sql + "'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "', '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "', '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                DBAccess1.ExecSql(conn, sql);
                DoInsertToOtherBook(row["id"].ToString(), conn, PoCode);
                #endregion
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 执行Insert采购明细表
        /// </summary>
        /// <param name="PoID">销货单汇总表ID</param>
        /// <param name="conn">具体同步到的那个帐套的连接字符串</param>
        /// <param name="PoID">采购订单Code</param>
        private void DoInsertToOtherBook(string SaleID, string conn, string PoCode)
        {
            string sql = "select * from SA_SaleDelivery_b where idSaleDeliveryDTO='" + SaleID + "'";
            DataTable dt = DBAccess1.QueryDataTable(Conn1, sql, null);
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                string code = i.ToString("0000");
                string idinventory = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idinventory"].ToString(), "AA_Inventory", Conn1), conn, "AA_Inventory");
                string idbaseunit = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idbaseunit"].ToString(), "AA_Unit", Conn1), conn, "AA_Unit");
                string idunit = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idunit"].ToString(), "AA_Unit", Conn1), conn, "AA_Unit");
                string idPurchaseOrderDTO = GetidByCodeFormOtherBook(PoCode, conn, "PU_PurchaseOrder");
                string childSql = string.Empty;
                childSql = childSql + "insert into PU_PurchaseOrder_b (code,quantity,compositionQuantity,discountRate,origDiscountPrice,";
                childSql = childSql + "taxRate,origTaxPrice,origDiscountAmount,origTax,origTaxAmount,discountPrice,taxPrice,discountAmount,";
                childSql = childSql + "tax,taxAmount,isPresent,countArrivalQuantity,countQuantity,baseQuantity,taxFlag,baseTaxPrice,";
                childSql = childSql + "baseDiscountPrice,arrivalTimes,stockTimes,ts,updatedBy,islaborcost,idinventory,idbaseunit,idunit,";
                childSql = childSql + "idPurchaseOrderDTO,updated) values('" + code + "', " + row["quantity"].ToString() + ", " + row["compositionQuantity"].ToString() + ", " + row["discountRate"].ToString() + ",";
                childSql = childSql + "" + row["discountPrice"].ToString() + ", " + row["taxRate"].ToString() + ", " + row["origTaxPrice"].ToString() + ", " + row["discountAmount"].ToString() + ", " + row["origTax"].ToString() + ", " + row["origTaxAmount"].ToString() + ",";
                childSql = childSql + "" + row["discountPrice"].ToString() + ", " + row["taxPrice"].ToString() + ", " + row["discountAmount"].ToString() + ", " + row["tax"].ToString() + ", " + row["taxAmount"].ToString() + ", " + row["isPresent"].ToString() + ", " + row["baseQuantity"].ToString() + ",";
                childSql = childSql + "', " + row["baseQuantity"].ToString() + ", " + row["taxFlag"].ToString() + ", " + row["baseTaxPrice"].ToString() + ", " + row["baseDiscountPrice"].ToString() + ", '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "', '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',";
                childSql = childSql + "'', '" + row["updatedBy"].ToString() + "', 0, " + idinventory + ", " + idbaseunit + ", " + idunit + ", " + idPurchaseOrderDTO + ", '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                DBAccess1.ExecSql(conn, childSql);
                i++;
            }
        }
        /// <summary>
        /// 执行Delete操作
        /// </summary>
        /// <param name="row"></param>
        private void DoDeleteToOtherBook(DataRow row)
        {
            string sql = string.Empty;
            sql = "delete from " + TableName + " where code = '" + row["SyncCode"] + "'";
            try
            {
                DBAccess1.ExecSql(Conn2, sql);
                DBAccess1.ExecSql(Conn3, sql);
                DBAccess1.ExecSql(Conn4, sql);
                DBAccess1.ExecSql(Conn5, sql);
            }
            catch (Exception ex)
            { }
        }

        #endregion
        /// <summary>
        /// 设置同步到哪个帐套，需要根据字符自定义项来操作。所以独立出一个方法
        /// </summary>
        /// <param name="conn">帐套链接字符串</param>
        /// <param name="idpartner">同步到的那个帐套的id</param>
        /// <param name="row">需要同步的记录</param>
        private void SetWhichBooks(ref string conn, ref string idpartner, DataRow row)
        {
            try
            {
                string cusID = row["idcustomer"].ToString();
                string sql = "select priuserdefnvc1 from AA_Partner where id='" + cusID + "'";
                string cusName = DBAccess1.QueryValue(Conn1, sql).ToString();
                #region 获取其他帐套下的ID
                string idparent2 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook("9901", "AA_Partner", Conn1), Conn2, "AA_Partner");
                string idparent3 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook("9901", "AA_Partner", Conn1), Conn3, "AA_Partner");
                string idparent4 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook("9901", "AA_Partner", Conn1), Conn4, "AA_Partner");
                string idparent5 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook("9901", "AA_Partner", Conn1), Conn5, "AA_Partner");
                #endregion
                switch (cusName)
                {
                    case "中央厨房":
                        conn = Conn1;
                        idpartner = cusID;
                        break;
                    case "皇厨主楼":
                        conn = Conn2;
                        idpartner = idparent2;
                        break;
                    case "皇厨附楼":
                        conn = Conn3;
                        idpartner = idparent3;
                        break;
                    case "荣升":
                        conn = Conn4;
                        idpartner = idparent4;
                        break;
                    case "十个铜钱":
                        conn = Conn5;
                        idpartner = idparent5;
                        break;
                    default: break;
                }
            }
            catch (Exception ex) { }
        }
        /// <summary>
        /// 获取新的PoCode
        /// </summary>
        /// <param name="conn">数据库连接字符串</param>
        /// <param name="tbName">表名</param>
        /// <returns></returns>
        private string GetNewPoCode(string conn, string tbName)
        {
            string ReturnCode = string.Empty;
            string sql = "select top 1 code from " + tbName + " order by code desc";
            string OldCode = DBAccess1.QueryValue(conn, sql).ToString();
            string BeginCode = OldCode.Substring(0, OldCode.LastIndexOf('-') + 1);
            string EndCode = OldCode.Substring(OldCode.LastIndexOf('-') + 1);
            int i = 1;
            try
            {
                i = i + Convert.ToInt32(EndCode);
            }
            catch (Exception ex)
            {

            }
            ReturnCode = BeginCode + i.ToString("0000");
            return ReturnCode;
        }
        /// <summary>
        /// 根据ID获取当前帐套下的Code
        /// </summary>
        /// <param name="id">当前记录的ID</param>
        /// <param name="table">表名</param>
        /// <param name="conn">帐套的链接字符串</param>
        /// <returns></returns>
        private string GetCodeByIDFromThisBook(string id, string table, string conn)
        {
            string ReturnCode = string.Empty;
            string sql = "select code from " + table + " where id='" + id + "'";
            try
            {
                DataTable dt = DBAccess1.QueryDataTable(conn, sql, null);
                if (dt != null && dt.Rows.Count > 0)
                    ReturnCode = dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {

            }
            return ReturnCode;
        }
        /// <summary>
        /// 根据Code查找在其他帐套下的ID
        /// </summary>
        /// <param name="code">当前记录的Code值</param>
        /// <param name="conn">其他帐套的链接字符串</param>
        /// <param name="othertable">其他帐套的表名</param>
        /// <returns>返回在其他帐套下的ID</returns>
        private string GetidByCodeFormOtherBook(string code, string conn, string othertable)
        {
            string ReturnCode = "1";
            string sql = "select id from " + othertable + " where code ='" + code + "'";
            try
            {
                DataTable dt = DBAccess1.QueryDataTable(conn, sql, null);
                if (dt != null && dt.Rows.Count > 0)
                    ReturnCode = dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {

            }
            return ReturnCode;
        }

        /// <summary>
        /// 获取需要同步的记录(业务表中的记录)
        /// </summary>
        /// <param name="code">每个表都有一个主键</param>
        /// <returns></returns>
        private DataRow GetNeedSyncRow(string code)
        {
            DataRow temp;
            string sql = "select * from SA_SaleDelivery where code='" + code + "'";
            temp = DBAccess1.QueryDataTable(Conn1, sql, null).Rows[0];
            return temp;
        }

        /// <summary>
        /// 检查记录是否在其他帐套存在，将结果存储在类顶部的Hashtable中
        /// </summary>
        private void CheckIsExist(string code)
        {
            string sql = "select id,code from " + TableName + " where code = '" + code + "'";
            try
            {
                //主楼
                DataTable tempDt;
                tempDt = DBAccess1.QueryDataTable(Conn2, sql, null);
                if (tempDt != null && tempDt.Rows.Count > 0)
                    ht.Add(Conn2, true);
                else
                    ht.Add(Conn2, false);
                //附楼
                DataTable tempDt2;
                tempDt2 = DBAccess1.QueryDataTable(Conn3, sql, null);
                if (tempDt2 != null && tempDt2.Rows.Count > 0)
                    ht.Add(Conn3, true);
                else
                    ht.Add(Conn3, false);
                //荣升
                DataTable tempDt3;
                tempDt3 = DBAccess1.QueryDataTable(Conn4, sql, null);
                if (tempDt3 != null && tempDt3.Rows.Count > 0)
                    ht.Add(Conn4, true);
                else
                    ht.Add(Conn4, false);
                //十个铜钱
                DataTable tempDt4;
                tempDt4 = DBAccess1.QueryDataTable(Conn5, sql, null);
                if (tempDt4 != null && tempDt4.Rows.Count > 0)
                    ht.Add(Conn5, true);
                else
                    ht.Add(Conn5, false);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 获取需要同步的记录(日志表中的记录)
        /// </summary>
        /// <returns></returns>
        private DataTable GetNeedSyncDataTable()
        {
            try
            {
                string sql = "select * from SyncLog where IsSync='0' and SyncName='_SyncSaleDelivery'";
                DataTable temp = DBAccess1.QueryDataTable(sql);
                return temp;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
