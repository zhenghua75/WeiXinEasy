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
    /// 帖子图集操作类
    /// </summary>
   public class MSForumTopicAtlasDAL
    {
       /// <summary>
        /// 帖子图集操作类
       /// </summary>
       public MSForumTopicAtlasDAL() { ;}
       #region 添加帖子图集
        /// <summary>
       /// 添加帖子图集
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddMSForumTopicAtlas(MSForumTopicAtlas model)
       {
           string sql = @"INSERT INTO [MS_ForumTopicAtlas]
                        ([ID],[TID],[ImgName],[ImgUrl],[ImgState],[AddTime])
                 VALUES
                        (@ID,@TID,@ImgName,@ImgUrl,@ImgState,@AddTime)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@TID", model.TID),
                new System.Data.SqlClient.SqlParameter("@ImgName", model.ImgName),
                new System.Data.SqlClient.SqlParameter("@ImgUrl", model.ImgUrl),
                new System.Data.SqlClient.SqlParameter("@ImgState", (model.ImgState==0?0:1)),
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
       #region 更新图集
       /// <summary>
       /// 更新图集
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateMSForumTopicAtlas(MSForumTopicAtlas model)
       {
           string safeslq = "";
           safeslq = "UPDATE MS_ForumTopicAtlas SET ";
           if (model.TID != null && model.TID != "")
           {
               safeslq += "[TID]='" + model.TID + "',";
           }
           if (model.ImgName != null && model.ImgName != "")
           {
               safeslq += "ImgName='" + model.ImgName + "',";
           }
           if (model.ImgUrl != null && model.ImgUrl.ToString() != "")
           {
               safeslq += "ImgUrl='" + model.ImgUrl + "',";
           }
           safeslq += " ImgState=" + (model.ImgState == 1 ? 1 : 0) + " ";
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
       #region 获取图集属性
       /// <summary>
       /// 获取图集属性
       /// </summary>
       /// <param name="strValue"></param>
       /// <param name="strID"></param>
       /// <returns></returns>
       public object GetMSForumTopicAtlasValue(string strValue, string strID)
       {
           string safesql = "";
           safesql = "select " + strValue + " from MS_ForumTopicAtlas where ID='" + strID + "'";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion
       #region 更新图集状态
       /// <summary>
       /// 更新图集状态
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public bool UpdateMSForumTopicAtlasState(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetMSForumTopicAtlasValue("ImgState", strID));
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
           strSql.Append(" UPDATE MS_ForumTopicAtlas ");
           strSql.Append(" SET ImgState = @ImgState ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strID),
                new System.Data.SqlClient.SqlParameter("@ImgState", state)
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
       #region 获取有效的图集列表
       /// <summary>
       /// 获取有效的图集列表
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetMSFTAtlasList(string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select * ");
           strSql.Append(" from MS_ForumTopicAtlas ");
           strSql.Append(" where ImgState=0 ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by AddTime desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
    }
}
