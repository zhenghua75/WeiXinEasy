using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.MiniShop;
using Model.MiniShop;
using Mozart.Common;

namespace Mozart.PalmShop.ShopCode
{
    public partial class ChargeOrder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strOpenID = string.Empty;

                if (null == Request.QueryString["openid"])
                {
                    return;
                }
                else
                {
                    if (Request.QueryString["openid"].ToString().Length > 29 || Request.QueryString["openid"].ToString().Length < 25)
                    {
                        strOpenID = Common.Common.NoHtml(Request.QueryString["openid"].ToString());
                    }
                    else
                    {
                        strOpenID = Request.QueryString["openid"].ToString();
                    }
                    Session["openid"] = strOpenID;
                }

                //取所有参加的活动列表
                List<MSChargeOrder> liPo = new List<MSChargeOrder>();
                MSProductOrderDAL dalPo = new MSProductOrderDAL();
                DataSet ds = dalPo.GetPOListByOpid(strOpenID);

                if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        MSChargeOrder model = DataConvert.DataRowToModel<MSChargeOrder>(row);
                        liPo.Add(model);
                    }
                }

                string text = System.IO.File.ReadAllText(Server.MapPath("../ShopPage/ChargeOrder.html"));
                JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();
                context.TempData["Po_list"] = liPo;
                context.TempData["OpenID"] = strOpenID;
                JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
                t.Render(Response.Output);
            }
        }
    }
}