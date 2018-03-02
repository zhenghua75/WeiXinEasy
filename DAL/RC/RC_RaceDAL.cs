using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.RC;
using Maticsoft.DBUtility;

namespace DAL.RC
{
   public class RC_RaceDAL
    {
       public RC_RaceDAL() { ;}
       #region 添加赛跑信息说明
       /// <summary>
       /// 添加赛跑信息说明
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddRCRace(RC_Race model)
       {
           string sql = @"INSERT INTO [RC_Race]
                        ([ID],[SiteCode],[Rtitle],[RaceDesc],[AppID],[CodeImg],[MoveNum],[StartTime],[EndTime],[IsDel],[AddTime])
                 VALUES
                        (@ID,@SiteCode,@Rtitle,@RaceDesc,@AppID,@CodeImg,@MoveNum,@StartTime,@EndTime,@IsDel,@AddTime)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@SiteCode", model.SiteCode),
                new System.Data.SqlClient.SqlParameter("@Rtitle", model.Rtitle),
                new System.Data.SqlClient.SqlParameter("@RaceDesc", model.RaceDesc),
                new System.Data.SqlClient.SqlParameter("@AppID", model.AppID),
                new System.Data.SqlClient.SqlParameter("@CodeImg", model.CodeImg),
                new System.Data.SqlClient.SqlParameter("@MoveNum", model.MoveNum),
                new System.Data.SqlClient.SqlParameter("@StartTime", model.StartTime),
                new System.Data.SqlClient.SqlParameter("@EndTime", model.EndTime),
                new System.Data.SqlClient.SqlParameter("@IsDel", (model.IsDel==1?1:0)),
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
       #region 更新赛跑信息
       /// <summary>
       /// 更新赛跑信息
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateRCRace(RC_Race model)
       {
           string safesql = "";
           safesql = " update RC_Race set ";
           if (model.SiteCode != null && model.SiteCode != "")
           {
               safesql += "[SiteCode]='" + model.SiteCode + "',";
           }
           if (model.Rtitle != null && model.Rtitle != "")
           {
               safesql += "[Rtitle]='" + model.Rtitle + "',";
           }
           if (model.RaceDesc != null && model.RaceDesc != "")
           {
               safesql += "[RaceDesc]='" + model.RaceDesc + "',";
           }
           if (model.StartTime != null && model.StartTime.ToString() != "")
           {
               safesql += "[StartTime]='" + model.StartTime + "',";
           }
           if (model.EndTime != null && model.EndTime.ToString() != "")
           {
               safesql += "[EndTime]='" + model.EndTime + "',";
           }
           if (model.CodeImg != null && model.CodeImg.ToString() != "")
           {
               safesql += "[CodeImg]='" + model.CodeImg + "',";
           }
           if (model.AppID != null && model.AppID.ToString() != "")
           {
               safesql += "[AppID]='" + model.AppID + "',";
           }
           safesql += "[MoveNum]=" + (model.MoveNum > 0 ? model.MoveNum : 3000)+",";
           safesql += "[IsDel]=" + (model.IsDel == 1 ? 1 : 0);
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
       #region 获取赛跑属性
       /// <summary>
       /// 获取赛跑属性
       /// </summary>
       /// <param name="value"></param>
       /// <param name="strID"></param>
       /// <returns></returns>
       public object GetRCRaceValueByID(string value, string strID)
       {
           string safesql = "";
           safesql = "select " + value + " from RC_Race where ID='" + strID + "'";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion 
       #region 更新赛跑信息属性
       /// <summary>
       /// 更新赛跑信息属性
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public bool UpdateRCRaceIsDel(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetRCRaceValueByID("IsDel", strID));
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
           strSql.Append(" UPDATE RC_Race ");
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
       #region 获取有效的赛跑列表
       /// <summary>
       /// 获取有效的赛跑列表
       /// </summary>
       /// <param name="where"></param>
       /// <returns></returns>
       public DataSet GetRCRList(string where)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * from RC_Race WHERE IsDel=0 ");
           if (!string.IsNullOrEmpty(where))
           {
               strSql.Append(" AND " + where);
           }
           strSql.Append(" order by AddTime asc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 获取赛跑信息详细
       /// <summary>
       /// 获取赛跑信息详细
       /// </summary>
       /// <param name="strID"></param>
       /// <param name="strSiteCode"></param>
       /// <returns></returns>
       public DataSet GetRaceDetail(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM RC_Race ");
           strSql.Append(" WHERE [ID] = @ID");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", strID)
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion 
    }
}
