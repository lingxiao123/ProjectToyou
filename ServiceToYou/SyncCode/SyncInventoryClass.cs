using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceToYou.DbAccess;
using System.Collections;
using System.Data;
namespace ServiceToYou.SyncCode
{
    public class SyncInventoryClass
    {
        string TableName = "AA_InventoryClass";//表名[存放位置]
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
                        //DoSyncToBookTwo(rows);
                        //DoSyncToBookThree(rows);
                        //DoSyncToBookFour(rows);
                        //DoSyncToBookFive(rows);
                        _AllSync.DoSyncEnd(rows["id"].ToString());
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }
        #region 同步数据到其他帐套，因为从主帐套同步到其他帐套，所以从帐套2开始
        private void DoSyncForCommand(DataRow row, string Command)
        {
            DataRow temp = GetNeedSyncRow(row["SyncCode"].ToString());
            if (temp == null)
                return;
            switch (Command)
            {
                case "Insert":
                    DoInsertToOtherBook(temp);
                    break;
                case "Update":
                    DoUpdateToOtherBook(temp);
                    break;
                case "Delete":
                    DoDeleteToOtherBook(temp);
                    break;
                default: break;
            }
        }
        /// <summary>
        /// 执行Insert
        /// </summary>
        /// <param name="row"></param>
        private void DoInsertToOtherBook(DataRow row)
        {
            try
            {
                #region 获取其他帐套下的ID
                string idparent2 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idparent"].ToString(), "AA_InventoryClass", Conn1), Conn2, "AA_InventoryClass");
                string idparent3 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idparent"].ToString(), "AA_InventoryClass", Conn1), Conn3, "AA_InventoryClass");
                string idparent4 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idparent"].ToString(), "AA_InventoryClass", Conn1), Conn4, "AA_InventoryClass");
                string idparent5 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idparent"].ToString(), "AA_InventoryClass", Conn1), Conn5, "AA_InventoryClass");
                #endregion

                string sql;//声明SQL变量，以后每个帐套的变量都重新清空在赋值执行。

                #region Insert到皇厨主楼
                sql = string.Empty;
                sql = sql + "insert into AA_InventoryClass(code,name,isEndNode,depth,disabled,ts,updatedBy,inId,idMarketingOrgan," + (string.IsNullOrEmpty(idparent2) ? "" : "idpartner,") + "madeDate,updated,createdTime) values('" + row["code"].ToString() + "',";
                sql = sql + "'" + row["name"].ToString() + "'," + row["isEndNode"].ToString() + ",";
                sql = sql + "" + row["depth"].ToString() + "," + row["disabled"].ToString() + ",";
                sql = sql + "DEFAULT,'" + row["updatedBy"].ToString() + "',";
                sql = sql + "'" + row["inId"].ToString() + "'," + row["idMarketingOrgan"].ToString() + ",";
                sql = sql + "" + (string.IsNullOrEmpty(idparent2) ? "'" : (idparent2 + ",'")) + Convert.ToDateTime(row["madeDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss") + "',";
                sql = sql + "'" + Convert.ToDateTime(row["updated"].ToString()).ToString("yyyy-MM-dd hh:mm:ss") + "','" + Convert.ToDateTime(row["createdTime"].ToString()).ToString("yyyy-MM-dd hh:mm:ss") + "')";
                DBAccess1.ExecSql(Conn2, sql);
                #endregion
                #region Insert到皇厨附楼
                sql = string.Empty;
                sql = sql + "insert into AA_InventoryClass(code,name,isEndNode,depth,disabled,ts,updatedBy,inId,idMarketingOrgan," + (string.IsNullOrEmpty(idparent3) ? "" : "idpartner,") + "madeDate,updated,createdTime) values('" + row["code"].ToString() + "',";
                sql = sql + "'" + row["name"].ToString() + "'," + row["isEndNode"].ToString() + ",";
                sql = sql + "" + row["depth"].ToString() + "," + row["disabled"].ToString() + ",";
                sql = sql + "DEFAULT,'" + row["updatedBy"].ToString() + "',";
                sql = sql + "'" + row["inId"].ToString() + "'," + row["idMarketingOrgan"].ToString() + ",";
                sql = sql + "" + (string.IsNullOrEmpty(idparent3) ? "'" : (idparent3 + ",'")) + Convert.ToDateTime(row["madeDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss") + "',";
                sql = sql + "'" + Convert.ToDateTime(row["updated"].ToString()).ToString("yyyy-MM-dd hh:mm:ss") + "','" + Convert.ToDateTime(row["createdTime"].ToString()).ToString("yyyy-MM-dd hh:mm:ss") + "')";
                DBAccess1.ExecSql(Conn3, sql);
                #endregion
                #region Insert到荣升
                sql = string.Empty;
                sql = sql + "insert into AA_InventoryClass(code,name,isEndNode,depth,disabled,ts,updatedBy,inId,idMarketingOrgan," + (string.IsNullOrEmpty(idparent4) ? "" : "idpartner,") + "madeDate,updated,createdTime) values('" + row["code"].ToString() + "',";
                sql = sql + "'" + row["name"].ToString() + "'," + row["isEndNode"].ToString() + ",";
                sql = sql + "" + row["depth"].ToString() + "," + row["disabled"].ToString() + ",";
                sql = sql + "DEFAULT,'" + row["updatedBy"].ToString() + "',";
                sql = sql + "'" + row["inId"].ToString() + "'," + row["idMarketingOrgan"].ToString() + ",";
                sql = sql + "" + (string.IsNullOrEmpty(idparent4) ? "'" : (idparent4 + ",'")) + Convert.ToDateTime(row["madeDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss") + "',";
                sql = sql + "'" + Convert.ToDateTime(row["updated"].ToString()).ToString("yyyy-MM-dd hh:mm:ss") + "','" + Convert.ToDateTime(row["createdTime"].ToString()).ToString("yyyy-MM-dd hh:mm:ss") + "')";
                DBAccess1.ExecSql(Conn4, sql);
                #endregion
                #region Insert到十个铜钱
                sql = string.Empty;
                sql = sql + "insert into AA_InventoryClass(code,name,isEndNode,depth,disabled,ts,updatedBy,inId,idMarketingOrgan," + (string.IsNullOrEmpty(idparent5) ? "" : "idpartner,") + "madeDate,updated,createdTime) values('" + row["code"].ToString() + "',";
                sql = sql + "'" + row["name"].ToString() + "'," + row["isEndNode"].ToString() + ",";
                sql = sql + "" + row["depth"].ToString() + "," + row["disabled"].ToString() + ",";
                sql = sql + "DEFAULT,'" + row["updatedBy"].ToString() + "',";
                sql = sql + "'" + row["inId"].ToString() + "'," + row["idMarketingOrgan"].ToString() + ",";
                sql = sql + "" + (string.IsNullOrEmpty(idparent5) ? "'" : (idparent5 + ",'")) + Convert.ToDateTime(row["madeDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss") + "',";
                sql = sql + "'" + Convert.ToDateTime(row["updated"].ToString()).ToString("yyyy-MM-dd hh:mm:ss") + "','" + Convert.ToDateTime(row["createdTime"].ToString()).ToString("yyyy-MM-dd hh:mm:ss") + "')";
                DBAccess1.ExecSql(Conn5, sql);
                #endregion
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 执行Update
        /// </summary>
        /// <param name="row"></param>
        private void DoUpdateToOtherBook(DataRow row)
        {
            try
            {
                #region 获取其他帐套下的ID
                string idparent2 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idparent"].ToString(), TableName, Conn1), Conn2, TableName);
                string idparent3 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idparent"].ToString(), TableName, Conn1), Conn3, TableName);
                string idparent4 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idparent"].ToString(), TableName, Conn1), Conn4, TableName);
                string idparent5 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idparent"].ToString(), TableName, Conn1), Conn5, TableName);


                #endregion
                string sql;
                #region Update到皇厨主楼
                sql = string.Empty;
                sql = sql + "update AA_InventoryClass set name='" + row["name"].ToString() + "',";
                sql = sql + "isEndNode = " + row["isEndNode"].ToString() + ",depth = " + row["depth"].ToString() + ",";
                sql = sql + "disabled = " + row["desabled"].ToString() + ",ts = DEFAULT,";
                sql = sql + "updatedBy = '" + row["updatedBy"].ToString() + "',inId = '" + row["inId"].ToString() + "',";
                sql = sql + "idMarketingOrgan = " + row["idMarketingOrgan"].ToString() + ",";
                sql = sql + (string.IsNullOrEmpty(idparent2) ? "" : ("idparent = " + idparent2 + ",")) + "madeDate = '" + Convert.ToDateTime(row["madeDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss") + "',";
                sql = sql + "updated = '" + Convert.ToDateTime(row["updated"].ToString()).ToString("yyyy-MM-dd hh:mm:ss") + "',createdTime = '" + Convert.ToDateTime(row["createdTime"].ToString()).ToString("yyyy-MM-dd hh:mm:ss") + "'";
                sql = sql + "where code = '" + row["code"].ToString() + "'";


                DBAccess1.ExecSql(Conn2, sql);
                #endregion
                #region Update到皇厨附楼
                sql = string.Empty;
                sql = sql + "update AA_InventoryClass set name='" + row["name"].ToString() + "',";
                sql = sql + "isEndNode = " + row["isEndNode"].ToString() + ",depth = " + row["depth"].ToString() + ",";
                sql = sql + "disabled = " + row["desabled"].ToString() + ",ts = DEFAULT,";
                sql = sql + "updatedBy = '" + row["updatedBy"].ToString() + "',inId = '" + row["inId"].ToString() + "',";
                sql = sql + "idMarketingOrgan = " + row["idMarketingOrgan"].ToString() + ",";
                sql = sql + (string.IsNullOrEmpty(idparent3) ? "" : ("idparent = " + idparent3 + ",")) + "madeDate = '" + Convert.ToDateTime(row["madeDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss") + "',";
                sql = sql + "updated = '" + Convert.ToDateTime(row["updated"].ToString()).ToString("yyyy-MM-dd hh:mm:ss") + "',createdTime = '" + Convert.ToDateTime(row["createdTime"].ToString()).ToString("yyyy-MM-dd hh:mm:ss") + "'";
                sql = sql + "where code = '" + row["code"].ToString() + "'";
                DBAccess1.ExecSql(Conn3, sql);
                #endregion
                #region Update到荣升
                sql = string.Empty;
                sql = sql + "update AA_InventoryClass set name='" + row["name"].ToString() + "',";
                sql = sql + "isEndNode = " + row["isEndNode"].ToString() + ",depth = " + row["depth"].ToString() + ",";
                sql = sql + "disabled = " + row["desabled"].ToString() + ",ts = DEFAULT,";
                sql = sql + "updatedBy = '" + row["updatedBy"].ToString() + "',inId = '" + row["inId"].ToString() + "',";
                sql = sql + "idMarketingOrgan = " + row["idMarketingOrgan"].ToString() + ",";
                sql = sql + (string.IsNullOrEmpty(idparent4) ? "" : ("idparent = " + idparent4 + ",")) + "madeDate = '" + Convert.ToDateTime(row["madeDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss") + "',";
                sql = sql + "updated = '" + Convert.ToDateTime(row["updated"].ToString()).ToString("yyyy-MM-dd hh:mm:ss") + "',createdTime = '" + Convert.ToDateTime(row["createdTime"].ToString()).ToString("yyyy-MM-dd hh:mm:ss") + "'";
                sql = sql + "where code = '" + row["code"].ToString() + "'";
                DBAccess1.ExecSql(Conn4, sql);
                #endregion
                #region Update到十个铜钱
                sql = string.Empty;
                sql = sql + "update AA_InventoryClass set name='" + row["name"].ToString() + "',";
                sql = sql + "isEndNode = " + row["isEndNode"].ToString() + ",depth = " + row["depth"].ToString() + ",";
                sql = sql + "disabled = " + row["desabled"].ToString() + ",ts = DEFAULT,";
                sql = sql + "updatedBy = '" + row["updatedBy"].ToString() + "',inId = '" + row["inId"].ToString() + "',";
                sql = sql + "idMarketingOrgan = " + row["idMarketingOrgan"].ToString() + ",";
                sql = sql + (string.IsNullOrEmpty(idparent5) ? "" : ("idparent = " + idparent5 + ",")) + "madeDate = '" + Convert.ToDateTime(row["madeDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss") + "',";
                sql = sql + "updated = '" + Convert.ToDateTime(row["updated"].ToString()).ToString("yyyy-MM-dd hh:mm:ss") + "',createdTime = '" + Convert.ToDateTime(row["createdTime"].ToString()).ToString("yyyy-MM-dd hh:mm:ss") + "'";
                sql = sql + "where code = '" + row["code"].ToString() + "'";
                DBAccess1.ExecSql(Conn5, sql);
                #endregion
            }
            catch (Exception ex)
            {

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
            string ReturnCode = string.Empty;
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
            string sql = "select * from " + TableName + " where code='" + code + "'";
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
                string sql = "select * from SyncLog where IsSync='0' and SyncName='_SyncInventoryClass'";
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
