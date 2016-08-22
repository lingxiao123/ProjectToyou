/*----------------------------------------------------------------

           // 文件名：PortDataReciveEventArgs
           // 文件功能描述：重写PortDataReciveEventArgs参数类

//----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectToYou.Code
{
    public delegate void PortDataReceivedEventHandle(object sender, PortDataReciveEventArgs e);

    public class PortDataReciveEventArgs : EventArgs
    {
        private byte[] data;

        public byte[] Data
        {
            get { return data; }
            set { data = value; }
        }

        public PortDataReciveEventArgs()
        {
            this.data = null;
        }

        public PortDataReciveEventArgs(byte[] data)
        {
            this.data = data;
        }
    }
}