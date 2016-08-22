using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ServiceToYou.DbAccess;
using System.Configuration;
namespace ServiceToYou.SyncCode
{
    public class SyncPosition
    {
        string TableName = "AA_Position";//表名[存放位置]
        string Conn2 = "DefaultConnection2";
        string Conn3 = "DefaultConnection3";
        string Conn4 = "DefaultConnection4";
        string Conn5 = "DefaultConnection5";
        Sync_All _AllSync = new Sync_All();
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
                        DoSyncToBookTwo(rows);
                        DoSyncToBookThree(rows);
                        DoSyncToBookFour(rows);
                        DoSyncToBookFive(rows);
                        _AllSync.DoSyncEnd(rows["id"].ToString());
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }
        #region 同步数据到其他帐套，因为从主帐套同步到其他帐套，所以从帐套2开始
        /// <summary>
        /// 皇厨主楼
        /// </summary>
        /// <param name="row"></param>
        private void DoSyncToBookTwo(DataRow row)
        {
            DataRow temp = GetNeedSyncRow(row["SyncCode"].ToString());
            if (temp == null)
                return;
            string sql = string.Empty;
            sql = sql + "INSERT INTO [AA_Position]";
            sql = sql + "([code]";
            sql = sql + ",[name]";
            sql = sql + ",[depth]";
            sql = sql + ",[isendnode]";
            sql = sql + ",[inid]";
            sql = sql + " ,[idparent]";
            sql = sql + ",[createdTime])";
            sql = sql + "VALUES";
            sql = sql + " (" + temp["code"] + " ";
            sql = sql + " ," + temp["name"] + "";
            sql = sql + " ," + temp["depth"] + "";
            sql = sql + " ," + temp["isendnode"] + "";
            sql = sql + " ," + temp["inid"] + "";
            sql = sql + "," + temp["idparent"] + "";
            sql = sql + " ," + temp["createdTime"] + ")";
            DBAccess1.ExecSql(Conn2, sql);
        }
        /// <summary>
        /// 皇厨附楼
        /// </summary>
        /// <param name="row"></param>
        private void DoSyncToBookThree(DataRow row)
        {
            DataRow temp = GetNeedSyncRow(row["SyncCode"].ToString());
            if (temp == null)
                return;
            string sql = string.Empty;
            sql = sql + "INSERT INTO [AA_Position]";
            sql = sql + "([code]";
            sql = sql + ",[name]";
            sql = sql + ",[depth]";
            sql = sql + ",[isendnode]";
            sql = sql + ",[inid]";
            sql = sql + " ,[idparent]";
            sql = sql + ",[createdTime])";
            sql = sql + "VALUES";
            sql = sql + " (" + temp["code"] + " ";
            sql = sql + " ," + temp["name"] + "";
            sql = sql + " ," + temp["depth"] + "";
            sql = sql + " ," + temp["isendnode"] + "";
            sql = sql + " ," + temp["inid"] + "";
            sql = sql + "," + temp["idparent"] + "";
            sql = sql + " ," + temp["createdTime"] + ")";
            DBAccess1.ExecSql(Conn3, sql);
        }
        /// <summary>
        /// 荣升
        /// </summary>
        /// <param name="row"></param>
        private void DoSyncToBookFour(DataRow row)
        {
            DataRow temp = GetNeedSyncRow(row["SyncCode"].ToString());
            if (temp == null)
                return;
            string sql = string.Empty;
            sql = sql + "INSERT INTO [AA_Position]";
            sql = sql + "([code]";
            sql = sql + ",[name]";
            sql = sql + ",[depth]";
            sql = sql + ",[isendnode]";
            sql = sql + ",[inid]";
            sql = sql + " ,[idparent]";
            sql = sql + ",[createdTime])";
            sql = sql + "VALUES";
            sql = sql + " (" + temp["code"] + " ";
            sql = sql + " ," + temp["name"] + "";
            sql = sql + " ," + temp["depth"] + "";
            sql = sql + " ," + temp["isendnode"] + "";
            sql = sql + " ," + temp["inid"] + "";
            sql = sql + "," + temp["idparent"] + "";
            sql = sql + " ," + temp["createdTime"] + ")";
            DBAccess1.ExecSql(Conn4, sql);
        }
        /// <summary>
        /// 十个铜钱
        /// </summary>
        /// <param name="row"></param>
        private void DoSyncToBookFive(DataRow row)
        {
            DataRow temp = GetNeedSyncRow(row["SyncCode"].ToString());
            if (temp == null)
                return;
            string sql = string.Empty;
            sql = sql + "INSERT INTO [AA_Position]";
            sql = sql + "([code]";
            sql = sql + ",[name]";
            sql = sql + ",[depth]";
            sql = sql + ",[isendnode]";
            sql = sql + ",[inid]";
            sql = sql + " ,[idparent]";
            sql = sql + ",[createdTime])";
            sql = sql + "VALUES";
            sql = sql + " (" + temp["code"] + " ";
            sql = sql + " ," + temp["name"] + "";
            sql = sql + " ," + temp["depth"] + "";
            sql = sql + " ," + temp["isendnode"] + "";
            sql = sql + " ," + temp["inid"] + "";
            sql = sql + "," + temp["idparent"] + "";
            sql = sql + " ," + temp["createdTime"] + ")";
            DBAccess1.ExecSql(Conn5, sql);
        }
        #endregion

        /// <summary>
        /// 根据Code查找在其他帐套下的ID
        /// </summary>
        /// <param name="code">当前记录的Code值</param>
        /// <param name="Books">其他帐套的名称</param>
        /// <returns>返回在其他帐套下的ID</returns>
        private string GetidByCodeFormOtherBook(string code, string Books)
        {
            string ReturnCode = string.Empty;

            return ReturnCode;
        }
        private DataRow GetNeedSyncRow(string code)
        {
            DataRow temp;
            string sql = "select * from " + TableName + " where code='" + code + "'";
            temp = DBAccess1.QueryDataTable(sql).Rows[0];
            return temp;
        }

        /// <summary>
        /// 获取需要同步的记录
        /// </summary>
        /// <returns></returns>
        private DataTable GetNeedSyncDataTable()
        {
            try
            {
                string sql = "select * from SyncLog where IsSync='0' and SyncName='_SyncPosition'";
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
