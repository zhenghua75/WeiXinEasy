using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DBUtility;
using Maticsoft.DBUtility;

namespace DAL.BBS
{
   public class BBSGuestBookDAL
   {
       #region 返回所有留言列表
       /// <summary>
       /// 返回所有留言列表
       /// </summary>
       /// <param name="where">查询条件</param>
       /// <returns></returns>
       public DataSet GetGuestBookList(string where)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * from BBS_GuestBook ");
           if (!string.IsNullOrEmpty(where))
           {
               strSql.Append(" WHERE " + where);
           }
           DataSet ds = DbHelperSQL.Query(strSql.ToString() + " order by CreateTime desc");
           return ds;
       }
       #endregion

       #region 返回留言详细信息
       /// <summary>
       /// 返回留言详细信息
       /// </summary>
       /// <param name="gbid">留言ID</param>
       /// <returns></returns>
       public DataSet GetGuestBookDetail(string gbid)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM BBS_GuestBook ");
           strSql.Append(" WHERE [ID] = @ID ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", gbid),
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion

       #region 回复留言
       /// <summary>
       /// 回复留言
       /// </summary>
       /// <param name="wd">回复内容</param>
       /// <param name="id">留言ID</param>
       /// <returns></returns>
       public bool SaveReplay(string wd,string id)
       {
           string safesql = "update BBS_GuestBook set replay='"+wd+"' where id='"+id+"'";
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

       #region 更新留言信息状态
       /// <summary>
       /// 更新留言信息状态
       /// </summary>
       /// <param name="id">留言ID</param>
       /// <returns></returns>
       public bool UpdateState(string id)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" update BBS_GuestBook set [state]=1 ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", id)
            };
           int rowsAffected = DbHelperSQL.ExecuteSql(strSql.ToString(),paras);
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

       #region 添加留言信息
       /// <summary>
       /// 添加留言信息
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddGuestBook(Model.BBS.BBS_GuestBook model)
       {
           string sql = @"INSERT INTO [BBS_GuestBook]
                        (ID,UserName,UserMobile,[Content],Replay,[State],CreateTime,SiteCode)
                 VALUES
                        (@ID,@UserName,@UserMobile,@Content,@Replay,@State,@CreateTime,@SiteCode)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@UserName", model.UserName),
                new System.Data.SqlClient.SqlParameter("@UserMobile", model.UserMobile),
                new System.Data.SqlClient.SqlParameter("@Content", model.Content),
                new System.Data.SqlClient.SqlParameter("@Replay", model.Replay),
                new System.Data.SqlClient.SqlParameter("@State", model.State),
                new System.Data.SqlClient.SqlParameter("@CreateTime", model.CreateTime),
                new System.Data.SqlClient.SqlParameter("@SiteCode", model.SiteCode)
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

       #region 判断留言是否重复
       /// <summary>
       /// 判断留言是否重复
       /// </summary>
       /// <param name="msg"></param>
       /// <returns></returns>
       public bool ExistMsg(string msg)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT count(ID) ");
           strSql.Append(" FROM BBS_GuestBook ");
           strSql.Append(" WHERE [Content] = @Content ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@Content", msg)
                };
           return DbHelperSQL.Exists(strSql.ToString(), paras.ToArray());
       }
       #endregion
   }
}
