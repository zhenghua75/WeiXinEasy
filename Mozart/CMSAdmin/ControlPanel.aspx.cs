using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Threading;

namespace Mozart.CMSAdmin
{
    public partial class ControlPanel : System.Web.UI.Page
    {
        bool bGDateIsExit = false;
        public static string msghtml = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetMsgData));
            }
        }
        void GetMsgData(object strService)
        {
            SecHandProductTip.SecHandProductMsgSoapClient SecHandTip =
                               new SecHandProductTip.SecHandProductMsgSoapClient();
            Object o = new Object();
            lock (o)
            {
                if (!bGDateIsExit)
                {
                    try
                    {
                        msghtml = SecHandTip.SecHandProductTip();
                        Thread.Sleep(5000);
                    }
                    catch (Exception)
                    {
                        Thread.Sleep(20000);
                    }
                }
            }
        }
    }
}