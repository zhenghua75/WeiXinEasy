using DAL.WeiXin;
using Model.WeiXin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.Common
{
    public class WXHelper
    {
        /// <summary>
        /// 根据wxConfigId创建微信对象
        /// </summary>
        /// <param name="wxConfigId"></param>
        /// <returns></returns>
        public static WeiXinCore.WeiXin CreateWeiXinInstanceById(string wxConfigId)
        {
            WeiXinCore.WeiXin res=null;
            WXConfigDAL dal = new WXConfigDAL();
            WXConfig wxConfig = dal.GetWXConfigByID(wxConfigId);
            if (wxConfig != null)
            {
                WeiXinCore.Models.WeiXinConfig weixinConfig = new WeiXinCore.Models.WeiXinConfig()
                {
                    ID = wxConfig.WXID,
                    Name = wxConfig.WXName,
                    Token = wxConfig.WXToken,
                    AppId = wxConfig.WXAppID,
                    AppSecret = wxConfig.WXAppSecret
                };
                res = new WeiXinCore.WeiXin(weixinConfig);
            }
            return res;
        }

        /// <summary>
        /// 根据wxConfigId创建微信对象
        /// </summary>
        /// <param name="wxConfigId"></param>
        /// <returns></returns>
        public static WeiXinCore.WeiXin CreateWeiXinInstanceBySiteCode(string siteCode)
        {
            WeiXinCore.WeiXin res = null;
            WXConfigDAL dal = new WXConfigDAL();
            WXConfig wxConfig = dal.GetWXConfigBySiteCode(siteCode);
            if (wxConfig != null)
            {
                WeiXinCore.Models.WeiXinConfig weixinConfig = new WeiXinCore.Models.WeiXinConfig()
                {
                    ID = wxConfig.WXID,
                    Name = wxConfig.WXName,
                    Token = wxConfig.WXToken,
                    AppId = wxConfig.WXAppID,
                    AppSecret = wxConfig.WXAppSecret
                };
                res = new WeiXinCore.WeiXin(weixinConfig);
            }
            return res;
        }
    }
}