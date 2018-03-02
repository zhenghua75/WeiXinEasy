using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.DC;
using Maticsoft.DBUtility;

namespace DAL.DC
{
   public class DC_BuildingsDAL
    {
       public DC_BuildingsDAL() { ;}

       #region 添加小区
       /// <summary>
       /// 添加小区
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public int AddDCBuilding(DC_Building model)
       {
           string sql = @"INSERT INTO [DC_Buildings]
                        ([ID]
                       ,[SiteCode]
                       ,[Name]
                       ,[Photo]
                       ,[AVEPrice]
                       ,[GreenRate]
                       ,[VolumeRate]
                       ,[School]
                       ,[ParkingSpaces]
                       ,[PropertyDevelopers]
                       ,[PropertyCompany]
                       ,[Regions]
                       ,[BusLine]
                       ,[Address]
                       ,[Content]
                       ,[CreateTime]
                       ,[IsDel])
                 VALUES
                        (@ID
                       ,@SiteCode
                       ,@Name
                       ,@Photo
                       ,@AVEPrice
                       ,@GreenRate
                       ,@VolumeRate
                       ,@School
                       ,@ParkingSpaces
                       ,@PropertyDevelopers
                       ,@PropertyCompany
                       ,@Regions
                       ,@BusLine
                       ,@Address
                       ,@Content
                       ,@CreateTime
                       ,@IsDel)";
               System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@SiteCode", model.SiteCode),
                new System.Data.SqlClient.SqlParameter("@Name", model.Name),
                new System.Data.SqlClient.SqlParameter("@Photo", model.Photo),
                new System.Data.SqlClient.SqlParameter("@AVEPrice", model.AVEPrice),
                new System.Data.SqlClient.SqlParameter("@GreenRate", model.GreenRate),
                new System.Data.SqlClient.SqlParameter("@VolumeRate", model.VolumeRate),
                new System.Data.SqlClient.SqlParameter("@School", model.School),
                new System.Data.SqlClient.SqlParameter("@ParkingSpaces", model.ParkingSpaces),
                new System.Data.SqlClient.SqlParameter("@PropertyDevelopers", model.PropertyDevelopers),
                new System.Data.SqlClient.SqlParameter("@PropertyCompany", model.PropertyCompany),
                new System.Data.SqlClient.SqlParameter("@Regions", model.Regions),
                new System.Data.SqlClient.SqlParameter("@BusLine",model.BusLine),
                new System.Data.SqlClient.SqlParameter("@Address", model.Address),
                new System.Data.SqlClient.SqlParameter("@Content", model.Content),
                new System.Data.SqlClient.SqlParameter("@IsDel", model.IsDel),
                new System.Data.SqlClient.SqlParameter("@CreateTime", DateTime.Now)
            };
               return DbHelperSQL.ExecuteSql(sql, paras);
       }
       #endregion

