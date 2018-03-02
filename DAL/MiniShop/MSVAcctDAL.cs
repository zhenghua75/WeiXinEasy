using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.MiniShop;
using Maticsoft.DBUtility;

namespace DAL.MiniShop
{
   public class MSVAcctDAL
    {
       public MSVAcctDAL() { ;}
       #region -添加V币
       /// <summary>
       /// 添加V币
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddMSVAcct(MSVAcct model)
       {
           string sql = @"INSERT INTO [MS_VAcct]
                        ([CustID],[V_Amont],[SiteCode])
                 VALUES
                        (@CustID,@V_Amont,@SiteCode)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@CustID", model.CustID),
                new System.Data.SqlClient.SqlParameter("@V_Amont", model.V_Amont),
                new System.Data.SqlClient.SqlParameter("@SiteCode", model.SiteCode)
            };
           int rowsAffected = DbHelperSQL.ExecuteSql(sql.ToString(), paras);
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
       #region 更新账户V币数量
       /// <summary>
       /// 更新账户V币数量
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateMSVAcct(MSVAcct model)
       {
           string safeslq = "";
           safeslq = "UPDATE MS_VAcct SET ";
           if (model.V_Amont != null && model.V_Amont.ToString() != "")
           {
               safeslq += "V_Amont=" + model.V_Amont + "";
           }
           safeslq += " where CustID='" + model.CustID + "' and SiteCode='" + model.SiteCode + "' ";
           int rowsAffected = DbHelperSQL.ExecuteSql(safeslq.ToString());
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
       #region 获取V币属性
       /// <summary>
       /// 获取V币属性
       /// </summary>
       /// <param name="strValue"></param>
       /// <param name="strCustID"></param>
       /// <returns></returns>
       public object GetMSVAcct(string strValue, string strCustID)
       {
           string safesql = "";
           safesql = "select " + strValue + " from MS_VAcct where CustID='" + strCustID + "'";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion
       #region 获取所有V币列表
       /// <summary>
       /// 获取所有V币列表
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetMSVAcctList(string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select * ");
           strSql.Append(" from MS_VAcct ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 获取数据详细
       /// <summary>
       /// 获取数据详细
       /// </summary>
       /// <param name="strCustID"></param>
       /// <returns></returns>
       public DataSet GetMSVAcctDetail(string strCustID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM MS_VAcct ");
           strSql.Append(" WHERE [CustID] = @CustID");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@CustID", strCustID)
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion
       #region 验证账户V币是否存在
       /// <summary>
       /// 验证账户V币是否存在
       /// </summary>
       /// <param name="strCustID"></param>
       /// <param name="strSiteCode"></param>
       /// <returns></returns>
       public bool ExistMSVAcct(string strCustID, string strSiteCode)
       {
           string strSql = string.Empty;
           strSql += " SELECT count(CustID) FROM MS_VAcct  ";
           if (strCustID.Trim() != null && strCustID.Trim() != "")
           {
               if (strSql.Trim().ToLower().Contains("where"))
               {
                   strSql += " and ";
               }
               else
               {
                   strSql += "where";
               }
               strSql += "  [CustID] ='" + strCustID + "' ";
           }
           if (strSiteCode.Trim() != null && strSiteCode.Trim() != "")
           {
               if (strSql.Trim().ToLower().Contains("where"))
               {
                   strSql += " and ";
               }
               else
               {
                   strSql += "where";
               }
               strSql += "  SiteCode='" + strSiteCode + "' ";
           }
           return DbHelperSQL.Exists(strSql.ToString());
       }
       #endregion
       #region 获取V币列表(客户信息|微信配置信息)
       /// <summary>
       /// 获取V币列表(客户信息|微信配置信息)
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetVacctList(string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select  ");
           strSql.Append(" a.*,b.Phone,b.NickName,c.WXName ");
           strSql.Append(" from dbo.MS_VAcct a,MS_Customers b,WX_WXConfig c ");
           strSql.Append(" where a.SiteCode=c.SiteCode and a.CustID=b.ID and b.IsDel=0 and c.[State]=1 ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
    }
}
