using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Data.Common;
using System.Collections;

namespace ServiceToYou
{
    public partial class Service1 : ServiceBase
    {

        public Service1()
        {
            InitializeComponent();
        }
        //public void OnStart()
        //{
        //    SetSyncHt();
        //    FalseStart();
        //    //AllWaysRun();
        //}
        protected override void OnStart(string[] args)
        {
            SetSyncHt();
            AllWaysRun();
        }
        protected override void OnStop()
        {
        }
        #region 变量
        //定义同步数量的数组
        Hashtable _SyncHt = new Hashtable();
        SyncCode.Sync_All _SynaAll = new SyncCode.Sync_All();
        SyncCode.SyncPosition _SyncPosition = new SyncCode.SyncPosition();
        SyncCode.SyncInventory _SyncInventory = new SyncCode.SyncInventory();
        SyncCode.SyncInventoryClass _SyncInventoryClass = new SyncCode.SyncInventoryClass();
        SyncCode.SyncPurchaseOrder _SyncPurchaseOrder = new SyncCode.SyncPurchaseOrder();
        #endregion
        #region 基础变量定义方法
        /// <summary>
        /// 将需要同步的内容逐一设定
        /// 使用HashTable
        /// Key：需要同步的功能
        /// Value：初始值为0，需要同步时设置成1
        /// </summary>
        private void SetSyncHt()
        {
            _SyncHt.Add("_SyncDep", 0);//部门
            _SyncHt.Add("_SyncPerson", 0);//员工
            _SyncHt.Add("_SyncPartner", 0);//往来单位
            _SyncHt.Add("_SyncUnit", 0);//计量单位
            _SyncHt.Add("_SyncInventory", 0);//存货
            _SyncHt.Add("_SyncInventoryClass", 0);//存货分类
            _SyncHt.Add("_SyncWarehouse", 0);//仓库
            _SyncHt.Add("_SyncRDStyle", 0);//出入库类别
            _SyncHt.Add("_SyncBankCount", 0);//帐号
            _SyncHt.Add("_SyncSettleStyle", 0);//结算方式
            _SyncHt.Add("_SyncAccount", 0);//科目
            _SyncHt.Add("_SyncAccountType", 0);//科目设置
            _SyncHt.Add("_SyncAsset", 0);//资产属性
            _SyncHt.Add("_SyncAssetClass", 0);//资产分类
            _SyncHt.Add("_SyncIncDec", 0);//增减方式
            _SyncHt.Add("_SyncUseStatus", 0);//使用状况
            _SyncHt.Add("_SyncHandleReason", 0);//处理原因
            _SyncHt.Add("_SyncPosition", 0);//存放位置
            _SyncHt.Add("_SyncBizUsage", 0);//经济用途
            _SyncHt.Add("_SyncSaleDelivery", 0);//销货单
            _SyncHt.Add("_SyncPurchaseOrder", 0);//采购订单
        }
        #endregion
        /// <summary>
        /// 设置定时器方法
        /// </summary>
        private void AllWaysRun()
        {
            System.Timers.Timer mt = new System.Timers.Timer(60000);
            mt.Elapsed += new System.Timers.ElapsedEventHandler(MTimedEvent);
            mt.Enabled = true;
        }
        /// <summary>
        /// 定时器方法体（同步功能的主入口）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MTimedEvent(object sender, ElapsedEventArgs e)
        {
            #region 检索需要同步的功能
            SetSyncValue();
            #endregion
            #region 进行同步
            StartSync();
            #endregion
        }
        private void FalseStart()
        {
            #region 检索需要同步的功能
            SetSyncValue();
            #endregion
            #region 进行同步
            StartSync();
            #endregion
        }
        private void StartSync()
        {
            foreach (DictionaryEntry ht in _SyncHt)
            {
                switch (ht.Key.ToString())
                {
                    case "_SyncDep"://部门
                        if (CheckIsNeedSync(ht.Value.ToString()))
                        {

                        }
                        break;
                    case "_SyncPerson"://员工
                        if (CheckIsNeedSync(ht.Value.ToString()))
                        {

                        }
                        break;
                    case "_SyncPartner"://往来单位
                        if (CheckIsNeedSync(ht.Value.ToString()))
                        {

                        }
                        break;
                    case "_SyncUnit"://计量单位
                        if (CheckIsNeedSync(ht.Value.ToString()))
                        {

                        }
                        break;
                    case "_SyncInventory"://存货
                        if (CheckIsNeedSync(ht.Value.ToString()))
                        {
                            _SyncInventory.DoSync();
                        }
                        break;
                    case "_SyncInventoryClass"://存货分类
                        if (CheckIsNeedSync(ht.Value.ToString()))
                        {
                            _SyncInventoryClass.DoSync();
                        }
                        break;
                    case "_SyncWarehouse"://仓库
                        if (CheckIsNeedSync(ht.Value.ToString()))
                        {

                        }
                        break;
                    case "_SyncRDStyle"://出入库类别
                        if (CheckIsNeedSync(ht.Value.ToString()))
                        {

                        }
                        break;
                    case "_SyncBankCount"://帐号
                        if (CheckIsNeedSync(ht.Value.ToString()))
                        {

                        }
                        break;
                    case "_SyncSettleStyle"://结算方式
                        if (CheckIsNeedSync(ht.Value.ToString()))
                        {

                        }
                        break;
                    case "_SyncAccount"://科目
                        if (CheckIsNeedSync(ht.Value.ToString()))
                        {

                        }
                        break;
                    case "_SyncAccountType"://科目设置
                        if (CheckIsNeedSync(ht.Value.ToString()))
                        {

                        }
                        break;
                    case "_SyncAsset"://资产属性
                        if (CheckIsNeedSync(ht.Value.ToString()))
                        {

                        }
                        break;
                    case "_SyncAssetClass"://资产分类
                        if (CheckIsNeedSync(ht.Value.ToString()))
                        {

                        }
                        break;
                    case "_SyncIncDec"://增减方式
                        if (CheckIsNeedSync(ht.Value.ToString()))
                        {

                        }
                        break;
                    case "_SyncUseStatus"://使用状况
                        if (CheckIsNeedSync(ht.Value.ToString()))
                        {

                        }
                        break;
                    case "_SyncHandleReason"://处理原因
                        if (CheckIsNeedSync(ht.Value.ToString()))
                        {

                        }
                        break;
                    case "_SyncPosition"://存放位置
                        if (CheckIsNeedSync(ht.Value.ToString()))
                        {
                            _SyncPosition.DoSync();
                        }
                        break;
                    case "_SyncBizUsage"://经济用途
                        if (CheckIsNeedSync(ht.Value.ToString()))
                        {

                        }
                        break;
                    case "_SyncSaleDelivery"://销货单
                        if (CheckIsNeedSync(ht.Value.ToString()))
                        {
                            _SyncPurchaseOrder.DoSync();
                        }
                        break;
                    case "_SyncPurchaseOrder"://采购订单
                        if (CheckIsNeedSync(ht.Value.ToString()))
                        {

                        }
                        break;
                    default: break;
                }

            }
        }
        /// <summary>
        /// 根据实际情况设置_SyncHt的值
        /// 0：不需要同步
        /// 1：需要同步
        /// </summary>
        private void SetSyncValue()
        {
            _SynaAll.SetHashTableForSync(ref _SyncHt);
        }
        /// 检索是否需要同步
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool CheckIsNeedSync(string str)
        {
            bool ReturnCode = false;
            if (str == "1")
                ReturnCode = true;
            return ReturnCode;
        }
    }
}
