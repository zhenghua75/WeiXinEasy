using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.MiniShop;
using Maticsoft.DBUtility;

namespace DAL.MiniShop
{
    /// <summary>
    /// 订单物流操作
    /// </summary>
   public class MSOrderLogisticsDAL
    {
       public MSOrderLogisticsDAL() { ;}
       #region-订单发货
       /// <summary>
       /// 订单发货
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddOrderLogistics(MSOrderLogistics model)
       {
           string sql = @"INSERT INTO [MS_OrderLogistics]
                        ([ID],[OID],[CName],[AddTime])
                 VALUES
                        (@ID,@OID,@CName,@AddTime)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@OID", model.OID),
                new System.Data.SqlClient.SqlParameter("@CName", model.CName),
                new System.Data.SqlClient.SqlParameter("@AddTime", DateTime.Now)
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
       #region-更新发货单信息
       /// <summary>
       /// 更新发货单信息
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateOrderLogistics(MSOrderLogistics model)
       {
           string safeslq = "";
           safeslq = "UPDATE MS_OrderLogistics SET ";
           if (model.OID != null && model.OID != "")
           {
               if (!safeslq.Trim().ToLower().Contains(","))
               {
                   safeslq += ",";
               }
               safeslq += "OID='" + model.OID + "'";
           }
           if (model.CName != null && model.CName.ToString() != "")
           {
               if (!safeslq.Trim().ToLower().Contains(","))
               {
                   safeslq += ",";
               }
               safeslq += "CName='" + model.CName + "'";
           }
           safeslq += " where ID='" + model.ID + "'";
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
       #region 获取发货单详情
       /// <summary>
       /// 获取发货单详情
       /// </summary>
       /// <param name="strID">物流号</param>
       /// <returns></returns>
       public DataSet GetOLdetail(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM MS_OrderLogistics ");
           strSql.Append(" WHERE [ID] = @ID");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", strID)
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion
       #region 根据订单号获取发货单详细
       /// <summary>
       /// 根据订单号获取发货单详细
       /// </summary>
       /// <param name="strOID"></param>
       /// <returns></returns>
       public DataSet GetMSODetailByOID(string strOID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM MS_OrderLogistics ");
           strSql.Append(" WHERE [OID] = @OID");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@OID", strOID)
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion
       #region 获取发货单列表
       /// <summary>
       /// 获取发货单列表
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetOLlist(string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select * ");
           strSql.Append(" from MS_OrderLogistics ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by AddTime desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 验证订单是否已提交
       /// <summary>
       /// 验证订单是否已提交
       /// </summary>
       /// <param name="CID">物流订单</param>
       /// <param name="strOID">产品订单</param>
       /// <returns></returns>
       public bool ExistOID(string CID,string strOID)
       {
           string strSql = string.Empty;
           strSql += " SELECT count(ID) FROM MS_OrderLogistics ";
           if (CID != null && CID != "")
           {
               if (strSql.Trim().ToLower().Contains("where"))
               {
                   strSql += " and ";
               }
               else
               {
                   strSql += " where ";
               }
               strSql += " ID='"+CID+"' ";
           }
           if (strOID != null && strOID != "")
           {
               if (strSql.Trim().ToLower().Contains("where"))
               {
                   strSql += " and ";
               }
               else
               {
                   strSql += " where ";
               }
               strSql += " OID='" + strOID + "' ";
           }
           return DbHelperSQL.Exists(strSql.ToString());
       }
       #endregion
    }
}
