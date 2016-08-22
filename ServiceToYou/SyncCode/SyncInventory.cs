using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceToYou.DbAccess;
using System.Collections;

namespace ServiceToYou.SyncCode
{
    public class SyncInventory
    {
        string TableName = "AA_Inventory";//表名[存放位置]
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
                string idinventoryclass2 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idinventoryclass"].ToString(), "AA_InventoryClass", Conn1), Conn2, "AA_InventoryClass");
                string idinventoryclass3 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idinventoryclass"].ToString(), "AA_InventoryClass", Conn1), Conn3, "AA_InventoryClass");
                string idinventoryclass4 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idinventoryclass"].ToString(), "AA_InventoryClass", Conn1), Conn4, "AA_InventoryClass");
                string idinventoryclass5 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idinventoryclass"].ToString(), "AA_InventoryClass", Conn1), Conn5, "AA_InventoryClass");

                string idMarketingOrgan2 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idMarketingOrgan"].ToString(), "AA_MarketingOrgan", Conn1), Conn2, "AA_MarketingOrgan");
                string idMarketingOrgan3 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idMarketingOrgan"].ToString(), "AA_MarketingOrgan", Conn1), Conn3, "AA_MarketingOrgan");
                string idMarketingOrgan4 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idMarketingOrgan"].ToString(), "AA_MarketingOrgan", Conn1), Conn4, "AA_MarketingOrgan");
                string idMarketingOrgan5 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idMarketingOrgan"].ToString(), "AA_MarketingOrgan", Conn1), Conn5, "AA_MarketingOrgan");

                string idunit2 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idunit"].ToString(), "AA_Unit", Conn1), Conn2, "AA_Unit");
                string idunit3 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idunit"].ToString(), "AA_Unit", Conn1), Conn3, "AA_Unit");
                string idunit4 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idunit"].ToString(), "AA_Unit", Conn1), Conn4, "AA_Unit");
                string idunit5 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idunit"].ToString(), "AA_Unit", Conn1), Conn5, "AA_Unit");

                string idunitbymanufacture2 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idunitbymanufacture"].ToString(), "AA_Unit", Conn1), Conn2, "AA_Unit");
                string idunitbymanufacture3 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idunitbymanufacture"].ToString(), "AA_Unit", Conn1), Conn3, "AA_Unit");
                string idunitbymanufacture4 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idunitbymanufacture"].ToString(), "AA_Unit", Conn1), Conn4, "AA_Unit");
                string idunitbymanufacture5 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idunitbymanufacture"].ToString(), "AA_Unit", Conn1), Conn5, "AA_Unit");

                string idUnitByPurchase2 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitByPurchase"].ToString(), "AA_Unit", Conn1), Conn2, "AA_Unit");
                string idUnitByPurchase3 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitByPurchase"].ToString(), "AA_Unit", Conn1), Conn3, "AA_Unit");
                string idUnitByPurchase4 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitByPurchase"].ToString(), "AA_Unit", Conn1), Conn4, "AA_Unit");
                string idUnitByPurchase5 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitByPurchase"].ToString(), "AA_Unit", Conn1), Conn5, "AA_Unit");

                string idUnitByRetail2 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitByRetail"].ToString(), "AA_Unit", Conn1), Conn2, "AA_Unit");
                string idUnitByRetail3 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitByRetail"].ToString(), "AA_Unit", Conn1), Conn3, "AA_Unit");
                string idUnitByRetail4 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitByRetail"].ToString(), "AA_Unit", Conn1), Conn4, "AA_Unit");
                string idUnitByRetail5 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitByRetail"].ToString(), "AA_Unit", Conn1), Conn5, "AA_Unit");

                string idUnitBySale2 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitBySale"].ToString(), "AA_Unit", Conn1), Conn2, "AA_Unit");
                string idUnitBySale3 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitBySale"].ToString(), "AA_Unit", Conn1), Conn3, "AA_Unit");
                string idUnitBySale4 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitBySale"].ToString(), "AA_Unit", Conn1), Conn4, "AA_Unit");
                string idUnitBySale5 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitBySale"].ToString(), "AA_Unit", Conn1), Conn5, "AA_Unit");

                string idUnitByStock2 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitByStock"].ToString(), "AA_Unit", Conn1), Conn2, "AA_Unit");
                string idUnitByStock3 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitByStock"].ToString(), "AA_Unit", Conn1), Conn3, "AA_Unit");
                string idUnitByStock4 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitByStock"].ToString(), "AA_Unit", Conn1), Conn4, "AA_Unit");
                string idUnitByStock5 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitByStock"].ToString(), "AA_Unit", Conn1), Conn5, "AA_Unit");
                #endregion

                string sql;//声明SQL变量，以后每个帐套的变量都重新清空在赋值执行。

