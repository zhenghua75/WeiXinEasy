using Maticsoft.DBUtility;
/* ==============================================================================
 * 类名称：SQLHelperExt
 * 类描述：
 * 创建人：yhn,51809571@qq.com
 * 创建时间：2014/4/12 11:57:42
 * 修改人：
 * 修改时间：
 * 修改备注：
 * @version 1.0
 * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using DAL.SYS;
using Model.SYS;

namespace DAL.Common
{
    public class SQLHelperExt
    {
        #region INSERT
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static int Insert(string tableName,Dictionary<string,object> datas)
        {
            int res = 0;
            if (!string.IsNullOrEmpty(tableName) && datas != null && datas.Count > 0)
            {
                StringBuilder fields = new StringBuilder();
                StringBuilder values = new StringBuilder();
                List<SqlParameter> paraList = new List<SqlParameter>();
                bool flag = false;
                foreach (string key in datas.Keys)
                {
                    if (flag)
                    {
                        fields.Append(",");
                        values.Append(",");
                    }
                    fields.AppendFormat("{0}", key);
                    values.AppendFormat("@{0}", key);
                    paraList.Add(new SqlParameter(string.Format("@{0}",key),datas[key]));
                    if (!flag)
                    {
                        flag = true;
                    }
                }
                string sql=string.Format("INSERT INTO {0} ({1}) VALUES ({2})",tableName,fields.ToString(),values.ToString());
                res=DbHelperSQL.ExecuteSql(sql, paraList.ToArray());
            }
            return res;
        }
        #endregion

        #region DELETE
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static int Delete(string tableName,string where)
        {
            int res = 0;
            if (!string.IsNullOrEmpty(where))
            {
                string sql = string.Format("DELETE FROM {0} WHERE {1}",tableName,where);
                res = DbHelperSQL.ExecuteSql(sql);
            }
            return res;
        }
        #endregion

        #region UPDATE
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static int Update(string tableName, Dictionary<string, object> datas,string where)
        {
            int res = 0;
            if (!string.IsNullOrEmpty(tableName) && datas != null && datas.Count > 0)
            {
                StringBuilder values = new StringBuilder();
                List<SqlParameter> paraList = new List<SqlParameter>();
                bool flag = false;
                foreach (string key in datas.Keys)
                {
                    if (flag)
                    {
                        values.Append(",");
                    }
                    values.AppendFormat("{0}=@{0}", key);
                    paraList.Add(new SqlParameter(string.Format("@{0}", key), datas[key]));
                    if (!flag)
                    {
                        flag = true;
                    }
                }
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat("UPDATE {0} SET {1}", tableName, values.ToString());
                if (!string.IsNullOrEmpty(where))
                {
                    sql.AppendFormat(" WHERE {0}",where);
                }
                ExceptionLogDAL.InsertExceptionLog(
                    new ExceptionLog()
                    {
                        Message = sql.ToString()
                    }
                    );
                res = DbHelperSQL.ExecuteSql(sql.ToString(), paraList.ToArray());
            }
            return res;
        }
        #endregion

        #region SELECT
        #endregion

        #region Other
        //sql防流放过滤关键字   
        public static bool CheckKeyWord(string sWord)
        {
            //过滤关键字
            string StrKeyWord = @"select|insert|delete|from|count\(|drop table|update|truncate|asc\(|mid\(|char\(|xp_cmdshell|exec master|netlocalgroup administrators|:|net user|""|or|and";
            //过滤关键字符
            string StrRegex = @"[;|,|/|\(|\)|\[|\]|}|{|%|\@|*|!|']";
            if (Regex.IsMatch(sWord, StrKeyWord, RegexOptions.IgnoreCase) || Regex.IsMatch(sWord, StrRegex))
                return true;
            return false;
        }
        #endregion
    }
}
