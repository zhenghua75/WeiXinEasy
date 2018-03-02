using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.Game;
using Maticsoft.DBUtility;

namespace DAL.Game
{
    public class LuckyAwardDAL
    {
        public LuckyAwardDAL() { ;}
        #region 添加奖项设置
        /// <summary>
        /// 添加奖项设置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddAward(LuckyAward model)
        {
            string sql = @"INSERT INTO [Game_LuckyAward]
                        (ID,ActID,Award,AwardContent,AwardNum,AwardPro,AwardSort,AddTime,IsDel)
                 VALUES
                        (@ID,@ActID,@Award,@AwardContent,@AwardNum,@AwardPro,@AwardSort,@AddTime,@IsDel)";
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@ActID", model.ActID),
                new System.Data.SqlClient.SqlParameter("@Award", model.Award),
                new System.Data.SqlClient.SqlParameter("@AwardContent", model.AwardContent),
                new System.Data.SqlClient.SqlParameter("@AwardNum", (model.AwardNum!=0?model.AwardNum:1)),
                new System.Data.SqlClient.SqlParameter("@AwardPro", (model.AwardPro!=0?model.AwardPro:1)),
                new System.Data.SqlClient.SqlParameter("@AwardSort",(model.AwardSort!=0?model.AwardSort:0)),
                new System.Data.SqlClient.SqlParameter("@AddTime", DateTime.Now),
                new System.Data.SqlClient.SqlParameter("@IsDel", (model.IsDel==1?1:0))
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
        #region 更新奖项设置
        /// <summary>
        /// 更新奖项设置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateAward(LuckyAward model)
        {
            string safesql = "";
            safesql = " update Game_LuckyAward set ";
            if (model.Award.ToString() != null && model.Award.ToString() != "")
            {
                safesql += "[Award]='" + model.Award + "',";
            }
            if (model.ActID.ToString() != null && model.ActID.ToString() != "")
            {
                safesql += "[ActID]='" + model.ActID + "',";
            }
            if (model.AwardContent != null && model.AwardContent != "")
            {
                safesql += "[AwardContent]='" + model.AwardContent + "',";
            }
            safesql += "[AwardNum]=" + (model.AwardNum!=0?model.AwardNum:1) + ",";
            safesql += "[AwardPro]=" + (model.AwardPro!=0?model.AwardPro:1) + ",";
            safesql += "[AwardSort]=" + (model.AwardSort != 0 ? model.AwardSort : 0) + ",";
            safesql += "[IsDel]=" + (model.IsDel == 1 ? 1 : 0);
            safesql += " where id='" + model.ID + "' ";
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
        #region 获取奖项属性
        /// <summary>
        /// 获取奖项属性
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="strID"></param>
        /// <returns></returns>
        public object GetAwardValueByID(string strValue, string strID)
        {
            string safesql = "";
            safesql = "select " + strValue + " from Game_LuckyAward where ID='" + strID + "'";
            return DbHelperSQL.GetSingle(safesql.ToString());
        }
        #endregion
        #region 更新奖项状态
        /// <summary>
        /// 更新奖项状态
        /// </summary>
        /// <param name="strID"></param>
        /// <returns></returns>
        public bool UpdateAwardIsDel(string strID)
        {
            StringBuilder strSql = new StringBuilder();
            int state = 0;
            try
            {
                state = Convert.ToInt32(GetAwardValueByID("IsDel", strID));
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
            strSql.Append(" UPDATE Game_LuckyAward ");
            strSql.Append(" SET IsDel = @IsDel ");
            strSql.Append(" WHERE ID = @ID ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strID),
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
        #region 获取奖项列表
        /// <summary>
        /// 获取奖项列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetAwardList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select a.*,Award,AwardNum,ActTitle from Game_LuckyAward a,ACT_SiteActivity b   ");
            strSql.Append(" where a.IsDel=0  and a.ActID=b.ID ");
            if (strWhere.Trim() != null && strWhere.Trim() != "")
            {
                strSql.Append("  " + strWhere);
            }
            strSql.Append(" order by a.AddTime asc ");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion

        #region 根据活动编号、奖项类别 获取奖项列表
        /// <summary>
        /// 根据活动编号、奖项类别 获取奖项列表 
        /// 奖项类别 0 表示大转盘 1 表示刮刮卡
        /// </summary>
        /// <param name="strActID"></param>
        /// <returns></returns>
        public DataSet GetActAwardList(string strActID,int strAwardSort)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT b.ID,b.Award,b.AwardNum,b.AwardPro,a.ActTitle ");
            strSql.Append(" FROM ACT_SiteActivity a ");
            strSql.Append(" LEFT JOIN  Game_LuckyAward b ON (b.ActID = a.ID) ");
            strSql.Append(" WHERE a.ID = '" + strActID + "' and AwardSort="+strAwardSort+" ");
            strSql.Append(" order by b.AwardPro asc ");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion

        #region 获取奖项详细
        /// <summary>
        /// 获取奖项详细
        /// </summary>
        /// <param name="strID"></param>
        /// <returns></returns>
        public DataSet GetAwardDetail(string strID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM Game_LuckyAward ");
            strSql.Append(" WHERE [ID] = @ID ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", strID)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion
        #region 检查奖项是否存在
        /// <summary>
        /// 检查奖项是否存在
        /// </summary>
        /// <param name="strAward"></param>
        /// <param name="strSiteCode"></param>
        /// <returns></returns>
        public bool ExistAward(string strAward, string strActID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT count(ID) ");
            strSql.Append(" FROM Game_LuckyAward ");
            strSql.Append(" WHERE [ActID] = @ActID ");
            strSql.Append(" AND [Award] = @Award ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ActID", strActID),
                    new System.Data.SqlClient.SqlParameter("@Award", strAward)
                };
            return DbHelperSQL.Exists(strSql.ToString(), paras.ToArray());
        }
        #endregion
        #region 根据用户代码获取奖项总数
        /// <summary>
        /// 根据用户代码获取奖项总数
        /// </summary>
        /// <param name="strSiteCode"></param>
        /// <returns></returns>
        public int GetAwardCountBySiteCode(string strActID)
        {
            StringBuilder strSql = new StringBuilder();
            int count = 0;
            strSql.Append(" SELECT count(ID) ");
            strSql.Append(" FROM Game_LuckyAward WHERE IsDel=0 ");
            if (strActID.Trim() != null && strActID.Trim() != "")
            {
                strSql.Append(" AND  [ActID] ='" + strActID + "' ");
            }
            try
            {
                count = Convert.ToInt32(DbHelperSQL.GetSingle(strSql.ToString()));
            }
            catch (Exception)
            {
            }
            return count;
        }
        #endregion
    }
}
