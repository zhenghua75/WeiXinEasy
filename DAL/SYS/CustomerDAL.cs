using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maticsoft.DBUtility;

namespace DAL.SYS
{
    public class CustomerDAL
    {
        public CustomerDAL() { ; }

        #region 用户号码是否已经注册
        /// <summary>
        /// 用户号码是否已经注册
        /// </summary>
        /// <param name="strMobile">注册手机号</param>
        /// 
        public bool CheckMobile(string strMobile)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" UPDATE SYS_Customers SET Mobile = @Mobile ");
            strSql.Append(" WHERE Mobile = @Mobile ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@Mobile", strMobile)
            };
            int rowsAffected = DbHelperSQL.ExecuteSql(strSql.ToString(),paras);
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 返回用户登录信息
        /// <summary>
        /// 返回用户登录信息
        /// </summary>
        /// <param name="strMobile">登录名</param>
        /// <param name="strPassword">登录口令</param>
        /// 
        public DataSet GetCustomerData(string strMobile, string strPassword, string strSiteCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT ID,Mobile,Name,PassWord,SiteCode,OpenID ");
            strSql.Append(" FROM SYS_Customers ");
            strSql.Append(" WHERE Mobile = @Mobile ");
            strSql.Append(" AND [PassWord] = @PassWord ");
            strSql.Append(" AND [SiteCode] = @SiteCode ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@Mobile", strMobile),
                new System.Data.SqlClient.SqlParameter("@PassWord", strPassword),
                new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode)
            };
            DataSet ds = DbHelperSQL.Query(strSql.ToString(),paras);
            return ds;
        }
        #endregion

        #region 添加客户信息
        /// <summary>
        /// 添加客户信息
        /// </summary>
        /// <param name="model">客户信息</param>
        /// 
        public bool AddCustomerData(Model.SYS.SYS_Customer model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            StringBuilder strSql2 = new StringBuilder();
            if (model.ID != null)
            {
                strSql1.Append("ID,");
                strSql2.Append("'" + model.ID + "',");
            }
            if (model.Mobile != null)
            {
                strSql1.Append("Mobile,");
                strSql2.Append("'" + model.Mobile + "',");
            }
            if (model.Name != null)
            {
                strSql1.Append("Name,");
                strSql2.Append("'" + model.Name + "',");
            }
            if (model.PassWord != null)
            {
                strSql1.Append("PassWord,");
                strSql2.Append("'" + model.PassWord + "',");
            }
            if (model.OpenID != null)
            {
                strSql1.Append("OpenID,");
                strSql2.Append("'" + model.OpenID + "',");
            } 
            if (model.SiteCode != null)
            {
                strSql1.Append("SiteCode,");
                strSql2.Append("'" + model.SiteCode + "',");
            }
            if (model.MemberShipNo != null)
            {
                strSql1.Append("MemberShipNo,");
                strSql2.Append("'" + model.MemberShipNo + "',");
            }
            strSql.Append("INSERT INTO SYS_Customers(");
            strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
            strSql.Append(")");
            strSql.Append(" VALUES (");
            strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
            strSql.Append(");");

            strSql.Append(" UPDATE SYS_Customers SET ID = '" + model.ID + "'");
            strSql.Append(" WHERE ID ='" + model.ID + "' ");
            int rowsAffected = DbHelperSQL.ExecuteSql(strSql.ToString());
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 客户数据是否存在
        /// <summary>
        /// 客户数据是否存在
        /// </summary>
        /// <param name="strSiteCode">站点代码</param>
        /// <param name="strOpenID">用户OpenID</param>
        /// 
        public string GetCustMemberShipNo(string strSiteCode,string strOpenID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT ISNULL(MemberShipNo,0) AS MSNo FROM SYS_Customers ");
            strSql.Append(" WHERE SiteCode = @SiteCode ");
            //strSql.Append(" AND REPLACE(OpenID,'-','') = @OpenID ");
            strSql.Append(" AND OpenID = @OpenID ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode),
                new System.Data.SqlClient.SqlParameter("@OpenID", strOpenID)
            };
            DataSet ds = DbHelperSQL.Query(strSql.ToString(),paras);
            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string strMSNo = ds.Tables[0].Rows[0]["MSNo"].ToString();
                if (string.IsNullOrEmpty(strMSNo))
                { 
                    strMSNo = "0";
                }
                return strMSNo;
            }
            else
            {
                return "-1";
            }

        }
        #endregion

        #region 修改客户账号
        /// <summary>
        /// 修改客户账号
        /// </summary>
        /// <param name="strSiteCode">站点代码</param>
        /// <param name="strOpenID">用户OpenID</param>
        /// <param name="strMemNo">用户账号</param>
        /// 
        public int UpdateCutMemberShipNo(string strSiteCode, string strOpenID,string strMemNo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" UPDATE SYS_Customers SET MemberShipNo = @MemNo ");
            strSql.Append(" WHERE SiteCode = @SiteCode ");
            strSql.Append(" AND REPLACE(OpenID,'-','') = @OpenID ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode),
                new System.Data.SqlClient.SqlParameter("@OpenID", strOpenID),
                new System.Data.SqlClient.SqlParameter("@MemNo", strMemNo)
            };
            return DbHelperSQL.ExecuteSql(strSql.ToString(), paras);
        }
        #endregion
    }
}
