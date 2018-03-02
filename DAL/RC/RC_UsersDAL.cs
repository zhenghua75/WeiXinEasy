using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.RC;
using Maticsoft.DBUtility;

namespace DAL.RC
{
   public class RC_UsersDAL
    {
       public RC_UsersDAL() { ;}

       #region 添加赛跑用户
       /// <summary>
       /// 添加赛跑用户
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddRCUser(RC_Users model)
       {
           string sql = @"INSERT INTO [RC_Users]
                        ([ID],[RaceID],[OpenID],[Speed],[IsDel],[IsWin],[AddTime])
                 VALUES
                        (@ID,@RaceID,@OpenID,@Speed,@IsDel,@IsWin,@AddTime)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@RaceID", model.RaceID),
                new System.Data.SqlClient.SqlParameter("@OpenID", model.OpenID),
                new System.Data.SqlClient.SqlParameter("@Speed", model.Speed),
                new System.Data.SqlClient.SqlParameter("@IsDel", (model.IsDel==1?1:0)),
                new System.Data.SqlClient.SqlParameter("@IsWin", (model.IsWin==1?1:0)),
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
       #region 修改赛跑用户信息
       /// <summary>
       /// 修改赛跑用户信息
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateCRUser(RC_Users model)
       {
           string safesql = "";
           safesql = " update RC_Users set ";
           if (model.OpenID != null && model.OpenID != "")
           {
               safesql += "[OpenID]='" + model.OpenID + "',";
           }
           if (model.RaceID != null && model.RaceID != "")
           {
               safesql += "[RaceID]='" + model.RaceID + "',";
           }
           if (model.Speed != null && model.Speed != "")
           {
               safesql += "[Speed]='" + model.Speed + "',";
           }
           safesql += "[IsDel]=" + (model.IsDel == 1 ? 1 : 0) + ",";
           safesql += "[IsWin]=" + (model.IsWin == 1 ? 1 : 0);
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
       #region 更新用户速度
       /// <summary>
       /// 更新用户速度
       /// </summary>
       /// <param name="strID"></param>
       /// <param name="strSpeed"></param>
       /// <param name="strRID"></param>
       /// <returns></returns>
       public bool UpdateUserSpeedByOpenID(string strID, string strSpeed,string strRID)
       {
           string safesql = "";
           safesql = " update RC_Users set Speed='" + strSpeed + "' where RaceID='" + strRID + "' and ID='" + strID + "'";
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
       #region 获取赛跑用户属性
       /// <summary>
       /// 获取赛跑用户属性
       /// </summary>
       /// <param name="value"></param>
       /// <param name="strID"></param>
       /// <returns></returns>
       public object GetRCUserValueByID(string value,string strID)
       {
           string safesql = "";
           safesql = "select " + value + " from RC_Users where ID='" + strID + "'";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion 
       #region 获取用户属性信息
       /// <summary>
       /// 获取用户属性信息
       /// </summary>
       /// <param name="strvalue"></param>
       /// <param name="where"></param>
       /// <returns></returns>
       public object GetRaceUserValue(string strvalue, string strOpenID,string strRid)
       {
           string safesql = "";
           safesql = "select " + strvalue + " from RC_Users where OpenID='"+strOpenID+"' and RaceID='"+strRid+"' ";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion
       #region 更新赛跑用户状态
       /// <summary>
       /// 更新赛跑用户状态
       /// </summary>
       /// <param name="valueName"></param>
       /// <param name="strID"></param>
       /// <returns></returns>
       public bool UpdateRCUserIsDel(string valueName,string strID)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetRCUserValueByID(valueName, strID));
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
           strSql.Append(" UPDATE RC_Users ");
           strSql.Append(" SET " + valueName + " = @valueName ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strID),
                new System.Data.SqlClient.SqlParameter("@valueName", state)
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
       #region 获取赛跑用户信息列表
       /// <summary>
       /// 获取赛跑用户信息列表
       /// </summary>
       /// <param name="where"></param>
       /// <returns></returns>
       public DataSet GetRCUserList(string where)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select a.*,case a.IsWin when 0 then '未获奖' when 1 then '获奖' end as Winer,");
           strSql.Append(" b.SiteCode SiteCode,b.Rtitle Rtitle,b.RaceDesc RaceDesc,b.MoveNum,");
           strSql.Append(" b.StartTime StartTime,b.EndTime EndTime,c.WXConfigID WXConfigID,c.NickName NickName,");
           strSql.Append(" c.City City,c.Province Province,c.HeadImgUrl HeadImgUrl,c.Sex Sex ");
           strSql.Append(" from RC_Users a,RC_Race b,WX_User c ");
           strSql.Append(" where a.RaceID=b.ID and a.OpenID=c.OpenID and a.IsDel=0 and b.IsDel=0  ");
           if (!string.IsNullOrEmpty(where))
           {
               strSql.Append("  " + where);
           }
           strSql.Append(" order by CAST(REPLACE(Speed,'.','') as decimal) desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 获取赛跑用户列表
       /// <summary>
       /// 获取赛跑用户列表
       /// </summary>
       /// <param name="where"></param>
       /// <returns></returns>
       public DataSet GetRcUserList(string where)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select a.*,case a.IsWin when 0 then '未获奖' when 1 then '获奖' end as Winer,");
           strSql.Append(" b.SiteCode SiteCode,b.Rtitle Rtitle,b.RaceDesc RaceDesc,b.MoveNum,");
           strSql.Append(" b.StartTime StartTime,b.EndTime EndTime");
           strSql.Append(" from RC_Users a,RC_Race b");
           strSql.Append(" where a.RaceID=b.ID and a.IsDel=0 and b.IsDel=0 and  a.Speed <>'' and a.Speed is not null ");
           if (!string.IsNullOrEmpty(where))
           {
               strSql.Append("  " + where);
           }
           strSql.Append(" order by CAST(REPLACE(Speed,'.','') as decimal) desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 按条数获取赛跑用户
       /// <summary>
       /// 按条数获取赛跑用户
       /// </summary>
       /// <param name="top"></param>
       /// <param name="where"></param>
       /// <returns></returns>
       public DataSet GetTopRcuserList(int top, string where)
       {
           StringBuilder strSql = new StringBuilder();
           string topnum = string.Empty;
           if (top > 0)
           {
               topnum = " top "+top+" ";
           }
           strSql.Append(" select " + topnum + " a.*,case a.IsWin when 0 then '未获奖' when 1 then '获奖' end as Winer,");
           strSql.Append(" b.SiteCode SiteCode,b.Rtitle Rtitle,b.RaceDesc RaceDesc,b.MoveNum,");
           strSql.Append(" b.StartTime StartTime,b.EndTime EndTime,c.WXConfigID WXConfigID,c.NickName NickName,");
           strSql.Append(" c.City City,c.Province Province,c.HeadImgUrl HeadImgUrl,c.Sex Sex ");
           strSql.Append(" from RC_Users a,RC_Race b,WX_User c ");
           strSql.Append(" where a.RaceID=b.ID and a.OpenID=c.OpenID and a.IsDel=0 and b.IsDel=0  ");
           if (!string.IsNullOrEmpty(where))
           {
               strSql.Append("  " + where);
           }
           strSql.Append(" order by CAST(REPLACE(Speed,'.','') as decimal) desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 获取最新赛跑用户列表
       /// <summary>
       /// 获取最新赛跑用户列表
       /// </summary>
       /// <param name="top"></param>
       /// <param name="where"></param>
       /// <returns></returns>
       public DataSet GetTopRcUserList(int top, string where)
       {
           StringBuilder strSql = new StringBuilder();
           string topnum = string.Empty;
           if (top > 0)
           {
               topnum = " top " + top + " ";
           }
           strSql.Append(" select " + topnum + " a.*,case a.IsWin when 0 then '未获奖' when 1 then '获奖' end as Winer,");
           strSql.Append(" b.SiteCode SiteCode,b.Rtitle Rtitle,b.RaceDesc RaceDesc,b.MoveNum,");
           strSql.Append(" b.StartTime StartTime,b.EndTime EndTime");
           strSql.Append(" from RC_Users a,RC_Race b");
           strSql.Append(" where a.RaceID=b.ID and a.IsDel=0 and b.IsDel=0  ");
           if (!string.IsNullOrEmpty(where))
           {
               strSql.Append("  " + where);
           }
           strSql.Append(" order by CAST(REPLACE(Speed,'.','') as decimal) desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 判断用户是否存在
       /// <summary>
       /// 判断用户是否存在
       /// </summary>
       /// <param name="openID"></param>
       /// <param name="rid"></param>
       /// <returns></returns>
       public bool ExistUser(string openID,string rid)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT count(ID) ");
           strSql.Append(" FROM RC_Users ");
           strSql.Append(" WHERE [RaceID] = @RaceID ");
           strSql.Append(" AND [OpenID] = @OpenID ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@RaceID", rid),
                    new System.Data.SqlClient.SqlParameter("@OpenID", openID)
                };
           return DbHelperSQL.Exists(strSql.ToString(), paras.ToArray());
       }
       #endregion
    }
}
