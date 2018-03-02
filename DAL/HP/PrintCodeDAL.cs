using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maticsoft.DBUtility;
using DAL.SYS;
using Model.SYS;

namespace DAL.HP
{
    public class PrintCodeDAL
    {
        public PrintCodeDAL() { ; }

        #region 生成并返回打印码
        /// <summary>
        /// 生成并返回打印码
        /// </summary>
        /// <param name="iAmount">指定的数字串个数</param>
        /// <param name="strSiteCode">站点代码</param>
        /// <param name="strClientCode">指定的终端代码</param>
        /// <param name="strStart">有效开始时间</param>
        /// <param name="strEnd">有效的结束时间</param>
        /// <returns></returns>
        public DataSet CreatePrintCode(int iAmount, string strSiteCode,string strClientCode, string strStart, string strEnd)
        {
            StringBuilder strSql = new StringBuilder();
            string strExtracode = Guid.NewGuid().ToString("N");
            string strBeginTime = "2000-01-01 00:00:00";
            string strEndTime = "2099-01-01 00:00:00";
            if (!string.IsNullOrEmpty(strStart)) strBeginTime = strStart;
            if (!string.IsNullOrEmpty(strEnd)) strEndTime = strStart;
            for (int i = 0; i < iAmount; i++) 
            {
                strSql.Append(" INSERT INTO HP_PrintCode ([ID],[PrintCode],[SiteCode],[ClientID],[Start],[End],[State],[Extracode],[Create]) ");
                strSql.Append(" VALUES ('" + Guid.NewGuid().ToString("N") + "','" + GenerateCheckCode(7) + "','" + strSiteCode + "','" + strClientCode + "','"
                                           + strBeginTime + "','" + strEndTime + "',0,'" + strExtracode + "','" + DateTime.Now.ToString() + "'); ");
            }
            strSql.Append(" INSERT INTO HP_CodePrint (ID,SiteCode,[State],Amount,PrintCode) VALUES('" + Guid.NewGuid().ToString("N") + "','" + strSiteCode + "',0," + iAmount.ToString() + ",'" + strExtracode + "') ");
            strSql.Append(" SELECT [PrintCode] FROM HP_PrintCode WHERE Extracode = '" + strExtracode + "'");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds ;
        }
        #endregion

