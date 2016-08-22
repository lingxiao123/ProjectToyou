using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Configuration;
namespace ProjectToYou
{
    public partial class ImgWeb : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int str = Convert.ToInt32(string.Empty.Equals(ConfigurationManager.AppSettings["PrintFontSize"].ToString()) ? "20" : ConfigurationManager.AppSettings["PrintFontSize"].ToString());
            string strs = ConfigurationManager.AppSettings["PrintFont"].ToString();
            System.Drawing.Image tempImg = new Bitmap(280, 330);
            Graphics g = Graphics.FromImage(tempImg);
            g.DrawString(Session["PName"].ToString(), new Font(new FontFamily(strs), str), System.Drawing.Brushes.Black, Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow1X"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow1Y"].ToString()));
            g.DrawString(Session["PSize"].ToString(), new Font(new FontFamily(strs), str), System.Drawing.Brushes.Black, Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow2X"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow2Y"].ToString()));
            g.DrawString(Session["PGW"].ToString(), new Font(new FontFamily(strs), str), System.Drawing.Brushes.Black, Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow3X"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow3Y"].ToString()));
            g.DrawString(Session["PBW"].ToString(), new Font(new FontFamily(strs), str), System.Drawing.Brushes.Black, Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow4X"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow4Y"].ToString()));
            g.DrawString(Session["PNW"].ToString(), new Font(new FontFamily(strs), str), System.Drawing.Brushes.Black, Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow5X"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow5Y"].ToString()));
            g.DrawString(Session["PDate"].ToString(), new Font(new FontFamily(strs), str), System.Drawing.Brushes.Black, Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow6X"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow6Y"].ToString()));
            g.DrawString(Session["PSno"].ToString(), new Font(new FontFamily(strs), str), System.Drawing.Brushes.Black, Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow7X"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["PrintRow7Y"].ToString()));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            tempImg.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            Response.ClearContent();
            Response.ContentType = "text/html";
            Response.BinaryWrite(ms.ToArray());
        }
    }
}