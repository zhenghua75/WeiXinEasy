using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.SYS;

namespace Mozart.HPPrint
{
    public partial class Index : System.Web.UI.Page
    {
        public string printimglist = string.Empty;
        public string codeimg = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["sitecode"] != null && Request["sitecode"] != "")
                {
                    getprintimglist();
                }
                else
                {
                    return;
                }
            }
        }
        void getprintimglist()
        {
            string sitecode = string.Empty;
            string name = string.Empty;
            string strcodeimg = string.Empty;
            string printimg1 = string.Empty;
            string printimg2 = string.Empty;
            string printimg3 = string.Empty;
            string printimg4 = string.Empty;
            sitecode=Common.Common.NoHtml(Request["sitecode"]);
            Account_ExtDAL dal = new Account_ExtDAL();
            DataSet ds = dal.GetPrintImgBySiteCode(sitecode);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                name = ds.Tables[0].Rows[0]["Name"].ToString();
                strcodeimg=ds.Tables[0].Rows[0]["CodeImg"].ToString();
                if(strcodeimg.Trim()!=null&&strcodeimg.Trim()!="")
                {
                    codeimg = "<img src=\"" + strcodeimg + "\" alt=\"" + name + "\" />";
                }
                printimg1 = ds.Tables[0].Rows[0]["PrintImg1"].ToString();
                printimg2 = ds.Tables[0].Rows[0]["PrintImg2"].ToString();
                printimg3 = ds.Tables[0].Rows[0]["PrintImg3"].ToString();
                printimg4 = ds.Tables[0].Rows[0]["PrintImg4"].ToString();
                if (printimg1.Trim() != null && printimg1.Trim() != "")
                {
                    printimg1 = "<img src=\"" + printimg1 + "\" alt=\"" + name + "\" />";
                }
                if (printimg2.Trim() != null && printimg2.Trim() != "")
                {
                    printimg2 = "<img src=\"" + printimg2 + "\" alt=\"" + name + "\" />";
                }
                if (printimg3.Trim() != null && printimg3.Trim() != "")
                {
                    printimg3 = "<img src=\"" + printimg3 + "\" alt=\"" + name + "\" />";
                }
                if (printimg4.Trim() != null && printimg4.Trim() != "")
                {
                    printimg4 = "<img src=\"" + printimg4 + "\" alt=\"" + name + "\" />";
                }
                printimglist = "\r\n" + printimg1 + "\r\n" + printimg2 + "\r\n" + printimg3 + "\r\n" + printimg4+"\r\n";
            }
            else {
                return;
            }
        }
    }
}