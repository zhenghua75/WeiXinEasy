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
    /// 照片提交处理类
    /// </summary>
   public class MSPhotoSubmitDAL
    {
       public MSPhotoSubmitDAL() { ;}
       #region 添加订单及照片信息
       /// <summary>
       /// 添加订单及照片信息
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddPhotoSubmit(MSPhotoSubmit model)
       {
           string sql = @"INSERT INTO [MS_PhotoSubmit]
                        ([ID],[UID],[OrderNum],[Img1],[Img2],[Reivew],[Pstate],[AddTime])
                 VALUES
                        (@ID,@UID,@OrderNum,@Img1,@Img2,@Reivew,@Pstate,@AddTime)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@UID", model.UID),
                new System.Data.SqlClient.SqlParameter("@OrderNum", model.OrderNum),
                new System.Data.SqlClient.SqlParameter("@Img1", model.Img1),
                new System.Data.SqlClient.SqlParameter("@Img2", model.Img2),
                new System.Data.SqlClient.SqlParameter("@Reivew",(model.Reivew==1?1:0)),
                new System.Data.SqlClient.SqlParameter("@Pstate",(model.Pstate==1?1:0)),
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
       #region 修改订单及照片信息
       /// <summary>
       /// 修改订单及照片信息
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdatePhotoSubmit(MSPhotoSubmit model)
       {
           string safeslq = "";
           safeslq = "UPDATE MS_PhotoSubmit SET ";
           if (model.OrderNum != null && model.OrderNum != "")
           {
               safeslq += "OrderNum='" + model.OrderNum + "',";
           }
           if (model.Img1 != null && model.Img1 != "")
           {
               safeslq += "Img1='" + model.Img1 + "',";
           }
           if (model.Img2 != null && model.Img2.ToString() != "")
           {
               safeslq += "Img2='" + model.Img2 + "',";
           }
           safeslq += " Reivew=" + (model.Reivew == 1 ? 1 : 0) + ", ";
           safeslq += " Pstate=" + (model.Pstate == 1 ? 1 : 0) + " ";
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
       #region 获取订单信息值
       /// <summary>
       /// 获取订单信息值
       /// </summary>
       /// <param name="strValue"></param>
       /// <param name="strID"></param>
       /// <returns></returns>
       public object GetPhotoSubmitByID(string strValue, string strID)
       {
           string safesql = "";
           safesql = "select " + strValue + " from MS_PhotoSubmit where ID='" + strID + "'";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion
       #region 更新订单审核状态
       /// <summary>
       /// 更新订单审核状态
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public bool UpdatePhotoSubmitReivew(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetPhotoSubmitByID("Reivew", strID));
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
           strSql.Append(" UPDATE MS_PhotoSubmit ");
           strSql.Append(" SET Reivew = @Reivew ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strID),
                new System.Data.SqlClient.SqlParameter("@Reivew", state)
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
       #region 更新订单审核状态(后台用)
       /// <summary>
       /// 更新订单审核状态(后台用)
       /// </summary>
       /// <param name="strID"></param>
       /// <param name="state"></param>
       /// <returns></returns>
       public bool UpdatePhotoSubmitRv(string strID, int state)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" UPDATE MS_PhotoSubmit ");
           strSql.Append(" SET Reivew = @Reivew ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strID),
                new System.Data.SqlClient.SqlParameter("@Reivew", state)
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
       #region 更新订单状态
       /// <summary>
       /// 更新订单状态
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public bool UpdatePhotoSubmitState(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetPhotoSubmitByID("Pstate", strID));
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
           strSql.Append(" UPDATE MS_PhotoSubmit ");
           strSql.Append(" SET Pstate = @Pstate ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strID),
                new System.Data.SqlClient.SqlParameter("@Pstate", state)
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
       #region 获取订单列表
       /// <summary>
       /// 获取订单列表
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetPhotoSubmitList(string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select * ");
           strSql.Append(" from MS_PhotoSubmit  where Pstate=0 ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by AddTime desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 获取提交订单详细
       /// <summary>
       /// 获取提交订单详细
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public DataSet GetPhotoSubmitDetail(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM MS_PhotoSubmit ");
           strSql.Append(" WHERE [ID] = @ID");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", strID)
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion
       #region 判断订单是否存在
       /// <summary>
       /// 判断订单是否存在
       /// </summary>
       /// <param name="strOrderNum"></param>
       /// <returns></returns>
       public bool ExsitOrderNum(string strOrderNum)
       {
           string strSql = string.Empty;
           strSql += " SELECT count(ID) FROM MS_ProductOrder where id='"+strOrderNum+"'";
           return DbHelperSQL.Exists(strSql.ToString());
       }
       #endregion
       #region 判断订单是否已提交过
       /// <summary>
       /// 判断订单是否已提交过
       /// </summary>
       /// <param name="strOrderNum"></param>
       /// <returns></returns>
       public bool ExistisSubmit(string strOrderNum)
       {
           string strSql = string.Empty;
           strSql += " SELECT count(ID) FROM MS_PhotoSubmit where OrderNum='" + strOrderNum + "' and Pstate=0 ";
           return DbHelperSQL.Exists(strSql.ToString());
       }
       #endregion
    }
}
