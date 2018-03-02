using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.JC;
using Maticsoft.DBUtility;

namespace DAL.JC
{
   public class JC_ScoreDAL
    {
       public JC_ScoreDAL() { ;}

       #region 添加竞猜结果
       /// <summary>
       /// 添加竞猜结果
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddJCScore(JC_Score model)
       {
           string sql = @"INSERT INTO [JC_Score]
                        ([ID],[QuizId],[SiteCode],[OpenID],[GuessScore],[AddTime],[State])
                 VALUES
                        (@ID,@QuizId,@SiteCode,@OpenID,@GuessScore,@AddTime,@State)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@QuizId", model.QuizId),
                new System.Data.SqlClient.SqlParameter("@SiteCode", model.SiteCode),
                new System.Data.SqlClient.SqlParameter("@OpenID", model.OpenID),
                new System.Data.SqlClient.SqlParameter("@GuessScore", model.GuessScore),
                new System.Data.SqlClient.SqlParameter("@AddTime", DateTime.Now),
                new System.Data.SqlClient.SqlParameter("@State", (model.State==1?1:0)),
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

       #region 更新竞猜结果信息
       /// <summary>
       /// 更新竞猜结果信息
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateJCScore(JC_Score model)
       {
           string safeslq = "";
           safeslq = "UPDATE JC_Score SET ";
           if (model.QuizId != null && model.QuizId != "")
           {
               safeslq += "QuizId='" + model.QuizId + "',";
           }
           if (model.SiteCode != null && model.SiteCode != "")
           {
               safeslq += "SiteCode='" + model.SiteCode + "',";
           }
           if (model.OpenID != null && model.OpenID!= "")
           {
               safeslq += "OpenID='" + model.OpenID + "',";
           }
           if (model.GuessScore != null && model.GuessScore != "")
           {
               safeslq += "GuessScore='" + model.GuessScore + "',";
           }
           if (model.State.ToString() != null && model.State.ToString() != "")
           {
               safeslq += "State=" + (model.State==1?1:0) + ",";
           }
           safeslq += "AddTime='" + DateTime.Now + "' where ID='" + model.ID + "'";
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

       #region 根据ID获取相对于的信息
       /// <summary>
       /// 根据ID获取相对于的信息
       /// </summary>
       /// <param name="value"></param>
       /// <param name="jcsID"></param>
       /// <returns></returns>
       public object GetJCScoreValueById(string value, string jcsID)
       {
           string safesql = "";
           safesql = "select [" + value + "] from JC_Score where ID='" + jcsID + "'";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion

       #region 根据ID修改竞猜信息状态
       /// <summary>
       /// 根据ID修改竞猜信息状态
       /// </summary>
       /// <param name="jcID"></param>
       /// <returns></returns>
       public bool UpdateJCScoreState(string jcID)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetJCScoreValueById("State", jcID));
           }
           catch (Exception)
           {
               state = 0;
           }
           if (state == 1)
           {
               return true;
           }
           else
           {
               strSql.Append(" UPDATE JC_Score ");
               strSql.Append(" SET State = 1 ");
               strSql.Append(" WHERE ID = @ID ");
               System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
                {
                    new System.Data.SqlClient.SqlParameter("@ID", jcID)
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
       }
       #endregion

       #region 获取所有竞猜结果
       /// <summary>
       /// 获取所有竞猜结果
       /// </summary>
       /// <param name="where"></param>
       /// <returns></returns>
       public DataSet GetJCScoreList(string where)
       {
           //string safesql=SELECT a.*,b.Name as Name,b.HomeTeam as HomeTeam,b.HomeTeamImg as HomeTeamImg,
           //b.VisitingTeam as VisitingTeam,b.VisitingTeamImg as VisitingTeamImg,b.StartTime as StartTime  
           //   FROM JC_Score a LEFT JOIN JC_Quiz b ON (a.QuizID=b.ID) 
           StringBuilder strSql = new StringBuilder();
           //strSql.Append(" SELECT * from JC_Score ");
           //if (!string.IsNullOrEmpty(where))
           //{
           //    strSql.Append(" WHERE " + where);
           //}
           strSql.Append("  SELECT a.*,b.Name as Name,b.HomeTeam as HomeTeam,b.HomeTeamImg as HomeTeamImg," +
           "b.VisitingTeam as VisitingTeam,b.VisitingTeamImg as VisitingTeamImg,b.StartTime as StartTime,  " +
           "ISNULL(b.RightScore,'未公布结果') as RightScore ," + 
            " CASE WHEN b.QuizType = '0' AND a.GuessScore = '1:0' THEN b.HomeTeam + '胜' " +
		    " WHEN b.QuizType = '0' AND a.GuessScore = '0:1' THEN b.VisitingTeam + '胜' " +
		    " WHEN b.QuizType = '0' AND a.GuessScore = '0:0' THEN '平局' " +
		    " WHEN b.QuizType = '1' THEN a.GuessScore "+
	        " END AS QuizREsult FROM JC_Score a LEFT JOIN JC_Quiz b ON (a.QuizID=b.ID) ");
           if (!string.IsNullOrEmpty(where) && where.Trim() != null && where.Trim() != "")
           {
               strSql.Append(" WHERE " + where);
           }
           strSql.Append(" order by StartTime DESC ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion

       #region 获取所有有效的竞猜结果
       /// <summary>
       /// 获取所有有效的竞猜结果
       /// </summary>
       /// <param name="where"></param>
       /// <returns></returns>
       public DataSet GetJCScoreListByState(string where)
       {
           StringBuilder strSql = new StringBuilder();
           //strSql.Append(" SELECT * from JC_Score WHERE State=0 ");
           //if (!string.IsNullOrEmpty(where))
           //{
           //    strSql.Append(" AND " + where);
           //}
           strSql.Append("  SELECT a.*,CASE a.State WHEN '0' THEN '未发放' WHEN '1' THEN '已发放' END AS CouponState,b.Name as Name,b.HomeTeam as HomeTeam,b.HomeTeamImg as HomeTeamImg," +
           "b.VisitingTeam as VisitingTeam,b.VisitingTeamImg as VisitingTeamImg,b.StartTime as StartTime,  " +
           "b.RightScore as RightScore FROM JC_Score a LEFT JOIN JC_Quiz b ON (a.QuizID=b.ID) ");
           if (!string.IsNullOrEmpty(where) && where.Trim() != null && where.Trim()!="")
           {
               strSql.Append(" WHERE " + where);
           }
           strSql.Append(" order by StartTime asc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion

       #region 获取未发放有效的竞猜结果
       /// <summary>
       /// 获取所有有效的竞猜结果
       /// </summary>
       /// <param name="where"></param>
       /// <returns></returns>
       public DataSet GetJCScoreListByStateNO(string where)
       {
           StringBuilder strSql = new StringBuilder();
           //strSql.Append(" SELECT * from JC_Score WHERE State=0 ");
           //if (!string.IsNullOrEmpty(where))
           //{
           //    strSql.Append(" AND " + where);
           //}
           strSql.Append("  SELECT a.*,b.Name as Name,b.HomeTeam as HomeTeam,b.HomeTeamImg as HomeTeamImg," +
           "b.VisitingTeam as VisitingTeam,b.VisitingTeamImg as VisitingTeamImg,b.StartTime as StartTime,  " +
           "b.RightScore as RightScore FROM JC_Score a LEFT JOIN JC_Quiz b ON (a.QuizID=b.ID) WHERE a.State=0 ");
           if (!string.IsNullOrEmpty(where) && where.Trim() != null && where.Trim() != "")
           {
               strSql.Append(" AND " + where);
           }
           strSql.Append(" order by StartTime asc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion

       #region 获取竞猜结果详细
       /// <summary>
       /// 获取竞猜结果详细
       /// </summary>
       /// <param name="jcid"></param>
       /// <returns></returns>
       public DataSet GetJCScoreDetail(string jcid)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM JC_Score ");
           strSql.Append(" WHERE [ID] = @ID ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", jcid)
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion

       #region 判断是否有重复
       /// <summary>
       /// 判断是否有重复
       /// </summary>
       /// <param name="jcsid"></param>
       /// <param name="openid"></param>
       /// <returns></returns>
       public bool ExistJCScore(string jcquizid, string openid)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT count(ID) ");
           strSql.Append(" FROM JC_Score ");
           strSql.Append(" WHERE [QuizID] = @QuizID ");
           strSql.Append(" AND [OpenID] = @OpenID ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@QuizID", jcquizid),
                    new System.Data.SqlClient.SqlParameter("@OpenID", openid)
                };
           return DbHelperSQL.Exists(strSql.ToString(), paras.ToArray());
       }
       #endregion

       #region 猜对比分排名
       /// <summary>
       /// 猜对比分排名
       /// </summary>
       /// <returns></returns>
       public DataSet GetRightGuessTop(string QuizID)
       {
           //string safesql = "SELECT COUNT(a.SiteCode) as CountTop,a.OpenID as OpenID,a.SiteCode as SiteCode"+
           //    ",(select c.Name from SYS_Customers c where c.OpenID=a.OpenID) as UserName FROM JC_Score a  LEFT JOIN " +
           //    " JC_Quiz b ON (a.GuessScore=b.RightScore) WHERE a.QuizID =@QuizID group by OpenID,a.SiteCode order by CountTop desc";
           string safesql = "SELECT COUNT(a.SiteCode) as CountTop,a.OpenID as OpenID,a.SiteCode as SiteCode" +
               " FROM JC_Score a  LEFT JOIN " +
               " JC_Quiz b ON (a.GuessScore=b.RightScore) WHERE a.QuizID =@QuizID group by OpenID,a.SiteCode order by CountTop desc";
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@QuizID", QuizID)
                };
           return DbHelperSQL.Query(safesql.ToString(), paras.ToArray());
       }
       #endregion

       #region 获取竞猜输赢列表
       /// <summary>
       /// 获取竞猜输赢列表
       /// </summary>
       /// <param name="QuizID"></param>
       /// <param name="LoseWin"></param>
       /// <returns></returns>
       public DataSet GetLoseWin(string QuizID,string LoseWin)
       {
           StringBuilder strSql = new StringBuilder();
           //strSql.Append(" SELECT COUNT(a.SiteCode) as CountTop,a.SiteCode as SiteCode,a.OpenID as OpenID,"+
           //    "(select c.Name from SYS_Customers c where c.OpenID=a.OpenID) as UserName ");
           //strSql.Append(" FROM JC_Score a ");
           //strSql.Append(" WHERE [QuizID] = @QuizID AND [dbo].[split](GuessScore,':',0) "+LoseWin+" [dbo].[split](GuessScore,':',1)");
           //strSql.Append(" group by SiteCode,OpenID order by CountTop desc ");
           strSql.Append(" SELECT COUNT(a.SiteCode) as CountTop,a.SiteCode as SiteCode,a.OpenID as OpenID ");
           strSql.Append(" FROM JC_Score a ");
           strSql.Append(" WHERE [QuizID] = @QuizID AND [dbo].[split](GuessScore,':',0) " + LoseWin + " [dbo].[split](GuessScore,':',1)");
           strSql.Append(" group by SiteCode,OpenID order by CountTop desc ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@QuizID", QuizID)
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion

       #region 猜错比分排名
       /// <summary>
       /// 猜错比分排名
       /// </summary>
       /// <returns></returns>
       public DataSet GetWrongGuessTop()
       {
           string safesql = "SELECT COUNT(a.ID) as CountTop,a.OpenID as OpenID FROM JC_Score a  LEFT JOIN " +
                        " JC_Quiz b ON (a.QuizID=b.ID) WHERE a.GuessScore <> b.RightScore group by OpenID order by CountTop desc";
           return DbHelperSQL.Query(safesql);
       }
       #endregion
    }
}
