using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.CMS
{
    /// <summary>
    /// 评论类的数据访问层
    /// </summary>
    public class CommentDAL
    {
        /// <summary>
        /// 返回某文章的所有评论
        /// </summary>
        /// <param name="ArticleID">文章ID</param>
        /// <returns></returns>
        public DataSet GetCommentsByArticleID(string ArticleID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT a.ID,b.Title,[UserName],a.[Content],a.[CreateTime],[ReplyCount] ");
            strSql.Append(" FROM [dbo].[Cms_Comment] a,dbo.Cms_Article b ");
            strSql.Append(String.Format(" WHERE a.ArticleID=b.ID and b.ID='{0}'", ArticleID));
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }

        /// <summary>
        /// 返回某文章的评论
        /// </summary>
        /// <param name="ArticleID">文章ID</param>
        /// <param name="strWhere">其他条件</param>
        /// <returns></returns>
        public DataSet GetCommentsByArticleID(string ArticleID, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT a.ID,b.Title,[UserName],a.[Content],a.[CreateTime],[ReplyCount] ");
            strSql.Append(" FROM [dbo].[Cms_Comment] a,dbo.Cms_Article b ");
            strSql.Append(String.Format(" WHERE a.ArticleID=b.ID and b.ID='{0}'", ArticleID));
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql.Append(" AND " + strWhere);
            }
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddCommentData(Model.CMS.CMS_Comment model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            StringBuilder strSql2 = new StringBuilder();
            //strSql1.Append("ID,");
            //strSql2.Append(String.Format("{0},", model.ID));
            if (model.ArticleID != null)
            {
                strSql1.Append("ArticleID,");
                strSql2.Append(String.Format("'{0}',", model.ArticleID));
            }
            if (model.UserName != null)
            {
                strSql1.Append("UserName,");
                strSql2.Append(String.Format("'{0}',", model.UserName));
            }
            if (model.Content != null)
            {
                strSql1.Append("Content,");
                strSql2.Append(String.Format("'{0}',", model.Content));
            }
            if (model.CreateTime != null)
            {
                strSql1.Append("CreateTime,");
                strSql2.Append(String.Format("'{0:yyyy-MM-dd HH:mm:ss}',", model.CreateTime));
            }
            strSql1.Append("ReplyCount,");
            strSql2.Append(model.ReplyCount + ",");

            strSql.Append("INSERT INTO Cms_Comment(");
            strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
            strSql.Append(")");
            strSql.Append(" VALUES (");
            strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
            strSql.Append(")");
            int rowsAffected = DbHelperSQL.ExecuteSql(strSql.ToString());
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
}