        #region 返回所有打印码
        /// <summary>
        /// 返回所有打印码
        /// </summary>
        /// <param name="strSiteCode">站点代码</param>
        /// <returns></returns>
        public DataSet GetPrintCodeBySiteCode(string strSiteCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * FROM HP_PrintCode WHERE SiteCode = @SiteCode ORDER BY [Create] DESC ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode)
                };
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
            return ds;
        }
        #endregion

        #region 返回查询条件的打印码
        /// <summary>
        /// 返回所有打印码
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public DataSet GetPrintCodeByWhere(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT ID,PrintCode,ClientID,[Start],[End],[Create],CASE [State] WHEN 0 THEN '有效' ELSE '无效' END AS [State] FROM HP_PrintCode ");
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql.Append(" WHERE " + strWhere); 
            }
            strSql.Append(" ORDER BY [Create] DESC ");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion

        #region 重置打印码
        /// <summary>
        /// 重置打印码
        /// </summary>
        /// <param name="strID">重置ID</param>
        /// <returns></returns>
        public bool ResetPrintCode(string strID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" Update HP_PrintCode SET [State] = 0 ");
            strSql.Append(" WHERE ID = '" + strID + "'" );
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

        #region 获取所需要打印的图片
        /// <summary>
        /// 获取所需要打印的图片
        /// </summary>
        /// <param name="strSiteCode">站点代码</param>
        /// <param name="strClientCode">指定的终端代码</param>
        /// <returns></returns>
        public DataSet getPrintPhoto(string strSiteCode, string strClientCode)
        {

            string sql = @"SELECT TOP 1 ID,SiteCode,OpenId,ClientID,PrintCode,Img,[State],AttachText  
                        FROM [HP_Photo]
                        WHERE [State]=0 
                        AND SiteCode=@SiteCode 
                        AND ClientID=@ClientID
                        AND PrintCode != '' ";
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode),
                    new System.Data.SqlClient.SqlParameter("@ClientID", strClientCode)
                };
            DataSet ds = DbHelperSQL.Query(sql, paras.ToArray());
            return ds;
        }
        #endregion


        #region 修改照片与打印码状态
        /// <summary>
        /// 获取所需要打印的图片
        /// </summary>
        /// <param name="strPhotoID">照片ID</param>
        /// <param name="strState">修改后状态</param>
        /// <returns></returns>
        public bool UpdatePrintState(string strPhotoID,string strState)
        {
            string sql = @" UPDATE [HP_Photo] SET [State ]= @State WHERE ID = @ID; ";
            sql = sql + @" UPDATE a SET a.[State] = @State FROM HP_PrintCode a JOIN HP_Photo b ON (a.ClientID = b.ClientID AND a.PrintCode = b.PrintCode) 
                            WHERE b.ID = @ID ";
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strPhotoID),
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

        #region 生成随机位数的数字串
        /// <summary>
        /// 生成随机校验码字符串
        /// </summary>
        /// <param name="iAmount">生成数字串的位数</param>
        /// <returns>生成的随机校验码字符串</returns>
        public string GenerateCheckCode(int iAmount)
        {
            int number;
            string strCode = string.Empty;

            //随机数种子
            Random random = new Random(GetRandomSeed());

            for (int i = 0; i < iAmount; i++) //校验码长度为4
            {
                //随机的整数
                number = random.Next();

                ////字符从0-9,A-Z中随机产生,对应的ASCII码分别为
                ////48-57,65-90
                //number = number % 36;
                //if (number < 10)
                //{
                //    number += 48;
                //}
                //else
                //{
                //    number += 55;
                //}
                //字符从0-9,A-Z中随机产生,对应的ASCII码分别为
                number = number % 10 + 48;
                strCode += ((char)number).ToString();
            }
            return strCode;
        }

        public int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
        #endregion

        /// <summary>
        /// 判断是否存在打印码
        /// </summary>
        /// <param name="printCode"></param>
        /// <returns></returns>
        public bool ExistPrintCode(string printCode)
        {
            string sql = @"SELECT count(ID) FROM [HP_PrintCode] 
                    WHERE PrintCode=@PrintCode 
                    AND State=@State";
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@PrintCode", printCode),
                    new System.Data.SqlClient.SqlParameter("@State", 0)
                };
            return DbHelperSQL.Exists(sql, paras.ToArray());
        }

        /// <summary>
        /// 根据打印码获取客户机ID
        /// </summary>
        /// <param name="printCode"></param>
        /// <returns></returns>
        public string GetClientIDByPrintCode(string printCode,string siteCode)
        {
            string sql = @"SELECT ClientID FROM [HP_PrintCode] 
                    WHERE PrintCode=@PrintCode 
                    AND SiteCode=@SiteCode
                    AND State=0";
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@PrintCode", printCode),
                    new System.Data.SqlClient.SqlParameter("@SiteCode", siteCode)
                };
            ExceptionLogDAL.InsertExceptionLog(new ExceptionLog() { Message = string.Format("{0}||{1}",printCode,siteCode) });
            return DbHelperSQL.GetSingle(sql, paras.ToArray()) as string;
        }
        #region 添加并返回打印码
        /// <summary>
        /// 添加并返回打印码
        /// </summary>
        /// <param name="iAmount">指定的数字串个数</param>
        /// <param name="strSiteCode">站点代码</param>
        /// <param name="strClientCode">指定的终端代码</param>
        /// <param name="strStart">有效开始时间</param>
        /// <param name="strEnd">有效的结束时间</param>
        /// <returns></returns>
        public DataSet AddPrintCode(int iAmount, string strSiteCode, string strClientCode, string strStart, string strEnd)
        {
            StringBuilder strSql = new StringBuilder();
            string strExtracode = Guid.NewGuid().ToString("N");
            string strBeginTime =DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string strEndTime = "2099-01-01 00:00:00";
            if (!string.IsNullOrEmpty(strStart)) strBeginTime = strStart;
            if (!string.IsNullOrEmpty(strEnd)) strEndTime = strStart;
            for (int i = 0; i < iAmount; i++)
            {
                strSql.Append(" INSERT INTO HP_PrintCode ([ID],[PrintCode],[SiteCode],[ClientID],[Start],[End],[State],[Extracode],[Create]) ");
                strSql.Append(" VALUES ('" + Guid.NewGuid().ToString("N") + "','" + GenerateCheckCode(7) + "','" + strSiteCode + "','" + strClientCode + "','"
                                           + strBeginTime + "','" + strEndTime + "',0,'" + strExtracode + "','" + DateTime.Now.ToString() + "'); ");
            }
            strSql.Append(" INSERT INTO HP_CodePrint (ID,SiteCode,[State],Amount,PrintCode) VALUES('" + Guid.NewGuid().ToString("N") + "','" + strSiteCode + "',0," + iAmount.ToString() + ",'" + strExtracode + "') ");
            strSql.Append(" SELECT * FROM HP_PrintCode WHERE Extracode = '" + strExtracode + "'");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion
    }
}
