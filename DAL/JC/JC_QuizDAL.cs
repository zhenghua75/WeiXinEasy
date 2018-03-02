using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.JC;
using Maticsoft.DBUtility;

namespace DAL.JC
{
   public class JC_QuizDAL
    {
       public JC_QuizDAL() { ;}

       #region 添加竞猜信息
       /// <summary>
       /// 添加竞猜信息
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddJCQuiz(JC_Quiz model)
       {
           string sql = @"INSERT INTO [JC_Quiz]
                        ([ID],[SiteCode],[QuizType],[StartTime],[Name],[HomeTeam],[HomeTeamImg],[VisitingTeam],[VisitingTeamImg],[MatchDesc],[RightScore],[AddTime],[State])
                 VALUES
                        (@ID,@SiteCode,@QuizType,@StartTime,@Name,@HomeTeam,@HomeTeamImg,@VisitingTeam,@VisitingTeamImg,@MatchDesc,@RightScore,@AddTime,@State)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@SiteCode", model.SiteCode),
                new System.Data.SqlClient.SqlParameter("@QuizType", model.QuizType),
                new System.Data.SqlClient.SqlParameter("@StartTime", model.StartTime),
                new System.Data.SqlClient.SqlParameter("@Name", model.Name),
                new System.Data.SqlClient.SqlParameter("@HomeTeam", model.HomeTeam),
                new System.Data.SqlClient.SqlParameter("@HomeTeamImg", model.HomeTeamImg),
                new System.Data.SqlClient.SqlParameter("@VisitingTeam", model.VisitingTeam),
                new System.Data.SqlClient.SqlParameter("@VisitingTeamImg", model.VisitingTeamImg),
                new System.Data.SqlClient.SqlParameter("@MatchDesc", model.MatchDesc),
                new System.Data.SqlClient.SqlParameter("@RightScore", model.RightScore),
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

       #region 竞猜信息修改
       /// <summary>
       /// 竞猜信息修改
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpDateJCQuiz(JC_Quiz model)
       {
           string safeslq = "";
           safeslq = "UPDATE JC_Quiz SET ";
           if (model.SiteCode != null && model.SiteCode != "")
           {
               safeslq += "SiteCode='" + model.SiteCode + "',";
           }
           if (model.QuizType != null && model.QuizType != "")
           {
               safeslq += "QuizType='" + model.QuizType + "',";
           }
           if (model.StartTime != null && model.StartTime.ToString() != "")
           {
               safeslq += "StartTime='" + model.StartTime + "',";
           }
           if (model.Name != null && model.Name != "")
           {
               safeslq += "Name='" + model.Name + "',";
           }
           if (model.HomeTeam != null && model.HomeTeam != "")
           {
               safeslq += "HomeTeam='" + model.HomeTeam + "',";
           }
           if (model.HomeTeamImg != null && model.HomeTeamImg != "")
           {
               safeslq += "HomeTeamImg='" + model.HomeTeamImg + "',";
           }
           if (model.VisitingTeam != null && model.VisitingTeam != "")
           {
               safeslq += "VisitingTeam='" + model.VisitingTeam + "',";
           }
           if (model.VisitingTeamImg != null && model.VisitingTeamImg != "")
           {
               safeslq += "VisitingTeamImg='" + model.VisitingTeamImg + "',";
           }
           if (model.MatchDesc != null && model.MatchDesc != "")
           {
               safeslq += "MatchDesc='" + model.MatchDesc + "',";
           }
           safeslq += "RightScore='" + model.RightScore + "',";
           if (model.State.ToString() != null && model.State.ToString() != "")
           {
               safeslq += "State=" + (model.State == 1 ? 1 : 0);
           }
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

       #region 修改竞猜信息状态
       /// <summary>
       /// 修改竞猜信息状态
       /// </summary>
       /// <param name="QuizID"></param>
       /// <param name="state"></param>
       /// <returns></returns>
       public bool UpdateJCState(string QuizID)
       {
           StringBuilder strSql = new StringBuilder();
           int state =0;
           try
           {
               state = Convert.ToInt32(GetQuizValueById("State", QuizID));
           }
           catch (Exception)
           {
              state =0;
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
           strSql.Append(" UPDATE JC_Quiz ");
           strSql.Append(" SET State = @State ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", QuizID),
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

       #region 根据标识获取相对应的值
       /// <summary>
       /// 根据标识获取相对应的值
       /// </summary>
       /// <param name="value"></param>
       /// <param name="QuizID"></param>
       /// <returns></returns>
       public object GetQuizValueById(string value,string QuizID)
       {
           string safesql = "";
           safesql = "select " + value + " from JC_Quiz where ID='" + QuizID + "'";
          return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion

       #region 根据条件返回所有竞猜列表信息
       /// <summary>
       /// 根据条件返回所有竞猜列表信息
       /// </summary>
       /// <param name="where"></param>
       /// <returns></returns>
       public DataSet GetJCQuizList(string where)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * from JC_Quiz ");
           if (!string.IsNullOrEmpty(where))
           {
               strSql.Append(" WHERE " + where);
           }
           strSql.Append(" order by StartTime asc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion

       #region 返回有效的竞猜列表
       /// <summary>
       /// 返回有效的竞猜列表
       /// </summary>
       /// <param name="where"></param>
       /// <returns></returns>
       public DataSet GetJCQuizDataList(string where)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * from JC_Quiz WHERE State=0 ");
           if (where!=null&&where.Trim()!="")
           {
               strSql.Append(" And " + where);
           }
           strSql.Append(" order by StartTime asc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion

       #region 根据当前时间获取接近的最小时间
       public DateTime GetStartDateTime()
       {
           string safesql = "select top 1 StartTime from JC_Quiz where StartTime>=GETDATE() order by StartTime asc";
           return Convert.ToDateTime(DbHelperSQL.GetSingle(safesql.ToString()));
       }
       #endregion

       #region 根据时间获取最大时间
       /// <summary>
       /// 根据时间获取最大时间
       /// </summary>
       /// <param name="smalltime"></param>
       /// <returns></returns>
       public DateTime GetBigStartTime(DateTime smalltime)
       {
           string safesql = string.Empty;
           safesql = "select top 1 StartTime from JC_Quiz where StartTime>=@StartTime and StartTime<@BigStartTime order by StartTime desc";
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@StartTime", smalltime.ToString("yyyy-MM-dd")),
                    new System.Data.SqlClient.SqlParameter("@BigStartTime", smalltime.AddDays(1).ToString("yyyy-MM-dd"))
                };
           return Convert.ToDateTime(DbHelperSQL.GetSingle(safesql.ToString(), paras.ToArray()));
       }
       #endregion

       #region 返回竞猜详细
       /// <summary>
       /// 返回竞猜详细
       /// </summary>
       /// <param name="QuizID"></param>
       /// <returns></returns>
       public DataSet GetJCQuizDetail(string QuizID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM JC_Quiz ");
           strSql.Append(" WHERE [ID] = @ID ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", QuizID),
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion

       #region 判断是否为竞猜
       /// <summary>
       /// 判断是否为竞猜
       /// </summary>
       /// <param name="quizID"></param>
       /// <param name="SiteCode"></param>
       /// <returns></returns>
       public bool ExistQuizID(string quizID, string SiteCode)
       {
           string sql = @"select COUNT(ID) from JC_Quiz where ID=@quizID and SiteCode=@SiteCode ";
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", SiteCode),
                    new System.Data.SqlClient.SqlParameter("@quizID", quizID)
                };
           return DbHelperSQL.Exists(sql, paras.ToArray());
       }
       #endregion
    }
}
