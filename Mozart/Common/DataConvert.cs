using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.ComponentModel;
using System.Diagnostics;


namespace Mozart.Common
{
    public class DataConvert
    {
        public static T DataRowToModel<T>(DataRow dr) where T : new()
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


        #region DataTable转List<T>
        /// <summary>
        /// DataTable转List<T>
        /// </summary>
        /// <typeparam name="T">数据项类型</typeparam>
        /// <param name="dt">DataTable</param>
        /// <returns>List数据集</returns>
        public static List<T> DataTableToList<T>(DataTable dt) where T : new()
        {
            List<T> tList = new List<T>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    T t = DataRowToModel<T>(dr);
                    tList.Add(t);
                }
            }
            return tList;
        }
        #endregion
    }
}