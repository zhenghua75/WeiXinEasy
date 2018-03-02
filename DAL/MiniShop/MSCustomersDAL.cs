using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.MiniShop;
using Maticsoft.DBUtility;

namespace DAL.MiniShop
{
    public class MSCustomersDAL
    {
        public MSCustomersDAL() { ;}
        #region 客户信息添加
        /// <summary>
        /// 客户信息添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddCustomers(MSCustomers model)
        {
            string sql = @"INSERT INTO [MS_Customers]
                        ([ID],[OpenID],[NickName],[RealName],[IDnum],[IDimg],[Phone],[UserPwd],[Email],[QQnum],[HeadImg],[Sex],[Pnote],[IsDel],[AddTime])
                 VALUES
                        (@ID,@OpenID,@NickName,@RealName,@IDnum,@IDimg,@Phone,@UserPwd,@Email,@QQnum,@HeadImg,@Sex,@Pnote,@IsDel,@AddTime)";
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@OpenID", model.OpenID),
                new System.Data.SqlClient.SqlParameter("@NickName", model.NickName),
                new System.Data.SqlClient.SqlParameter("@RealName", model.RealName),
                new System.Data.SqlClient.SqlParameter("@IDnum", model.IDnum),
                new System.Data.SqlClient.SqlParameter("@IDimg", model.IDimg),
                new System.Data.SqlClient.SqlParameter("@Phone", model.Phone),
                new System.Data.SqlClient.SqlParameter("@UserPwd", model.UserPwd),
                new System.Data.SqlClient.SqlParameter("@Email", model.Email),
                new System.Data.SqlClient.SqlParameter("@QQnum", model.QQnum),
                new System.Data.SqlClient.SqlParameter("@HeadImg", model.HeadImg),
                new System.Data.SqlClient.SqlParameter("@Sex",(model.Sex==1?1:0)),
                new System.Data.SqlClient.SqlParameter("@Pnote",model.Pnote),
                new System.Data.SqlClient.SqlParameter("@IsDel",(model.IsDel==1?1:0)),
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
        #region 客户信息更改
        /// <summary>
        /// 客户信息更改   
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateCustomers(MSCustomers model)
        {
            string safeslq = "";
            safeslq = "UPDATE MS_Customers SET ";
            if (model.NickName != null && model.NickName != "")
            {
                safeslq += "NickName='" + model.NickName + "',";
            }
            if (model.RealName != null && model.RealName != "")
            {
                safeslq += "RealName='" + model.RealName + "',";
            }
            if (model.IDnum != null && model.IDnum != "")
            {
                safeslq += "IDnum='" + model.IDnum + "',";
            }
            if (model.IDimg != null && model.IDimg != "")
            {
                safeslq += "IDimg='" + model.IDimg + "',";
            }
            if (model.Phone != null && model.Phone != "")
            {
                safeslq += "Phone='" + model.Phone + "',";
            }
            if (model.UserPwd != null && model.UserPwd.ToString() != "")
            {
                safeslq += "UserPwd='" + model.UserPwd + "',";
            }
            if (model.Email != null && model.Email.ToString() != "")
            {
                safeslq += "Email='" + model.Email + "',";
            }
            if (model.QQnum != null && model.QQnum.ToString() != "")
            {
                safeslq += "QQnum='" + model.QQnum + "',";
            }
            if (model.HeadImg != null && model.HeadImg != "")
            {
                safeslq += "HeadImg='" + model.HeadImg + "',";
            }
            if (model.Pnote != null && model.Pnote != "")
            {
                safeslq += "Pnote='" + model.Pnote + "',";
            }
            safeslq += " Sex=" + (model.Sex == 1 ? 1 : 0) + ", ";
            safeslq += " IsDel=" + (model.IsDel == 1 ? 1 : 0) + " ";
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
        #region 根据客户编号获取相应的值
        /// <summary>
        /// 根据客户编号获取相应的值
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="strID"></param>
        /// <returns></returns>
        public object GetCustomerValueByID(string strValue, string strID)
        {
            string safesql = "";
            safesql = "select " + strValue + " from MS_Customers where ID='" + strID + "'";
            return DbHelperSQL.GetSingle(safesql.ToString());
        }
        #endregion
        #region 获取用户属性 (通过用户openid)
        /// <summary>
        /// 获取用户属性 (通过用户openid)
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="strOpenID"></param>
        /// <returns></returns>
        public object GetCustomerValueByOpenID(string strValue, string strOpenID)
        {
            string safesql = "";
            safesql = "select top 1 " + strValue + " from MS_Customers where OpenID='" + strOpenID + "'";
            return DbHelperSQL.GetSingle(safesql.ToString());
        }
        #endregion
        #region 更新客户状态
        /// <summary>
        /// 更新客户状态
        /// </summary>
        /// <param name="strID"></param>
        /// <returns></returns>
        public bool UpdateCustomerState(string strID)
        {
            StringBuilder strSql = new StringBuilder();
            int state = 0;
            try
            {
                state = Convert.ToInt32(GetCustomerValueByID("ParState", strID));
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
            strSql.Append(" UPDATE MS_Customers ");
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
        #region 更新用户身份信息
        /// <summary>
        /// 更新用户身份信息
        /// </summary>
        /// <param name="strUID">用户编号</param>
        /// <param name="strRealName">真实姓名</param>
        /// <param name="strIDnum">身份证号码</param>
        /// <param name="strIDimg">身份证照片</param>
        /// <returns></returns>
        public bool UpdateUserIDnum(string strUID,string strRealName,string strIDnum,string strIDimg)
        {
            string safeslq = "";
            safeslq = "UPDATE MS_Customers SET ";
            if (strRealName != null && strRealName != "")
            {
                safeslq += "RealName ='" + strRealName + "',";
            }
            if (strIDnum != null && strIDnum != "")
            {
                safeslq += "IDnum='" + strIDnum + "',";
            }
            if (strIDimg != null && strIDimg != "")
            {
                safeslq += "IDimg='" + strIDimg + "',";
            }
            safeslq += " IsDel=0 where ID='" + strUID + "'";
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
        #region 更新用户的OpenID
        /// <summary>
        /// 更新用户的OpenID
        /// </summary>
        /// <param name="strUID">用户编号</param>
        /// <param name="strOpenID">OpenID</param>
        /// <returns></returns>
        public bool UpdateUserOpenID(string strUID,string strOpenID)
        {
            string safeslq = "";
            safeslq = "UPDATE MS_Customers SET ";
            if (strOpenID != null && strOpenID != "")
            {
                safeslq += "OpenID ='" + strOpenID + "' ";
            }
            safeslq += "  where ID='" + strUID + "'";
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
        #region 获取有效的客户列表
        /// <summary>
        /// 获取有效的客户列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetCustomersList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select ID,NickName,Phone,UserPwd,Email,QQnum,HeadImg,Pnote,IsDel,AddTime, ");
            strSql.Append(" case Sex when 0 then '男' when 1 then '女' end Sex ");
            strSql.Append(" from MS_Customers ");
            strSql.Append(" where IsDel=0 ");
            if (strWhere.Trim() != null && strWhere.Trim() != "")
            {
                strSql.Append("  " + strWhere);
            }
            strSql.Append(" order by AddTime desc ");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion
        #region 获取客户信息详细
        /// <summary>
        /// 获取客户信息详细
        /// </summary>
        /// <param name="strID"></param>
        /// <returns></returns>
        public DataSet GetCustomerDetail(string strID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM MS_Customers ");
            strSql.Append(" WHERE [ID] = @ID");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", strID)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion
        #region 判断用户是否存在(电子邮件)
        /// <summary>
        /// 判断用户是否存在(电子邮件)
        /// </summary>
        ///  <param name="strOpenID">用户OpenID</param>
        /// <param name="strLoginName">账户</param>
        /// <param name="strPhone">电话</param>
        /// <param name="strEmail">Email</param>
        /// <param name="strQQnum">QQ</param>
        /// <returns></returns>
        public bool ExistCustomer(string strOpenID,string strPhone, string strEmail, string strQQnum)
        {
            string strSql = string.Empty;
            strSql += " SELECT count(ID) FROM MS_Customers where isdel=0 ";
            if (strOpenID.Trim() != null && strOpenID.Trim() != "")
            {
                strSql += " and [OpenID] ='" + strOpenID + "' ";
            }
            if (strPhone.Trim() != null && strPhone.Trim() != "")
            {
                strSql += " and [Phone] ='" + strPhone + "' ";
            }
            if (strEmail.Trim() != null && strEmail.Trim() != "")
            {
                strSql += " and [Email] ='" + strEmail + "' ";
            }
            if (strQQnum.Trim() != null && strQQnum.Trim() != "")
            {
                strSql += " and [QQnum] ='" + strQQnum + "' ";
            }
            return DbHelperSQL.Exists(strSql.ToString());
        }
        #endregion
        #region 验证用户是否存在(身份证)
        /// <summary>
        /// 验证用户是否存在(身份证)
        /// </summary>
        /// <param name="strPhone">电话</param>
        /// <param name="strIDnum">身份证号</param>
        /// <param name="strRealName">真实姓名</param>
        /// <returns></returns>
        public bool ExistCustomerByIDnum(string strPhone, string strIDnum, string strRealName)
        {
            string strSql = string.Empty;
            strSql += " SELECT count(ID) FROM MS_Customers where isdel=0 ";
            if (strPhone.Trim() != null && strPhone.Trim() != "")
            {
                strSql += " and [Phone] ='" + strPhone + "' ";
            }
            if (strIDnum.Trim() != null && strIDnum.Trim() != "")
            {
                strSql += " and [IDnum] ='" + strIDnum + "' ";
            }
            if (strRealName.Trim() != null && strRealName.Trim() != "")
            {
                strSql += " and [RealName] ='" + strRealName + "' ";
            }
            return DbHelperSQL.Exists(strSql.ToString());
        }
        #endregion
        #region 用户登录(获取店铺信息)
        /// <summary>
        /// 用户登录(获取店铺信息)
        /// </summary>
        /// <param name="strLogPhone">电话</param>
        /// <param name="strLoginPwd">密码</param>
        /// <returns></returns>
        public DataSet UserLogin(string strLogPhone, string strLoginPwd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select a.*,b.ID [SID] from MS_Customers a,MS_Shop b ");
            strSql.Append(" where a.ID=b.[UID] and a.IsDel=0 ");
            strSql.Append(" AND a.Phone = @Phone ");
            strSql.Append(" AND a.[UserPwd] = @UserPwd ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@Phone", strLogPhone),
                new System.Data.SqlClient.SqlParameter("@UserPwd", strLoginPwd)
            };
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), paras);
            return ds;
        }
        #endregion
        #region 用户登录(获取客户信息)
        /// <summary>
        /// 用户登录(获取客户信息)
        /// </summary>
        /// <param name="strLogPhone"></param>
        /// <param name="strLoginPwd"></param>
        /// <returns></returns>
        public DataSet CustomerLogin(string strOpenID,string strLogPhone, string strLoginPwd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select * from MS_Customers ");
            strSql.Append(" where IsDel=0 ");
            if (strOpenID != null && strOpenID != "")
            {
                strSql.Append(" AND [OpenID] ='" + strOpenID + "' ");
            }
            if (strLogPhone != null && strLogPhone != "")
            {
                strSql.Append(" AND [Phone] ='" + strLogPhone + "' ");
            }
            if (strLoginPwd != null && strLoginPwd != "")
            {
                strSql.Append(" AND [UserPwd] ='" + strLoginPwd + "' ");
            }
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion
        #region 验证用户电话和密码
        /// <summary>
        /// 验证用户电话和密码
        /// </summary>
        /// <param name="strPhone"></param>
        /// <param name="strPwd"></param>
        /// <returns></returns>
        public bool ExistPhoneAndPwd(string strPhone, string strPwd)
        {
            string strSql = string.Empty;
            strSql += " SELECT count(ID) FROM MS_Customers where isdel=0 ";
            if (strPhone.Trim() != null && strPhone.Trim() != "")
            {
                strSql += " and [Phone] ='" + strPhone + "' ";
            }
            if (strPwd.Trim() != null && strPwd.Trim() != "")
            {
                strSql += " and [UserPwd] ='" + strPwd + "' ";
            }
            return DbHelperSQL.Exists(strSql.ToString());
        }
        #endregion
        #region 修改密码通过电话
        /// <summary>
        /// 修改密码通过电话
        /// </summary>
        /// <param name="strPhone"></param>
        /// <param name="strPwd"></param>
        /// <returns></returns>
        public bool UpdatePwdByPhone(string strPhone, string strPwd)
        {
            string safeslq = "";
            safeslq = "UPDATE MS_Customers SET ";
            if (strPwd != null && strPwd != "")
            {
                safeslq += "UserPwd='" + strPwd + "'";
            }
            safeslq += " where Phone='" + strPhone + "'";
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
    }
}