                #region Insert到皇厨主楼
                sql = string.Empty;
                sql = sql + "insert into AA_Inventory (isSale,isMadeSelf,code,name,shorthand,specification,isLimitedWithdraw,isBatch,isQualityPeriod,isPurchase,isMaterial,disabled,isQualityCheck,";
                sql = sql + "isMadeRequest,isSingleUnit,ts,updatedBy,Userfreeitem7,Userfreeitem6,Userfreeitem2,Userfreeitem1,Userfreeitem9,Userfreeitem0,Userfreeitem8,Userfreeitem5,Userfreeitem4,Userfreeitem3,MustInputFreeitem7,";
                sql = sql + " MustInputFreeitem2,MustInputFreeitem6,MustInputFreeitem3,MustInputFreeitem5,MustInputFreeitem4,MustInputFreeitem9,MustInputFreeitem1,MustInputFreeitem8,MustInputFreeitem0,HasEverChanged,isphantom,";
                sql = sql + "ControlRangeFreeitem0,ControlRangeFreeitem1,ControlRangeFreeitem2,ControlRangeFreeitem3,ControlRangeFreeitem4,ControlRangeFreeitem5,ControlRangeFreeitem6,ControlRangeFreeitem7,ControlRangeFreeitem8,";
                sql = sql + "ControlRangeFreeitem9,IsLaborCost,IsNew,MadeRecordDate,IsSuite,IsWeigh,idinventoryclass,idMarketingOrgan,idunit,idunitbymanufacture,idUnitByPurchase,idUnitByRetail,idUnitBySale,idUnitByStock,taxRate,";
                sql = sql + "unittype,valueType,madeDate,updated,createdTime,Creater,Changer) values ("+ row["isSale"].ToString() + ","+ row["isMadeSelf"].ToString() + ",'" + row["code"].ToString() + "','" + row["name"].ToString() + "','" + row["shorthand"].ToString() + "','" + row["specification"].ToString() + "',";
                sql = sql + "" + row["isLimitedWithdraw"].ToString() + "," + row["isBatch"].ToString() + "," + row["isQualityPeriod"].ToString() + ",";
                sql = sql + "" + row["isPurchase"].ToString() + "," + row["isMaterial"].ToString() + "," + row["disabled"].ToString() + "," + row["isQualityCheck"].ToString() + "," + row["isMadeRequest"].ToString() + "," + row["isSingleUnit"].ToString() + ",";
                sql = sql + "DEFAULT,'" + row["updatedBy"].ToString() + "'," + row["Userfreeitem7"].ToString() + "," + row["Userfreeitem6"].ToString() + "," + row["Userfreeitem2"].ToString() + "," + row["Userfreeitem1"].ToString() + "," + row["Userfreeitem9"].ToString() + "";
                sql = sql + "," + row["Userfreeitem0"].ToString() + "," + row["Userfreeitem8"].ToString() + "," + row["Userfreeitem5"].ToString() + "," + row["Userfreeitem4"].ToString() + "," + row["Userfreeitem3"].ToString() + ",";
                sql = sql + "" + row["MustInputFreeitem7"].ToString() + "," + row["MustInputFreeitem2"].ToString() + "," + row["MustInputFreeitem6"].ToString() + "," + row["MustInputFreeitem3"].ToString() + "," + row["MustInputFreeitem5"].ToString() + "";
                sql = sql + "," + row["MustInputFreeitem4"].ToString() + "," + row["MustInputFreeitem9"].ToString() + "," + row["MustInputFreeitem1"].ToString() + ",";
                sql = sql + "" + row["MustInputFreeitem8"].ToString() + "," + row["MustInputFreeitem0"].ToString() + "";
                sql = sql + ",'" + row["HasEverChanged"].ToString() + "'," + row["isphantom"].ToString() + "," + row["ControlRangeFreeitem0"].ToString() + "," + row["ControlRangeFreeitem1"].ToString() + ",";
                sql = sql + "" + row["ControlRangeFreeitem2"].ToString() + "," + row["ControlRangeFreeitem3"].ToString() + "," + row["ControlRangeFreeitem4"].ToString() + "," + row["ControlRangeFreeitem5"].ToString() + "," + row["ControlRangeFreeitem6"].ToString() + "," + row["ControlRangeFreeitem7"].ToString() + "," + row["ControlRangeFreeitem8"].ToString() + "," + row["ControlRangeFreeitem9"].ToString() + ",";
                sql = sql + "" + row["IslaborCost"].ToString() + "," + row["IsNew"].ToString() + ",'" + (Convert.ToDateTime(row["MadeRecordDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "'," + row["IsSuite"] + "," + row["IsWeigh"].ToString() + ",";
                sql = sql + "" + idinventoryclass2 + "," + idMarketingOrgan2 + "," + idunit2 + "," + idunitbymanufacture2 + "," + idUnitByPurchase2 + ",";
                sql = sql + "" + idUnitByRetail2 + "," + idUnitBySale2 + "," + idUnitByStock2 + ",";
                sql = sql + "" + row["taxRate"].ToString() + "," + row["unittype"].ToString() + "," + row["valueType"].ToString() + ",'" + (Convert.ToDateTime(row["madeDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "','" + (Convert.ToDateTime(row["updated"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "','" + (Convert.ToDateTime(row["createdTime"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "','" + row["Creater"].ToString() + "','" + row["Changer"].ToString() + "')";
                DBAccess1.ExecSql(Conn2, sql);
                #endregion
                #region Insert到皇厨附楼
                sql = string.Empty;
                sql = sql + "insert into AA_Inventory (isSale,isMadeSelf,code,name,shorthand,specification,isLimitedWithdraw,isBatch,isQualityPeriod,isPurchase,isMaterial,disabled,isQualityCheck,";
                sql = sql + "isMadeRequest,isSingleUnit,ts,updatedBy,Userfreeitem7,Userfreeitem6,Userfreeitem2,Userfreeitem1,Userfreeitem9,Userfreeitem0,Userfreeitem8,Userfreeitem5,Userfreeitem4,Userfreeitem3,MustInputFreeitem7,";
                sql = sql + " MustInputFreeitem2,MustInputFreeitem6,MustInputFreeitem3,MustInputFreeitem5,MustInputFreeitem4,MustInputFreeitem9,MustInputFreeitem1,MustInputFreeitem8,MustInputFreeitem0,HasEverChanged,isphantom,";
                sql = sql + "ControlRangeFreeitem0,ControlRangeFreeitem1,ControlRangeFreeitem2,ControlRangeFreeitem3,ControlRangeFreeitem4,ControlRangeFreeitem5,ControlRangeFreeitem6,ControlRangeFreeitem7,ControlRangeFreeitem8,";
                sql = sql + "ControlRangeFreeitem9,IsLaborCost,IsNew,MadeRecordDate,IsSuite,IsWeigh,idinventoryclass,idMarketingOrgan,idunit,idunitbymanufacture,idUnitByPurchase,idUnitByRetail,idUnitBySale,idUnitByStock,taxRate,";
                sql = sql + "unittype,valueType,madeDate,updated,createdTime,Creater,Changer) values (" + row["isSale"].ToString() + "," + row["isMadeSelf"].ToString() + ",'" + row["code"].ToString() + "','" + row["name"].ToString() + "','" + row["shorthand"].ToString() + "','" + row["specification"].ToString() + "',";
                sql = sql + "" + row["isLimitedWithdraw"].ToString() + "," + row["isBatch"].ToString() + "," + row["isQualityPeriod"].ToString() + ",";
                sql = sql + "" + row["isPurchase"].ToString() + "," + row["isMaterial"].ToString() + "," + row["disabled"].ToString() + "," + row["isQualityCheck"].ToString() + "," + row["isMadeRequest"].ToString() + "," + row["isSingleUnit"].ToString() + ",";
                sql = sql + "DEFAULT,'" + row["updatedBy"].ToString() + "'," + row["Userfreeitem7"].ToString() + "," + row["Userfreeitem6"].ToString() + "," + row["Userfreeitem2"].ToString() + "," + row["Userfreeitem1"].ToString() + "," + row["Userfreeitem9"].ToString() + "";
                sql = sql + "," + row["Userfreeitem0"].ToString() + "," + row["Userfreeitem8"].ToString() + "," + row["Userfreeitem5"].ToString() + "," + row["Userfreeitem4"].ToString() + "," + row["Userfreeitem3"].ToString() + ",";
                sql = sql + "" + row["MustInputFreeitem7"].ToString() + "," + row["MustInputFreeitem2"].ToString() + "," + row["MustInputFreeitem6"].ToString() + "," + row["MustInputFreeitem3"].ToString() + "," + row["MustInputFreeitem5"].ToString() + "";
                sql = sql + "," + row["MustInputFreeitem4"].ToString() + "," + row["MustInputFreeitem9"].ToString() + "," + row["MustInputFreeitem1"].ToString() + ",";
                sql = sql + "" + row["MustInputFreeitem8"].ToString() + "," + row["MustInputFreeitem0"].ToString() + "";
                sql = sql + ",'" + row["HasEverChanged"].ToString() + "'," + row["isphantom"].ToString() + "," + row["ControlRangeFreeitem0"].ToString() + "," + row["ControlRangeFreeitem1"].ToString() + ",";
                sql = sql + "" + row["ControlRangeFreeitem2"].ToString() + "," + row["ControlRangeFreeitem3"].ToString() + "," + row["ControlRangeFreeitem4"].ToString() + "," + row["ControlRangeFreeitem5"].ToString() + "," + row["ControlRangeFreeitem6"].ToString() + "," + row["ControlRangeFreeitem7"].ToString() + "," + row["ControlRangeFreeitem8"].ToString() + "," + row["ControlRangeFreeitem9"].ToString() + ",";
                sql = sql + "" + row["IslaborCost"].ToString() + "," + row["IsNew"].ToString() + ",'" + (Convert.ToDateTime(row["MadeRecordDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "'," + row["IsSuite"] + "," + row["IsWeigh"].ToString() + ",";
                sql = sql + "" + idinventoryclass3 + "," + idMarketingOrgan3 + "," + idunit3 + "," + idunitbymanufacture3 + "," + idUnitByPurchase3 + ",";
                sql = sql + "" + idUnitByRetail3 + "," + idUnitBySale3 + "," + idUnitByStock3 + ",";
                sql = sql + "" + row["taxRate"].ToString() + "," + row["unittype"].ToString() + "," + row["valueType"].ToString() + ",'" + (Convert.ToDateTime(row["madeDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "','" + (Convert.ToDateTime(row["updated"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "','" + (Convert.ToDateTime(row["createdTime"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "','" + row["Creater"].ToString() + "','" + row["Changer"].ToString() + "')";
                DBAccess1.ExecSql(Conn3, sql);
                #endregion
                #region Insert到荣升
                sql = string.Empty;
                sql = sql + "insert into AA_Inventory (isSale,isMadeSelf,code,name,shorthand,specification,isLimitedWithdraw,isBatch,isQualityPeriod,isPurchase,isMaterial,disabled,isQualityCheck,";
                sql = sql + "isMadeRequest,isSingleUnit,ts,updatedBy,Userfreeitem7,Userfreeitem6,Userfreeitem2,Userfreeitem1,Userfreeitem9,Userfreeitem0,Userfreeitem8,Userfreeitem5,Userfreeitem4,Userfreeitem3,MustInputFreeitem7,";
                sql = sql + " MustInputFreeitem2,MustInputFreeitem6,MustInputFreeitem3,MustInputFreeitem5,MustInputFreeitem4,MustInputFreeitem9,MustInputFreeitem1,MustInputFreeitem8,MustInputFreeitem0,HasEverChanged,isphantom,";
                sql = sql + "ControlRangeFreeitem0,ControlRangeFreeitem1,ControlRangeFreeitem2,ControlRangeFreeitem3,ControlRangeFreeitem4,ControlRangeFreeitem5,ControlRangeFreeitem6,ControlRangeFreeitem7,ControlRangeFreeitem8,";
                sql = sql + "ControlRangeFreeitem9,IsLaborCost,IsNew,MadeRecordDate,IsSuite,IsWeigh,idinventoryclass,idMarketingOrgan,idunit,idunitbymanufacture,idUnitByPurchase,idUnitByRetail,idUnitBySale,idUnitByStock,taxRate,";
                sql = sql + "unittype,valueType,madeDate,updated,createdTime,Creater,Changer) values (" + row["isSale"].ToString() + "," + row["isMadeSelf"].ToString() + ",'" + row["code"].ToString() + "','" + row["name"].ToString() + "','" + row["shorthand"].ToString() + "','" + row["specification"].ToString() + "',";
                sql = sql + "" + row["isLimitedWithdraw"].ToString() + "," + row["isBatch"].ToString() + "," + row["isQualityPeriod"].ToString() + ",";
                sql = sql + "" + row["isPurchase"].ToString() + "," + row["isMaterial"].ToString() + "," + row["disabled"].ToString() + "," + row["isQualityCheck"].ToString() + "," + row["isMadeRequest"].ToString() + "," + row["isSingleUnit"].ToString() + ",";
                sql = sql + "DEFAULT,'" + row["updatedBy"].ToString() + "'," + row["Userfreeitem7"].ToString() + "," + row["Userfreeitem6"].ToString() + "," + row["Userfreeitem2"].ToString() + "," + row["Userfreeitem1"].ToString() + "," + row["Userfreeitem9"].ToString() + "";
                sql = sql + "," + row["Userfreeitem0"].ToString() + "," + row["Userfreeitem8"].ToString() + "," + row["Userfreeitem5"].ToString() + "," + row["Userfreeitem4"].ToString() + "," + row["Userfreeitem3"].ToString() + ",";
                sql = sql + "" + row["MustInputFreeitem7"].ToString() + "," + row["MustInputFreeitem2"].ToString() + "," + row["MustInputFreeitem6"].ToString() + "," + row["MustInputFreeitem3"].ToString() + "," + row["MustInputFreeitem5"].ToString() + "";
                sql = sql + "," + row["MustInputFreeitem4"].ToString() + "," + row["MustInputFreeitem9"].ToString() + "," + row["MustInputFreeitem1"].ToString() + ",";
                sql = sql + "" + row["MustInputFreeitem8"].ToString() + "," + row["MustInputFreeitem0"].ToString() + "";
                sql = sql + ",'" + row["HasEverChanged"].ToString() + "'," + row["isphantom"].ToString() + "," + row["ControlRangeFreeitem0"].ToString() + "," + row["ControlRangeFreeitem1"].ToString() + ",";
                sql = sql + "" + row["ControlRangeFreeitem2"].ToString() + "," + row["ControlRangeFreeitem3"].ToString() + "," + row["ControlRangeFreeitem4"].ToString() + "," + row["ControlRangeFreeitem5"].ToString() + "," + row["ControlRangeFreeitem6"].ToString() + "," + row["ControlRangeFreeitem7"].ToString() + "," + row["ControlRangeFreeitem8"].ToString() + "," + row["ControlRangeFreeitem9"].ToString() + ",";
                sql = sql + "" + row["IslaborCost"].ToString() + "," + row["IsNew"].ToString() + ",'" + (Convert.ToDateTime(row["MadeRecordDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "'," + row["IsSuite"] + "," + row["IsWeigh"].ToString() + ",";
                sql = sql + "" + idinventoryclass4 + "," + idMarketingOrgan4 + "," + idunit4 + "," + idunitbymanufacture4 + "," + idUnitByPurchase4 + ",";
                sql = sql + "" + idUnitByRetail4 + "," + idUnitBySale4 + "," + idUnitByStock4 + ",";
                sql = sql + "" + row["taxRate"].ToString() + "," + row["unittype"].ToString() + "," + row["valueType"].ToString() + ",'" + (Convert.ToDateTime(row["madeDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "','" + (Convert.ToDateTime(row["updated"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "','" + (Convert.ToDateTime(row["createdTime"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "','" + row["Creater"].ToString() + "','" + row["Changer"].ToString() + "')";
                DBAccess1.ExecSql(Conn4, sql);
                #endregion
                #region Insert到十个铜钱
                sql = string.Empty;
                sql = sql + "insert into AA_Inventory (isSale,isMadeSelf,code,name,shorthand,specification,isLimitedWithdraw,isBatch,isQualityPeriod,isPurchase,isMaterial,disabled,isQualityCheck,";
                sql = sql + "isMadeRequest,isSingleUnit,ts,updatedBy,Userfreeitem7,Userfreeitem6,Userfreeitem2,Userfreeitem1,Userfreeitem9,Userfreeitem0,Userfreeitem8,Userfreeitem5,Userfreeitem4,Userfreeitem3,MustInputFreeitem7,";
                sql = sql + " MustInputFreeitem2,MustInputFreeitem6,MustInputFreeitem3,MustInputFreeitem5,MustInputFreeitem4,MustInputFreeitem9,MustInputFreeitem1,MustInputFreeitem8,MustInputFreeitem0,HasEverChanged,isphantom,";
                sql = sql + "ControlRangeFreeitem0,ControlRangeFreeitem1,ControlRangeFreeitem2,ControlRangeFreeitem3,ControlRangeFreeitem4,ControlRangeFreeitem5,ControlRangeFreeitem6,ControlRangeFreeitem7,ControlRangeFreeitem8,";
                sql = sql + "ControlRangeFreeitem9,IsLaborCost,IsNew,MadeRecordDate,IsSuite,IsWeigh,idinventoryclass,idMarketingOrgan,idunit,idunitbymanufacture,idUnitByPurchase,idUnitByRetail,idUnitBySale,idUnitByStock,taxRate,";
                sql = sql + "unittype,valueType,madeDate,updated,createdTime,Creater,Changer) values (" + row["isSale"].ToString() + "," + row["isMadeSelf"].ToString() + ",'" + row["code"].ToString() + "','" + row["name"].ToString() + "','" + row["shorthand"].ToString() + "','" + row["specification"].ToString() + "',";
                sql = sql + "" + row["isLimitedWithdraw"].ToString() + "," + row["isBatch"].ToString() + "," + row["isQualityPeriod"].ToString() + ",";
                sql = sql + "" + row["isPurchase"].ToString() + "," + row["isMaterial"].ToString() + "," + row["disabled"].ToString() + "," + row["isQualityCheck"].ToString() + "," + row["isMadeRequest"].ToString() + "," + row["isSingleUnit"].ToString() + ",";
                sql = sql + "DEFAULT,'" + row["updatedBy"].ToString() + "'," + row["Userfreeitem7"].ToString() + "," + row["Userfreeitem6"].ToString() + "," + row["Userfreeitem2"].ToString() + "," + row["Userfreeitem1"].ToString() + "," + row["Userfreeitem9"].ToString() + "";
                sql = sql + "," + row["Userfreeitem0"].ToString() + "," + row["Userfreeitem8"].ToString() + "," + row["Userfreeitem5"].ToString() + "," + row["Userfreeitem4"].ToString() + "," + row["Userfreeitem3"].ToString() + ",";
                sql = sql + "" + row["MustInputFreeitem7"].ToString() + "," + row["MustInputFreeitem2"].ToString() + "," + row["MustInputFreeitem6"].ToString() + "," + row["MustInputFreeitem3"].ToString() + "," + row["MustInputFreeitem5"].ToString() + "";
                sql = sql + "," + row["MustInputFreeitem4"].ToString() + "," + row["MustInputFreeitem9"].ToString() + "," + row["MustInputFreeitem1"].ToString() + ",";
                sql = sql + "" + row["MustInputFreeitem8"].ToString() + "," + row["MustInputFreeitem0"].ToString() + "";
                sql = sql + ",'" + row["HasEverChanged"].ToString() + "'," + row["isphantom"].ToString() + "," + row["ControlRangeFreeitem0"].ToString() + "," + row["ControlRangeFreeitem1"].ToString() + ",";
                sql = sql + "" + row["ControlRangeFreeitem2"].ToString() + "," + row["ControlRangeFreeitem3"].ToString() + "," + row["ControlRangeFreeitem4"].ToString() + "," + row["ControlRangeFreeitem5"].ToString() + "," + row["ControlRangeFreeitem6"].ToString() + "," + row["ControlRangeFreeitem7"].ToString() + "," + row["ControlRangeFreeitem8"].ToString() + "," + row["ControlRangeFreeitem9"].ToString() + ",";
                sql = sql + "" + row["IslaborCost"].ToString() + "," + row["IsNew"].ToString() + ",'" + (Convert.ToDateTime(row["MadeRecordDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "'," + row["IsSuite"] + "," + row["IsWeigh"].ToString() + ",";
                sql = sql + "" + idinventoryclass5 + "," + idMarketingOrgan5 + "," + idunit5 + "," + idunitbymanufacture5 + "," + idUnitByPurchase5 + ",";
                sql = sql + "" + idUnitByRetail5 + "," + idUnitBySale5 + "," + idUnitByStock5 + ",";
                sql = sql + "" + row["taxRate"].ToString() + "," + row["unittype"].ToString() + "," + row["valueType"].ToString() + ",'" + (Convert.ToDateTime(row["madeDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "','" + (Convert.ToDateTime(row["updated"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "','" + (Convert.ToDateTime(row["createdTime"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "','" + row["Creater"].ToString() + "','" + row["Changer"].ToString() + "')";
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
                string idinventoryclass2 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idinventoryclass"].ToString(), "AA_InventoryClass", Conn1), Conn2, "AA_InventoryClass");
                string idinventoryclass3 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idinventoryclass"].ToString(), "AA_InventoryClass", Conn1), Conn3, "AA_InventoryClass");
                string idinventoryclass4 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idinventoryclass"].ToString(), "AA_InventoryClass", Conn1), Conn4, "AA_InventoryClass");
                string idinventoryclass5 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idinventoryclass"].ToString(), "AA_InventoryClass", Conn1), Conn5, "AA_InventoryClass");

                string idMarketingOrgan2 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idMarketingOrgan"].ToString(), "AA_MarketingOrgan", Conn1), Conn2, "AA_MarketingOrgan");
                string idMarketingOrgan3 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idMarketingOrgan"].ToString(), "AA_MarketingOrgan", Conn1), Conn3, "AA_MarketingOrgan");
                string idMarketingOrgan4 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idMarketingOrgan"].ToString(), "AA_MarketingOrgan", Conn1), Conn4, "AA_MarketingOrgan");
                string idMarketingOrgan5 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idMarketingOrgan"].ToString(), "AA_MarketingOrgan", Conn1), Conn5, "AA_MarketingOrgan");

                string idunit2 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idunit"].ToString(), "AA_Unit", Conn1), Conn2, "AA_Unit");
                string idunit3 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idunit"].ToString(), "AA_Unit", Conn1), Conn3, "AA_Unit");
                string idunit4 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idunit"].ToString(), "AA_Unit", Conn1), Conn4, "AA_Unit");
                string idunit5 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idunit"].ToString(), "AA_Unit", Conn1), Conn5, "AA_Unit");

                string idunitbymanufacture2 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idunitbymanufacture"].ToString(), "AA_Unit", Conn1), Conn2, "AA_Unit");
                string idunitbymanufacture3 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idunitbymanufacture"].ToString(), "AA_Unit", Conn1), Conn3, "AA_Unit");
                string idunitbymanufacture4 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idunitbymanufacture"].ToString(), "AA_Unit", Conn1), Conn4, "AA_Unit");
                string idunitbymanufacture5 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idunitbymanufacture"].ToString(), "AA_Unit", Conn1), Conn5, "AA_Unit");

                string idUnitByPurchase2 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitByPurchase"].ToString(), "AA_Unit", Conn1), Conn2, "AA_Unit");
                string idUnitByPurchase3 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitByPurchase"].ToString(), "AA_Unit", Conn1), Conn3, "AA_Unit");
                string idUnitByPurchase4 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitByPurchase"].ToString(), "AA_Unit", Conn1), Conn4, "AA_Unit");
                string idUnitByPurchase5 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitByPurchase"].ToString(), "AA_Unit", Conn1), Conn5, "AA_Unit");

                string idUnitByRetail2 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitByRetail"].ToString(), "AA_Unit", Conn1), Conn2, "AA_Unit");
                string idUnitByRetail3 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitByRetail"].ToString(), "AA_Unit", Conn1), Conn3, "AA_Unit");
                string idUnitByRetail4 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitByRetail"].ToString(), "AA_Unit", Conn1), Conn4, "AA_Unit");
                string idUnitByRetail5 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitByRetail"].ToString(), "AA_Unit", Conn1), Conn5, "AA_Unit");

                string idUnitBySale2 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitBySale"].ToString(), "AA_Unit", Conn1), Conn2, "AA_Unit");
                string idUnitBySale3 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitBySale"].ToString(), "AA_Unit", Conn1), Conn3, "AA_Unit");
                string idUnitBySale4 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitBySale"].ToString(), "AA_Unit", Conn1), Conn4, "AA_Unit");
                string idUnitBySale5 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitBySale"].ToString(), "AA_Unit", Conn1), Conn5, "AA_Unit");

                string idUnitByStock2 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitByStock"].ToString(), "AA_Unit", Conn1), Conn2, "AA_Unit");
                string idUnitByStock3 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitByStock"].ToString(), "AA_Unit", Conn1), Conn3, "AA_Unit");
                string idUnitByStock4 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitByStock"].ToString(), "AA_Unit", Conn1), Conn4, "AA_Unit");
                string idUnitByStock5 = GetidByCodeFormOtherBook(GetCodeByIDFromThisBook(row["idUnitByStock"].ToString(), "AA_Unit", Conn1), Conn5, "AA_Unit");
                #endregion
                string sql;
                #region Update到皇厨主楼
                sql = string.Empty;
                sql = sql + "update AA_Inventory set code='" + row["code"].ToString() + "',name='" + row["name"].ToString() + "',shorthand='" + row["shorthand"].ToString() + "',";
                sql = sql + "specification = '" + row["specification"].ToString() + "',";
                sql = sql + "isLimitedWithdraw = " + row["isLimitedWithdraw"].ToString() + ",";
                sql = sql + "isBatch = " + row["isBatch"].ToString() + ",isQualityPeriod = " + row["isQualityPeriod"].ToString() + ",isSale = " + row["isSale"].ToString() + ",";
                sql = sql + "isMadeSelf = " + row["isMadeSelf"].ToString() + ",isPurchase = " + row["isPurchase"].ToString() + ",isMaterial = " + row["isMaterial"].ToString() + ",";
                sql = sql + "disabled = " + row["disabled"].ToString() + ",isQualityCheck = " + row["isQualityCheck"].ToString() + ",";
                sql = sql + "isMadeRequest = " + row["isMadeRequest"].ToString() + ",isSingleUnit = " + row["isSingleUnit"].ToString() + ",";
                sql = sql + "updatedBy = '" + row["updatedBy"].ToString() + "',Userfreeitem7 = " + row["Userfreeitem7"].ToString() + ",Userfreeitem6 = " + row["Userfreeitem6"].ToString() + ",";
                sql = sql + "Userfreeitem2 = " + row["Userfreeitem2"].ToString() + ",Userfreeitem1 = " + row["Userfreeitem1"].ToString() + ",Userfreeitem9 = " + row["Userfreeitem9"].ToString() + ",";
                sql = sql + "Userfreeitem0 = " + row["Userfreeitem0"].ToString() + ",Userfreeitem8 = " + row["Userfreeitem8"].ToString() + ",Userfreeitem5 = " + row["Userfreeitem5"].ToString() + ",";
                sql = sql + "Userfreeitem4 = " + row["Userfreeitem4"].ToString() + ",Userfreeitem3 = " + row["Userfreeitem3"].ToString() + ",MustInputFreeitem7 = " + row["MustInputFreeitem7"].ToString() + ",";
                sql = sql + "MustInputFreeitem2 = " + row["MustInputFreeitem2"].ToString() + ",MustInputFreeitem6 = " + row["MustInputFreeitem6"].ToString() + ",";
                sql = sql + "MustInputFreeitem3 = " + row["MustInputFreeitem3"].ToString() + ",MustInputFreeitem5 = " + row["MustInputFreeitem5"].ToString() + ",";
                sql = sql + "MustInputFreeitem4 = " + row["MustInputFreeitem4"].ToString() + ",MustInputFreeitem9 = " + row["MustInputFreeitem9"].ToString() + ",";
                sql = sql + "MustInputFreeitem1 = " + row["MustInputFreeitem1"].ToString() + ",MustInputFreeitem8 = " + row["MustInputFreeitem8"].ToString() + ",";
                sql = sql + "MustInputFreeitem0 = " + row["MustInputFreeitem0"].ToString() + ",";
                sql = sql + "HasEverChanged = '" + row["HasEverChanged"].ToString() + "',";
                sql = sql + "isphantom = " + row["isphantom"].ToString() + ",ControlRangeFreeitem0 = " + row["ControlRangeFreeitem0"].ToString() + ",";
                sql = sql + "ControlRangeFreeitem1 = " + row["ControlRangeFreeitem1"].ToString() + ",ControlRangeFreeitem2 = " + row["ControlRangeFreeitem2"].ToString() + ",";
                sql = sql + "ControlRangeFreeitem3 = " + row["ControlRangeFreeitem3"].ToString() + ",ControlRangeFreeitem4 = " + row["ControlRangeFreeitem4"].ToString() + ",";
                sql = sql + "ControlRangeFreeitem5 = " + row["ControlRangeFreeitem5"].ToString() + ",ControlRangeFreeitem6 = " + row["ControlRangeFreeitem6"].ToString() + ",";
                sql = sql + "ControlRangeFreeitem7 = " + row["ControlRangeFreeitem7"].ToString() + ",ControlRangeFreeitem8 = " + row["ControlRangeFreeitem8"].ToString() + ",";
                sql = sql + "ControlRangeFreeitem9 = " + row["ControlRangeFreeitem9"].ToString() + ",IsLaborCost = " + row["IsLaborCost"].ToString() + ",";
                sql = sql + "IsNew = " + row["IsNew"].ToString() + ",MadeRecordDate = '" + (Convert.ToDateTime(row["MadeRecordDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "',";
                sql = sql + "IsSuite = " + row["IsSuite"].ToString() + ",";
                sql = sql + "IsWeigh = " + row["IsWeigh"].ToString() + ",";
                sql = sql + "idinventoryclass = " + idinventoryclass2 + ",idMarketingOrgan = " + idMarketingOrgan2 + ",";
                sql = sql + "idunit = " + idunit2 + ",idunitbymanufacture = " + idunitbymanufacture2 + ",";
                sql = sql + "idUnitByPurchase = " + idUnitByPurchase2 + ",idUnitByRetail = " + idUnitByRetail2 + ",idUnitBySale = " + idUnitBySale2 + ",";
                sql = sql + "idUnitByStock = " + idUnitByStock2 + ",";
                sql = sql + "taxRate = " + row["taxRate"].ToString() + ",unittype = " + row["unittype"].ToString() + ",";
                sql = sql + "valueType = " + row["valueType"].ToString() + ",madeDate = '" + (Convert.ToDateTime(row["madeDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "',updated = '" + (Convert.ToDateTime(row["updated"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "',createdTime = '" + (Convert.ToDateTime(row["createdTime"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "',";
                sql = sql + "Creater = '" + row["Creater"].ToString() + "',Changer = '" + row["Changer"].ToString() + "',Changedate = '" + (Convert.ToDateTime(row["Changedate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "' where code = '" + row["code"].ToString() + "'";
                DBAccess1.ExecSql(Conn2, sql);
                #endregion
                #region Update到皇厨附楼
                sql = string.Empty;
                sql = sql + "update AA_Inventory set code='" + row["code"].ToString() + "',name='" + row["name"].ToString() + "',shorthand='" + row["shorthand"].ToString() + "',";
                sql = sql + "specification = '" + row["specification"].ToString() + "',";
                sql = sql + "isLimitedWithdraw = " + row["isLimitedWithdraw"].ToString() + ",";
                sql = sql + "isBatch = " + row["isBatch"].ToString() + ",isQualityPeriod = " + row["isQualityPeriod"].ToString() + ",isSale = " + row["isSale"].ToString() + ",";
                sql = sql + "isMadeSelf = " + row["isMadeSelf"].ToString() + ",isPurchase = " + row["isPurchase"].ToString() + ",isMaterial = " + row["isMaterial"].ToString() + ",";
                sql = sql + "disabled = " + row["disabled"].ToString() + ",isQualityCheck = " + row["isQualityCheck"].ToString() + ",";
                sql = sql + "isMadeRequest = " + row["isMadeRequest"].ToString() + ",isSingleUnit = " + row["isSingleUnit"].ToString() + ",";
                sql = sql + "updatedBy = '" + row["updatedBy"].ToString() + "',Userfreeitem7 = " + row["Userfreeitem7"].ToString() + ",Userfreeitem6 = " + row["Userfreeitem6"].ToString() + ",";
                sql = sql + "Userfreeitem2 = " + row["Userfreeitem2"].ToString() + ",Userfreeitem1 = " + row["Userfreeitem1"].ToString() + ",Userfreeitem9 = " + row["Userfreeitem9"].ToString() + ",";
                sql = sql + "Userfreeitem0 = " + row["Userfreeitem0"].ToString() + ",Userfreeitem8 = " + row["Userfreeitem8"].ToString() + ",Userfreeitem5 = " + row["Userfreeitem5"].ToString() + ",";
                sql = sql + "Userfreeitem4 = " + row["Userfreeitem4"].ToString() + ",Userfreeitem3 = " + row["Userfreeitem3"].ToString() + ",MustInputFreeitem7 = " + row["MustInputFreeitem7"].ToString() + ",";
                sql = sql + "MustInputFreeitem2 = " + row["MustInputFreeitem2"].ToString() + ",MustInputFreeitem6 = " + row["MustInputFreeitem6"].ToString() + ",";
                sql = sql + "MustInputFreeitem3 = " + row["MustInputFreeitem3"].ToString() + ",MustInputFreeitem5 = " + row["MustInputFreeitem5"].ToString() + ",";
                sql = sql + "MustInputFreeitem4 = " + row["MustInputFreeitem4"].ToString() + ",MustInputFreeitem9 = " + row["MustInputFreeitem9"].ToString() + ",";
                sql = sql + "MustInputFreeitem1 = " + row["MustInputFreeitem1"].ToString() + ",MustInputFreeitem8 = " + row["MustInputFreeitem8"].ToString() + ",";
                sql = sql + "MustInputFreeitem0 = " + row["MustInputFreeitem0"].ToString() + ",";
                sql = sql + "HasEverChanged = '" + row["HasEverChanged"].ToString() + "',";
                sql = sql + "isphantom = " + row["isphantom"].ToString() + ",ControlRangeFreeitem0 = " + row["ControlRangeFreeitem0"].ToString() + ",";
                sql = sql + "ControlRangeFreeitem1 = " + row["ControlRangeFreeitem1"].ToString() + ",ControlRangeFreeitem2 = " + row["ControlRangeFreeitem2"].ToString() + ",";
                sql = sql + "ControlRangeFreeitem3 = " + row["ControlRangeFreeitem3"].ToString() + ",ControlRangeFreeitem4 = " + row["ControlRangeFreeitem4"].ToString() + ",";
                sql = sql + "ControlRangeFreeitem5 = " + row["ControlRangeFreeitem5"].ToString() + ",ControlRangeFreeitem6 = " + row["ControlRangeFreeitem6"].ToString() + ",";
                sql = sql + "ControlRangeFreeitem7 = " + row["ControlRangeFreeitem7"].ToString() + ",ControlRangeFreeitem8 = " + row["ControlRangeFreeitem8"].ToString() + ",";
                sql = sql + "ControlRangeFreeitem9 = " + row["ControlRangeFreeitem9"].ToString() + ",IsLaborCost = " + row["IsLaborCost"].ToString() + ",";
                sql = sql + "IsNew = " + row["IsNew"].ToString() + ",MadeRecordDate = '" + (Convert.ToDateTime(row["MadeRecordDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "',";
                sql = sql + "IsSuite = " + row["IsSuite"].ToString() + ",";
                sql = sql + "IsWeigh = " + row["IsWeigh"].ToString() + ",";
                sql = sql + "idinventoryclass = " + idinventoryclass3 + ",idMarketingOrgan = " + idMarketingOrgan3 + ",";
                sql = sql + "idunit = " + idunit3 + ",idunitbymanufacture = " + idunitbymanufacture3 + ",";
                sql = sql + "idUnitByPurchase = " + idUnitByPurchase3 + ",idUnitByRetail = " + idUnitByRetail3 + ",idUnitBySale = " + idUnitBySale3 + ",";
                sql = sql + "idUnitByStock = " + idUnitByStock3 + ",";
                sql = sql + "taxRate = " + row["taxRate"].ToString() + ",unittype = " + row["unittype"].ToString() + ",";
                sql = sql + "valueType = " + row["valueType"].ToString() + ",madeDate = '" + (Convert.ToDateTime(row["madeDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "',updated = '" + (Convert.ToDateTime(row["updated"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "',createdTime = '" + (Convert.ToDateTime(row["createdTime"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "',";
                sql = sql + "Creater = '" + row["Creater"].ToString() + "',Changer = '" + row["Changer"].ToString() + "',Changedate = '" + (Convert.ToDateTime(row["Changedate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "' where code = '" + row["code"].ToString() + "'";
                DBAccess1.ExecSql(Conn3, sql);
                #endregion
                #region Update到荣升
                sql = string.Empty;
                sql = sql + "update AA_Inventory set code='" + row["code"].ToString() + "',name='" + row["name"].ToString() + "',shorthand='" + row["shorthand"].ToString() + "',";
                sql = sql + "specification = '" + row["specification"].ToString() + "',";
                sql = sql + "isLimitedWithdraw = " + row["isLimitedWithdraw"].ToString() + ",";
                sql = sql + "isBatch = " + row["isBatch"].ToString() + ",isQualityPeriod = " + row["isQualityPeriod"].ToString() + ",isSale = " + row["isSale"].ToString() + ",";
                sql = sql + "isMadeSelf = " + row["isMadeSelf"].ToString() + ",isPurchase = " + row["isPurchase"].ToString() + ",isMaterial = " + row["isMaterial"].ToString() + ",";
                sql = sql + "disabled = " + row["disabled"].ToString() + ",isQualityCheck = " + row["isQualityCheck"].ToString() + ",";
                sql = sql + "isMadeRequest = " + row["isMadeRequest"].ToString() + ",isSingleUnit = " + row["isSingleUnit"].ToString() + ",";
                sql = sql + "updatedBy = '" + row["updatedBy"].ToString() + "',Userfreeitem7 = " + row["Userfreeitem7"].ToString() + ",Userfreeitem6 = " + row["Userfreeitem6"].ToString() + ",";
                sql = sql + "Userfreeitem2 = " + row["Userfreeitem2"].ToString() + ",Userfreeitem1 = " + row["Userfreeitem1"].ToString() + ",Userfreeitem9 = " + row["Userfreeitem9"].ToString() + ",";
                sql = sql + "Userfreeitem0 = " + row["Userfreeitem0"].ToString() + ",Userfreeitem8 = " + row["Userfreeitem8"].ToString() + ",Userfreeitem5 = " + row["Userfreeitem5"].ToString() + ",";
                sql = sql + "Userfreeitem4 = " + row["Userfreeitem4"].ToString() + ",Userfreeitem3 = " + row["Userfreeitem3"].ToString() + ",MustInputFreeitem7 = " + row["MustInputFreeitem7"].ToString() + ",";
                sql = sql + "MustInputFreeitem2 = " + row["MustInputFreeitem2"].ToString() + ",MustInputFreeitem6 = " + row["MustInputFreeitem6"].ToString() + ",";
                sql = sql + "MustInputFreeitem3 = " + row["MustInputFreeitem3"].ToString() + ",MustInputFreeitem5 = " + row["MustInputFreeitem5"].ToString() + ",";
                sql = sql + "MustInputFreeitem4 = " + row["MustInputFreeitem4"].ToString() + ",MustInputFreeitem9 = " + row["MustInputFreeitem9"].ToString() + ",";
                sql = sql + "MustInputFreeitem1 = " + row["MustInputFreeitem1"].ToString() + ",MustInputFreeitem8 = " + row["MustInputFreeitem8"].ToString() + ",";
                sql = sql + "MustInputFreeitem0 = " + row["MustInputFreeitem0"].ToString() + ",";
                sql = sql + "HasEverChanged = '" + row["HasEverChanged"].ToString() + "',";
                sql = sql + "isphantom = " + row["isphantom"].ToString() + ",ControlRangeFreeitem0 = " + row["ControlRangeFreeitem0"].ToString() + ",";
                sql = sql + "ControlRangeFreeitem1 = " + row["ControlRangeFreeitem1"].ToString() + ",ControlRangeFreeitem2 = " + row["ControlRangeFreeitem2"].ToString() + ",";
                sql = sql + "ControlRangeFreeitem3 = " + row["ControlRangeFreeitem3"].ToString() + ",ControlRangeFreeitem4 = " + row["ControlRangeFreeitem4"].ToString() + ",";
                sql = sql + "ControlRangeFreeitem5 = " + row["ControlRangeFreeitem5"].ToString() + ",ControlRangeFreeitem6 = " + row["ControlRangeFreeitem6"].ToString() + ",";
                sql = sql + "ControlRangeFreeitem7 = " + row["ControlRangeFreeitem7"].ToString() + ",ControlRangeFreeitem8 = " + row["ControlRangeFreeitem8"].ToString() + ",";
                sql = sql + "ControlRangeFreeitem9 = " + row["ControlRangeFreeitem9"].ToString() + ",IsLaborCost = " + row["IsLaborCost"].ToString() + ",";
                sql = sql + "IsNew = " + row["IsNew"].ToString() + ",MadeRecordDate = '" + (Convert.ToDateTime(row["MadeRecordDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "',";
                sql = sql + "IsSuite = " + row["IsSuite"].ToString() + ",";
                sql = sql + "IsWeigh = " + row["IsWeigh"].ToString() + ",";
                sql = sql + "idinventoryclass = " + idinventoryclass4 + ",idMarketingOrgan = " + idMarketingOrgan4 + ",";
                sql = sql + "idunit = " + idunit4 + ",idunitbymanufacture = " + idunitbymanufacture4 + ",";
                sql = sql + "idUnitByPurchase = " + idUnitByPurchase4 + ",idUnitByRetail = " + idUnitByRetail4 + ",idUnitBySale = " + idUnitBySale4 + ",";
                sql = sql + "idUnitByStock = " + idUnitByStock4 + ",";
                sql = sql + "taxRate = " + row["taxRate"].ToString() + ",unittype = " + row["unittype"].ToString() + ",";
                sql = sql + "valueType = " + row["valueType"].ToString() + ",madeDate = '" + (Convert.ToDateTime(row["madeDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "',updated = '" + (Convert.ToDateTime(row["updated"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "',createdTime = '" + (Convert.ToDateTime(row["createdTime"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "',";
                sql = sql + "Creater = '" + row["Creater"].ToString() + "',Changer = '" + row["Changer"].ToString() + "',Changedate = '" + (Convert.ToDateTime(row["Changedate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "' where code = '" + row["code"].ToString() + "'";
                DBAccess1.ExecSql(Conn4, sql);
                #endregion
                #region Update到十个铜钱
                sql = string.Empty;
                sql = sql + "update AA_Inventory set code='" + row["code"].ToString() + "',name='" + row["name"].ToString() + "',shorthand='" + row["shorthand"].ToString() + "',";
                sql = sql + "specification = '" + row["specification"].ToString() + "',";
                sql = sql + "isLimitedWithdraw = " + row["isLimitedWithdraw"].ToString() + ",";
                sql = sql + "isBatch = " + row["isBatch"].ToString() + ",isQualityPeriod = " + row["isQualityPeriod"].ToString() + ",isSale = " + row["isSale"].ToString() + ",";
                sql = sql + "isMadeSelf = " + row["isMadeSelf"].ToString() + ",isPurchase = " + row["isPurchase"].ToString() + ",isMaterial = " + row["isMaterial"].ToString() + ",";
                sql = sql + "disabled = " + row["disabled"].ToString() + ",isQualityCheck = " + row["isQualityCheck"].ToString() + ",";
                sql = sql + "isMadeRequest = " + row["isMadeRequest"].ToString() + ",isSingleUnit = " + row["isSingleUnit"].ToString() + ",";
                sql = sql + "updatedBy = '" + row["updatedBy"].ToString() + "',Userfreeitem7 = " + row["Userfreeitem7"].ToString() + ",Userfreeitem6 = " + row["Userfreeitem6"].ToString() + ",";
                sql = sql + "Userfreeitem2 = " + row["Userfreeitem2"].ToString() + ",Userfreeitem1 = " + row["Userfreeitem1"].ToString() + ",Userfreeitem9 = " + row["Userfreeitem9"].ToString() + ",";
                sql = sql + "Userfreeitem0 = " + row["Userfreeitem0"].ToString() + ",Userfreeitem8 = " + row["Userfreeitem8"].ToString() + ",Userfreeitem5 = " + row["Userfreeitem5"].ToString() + ",";
                sql = sql + "Userfreeitem4 = " + row["Userfreeitem4"].ToString() + ",Userfreeitem3 = " + row["Userfreeitem3"].ToString() + ",MustInputFreeitem7 = " + row["MustInputFreeitem7"].ToString() + ",";
                sql = sql + "MustInputFreeitem2 = " + row["MustInputFreeitem2"].ToString() + ",MustInputFreeitem6 = " + row["MustInputFreeitem6"].ToString() + ",";
                sql = sql + "MustInputFreeitem3 = " + row["MustInputFreeitem3"].ToString() + ",MustInputFreeitem5 = " + row["MustInputFreeitem5"].ToString() + ",";
                sql = sql + "MustInputFreeitem4 = " + row["MustInputFreeitem4"].ToString() + ",MustInputFreeitem9 = " + row["MustInputFreeitem9"].ToString() + ",";
                sql = sql + "MustInputFreeitem1 = " + row["MustInputFreeitem1"].ToString() + ",MustInputFreeitem8 = " + row["MustInputFreeitem8"].ToString() + ",";
                sql = sql + "MustInputFreeitem0 = " + row["MustInputFreeitem0"].ToString() + ",";
                sql = sql + "HasEverChanged = '" + row["HasEverChanged"].ToString() + "',";
                sql = sql + "isphantom = " + row["isphantom"].ToString() + ",ControlRangeFreeitem0 = " + row["ControlRangeFreeitem0"].ToString() + ",";
                sql = sql + "ControlRangeFreeitem1 = " + row["ControlRangeFreeitem1"].ToString() + ",ControlRangeFreeitem2 = " + row["ControlRangeFreeitem2"].ToString() + ",";
                sql = sql + "ControlRangeFreeitem3 = " + row["ControlRangeFreeitem3"].ToString() + ",ControlRangeFreeitem4 = " + row["ControlRangeFreeitem4"].ToString() + ",";
                sql = sql + "ControlRangeFreeitem5 = " + row["ControlRangeFreeitem5"].ToString() + ",ControlRangeFreeitem6 = " + row["ControlRangeFreeitem6"].ToString() + ",";
                sql = sql + "ControlRangeFreeitem7 = " + row["ControlRangeFreeitem7"].ToString() + ",ControlRangeFreeitem8 = " + row["ControlRangeFreeitem8"].ToString() + ",";
                sql = sql + "ControlRangeFreeitem9 = " + row["ControlRangeFreeitem9"].ToString() + ",IsLaborCost = " + row["IsLaborCost"].ToString() + ",";
                sql = sql + "IsNew = " + row["IsNew"].ToString() + ",MadeRecordDate = '" + (Convert.ToDateTime(row["MadeRecordDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "',";
                sql = sql + "IsSuite = " + row["IsSuite"].ToString() + ",";
                sql = sql + "IsWeigh = " + row["IsWeigh"].ToString() + ",";
                sql = sql + "idinventoryclass = " + idinventoryclass5 + ",idMarketingOrgan = " + idMarketingOrgan5 + ",";
                sql = sql + "idunit = " + idunit5 + ",idunitbymanufacture = " + idunitbymanufacture5 + ",";
                sql = sql + "idUnitByPurchase = " + idUnitByPurchase5 + ",idUnitByRetail = " + idUnitByRetail5 + ",idUnitBySale = " + idUnitBySale5 + ",";
                sql = sql + "idUnitByStock = " + idUnitByStock5 + ",";
                sql = sql + "taxRate = " + row["taxRate"].ToString() + ",unittype = " + row["unittype"].ToString() + ",";
                sql = sql + "valueType = " + row["valueType"].ToString() + ",madeDate = '" + (Convert.ToDateTime(row["madeDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "',updated = '" + (Convert.ToDateTime(row["updated"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "',createdTime = '" + (Convert.ToDateTime(row["createdTime"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "',";
                sql = sql + "Creater = '" + row["Creater"].ToString() + "',Changer = '" + row["Changer"].ToString() + "',Changedate = '" + (Convert.ToDateTime(row["Changedate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss")) + "' where code = '" + row["code"].ToString() + "'";
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
        /// 根据Code获取id
        /// </summary>
        /// <param name="id">当前记录的ID</param>
        /// <param name="table">表名</param>
        /// <param name="conn">帐套连接字符串</param>
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
                string sql = "select * from SyncLog where IsSync='0' and SyncName='_SyncInventory'";
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
