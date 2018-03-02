using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.MiniShop;
using Maticsoft.DBUtility;

namespace DAL.MiniShop
{
   public class MSProductCategoryDAL
    {
       public MSProductCategoryDAL() { ;}
       #region 添加产品类别
       /// <summary>
       /// 添加产品类别
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddMSPCategory(MSProductCategory model)
       {
           string sql = @"INSERT INTO [MS_ProductCategory]
                        ([ID],[SID],[UpID],[Cname],[CsecHand],[Cstate],[Sortin],[AddTime])
                 VALUES
                        (@ID,@SID,@UpID,@Cname,@CsecHand,@Cstate,@Sortin,@AddTime)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@SID", model.SID),
                new System.Data.SqlClient.SqlParameter("@UpID", model.UpID),
                new System.Data.SqlClient.SqlParameter("@Cname", model.Cname),
                new System.Data.SqlClient.SqlParameter("@CsecHand",(model.CsecHand==1?1:0)),
                new System.Data.SqlClient.SqlParameter("@Cstate",(model.Cstate==1?1:0)),
                new System.Data.SqlClient.SqlParameter("@Sortin",(model.Sortin!=1?model.Sortin:1)),
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
       #region 更新产品类别
       /// <summary>
       /// 更新产品类别
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateMSPCategory(MSProductCategory model)
       {
           string safeslq = "";
           safeslq = "UPDATE MS_ProductCategory SET ";
           if (model.SID != null && model.SID != "")
           {
               safeslq += "[SID]='" + model.SID + "',";
           }
           if (model.UpID != null)
           {
               safeslq += "[UpID]='" + model.UpID + "',";
           }
           if (model.Cname != null && model.Cname != "")
           {
               safeslq += "Cname='" + model.Cname + "',";
           }
           if (model.CsecHand != null)
           {
               safeslq += " CsecHand=" + (model.CsecHand == 1 ? 1 : 0) + ", ";
           }
           safeslq += " Cstate=" + (model.Cstate == 1 ? 1 : 0) + ", ";
           safeslq += " Sortin=" + (model.Sortin != 1 ? model.Sortin : 1) + " ";
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
       #region 根据编号获取产品类别属性值
       /// <summary>
       /// 根据编号获取产品类别属性值
       /// </summary>
       /// <param name="strValue"></param>
       /// <param name="strID"></param>
       /// <returns></returns>
       public object GetMSPCategoryValueByID(string strValue, string strID)
       {
           string safesql = "";
           safesql = "select " + strValue + " from MS_ProductCategory where ID='" + strID + "'";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion 
       #region 更新类别状态
       /// <summary>
       /// 更新类别状态
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public bool UpdateMSPCState(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetMSPCategoryValueByID("Cstate", strID));
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
           strSql.Append(" UPDATE MS_ProductCategory ");
           strSql.Append(" SET Cstate = @Cstate ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strID),
                new System.Data.SqlClient.SqlParameter("@Cstate", state)
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
       #region 获取有效类别列表
       /// <summary>
       /// 获取有效类别列表
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetMSPCList(string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select a.ID,a.[SID],a.Cname,a.CsecHand,a.Cstate,a.AddTime, ");
           strSql.Append(" case  when a.[SID]=NULL or a.[SID]='' then ''  when a.[SID]<>'' OR a.[SID]<>null ");
           strSql.Append(" then (select ShopName from MS_Shop b where b.ID=a.[SID])  end ShopName ");
           strSql.Append(" from MS_ProductCategory a where Cstate=0 ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by a.AddTime desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 获取有效的店铺类别
       /// <summary>
       /// 获取有效的店铺类别
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetShopCategoryList(string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select a.*,b.ShopName from MS_ProductCategory a,MS_Shop b ");
           strSql.Append(" where a.Cstate=0 and a.[SID]=b.ID ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by a.Sortin asc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 获取有效的类别列表，二手或非店铺类别
       /// <summary>
       /// 获取有效的类别列表
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetSecHandCategoryList(string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select * from MS_ProductCategory ");
           strSql.Append(" where Cstate=0 ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by Sortin asc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 获取有效类别列表
       /// <summary>
       /// 获取有效类别列表
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetCategoryList(string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select * from MS_ProductCategory ");
           strSql.Append(" where Cstate=0 ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by Sortin asc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 获取类别详细
       /// <summary>
       /// 获取类别详细
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public DataSet GetCategoryDetail(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM MS_ProductCategory ");
           strSql.Append(" WHERE [ID] = @ID");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", strID)
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion
       #region 根据店铺编号、顶级导航编号、名称 判断类别是否存在
       /// <summary>
       /// 根据店铺编号、顶级导航编号、名称 判断类别是否存在
       /// </summary>
       /// <param name="strSID">店铺编号</param>
       /// <param name="strSID">顶级导航编号</param>
       /// <param name="strCname">名称</param>
       /// <returns></returns>
       public bool ExistMSPC(string strSID,string strUpID, string strCname)
       {
           string strSql = string.Empty;
           strSql += " SELECT count(ID) FROM MS_ProductCategory ";
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
           if (strUpID.Trim() != null && strUpID.Trim() != "")
           {
               if (strSql.ToString().Trim().ToLower().Contains("where"))
               {
                   strSql += " and ";
               }
               else
               {
                   strSql += " where ";
               }
               strSql += " [UpID] ='" + strUpID + "' ";
           }
           if (strCname.Trim() != null && strCname.Trim() != "")
           {
               if (strSql.ToString().Trim().ToLower().Contains("where"))
               {
                   strSql += " and ";
               }
               else
               {
                   strSql += " where ";
               }
               strSql += " [Cname] ='" + strCname + "' ";
           }
           return DbHelperSQL.Exists(strSql.ToString());
       }
       #endregion
    }
}
