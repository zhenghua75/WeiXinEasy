using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maticsoft.DBUtility;

namespace DAL.BBS
{
    public class BBSSectionDAL
    {
        public BBSSectionDAL() { ; }

        #region 返回账户与站点信息
        /// <summary>
        /// 返回账户与站点信息
        /// </summary>
        /// <param name="strSection">版块ID</param>
        /// 
        public DataSet GetAccountData(string strSection)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT b.ID,b.SiteCode,b.Name,c.Themes ");
            strSql.Append(" FROM BBS_Section a ");
            strSql.Append(" LEFT JOIN SYS_Account b ON (b.SiteCode = a.SiteCode) ");
            strSql.Append(" LEFT JOIN SYS_Account_Ext c ON (c.AccountID = b.ID) ");
            strSql.Append(" WHERE a.ID = @ID ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", strSection)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion

        #region 返回当前版块的所有主题
        /// <summary>
        /// 返回当前版块的所有主题
        /// </summary>
        /// <param name="strSection">版块ID</param>
        /// 
        public DataSet GetTopicList(string strSection)
        {
            StringBuilder strSql = new StringBuilder();
            //strSql.Append(" SELECT b.Name AS SName,a.ID,a.Topic,a.ClickCount,a.IsTop,a.IsElite,a.LastDate,a.ReplyCount ");
            strSql.Append(" SELECT TOP 10 b.ID AS SID,b.Name AS SName,a.* ");
            strSql.Append(" FROM BBS_Topic a ");
            strSql.Append(" LEFT JOIN BBS_Section b ON (b.ID = a.SID)");
            strSql.Append(" WHERE a.SID = @SID ");
            strSql.Append(" AND a.Flag = 1 ");
            strSql.Append(" ORDER BY NEWID(), a.LastDate DESC ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SID", strSection)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion

        #region 返回当前主题
        /// <summary>
        /// 返回当前主题
        /// </summary>
        /// <param name="strTopic">主题ID</param>
        /// 
        public DataSet GetTopicInfo(string strTopic)
        {
            StringBuilder strSql = new StringBuilder();
            //strSql.Append(" SELECT b.Name AS SName,a.ID,a.Topic,a.ClickCount,a.IsTop,a.IsElite,a.LastDate,a.ReplyCount ");
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM BBS_Topic ");
            strSql.Append(" WHERE ID = @ID ");
            strSql.Append(" AND Flag = 1 ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", strTopic)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion

        #region 返回主题所有回复
        /// <summary>
        /// 返回主题所有回复
        /// </summary>
        /// <param name="strTopic">主题ID</param>
        /// 
        public DataSet GetReplyList(string strTopic)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM BBS_Reply ");
            strSql.Append(" WHERE RID = @RID ");
            strSql.Append(" AND Flag = 1 ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@RID", strTopic)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion
    }
}
