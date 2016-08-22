
/*----------------------------------------------------------------

           // 文件名：SerialPortDao
           // 文件功能描述：封装串口组件，实现对串口的统一访问和操作

//----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

namespace ProjectToYou.Code
{
    /// <summary>
    /// 提供对串口的统一访问
    /// </summary>
    public sealed class SerialPortDao
    {
        #region 事件和字段定义
        public event PortDataReceivedEventHandle Received;
        public SerialPort serialPort = null;
        public bool ReceiveEventFlag = false;  //接收事件是否有效 false表示有效

        private static readonly SerialPortDao instance = new SerialPortDao();

        #endregion

        #region 属性定义
        private string protName;
        public string PortName
        {
            get { return serialPort.PortName; }
            set
            {
                serialPort.PortName = value;
                protName = value;
            }
        }
        #endregion

        #region 构造函数
        private SerialPortDao()
        {
            LoadSerialPort();
        }

        private void LoadSerialPort()
        {
            serialPort = new SerialPort();
            serialPort.BaudRate = 9600;
            serialPort.Parity = Parity.Even;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            serialPort.Handshake = Handshake.None;
            serialPort.RtsEnable = true;
            serialPort.ReadTimeout = 2000;
            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
        }


        #endregion

        #region 串口操作集合
        /// <summary>
        /// 返回串口对象的单个实例
        /// </summary>
        /// <returns></returns>
        public static SerialPortDao GetSerialPortDao()
        {
            return instance;
        }

        /// <summary>
        /// 释放串口资源
        /// </summary>
        ~SerialPortDao()
        {
            Close();
        }

        /// <summary>
        /// 打开串口
        /// </summary>
        public void Open()
        {
            try
            {
                if (!serialPort.IsOpen)
                {
                    serialPort.Open();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        public void Close()
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }

        /// <summary>
        /// 串口是否打开
        /// </summary>
        /// <returns></returns>
        public bool IsOpen()
        {
            return serialPort.IsOpen;
        }



        /// <summary>
        /// 数据发送
        /// </summary>
        /// <param name="data">要发送的数据字节</param>
        public void SendData(byte[] data)
        {
            try
            {
                serialPort.DiscardInBuffer();
                serialPort.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="SendData">发送数据</param>
        /// <param name="ReceiveData">接收数据</param>
        /// <param name="Overtime">超时时间</param>
        /// <returns></returns>
        public int SendCommand(byte[] SendData, ref byte[] ReceiveData, int Overtime)
        {
            if (serialPort.IsOpen)
            {
                try
                {
                    ReceiveEventFlag = true;        //关闭接收事件
                    serialPort.DiscardInBuffer();   //清空接收缓冲区                 
                    serialPort.Write(SendData, 0, SendData.Length);
                    System.Threading.Thread.Sleep(50);
                    int num = 0, ret = 0;
                    while (num++ < Overtime)
                    {
                        if (serialPort.BytesToRead >= ReceiveData.Length)
                        {
                            break;
                        }
                        System.Threading.Thread.Sleep(50);
                    }
                    if (serialPort.BytesToRead >= ReceiveData.Length)
                    {
                        ret = serialPort.Read(ReceiveData, 0, ReceiveData.Length);
                    }
                    else
                    {
                        ret = serialPort.Read(ReceiveData, 0, serialPort.BytesToRead);
                    }
                    ReceiveEventFlag = false;       //打开事件
                    return ret;
                }
                catch (Exception ex)
                {
                    ReceiveEventFlag = false;
                    throw ex;
                }
            }
            return -1;
        }

        ///<summary>
        ///数据发送
        ///</summary>
        ///<param name="data">要发送的数据字符串</param>
        public void SendData(string data)
        {
            //禁止接收事件时直接退出
            if (ReceiveEventFlag)
            {
                return;
            }
            if (serialPort.IsOpen)
            {
                serialPort.Write(data);
            }
        }

        ///<summary>
        ///将指定数量的字节写入输出缓冲区中的指定偏移量处。
        ///</summary>
        ///<param name="data">发送的字节数据</param>
        ///<param name="offset">写入偏移量</param>
        ///<param name="count">写入的字节数</param>
        public void SendData(byte[] data, int offset, int count)
        {
            //禁止接收事件时直接退出
            if (ReceiveEventFlag)
            {
                return;
            }
            if (serialPort.IsOpen)
            {
                serialPort.Write(data, offset, count);
            }
        }


        /// <summary>
        /// 数据接收
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //禁止接收事件时直接退出
            if (ReceiveEventFlag)
            {
                return;
            }

            try
            {
                byte[] data = new byte[serialPort.BytesToRead];
                serialPort.Read(data, 0, data.Length);
                if (Received != null)
                {
                    Received(sender, new PortDataReciveEventArgs(data));
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }
        #endregion
    }
}