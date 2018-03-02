using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DAL.MiniShop;

namespace Mozart.WebService
{
    /// <summary>
    /// SecHandProductMsg 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class SecHandProductMsg : System.Web.Services.WebService
    {
        /// <summary>
        /// 返回产品提示信息
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description="消息提醒")]
        public string SecHandProductTip()
        {
            int count = 0; string msg = string.Empty;
            MSProductDAL productDal=new MSProductDAL ();
            try
            {
                count = productDal.GetNotPassProductCount(" and IsSecHand=1 and Review=0 ");
            }
            catch (Exception)
            {
                count = 0;
            }
            msg = "";
            if (count > 0)
            {
                msg = "共有"+count+"条产品信息待审核！";
            }
            return msg;
        }
    }
}
