using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.MiniShop;
using Maticsoft.DBUtility;

namespace DAL.MiniShop
{
   public class MSShopContactsDAL
    {
       public MSShopContactsDAL() { ;}
       #region 添加店铺联系方式
       /// <summary>
       /// 添加店铺联系方式
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddMSSContacts(MSShopContacts model)
       {
           string sql = @"INSERT INTO [MS_ShopContacts]
                        ([ID],[SID],[PID],[NickName],[Phone],[QQnum],[Email],[Address],[IsDefault],[IsDel],[AddTime])
                 VALUES
                        (@ID,@SID,@PID,@NickName,@Phone,@QQnum,@Email,@Address,@IsDefault,@IsDel,@AddTime)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@SID", model.SID),
                new System.Data.SqlClient.SqlParameter("@PID", model.PID),
                new System.Data.SqlClient.SqlParameter("@NickName", model.NickName),
                new System.Data.SqlClient.SqlParameter("@Phone", model.Phone),
                new System.Data.SqlClient.SqlParameter("@QQnum", model.QQnum),
                new System.Data.SqlClient.SqlParameter("@Email", model.Email),
                new System.Data.SqlClient.SqlParameter("@Address", model.Address),
                new System.Data.SqlClient.SqlParameter("@IsDefault", model.IsDefault),
                new System.Data.SqlClient.SqlParameter("@IsDel",(model.IsDel==1?1:0)),
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
       #region 更新店铺联系方式
       /// <summary>
       /// 更新店铺联系方式
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateMSSContacts(MSShopContacts model)
       {
           string safeslq = "";
           safeslq = "UPDATE MS_ShopContacts SET ";
           if (model.SID != null && model.SID != "")
           {
               safeslq += "SID='" + model.SID + "',";
           }
           if (model.PID != null && model.PID != "")
           {
               safeslq += "PID='" + model.PID + "',";
           }
           if (model.NickName != null && model.NickName.ToString() != "")
           {
               safeslq += "NickName='" + model.NickName + "',";
           }
           if (model.Phone != null && model.Phone.ToString() != "")
           {
               safeslq += "Phone='" + model.Phone + "',";
           }
           safeslq += "QQnum=" + 
               (model.QQnum.ToString() != null && model.QQnum.ToString() != "" ? model.QQnum : 0) + ",";
           if (model.Email != null && model.Email != "")
           {
               safeslq += "Email='" + model.Email + "',";
           }
           if (model.Address != null && model.Address != "")
           {
               safeslq += "[Address]='" + model.Address + "',";
           }
           safeslq += "IsDefault=" + (model.IsDefault == 1 ? 1 : 0) + ",";
           safeslq += " IsDel=" + (model.IsDel == 1 ? 1 : 0) + " ";
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
       #region 获取店铺联系方式属性
       /// <summary>
       /// 获取店铺联系方式属性
       /// </summary>
       /// <param name="strValue"></param>
       /// <param name="strID"></param>
       /// <returns></returns>
       public object GetMSSContactValueByID(string strValue, string strID)
       {
           string safesql = "";
           safesql = "select " + strValue + " from MS_ShopContacts where ID='" + strID + "'";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion
       #region 更新联系方式状态
       /// <summary>
       /// 更新联系方式状态
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public bool UpdateMSSContactState(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetMSSContactValueByID("IsDel", strID));
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
           strSql.Append(" UPDATE MS_ShopContacts ");
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
       #region 设置默认联系方式
       /// <summary>
       /// 设置默认联系方式
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public bool UpdateMSSContactIsDefault(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetMSSContactValueByID("IsDefault", strID));
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
           strSql.Append(" UPDATE MS_ShopContacts ");
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
       #region 获取有效的联系方式列表
       /// <summary>
       /// 获取有效的联系方式列表
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetMSShopContactsList(string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select b.ShopName,a.ID,a.[SID],a.Phone,a.QQnum,a.Email,a.[Address],a.NickName,");
           strSql.Append(" (case when(a.IsDefault=0)then '否' when(a.IsDefault=1) then '是' end) as IsDefault,a.AddTime");
           strSql.Append(" from MS_ShopContacts a,MS_Shop b where IsDel=0 and a.[SID]=b.ID ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by a.AddTime desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 获取联系方式详细
       /// <summary>
       /// 获取联系方式详细
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public DataSet GetContactDetail(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM MS_ShopContacts ");
           strSql.Append(" WHERE [ID] = @ID");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", strID)
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion
       #region 判断联系方式是否存在
       /// <summary>
       /// 判断联系方式是否存在
       /// </summary>
       /// <param name="strPhone">店铺电话</param>
       /// <param name="strNickName">用户昵称</param>
       /// <param name="strSID">店铺编号</param>
       /// <param name="strPID">产品编号</param>
       /// <returns></returns>
       public bool ExistContact(string strPhone, string strNickName,string strSID,string strPID)
       {
           string strSql = string.Empty;
           strSql += " SELECT count(ID) FROM MS_ShopContacts where isDel=0 ";
           if (strPhone.Trim() != null && strPhone.Trim() != "")
           {
               if (strSql.ToString().Trim().ToLower().Contains("where"))
               {
                   strSql += " and ";
               }
               else
               {
                   strSql += " where ";
               }
               strSql += " [Phone] ='" + strPhone + "' ";
           }
           if (strSID.Trim() != null && strSID.Trim() != "")
           {
               if (strSql.ToString().Trim().ToLower().Contains("where"))
               {
                   strSql += " and ";
               }
               else
               {
                   strSql += " where ";
               }
               strSql += " [SID] ='" + strSID + "' ";
           }
           if (strPID.Trim() != null && strPID.Trim() != "")
           {
               if (strSql.ToString().Trim().ToLower().Contains("where"))
               {
                   strSql += " and ";
               }
               else
               {
                   strSql += " where ";
               }
               strSql += " [PID] ='" + strPID + "' ";
           }
           if (strNickName.Trim() != null && strNickName.Trim() != "")
           {
               if (strSql.ToString().Trim().ToLower().Contains("where"))
               {
                   strSql += " and ";
               }
               else
               {
                   strSql += " where ";
               }
               strSql += " [NickName] ='" + strNickName + "' ";
           }
           return DbHelperSQL.Exists(strSql.ToString());
       }
       #endregion
       #region 根据产品编号获取联系方式详细
       /// <summary>
       /// 根据产品编号获取联系方式详细
       /// </summary>
       /// <param name="strPID">产品编号</param>
       /// <returns></returns>
       public DataSet GetContactDetailByPID(string strPID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM MS_ShopContacts ");
           strSql.Append(" WHERE [PID] = @PID");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@PID", strPID)
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion
    }
}
