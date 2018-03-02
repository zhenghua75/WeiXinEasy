using Maticsoft.DBUtility;
using Model.ACT;
/* ==============================================================================
 * 类名称：SiteActivityDAL
 * 类描述：
 * 创建人：yhn,51809571@qq.com
 * 创建时间：2014/3/22 14:51:00
 * 修改人：
 * 修改时间：
 * 修改备注：
 * @version 1.0
 * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBUtility;

namespace DAL.ACT
{
    public class SiteActivityDAL
    {
        #region 获取站点活动
        /// <summary>
        /// 获取站点活动
        /// </summary>
        /// <param name="siteCode"></param>
        /// <param name="actType"></param>
        /// <returns></returns>
        public SiteActivity GetSiteAct(string siteCode,string actType)
        {
            SiteActivity res = null;
            if (!string.IsNullOrEmpty(siteCode))
            {
                string sql = @"SELECT * FROM [ACT_SiteActivity]
                        WHERE ActStatus=1 AND SiteCode=@SiteCode AND ActType=@ActType";
                IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", siteCode),
                    new System.Data.SqlClient.SqlParameter("@ActType", actType)
                };
                DataSet ds = DbHelperSQL.Query(sql, paras.ToArray());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    res = new SiteActivity()
                    {
                        ID = dr.GetColumnValue("ID", string.Empty),
                        SiteCode = dr.GetColumnValue("SiteCode", string.Empty),
                        ActTitle = dr.GetColumnValue("ActTitle", string.Empty),
                        ActContent = dr.GetColumnValue("ActContent", string.Empty),
                        ActType = dr.GetColumnValue("ActType", string.Empty),
                        ActStatus = dr.GetColumnValue("ActStatus", -1),
                        StartTime = dr.GetColumnValue("StartTime", string.Empty),
                        EndTime = dr.GetColumnValue("EndTime", string.Empty),
                        AddTime = dr.GetColumnValue("AddTime", DateTime.Now),
                        Remark = dr.GetColumnValue("Remark", string.Empty)
                    };
                }
            }
            return res;
        }
        #endregion

        #region 返回所有站点活动列表
        /// <summary>
        /// 返回所有站点活动列表
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public DataSet GetActivityList(string where)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * from ACT_SiteActivity ");
            if (!string.IsNullOrEmpty(where))
            {
                strSql.Append(" WHERE " + where);
            }
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion
        #region 获取有效活动列表
        /// <summary>
        /// 获取有效活动列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet GetActivityListByState(string where)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * from ACT_SiteActivity where actstatus=1 ");
            if (!string.IsNullOrEmpty(where))
            {
                strSql.Append("  " + where);
            }
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion

        #region 返回所有站点活动列表
        /// <summary>
        /// 返回所有站点活动列表
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public DataSet GetAllActivity(string where)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * from ACT_SiteActivity ");
            if (!string.IsNullOrEmpty(where))
            {
                strSql.Append(" WHERE " + where);
            }
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion

        #region 获取有效的活动
        /// <summary>
        /// 获取有效的活动
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet GeteffecitvActivity(string where)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * from ACT_SiteActivity WHERE ActStatus=1 ");
            if (!string.IsNullOrEmpty(where))
            {
                strSql.Append(" And " + where);
            }
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion

        #region 返回活动详细信息
        /// <summary>
        /// 返回活动详细信息
        /// </summary>
        /// <param name="strArticleID">活动ID</param>
        /// 
        public DataSet GetActivityDetail(string ActivityID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM ACT_SiteActivity ");
            strSql.Append(" WHERE [ID] = @ID ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", ActivityID),
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion
        #region 添加活动信息
        /// <summary>
        /// 添加活动信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int SaveActivity(SiteActivity model)
        {
            string sql = @"INSERT INTO [ACT_SiteActivity]
                        ([ID]
                       ,[SiteCode]
                       ,[ActTitle]
                       ,[Photo]
                       ,[ActContent]
                       ,[ActType]
                       ,[ActStatus]
                       ,[StartTime]
                       ,[EndTime]
                       ,[CutOffTime]
                       ,[Discount]
                       ,[OpenTime]
                       ,[CloseTime]
                       ,[DayLimit]
                       ,[Remark]
                       ,[AddTime])
                 VALUES
                        (@ID
                       ,@SiteCode
                       ,@ActTitle
                       ,@Photo
                       ,@ActContent
                       ,@ActType
                       ,@ActStatus
                       ,@StartTime
                       ,@EndTime
                       ,@CutOffTime
                       ,@Discount
                       ,@OpenTime
                       ,@CloseTime
                       ,@DayLimit
                       ,@Remark
                       ,@AddTime)";
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@SiteCode", model.SiteCode),
                new System.Data.SqlClient.SqlParameter("@ActTitle", model.ActTitle),
                new System.Data.SqlClient.SqlParameter("@Photo", model.Photo),
                new System.Data.SqlClient.SqlParameter("@ActContent", model.ActContent),
                new System.Data.SqlClient.SqlParameter("@ActType", model.ActType),
                new System.Data.SqlClient.SqlParameter("@ActStatus", model.ActStatus),
                new System.Data.SqlClient.SqlParameter("@StartTime", model.StartTime),
                new System.Data.SqlClient.SqlParameter("@EndTime", model.EndTime),
                new System.Data.SqlClient.SqlParameter("@CutOffTime", model.CutOffTime),
                new System.Data.SqlClient.SqlParameter("@Discount", model.DisCount),
                new System.Data.SqlClient.SqlParameter("@OpenTime", model.OpenTime),
                new System.Data.SqlClient.SqlParameter("@CloseTime", model.CloseTime),
                new System.Data.SqlClient.SqlParameter("@DayLimit", model.DayLimit),
                new System.Data.SqlClient.SqlParameter("@Remark", model.Remark),
                new System.Data.SqlClient.SqlParameter("@AddTime", DateTime.Now)
            };
           return  DbHelperSQL.ExecuteSql(sql, paras);
        }
        #endregion
        #region 更新活动信息
        /// <summary>
        /// 更新活动信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateActivity(SiteActivity model)
        {
            string safesql = "";
            safesql = "update ACT_SiteActivity set ";
            if (model.ActTitle != null&&model.ActTitle !="")
            {
                safesql += "acttitle='"+model.ActTitle+"',";
            }
            if (model.Photo != null && model.Photo != "")
            {
                safesql += "Photo='" + model.Photo + "',";
            }
            if (model.SiteCode != null&&model.SiteCode !="")
            {
                safesql += "SiteCode='" + model.SiteCode + "',";
            }
            if (model.ActContent != null&&model.ActContent != "")
            {
                safesql += "ActContent='" + model.ActContent + "',";
            }
            if (model.StartTime != null)
            {
                safesql += "StartTime='" + model.StartTime + "',";
            }
            if (model.EndTime != null)
            {
                safesql += "EndTime='" + model.EndTime + "',";
            }
            if (model.CutOffTime != null)
            {
                safesql += "CutOffTime='" + model.CutOffTime + "',";
            }
            if (model.DisCount != null && model.DisCount != "")
            {
                safesql += "Discount='" + model.DisCount + "',";
            }
            if (model.OpenTime != null && model.OpenTime != "")
            {
                safesql += "OpenTime='" + model.OpenTime + "',";
            }
            if (model.CloseTime != null && model.CloseTime != "")
            {
                safesql += "CloseTime='" + model.CloseTime + "',";
            }
            if (model.DayLimit.ToString() != null && model.DayLimit.ToString() != "")
            {
                safesql += "DayLimit=" + model.DayLimit + ",";
            }
            if (model.Remark != null&&model.Remark !="")
            {
                safesql += "Remark='" + model.Remark + "',";
            }
            safesql += "AddTime='" + DateTime.Now + "' where id='" + model.ID+"'";
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

        #region      修改活动状态
        /// <summary>
        /// 修改活动状态
        /// </summary>
        /// <param name="strID">活动ID</param>
        /// <param name="strState">修改的状态</param>
        /// <returns></returns>
        public bool UpdateActivityActStatus(string strID,string strState)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" UPDATE ACT_SiteActivity ");
            strSql.Append(" SET ActStatus = @State ");
            strSql.Append(" WHERE ID = @ID ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strID),
                new System.Data.SqlClient.SqlParameter("@State", strState)
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

        #region 获取活动属性
        /// <summary>
        /// 获取活动属性
        /// </summary>
        /// <param name="value"></param>
        /// <param name="SiteActivityId"></param>
        /// <returns></returns>
        public object GetSiteActivityByValue(string value, string SiteActivityId)
        {
            object obj = null;
            if (value.Trim() != null && value.Trim() != "")
            {
                string safesql = " select " + value + " from ACT_SiteActivity where id='" + SiteActivityId + "' ";
                obj = DbHelperSQL.GetSingle(safesql);
            }
            return obj;
        }
        #endregion
    }
}
