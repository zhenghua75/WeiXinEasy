using Maticsoft.DBUtility;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.CMS
{
    public class ArticleDAL
    {
        #region 返回所有文章信息列表
        /// <summary>
        /// 返回所有文章信息列表
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public DataSet GetAllArticle(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT a.*,b.Name AS CategoryName ");
            strSql.Append(" FROM Cms_Article a ");
            strSql.Append(" LEFT JOIN Cms_Category b ON (b.ID = a.Category) ");
            strSql.Append(" WHERE a.isDel = 0 ");
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql.Append(" AND " + strWhere);
            }
            strSql.Append(" ORDER BY [Order] ASC");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion

        #region 返回某站点文章信息列表
        /// <summary>
        /// 返回某站点文章信息列表
        /// </summary>
        /// <param name="strSiteCode">站点代码</param>
        /// <param name="strWhere">其它条件</param>
        /// <returns></returns>
        public DataSet GetArticleBySite(string strSiteCode,string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT [Title],[ClickNum],[IsTop],[IsDel],[CreateTime] ");
            strSql.Append(" FROM Cms_Article ");
            strSql.Append(" WHERE SiteCode = @strSiteCode ");
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql.Append(" AND " + strWhere);
            }
            strSql.Append(" ORDER BY [Order] ASC");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@strSiteCode", strSiteCode),
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion

        #region 返回文章信息列表(包括评论)
        /// <summary>
        /// 返回文章信息列表(包括评论)
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetArticleList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT a.[ID],a.[Title],a.[Content],a.[Author],a.[ClickNum],a.[CreateTime],a.[CreateUser],a.[SiteCode],b.CUID,b.CContent,b.CTime ");
            strSql.Append("FROM [dbo].[Cms_Article] a,dbo.Cms_Comment b ");
            strSql.Append("WHERE a.ID = b.CAID ");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion

        #region 添加文章
        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddArticleData(Model.CMS.CMS_Article model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            StringBuilder strSql2 = new StringBuilder();
            strSql1.Append("ID,");
            strSql2.Append(String.Format("'{0}',", model.ID));
            if (model.Title != null)
            {
                strSql1.Append("Title,");
                strSql2.Append(String.Format("'{0}',", model.Title));
            }
            if (model.Pic != null)
            {
                strSql1.Append("Pic,");
                strSql2.Append(String.Format("'{0}',", model.Pic));
            }
            if (model.Summary != null)
            {
                strSql1.Append("Summary,");
                strSql2.Append(String.Format("'{0}',", model.Summary));
            }
            if (model.Content != null)
            {
                strSql1.Append("Content,");
                strSql2.Append(String.Format("'{0}',", model.Content));
            }
            if (model.Author != null)
            {
                strSql1.Append("Author,");
                strSql2.Append(String.Format("'{0}',", model.Author));
            }
            if (model.Category != null)
            {
                strSql1.Append("Category,");
                strSql2.Append(String.Format("'{0}',", model.Category));
            }
            strSql1.Append("ClickNum,");
            strSql2.Append(model.ClickNum + ",");
            strSql1.Append("IsTop,");
            strSql2.Append((model.IsTop ? 1 : 0) + ",");
            strSql1.Append("IsDel,");
            strSql2.Append((model.IsDel ? 1 : 0) + ",");
            if (model.CreateTime != null)
            {
                strSql1.Append("CreateTime,");
                strSql2.Append(String.Format("'{0:yyyy-MM-dd HH:mm:ss}',", model.CreateTime));
            }
            if (model.CreateUser != null)
            {
                strSql1.Append("CreateUser,");
                strSql2.Append(String.Format("'{0}',", model.CreateUser));
            }
            if (model.SiteCode != null)
            {
                strSql1.Append("SiteCode,");
                strSql2.Append(String.Format("'{0}',", model.SiteCode));
            }
            
            strSql.Append("INSERT INTO Cms_Article(");
            strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
            strSql.Append(")");
            strSql.Append(" VALUES (");
            strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
            strSql.Append(");");
            strSql.Append(String.Format(" UPDATE SYS_Account SET ID = '{0}'", model.ID));
            strSql.Append(String.Format(" WHERE ID ='{0}' ", model.ID));
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
        #endregion

        #region 返回文章详细信息
        /// <summary>
        /// 返回文章详细信息
        /// </summary>
        /// <param name="strArticleID">文章ID</param>
        /// 
        public DataSet GetArticleDetail(string strArticleID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM Cms_Article ");
            strSql.Append(" WHERE [ID] = @ID ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", strArticleID),
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion

        #region 返回同类别文章列表
        /// <summary>
        /// 返回商品列表
        /// </summary>
        /// <param name="strSiteCode">站点代码</param>
        /// <param name="strCatID">商品类别ID</param>
        /// 
        public DataSet GetCategoryList(string strSiteCode, string strCatID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM Cms_Article ");
            strSql.Append(" WHERE SiteCode = @SiteCode ");
            strSql.Append(" AND [Category] = @Category ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode),
                    new System.Data.SqlClient.SqlParameter("@Category", strCatID),
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion

        #region 更新文章信息
        /// <summary>
        /// 更新文章信息
        /// </summary>
        /// <param name="model">文章信息</param>
        /// <returns></returns>
        public bool UpdateArticleData(Model.CMS.CMS_Article model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Cms_Article SET ");
            if (!string.IsNullOrEmpty(model.Title))
            {
                strSql.Append("Title='" + model.Title + "',");
            }
            if (!string.IsNullOrEmpty(model.Pic))
            {
                strSql.Append("Pic='" + model.Pic + "',");
            }
            if (!string.IsNullOrEmpty(model.Summary))
            {
                strSql.Append("Summary='" + model.Summary + "',");
            }
            if (!string.IsNullOrEmpty(model.Content))
            {
                strSql.Append("Content='" + model.Content + "',");
            }
            if (!string.IsNullOrEmpty(model.Author))
            {
                strSql.Append("Author='" + model.Author + "',");
            }
            if (!string.IsNullOrEmpty(model.Category))
            {
                strSql.Append("Category='" + model.Category + "',");
            }
            if (model.ClickNum > 0)
            {
                strSql.Append("ClickNum=" + model.ClickNum + ",");
            }
            if (!string.IsNullOrEmpty(model.IsTop.ToString()))
            {
                strSql.Append("IsTop=" + (model.IsTop ? 1 : 0) + ",");
            }
            if (!string.IsNullOrEmpty(model.IsDel.ToString()))
            {
                strSql.Append("IsDel=" + (model.IsDel ? 1 : 0) + ",");
            }
            if (!string.IsNullOrEmpty(model.CreateUser))
            {
                strSql.Append("CreateUser='" + model.CreateUser + "',");
            }
            if (!string.IsNullOrEmpty(model.SiteCode))
            {
                strSql.Append("SiteCode='" + model.SiteCode + "',");
            }
            if (model.Order > 0)
            {
                strSql.Append("[Order]=" + model.Order + ",");
            }
            int n = strSql.ToString().LastIndexOf(",");
            strSql.Remove(n, 1);
            strSql.Append(" WHERE ID ='" + model.ID + "' ");
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
        #endregion

        #region 修改文章状态
        /// <summary>
        /// 修改文章状态
        /// </summary>
        /// <param name="strArticleID">文章信息</param>
        /// <returns></returns>
        public bool UpdateArticleState(string strArticleID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Cms_Article SET ");
            strSql.Append(" IsDel = 1 ");
            strSql.Append(" WHERE ID =@ID ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strArticleID)
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
    }
}
