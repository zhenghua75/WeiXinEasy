using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.CR;
using Maticsoft.DBUtility;

namespace DAL.CR
{
   public class ChatMessageDAL
    {
       public ChatMessageDAL() { ;}

       #region 添加聊天消息
       /// <summary>
       /// 添加聊天消息
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddChatMessage(ChatMessage model)
       {
           string sql = @"INSERT INTO [CR_ChatMessage]
                        ([ID],[UserID],[RoomID],[MsgType],[MsgText],[MsgState],[IsDel],[AddTime])
                 VALUES
                        (@ID,@UserID,@RoomID,@MsgType,@MsgText,@MsgState,@IsDel,@AddTime)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@UserID", model.UserID),
                new System.Data.SqlClient.SqlParameter("@RoomID", model.RoomID),
                new System.Data.SqlClient.SqlParameter("@MsgType", model.MsgType),
                new System.Data.SqlClient.SqlParameter("@MsgText", model.MsgText),
                new System.Data.SqlClient.SqlParameter("@MsgState", (model.MsgState==1?1:0)),
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

       #region 更新聊天消息
       /// <summary>
       /// 更新聊天消息
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateChatMessage(ChatMessage model)
       {
           string safesql = "";
           safesql = " update CR_ChatMessage set ";
           if (model.UserID != null && model.UserID != "")
           {
               safesql += "[UserID]='" + model.UserID + "',";
           }
           if (model.MsgType != null && model.MsgType != "")
           {
               safesql += "[MsgType]='" + model.MsgType + "',";
           }
           if (model.MsgText != null && model.MsgText != "")
           {
               safesql += "[MsgText]='" + model.MsgText + "',";
           }
           safesql += "[MsgState]=" + (model.MsgState == 1 ? 1 : 0);
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

       #region 获取聊天消息属性值
       /// <summary>
       /// 获取聊天消息属性值
       /// </summary>
       /// <param name="value"></param>
       /// <param name="ChatMessageID"></param>
       /// <returns></returns>
       public object GetChatMessageValueByID(string value, string ChatMessageID)
       {
           string safesql = "";
           safesql = "select " + value + " from CR_ChatMessage where ID='" + ChatMessageID + "'";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion

       #region 更新聊天消息状态
       /// <summary>
       /// 更新聊天消息状态
       /// </summary>
       /// <param name="msgID"></param>
       /// <returns></returns>
       public bool UpdateChatMsgState(string msgID)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetChatMessageValueByID("MsgState", msgID));
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
           strSql.Append(" UPDATE CR_ChatMessage ");
           strSql.Append(" SET MsgState = @MsgState ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", msgID),
                new System.Data.SqlClient.SqlParameter("@MsgState", state)
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

       #region 删除聊天消息
       /// <summary>
       /// 删除聊天消息
       /// </summary>
       /// <param name="msgID"></param>
       /// <returns></returns>
       public bool UpdateChatMsgIsDel(string msgID)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetChatMessageValueByID("IsDel", msgID));
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
           strSql.Append(" UPDATE CR_ChatMessage ");
           strSql.Append(" SET IsDel = @IsDel ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", msgID),
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

       #region 获取聊天消息列表
       /// <summary>
       /// 获取聊天消息列表
       /// </summary>
       /// <param name="where"></param>
       /// <returns></returns>
       public DataSet GetChatMsgList(string where)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select  a.*,");
           strSql.Append(" (case when (b.OpenID='') then b.NickName when (b.OpenID=null) then b.NickName  ");
           strSql.Append(" when (b.OpenID!='') then (select NickName from WX_User where OpenID=b.OpenID) end) NickName, ");
           strSql.Append(" c.RoomName RoomName,c.PhoneNum PhoneNum,c.RoomDesc RoomDesc,c.RoomImg RoomImg ");
           strSql.Append(" from CR_ChatMessage a,CR_ChatUsers b,CR_ChatRoom c ");
           strSql.Append(" where a.RoomID=c.ID and a.UserId=b.ID and a.IsDel=0  ");
           if (where.Trim() != null && where.Trim()!="")
           {
               strSql.Append("  " + where);
           }
           strSql.Append(" order by a.AddTime desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion

       #region 获取通过审核的聊天消息
       /// <summary>
       /// 获取通过审核的聊天消息
       /// </summary>
       /// <param name="top"></param>
       /// <returns></returns>
       public DataSet GetChatMsgByTop(int top, string strSiteCode, string RoomID)
       {
           StringBuilder strSql = new StringBuilder();
           if (top < 0||top==0)
           {
               top = 20;
           }
           strSql.Append(" select top "+top+" ");
           strSql.Append(" a.*,b.OpenID OpenID,b.NickName NickName,c.RoomName RoomName, ");
           strSql.Append(" c.PhoneNum PhoneNum,c.RoomDesc RoomDesc,c.RoomImg RoomImg ");
           strSql.Append(" from CR_ChatMessage a,CR_ChatUsers b,CR_ChatRoom c ");
           strSql.Append(" where a.RoomID=c.ID and a.UserId=b.ID and a.IsDel=0 and a.MsgState=1 ");
           if (strSiteCode.Trim() != null && strSiteCode.Trim() != "")
           {
               strSql.Append(" And c.SiteCode='"+strSiteCode+"' ");
           }
           if (RoomID.ToString().Trim() != null && RoomID.ToString().Trim() != "")
           {
               strSql.Append(" and a.RoomID=" + RoomID + " ");
           }
           strSql.Append(" order by a.AddTime desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion

       #region 获取聊天室总消息数
       /// <summary>
       /// 获取聊天室总消息数
       /// </summary>
       /// <param name="strRoomid"></param>
       /// <returns></returns>
       public object GetChatMsgCount(string strRoomid)
       {
           string safesql = "select count(ID) from CR_ChatMessage where RoomID="+strRoomid;
           object obj = "";
           try
           {
               obj = DbHelperSQL.GetSingle(safesql);
           }
           catch (Exception)
           {
           }
           return obj;
       }
       #endregion

       #region 获取所有图片
       /// <summary>
       /// 获取所有图片
       /// </summary>
       /// <param name="strSiteCode"></param>
       /// <param name="RoomID"></param>
       /// <returns></returns>
       public DataSet GetChatMsgImgList(string strSiteCode, string RoomID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select  ");
           strSql.Append("  a.*,b.WXConfigID WXConfigID,b.NickName NickName,b.Sex Sex,b.City City,");
           strSql.Append(" b.Province Province,b.HeadImgUrl HeadImgUrl,c.RoomName RoomName, ");
           strSql.Append(" c.PhoneNum PhoneNum,c.RoomDesc RoomDesc,c.RoomImg RoomImg  ");
           strSql.Append(" from CR_ChatMessage a,WX_User b,CR_ChatRoom c,CR_ChatUsers d  ");
           strSql.Append(" where  d.OpenID=b.OpenID and a.UserId=d.ID  and a.RoomID=c.ID and a.IsDel=0 and a.MsgState=1 and a.MsgType='image' ");
           if (strSiteCode.Trim() != null && strSiteCode.Trim() != "")
           {
               strSql.Append(" And c.SiteCode='" + strSiteCode + "' ");
           }
           if (RoomID.ToString().Trim() != null && RoomID.ToString().Trim() != "")
           {
               strSql.Append(" and a.RoomID=" + RoomID + " ");
           }
           strSql.Append(" order by a.AddTime desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
    }
}