       #region 更新小区信息
       /// <summary>
       /// 更新小区信息
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateDCBuilding(DC_Building model)
       {
           string safesql = "";
           safesql = "update DC_Buildings set ";
           if (model.SiteCode != null && model.SiteCode != "")
           {
               safesql += "[SiteCode]='" + model.SiteCode + "',";
           }
           if (model.Name != null && model.Name != "")
           {
               safesql += "[Name]='" + model.Name + "',";
           }
           if (model.Photo != null && model.Photo != "")
           {
               safesql += "[Photo]='" + model.Photo + "',";
           }
           if (model.AVEPrice > 0 && model.AVEPrice.ToString() != "")
           {
               safesql += "[AVEPrice]=" + model.AVEPrice + ",";
           }
           if (model.GreenRate > 0 && model.GreenRate.ToString() != "")
           {
               safesql += "[GreenRate]='" + model.GreenRate + "',";
           }
           if (model.VolumeRate > 0 && model.VolumeRate.ToString() != "")
           {
               safesql += "[VolumeRate]='" + model.VolumeRate + "',";
           }
           if (model.School != null && model.School != "")
           {
               safesql += "[School]='" + model.School + "',";
           }
           if (model.ParkingSpaces != null && model.ParkingSpaces.ToString() != "")
           {
               safesql += "[ParkingSpaces]='" + model.ParkingSpaces + "',";
           }
           if (model.PropertyDevelopers != null && model.PropertyDevelopers != "")
           {
               safesql += "[PropertyDevelopers]='" + model.PropertyDevelopers + "',";
           }
           if (model.PropertyCompany != null && model.PropertyCompany != "")
           {
               safesql += "[PropertyCompany]='" + model.PropertyCompany + "',";
           }
           if (model.Regions != null && model.Regions != "")
           {
               safesql += "[Regions]='" + model.Regions + "',";
           }
           if (model.BusLine != null && model.BusLine != "")
           {
               safesql += "[BusLine]='" + model.BusLine + "',";
           }
           if (model.Address != null && model.Address.ToString() != "")
           {
               safesql += "[Address]='" + model.Address + "',";
           }
           if (model.Content != null && model.Content != "")
           {
               safesql += "[Content]='" + model.Content + "',";
           }
           safesql += "[IsDel]=" + model.IsDel + ",";
           safesql += "[CreateTime]='" + DateTime.Now + "'";
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

       #region 获取小区列表
       /// <summary>
       /// 获取小区列表
       /// </summary>
       /// <param name="where"></param>
       /// <returns></returns>
       public DataSet GetDCBuildingList(string where)
       {
           //select [ID],[Name],[Photo],[AVEPrice],[GreenRate],[VolumeRate],[School],[ParkingSpaces],[PropertyDevelopers],[PropertyCompany],[Regions],[BusLine],[Address],[Content],[CreateTime],CASE  IsDel WHEN 0 THEN '失效' ELSE '有效' END AS IsDel from DC_Buildings
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT  * ");
           strSql.Append(" FROM DC_Buildings WHERE IsDel=0 ");
           if (where.Trim() != null && where.Trim() != "")
           {
               strSql.Append(" AND " + where);
           }
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
      
       #region 根据站点代码返回楼盘列表
       /// <summary>
       /// 根据站点代码返回楼盘列表
       /// </summary>
       /// <param name="strSiteCode"></param>
       /// <returns></returns>
       public DataSet GetDCBuildingListBySiteCode(string strSiteCode)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM DC_Buildings ");
           strSql.Append(" WHERE IsDel=0 ");
           strSql.Append(" AND [SiteCode] = @SiteCode ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", strSiteCode)
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion

       #region 获取相关楼盘信息
       /// <summary>
       /// 获取相关楼盘信息
       /// </summary>
       /// <param name="top"></param>
       /// <param name="buildingID"></param>
       /// <param name="strSiteCode"></param>
       /// <returns></returns>
       public DataSet GetDCBuildingList(int top,string buildingID)
       {
           StringBuilder strSql = new StringBuilder();
           string siteCode = string.Empty;
           try
           {
               siteCode = GetDCBuildingValue("SiteCode", buildingID).ToString();
           }
           catch (Exception)
           {
               siteCode = "";
           }
           strSql.Append(" SELECT TOP "+top+" * ");
           strSql.Append(" FROM DC_Buildings WHERE IsDel=0 ");
           strSql.Append(" AND [SiteCode] = @SiteCode ");
           strSql.Append(" AND ID<>@ID ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", siteCode),
                    new System.Data.SqlClient.SqlParameter("@ID", buildingID)
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion

       #region 返回相应的小区值
       /// <summary>
       /// 返回相应的小区值
       /// </summary>
       /// <param name="value"></param>
       /// <param name="DCBID"></param>
       /// <returns></returns>
       public object GetDCBuildingValue(string value, string DCBID)
       {
           object obj = null;
           if (value.Trim() != null && value.Trim() != "")
           {
               string safesql = " select " + value + " from DC_Buildings where id='" + DCBID + "' ";
               obj = DbHelperSQL.GetSingle(safesql);
           }
           return obj;
       }
       #endregion

       #region 获取小区详细
       /// <summary>
       /// 获取小区详细
       /// </summary>
       /// <param name="DCBID"></param>
       /// <returns></returns>
       public DataSet GetDCBuildingDetail(string DCBID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM DC_Buildings ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", DCBID)
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
       public bool UpdateIsDel(string id,string state)
       {
           string safesql = "";
           if (id.Trim() != null && id.Trim() != "" && state.Trim() != null && state.Trim() != "")
           {
               safesql = "update DC_Buildings set IsDel=" + state+" WHERE ID='"+id+"'";
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
