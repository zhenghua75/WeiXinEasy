using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.SYS;
using Maticsoft.DBUtility;

namespace DAL.SYS
{
   public class Account_ExtDAL
    {
       public Account_ExtDAL() { ; }
       #region 返回所有信息
       /// <summary>
       /// 返回所有信息
       /// </summary>
       /// <param name="where">条件</param>
       /// <returns></returns>
       public DataSet GetAccount_ExtList(string where)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM SYS_Account_Ext ");
           if (!string.IsNullOrEmpty(where))
           {
               strSql.Append(" WHERE " + where);
           }
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion

       #region 根据账户获取相应信息
       /// <summary>
       /// 根据账户获取相应信息
       /// </summary>
       /// <param name="accountid">账户标识</param>
       /// <returns></returns>
       public DataSet GetAccountds(string accountid)
       {
           string safesql = "";
           if (accountid.Trim() != null && accountid.Trim() != "")
           {
               safesql = " select * from SYS_Account_Ext where accountid='"+accountid+"'";
               DataSet ds = DbHelperSQL.Query(safesql.ToString());
               return ds;
           }
           return null;
       }
       #endregion

       #region 添加信息
       /// <summary>
       /// 添加信息
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool SaveAccount_Ext(Account_Ext model)
       {
           string sql = @"INSERT INTO [SYS_Account_Ext]
                        ([AccountID]
                        ,[Photo]
                        ,[Summary]
                        ,[Remark]
                        ,[SiteCategory]
                        ,[Themes],[CodeImg],[PrintImg1],[PrintImg2],[PrintImg3],[PrintImg4])
                 VALUES
                        (@AccountID
                       ,@Photo
                       ,@Summary
                       ,@Remark
                       ,@SiteCategory
                       ,@Themes,@CodeImg,@PrintImg1,@PrintImg2,@PrintImg3,@PrintImg4)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@AccountID", model.AccountID),
                new System.Data.SqlClient.SqlParameter("@Photo", model.Photo),
                new System.Data.SqlClient.SqlParameter("@Summary", model.Summary),
                new System.Data.SqlClient.SqlParameter("@Remark", model.Remark),
                new System.Data.SqlClient.SqlParameter("@SiteCategory", model.SiteCategory),
                new System.Data.SqlClient.SqlParameter("@Themes", model.Themes),
                new System.Data.SqlClient.SqlParameter("@CodeImg", model.CodeImg),
                new System.Data.SqlClient.SqlParameter("@PrintImg1", model.PrintImg1),
                new System.Data.SqlClient.SqlParameter("@PrintImg2", model.PrintImg2),
                new System.Data.SqlClient.SqlParameter("@PrintImg3", model.PrintImg3),
                new System.Data.SqlClient.SqlParameter("@PrintImg4", model.PrintImg4)
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

       #region 更新信息
       /// <summary>
       /// 更新信息
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateAccount_Ext(Account_Ext model)
       {
           string safesql = "";
           safesql = "update SYS_Account_Ext set ";
           if (model.AccountID != null && model.AccountID != "")
           {
               safesql += "AccountID='" + model.AccountID + "',";
           }
           if (model.Photo != null && model.Photo != "")
           {
               safesql += "Photo='" + model.Photo+"',";
           }
           if (model.Summary != null && model.Summary != "")
           {
               safesql += "Summary='" + model.Summary + "',";
           }
           if (model.Remark != null && model.Remark != "")
           {
               safesql += "Remark='" + model.Remark + "',";
           }
           if (model.SiteCategory != null && model.SiteCategory != "")
           {
               safesql += "SiteCategory='" + model.SiteCategory + "',";
           }
           
           if (model.CodeImg != null && model.CodeImg != "")
           {
               safesql += "CodeImg='" + model.CodeImg + "',";
           }
           if (model.PrintImg1 != null && model.PrintImg1 != "")
           {
               safesql += "PrintImg1='" + model.PrintImg1 + "',";
           }
           if (model.PrintImg2 != null && model.PrintImg2 != "")
           {
               safesql += "PrintImg2='" + model.PrintImg2 + "',";
           }
           if (model.PrintImg3 != null && model.PrintImg3 != "")
           {
               safesql += "PrintImg3='" + model.PrintImg3 + "',";
           }
           if (model.PrintImg4 != null && model.PrintImg4 != "")
           {
               safesql += "PrintImg4='" + model.PrintImg4 + "',";
           }
           if (model.Themes != null && model.Themes != "")
           {
               safesql += "Themes='" + model.Themes + "'";
           }
           else
           {
               safesql += "Themes='" + GetAccountValue("Themes", model.AccountID) + "'";
           }
           safesql += " where AccountID='" + model.AccountID + "'";
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

       #region 获取相应的数据
       /// <summary>
       /// 获取相应的数据
       /// </summary>
       /// <param name="value">字段</param>
       /// <param name="accountid">条件值</param>
       /// <returns></returns>
       public string GetAccountValue(string value, string accountid)
       {
           if (value.Trim() != null && value.Trim() != "")
           {
               string safesql = "select " + value + " from SYS_Account_Ext where accountid='" + accountid + "'";
               return DbHelperSQL.GetSingle(safesql).ToString();
           }
           return null;
       }
       #endregion

       #region 检查是否已经存在信息
       /// <summary>
       /// 检查是否已经存在信息
       /// </summary>
       /// <param name="accountid">账户标识</param>
       /// <returns></returns>
       public bool IsExsit(string accountid)
       {
           string safesql = "select count(accountid) from SYS_Account_Ext where accountid='" + accountid + "'";
           int rowsAffected = 0;
           try
           {
               rowsAffected = Convert.ToInt32(DbHelperSQL.GetSingle(safesql.ToString()));
           }
           catch (Exception)
           {
               rowsAffected = 0;
           }

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

       #region 根据站点代码获取账户打印照片信息
       /// <summary>
       /// 根据站点代码获取账户打印照片信息
       /// </summary>
       /// <param name="strSiteCode"></param>
       /// <returns></returns>
       public DataSet GetPrintImgBySiteCode(string strSiteCode)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select a.*,b.Name Name from SYS_Account_Ext a,SYS_Account b where b.SiteCode='" + strSiteCode + "' ");
           strSql.Append(" and b.ID=a.AccountID ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
    }
}
