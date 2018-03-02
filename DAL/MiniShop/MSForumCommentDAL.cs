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
    /// 帖子评论 操作类
    /// </summary>
   public class MSForumCommentDAL
    {
       /// <summary>
        /// 帖子评论 操作类
       /// </summary>
       public MSForumCommentDAL() { ;}
       #region 添加评论
       /// <summary>
       /// 添加评论
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddComment(MSForumComment model)
       {
           string sql = @"INSERT INTO [MS_ForumComment]
                        ([ID],[UpID],[TID],[UID],[Ctext],[Review],[Cstate],[AddTime])
                 VALUES
                        (@ID,@UpID,@TID,@UID,@Ctext,@Review,@Cstate,@AddTime)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@UpID", model.UpID),
                new System.Data.SqlClient.SqlParameter("@TID", model.TID),
                new System.Data.SqlClient.SqlParameter("@UID", model.UID),
                new System.Data.SqlClient.SqlParameter("@Ctext", model.Ctext),
                new System.Data.SqlClient.SqlParameter("@Review",(model.Review==1?1:0)),
                new System.Data.SqlClient.SqlParameter("@Cstate",(model.Cstate==1?1:0)),
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
       #region 更新评论信息
       /// <summary>
       /// 更新评论信息
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateComment(MSForumComment model)
       {
           string safeslq = "";
           safeslq = "UPDATE MS_ForumComment SET ";
           if (model.UpID != null && model.UpID != "")
           {
               safeslq += "UpID='" + model.UpID + "',";
           }
           if (model.TID != null && model.TID.ToString() != "")
           {
               safeslq += "TID='" + model.TID + "',";
           }
           if (model.UID != null && model.UID.ToString() != "")
           {
               safeslq += "UID='" + model.UID + "',";
           }
           if (model.Ctext != null && model.Ctext != "")
           {
               safeslq += "Ctext='" + model.Ctext + "',";
           }
           safeslq += " Cstate=" + (model.Cstate == 1 ? 1 : 0) + " ";
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
       #region 获取评论信息 值
       /// <summary>
       /// 获取评论信息 值
       /// </summary>
       /// <param name="strValue"></param>
       /// <param name="strID"></param>
       /// <returns></returns>
       public object GetCommentValueByID(string strValue, string strID)
       {
           string safesql = "";
           safesql = "select " + strValue + " from MS_ForumComment where ID='" + strID + "'";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion
       #region 更新评论信息状态
       /// <summary>
       /// 更新评论信息状态
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public bool UpdateCommentState(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetCommentValueByID("Cstate", strID));
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
           strSql.Append(" UPDATE MS_ForumComment ");
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
       #region 更新评论审核状态 1为审核通过  默认0 未通过
       /// <summary>
       /// 更新评论审核状态 1为审核通过  默认0 未通过
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public bool UpdateCommentReview(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetCommentValueByID("Review", strID));
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
           strSql.Append(" UPDATE MS_ForumComment ");
           strSql.Append(" SET Review = @Review ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strID),
                new System.Data.SqlClient.SqlParameter("@Review", state)
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
       #region 获取有效的评论列表
       /// <summary>
       /// 获取有效的评论列表
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetCommentList(string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select a.*,b.NickName,b.HeadImg,c.TopicTitle,d.FTitle ");
           strSql.Append(" from dbo.MS_ForumComment a,MS_Customers b,MS_ForumTopic c,MS_ForumSet d ");
           strSql.Append(" where a.Cstate=0 and a.[UID]=b.ID and a.TID=c.ID and c.FID=d.ID and a.UpID='' ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by a.AddTime desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 获取有效的评论列表回复
       /// <summary>
       /// 获取有效的评论列表回复
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetRepCommentList(string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select a.*,b.NickName,b.HeadImg ");
           strSql.Append(" from dbo.MS_ForumComment a,MS_Customers b ");
           strSql.Append(" where a.Cstate=0 and a.[UID]=b.ID and a.UpID!='' ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by a.AddTime desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 获取消息总数
       /// <summary>
       /// 获取消息总数
       /// </summary>
       /// <param name="strUID">户编号</param>
       /// <returns></returns>
       public int GetCommentCountByUID(string strUID)
       {
           string safesql = "select count(ID) from MS_ForumComment where Cstate=0 ";
           if (strUID != null && strUID != "")
           {
               safesql += " AND [UID]='" + strUID + "' ";
           }
           int count = 0;
           try
           {
               count = Convert.ToInt32(DbHelperSQL.GetSingle(safesql.ToString()));
           }
           catch (Exception)
           {
               count = 0;
           }
           return count;
       }
       #endregion
       #region 获取评论详细
       /// <summary>
       /// 获取评论详细
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public DataSet GetCommentDetail(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM MS_ForumComment ");
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
