using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.MiniShop;
using Maticsoft.DBUtility;

namespace DAL.MiniShop
{
   public class MSVAcctDetailDAL
    {
       public MSVAcctDetailDAL() { ;}
       #region 添加V币详细
       /// <summary>
       /// 添加V币详细
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddMSVAcctDetail(MSVAcctDetail model) {
           string sql = @"INSERT INTO [MS_VAcctDetail]
                        ([CustID],[Amount],[ChargeType],[SiteCode],[Ext_Fld1],[IsReceive],[Vdate])
                 VALUES
                        (@CustID,@Amount,@ChargeType,@SiteCode,@Ext_Fld1,@IsReceive,@Vdate)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@CustID", model.CustID),
                new System.Data.SqlClient.SqlParameter("@Amount", model.Amount),
                new System.Data.SqlClient.SqlParameter("@ChargeType", model.ChargeType),
                new System.Data.SqlClient.SqlParameter("@SiteCode", model.SiteCode),
                new System.Data.SqlClient.SqlParameter("@Ext_Fld1", model.Ext_Fld1),
                new System.Data.SqlClient.SqlParameter("@IsReceive", (model.IsReceive==1?1:0)),
                new System.Data.SqlClient.SqlParameter("@Vdate", DateTime.Now)
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
       #region 更新V币详细
       /// <summary>
       /// 更新V币详细
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateMSVAcctDetail(MSVAcctDetail model)
       {
           string safeslq = "";
           safeslq = "UPDATE MS_VAcctDetail SET ";
           if (model.Amount != null && model.Amount.ToString() != "")
           {
               safeslq += "Amount=" + model.Amount + ",";
           }
           if (model.ChargeType != null && model.ChargeType.ToString() != "")
           {
               safeslq += "ChargeType='" + model.ChargeType + "',";
           }
           if (model.Ext_Fld1 != null && model.Ext_Fld1.ToString() != "")
           {
               safeslq += "Ext_Fld1='" + model.Ext_Fld1 + "',";
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
       #region 获取V币详细属性
       /// <summary>
       /// 获取V币详细属性
       /// </summary>
       /// <param name="strValue"></param>
       /// <param name="strCustID"></param>
       /// <returns></returns>
       public object GetMSVAcctDetail(string strValue, string strCustID)
       {
           string safesql = "";
           safesql = "select " + strValue + " from MS_VAcctDetail where CustID='" + strCustID + "'";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion
       #region 根据ID获取V币属性值
       /// <summary>
       /// 根据ID获取V币属性值
       /// </summary>
       /// <param name="strValue"></param>
       /// <param name="strExt_Fid"></param>
       /// <returns></returns>
       public object GetMSVAcctDetailByFid(string strValue, string strExt_Fid)
       {
           string safesql = "";
           safesql = "select " + strValue + " from MS_VAcctDetail where Ext_Fld1='" + strExt_Fid + "'";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion
       #region 根据UID获取V币属性值
       /// <summary>
       /// 根据UID获取V币属性值
       /// </summary>
       /// <param name="strVlaue"></param>
       /// <param name="strUID"></param>
       /// <returns></returns>
       public object GetMSVAcctDetailByUID(string strVlaue, string strUID)
       {
           string safesql = "";
           safesql = "select top 1 " + strVlaue + " from MS_VAcctDetail where CustID='" + strUID + "' order by Vdate desc";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion
       #region 获取V币详细列表
       /// <summary>
       /// 获取V币详细列表
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetMSVAcctDetailList(string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select * ");
           strSql.Append(" from MS_VAcctDetail ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by Vdate desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 更新V币领取状态
       /// <summary>
       /// 更新V币领取状态
       /// </summary>
       /// <param name="strFid"></param>
       /// <returns></returns>
       public bool UpdateMSVacct(string strFid)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetMSVAcctDetailByFid("IsReceive", strFid));
           }
           catch (Exception)
           {
               state = 0;
           }
           switch (state)
           {
               case 0:
                   state = 1;
                   break;
               default:
                   state = 0;
                   break;
           }
           strSql.Append(" UPDATE MS_VAcctDetail ");
           strSql.Append(" SET IsReceive = @IsReceive ");
           strSql.Append(" WHERE Ext_fld1 = @Ext_fld1 ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@IsReceive", state),
                new System.Data.SqlClient.SqlParameter("@Ext_fld1", strFid)
            };
           int rowsAffected = DbHelperSQL.ExecuteSql(strSql.ToString(), paras);
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
       #region 获取V币使用/获取详细列表
       /// <summary>
       /// 获取V币使用/获取详细列表
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetVaccdetail(string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select  ");
           strSql.Append(" a.*,b.V_Amont ");
           strSql.Append(" from MS_VAcctDetail a, MS_VAcct b ");
           strSql.Append(" where a.CustID=b.CustID and a.SiteCode=b.SiteCode ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by a.Vdate desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
    }
}
