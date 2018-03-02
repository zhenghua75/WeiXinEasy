using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maticsoft.DBUtility;
using Model.PA;

namespace DAL.Album
{
    public class AlbumDAL
    {
        public AlbumDAL() { ; }

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
            strSql.Append(" FROM PA_AlbumType a ");
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
        
        #region 返回当前站点所有相册
        /// <summary>
        /// 返回当前站点所有相册
        /// </summary>
        /// <param name="strSiteCode">站点代码</param>
        /// 
        public DataSet GetAlbumTypeList(string strSiteCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM PA_AlbumType ");
            strSql.Append(" WHERE SiteCode = @SiteCode ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion

        #region 返回当前类别所有照片
        /// <summary>
        /// 返回当前类别所有照片
        /// </summary>
        /// <param name="strAlbumTypeID">相册类别</param>
        /// 
        public DataSet GetAlbumList(string strAlbumTypeID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT a.*,b.Name AS typename FROM PA_Album a ");
            strSql.Append(" LEFT JOIN PA_AlbumType b ON (b.ID = a.[Type]) WHERE a.IsDel=0 ");
            strSql.Append(" AND b.ID = @AlbumTypeID ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@AlbumTypeID", strAlbumTypeID)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion

        #region 返回当前相册详细信息
        /// <summary>
        /// 返回当前相册详细信息
        /// </summary>
        /// <param name="AlbumId"></param>
        /// <returns></returns>
        public DataSet GetAlbumDetail(string AlbumId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT a.*,b.Name AS typename FROM PA_Album a ");
            strSql.Append(" LEFT JOIN PA_AlbumType b ON (b.ID = a.[Type]) ");
            strSql.Append(" WHERE a.[ID] = @ID ");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", AlbumId),
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion

        #region　添加相册
        /// <summary>
        /// 添加相册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertAlbum(PA_Album model)
        {
            string sql = @"INSERT INTO [PA_Album]
                        ([ID]
                        ,[Type]
                        ,[Name]
                        ,[Photo]
                        ,[Note]
                         ,[IsDel])
                 VALUES
                        (@ID
                       ,@Type
                       ,@Name
                       ,@Photo
                       ,@Note
                       ,@IsDel)";
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@Type", model.Type),
                new System.Data.SqlClient.SqlParameter("@Name", model.Name),
                new System.Data.SqlClient.SqlParameter("@Photo", model.Photo),
                new System.Data.SqlClient.SqlParameter("@Note", model.Note),
                new System.Data.SqlClient.SqlParameter("@IsDel", model.IsDel)
            };
            int rowsAffected = DbHelperSQL.ExecuteSql(sql.ToString(),paras);
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

        #region 修改相册信息
        /// <summary>
        /// 修改相册信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateAlbum(PA_Album model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE PA_Album SET ");
            if (!string.IsNullOrEmpty(model.Type))
            {
                strSql.Append("[Type]='" + model.Type + "',");
            }
            if (!string.IsNullOrEmpty(model.Name))
            {
                strSql.Append("[Name]='" + model.Name + "',");
            }
            if (!string.IsNullOrEmpty(model.Photo))
            {
                strSql.Append("[Photo]='" + model.Photo + "',");
            }
            if (!string.IsNullOrEmpty(model.Note))
            {
                strSql.Append("[Note]='" + model.Note + "',");
            }
            if (model.IsDel > 0)
            {
                strSql.Append("[IsDel]=" + model.IsDel + ",");
            }
            else
            {
                strSql.Append("[IsDel]=0,");
            }
            int n = strSql.ToString().LastIndexOf(",");
            strSql.Remove(n, 1);
            strSql.Append(" WHERE ID ='" + model.ID + "' ");
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

        #region 获取相册属性
        /// <summary>
        /// 获取相册属性
        /// </summary>
        /// <param name="value"></param>
        /// <param name="albumid"></param>
        /// <returns></returns>
        public object GetAlbumValue(string value, string albumid)
        {
            object obj = null;
            if (value.Trim() != null && value.Trim() != "")
            {
                string safesql = " select " + value + " from PA_Album where id='" + albumid + "' ";
                obj = DbHelperSQL.GetSingle(safesql);
            }
            return obj;
        }
        #endregion

        #region 删除相册
        /// <summary>
        /// 删除相册
        /// </summary>
        /// <param name="albumid"></param>
        /// <returns></returns>
        public bool DelAlbum(string albumid)
        {
            StringBuilder strSql = new StringBuilder();
            int state = 0;
            try
            {
                state = Convert.ToInt32(GetAlbumValue("IsDel", albumid));
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
            strSql.Append(" UPDATE PA_Album ");
            strSql.Append(" SET IsDel = @IsDel ");
            strSql.Append(" WHERE ID = @ID ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", albumid),
                new System.Data.SqlClient.SqlParameter("@IsDel", state)
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
