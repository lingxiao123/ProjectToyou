using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceToYou.DbAccess;
using System.Data;
using System.Collections;

namespace ServiceToYou.SyncCode
{
    public class Sync_All
    {
        /// <summary>
        /// 获取所有需要同步的条目
        /// 即IsSync=0的条目
        /// </summary>
        /// <returns>查询结果以DataTable显示</returns>
        private System.Data.DataTable GetSyncDataTable()
        {
            DataTable ReturnDataTable;
            string sql = "select * from SyncLog where IsSync = '0'";
            ReturnDataTable= DBAccess1.QueryDataTable(sql);
            return ReturnDataTable;
        }
        /// <summary>
        /// 根据所有需要同步的条目进行编辑Hashtable
        /// </summary>
        /// <param name="ht">HashTable为定义好的各个基础档案的功能Code名称[_SyncPosition]</param>
        public void SetHashTableForSync(ref Hashtable ht)
        {
            //从同步日志表中获取所有需要同步的条目
            DataTable temp = GetSyncDataTable();
            //若同步日志表中有数据，就设置_SyncPosition的值为1,代表需要同步.
            if (temp != null && temp.Rows.Count > 0)
            {
                //因为数据库中字段名为SyncName的字段中存的值与ht中Key值相通
                //每一条的记录的key都可以从DataTable中相对应的设置Value值
                foreach (DataRow row in temp.Rows)
                {
                    ht[row["SyncName"]] = "1";
                }
            }
        }
        /// <summary>
        /// 同步完成后将数据库中的IsSync字段改为1
        /// </summary>
        public void DoSyncEnd(string id)
        {
            string sql = "update SyncLog set IsSync=1 where id='" + id + "'";
            DBAccess1.ExecSql(sql);
        }
    }
}
