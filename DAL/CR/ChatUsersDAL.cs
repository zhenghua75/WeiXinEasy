using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.CR;
using Maticsoft.DBUtility;

namespace DAL.CR
{
   public class ChatUsersDAL
    {
       public ChatUsersDAL() { ;}

       #region 添加聊天室用户
       /// <summary>
       /// 添加聊天室用户
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddChatUsers(ChatUsers model)
       {
           string sql = @"INSERT INTO [CR_ChatUsers]
                        ([ID],[OpenID],[NickName],[RoomID],[AddTime],[IsDel],[IsWin])
                 VALUES
                        (@ID,@OpenID,@NickName,@RoomID,@AddTime,@IsDel,@IsWin)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@OpenID", model.OpenID),
                new System.Data.SqlClient.SqlParameter("@NickName", model.NickName),
                new System.Data.SqlClient.SqlParameter("@RoomID", model.RoomID),
                new System.Data.SqlClient.SqlParameter("@AddTime", DateTime.Now),
                new System.Data.SqlClient.SqlParameter("@IsDel", (model.IsDel==1?1:0)),
                new System.Data.SqlClient.SqlParameter("@IsWin", (model.IsWin==1?1:0))
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

       #region 更新聊天室用户
       /// <summary>
       /// 更新聊天室用户
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateChatUsers(ChatUsers model)
       {
           string safesql = "";
           safesql = " update CR_ChatUsers set ";
           if (model.OpenID != null && model.OpenID != "")
           {
               safesql += "[OpenID]='" + model.OpenID + "',";
           }
           if (model.NickName != null && model.NickName != "")
           {
               safesql += "[NickName]='" + model.NickName + "',";
           }
           safesql += "[IsDel]=" + (model.IsDel == 1 ? 1 : 0)+",";
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

       #region 获取聊天室用户属性
       /// <summary>
       /// 获取聊天室用户属性
       /// </summary>
       /// <param name="value"></param>
       /// <param name="ChatUserID"></param>
       /// <returns></returns>
       public object GetChatUserValueByID(string value, string ChatUserID)
       {
           string safesql = "";
           safesql = "select " + value + " from CR_ChatUsers where ID='" + ChatUserID + "'";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion

       #region 更新聊天室用户状态
       /// <summary>
       /// 更新聊天室用户状态
       /// </summary>
       /// <param name="ChatUserID"></param>
       /// <returns></returns>
       public bool UpdateChatUserIsDel(string ChatUserID)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetChatUserValueByID("IsDel", ChatUserID));
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
           strSql.Append(" UPDATE CR_ChatUsers ");
           strSql.Append(" SET IsDel = @IsDel ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", ChatUserID),
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

       #region 根据OpenID获取相关属性
       /// <summary>
       /// 根据OpenID获取相关属性
       /// </summary>
       /// <param name="value"></param>
       /// <param name="ChatUserOpenID"></param>
       /// <returns></returns>
       public object GetChatUserValueByOpenID(string value, string ChatOpenIDID, int rmID)
       {
           string safesql = "";
           safesql = "select " + value + " from CR_ChatUsers where OpenID='" + ChatOpenIDID + "' AND RoomID=" + rmID;
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion

       #region 获取聊天室有效用户列表
       /// <summary>
       /// 获取聊天室有效用户列表
       /// </summary>
       /// <param name="where"></param>
       /// <returns></returns>
       public DataSet GetChatUsersList(string where)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select a.ID ID,a.OpenID OpenID,a.RoomID RoomID,a.AddTime AddTime,a.IsDel IsDel,a.IsWin IsWin,");
           strSql.Append(" (case when (a.OpenID='')then NickName when (a.OpenID=null)then NickName ");
           strSql.Append(" when (a.OpenID!='') then (select NickName from WX_User where OpenID=a.OpenID) end) NickName, ");
           strSql.Append(" b.SiteCode SiteCode,b.RoomName RoomName,b.PhoneNum PhoneNum,");
           strSql.Append(" b.RoomDesc RoomDesc,b.RoomImg RoomImg ");
           strSql.Append(" from CR_ChatUsers a,CR_ChatRoom b ");
           strSql.Append(" where a.RoomID=b.ID and a.IsDel=0 and b.IsDel=0 and a.RoomID=b.ID ");
           if (!string.IsNullOrEmpty(where))
           {
               strSql.Append("  " + where);
           }
           strSql.Append(" order by a.AddTime desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion

       #region 获取某房间用户数量
       /// <summary>
       /// 获取某房间用户数量
       /// </summary>
       /// <param name="strRoomID"></param>
       /// <returns></returns>
       public object GetUserCount(int strRoomID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT count(ID) ");
           strSql.Append(" FROM CR_ChatUsers ");
           strSql.Append(" WHERE [RoomID] = @RoomID AND IsDel=0 ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@RoomID", strRoomID)
                };
           object count = "0";
           try
           {
               count=DbHelperSQL.GetSingle(strSql.ToString(), paras.ToArray()).ToString();
           }
           catch (Exception)
           {
               count = "0";
           }
           return count;
       }
       #endregion

       #region 更新抽奖抽中用户状态
       /// <summary>
       /// 更新抽奖抽中用户状态
       /// </summary>
       /// <param name="openid"></param>
       /// <param name="ChatUserID"></param>
       /// <returns></returns>
       public bool UpdateUserWin(string ChatUserID,int rmid)
       {
           StringBuilder strSql = new StringBuilder();
           int state =1;
           try
           {
               state = Convert.ToInt32(GetChatUserValueByID("IsWin", ChatUserID));
           }
           catch (Exception)
           {
               state =1;
           }
           switch (state)
           {
               case 1:
                   state = 0;
                   break;
               default:
                   state =1;
                   break;
           }
           strSql.Append(" UPDATE CR_ChatUsers ");
           strSql.Append(" SET IsWin = @IsWin ");
           strSql.Append(" WHERE ID = @ID AND RoomID=@RoomID");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", ChatUserID),
                new System.Data.SqlClient.SqlParameter("@RoomID", rmid),
                new System.Data.SqlClient.SqlParameter("@IsWin", state)
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

       #region 判断用户是否存在
       /// <summary>
       /// 判断用户是否存在
       /// </summary>
       /// <param name="strRoomID"></param>
       /// <param name="OpenID"></param>
       /// <returns></returns>
       public bool ExistChatUser(int strRoomID,string OpenID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT count(ID) ");
           strSql.Append(" FROM CR_ChatUsers ");
           strSql.Append(" WHERE [RoomID] = @RoomID ");
           strSql.Append(" AND [OpenID] = @OpenID ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@RoomID", strRoomID),
                    new System.Data.SqlClient.SqlParameter("@OpenID", OpenID)
                };
           return DbHelperSQL.Exists(strSql.ToString(), paras.ToArray());
       }
       #endregion
       #region 根据OpenID判断用户是否存在
       /// <summary>
       /// 根据OpenID判断用户是否存在
       /// </summary>
       /// <param name="strOpenID"></param>
       /// <returns></returns>
       public bool ExistChatUser(string strOpenID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT count(ID) ");
           strSql.Append(" FROM CR_ChatUsers ");
           strSql.Append(" WHERE [OpenID] = @OpenID ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@OpenID", strOpenID)
                };
           return DbHelperSQL.Exists(strSql.ToString(), paras.ToArray());
       }
       #endregion
       #region 删除所有获奖用户
       /// <summary>
       /// 删除所有获奖用户
       /// </summary>
       /// <param name="strRoomID"></param>
       /// <returns></returns>
       public bool UpdateAllIsWinByRoomID(string strRoomID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" UPDATE CR_ChatUsers ");
           strSql.Append(" SET IsWin =0 ");
           strSql.Append(" WHERE RoomID=@RoomID");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@RoomID", strRoomID)
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
