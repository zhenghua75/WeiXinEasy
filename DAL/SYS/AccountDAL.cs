using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maticsoft.DBUtility;

namespace DAL.SYS
{
    public class AccountDAL
    {
        public AccountDAL() { ; }

        #region 返回用户登录信息
        /// <summary>
        /// 返回用户登录信息
        /// </summary>
        /// <param name="strLoginName">登录名</param>
        /// <param name="strPassword">登录口令</param>
        /// 
        public DataSet GetAccountData(string strLoginName,string strPassword)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT a.ID,a.LoginName,a.[Password],a.AgentID,a.[Name],a.RoleID,a.Email,");
            strSql.Append(" a.[Address],a.Mobile,a.Telphone,a.[Status],a.CreateTime,a.SiteCode ");
            strSql.Append(" ,b.SiteCategory ");
            strSql.Append(" FROM SYS_Account a,SYS_Account_Ext b ");
            strSql.Append(" WHERE a.LoginName = @LoginName ");
            strSql.Append(" AND a.[Password] = @Password ");
            strSql.Append(" AND a.ID=b.AccountID ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@LoginName", strLoginName),
                    new System.Data.SqlClient.SqlParameter("@Password", strPassword)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion

        #region 返回用户扩展信息
        /// <summary>
        /// 返回用户扩展信息
        /// </summary>
        /// <param name="strAccountID">账户ID</param>
        /// 
        public DataSet GetAccountExtData(string strAccountID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT a.AccountID AS ID ,a.Photo,a.Summary,a.Remark,a.Themes,b.Name,b.SiteCode ");
            strSql.Append(" FROM SYS_Account_Ext a ");
            strSql.Append(" LEFT JOIN SYS_Account b ON (b.ID = a.AccountID) ");
            strSql.Append(" WHERE a.AccountID = @AccountID ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@AccountID", strAccountID)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion


        #region 通过站点代码返回用户扩展信息
        /// <summary>
        /// 通过站点代码返回用户扩展信息
        /// </summary>
        /// <param name="strSiteCode">站点代码</param>
        /// 
        public DataSet GetAExtDataBySiteCode(string strSiteCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT a.AccountID AS ID ,a.Photo,a.Summary,a.Remark,a.Themes,b.Name,b.SiteCode ");
            strSql.Append(" FROM SYS_Account_Ext a ");
            strSql.Append(" LEFT JOIN SYS_Account b ON (b.ID = a.AccountID) ");
            strSql.Append(" WHERE b.SiteCode = @SiteCode ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion

        #region 返回站点信息
        /// <summary>
        /// 返回用户登录信息
        /// </summary>
        /// <param name="strSiteCode">登录名</param>
        /// 
        public DataSet GetSiteData(string strSiteCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT ID,LoginName,[Password],AgentID,[Name],RoleID,Email,[Address],Mobile,Telphone,[Status],CreateTime,SiteCode  ");
            strSql.Append(" FROM SYS_Account ");
            strSql.Append(" WHERE SiteCode = @SiteCode ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion

        #region 根据ID返回用户登录信息
        /// <summary>
        /// 根据ID返回用户登录信息
        /// </summary>
        /// <param name="strID">账户ID号</param>
        /// 
        public DataSet GetAccountByID(string strID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM SYS_Account ");
            strSql.Append(" WHERE REPLACE(ID,'-','') = @ID ");//把数据中的特殊字符替换
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", strID)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion

        #region 返回系统账户信息
        /// <summary>
        /// 返回系统账户信息列表
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// 
        public DataSet GetAccountList(string strWhere)
        {            
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT a.ID,a.LoginName,ISNULL(b.Name,'') AS AgentID,a.[Name],c.Name AS RoleID,a.Email, ");
            strSql.Append(" a.[Address],a.Mobile,a.Telphone, ");
            strSql.Append(" d.Remark AS [Status], ");
            strSql.Append(" CONVERT(varchar(100), a.CreateTime, 20) AS CreateTime,a.SiteCode ");
            strSql.Append(" FROM SYS_Account a ");
            strSql.Append(" LEFT JOIN SYS_Account b ON (b.ID = a.AgentID) ");
            strSql.Append(" LEFT JOIN SYS_Role c ON (c.No = a.RoleID) ");
            strSql.Append(" LEFT JOIN SYS_Dictionary d ON (a.Status = d.ID) ");
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql.Append(" WHERE " + strWhere );
            }
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion

        #region 返回系统账户列表
        /// <summary>
        /// 返回系统账户信息列表
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// 
        public DataSet GetAllAccount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM SYS_Account ");
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion

        #region 添加账户信息
        /// <summary>
        /// 添加账户信息
        /// </summary>
        /// <param name="model">账户信息</param>
        /// 
        public bool AddAccountData(Model.SYS.SYS_Account model)
		{
			StringBuilder strSql=new StringBuilder();
			StringBuilder strSql1=new StringBuilder();
			StringBuilder strSql2=new StringBuilder();
			if (model.ID != null)
			{
				strSql1.Append("ID,");
                strSql2.Append("'" + model.ID + "',");
			}
			if (model.LoginName != null)
			{
                strSql1.Append("LoginName,");
                strSql2.Append("'" + model.LoginName + "',");
			}
            if (model.Password != null)
			{
                strSql1.Append("Password,");
                strSql2.Append("'" + model.Password + "',");
			}
            if (model.AgentID != null)
			{
                strSql1.Append("AgentID,");
                strSql2.Append("'" + model.AgentID + "',");
			}
            if (model.Name != null)
			{
                strSql1.Append("Name,");
                strSql2.Append("'" + model.Name + "',");
			}
            if (model.RoleID != null)
			{
                strSql1.Append("RoleID,");
                strSql2.Append("'" + model.RoleID + "',");
			}
            if (model.Email != null)
			{
                strSql1.Append("Email,");
                strSql2.Append("'" + model.Email + "',");
			}
            if (model.Address != null)
			{
                strSql1.Append("Address,");
                strSql2.Append("'" + model.Address + "',");
			}
            if (model.Mobile != null)
			{
                strSql1.Append("Mobile,");
                strSql2.Append("'" + model.Mobile + "',");
			}
            if (model.Telphone != null)
			{
                strSql1.Append("Telphone,");
                strSql2.Append("'" + model.Telphone + "',");
			}
            if (model.Status != null)
			{
                strSql1.Append("Status,");
                strSql2.Append("'" + model.Status + "',");
			}
            if (model.CreateTime != null)
			{
                strSql1.Append("CreateTime,");
                strSql2.Append("'" + model.CreateTime + "',");
			}
            if (model.SiteCode != null)
			{
                strSql1.Append("SiteCode,");
                strSql2.Append("'" + model.SiteCode + "',");
			}
            strSql.Append("INSERT INTO SYS_Account(");
			strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
			strSql.Append(")");
			strSql.Append(" VALUES (");
			strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
			strSql.Append(");");

            strSql.Append(" UPDATE SYS_Account SET ID = '" + model.ID + "'");
            strSql.Append(" WHERE ID ='" + model.ID + "' ");
            int rowsAffected = DbHelperSQL.ExecuteSql(strSql.ToString());
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

        #region 更新账户信息
        /// <summary>
        /// 更新账户信息
        /// <param name="model">账户信息</param>
        /// </summary>
        public bool UpdateAccountData(Model.SYS.SYS_Account model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE SYS_Account SET ");           
            if (model.Name != null)
            {
                strSql.Append("Name='" + model.Name + "',");
            }
            if (model.RoleID != null)
            {
                strSql.Append("RoleID='" + model.RoleID + "',");
            }
            if (model.Email != null)
            {
                strSql.Append("Email='" + model.Email + "',");
            }
            if (model.Address != null)
            {
                strSql.Append("Address='" + model.Address + "',");
            }
            if (model.Mobile != null)
            {
                strSql.Append("Mobile='" + model.Mobile + "',");
            }
            if (model.Telphone != null)
            {
                strSql.Append("Telphone='" + model.Telphone + "',");
            }
            if (model.Status != null)
            {
                strSql.Append("Status='" + model.Status + "',");
            }
            if (model.SiteCode != null)
            {
                strSql.Append("SiteCode='" + model.SiteCode + "',");
            }
            int n = strSql.ToString().LastIndexOf(",");
            strSql.Remove(n, 1);
            strSql.Append(" WHERE ID ='" + model.ID + "' ");
            int rowsAffected = DbHelperSQL.ExecuteSql(strSql.ToString());
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

        #region 修改账户口令
        /// <summary>
        /// 修改账户口令
        /// <param name="strAccountID">账户ID</param>
        /// <param name="strPD">账户口令</param>
        /// </summary>
        public bool UpdateAccountPD(string strAccountID ,string strPD)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE SYS_Account SET ");
            strSql.Append(" [Password] = @PD ");
            strSql.Append(" WHERE ID = @AccountID ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@AccountID", strAccountID),
                new System.Data.SqlClient.SqlParameter("@PD", strPD)
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
