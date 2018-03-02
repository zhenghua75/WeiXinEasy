using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Maticsoft.DBUtility;
using Model.Vote;

namespace DAL.Vote
{
   public class OptionDAL
    {
       public OptionDAL(){;}
       #region 获取投票选项列表
       /// <summary>
       /// 获取投票选项列表
       /// </summary>
       /// <param name="subid"></param>
       /// <param name="where"></param>
       /// <returns></returns>
       public DataSet GetOptionList(string subid,string like)
       {
           string safesql = "SELECT a.*,b.Subject AS VoteName FROM vote_options a  LEFT JOIN VOTE_Subject b ON a.SubjectID=b.ID where a.SubjectID=" + subid + "  ";
           if (like.Trim() != null && like.Trim() != "")
           {
               safesql += " AND a.Title like '" + like+"'";
           }
           safesql += " a.IsDel=1 order by [Order] asc ";
           DataSet ds = DbHelperSQL.Query(safesql);
           return ds;
       }
       #endregion

       #region 获取有效的选项列表
       /// <summary>
       /// 获取有效的选项列表
       /// </summary>
       /// <param name="where"></param>
       /// <returns></returns>
       public DataSet GetOptionDataList(string where)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT a.*,b.Subject AS VoteName,"+
               "(select COUNT(id)from Vote_Users c where c.VoteID=a.ID) as VoteCount "+
               " FROM vote_options a  LEFT JOIN VOTE_Subject b ON a.SubjectID=b.ID where isdel=1 ");
           if (where != null && where.Trim() != "")
           {
               strSql.Append(where);
           }
           strSql.Append(" order by VoteCount desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion

       #region 添加投票选项
       /// <summary>
       /// 添加投票选项
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddVoteOption(VOTE_Option model)
       {
           string sql = @"INSERT INTO [vote_options]
                        ([ID],[SubjectID],[Title],[ico],[contentdesc],[Order],[Amount])
                 VALUES
                        (@ID,@SubjectID,@Title,@ico,@contentdesc,@Order,@Amount)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@SubjectID", model.SubjectID),
                new System.Data.SqlClient.SqlParameter("@Title", model.Title),
                new System.Data.SqlClient.SqlParameter("@ico", model.Ico),
                new System.Data.SqlClient.SqlParameter("@contentdesc", model.Contentdesc),
                new System.Data.SqlClient.SqlParameter("@Order", model.Order),
                new System.Data.SqlClient.SqlParameter("@Amount", model.Amount)
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

       #region 获取同类最大Order
       /// <summary>
       /// 获取同类最大Order
       /// </summary>
       /// <param name="subid"></param>
       /// <returns></returns>
       public int GetOptionMaxOrder(string subid)
       {
           string safesql = "select max([order]) from vote_options where SubjectID='"+subid+"'";
           int max = 0;
           try
           {
               max = Convert.ToInt32(DbHelperSQL.GetSingle(safesql));
           }
           catch (Exception)
           {
               max = 0;
           }
           return max;
       }
       #endregion

       #region 更新投票选项信息
       /// <summary>
       /// 更新投票选项信息
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateOption(VOTE_Option model)
       {
           string safesql = "";
           safesql = "update vote_options set ";
           if (model.SubjectID != null && model.SubjectID != "")
           {
               safesql += "[SubjectID]='" + model.SubjectID + "',";
           }
           if (model.Title != null&&model.Title !="")
           {
               safesql += "[Title]='" + model.Title + "',";
           }
           if (model.Ico != null && model.Ico != "")
           {
               safesql += "[Ico]='" + model.Ico + "',";
           }
           if (model.Contentdesc != null && model.Contentdesc != "")
           {
               safesql += "[Contentdesc]='" + model.Contentdesc + "',";
           }
           if (model.Order != 0)
           {
               safesql += "[Order]=" + model.Order + ",";
           }
           else
           {
               safesql += "[Order]=" + Convert.ToInt32(GetOptionValue("[Order]", model.ID)) + ",";
           }
           safesql += "[IsDel]=" + model.IsDel;
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

       #region 根据ID判断该选项是否存在，存在则更新，否则插入
       /// <summary>
       /// 根据ID判断该选项是否存在，存在则更新，否则插入
       /// </summary>
       /// <param name="opid"></param>
       /// <returns></returns>
       public bool IsexistOption(string opid)
       {
           string safesql = "select count(ID) from VOTE_Options where ID='" + opid + "'";
           int rowsAffected = 0;
           try
           {
               rowsAffected = Convert.ToInt32(DbHelperSQL.GetSingle(safesql.ToString()));
           }
           catch (Exception)
           {
               rowsAffected = 0;
           }
           
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
       #region 判断选项是否重复
       /// <summary>
       /// 判断选项是否重复
       /// </summary>
       /// <param name="strTile"></param>
       /// <returns></returns>
       public bool ExistOptionTile(string strTile)
       {
           string safesql = "select count(ID) from VOTE_Options where Title='" + strTile + "' and isdel=1 ";
           int rowsAffected = 0;
           try
           {
               rowsAffected = Convert.ToInt32(DbHelperSQL.GetSingle(safesql.ToString()));
           }
           catch (Exception)
           {
               rowsAffected = 0;
           }

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
       #region 根据标识获取相关属性
       /// <summary>
       /// 根据标识获取相关属性
       /// </summary>
       /// <param name="value"></param>
       /// <param name="opid"></param>
       /// <returns></returns>
       public object GetOptionValue(string value, string opid)
       {
           if (value.Trim() != null && value.Trim() != "")
           {
               string safesql = "select " + value + " from VOTE_Options where ID='" + opid + "'";
               return DbHelperSQL.GetSingle(safesql);
           }
           else
           {
               return null;
           }
       }
       #endregion

       #region 获取选项详细
       /// <summary>
       /// 获取选项详细
       /// </summary>
       /// <param name="opID"></param>
       /// <returns></returns>
       public DataSet getOptionDetail(string opID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM vote_options ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", opID)
            };
           DataSet ds = DbHelperSQL.Query(strSql.ToString(), paras);
           return ds;
       }
       #endregion

       #region 删除选项
       /// <summary>
       /// 删除选项
       /// </summary>
       /// <param name="opid"></param>
       /// <returns></returns>
       public bool DelOption(string opid)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetOptionValue("IsDel", opid));
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
           strSql.Append(" UPDATE vote_options ");
           strSql.Append(" SET IsDel = @IsDel ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", opid),
                new System.Data.SqlClient.SqlParameter("@IsDel", state)
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

       #region 取投票照片
       /// <summary>
       /// 获取选项详细
       /// </summary>
       /// <param name="strSiteCode"></param>
       /// <returns></returns>
       public DataSet getVotePhoto(string strSiteCode)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT b.Title,b.Ico,b.ID ");
           strSql.Append(" FROM VOTE_Subject a ");
           strSql.Append(" LEFT JOIN VOTE_Options b ON (b.SubjectID = a.ID) ");
           strSql.Append(" WHERE a.SiteCode = @SiteCode ");
           strSql.Append(" Order BY b.SubjectID,b.[Order] ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode)
            };
           DataSet ds = DbHelperSQL.Query(strSql.ToString(), paras);
           return ds;
       }

       #endregion
    }
}
