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
    /// 帖子操作类
    /// </summary>
   public class MSForumTopicDAL
    {
       /// <summary>
        /// 帖子操作类
       /// </summary>
       public MSForumTopicDAL() { ;}
       #region 添加帖子
        /// <summary>
       /// 添加帖子
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddMSForumTopic(MSForumTopic model)
       {
           string sql = @"INSERT INTO [MS_ForumTopic]
                        ([ID],[FID],[UID],[TopicTitle],[TopicDesc],[TopicState],[Treview],[AddTime])
                 VALUES
                        (@ID,@FID,@UID,@TopicTitle,@TopicDesc,@TopicState,@Treview,@AddTime)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@FID", model.FID),
                new System.Data.SqlClient.SqlParameter("@UID", model.UID),
                new System.Data.SqlClient.SqlParameter("@TopicTitle", model.TopicTitle),
                new System.Data.SqlClient.SqlParameter("@TopicDesc", model.TopicDesc),
                new System.Data.SqlClient.SqlParameter("@TopicState", (model.TopicState==0?0:1)),
                new System.Data.SqlClient.SqlParameter("@Treview",(model.Treview==1?1:0)),
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
       #region 帖子更新
       /// <summary>
       /// 帖子更新
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateMSForumTopic(MSForumTopic model)
       {
           string safeslq = "";
           safeslq = "UPDATE MS_ForumTopic SET ";
           if (model.UID != null && model.UID != "")
           {
               safeslq += "[UID]='" + model.UID + "',";
           }
           if (model.FID != null && model.FID != "")
           {
               safeslq += "[FID]='" + model.FID + "',";
           }
           if (model.TopicTitle != null && model.TopicTitle != "")
           {
               safeslq += "TopicTitle='" + model.TopicTitle + "',";
           }
           if (model.TopicDesc != null && model.TopicDesc.ToString() != "")
           {
               safeslq += "TopicDesc='" + model.TopicDesc + "',";
           }
           safeslq += " TopicState=" + (model.TopicState == 1 ? 1 : 0) + " ";
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
       #region 获取帖子属性
       /// <summary>
       /// 获取帖子属性
       /// </summary>
       /// <param name="strValue"></param>
       /// <param name="strID"></param>
       /// <returns></returns>
       public object GetMSForumTopicValueByID(string strValue,string strID)
       {
           string safesql = "";
           safesql = "select " + strValue + " from MS_ForumTopic where ID='" + strID + "'";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion
       #region 更新帖子状态
       /// <summary>
       /// 更新帖子状态
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public bool UpdateMSForumTopicState(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetMSForumTopicValueByID("TopicState", strID));
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
           strSql.Append(" UPDATE MS_ForumTopic ");
           strSql.Append(" SET TopicState = @TopicState ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strID),
                new System.Data.SqlClient.SqlParameter("@TopicState", state)
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
       #region 更新帖子审核情况
       /// <summary>
       /// 更新帖子审核情况 默认0：未审核 1审核通过
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public bool UpdateMSForumTopicTreview(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetMSForumTopicValueByID("Treview", strID));
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
           strSql.Append(" UPDATE MS_ForumTopic ");
           strSql.Append(" SET Treview = @Treview ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strID),
                new System.Data.SqlClient.SqlParameter("@Treview", state)
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
       #region 获取有效的帖子列表
       /// <summary>
       /// 获取有效的帖子列表
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetMSForumTopicList(string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select a.ID,a.FID,a.[UID],a.TopicDesc,a.TopicState,a.TopicTitle, ");
           strSql.Append(" case a.Treview when 0 then '未通过' else '通过' end Treview,a.AddTime, ");
           strSql.Append(" b.FTitle,c.Phone,c.NickName,c.HeadImg, ");
           strSql.Append(" (select COUNT(id) from MS_ForumTopicLove where TID=a.ID and Tlike>0) Tlike, ");
           strSql.Append(" (select COUNT(id) from MS_ForumTopicLove where TID=a.ID and Tlove>0) Tlove, ");
           strSql.Append(" (select COUNT(id) from MS_ForumComment where UpID='' and TID=a.ID and Cstate=0) Ccount ");
           strSql.Append(" from MS_ForumTopic a,MS_ForumSet b,MS_Customers c ");
           strSql.Append(" where a.TopicState=0 and a.FID=b.ID and a.[UID]=c.ID ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by a.AddTime desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 获取有效的帖子列表
       /// <summary>
       /// 获取有效的帖子列表
       /// </summary>
       /// <param name="strWhere">条件</param>
       /// <param name="top">每页显示条数</param>
       /// <returns></returns>
       public DataSet GetMSForumTopicList(string strWhere,int top)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select ");
           if (top > 0)
           {
               strSql.Append(" TOP "+top+" ");
           }
           strSql.Append(" a.ID,a.FID,a.[UID],a.TopicDesc,a.TopicState,a.TopicTitle,a.Treview,a.AddTime, ");
           strSql.Append(" b.FTitle,c.Phone,c.NickName,c.HeadImg, ");
           strSql.Append(" (select COUNT(id) from MS_ForumTopicLove where TID=a.ID and Tlike>0) Tlike, ");
           strSql.Append(" (select COUNT(id) from MS_ForumTopicLove where TID=a.ID and Tlove>0) Tlove, ");
           strSql.Append(" (select COUNT(id) from MS_ForumComment where [UpID]='' " +
                            " and [TID]=a.ID and Cstate=0) Ccount ");
           strSql.Append(" from MS_ForumTopic a,MS_ForumSet b,MS_Customers c ");
           strSql.Append(" where a.TopicState=0 and a.FID=b.ID and a.[UID]=c.ID ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by a.AddTime desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 获取点赞或喜欢 的帖子列表
       /// <summary>
       /// 获取点赞或喜欢 的帖子列表
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetTopicByLikeOrLove(string strWhere,int top)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select ");
           if (top > 0)
           {
               strSql.Append(" TOP " + top + " ");
           }
           strSql.Append(" a.ID,a.FID,a.[UID],a.TopicDesc,a.TopicState,a.TopicTitle,a.Treview,a.AddTime, ");
           strSql.Append(" b.FTitle,c.Phone,c.NickName,c.HeadImg, ");
           strSql.Append(" (select COUNT(id) from MS_ForumTopicLove where TID=a.ID and [UID]=a.[UID]" +
                            " and Tlike>0) Tlike, ");
           strSql.Append(" (select COUNT(id) from MS_ForumTopicLove where TID=a.ID and [UID]=a.[UID]" +
                            " and Tlove>0) Tlove ");
           strSql.Append(" from MS_ForumTopic a,MS_ForumSet b,MS_Customers c,MS_ForumTopicLove d ");
           strSql.Append(" where a.TopicState=0 and a.FID=b.ID and a.[UID]=c.ID and d.[UID]=a.[UID] ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by a.AddTime desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 获取帖子详细
       /// <summary>
       /// 获取帖子详细
       /// </summary>
       /// <param name="strTID">帖子编号</param>
       /// <returns></returns>
       public DataSet GetTopicDetail(string strTID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM MS_ForumTopic ");
           strSql.Append(" WHERE [ID] = @ID");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", strTID)
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion
       #region 获取帖子总数
       /// <summary>
       /// 获取帖子总数
       /// </summary>
       /// <param name="strFID">论坛编号</param>
       /// <returns></returns>
       public object GetMSForumTopicCount(string strFID)
       {
           string safesql = "";
           safesql = "select count(ID) from MS_ForumTopic where TopicState=0 and Treview=1 and FID='" + strFID + "'";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion
       #region 获取帖子总数
       /// <summary>
       /// 获取帖子总数
       /// </summary>
       /// <param name="strUID">用户编号</param>
       /// <returns></returns>
       public int GetTopicCountByUID(string strUID)
       {
           string safesql = "";
           safesql = "select count(ID) from MS_ForumTopic where TopicState=0 and Treview=1 and [UID]='" + strUID + "'";
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
       #region 判断帖子是否存在
       /// <summary>
       /// 判断帖子是否存在
       /// </summary>
       /// <param name="strTitle">标题</param>
       /// <param name="strUID">户编号</param>
       /// <returns></returns>
       public bool ExistMSForumTopic(string strTitle,string strFID, string strUID)
       {
           string strSql = string.Empty;
           strSql += " SELECT count(ID) FROM MS_ForumTopic  ";
           strSql += " where TopicState=0 ";
           if (strUID.Trim() != null && strUID.Trim() != "")
           {
               strSql += " and [UID] ='" + strUID + "' ";
           }
           if (strFID.Trim() != null && strFID.Trim() != "")
           {
               strSql += " and [FID] ='" + strFID + "' ";
           }
           if (strTitle.Trim() != null && strTitle.Trim() != "")
           {
               strSql += " and [TopicTitle] ='" + strTitle + "' ";
           }
           return DbHelperSQL.Exists(strSql.ToString());
       }
       #endregion
    }
}
