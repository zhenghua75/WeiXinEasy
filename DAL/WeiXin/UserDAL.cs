using DAL.Common;
using Maticsoft.DBUtility;
using Model.WeiXin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WeiXinCore.Models;

namespace DAL.WeiXin
{
    public class UserDAL
    {
        public const string TABLE_NAME = "WX_User";

        public static UserDAL CreateInstance()
        {
            return new UserDAL();
        }

        /// <summary>
        /// 同步微信用户信息
        /// </summary>
        public static void SyncWXUserBySiteCode(string siteCode)
        {
            if (!string.IsNullOrEmpty(siteCode))
            {
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
                    WeiXinCore.WeiXin weixin = new WeiXinCore.WeiXin(weixinConfig);
                    List<UserInfo> users=weixin.GetUserInfos();
                    if (users != null)
                    {
                        UserDAL dal2 = CreateInstance();
                        dal2.ClearUser(wxConfig.ID);
                        foreach (UserInfo user in users)
                        {
                            User info = new User()
                            {
                                WXConfigID=wxConfig.ID,
                                OpenID=user.OpenId,
                                Subscribe=user.Subscribe,
                                NickName=user.NickName,
                                Sex=user.Sex,
                                Language=user.Language,
                                City=user.City,
                                Province=user.Province,
                                Country=user.Country,
                                HeadImgUrl=user.Headimgurl,
                                SubscribeTime=user.Subscribe_Time,
                            };
                            dal2.Insert(info);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 添加微信用户信息
        /// </summary>
        /// <param name="info"></param>
        public void Insert(User info)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("ID", info.ID);
            data.Add("OpenID", info.OpenID);
            data.Add("WXConfigID", info.WXConfigID);
            data.Add("Subscribe", info.Subscribe);
            data.Add("NickName", info.NickName);
            data.Add("Sex", info.Sex);
            data.Add("Language", info.Language);
            data.Add("City", info.City);
            data.Add("Province", info.Province);
            data.Add("Country", info.Country);
            data.Add("HeadImgUrl", info.HeadImgUrl);
            data.Add("SubscribeTime", info.SubscribeTime);
            data.Add("GroupID", info.GroupID);
            data.Add("AddTime", info.AddTime);
            SQLHelperExt.Insert(TABLE_NAME, data);
        }

        /// <summary>
        /// 清除微信配置下的所有用户
        /// </summary>
        /// <param name="wxConfigID"></param>
        public void ClearUser(string wxConfigID)
        {
            if (!string.IsNullOrEmpty(wxConfigID))
            {
                string sql = string.Format("DELETE FROM {0} WHERE WXConfigID=@WXConfigID", TABLE_NAME);
                IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@WXConfigID", wxConfigID)
                };
                DbHelperSQL.ExecuteSql(sql,paras.ToArray());
            }
        }
        #region 根据OpenID判断用户是否存在
        /// <summary>
        /// 根据OpenID判断用户是否存在
        /// </summary>
        /// <param name="strOpenID"></param>
        /// <returns></returns>
        public bool ExistUserByOpenID(string strOpenID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT count(ID) ");
            strSql.Append(" FROM WX_User ");
            strSql.Append(" WHERE  [OpenID] = @OpenID ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@OpenID", strOpenID)
                };
            return DbHelperSQL.Exists(strSql.ToString(), paras.ToArray());
        }
        #endregion

        #region 获取用户列表
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="siteCode"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet GetWXUserListBySiteCode(string where)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select a.* from WX_User a left join WX_WXConfig b on(a.WXConfigID=b.ID) ");
            if (where.Trim() != null && where.Trim() != "")
            {
                strSql.Append(" WHERE " + where);
            }
            strSql.Append(" ORDER BY AddTime DESC ");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion

        #region 获取用户属性
        /// <summary>
        /// 获取用户属性
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="strOpenID"></param>
        /// <returns></returns>
        public object GetUserValueByOpenID(string strValue, string strOpenID)
        {
            if (strValue.Trim() != null && strValue.Trim() != "")
            {
                string safesql = "select " + strValue + " from WX_User where OpenID='" + strOpenID + "'";
                return DbHelperSQL.GetSingle(safesql);
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}
