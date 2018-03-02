using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Maticsoft.DBUtility;
using Model.DC;

namespace DAL.DC
{
   public class DC_HouseDAL
    {
       public DC_HouseDAL() { ;}

          #region 添加房产信息
           /// <summary>
           /// 添加房产信息
           /// </summary>
           /// <param name="model"></param>
           /// <returns></returns>
           public int AddDCHouse(DC_House model)
           {

               string sql = @"INSERT INTO [DC_House]
                        ([ID]
                       ,[SiteCode]
                       ,[Photo]
                       ,[Summary]
                       ,[SaleRental]
                       ,[Price]
                       ,[HouseType]
                       ,[Faces]
                       ,[Area]
                       ,[Renovation]
                       ,[Floor]
                       ,[UseType]
                       ,[Buildings]
                       ,[CreateYear]
                       ,[Regions]
                       ,[Address]
                       ,[Content]
                       ,[IsDel]
                       ,[CreateTime])
                 VALUES
                        (@ID
                       ,@SiteCode
                       ,@Photo
                       ,@Summary
                       ,@SaleRental
                       ,@Price
                       ,@HouseType
                       ,@Faces
                       ,@Area
                       ,@Renovation
                       ,@Floor
                       ,@UseType
                       ,@Buildings
                       ,@CreateYear
                       ,@Regions
                       ,@Address
                       ,@Content
                       ,@IsDel
                       ,@CreateTime)";
               System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@SiteCode", model.SiteCode),
                new System.Data.SqlClient.SqlParameter("@Photo", model.Photo),
                new System.Data.SqlClient.SqlParameter("@Summary", model.Summary),
                new System.Data.SqlClient.SqlParameter("@SaleRental", model.SaleRental),
                new System.Data.SqlClient.SqlParameter("@Price", model.Price),
                new System.Data.SqlClient.SqlParameter("@HouseType", model.HouseType),
                new System.Data.SqlClient.SqlParameter("@Faces", model.Faces),
                new System.Data.SqlClient.SqlParameter("@Area", model.Area),
                new System.Data.SqlClient.SqlParameter("@Renovation", model.Renovation),
                new System.Data.SqlClient.SqlParameter("@Floor", model.Floor),
                new System.Data.SqlClient.SqlParameter("@UseType",model.UseType),
                new System.Data.SqlClient.SqlParameter("@Buildings", model.Buildings),
                new System.Data.SqlClient.SqlParameter("@CreateYear", model.CreateYear),
                new System.Data.SqlClient.SqlParameter("@Regions", model.Regions),
                new System.Data.SqlClient.SqlParameter("@Address", model.Address),
                new System.Data.SqlClient.SqlParameter("@Content", model.Content),
                new System.Data.SqlClient.SqlParameter("@IsDel", model.IsDel),
                new System.Data.SqlClient.SqlParameter("@CreateTime", DateTime.Now)
            };
               return DbHelperSQL.ExecuteSql(sql, paras);
           }
           #endregion

       #region 更新房产信息
       /// <summary>
       /// 更新房产信息
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateDCHouse(DC_House model)
       {
           string safesql = "";
           safesql = "update DC_House set ";
           if (model.SiteCode != null && model.SiteCode != "")
           {
               safesql += "[SiteCode]='" + model.SiteCode + "',";
           }
           if (model.Photo != null&&model.Photo !="")
           {
               safesql += "[Photo]='" + model.Photo + "',";
           }
           if (model.Summary != null&&model.Summary !="")
           {
               safesql += "[Summary]='" + model.Summary + "',";
           }
           if (model.SaleRental > 0 && model.SaleRental.ToString() != "")
           {
               safesql += "[SaleRental]=" + model.SaleRental + ",";
           }
           if (model.Price > 0 && model.Price.ToString() != "")
           {
               safesql += "[Price]='" + model.Price + "',";
           }
           if (model.HouseType != null&&model.HouseType !="")
           {
               safesql += "[HouseType]='" + model.HouseType + "',";
           }
           if (model.Faces != null&&model.Faces !="")
           {
               safesql += "[Faces]='" + model.Faces + "',";
           }
           if (model.Area > 0 && model.Area.ToString() != "")
           {
               safesql += "[Area]='" + model.Area + "',";
           }
           if (model.Renovation != null&&model.Renovation !="")
           {
               safesql += "[Renovation]='" + model.Renovation + "',";
           }
           if (model.Floor != null&&model.Floor !="")
           {
               safesql += "[Floor]='" + model.Floor + "',";
           }
           if (model.UseType != null&&model.UseType !="")
           {
               safesql += "[UseType]='" + model.UseType + "',";
           }
           if (model.Buildings != null&&model.Buildings !="")
           {
               safesql += "[Buildings]='" + model.Buildings + "',";
           }
           if (model.CreateYear != null&&model.CreateYear !="")
           {
               safesql += "[CreateYear]='" + model.CreateYear + "',";
           }
           if (model.Regions != null&&model.Regions !="")
           {
               safesql += "[Regions]='" + model.Regions + "',";
           }
           if (model.Address != null&&model.Address !="")
           {
               safesql += "[Address]='" + model.Address + "',";
           }
           if (model.Content != null&&model.Content !="")
           {
               safesql += "[Content]='" + model.Content + "',";
           }
           safesql += "[IsDel]=" +model.IsDel+ ",";
           safesql += "[CreateTime]='" +DateTime.Now + "'";
           safesql += " where id='" + model.ID + "'";
           int rowsAffected = DbHelperSQL.ExecuteSql(safesql.ToString());
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

       #region 获取房产列表
       /// <summary>
       /// 获取房产列表
       /// </summary>
       /// <param name="where"></param>
       /// <returns></returns>
       public DataSet GetDCHouseList(string where)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select [ID],[Photo],[Summary],"+
               " CASE SaleRental WHEN 0 THEN '租赁' ELSE '出售' END AS SaleRental," +
               "[Price],[HouseType],[Faces],[Area],[Renovation],[FLOOR],[UseType],"+
               "[Buildings],[CreateYear],[Regions],[Address],[Content],[CreateTime],"+
               "CASE  IsDel WHEN 0 THEN '有效' ELSE '失效' END AS IsDel  ");
           strSql.Append(" FROM DC_House WHERE IsDel=0 ");
           if (where.Trim() != null && where.Trim() != "")
           {
               strSql.Append(" AND " + where);
           }
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion

