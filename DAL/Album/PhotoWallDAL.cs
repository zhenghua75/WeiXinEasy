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
    public class PhotoWallDAL
    {
        public const string TABLE_NAME = "PA_PhotoWall";

        /// <summary>
        /// 添加用户相册照片
        /// </summary>
        /// <param name="info"></param>
        public void Insert(PhotoWall info)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("ID", info.ID);
            data.Add("Type", info.Type);
            data.Add("Name", info.Name);
            data.Add("SiteCode", info.SiteCode);
            data.Add("OpenId", info.OpenId);
            data.Add("FilePath", info.FilePath);
            data.Add("Remark", info.Remark);
            data.Add("AddTime", info.AddTime);
            data.Add("State", info.State);
            SQLHelperExt.Insert(TABLE_NAME, data);
        }

        #region 返回照片
        /// <summary>
        /// 返回照片
        /// </summary>
        /// <param name="strThumbID">相册列表ID</param>
        /// <returns></returns>
        public DataSet GetUserThumb(string strThumbID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM PA_PhotoWall a ");
            strSql.Append(" WHERE a.ID = @ID ");
            strSql.Append(" AND [State] = 1 ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", strThumbID)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion

        #region 返回照片集
        /// <summary>
        /// 返回照片集
        /// </summary>
        /// <param name="strListID">相册列表ID</param>
        /// <returns></returns>
        public DataSet GetPhotoWall(string strListID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT b.*,(SELECT COUNT(c.ID) FROM PA_GreatUserPhoto c WHERE c.ThumbID = b.ID) AS GreatCount ");
            strSql.Append(" FROM PA_PhotoList a ");
            strSql.Append(" LEFT JOIN PA_PhotoWall b ON (b.AddTime > a.BeginTime AND b.AddTime < a.EndTime AND b.SiteCode = a.SiteCode) ");
            strSql.Append(" WHERE a.ID = @ID ");
            strSql.Append(" AND [State] = 1 ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", strListID)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion

        #region 返回账户与站点信息
        /// <summary>
        /// 返回账户与站点信息
        /// </summary>
        /// <param name="strAlbunTypeID">相册类别ID</param>
        /// 
        public DataSet GetAccountData(string strAlbunTypeID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT b.ID,b.SiteCode,b.Name,c.Themes ");
            strSql.Append(" FROM PA_PhotoList a ");
            strSql.Append(" LEFT JOIN SYS_Account b ON (b.SiteCode = a.SiteCode) ");
            strSql.Append(" LEFT JOIN SYS_Account_Ext c ON (c.AccountID = b.ID) ");
            strSql.Append(" WHERE a.ID = @ID ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", strAlbunTypeID)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion

        #region 根据条件返回照片集
        /// <summary>
        /// 根据条件返回照片集
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public DataSet GetPhotoList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM PA_PhotoWall ");
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }
        #endregion

        #region 获取照片属性
        /// <summary>
        /// 获取照片属性
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="strID"></param>
        /// <returns></returns>
        public object GetUserPhotoValue(string strValue, string strID)
        {
            string safesql = "";
            safesql = "select " + strValue + " from " + TABLE_NAME + " where ID='" + strID + "'";
            return DbHelperSQL.GetSingle(safesql.ToString());
        }
        #endregion

        #region 更新照片状态
        /// <summary>
        /// 更新照片状态
        /// </summary>
        /// <param name="strID"></param>
        /// <returns></returns>
        public bool UpdateUserPhotoState(string strID)
        {
            StringBuilder strSql = new StringBuilder();
            int state = 0;
            try
            {
                state = Convert.ToInt32(GetUserPhotoValue("[State]", strID));
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
            strSql.Append(" UPDATE " + TABLE_NAME + " ");
            strSql.Append(" SET [State] = @State ");
            strSql.Append(" WHERE ID = @ID ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strID),
                new System.Data.SqlClient.SqlParameter("@State", state)
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
