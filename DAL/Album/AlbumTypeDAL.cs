using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.PA;
using Maticsoft.DBUtility;

namespace DAL.Album
{
   public class AlbumTypeDAL
    {
       public AlbumTypeDAL() { ;}

       #region 根据站点代码返回所有相册类别
       /// <summary>
       /// 根据站点代码返回所有相册类别
       /// </summary>
       /// <param name="SiteCode"></param>
       /// <returns></returns>
       public DataSet GetAlbumTypeBySiteCode(string where)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT *  FROM PA_AlbumType ");
           if (!string.IsNullOrEmpty(where))
           {
               strSql.Append(" WHERE " + where);
           }
           return DbHelperSQL.Query(strSql.ToString());
       }
       #endregion

       #region 获取未被删除的列表
       /// <summary>
       /// 获取未被删除的列表
       /// </summary>
       /// <param name="where"></param>
       /// <returns></returns>
       public DataSet GetAlbumTypeByIsDel(string where)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT *  FROM PA_AlbumType WHERE IsDel=0 ");
           if (!string.IsNullOrEmpty(where))
           {
               strSql.Append(" AND " + where);
           }
           return DbHelperSQL.Query(strSql.ToString());
       }
       #endregion

       #region 返回所有相册类别
       /// <summary>
       /// 返回所有相册类别
       /// </summary>
       /// <returns></returns>
       public DataSet GetAlbumTypeList()
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT *  FROM PA_AlbumType ");
           return DbHelperSQL.Query(strSql.ToString());
       }
       #endregion

       #region 返回详细内容
       /// <summary>
       /// 返回详细内容
       /// </summary>
       /// <param name="albumtypeid"></param>
       /// <returns></returns>
       public DataSet GetAlbumTypeDetail(string albumtypeid)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * FROM PA_AlbumType  ");
           strSql.Append(" WHERE [ID] = @ID ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", albumtypeid),
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion

       #region 添加相册类别
       /// <summary>
       /// 添加相册类别
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddAlbumType(PA_AlbumType model)
       {
           string sql = @"INSERT INTO [PA_AlbumType]
                        ([ID]
                        ,[SiteCode]
                        ,[Name]
                        ,[Cover]
                        ,[Content]
                        ,[IsDel])
                 VALUES
                        (@ID
                       ,@SiteCode
                       ,@Name
                       ,@Cover
                       ,@Content
                       ,@IsDel)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@SiteCode", model.SiteCode),
                new System.Data.SqlClient.SqlParameter("@Name", model.Name),
                new System.Data.SqlClient.SqlParameter("@Cover", model.Cover),
                new System.Data.SqlClient.SqlParameter("@Content", model.Content),
                new System.Data.SqlClient.SqlParameter("@IsDel", model.IsDel)
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

       #region 修改相册类别
       /// <summary>
       /// 修改相册类别
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateAlbumType(PA_AlbumType model)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append("UPDATE PA_AlbumType SET ");
           if (!string.IsNullOrEmpty(model.SiteCode))
           {
               strSql.Append("[SiteCode]='" + model.SiteCode + "',");
           }
           if (!string.IsNullOrEmpty(model.Name))
           {
               strSql.Append("[Name]='" + model.Name + "',");
           }
           if (!string.IsNullOrEmpty(model.Cover))
           {
               strSql.Append("[Cover]='" + model.Cover + "',");
           }
           if (!string.IsNullOrEmpty(model.Content))
           {
               strSql.Append("[Content]='" + model.Content + "',");
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

       #region 获取相册类别属性
       /// <summary>
       /// 获取相册类别属性
       /// </summary>
       /// <param name="value"></param>
       /// <param name="albumtypeid"></param>
       /// <returns></returns>
       public object GetAlbumTypeValue(string value, string albumtypeid)
       {
           object obj = null;
           if (value.Trim() != null && value.Trim() != "")
           {
               string safesql = " select " + value + " from PA_AlbumType where id='" + albumtypeid + "' ";
               obj = DbHelperSQL.GetSingle(safesql);
           }
           return obj;
       }
       #endregion

       #region 删除相册类别
       /// <summary>
       /// 删除相册类别
       /// </summary>
       /// <param name="albumId"></param>
       /// <returns></returns>
       public bool DelAlbumType(string albumId)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetAlbumTypeValue("IsDel", albumId));
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
           strSql.Append(" UPDATE PA_AlbumType ");
           strSql.Append(" SET IsDel = @IsDel ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", albumId),
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
