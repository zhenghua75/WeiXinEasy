using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.MiniShop;
using Maticsoft.DBUtility;

namespace DAL.MiniShop
{
   public class MSProductAtlasDAL
    {
       public MSProductAtlasDAL() { ;}
       #region 添加图像
       /// <summary>
       /// 添加图像
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddMSProductAtlas(MSProductAtlas model)
       {
           string sql = @"INSERT INTO [MS_ProductAtlas]
                        ([ID],[PID],[AtlasName],[PimgUrl],[ImgState],[IsDefault],[AddTime])
                 VALUES
                        (@ID,@PID,@AtlasName,@PimgUrl,@ImgState,@IsDefault,@AddTime)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@PID", model.PID),
                new System.Data.SqlClient.SqlParameter("@AtlasName", model.AtlasName),
                new System.Data.SqlClient.SqlParameter("@PimgUrl", model.PimgUrl),
                new System.Data.SqlClient.SqlParameter("@ImgState",(model.ImgState==0?0:1)),
                new System.Data.SqlClient.SqlParameter("@IsDefault",(model.IsDefault==0?0:1)),
                new System.Data.SqlClient.SqlParameter("@AddTime", DateTime.Now)
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
       #region 更新图像
       /// <summary>
       /// 更新图像
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateMSProductAtlas(MSProductAtlas model)
       {
           string safeslq = "";
           safeslq = "UPDATE MS_ProductAtlas SET ";
           if (model.PID != null && model.PID != "")
           {
               safeslq += "PID='" + model.PID + "',";
           }
           if (model.AtlasName != null && model.AtlasName != "")
           {
               safeslq += "AtlasName='" + model.AtlasName + "',";
           }
           if (model.PimgUrl != null && model.PimgUrl != "")
           {
               safeslq += "PimgUrl='" + model.PimgUrl + "',";
           }
           safeslq += " ImgState=" + (model.ImgState == 1 ? 1 : 0) + ", ";
           safeslq += " IsDefault=" + (model.IsDefault == 1 ? 1 : 0) + " ";
           safeslq += " where ID='" + model.ID + "'";
           int rowsAffected = DbHelperSQL.ExecuteSql(safeslq.ToString());
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
       #region 获取图像属性值
       /// <summary>
       /// 获取图像属性值
       /// </summary>
       /// <param name="strValue"></param>
       /// <param name="strID"></param>
       /// <returns></returns>
       public object GetMSProductAtlasValueByID(string strValue,string strID)
       {
           string safesql = "";
           safesql = "select " + strValue + " from MS_ProductAtlas where ID='" + strID + "'";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion
       #region 更新图像状态
       /// <summary>
       /// 更新图像状态
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public bool UpdateMSProductAtlasState(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetMSProductAtlasValueByID("ImgState", strID));
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
           strSql.Append(" UPDATE MS_ProductAtlas ");
           strSql.Append(" SET ImgState = @ImgState ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strID),
                new System.Data.SqlClient.SqlParameter("@ImgState", state)
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
       #region 获取有效产品图集列表
       /// <summary>
       /// 获取有效图产品集列表
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetMSProductAtlasList(string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT a.ID,a.PID,a.AtlasName,a.PimgUrl,a.ImgState,a.AddTime,");
           strSql.Append(" case a.IsDefault when 0 then '否' when 1 then '是' end as IsDefault, ");
           strSql.Append(" b.ShopName,c.Ptitle ");
           strSql.Append(" from MS_ProductAtlas a,MS_Shop b,MS_Product c,MS_ProductCategory d  ");
           strSql.Append(" where a.ImgState=0 and a.PID=c.ID ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by a.AddTime desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 获取有效的产品图集
       /// <summary>
       /// 获取有效的产品图集
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetProductAtlasList(string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT a.ID,a.PID,a.AtlasName,a.PimgUrl,a.ImgState,a.AddTime,");
           strSql.Append(" case a.IsDefault when 0 then '否' when 1 then '是' end as IsDefault ");
           strSql.Append(" from MS_ProductAtlas a ");
           strSql.Append(" where a.ImgState=0 ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by a.AddTime desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 获取图集详细
       /// <summary>
       /// 获取图集详细
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public DataSet GetAtlasDetail(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM MS_ProductAtlas ");
           strSql.Append(" WHERE [ID] = @ID");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", strID)
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion
       #region 设置图片首页展示
       /// <summary>
       /// 设置图片首页展示
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public bool UpdateIsDefault(string strID)
       {
           string safeslq =string.Empty;
           string pid = string.Empty;
           pid = GetMSProductAtlasValueByID("[PID]", strID).ToString();
           safeslq = "UPDATE MS_ProductAtlas SET IsDefault=0 WHERE PID='" + pid+ "' ";
           DbHelperSQL.ExecuteSql(safeslq.ToString());
           safeslq = string.Empty;
           safeslq = "UPDATE MS_ProductAtlas SET IsDefault=1 WHERE ID='" + strID + "' ";
           int rowsAffected = DbHelperSQL.ExecuteSql(safeslq.ToString());
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
       #region 根据产品编号获取首页展示图
       /// <summary>
       /// 根据产品编号获取首页展示图
       /// </summary>
       /// <param name="strPID"></param>
       /// <returns></returns>
       public string GetIsDefaultAtlas(string strPID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM MS_ProductAtlas ");
           strSql.Append(" WHERE [PID] = @PID AND IsDefault=1 ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@PID", strPID)
                };
           return DbHelperSQL.GetSingle(strSql.ToString(), paras.ToArray()).ToString();
       }
       #endregion
       #region 获取有效的图集
       /// <summary>
       /// 获取有效的图集
       /// </summary>
       /// <param name="strPID">产品编号</param>
       /// <returns></returns>
       public DataSet GetProductAtlasByPID(string strPID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM MS_ProductAtlas ");
           strSql.Append(" WHERE [PID] = @PID and ImgState=0 ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@PID", strPID)
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion
       #region 获取一条首页展示图
       /// <summary>
       /// 获取一条首页展示图
       /// </summary>
       /// <param name="strPID">产品编号</param>
       /// <returns></returns>
       public DataSet GetDefaultAtlasByPid(string strPID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT top 1 * ");
           strSql.Append(" FROM MS_ProductAtlas ");
           strSql.Append(" WHERE [PID] = @PID AND IsDefault=1 ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@PID", strPID)
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion
       #region 更新二手产品状态
       /// <summary>
       /// 更新二手产品状态
       /// </summary>
       /// <param name="strPID">产品编号</param>
       /// <returns></returns>
       public bool UpdateAtlasByPID(string strPID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" UPDATE MS_ProductAtlas ");
           strSql.Append(" SET ImgState = 1 ");
           strSql.Append(" WHERE PID = @PID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@PID", strPID)
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
       #region 获取是否有默认展示图
       /// <summary>
       /// 获取是否有默认展示图
       /// </summary>
       /// <param name="strPid">产品编号</param>
       /// <returns></returns>
       public bool IsExitDefaultImg(string strPid)
       {
           string strSql = string.Empty;
           strSql += " SELECT count(ID) FROM MS_ProductAtlas where IsDefault=1 and ImgState=0 ";
           if (strPid.Trim() != null && strPid.Trim() != "")
           {
               strSql += " and [PID] ='" + strPid + "' ";
           }
           return DbHelperSQL.Exists(strSql.ToString());
       }
       #endregion
    }
}
