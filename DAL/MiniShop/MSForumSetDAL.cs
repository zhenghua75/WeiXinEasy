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
    /// 论坛设置-功能类
    /// </summary>
   public class MSForumSetDAL
    {
       /// <summary>
        /// 论坛设置-功能类
       /// </summary>
       public MSForumSetDAL (){ ;}
       #region 添加设置
        /// <summary>
       /// 添加设置
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddMSForumSet(MSForumSet model)
       {
           string sql = @"INSERT INTO [MS_ForumSet]
                        ([ID],[FTitle],[Visit],[LogoImg],[BackImg],[Fstate],[AddTime])
                 VALUES
                        (@ID,@FTitle,@Visit,@LogoImg,@BackImg,@Fstate,@AddTime)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@FTitle", model.FTitle),
                new System.Data.SqlClient.SqlParameter("@Visit", (model.Visit==0?0:model.Visit)),
                new System.Data.SqlClient.SqlParameter("@LogoImg", model.LogoImg),
                new System.Data.SqlClient.SqlParameter("@BackImg", model.BackImg),
                new System.Data.SqlClient.SqlParameter("@Fstate",(model.Fstate==1?1:0)),
                new System.Data.SqlClient.SqlParameter("@AddTime", DateTime.Now)
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
       #region 更新论坛设置
       /// <summary>
       /// 更新论坛设置
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateMSForumSet(MSForumSet model)
       {
           string safeslq = "";
           safeslq = "UPDATE MS_ForumSet SET ";
           if (model.FTitle != null && model.FTitle != "")
           {
               safeslq += "FTitle='" + model.FTitle + "',";
           }
           if (model.LogoImg != null && model.LogoImg.ToString() != "")
           {
               safeslq += "LogoImg='" + model.LogoImg + "',";
           }
           if (model.BackImg != null && model.BackImg.ToString() != "")
           {
               safeslq += "BackImg='" + model.BackImg + "',";
           }
           safeslq += " Fstate=" + (model.Fstate == 1 ? 1 : 0) + " ";
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
       #region 获取属性
       /// <summary>
       /// 获取属性
       /// </summary>
       /// <param name="strValue"></param>
       /// <param name="strID"></param>
       /// <returns></returns>
       public object GetMSForumSetValueByID(string strValue, string strID)
       {
           string safesql = "";
           safesql = "select " + strValue + " from MS_ForumSet where ID='" + strID + "'";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion
       #region 更新论坛状态
       /// <summary>
       /// 更新论坛状态
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public bool UpdateMSForumSetState(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetMSForumSetValueByID("Fstate", strID));
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
           strSql.Append(" UPDATE MS_ForumSet ");
           strSql.Append(" SET Fstate = @Fstate ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strID),
                new System.Data.SqlClient.SqlParameter("@Fstate", state)
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
       #region 更新论坛访问
       /// <summary>
       /// 更新论坛访问
       /// </summary>
       /// <param name="visit">访问量</param>
       /// <param name="strFID">论坛编号</param>
       /// <returns></returns>
       public bool UpdateForumVist(int visit,string strFID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" UPDATE MS_ForumSet ");
           strSql.Append(" SET Visit = @Visit ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strFID),
                new System.Data.SqlClient.SqlParameter("@Visit", visit)
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
       #region 获取有效的设置列表
       /// <summary>
       /// 获取有效的设置列表
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetMSForumSetList(string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select a.*,(select COUNT(id)from MS_ForumTopic where FID=a.ID )topiccount ");
           strSql.Append(" from MS_ForumSet a ");
           strSql.Append(" where a.Fstate=0  ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by a.AddTime desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 获取论坛设置详细
       /// <summary>
       /// 获取论坛设置详细
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public DataSet GetMSForumSetDetail(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM MS_ForumSet ");
           strSql.Append(" WHERE [ID] = @ID");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", strID)
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion
       #region 判断用户是否存在
       /// <summary>
       /// 判断用户是否存在
       /// </summary>
       /// <param name="strFtitle">标题</param>
       /// <returns></returns>
       public bool ExistMSForumSet(string strFtitle)
       {
           string strSql = string.Empty;
           strSql += " SELECT count(ID) FROM MS_ForumSet  ";
           strSql += " where Fstate=0 ";
           if (strFtitle.Trim() != null && strFtitle.Trim() != "")
           {
               strSql += " and [Ftitle] ='" + strFtitle + "' ";
           }
           return DbHelperSQL.Exists(strSql.ToString());
       }
       #endregion
    }
}
