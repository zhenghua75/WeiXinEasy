/* ==============================================================================
 * 类名称：ExtensionMethods
 * 类描述：
 * 创建人：yhn,51809571@qq.com
 * 创建时间：2014/3/15 11:25:53
 * 修改人：
 * 修改时间：
 * 修改备注：
 * @version 1.0
 * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

namespace DBUtility
{
    public static class ExtensionMethods
    {
        public static T GetColumnValue<T>(this DataRow dr, string colName, T defaultValue)
        {
            T res = defaultValue;
            if (dr[colName] != null && dr[colName] != DBNull.Value)
            {
                res = (T)dr[colName];
            }
            return res;
        }

        public static T GetColumnValue<T>(this DataRow dr, string colName)
        {
            T res = default(T);
            if (dr[colName] != null && dr[colName] != DBNull.Value)
            {
                res = (T)dr[colName];
            }
            return res;
        }

        /// <summary>
        /// 通过反射将数据行转换为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static T ConvertToObj<T>(this DataRow dr) where T : new()
        {
            T t = new T();
            if (dr == null) return default(T);
            // 获得此模型的公共属性
            PropertyInfo[] propertys = t.GetType().GetProperties();
            DataColumnCollection Columns = dr.Table.Columns;
            foreach (PropertyInfo p in propertys)
            {
                string columnName = p.Name;
                // string columnName = p.Name;如果不用属性，数据库字段对应model属性,就用这个
                if (Columns.Contains(columnName))
                {
                    // 判断此属性是否有Setter或columnName值是否为空
                    object value = dr[columnName];
                    if (!p.CanWrite || value is DBNull || value == DBNull.Value) continue;

                    switch (p.PropertyType.ToString())
                    {
                        case "System.String":
                            p.SetValue(t, Convert.ToString(value), null);
                            break;
                        case "System.Int32":
                            p.SetValue(t, Convert.ToInt32(value), null);
                            break;
                        case "System.Int64":
                            p.SetValue(t, Convert.ToInt64(value), null);
                            break;
                        case "System.DateTime":
                            p.SetValue(t, Convert.ToDateTime(value), null);
                            break;
                        case "System.Boolean":
                            p.SetValue(t, Convert.ToBoolean(value), null);
                            break;
                        case "System.Double":
                            p.SetValue(t, Convert.ToDouble(value), null);
                            break;
                        case "System.Decimal":
                            p.SetValue(t, Convert.ToDecimal(value), null);
                            break;
                        default:
                            p.SetValue(t, value, null);
                            break;
                    }


                }
            }
            return t;
        }

        /// <summary>
        /// 通过反射将数据表转换为对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ConvertToList<T>(this DataTable dt) where T : new()
        {
            List<T> tList = new List<T>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    T t = dr.ConvertToObj<T>();
                    tList.Add(t);
                }
            }
            return tList;
        }

        /// <summary>
        /// 通过反射将数据表转换为对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ConvertToList<T>(this DataSet ds) where T : new()
        {
            List<T> tList = new List<T>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    T t = dr.ConvertToObj<T>();
                    tList.Add(t);
                }
            }
            return tList;
        }

        /// <summary>
        /// 通过反射将获取数据集的第一个数据表中的第一行数据对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static T ConvertToFirstObj<T>(this DataSet ds) where T : new()
        {
            T res = default(T);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                res=ds.Tables[0].Rows[0].ConvertToObj<T>();
            }
            return res;
        }

        /// <summary>
        /// 通过反射生成对象的插入数据库
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GenerateInsertSql<T>(this T obj,string tableName, string[] properties,out System.Data.SqlClient.SqlParameter[] parameters) where T : new()
        {
            StringBuilder res = new StringBuilder();
            parameters = new System.Data.SqlClient.SqlParameter[properties.Length];
            res.AppendFormat("INSERT INTO [{0}]",tableName);
            foreach (string property in properties)
            {

            }
            return res.ToString();
        }
    }
}
