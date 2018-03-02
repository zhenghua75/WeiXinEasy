using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.Game;
using Maticsoft.DBUtility;

namespace DAL.Game
{
   public class LuckyAwardUsersDAL
    {
       public LuckyAwardUsersDAL() { ;}
       #region 添加获奖用户信息
       /// <summary>
       /// 添加获奖用户
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddAwardUsers(LuckyAwardUsers model)
       {
           string sql = @"INSERT INTO [Game_LuckyAwardUsers]
                        (ID,ActID,AID,OpenID,NickName,Phone,SNCode,SendAward,AddTime,IsDel)
                 VALUES
                        (@ID,@ActID,@AID,@OpenID,@NickName,@Phone,@SNCode,@SendAward,@AddTime,@IsDel)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@ActID", model.ActID),
                new System.Data.SqlClient.SqlParameter("@AID", model.AID),
                new System.Data.SqlClient.SqlParameter("@OpenID", model.OpenID),
                new System.Data.SqlClient.SqlParameter("@NickName", model.NickName),
                new System.Data.SqlClient.SqlParameter("@Phone", model.Phone),
                new System.Data.SqlClient.SqlParameter("@SNCode", model.SNCode),
                new System.Data.SqlClient.SqlParameter("@SendAward", model.SendAward),
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
       #region 更新获奖用户信息
       /// <summary>
       /// 更新获奖用户信息
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateAwardUsers(LuckyAwardUsers model)
       {
           string safesql = "";
           safesql = " update Game_LuckyAwardUsers set ";
           if (model.ActID.ToString() != null && model.ActID.ToString() != "")
           {
               safesql += "[ActID]='" + model.ActID + "',";
           }
           if (model.AID.ToString() != null && model.AID.ToString() != "")
           {
               safesql += "[AID]='" + model.AID + "',";
           }
           if (model.OpenID != null && model.OpenID != "")
           {
               safesql += "[OpenID]='" + model.OpenID + "',";
           }
           if (model.NickName != null && model.NickName != "")
           {
               safesql += "[NickName]='" + model.NickName + "',";
           }
           if (model.Phone != null && model.Phone != "")
           {
               safesql += "[Phone]='" + model.Phone + "',";
           }
           if (model.SNCode != null && model.SNCode != "")
           {
               safesql += "[SNCode]='" + model.SNCode + "',";
           }
           safesql += "[SendAward]=" + (model.SendAward == 1 ? 1 : 0) + ",";
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
       #region 获取用户属性
       /// <summary>
       /// 获取用户属性
       /// </summary>
       /// <param name="strValue"></param>
       /// <param name="strID"></param>
       /// <returns></returns>
       public object GetAwardUserValueByID(string strValue, string strID)
       {
           string safesql = "";
           safesql = "select " + strValue + " from Game_LuckyAwardUsers where ID='" + strID + "'";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion
       #region 更新用户状态
       /// <summary>
       /// 更新用户状态
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public bool UpdateAwardUserState(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetAwardUserValueByID("IsDel", strID));
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
           strSql.Append(" UPDATE Game_LuckyAwardUsers ");
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
       #region 获取有效的用户列表
       /// <summary>
       /// 获取有效的用户列表
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetAwardUserList(string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT a.*,b.Award as Award,c.ActTitle as ActTitle"+
               " from Game_LuckyAwardUsers a,Game_LuckyAward b,ACT_SiteActivity c ");
           strSql.Append(" WHERE a.IsDel=0 and b.IsDel=0 and a.AID=b.ID AND a.ActID=c.ID ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by a.AddTime asc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 获取用户详细
       /// <summary>
       /// 获取用户详细
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public DataSet GetAwardUserDetail(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT a.*,b.Award as Award from Game_LuckyAwardUsers a,Game_LuckyAward b ");
           strSql.Append(" FROM Game_LuckyAward ");
           strSql.Append(" WHERE a.[ID] = @ID ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", strID)
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion
       #region 根据OpenID、昵称、电话、奖项ID、活动编号 判断用户是否存在
       /// <summary>
       /// 根据OpenID、昵称、电话、奖项ID、活动编号 判断用户是否存在
       /// </summary>
       /// <param name="strOpenID"></param>
       /// <param name="strNickName"></param>
       /// <param name="strAID"></param>
       /// <returns></returns>
       public bool ExistAwardUser(string strOpenID, string strNickName,string Phone, string strAID,string strActID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT count(ID) ");
           strSql.Append(" FROM Game_LuckyAwardUsers WHERE IsDel=0 ");
           if (strOpenID.Trim() != null && strOpenID.Trim() != "")
           {
               strSql.Append(" AND [OpenID] = '" + strOpenID+ "' ");
           }
           if (strNickName.Trim() != null && strNickName.Trim() != "")
           {
               strSql.Append(" AND [NickName] = '" + strNickName + "' ");
           }
           if (Phone.Trim() != null && Phone.Trim() != "")
           {
               strSql.Append(" AND [Phone] = '" + Phone + "' ");
           }
           if (strAID.Trim() != null && strAID.Trim() != "")
           {
               strSql.Append(" AND [AID] = '" + strAID + "' ");
           }
           if (strActID.Trim() != null && strActID.Trim() != "")
           {
               strSql.Append(" AND [ActID] = '" + strActID + "' ");
           }
           return DbHelperSQL.Exists(strSql.ToString());
       }
       #endregion
       #region 根据OpenID、昵称、电话、奖项ID、活动编号 获取SN码
       /// <summary>
       /// 根据OpenID、昵称、电话、奖项ID、活动编号 获取SN码
       /// </summary>
       /// <param name="strOpenID"></param>
       /// <param name="strPhone"></param>
       /// <param name="strAID"></param>
       /// /// <param name="strActID"></param>
       /// <returns></returns>
       public string GetSNCodeByPhone(string strOpenID, string strPhone, string strAID, string strActID)
       {
           string safesql = string.Empty;
           safesql = "select SNCode from Game_LuckyAwardUsers  ";
           if (strOpenID.Trim() != null && strOpenID.Trim() != "")
           {
               if (safesql.ToString().ToLower().Trim().Contains("where"))
               {
                   safesql += " and ";
               }
               else
               {
                   safesql += " where ";
               }
               safesql += " OpenID='"+strOpenID+"' ";
           }
           if (strPhone.Trim() != null && strPhone.Trim() != "")
           {
               if (safesql.ToString().ToLower().Trim().Contains("where"))
               {
                   safesql += " and ";
               }
               else
               {
                   safesql += " where ";
               }
               safesql += " Phone='" + strPhone + "' ";
           }
           if (strAID.Trim() != null && strAID.Trim() != "")
           {
               if (safesql.ToString().ToLower().Trim().Contains("where"))
               {
                   safesql += " and ";
               }
               else
               {
                   safesql += " where ";
               }
               safesql += " AID='" + strAID + "' ";
           }
           if (strActID.Trim() != null && strActID.Trim() != "")
           {
               if (safesql.ToString().ToLower().Trim().Contains("where"))
               {
                   safesql += " and ";
               }
               else
               {
                   safesql += " where ";
               }
               safesql += " ActID='" + strActID + "' ";
           }
           try
           {
               safesql=DbHelperSQL.GetSingle(safesql.ToString()).ToString();
           }
           catch (Exception)
           {
               safesql="";
           }
           return safesql;
       }
       #endregion 
       #region 根据OpenID、活动编号、奖项编号 更新用户兑奖SN码
       /// <summary>
       /// 根据OpenID、活动编号、奖项编号 更新用户兑奖SN码
       /// </summary>
       /// <param name="strOpenID"></param>
       /// <param name="strActID"></param>
       /// <param name="strSNCode"></param>
       /// <returns></returns>
       public bool UpdateUserSNCode(string strOpenID,string strAID, string strActID,string strSNCode)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" UPDATE Game_LuckyAwardUsers ");
           strSql.Append(" SET SNCode= @SNCode,AID=@aid ");
           strSql.Append(" WHERE OpenID = @OpenID and ActID=@ActID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@SNCode", strSNCode),
                new System.Data.SqlClient.SqlParameter("@AID", strAID),
                new System.Data.SqlClient.SqlParameter("@OpenID", strOpenID),
                new System.Data.SqlClient.SqlParameter("@ActID", strActID)
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
