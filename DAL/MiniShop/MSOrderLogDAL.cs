using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.MiniShop;
using Maticsoft.DBUtility;

namespace DAL.MiniShop
{
   public class MSOrderLogDAL
    {
       public MSOrderLogDAL() { ; }
       /// <summary>
       /// 添加订单日志
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddMSOrderLog(MSOrderLog model)
       {
           string sql = @"INSERT INTO [MSOrderLog] ([logdate],[textmsg])  VALUES (@logdate,@textmsg)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                 new System.Data.SqlClient.SqlParameter("@logdate", DateTime.Now),
                new System.Data.SqlClient.SqlParameter("@textmsg", model.Textmsg)
               
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
       /// <summary>
       /// 添加订单日志
       /// </summary>
       /// <param name="txtmsg"></param>
       /// <returns></returns>
       public static bool AddMSOrderLog(string txtmsg)
       {
           string sql = @"INSERT INTO [MSOrderLog] ([logdate],[textmsg])  VALUES (@logdate,@textmsg)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                 new System.Data.SqlClient.SqlParameter("@logdate", DateTime.Now),
                new System.Data.SqlClient.SqlParameter("@textmsg", txtmsg)
               
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
    }
}
