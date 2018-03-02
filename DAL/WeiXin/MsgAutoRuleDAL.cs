using Maticsoft.DBUtility;
using Model.WeiXin;
/* ==============================================================================
 * 类名称：MsgAutoRuleDAL
 * 类描述：
 * 创建人：yhn,51809571@qq.com
 * 创建时间：2014/3/15 14:34:27
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
using System.Text.RegularExpressions;

namespace DAL.WeiXin
{
    /// <summary>
    /// default：默认自动回复,当其它回复规则无效时触发
    /// subscribe：订阅自动回复，适用于订阅事件
    /// keywords：关键字自动回复，适用于文本消息，MatchPattern配置相应的关键字
    /// clickevent：自定义菜单单击事件自动回复，MatchPattern配置相应的EventKey
    /// </summary>
    public class MsgAutoRuleDAL
    {
        public MsgAutoRuleDAL() { ;}
        /// <summary>
        /// 获取关键字回复规则
        /// </summary>
        /// <param name="wxConfigID"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public MsgAutoRule GetKeywordsRule(string wxConfigID,string keywords)
        {
            MsgAutoRule res = null;
            if (!string.IsNullOrEmpty(wxConfigID) && !string.IsNullOrEmpty(keywords))
            {
                string sql = @"SELECT * FROM [WX_MsgAutoRule]
                        WHERE Enabled=1 AND MatchType='keywords' AND WXConfigID=@WXConfigID AND MatchPattern=@MatchPattern 
                        ORDER BY [Order] ASC";
                IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@WXConfigID", wxConfigID),
                    new System.Data.SqlClient.SqlParameter("@MatchPattern", keywords)
                };
                DataSet ds = DbHelperSQL.Query(sql, paras.ToArray());
                res = ds.ConvertToFirstObj<MsgAutoRule>();
                //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //{
                //    DataRow dr = ds.Tables[0].Rows[0];
                //    res = new MsgAutoRule()
                //    {
                //        ID = dr.GetColumnValue("ID", string.Empty),
                //        WXConfigID = dr.GetColumnValue("WXConfigID", string.Empty),
                //        MatchType = dr.GetColumnValue("MatchType", string.Empty),
                //        MatchPattern = dr.GetColumnValue("MatchPattern", string.Empty),
                //        MsgType = dr.GetColumnValue("MsgType", string.Empty),
                //        MsgValue = dr.GetColumnValue("MsgValue", string.Empty),
                //        Handle = dr.GetColumnValue("Handle", string.Empty),
                //        AddTime = dr.GetColumnValue("AddTime", DateTime.Now),
                //        AddUser = dr.GetColumnValue("AddUser", string.Empty),
                //        LastModTime = dr.GetColumnValue("LastModTime", DateTime.Now),
                //        LastModUser = dr.GetColumnValue("LastModUser", string.Empty)
                //    };
                //}
            }
            return res;
        }

        public MsgAutoRule GetRegexRule(string wxConfigID, string content)
        {
            MsgAutoRule res = null;
            if (!string.IsNullOrEmpty(wxConfigID) && !string.IsNullOrEmpty(content))
            {
                string sql = @"SELECT * FROM [WX_MsgAutoRule]
                        WHERE Enabled=1 AND MatchType='regex' AND WXConfigID=@WXConfigID 
                        ORDER BY [Order] ASC";
                IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@WXConfigID", wxConfigID)
                };
                DataSet ds = DbHelperSQL.Query(sql, paras.ToArray());
                List <MsgAutoRule> list = ds.ConvertToList<MsgAutoRule>();
                foreach (MsgAutoRule li in list)
                {
                    if (Regex.IsMatch(content, li.MatchPattern))
                    {
                        res = li;
                        break;
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// 获取自定义菜单Click事件规则
        /// </summary>
        /// <param name="wxConfigID"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public MsgAutoRule GetClickEventRule(string wxConfigID, string eventKey)
        {
            MsgAutoRule res = null;
            if (!string.IsNullOrEmpty(wxConfigID) && !string.IsNullOrEmpty(eventKey))
            {
                string sql = @"SELECT * FROM [WX_MsgAutoRule]
                        WHERE Enabled=1 AND MatchType='clickevent' AND WXConfigID=@WXConfigID AND MatchPattern=@MatchPattern 
                        ORDER BY [Order] ASC";
                IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@WXConfigID", wxConfigID),
                    new System.Data.SqlClient.SqlParameter("@MatchPattern", eventKey)
                };
                DataSet ds = DbHelperSQL.Query(sql, paras.ToArray());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    res = new MsgAutoRule()
                    {
                        ID = dr.GetColumnValue("ID", string.Empty),
                        WXConfigID = dr.GetColumnValue("WXConfigID", string.Empty),
                        MatchType = dr.GetColumnValue("MatchType", string.Empty),
                        MatchPattern = dr.GetColumnValue("MatchPattern", string.Empty),
                        MsgType = dr.GetColumnValue("MsgType", string.Empty),
                        MsgValue = dr.GetColumnValue("MsgValue", string.Empty),
                        Handle = dr.GetColumnValue("Handle", string.Empty),
                        AddTime = dr.GetColumnValue("AddTime", DateTime.Now),
                        AddUser = dr.GetColumnValue("AddUser", string.Empty),
                        LastModTime = dr.GetColumnValue("LastModTime", DateTime.Now),
                        LastModUser = dr.GetColumnValue("LastModUser", string.Empty)
                    };
                }
            }
            return res;
        }

        /// <summary>
        /// 获取订阅回复规则
        /// </summary>
        /// <param name="wxConfigID"></param>
        /// <returns></returns>
        public MsgAutoRule GetSubscribeRule(string wxConfigID)
        {
            MsgAutoRule res = null;
            if (!string.IsNullOrEmpty(wxConfigID))
            {
                string sql = @"SELECT * FROM [WX_MsgAutoRule]
                        WHERE Enabled=1 AND  MatchType='subscribe' AND WXConfigID=@WXConfigID
                        ORDER BY [Order] ASC";
                IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@WXConfigID", wxConfigID)
                };
                DataSet ds = DbHelperSQL.Query(sql, paras.ToArray());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    res = new MsgAutoRule()
                    {
                        ID = dr.GetColumnValue("ID", string.Empty),
                        WXConfigID = dr.GetColumnValue("WXConfigID", string.Empty),
                        MatchType = dr.GetColumnValue("MatchType", string.Empty),
                        MatchPattern = dr.GetColumnValue("MatchPattern", string.Empty),
                        MsgType = dr.GetColumnValue("MsgType", string.Empty),
                        MsgValue = dr.GetColumnValue("MsgValue", string.Empty),
                        Handle = dr.GetColumnValue("Handle", string.Empty),
                        AddTime = dr.GetColumnValue("AddTime", DateTime.Now),
                        AddUser = dr.GetColumnValue("AddUser", string.Empty),
                        LastModTime = dr.GetColumnValue("LastModTime", DateTime.Now),
                        LastModUser = dr.GetColumnValue("LastModUser", string.Empty)
                    };
                }
            }
            return res;
        }

        /// <summary>
        /// 获取默认回复规则
        /// </summary>
        /// <param name="wxConfigID"></param>
        /// <returns></returns>
        public MsgAutoRule GetDefaultRule(string wxConfigID)
        {
            MsgAutoRule res = null;
            if (!string.IsNullOrEmpty(wxConfigID))
            {
                string sql = @"SELECT * FROM [WX_MsgAutoRule]
                        WHERE Enabled=1 AND  MatchType='default' AND WXConfigID=@WXConfigID
                        ORDER BY [Order] ASC";
                IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@WXConfigID", wxConfigID)
                };
                DataSet ds = DbHelperSQL.Query(sql, paras.ToArray());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    res = new MsgAutoRule()
                    {
                        ID = dr.GetColumnValue("ID", string.Empty),
                        WXConfigID = dr.GetColumnValue("WXConfigID", string.Empty),
                        MatchType = dr.GetColumnValue("MatchType", string.Empty),
                        MatchPattern = dr.GetColumnValue("MatchPattern", string.Empty),
                        MsgType = dr.GetColumnValue("MsgType", string.Empty),
                        MsgValue = dr.GetColumnValue("MsgValue", string.Empty),
                        Handle = dr.GetColumnValue("Handle", string.Empty),
                        AddTime = dr.GetColumnValue("AddTime", DateTime.Now),
                        AddUser = dr.GetColumnValue("AddUser", string.Empty),
                        LastModTime = dr.GetColumnValue("LastModTime", DateTime.Now),
                        LastModUser = dr.GetColumnValue("LastModUser", string.Empty)
                    };
                }
            }
            return res;
        }

        public MsgAutoRule GetImageRule(string wxConfigID)
        {
            MsgAutoRule res = null;
            if (!string.IsNullOrEmpty(wxConfigID))
            {
                string sql = @"SELECT * FROM [WX_MsgAutoRule]
                        WHERE Enabled=1 AND MatchType='image' AND WXConfigID=@WXConfigID  
                        ORDER BY [Order] ASC";
                IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@WXConfigID", wxConfigID)
                };
                DataSet ds = DbHelperSQL.Query(sql, paras.ToArray());
                res = ds.ConvertToFirstObj<MsgAutoRule>();
            }
            return res;
        }

        #region 自动回复规则添加
        /// <summary>
        /// 自动回复规则添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddMsgAutoRule(MsgAutoRule model)
        {
            string sql = @"INSERT INTO [WX_MsgAutoRule]
                        ([ID],[WXConfigID],[MatchType],[MatchPattern],[MsgType],[MsgValue],[Handle],[AddTime],[AddUser],[LastModTime],[LastModUser],[Order],[Enabled])
                 VALUES
                        (@ID,@WXConfigID,@MatchType,@MatchPattern,@MsgType,@MsgValue,@Handle,@AddTime,@AddUser,@LastModTime,@LastModUser,@Order,@Enabled)";
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@WXConfigID", model.WXConfigID),
                new System.Data.SqlClient.SqlParameter("@MatchType", model.MatchType),
                new System.Data.SqlClient.SqlParameter("@MatchPattern", model.MatchPattern),
                new System.Data.SqlClient.SqlParameter("@MsgType", model.MsgType),
                new System.Data.SqlClient.SqlParameter("@MsgValue", model.MsgValue),
                new System.Data.SqlClient.SqlParameter("@Handle", model.Handle),
                new System.Data.SqlClient.SqlParameter("@AddTime", DateTime.Now),
                new System.Data.SqlClient.SqlParameter("@AddUser", model.AddUser),
                new System.Data.SqlClient.SqlParameter("@LastModTime",DateTime.Now),
                new System.Data.SqlClient.SqlParameter("@LastModUser", model.LastModUser),
                new System.Data.SqlClient.SqlParameter("@Order", model.Order),
                new System.Data.SqlClient.SqlParameter("@Enabled", (model.Enabled==1?1:0))
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

        #region 修改自动回复规则
        /// <summary>
        /// 修改自动回复规则
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateMsgAutoRule(MsgAutoRule model)
        {
            string safesql = "";
            safesql = " update WX_MsgAutoRule set ";
            if (model.WXConfigID != null&&model.WXConfigID != "")
            {
                safesql += "[WXConfigID]='" + model.WXConfigID + "',";
            }
            if (model.MatchType != null && model.MatchType != "")
            {
                safesql += "[MatchType]='" + model.MatchType + "',";
            }
            if (model.MatchPattern != null && model.MatchPattern != "")
            {
                safesql += "[MatchPattern]='" + model.MatchPattern + "',";
            }
            if (model.MsgType != null && model.MsgType != "")
            {
                safesql += "[MsgType]='"+ model.MsgType + "',";
            }
            if (model.MsgValue != null && model.MsgValue != "")
            {
                safesql += "[MsgValue]='" + model.MsgValue + "',";
            }
            if (model.Handle != null && model.Handle != "")
            {
                safesql += "[Handle]='" + model.Handle + "',";
            }
            if (model.AddUser != null && model.AddUser.ToString() != "")
            {
                safesql += "[AddUser]='" + model.AddUser + "',";
            }
            if (model.LastModTime != null && model.LastModTime.ToString() != "")
            {
                safesql += "[LastModTime]='" + model.LastModTime + "',";
            }
            if (model.LastModUser != null && model.LastModUser.ToString() != "")
            {
                safesql += "[LastModUser]='" + model.LastModUser + "',";
            }
            if (model.Order > 0 && model.Order.ToString() != "")
            {
                safesql += "[Order]=" + model.Order + ",";
            }
            if (model.Enabled > 0 && model.Enabled.ToString() != "")
            {
                safesql += "[Enabled]=" + (model.Enabled==1?1:0) + ",";
            }
            safesql += "[AddTime]='" + DateTime.Now + "' where ID='" + model.ID + "'";
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

        #region 根据标识获取相对于的信息
        /// <summary>
        /// 根据标识获取相对于的信息
        /// </summary>
        /// <param name="value"></param>
        /// <param name="msgid"></param>
        /// <returns></returns>
        public object GetMsgAutoRuleValue(string value, string msgid)
        {
            string safesql = "";
            safesql = "select [" + value + "] from WX_MsgAutoRule where ID='" + msgid + "'";
            return DbHelperSQL.GetSingle(safesql.ToString());
        }
        #endregion

        #region 更新自动回复信息状态
        /// <summary>
        /// 更新自动回复信息状态
        /// </summary>
        /// <param name="msgid"></param>
        /// <returns></returns>
        public bool UpdateMsgAutoRuleEnable(string msgid)
        {
            StringBuilder strSql = new StringBuilder();
            int state = 0;
            try
            {
                state = Convert.ToInt32(GetMsgAutoRuleValue("Enabled", msgid));
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
            strSql.Append(" UPDATE WX_MsgAutoRule ");
            strSql.Append(" SET [Enabled] = @Enabled ");
            strSql.Append(" WHERE ID = @ID ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", msgid),
                new System.Data.SqlClient.SqlParameter("@Enabled", state)
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

        #region 返回所有自动回复
        /// <summary>
        /// 返回所有自动回复
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet GetMsgAutoRuleList(string where)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("  select * from WX_MsgAutoRule  ");
            if (!string.IsNullOrEmpty(where) && where.Trim() != null && where.Trim() != "")
            {
                strSql.Append(" WHERE " + where);
            }
            strSql.Append(" order by [Order] asc ");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion

        #region 根据条件获取所有有效的自动回复列表
        /// <summary>
        /// 根据条件获取所有有效的自动回复列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet GetMsuAutoRuleListByEnable(string where)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("  select * from WX_MsgAutoRule  WHERE Enable=1 ");
            if (!string.IsNullOrEmpty(where) && where.Trim() != null && where.Trim() != "")
            {
                strSql.Append(" AND " + where);
            }
            strSql.Append(" order by [Order] asc ");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion

        #region 获取关键字文本回复列表
        /// <summary>
        /// 获取关键字文本回复列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet GetKeywordTextMsgAutoRuleList(string where)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT a.*,b.SiteCode as SiteCode,b.WXID as WXID,b.WXName as WXName"+
                " FROM WX_MsgAutoRule a LEFT JOIN WX_WXConfig " +
                " b ON (a.WXConfigID=b.ID) WHERE a.Enabled=1 and  MatchType='keywords' and MsgType='text' ");
            if (!string.IsNullOrEmpty(where) && where.Trim() != null && where.Trim() != "")
            {
                strSql.Append(" AND " + where);
            }
            strSql.Append(" order by [Order] asc ");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion

        #region 获取关键字文本回复列表
        /// <summary>
        /// 获取关键字语音回复列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet GetKeywordVoiceMsgAutoRuleList(string where)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT a.*,b.SiteCode as SiteCode,b.WXID as WXID,b.WXName as WXName" +
                " FROM WX_MsgAutoRule a LEFT JOIN WX_WXConfig " +
                " b ON (a.WXConfigID=b.ID) WHERE a.Enabled=1 and  MatchType='keywords' and MsgType='voice' ");
            if (!string.IsNullOrEmpty(where) && where.Trim() != null && where.Trim() != "")
            {
                strSql.Append(" AND " + where);
            }
            strSql.Append(" order by [Order] asc ");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion

        #region 获取自动回复详细
        /// <summary>
        /// 获取自动回复详细
        /// </summary>
        /// <param name="msgid"></param>
        /// <returns></returns>
        public DataSet GetMsuAutoRuleDetail(string msgid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM WX_MsgAutoRule ");
            strSql.Append(" WHERE ID = @ID ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", msgid)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion 
    }
}
