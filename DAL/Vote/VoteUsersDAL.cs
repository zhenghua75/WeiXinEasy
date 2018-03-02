using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.Vote;
using Maticsoft.DBUtility;

namespace DAL.Vote
{
   public class VoteUsersDAL
    {
       public VoteUsersDAL() { ;}

       #region 添加用户投票
       /// <summary>
       /// 添加用户投票
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddVoteUsers(VoteUsers model)
       {
           string sql = @"INSERT INTO [Vote_Users]
                        ([ID],[VoteID],[SubjectID],[OpenID],[UserName],[AddTime],[UserIP],[IsDel])
                 VALUES
                        (@ID,@VoteID,@SubjectID,@OpenID,@UserName,@AddTime,@UserIP,@IsDel)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@VoteID", model.VoteID),
                new System.Data.SqlClient.SqlParameter("@SubjectID", model.SubjectID),
                new System.Data.SqlClient.SqlParameter("@OpenID", model.OpenID),
                new System.Data.SqlClient.SqlParameter("@UserName", model.UserName),
                new System.Data.SqlClient.SqlParameter("@AddTime", DateTime.Now),
                new System.Data.SqlClient.SqlParameter("@UserIP",model.UserIP),
                new System.Data.SqlClient.SqlParameter("@IsDel", model.IsDel==1?1:0)
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

       #region 用户投票更新
       /// <summary>
       /// 用户投票更新
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateVoteUsers(VoteUsers model)
       {
           string safesql = "";
           safesql = "update Vote_Users set ";
           if (model.VoteID != null && model.VoteID != "")
           {
               safesql += "[VoteID]='" + model.VoteID + "',";
           }
           if (model.SubjectID != null && model.SubjectID != "")
           {
               safesql += "[SubjectID]='" + model.SubjectID + "',";
           }
           if (model.OpenID != null && model.OpenID != "")
           {
               safesql += "[OpenID]='" + model.OpenID + "',";
           }
           if (model.UserName != null && model.UserName != "")
           {
               safesql += "[UserName]='" + model.UserName + "',";
           }
           safesql += "[IsDel]=" + (model.IsDel==1?1:0);
           safesql += " where id='" + model.ID + "'";
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

       #region 获取用户投票属性
       /// <summary>
       /// 获取用户投票属性
       /// </summary>
       /// <param name="value"></param>
       /// <param name="voteID"></param>
       /// <returns></returns>
       public object GetVoteUserValue(string value, string voteID)
       {
           string safesql = "";
           safesql = "select " + value + " from Vote_Users where ID='" + voteID + "'";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion

       #region 更新用户投票状态
       /// <summary>
       /// 更新用户投票状态
       /// </summary>
       /// <param name="VoteID"></param>
       /// <returns></returns>
       public bool UpdateVoteUserIsDel(string VoteID)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetVoteUserValue("State", VoteID));
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
           strSql.Append(" UPDATE Vote_Users ");
           strSql.Append(" SET IsDel = @State ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", VoteID),
                new System.Data.SqlClient.SqlParameter("@State", state)
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

       #region 获取有效的用户投票信息
       /// <summary>
       /// 获取有效的用户投票信息
       /// </summary>
       /// <param name="where"></param>
       /// <returns></returns>
       public DataSet GetVoteUserlist(string where)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select a.*,b.Title as Title,b.Ico as Ico,b.ContentDesc as ContentDesc,");
           strSql.Append(" c.Subject as Subject,c.Content as subcontent from Vote_Users a , VOTE_Options b,VOTE_Subject c");
           strSql.Append(" where a.voteid=b.ID and c.ID=a.SubjectID and a.IsDel=1 and b.IsDel=1 ");
           if (where.Trim() != null && where.Trim()!="")
           {
               strSql.Append(where);
           }
           strSql.Append(" order by a.AddTime asc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion

       #region 判断选项是否有重复
       /// <summary>
       /// 判断选项是否有重复
       /// </summary>
       /// <param name="voteid"></param>
       /// <param name="openid"></param>
       /// <returns></returns>
       public bool VoteIsRepeat(string voteid, string openid)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT count(ID) ");
           strSql.Append(" FROM Vote_Users ");
           strSql.Append(" WHERE [VoteID] = @VoteID ");
           strSql.Append(" AND [OpenID] = @OpenID ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@VoteID", voteid),
                    new System.Data.SqlClient.SqlParameter("@OpenID", openid)
                };
           return DbHelperSQL.Exists(strSql.ToString(), paras.ToArray());
       }
       #endregion

       #region 判断主题是否重复
       /// <summary>
       /// 判断主题是否重复
       /// </summary>
       /// <param name="subid"></param>
       /// <param name="openid"></param>
       /// <returns></returns>
       public bool SubjectIsRepeat(string subid, string openid)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT count(ID) ");
           strSql.Append(" FROM Vote_Users ");
           strSql.Append(" WHERE [SubjectID] = @SubjectID ");
           strSql.Append(" AND [OpenID] = @OpenID ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SubjectID", subid),
                    new System.Data.SqlClient.SqlParameter("@OpenID", openid)
                };
           return DbHelperSQL.Exists(strSql.ToString(), paras.ToArray());
       }
       #endregion
       #region 根据号码判断选项是否重复
       /// <summary>
       /// 根据号码判断选项是否重复
       /// </summary>
       /// <param name="subid"></param>
       /// <param name="phonenum"></param>
       /// <returns></returns>
       public bool SubjectIsRepeatUser(string subid, string phonenum)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT count(ID) ");
           strSql.Append(" FROM Vote_Users ");
           strSql.Append(" WHERE [SubjectID] = @SubjectID ");
           strSql.Append(" AND [UserName] = @UserName ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SubjectID", subid),
                    new System.Data.SqlClient.SqlParameter("@UserName", phonenum)
                };
           return DbHelperSQL.Exists(strSql.ToString(), paras.ToArray());
       }
       #endregion
       #region 判断用户提交是否重复
       /// <summary>
       /// 判断用户提交是否重复
       /// </summary>
       /// <param name="subid"></param>
       /// <param name="UserName"></param>
       /// <param name="Uip"></param>
       /// <returns></returns>
       public bool UsreIsRepeat(string subid, string UserName)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT count(ID) ");
           strSql.Append(" FROM Vote_Users ");
           strSql.Append(" WHERE [SubjectID] = @SubjectID ");
           strSql.Append(" AND [UserName] = @UserName");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SubjectID", subid),
                    new System.Data.SqlClient.SqlParameter("@UserName", UserName)
                };
           return DbHelperSQL.Exists(strSql.ToString(), paras.ToArray());
       }
       #endregion
       /// <summary>
       /// 根据IP获取投票总数
       /// </summary>
       /// <param name="strUip"></param>
       /// <returns></returns>
       public int UserIsRepeat(string strUip,string strVoteID)
       {
           string safesql = "";
           safesql = "select count(ID) from Vote_Users where UserIP='" + strUip + "' and VoteID='"+strVoteID+"' ";
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

       #region 获取我的投票列表
       /// <summary>
       /// 获取我的投票列表
       /// </summary>
       /// <param name="where"></param>
       /// <returns></returns>
       public DataSet GetMyVoteList(string where)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select a.ID as MyID,a.VoteID as VoteID,a.SubjectID as SubjectID,");
           strSql.Append(" a.OpenID as OpenID,a.UserName as UserName,a.AddTime as AddTime,b.ID as ID,b.Title as Title,");
           strSql.Append(" b.Ico as Ico,b.ContentDesc as ContentDesc ");
           strSql.Append(" from Vote_Users a left join VOTE_Options b on(a.voteid=b.ID) ");
           strSql.Append(" where a.IsDel=1 and b.IsDel=1 ");
           if (!string.IsNullOrEmpty(where))
           {
               strSql.Append(where);
           }
           strSql.Append(" order by a.AddTime asc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
    }
}
