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
    /// 收货地址功能操作
    /// </summary>
   public class MSDeliveryAddressDAL
    {
       public MSDeliveryAddressDAL() { ;}
       #region 添加收货地址
       /// <summary>
       /// 添加收货地址
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddDA(MSDeliveryAddress model)
       {
           string sql = @"INSERT INTO [MS_DeliveryAddress]
                        ([ID],[UID],[DaName],[DaPhone],[DaAddress],[AddressDetail],[DaZipCode],[IsDefault],[IsDel])
                 VALUES
                        (@ID,@UID,@DaName,@DaPhone,@DaAddress,@AddressDetail,@DaZipCode,@IsDefault,@IsDel)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@UID", model.UID),
                new System.Data.SqlClient.SqlParameter("@DaName", model.DaName),
                new System.Data.SqlClient.SqlParameter("@DaPhone", model.DaPhone),
                new System.Data.SqlClient.SqlParameter("@DaAddress", model.DaAddress),
                new System.Data.SqlClient.SqlParameter("@AddressDetail", model.AddressDetail),
                new System.Data.SqlClient.SqlParameter("@DaZipCode", model.DaZipCode),
                new System.Data.SqlClient.SqlParameter("@IsDefault",(model.IsDefault==1?1:0)),
                new System.Data.SqlClient.SqlParameter("@IsDel",(model.IsDel==1?1:0))
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
       #region 更新收货地址
       /// <summary>
       /// 更新收货地址
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateDA(MSDeliveryAddress model)
       {
           string safeslq = "";
           safeslq = "UPDATE MS_DeliveryAddress SET ";
           if (model.DaName != null && model.DaName != "")
           {
               safeslq += "DaName='" + model.DaName + "',";
           }
           if (model.DaPhone != null && model.DaPhone != "")
           {
               safeslq += "DaPhone='" + model.DaPhone + "',";
           }
           if (model.DaAddress != null && model.DaAddress != "")
           {
               safeslq += "DaAddress='" + model.DaAddress + "',";
           }
           if (model.AddressDetail != null && model.AddressDetail.ToString() != "")
           {
               safeslq += "AddressDetail='" + model.AddressDetail + "',";
           }
           if (model.DaZipCode != null && model.DaZipCode.ToString() != "")
           {
               safeslq += "DaZipCode='" + model.DaZipCode + "',";
           }
           safeslq += " IsDefault=" + (model.IsDefault == 1 ? 1 : 0) + ", ";
           safeslq += " IsDel=" + (model.IsDel == 1 ? 1 : 0) + " ";
           safeslq += " where ID='" + model.ID + "' and [UID]='" + model.UID + "' ";
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
       #region 获取收货地址属性
       /// <summary>
       /// 获取收货地址属性
       /// </summary>
       /// <param name="strValue"></param>
       /// <param name="strID"></param>
       /// <returns></returns>
       public object GetDAValueByID(string strValue, string strID)
       {
           string safesql = "";
           safesql = "select " + strValue + " from MS_DeliveryAddress where ID='" + strID + "'";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion
       #region 更新收货地址状态
       /// <summary>
       /// 更新收货地址状态
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public bool UpdateDAstate(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetDAValueByID("IsDel", strID));
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
           strSql.Append(" UPDATE MS_DeliveryAddress ");
           strSql.Append(" SET IsDel = @IsDel ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strID),
                new System.Data.SqlClient.SqlParameter("@IsDel", state)
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
       #region 更新收货地址是否为默认
       /// <summary>
       /// 更新收货地址是否为默认
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public bool UpdateDAdefault(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetDAValueByID("IsDefault", strID));
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
           strSql.Append(" UPDATE MS_DeliveryAddress ");
           strSql.Append(" SET IsDefault = @IsDefault ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strID),
                new System.Data.SqlClient.SqlParameter("@IsDefault", state)
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
       #region 获取有效的收货地址
       /// <summary>
       /// 获取有效的收货地址
       /// </summary>
       /// <param name="top"></param>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetDAList(int top,string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select  ");
           if (top > 0)
           {
               strSql.Append(" top " + top);
           }
           strSql.Append(" * from MS_DeliveryAddress ");
           strSql.Append(" where IsDel=0 ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 获取收货地址详细
       /// <summary>
       /// 获取收货地址详细
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public DataSet GetDADetail(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM MS_DeliveryAddress ");
           strSql.Append(" WHERE [ID] = @ID");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", strID)
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion
    }
}
