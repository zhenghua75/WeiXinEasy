/* ==============================================================================
 * 类名称：WXConfigDAL
 * 类描述：
 * 创建人：yhn,51809571@qq.com
 * 创建时间：2014/3/15 10:31:29
 * 修改人：
 * 修改时间：
 * 修改备注：
 * @version 1.0
 * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.WeiXin;
using Maticsoft.DBUtility;
using System.Data;
using DBUtility;

namespace DAL.WeiXin
{
    public class WXConfigDAL
    {
        protected const string TABLE_NAME = "WX_WXConfig";

        /// <summary>
        /// 根据ID获取微信配置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WXConfig GetWXConfigByID(string id)
        {
            WXConfig res = null;
            if (!string.IsNullOrEmpty(id))
            {
                string sql = "SELECT * FROM WX_WXConfig WHERE ID = @ID AND [State] = 1 ";
                IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", id)
                };
                DataSet ds = DbHelperSQL.Query(sql, paras.ToArray());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    res = new WXConfig()
                    {
                        ID = dr.GetColumnValue("ID", string.Empty),
                        SiteCode = dr.GetColumnValue("SiteCode", string.Empty),
                        WXID = dr.GetColumnValue("WXID", string.Empty),
                        WXName = dr.GetColumnValue("WXName", string.Empty),
                        WXToken = dr.GetColumnValue("WXToken", string.Empty),
                        WXAppID = dr.GetColumnValue("WXAppID", string.Empty),
                        WXAppSecret = dr.GetColumnValue("WXAppSecret", string.Empty),
                        State = dr.GetColumnValue("State",1),
                        EncryptMode = dr.GetColumnValue("EncryptMode",0),
                        EncodingAESKey = dr.GetColumnValue("EncodingAESKey", string.Empty)
                    };
                }
            }
            return res;
        }


        /// <summary>
        /// 根据站点配置获取微信配置
        /// </summary>
        /// <param name="siteCode"></param>
        /// <returns></returns>
        public WXConfig GetWXConfigBySiteCode(string siteCode)
        {
            WXConfig res = null;
            if (!string.IsNullOrEmpty(siteCode))
            {
                string sql = "SELECT * FROM WX_WXConfig WHERE SiteCode = @SiteCode AND [State] = 1 ";
                IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", siteCode)
                };
                DataSet ds = DbHelperSQL.Query(sql, paras.ToArray());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    res = new WXConfig()
                    {
                        ID = dr.GetColumnValue("ID", string.Empty),
                        SiteCode = dr.GetColumnValue("SiteCode", string.Empty),
                        WXID = dr.GetColumnValue("WXID", string.Empty),
                        WXName = dr.GetColumnValue("WXName", string.Empty),
                        WXToken = dr.GetColumnValue("WXToken", string.Empty),
                        WXAppID = dr.GetColumnValue("WXAppID", string.Empty),
                        WXAppSecret = dr.GetColumnValue("WXAppSecret", string.Empty),
                        State = dr.GetColumnValue("State", 1),
                        EncryptMode = dr.GetColumnValue("EncryptMode",0),
                        EncodingAESKey = dr.GetColumnValue("EncodingAESKey", string.Empty)
                    };
                }
            }
            return res;
        }

        /// <summary>
        /// 根据站点代码获取访问Token，如过期返回Empty
        /// </summary>
        /// <param name="siteCode"></param>
        /// <returns></returns>
        public string GetAccessToken(string siteCode)
        {
            string res = string.Empty;
            if (!string.IsNullOrEmpty(siteCode))
            {
                string sql = "SELECT AccessToken FROM WX_WXConfig WHERE SiteCode = @SiteCode AND getdate()<TokenExpire";
                IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", siteCode)
                };
                res = DbHelperSQL.GetSingle(sql, paras.ToArray()) as string;
            }
            return res;
        }

        /// <summary>
        /// 更新AccessToken
        /// </summary>
        /// <param name="siteCode"></param>
        /// <param name="accessToken"></param>
        /// <param name="expiresIn"></param>
        /// <returns></returns>
        public bool UpdateAccessToken(string siteCode, string accessToken, int expiresIn=7200)
        {
            bool res = false;
            if (!string.IsNullOrEmpty(siteCode) && !string.IsNullOrEmpty(accessToken))
            {
                string sql = string.Format(@"
                    UPDATE {0} 
                    SET AccessToken=@AccessToken,TokenExpire= DateAdd(Second,{1},getdate())
                    WHERE SiteCode=@SiteCode",
                    TABLE_NAME, expiresIn);
                System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", siteCode),
                    new System.Data.SqlClient.SqlParameter("@AccessToken", accessToken)
                };
                res = DbHelperSQL.ExecuteSql(sql, paras) > 0;
            }
            return res;
        }
        #region 获取配置属性
        /// <summary>
        /// 获取配置属性
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="strSiteCode"></param>
        /// <returns></returns>
        public object GetWXConfigValue(string strValue, string strSiteCode)
        {
            string safesql = string.Empty; ;
            safesql = "select " + strValue + " from WX_WXConfig where SiteCode='" + strSiteCode + "' ";
            return DbHelperSQL.GetSingle(safesql.ToString());
        }
        #endregion
        #region 获取配置列表
        /// <summary>
        /// 获取配置列表
        /// </summary>
        /// <param name="SiteCode"></param>
        /// <returns></returns>
        public DataSet GetWXConfigDataList(string SiteCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM WX_WXConfig ");
            strSql.Append(" WHERE SiteCode = @SiteCode ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", SiteCode)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion
    }
}
