using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.CR;
using Maticsoft.DBUtility;

namespace DAL.CR
{
   public class ChatRoomDAL
    {
       public ChatRoomDAL() { ;}

       #region 添加聊天室
       /// <summary>
       /// 添加聊天室
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddChatRoom(ChatRoom model)
       {
           string sql = @"INSERT INTO [CR_ChatRoom]
                        ([ID],[SiteCode],[RoomName],[PhoneNum],[RoomDesc],[RoomImg],[AppID],[WebCodeImg],[AddTime],[IsDel])
                 VALUES
                        (@ID,@SiteCode,@RoomName,@PhoneNum,@RoomDesc,@RoomImg,@AppID,@WebCodeImg,@AddTime,@IsDel)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@SiteCode", model.SiteCode),
                new System.Data.SqlClient.SqlParameter("@RoomName", model.RoomName),
                new System.Data.SqlClient.SqlParameter("@PhoneNum", model.PhoneNum),
                new System.Data.SqlClient.SqlParameter("@RoomDesc", model.RoomDesc),
                new System.Data.SqlClient.SqlParameter("@RoomImg", model.RoomImg),
                new System.Data.SqlClient.SqlParameter("@AppID", model.AppID),
                new System.Data.SqlClient.SqlParameter("@WebCodeImg", model.WebCodeImg),
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

       #region 更新聊天室信息
       /// <summary>
       /// 更新聊天室信息
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateChatRoom(ChatRoom model)
       {
           string safesql = "";
           safesql = " update CR_ChatRoom set ";
           if (model.RoomName != null && model.RoomName != "")
           {
               safesql += "[RoomName]='" + model.RoomName + "',";
           }
           if (model.PhoneNum.ToString() != null && model.PhoneNum.ToString() != "")
           {
               safesql += "[PhoneNum]='" + model.PhoneNum + "',";
           }
           if (model.RoomDesc != null && model.RoomDesc != "")
           {
               safesql += "[RoomDesc]='" + model.RoomDesc + "',";
           }
           if (model.RoomImg != null && model.RoomImg != "")
           {
               safesql += "[RoomImg]='" + model.RoomImg + "',";
           }
           if (model.AppID != null && model.AppID != "")
           {
               safesql += "[AppID]='" + model.AppID + "',";
           }
           if (model.WebCodeImg != null && model.WebCodeImg != "")
           {
               safesql += "[WebCodeImg]='" + model.WebCodeImg + "',";
           }
           safesql += "[IsDel]=" + (model.IsDel==1?1:0);
           safesql += " where id=" + model.ID + " AND SiteCode='" + model.SiteCode + "'";
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

       #region 获取房间最大编号
       /// <summary>
       /// 获取房间最大编号
       /// </summary>
       /// <param name="strSiteCode"></param>
       /// <returns></returns>
       public object GetMaxRoomNum()
       {
           string safesql = "select max(id) from CR_ChatRoom";
           object obj = 0;
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

       #region 获取聊天室属性
       /// <summary>
       /// 获取聊天室属性
       /// </summary>
       /// <param name="value"></param>
       /// <param name="ChatRoomID"></param>
       /// <returns></returns>
       public object GetChatRoomValueByID(string value, int ChatRoomID)
       {
           string safesql = "";
           safesql = "select " + value + " from CR_ChatRoom where ID=" + ChatRoomID;
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion

       #region 更新聊天室状态
       /// <summary>
       /// 更新聊天室状态
       /// </summary>
       /// <param name="ChatRoomID"></param>
       /// <returns></returns>
       public bool UpdateChatRoomIsDel(string ChatRoomID)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetChatRoomValueByID("IsDel", Convert.ToInt32(ChatRoomID)));
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
           strSql.Append(" UPDATE CR_ChatRoom ");
           strSql.Append(" SET IsDel = @IsDel ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", ChatRoomID),
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

       #region 获取有效的聊天室列表
       /// <summary>
       /// 获取有效的聊天室列表
       /// </summary>
       /// <param name="where"></param>
       /// <returns></returns>
       public DataSet GetChatRoomList(string where)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * from CR_ChatRoom WHERE IsDel=0 ");
           if (where.Trim() != null && where.Trim()!="")
           {
               strSql.Append("  " + where);
           }
           strSql.Append(" order by AddTime asc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion

       #region 获取聊天室详细
       /// <summary>
       /// 获取聊天室详细
       /// </summary>
       /// <param name="ChatRoomID"></param>
       /// <returns></returns>
       public DataSet GetChatRoomDetail(int ChatRoomID,string strSiteCode)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM CR_ChatRoom ");
           strSql.Append(" WHERE [ID] = @ID and SiteCode=@SiteCode ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", ChatRoomID),
                    new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode)
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion

       #region 信息是否存在
       /// <summary>
       /// 信息是否存在
       /// </summary>
       /// <param name="strSiteCode"></param>
       /// <param name="strRoomName"></param>
       /// <returns></returns>
       public bool ExistChatRoom(string strSiteCode, string strRoomName)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT count(ID) ");
           strSql.Append(" FROM CR_ChatRoom ");
           strSql.Append(" WHERE [SiteCode] = @SiteCode ");
           strSql.Append(" AND [RoomName] = @RoomName ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode),
                    new System.Data.SqlClient.SqlParameter("@RoomName", strRoomName)
                };
           return DbHelperSQL.Exists(strSql.ToString(), paras.ToArray());
       }
        #endregion

       #region 根据公号判断房间是否存在
       /// <summary>
       /// 根据公号判断房间是否存在
       /// </summary>
       /// <param name="strAppID"></param>
       /// <returns></returns>
       public bool ExsitRoomNumByAppID(string strAppID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT count(ID) ");
           strSql.Append(" FROM CR_ChatRoom ");
           strSql.Append(" WHERE [AppID] = @AppID ");
           strSql.Append(" AND [IsDel] =0 ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@AppID", strAppID)
                };
           return DbHelperSQL.Exists(strSql.ToString(), paras.ToArray());
       }
       #endregion

       #region 返回1条最新的房间号
       /// <summary>
       /// 返回1条最新的房间号
       /// </summary>
       /// <param name="strAppID"></param>
       /// <returns></returns>
       public object top1RoomNum(string strAppID)
       {
           string safesql = "";
           safesql = "select top 1 ID from CR_ChatRoom where AppID='" + strAppID + "' order by AddTime desc";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion

       #region 根据公号获取属性
       /// <summary>
       /// 根据公号获取属性
       /// </summary>
       /// <param name="value"></param>
       /// <param name="strAppID"></param>
       /// <returns></returns>
       public object GetRoomInfoByAppID(string value,string strAppID)
       {
           string safesql = "";
           safesql = "select top 1 " + value + " from CR_ChatRoom where AppID='" + strAppID + "' order by AddTime desc ";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion

       #region 判断房间号是否存在
       /// <summary>
       /// 判断房间号是否存在
       /// </summary>
       /// <param name="strSiteCode"></param>
       /// <param name="strRoomNum"></param>
       /// <returns></returns>
       public bool ExistChatRoomNum(string strSiteCode, int strRoomNum)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT count(ID) ");
           strSql.Append(" FROM CR_ChatRoom ");
           strSql.Append(" WHERE [SiteCode] = @SiteCode ");
           strSql.Append(" AND [ID] = @RoomNum ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode),
                    new System.Data.SqlClient.SqlParameter("@RoomNum", strRoomNum)
                };
           return DbHelperSQL.Exists(strSql.ToString(), paras.ToArray());
       }
       #endregion

       #region 获取一个聊天室房间号
       /// <summary>
       /// 获取一个聊天室
       /// </summary>
       /// <param name="strSiteCode"></param>
       /// <returns></returns>
       public object GetChatRoomNum(string strSiteCode)
       {
           string safesql = "";
           safesql = "select TOP 1 ID from CR_ChatRoom  WHERE IsDel=0  AND SiteCode='" + strSiteCode + "' ";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion
    }
}