       #region 多条件获取房产列表
       /// <summary>
       /// 多条件获取房产列表
       /// </summary>
       /// <param name="strSiteCode"></param>
       /// <param name="strSale"></param>
       /// <returns></returns>
       public DataSet GetDCHouseList(string strSiteCode, string strSale)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM DC_House ");
           strSql.Append(" WHERE IsDel=0 ");
           strSql.Append(" AND [SiteCode] = @SiteCode ");
           strSql.Append(" AND [SaleRental] = @SaleRental ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode),
                    new System.Data.SqlClient.SqlParameter("@SaleRental", strSale)
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion

       #region 根据房产编号获取相关列表信息
       /// <summary>
       /// 根据房产编号获取相关列表信息
       /// </summary>
       /// <param name="top">条数</param>
       /// <param name="houseID">编号ID</param>
       /// <returns></returns>
       public DataSet GetRelevantHouseList(int top,string houseID)
       {
           int hosSaleRental = 0; string siteCode = string.Empty;
           if (houseID.Trim() != null && houseID.Trim() != "")
           {
               try
               {
                   hosSaleRental = Convert.ToInt32(GetDCHouseByValue("SaleRental", houseID));               }
               catch (Exception)
               {
                   hosSaleRental = 0;
               }
               try
               {
                   siteCode = GetDCHouseByValue("SiteCode", houseID).ToString();
               }
               catch (Exception)
               {
                   siteCode = "";
               }
           }
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT TOP "+top+" * ");
           strSql.Append(" FROM DC_House ");
           strSql.Append(" WHERE IsDel=0 ");
           strSql.Append(" AND ID<>@ID ");
           strSql.Append(" AND [SaleRental] = @SaleRental ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", houseID),
                    new System.Data.SqlClient.SqlParameter("@SiteCode", siteCode),
                    new System.Data.SqlClient.SqlParameter("@SaleRental", hosSaleRental)
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion

       #region 返回相应的值
       /// <summary>
       /// 返回相应的值
       /// </summary>
       /// <param name="value"></param>
       /// <param name="DCHID"></param>
       /// <returns></returns>
       public object GetDCHouseByValue(string value, string DCHID)
       {
           object obj = null;
           if (value.Trim() != null && value.Trim() != "")
           {
               string safesql = " select " + value + " from DC_House where id='" + DCHID + "' ";
               obj = DbHelperSQL.GetSingle(safesql);
           }
           return obj;
       }
       #endregion

       #region 获取房产详细
       /// <summary>
       /// 获取房产详细
       /// </summary>
       /// <param name="DCHID"></param>
       /// <returns></returns>
       public DataSet GetDCHouseDetail(string DCHID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select [ID],[Photo],[Summary],[SaleRental],[SiteCode]," +
              "[Price],[HouseType],[Faces],[Area],[Renovation],[FLOOR],[UseType]," +
              "[Buildings],[CreateYear],[Regions],[Address],[Content],[CreateTime]");
           strSql.Append(" FROM DC_House ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", DCHID)
            };
           DataSet ds = DbHelperSQL.Query(strSql.ToString(), paras);
           return ds;
       }
        #endregion

       #region 更新信息状态
       /// <summary>
       /// 更新信息状态
       /// </summary>
       /// <param name="id"></param>
       /// <param name="state"></param>
       /// <returns></returns>
       public bool UpdateIsDel(string id, string state)
       {
           string safesql = "";
           if (id.Trim() != null && id.Trim() != "" && state.Trim() != null && state.Trim() != "")
           {
               safesql = "update DC_House set IsDel=" + state + " WHERE ID='" + id + "'";
           }
           int rowsAffected = 0;
           try
           {
               rowsAffected = DbHelperSQL.ExecuteSql(safesql.ToString());
           }
           catch (Exception)
           {
               rowsAffected = 0;
           }
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
