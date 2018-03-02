using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Maticsoft.DBUtility;


namespace DataListen
{
    public class DataListenDAL
    {
        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool AddCouponInfo(DataListenMODEL.CouponInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" INSERT INTO CouponCheck ([ID],[OpenID],[AddTime],[CheckTime],[CouponNo],[CouponName],[CouponContent]) ");
            strSql.Append(" VALUES ('" + model.ID + "','" + model.OpenID + "','" + model.AddTime + "','" + model.CheckTime + "','" + model.CouponNo + "','" + model.ActTitle +"','" + model.ActContent+"') ");
            int rows = DbHelperSQLite.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 得到打印过的列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetCouponList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM CouponCheck ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQLite.Query(strSql.ToString());
        }
    }
}
