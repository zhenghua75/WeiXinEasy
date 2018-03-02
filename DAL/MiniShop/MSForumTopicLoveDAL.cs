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
    /// 帖子点赞、喜欢 操作类
    /// </summary>
   public class MSForumTopicLoveDAL
    {
       /// <summary>
        /// 帖子点赞、喜欢 操作类
       /// </summary>
       public MSForumTopicLoveDAL() { ;}
       #region 更新点赞、喜欢 数据
       /// <summary>
       /// 更新点赞、喜欢 数据
       /// </summary>
       /// <param name="strTID">帖子编号</param>
       /// <param name="strUID">用户编号</param>
        /// <param name="strLikeOrLove">点赞(tlove)或喜欢(tlike)</param>
       public bool UpdateTloveOrLike(string strTID, string strUID,string strLikeOrLove)
       {
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetTloveOrLike(strTID, strUID, strLikeOrLove));
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
               case 1:
                   state = 0;
                   break;
           }
           string safesql = " update MS_ForumTopicLove ";
           if (strLikeOrLove != null && strLikeOrLove != "")
           {
               switch (strLikeOrLove.ToLower().Trim())
               {
                   case "tlike":
                       safesql += " set tlike=" + state;
                       break;
                   case "tlove":
                       safesql += " set tlove=" + state;
                       break;
               }
           }
           if (strTID != null && strTID != "")
           {
               if (safesql.Trim().ToLower().Contains("where"))
               {
                   safesql += " AND ";
               }
               else
               {
                   safesql += " WHERE ";
               }
               safesql += " TID='" + strTID + "' ";
           }
           if (strUID != null && strUID != "")
           {
               if (safesql.Trim().ToLower().Contains("where"))
               {
                   safesql += " AND ";
               }
               else
               {
                   safesql += " WHERE ";
               }
               safesql += " [UID]='" + strUID + "' ";
           }
           int rowsAffected = DbHelperSQL.ExecuteSql(safesql.ToString());
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
       #region 添加喜欢或点赞
       /// <summary>
       /// 添加喜欢或点赞
       /// </summary>
       /// <param name="strTID">帖子编号</param>
       /// <param name="strUID">用户编号</param>
       /// <param name="strLikeOrLove">点赞(tlove)或喜欢(tlike)</param>
       /// <returns></returns>
       public bool AddTloveOrLike(string strTID, string strUID, string strLikeOrLove)
       {
           string strID = Guid.NewGuid().ToString("N").ToUpper(); 
           string safesql = string.Empty;
           safesql += " insert into MS_ForumTopicLove ([ID],[TID],[UID],";
           if (strLikeOrLove != null && strLikeOrLove != "")
           {
               switch (strLikeOrLove.ToLower().Trim())
               {
                   case "tlike":
                       safesql += " Tlike,";
                       break;
                   case "tlove":
                       safesql += " Tlove,";
                       break;
               }
           }
           safesql += " AddTime)values('" + 
               strID + "','" + strTID + "','" + strUID + "'," + 1 + ",'" + DateTime.Now + "')";
           int rowsAffected = DbHelperSQL.ExecuteSql(safesql.ToString());
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
       #region 获取帖子点赞属性
       /// <summary>
       /// 获取帖子点赞属性
       /// </summary>
       /// <param name="strTID">帖子编号</param>
       /// <param name="strUID">用户编号</param>
       /// <param name="strLikeOrLove">喜欢:like 或 点赞:love</param>
       /// <returns></returns>
       public object GetTloveOrLike(string strTID, string strUID, string strLikeOrLove)
       {
           string safesql = "select ";
           if (strLikeOrLove != null && strLikeOrLove != "")
           {
               switch (strLikeOrLove.ToLower().Trim())
               {
                   case "tlike":
                       safesql += " tlike ";
                       break;
                   case "tlove":
                       safesql += " tlove ";
                       break;
               }
           }
           safesql  += " from MS_ForumTopicLove ";
           if (strTID != null && strTID != "")
           {
               if (safesql.Trim().ToLower().Contains("where"))
               {
                   safesql += " AND ";
               }
               else
               {
                   safesql += " WHERE ";
               }
               safesql += " TID='" + strTID + "' ";
           }
           if (strUID != null && strUID != "")
           {
               if (safesql.Trim().ToLower().Contains("where"))
               {
                   safesql += " AND ";
               }
               else
               {
                   safesql += " WHERE ";
               }
               safesql += " [UID]='" + strUID + "' ";
           }
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion
       #region 获取帖子点赞或喜欢  总数
       /// <summary>
       /// 获取点赞或喜欢  总数
       /// </summary>
       /// <param name="strTID">帖子编号</param>
       /// <param name="strLikeOrLove">点赞(tlove)或喜欢(tlike)</param>
       /// <returns></returns>
       public int GetLoveOrLikeCount(string strTID, string strLikeOrLove)
       {
           string safesql = "select count(ID) from MS_ForumTopicLove ";
           if (strLikeOrLove != null && strLikeOrLove != "")
           {
               if (safesql.Trim().ToLower().Contains("where"))
               {
                   safesql += " AND ";
               }
               else
               {
                   safesql += " WHERE ";
               }
               switch (strLikeOrLove.ToLower().Trim())
               {
                   case "tlike":
                       safesql += " tlike >0 ";
                       break;
                   case "tlove":
                       safesql += " tlove >0 ";
                       break;
               }
           }
           if (strTID != null && strTID != "")
           {
               if (safesql.Trim().ToLower().Contains("where"))
               {
                   safesql += " AND ";
               }
               else
               {
                   safesql += " WHERE ";
               }
               safesql += " TID='" + strTID + "' ";
           }
           int count = 0;
           try
           {
               count=Convert.ToInt32(DbHelperSQL.GetSingle(safesql.ToString()));
           }
           catch (Exception)
           {
               count = 0;
           }
           return count;
       }
       #endregion
       #region 获取点赞或喜欢 总数
       /// <summary>
       /// 获取点赞或喜欢 总数
       /// </summary>
       /// <param name="strUID">户编号</param>
       /// <param name="strLikeOrLove">点赞(tlove)或喜欢(tlike)</param>
       /// <returns></returns>
       public int GetLoveOrLikeCountByUID(string strUID, string strLikeOrLove)
       {
           string safesql = "select count(ID) from MS_ForumTopicLove ";
           if (strLikeOrLove != null && strLikeOrLove != "")
           {
               if (safesql.Trim().ToLower().Contains("where"))
               {
                   safesql += " AND ";
               }
               else
               {
                   safesql += " WHERE ";
               }
               switch (strLikeOrLove.ToLower().Trim())
               {
                   case "tlike":
                       safesql += " tlike >0 ";
                       break;
                   case "tlove":
                       safesql += " tlove >0 ";
                       break;
               }
           }
           if (strUID != null && strUID != "")
           {
               if (safesql.Trim().ToLower().Contains("where"))
               {
                   safesql += " AND ";
               }
               else
               {
                   safesql += " WHERE ";
               }
               safesql += " [UID]='" + strUID + "' ";
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
       #region 判断信息是否存在
       /// <summary>
       /// 判断信息是否存在
       /// </summary>
       /// <param name="strTID">帖子编号</param>
       /// <param name="strUID">户编号</param>
       /// <returns></returns>
       public bool ExisTlikeOrlove(string strTID, string strUID)
       {
           string strSql = string.Empty;
           strSql += " SELECT count(ID) FROM MS_ForumTopicLove  ";
           if (strUID != null && strUID!= "")
           {
               if (strSql.Trim().ToLower().Contains("where"))
               {
                   strSql += " AND ";
               }
               else
               {
                   strSql += " WHERE ";
               }
               strSql += "  [UID] ='" + strUID + "' ";
           }
           if (strTID != null && strTID != "")
           {
               if (strSql.Trim().ToLower().Contains("where"))
               {
                   strSql += " AND ";
               }
               else
               {
                   strSql += " WHERE ";
               }
               strSql += "  [TID] ='" + strTID + "' ";
           }
           return DbHelperSQL.Exists(strSql.ToString());
       }
       #endregion
    }
}
