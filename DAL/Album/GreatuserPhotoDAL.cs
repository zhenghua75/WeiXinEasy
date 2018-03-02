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
    public class GreatuserPhotoDAL
    {
        public const string TABLE_NAME = "PA_GreatUserPhoto";

        /// <summary>
        /// 添加用户相册照片点赞
        /// </summary>
        /// <param name="info"></param>
        public void Insert(GreatUserPhoto info)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("ID", info.ID);
            data.Add("SiteCode", info.SiteCode);
            data.Add("OpenId", info.OpenId);
            data.Add("ThumbID", info.ThumbID);
            data.Add("AddTime", info.AddTime);
            SQLHelperExt.Insert(TABLE_NAME, data);
        }

        #region 判断用户是否已经点赞
        /// <summary>
        /// 判断用户是否已经点赞
        /// </summary>
        /// <param name="siteCode">站点代码</param>
        /// <param name="strThumbID">照片ID</param>
        /// <param name="openID">用户ID</param>
        /// <returns></returns>
        public bool ExistGreatUserPhoto(string siteCode, string strThumbID, string openID)
        {
            string sql = @"SELECT count(ID) FROM PA_GreatUserPhoto 
                    WHERE SiteCode=@SiteCode 
                    AND ThumbID=@ThumbID
                    AND OpenID=@OpenID";
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", siteCode),
                    new System.Data.SqlClient.SqlParameter("@ThumbID", strThumbID),
                    new System.Data.SqlClient.SqlParameter("@OpenID", openID)
                };
            return DbHelperSQL.Exists(sql, paras.ToArray());
        }
        #endregion
    }
}
