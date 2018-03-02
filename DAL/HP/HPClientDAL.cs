using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Model.HP;
using Maticsoft.DBUtility;

namespace DAL.HP
{
    public class HPClientDAL
    {
        public HPClientDAL() { ; }
        #region 添加打印码
        /// <summary>
        /// 添加打印码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddHPClient(HP_Client model)
        {
            string sql = @"INSERT INTO [Hp_client] ([ID],[SiteCode],[ClientCode],[IsDel])
                 VALUES (@ID,@SiteCode,@ClientCode,@IsDel)";
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@SiteCode", model.SiteCode),
                new System.Data.SqlClient.SqlParameter("@ClientCode", model.ClientCode),
                new System.Data.SqlClient.SqlParameter("@IsDel", model.IsDel)
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

        #region 更新打印码信息
        /// <summary>
        /// 更新打印码信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateHPClient(HP_Client model)
        {
            string safesql = " update Hp_client set ";
            if (model.SiteCode != null && model.SiteCode != "")
            {
                safesql += " SiteCode='" + model.SiteCode + "',";
            }
            if (model.ClientCode != null && model.ClientCode != "")
            {
                safesql += " ClientCode='" + model.ClientCode + "',";
            }
            safesql += " IsDel=" + (model.IsDel==1?1:0) + " where ID='" + model.ID + "'";
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

        #region 返回所有打印终端
        /// <summary>
        /// 返回所有打印终端
        /// </summary>
        /// <param name="strSiteCode">站点代码</param>
        /// <returns></returns>
        public DataSet GetPrintClient(string strSiteCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * FROM HP_Client WHERE SiteCode = @SiteCode AND IsDel = 0 ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode)
                };
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
            return ds;
        }
        #endregion

        #region 通过打印码与站点ID返回打印终端
        /// <summary>
        /// 通过打印码与站点ID返回打印终端
        /// </summary>
        /// <param name="strSiteCode">站点代码</param>
        /// <param name="strPrintCode">打印码</param>
        /// <returns></returns>
        public DataSet GetPrintClient(string strSiteCode, string strPrintCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * FROM HP_Client WHERE SiteCode = @SiteCode AND FreeCode = @FreeCode AND IsDel = 0 ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode),
                    new System.Data.SqlClient.SqlParameter("@FreeCode", strPrintCode)
                };
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
            return ds;
        }
        #endregion

        #region 返回所有有效的打印端编码列表
        /// <summary>
        /// 返回所有有效的打印端编码列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetPrintClientList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * FROM HP_Client WHERE IsDel = 0 ");
            if (strWhere.Trim() != null && strWhere.Trim() != "")
            {
                strSql.Append(" AND "+strWhere );
            }
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion

        #region 获取待打印的批量代码
        /// <summary>
        /// 获取待打印的批量代码
        /// </summary>
        /// <param name="strSiteCode">站点ID</param>
        /// <param name="strState">状态</param>
        /// <returns></returns>
        public DataSet GetPrintCode(string strSiteCode, string strState)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT TOP 1 PrintCode,Amount FROM HP_CodePrint WHERE SiteCode = @SiteCode AND [State] = @Status ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode),
                new System.Data.SqlClient.SqlParameter("@Status",strState),
            };
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), paras);
            DataSet dsPrintCode = null;
            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string strExtracode = ds.Tables[0].Rows[0]["PrintCode"].ToString();
                int iAmount = int.Parse(ds.Tables[0].Rows[0]["Amount"].ToString());
                strSql.Clear();
                strSql.Append(" SELECT PrintCode,Extracode FROM HP_PrintCode WHERE Extracode = @Extracode ");
                System.Data.SqlClient.SqlParameter[] paras1 = new System.Data.SqlClient.SqlParameter[]
                {
                    new System.Data.SqlClient.SqlParameter("@Extracode", strExtracode)
                };
                dsPrintCode = DbHelperSQL.Query(strSql.ToString(), paras1);
            }
            return dsPrintCode;
        }
        #endregion

        #region 修改照片与打印码状态
        /// <summary>
        /// 修改照片与打印码状态
        /// </summary>
        /// <param name="strPrintCode">打印编码</param>
        /// <param name="strState">修改状态</param>
        /// <returns></returns>
        public bool UpdatePrintCodeState(string strPrintCode, string strState)
        {
            string sql = @" UPDATE [HP_CodePrint] SET [State] = @State WHERE PrintCode = @PrintCode; ";
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@PrintCode", strPrintCode),
                new System.Data.SqlClient.SqlParameter("@State", strState)
            };
            int rowsAffected = DbHelperSQL.ExecuteSql(sql, paras);
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

        #region 判断信息是否存在
        /// <summary>
        /// 判断信息是否存在
        /// </summary>
        /// <param name="strClientCode"></param>
        /// <returns></returns>
        public bool IsExist(string strClientCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT count(ID) ");
            strSql.Append(" FROM Hp_client ");
            strSql.Append(" WHERE [ClientCode] = @ClientCode ");
            strSql.Append(" AND IsDel=0 ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ClientCode", strClientCode)
                };
            return DbHelperSQL.Exists(strSql.ToString(), paras.ToArray());
        }
        #endregion

        #region 获取信息详细
        /// <summary>
        /// 获取信息详细
        /// </summary>
        /// <param name="strID"></param>
        /// <returns></returns>
        public DataSet GetHpClientDetail(string strID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM Hp_client ");
            strSql.Append(" WHERE [ID] = @ID ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", strID)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion

        #region 获取信息属性
        /// <summary>
        /// 获取信息属性
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="strID"></param>
        /// <returns></returns>
        public object GetHpClientValue(string strValue, string strID)
        {
            string safesql = string.Empty; ;
            safesql = "select " + strValue + " from Hp_client where ID='" + strID + "' ";
            return DbHelperSQL.GetSingle(safesql.ToString());
        }
        #endregion 

        #region 更新信息状态
        /// <summary>
        /// 更新信息状态
        /// </summary>
        /// <param name="strID"></param>
        /// <returns></returns>
        public bool UpHpClientState(string strID)
        {
            StringBuilder strSql = new StringBuilder();
            int state = 0;
            try
            {
                state = Convert.ToInt32(GetHpClientValue("IsDel", strID));
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
            strSql.Append(" UPDATE Hp_client ");
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
    }
}
