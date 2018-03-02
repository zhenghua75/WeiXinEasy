using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Maticsoft.DBUtility;
using Model.Vote;

namespace DAL.Vote
{
    public class SubjectDAL
    {
        public SubjectDAL() { ; }

        #region 返回站点投票信息
        /// <summary>
        /// 返回站点投票信息
        /// </summary>
        /// <param name="strSiteCode">站点代码</param>
        /// 
        public DataSet GetSubjectData(string strSiteCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT TOP 1 * ");
            strSql.Append(" FROM VOTE_Subject ");
            //strSql.Append(" WHERE SiteCode = '" + strSiteCode + "' ");
            strSql.Append(" WHERE SiteCode = @SiteCode ");
            strSql.Append(" AND IsValid = 1 ");
            strSql.Append(" ORDER BY CreateTime DESC ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode)
            };
            DataSet ds = DbHelperSQL.Query(strSql.ToString(),paras);
            return ds;
        }
        #endregion

        #region 获取有效的活动列表
        /// <summary>
        /// 获取有效的活动列表
        /// </summary>
        /// <param name="strSiteCode"></param>
        /// <returns></returns>
        public DataSet GetSubjectDataList(string strSiteCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT  * ");
            strSql.Append(" FROM VOTE_Subject ");
            //strSql.Append(" WHERE SiteCode = '" + strSiteCode + "' ");
            strSql.Append(" WHERE SiteCode = @SiteCode ");
            strSql.Append(" AND IsValid = 1 ");
            strSql.Append(" ORDER BY CreateTime DESC ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode)
            };
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), paras);
            return ds;
        }
        #endregion

        #region 获取投票列表
        /// <summary>
        /// 获取投票列表
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public DataSet GetVoteDataList(string where)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT  * ");
            strSql.Append(" FROM VOTE_Subject ");
            strSql.Append(" WHERE  IsValid = 1 ");
            if (where.Trim() != null && where.Trim() != "")
            {
                strSql.Append(" AND "+where);
            }
            strSql.Append(" ORDER BY CreateTime DESC ");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion

        #region 返回站点投票信息选项
        /// <summary>
        /// 返回站点投票信息选项
        /// </summary>
        /// <param name="strSubjectID">主题ID</param>
        /// 
        public DataSet GetOptionsData(string strSubjectID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM VOTE_Options ");
            //strSql.Append(" WHERE SubjectID = '" + strSubjectID +"' ");
            strSql.Append(" WHERE SubjectID = @SubjectID AND ISDEL=1 ");
            strSql.Append(" ORDER BY [Order] ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@SubjectID", strSubjectID)
            };
            DataSet ds = DbHelperSQL.Query(strSql.ToString(),paras);
            return ds;
        }
        #endregion

        #region 返回投票详细信息
        /// <summary>
        /// 返回投票详细信息
        /// </summary>
        /// <param name="voteid"></param>
        /// <returns></returns>
        public DataSet GetVoteDetail(string voteid,string strSiteCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM VOTE_Subject ");
            strSql.Append(" WHERE ID = @ID and SiteCode=@SiteCode ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", voteid),
                new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode)
            };
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), paras);
            return ds;
        }
        #endregion

        #region 更新投票信息
        /// <summary>
        /// 更新投票信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateVoteInfo(VOTE_Subject model)
        {
            string safesql = "";
            safesql = "update VOTE_Subject set ";
            if (model.Subject != null)
            {
                safesql += "Subject='" + model.Subject + "',";
            }
            if (model.SiteCode != null)
            {
                safesql += "SiteCode='" + model.SiteCode + "',";
            }
            if (model.Content != null)
            {
                safesql += "Content='" + model.Content + "',";
            }
            if (model.BeginTime != null)
            {
                safesql += "BeginTime='" + model.BeginTime.ToString("yyyy-MM-dd") + "',";
            }
            if (model.EndTime != null)
            {
                safesql += "EndTime='" + model.EndTime.ToString("yyyy-MM-dd") + "',";
            }
            safesql += "CreateTime='" + DateTime.Now + "' where id='" + model.ID + "'";
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

        #region 更新投票状态
        /// <summary>
        /// 更新投票状态
        /// </summary>
        /// <param name="id">唯一标识</param>
        /// <returns></returns>
        public bool UpdateVoteIsValid(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" update VOTE_Subject set IsValid=0 ");
            strSql.Append(" WHERE ID = @ID ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID",id)
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

        #region 添加投票
        /// <summary>
        /// 添加投票
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddVoteinfo(VOTE_Subject model)
        {
            string sql = @"INSERT INTO [VOTE_Subject]
                        ([ID]
                        ,[Subject]
                        ,[Content]
                        ,[Type]
                        ,[SiteCode]
                        ,[BeginTime]
                        ,[EndTime]
                        ,[CreateTime]
                        ,[IsValid])
                 VALUES
                        (@ID
                       ,@Subject
                       ,@Content
                       ,@Type
                       ,@SiteCode
                       ,@BeginTime
                       ,@EndTime
                       ,@CreateTime
                       ,@IsValid)";
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@Subject", model.Subject),
                new System.Data.SqlClient.SqlParameter("@Content", model.Content),
                new System.Data.SqlClient.SqlParameter("@Type", model.Type),
                new System.Data.SqlClient.SqlParameter("@SiteCode", model.SiteCode),
                new System.Data.SqlClient.SqlParameter("@BeginTime", model.BeginTime),
                new System.Data.SqlClient.SqlParameter("@EndTime", model.EndTime),
                new System.Data.SqlClient.SqlParameter("@CreateTime", DateTime.Now),
                new System.Data.SqlClient.SqlParameter("@IsValid", model.IsValid)
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
    }
}
