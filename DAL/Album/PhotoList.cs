using DAL.Common;
using Model.Album;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Maticsoft.DBUtility;

namespace DAL.Album
{
    public class PhotoList
    {
        #region 根据条件返回照片集
        /// <summary>
        /// 根据条件返回照片集
        /// </summary>
        /// <param name="strSiteCode">站点代码</param>
        /// <returns></returns>
        public DataSet GetPhotoList(string strSiteCode)
        {
            string sql = @"SELECT * FROM PA_PhotoList
                        WHERE SiteCode = @SiteCode";
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode),
                };
            return DbHelperSQL.Query(sql, paras.ToArray());
        }
        #endregion
    }
}
