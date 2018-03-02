using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using Model.MiniShop;
using Maticsoft.DBUtility;

namespace DAL.MiniShop
{
   public class MSProductDAL
    {
       public MSProductDAL() { ;}
       #region 添加产品
       /// <summary>
       /// 添加产品
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddMSProduct(MSProduct model)
       {
           string sql = @"INSERT INTO [MS_Product]
                        ([ID],[CustomerID],[SiteCode],[SID],[Cid],[Ptitle],[Pcontent],[Price],[IsSecHand],
                          [Pstate],[Review],[ZipCode],[AddTime])
                 VALUES
                        (@ID,@CustomerID,@SiteCode,@SID,@Cid,@Ptitle,@Pcontent,@Price,@IsSecHand,@Pstate,@Review,@ZipCode,@AddTime)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@CustomerID", model.CustomerID),
                new System.Data.SqlClient.SqlParameter("@SiteCode", model.SiteCode),
                new System.Data.SqlClient.SqlParameter("@SID", model.SID),
                new System.Data.SqlClient.SqlParameter("@Cid", model.Cid),
                new System.Data.SqlClient.SqlParameter("@Ptitle", model.Ptitle),
                new System.Data.SqlClient.SqlParameter("@Pcontent", model.Pcontent),
                new System.Data.SqlClient.SqlParameter("@Price",model.Price),
                new System.Data.SqlClient.SqlParameter("@IsSecHand", (model.IsSecHand==1?1:0)),
                new System.Data.SqlClient.SqlParameter("@Pstate",(model.Pstate==1?1:0)),
                new System.Data.SqlClient.SqlParameter("@Review",(model.Review==1?1:0)),
                new System.Data.SqlClient.SqlParameter("@ZipCode",model.ZipCode),
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
       #region 产品信息修改
       /// <summary>
       /// 产品信息修改
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateMSProduct(MSProduct model)
       {
           string safeslq = "";
           safeslq = "UPDATE MS_Product SET ";
           if (model.SiteCode != null && model.SiteCode != "")
           {
               safeslq += "SiteCode='" + model.SiteCode + "',";
           }
           if (model.CustomerID != null && model.CustomerID != "")
           {
               safeslq += "CustomerID='" + model.CustomerID + "',";
           }
           if (model.SID != null && model.SID != "")
           {
               safeslq += "SID='" + model.SID + "',";
           }
           if (model.Cid != null && model.Cid.ToString() != "")
           {
               safeslq += "Cid='" + model.Cid + "',";
           }
           if (model.Ptitle != null && model.Ptitle != "")
           {
               safeslq += "Ptitle='" + model.Ptitle + "',";
           }
           if (model.Pcontent != null && model.Pcontent != "")
           {
               safeslq += "Pcontent='" + model.Pcontent + "',";
           }
           if (model.Price != null && model.Price.ToString() != "")
           {
               safeslq += "Price='" + model.Price + "',";
           }
           if (model.ZipCode != null && model.ZipCode.ToString() != "")
           {
               safeslq += "ZipCode='" + model.ZipCode + "',";
           }
           safeslq += " Pstate=" + (model.Pstate ==1?1 :0) + " ";
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
       #region 根据编号获取相对应的值
       /// <summary>
       /// 根据编号获取相对应的值
       /// </summary>
       /// <param name="strValue"></param>
       /// <param name="strID"></param>
       /// <returns></returns>
       public object GetMSProductVaueByID(string strValue, string strID)
       {
           string safesql = "";
           safesql = "select " + strValue + " from MS_Product where ID='" + strID + "'";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion
       #region 更新产品状态
       /// <summary>
       /// 更新产品状态
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public bool UpdateMSProductState(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetMSProductVaueByID("Pstate", strID));
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
           strSql.Append(" UPDATE MS_Product ");
           strSql.Append(" SET Pstate = @Pstate ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strID),
                new System.Data.SqlClient.SqlParameter("@Pstate", state)
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
       #region 审核产品
       /// <summary>
       /// 审核产品
       /// </summary>
       /// <param name="strPID">产品编号</param>
       /// <returns></returns>
       public bool UpdateProductReview(string strPID)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetMSProductVaueByID("Review", strPID));
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
           strSql.Append(" UPDATE MS_Product ");
           strSql.Append(" SET Review = @Review ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strPID),
                new System.Data.SqlClient.SqlParameter("@Review", state)
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
       #region 获取有效的产品列表
       /// <summary>
       /// 获取有效的产品列表
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetMSProductList(string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT a.ID,a.SiteCode,a.[SID],a.Cid,a.Ptitle,a.Pcontent,a.[Price],a.[ZipCode], ");
           strSql.Append(" case a.IsSecHand when 1 then '是' when 0 then '否' end IsSecHand,a.Review, ");
           strSql.Append(" a.[Pstate],a.[AddTime],b.ShopName,b.ShopLogo,c.Cname,  ");
           strSql.Append(" (select top 1 PimgUrl from MS_ProductAtlas where PID=a.ID and IsDefault=1) PimgUrl ");
           strSql.Append(" from MS_Product a,MS_Shop b,MS_ProductCategory c ");
           strSql.Append(" where a.Pstate=0 and a.[SID]=b.ID and a.Cid=c.ID ");
           if (strWhere.Trim() != null && strWhere.Trim()!="")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by a.AddTime desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 获取有效的产品列表
       /// <summary>
       /// 获取有效的产品列表
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetProductList(string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           //strSql.Append(" SELECT a.ID,a.SiteCode,a.[SID],a.Cid,a.Ptitle,a.Pcontent,a.[Price],a.[ZipCode], ");
           //strSql.Append(" a.IsSecHand, ");
           //strSql.Append(" a.[Pstate],a.[AddTime],b.ShopName,b.ShopLogo,c.Cname,  ");
           //strSql.Append(" (select top 1 PimgUrl from MS_ProductAtlas where PID=a.ID and IsDefault=1) PimgUrl ");
           //strSql.Append(" from MS_Product a,MS_Shop b,MS_ProductCategory c ");
           //strSql.Append(" where a.Pstate=0 and a.[SID]=b.ID and a.Cid=c.ID ");
           strSql.Append(" SELECT a.ID,a.SiteCode,a.[SID],a.Cid,a.Ptitle,a.Pcontent,a.[Price],a.[ZipCode], ");
           strSql.Append(" a.IsSecHand, ");
           strSql.Append(" a.[Pstate],a.[AddTime],b.ShopName,b.ShopLogo, ");
           strSql.Append(" (select top 1 PimgUrl from MS_ProductAtlas where PID=a.ID and IsDefault=1) PimgUrl ");
           strSql.Append(" from MS_Product a,MS_Shop b ");
           strSql.Append(" where a.Pstate=0 and a.[SID]=b.ID and a.Review=1 ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by a.AddTime desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 获取有效的产品列表(根据条数查询)
       /// <summary>
       /// 获取有效的产品列表(根据条数查询)
       /// </summary>
       /// <param name="top"></param>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetProductListByTop(int top, string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT ");
           if (top > 0)
           {
               strSql.Append(" Top " + top);
           }
           strSql.Append(" a.ID,a.SiteCode,a.[SID],a.Cid,a.Ptitle,a.Pcontent,a.[Price],a.[ZipCode], ");
           strSql.Append(" a.IsSecHand, ");
           strSql.Append(" a.[Pstate],a.[AddTime],b.ShopName,b.ShopLogo, ");
           strSql.Append(" (select top 1 PimgUrl from MS_ProductAtlas where PID=a.ID and IsDefault=1) PimgUrl ");
           strSql.Append(" from MS_Product a,MS_Shop b ");
           strSql.Append(" where a.Pstate=0 and a.[SID]=b.ID and a.Review=1 ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by a.AddTime desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 获取有效二手产品列表
       /// <summary>
       /// 获取有效二手产品列表
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetSecHandProduct(string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT a.ID,a.CustomerID,a.SiteCode,a.[SID],a.Cid,a.Ptitle,a.Pcontent,a.[Price],a.[ZipCode], ");
           strSql.Append(" a.IsSecHand, a.[Pstate],a.Review,a.[AddTime],b.NickName,b.Phone,c.Cname,  ");
           strSql.Append(" (select NickName from MS_ShopContacts where PID=a.ID) ContUserName, ");
           strSql.Append(" (select Phone from MS_ShopContacts where PID=a.ID) ContUserPhone, ");
           strSql.Append(" case when (select top 1 PimgUrl from MS_ProductAtlas where PID=a.ID and IsDefault=1) ");
           strSql.Append(" is null then 'ShopLogo/default.png' ");
           strSql.Append(" else (select top 1 PimgUrl from MS_ProductAtlas where PID=a.ID and IsDefault=1) end PimgUrl ");
           strSql.Append(" from MS_Product a,MS_Customers b,MS_ProductCategory c ");
           strSql.Append(" where a.Pstate=0 and a.IsSecHand=1 and a.Cid=c.ID and a.CustomerID=b.ID and a.review=1 ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by a.AddTime desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 获取有效产品列表
       /// <summary>
       /// 获取有效产品列表
       /// </summary>
       /// <param name="top">条数</param>
       /// <param name="strWhere">条件</param>
       /// <returns></returns>
       public DataSet GetProductByTop(int top,string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT ");
           if (top > 0)
           {
               strSql.Append(" Top " + top);
           }
           strSql.Append("  a.ID,a.CustomerID,a.SiteCode,a.[SID],a.Cid,a.Ptitle,a.Pcontent,a.[Price],a.[ZipCode], ");
           strSql.Append(" a.IsSecHand, a.[Pstate],a.Review,a.[AddTime],b.NickName,b.Phone,  ");
           strSql.Append(" (select NickName from MS_ShopContacts where PID=a.ID) ContUserName, ");
           strSql.Append(" (select Phone from MS_ShopContacts where PID=a.ID) ContUserPhone, ");
           strSql.Append(" case when (select top 1 PimgUrl from MS_ProductAtlas where PID=a.ID and IsDefault=1) ");
           strSql.Append(" is null then 'ShopLogo/default.png' ");
           strSql.Append(" else (select top 1 PimgUrl from MS_ProductAtlas where PID=a.ID and IsDefault=1) end PimgUrl ");
           strSql.Append(" from MS_Product a,MS_Customers b ");
           strSql.Append(" where a.Pstate=0  and a.CustomerID=b.ID and a.Review=1");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by a.AddTime desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 获取产品详细
       /// <summary>
       /// 获取产品详细
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public DataSet GetProductDetail(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM MS_Product ");
           strSql.Append(" WHERE [ID] = @ID");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", strID)
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion
       #region 根据标题、类别、店铺判断产品是否存在
       /// <summary>
       /// 根据标题、类别、店铺判断产品是否存在
       /// </summary>
       /// <param name="strSiteCode"></param>
       /// <param name="strPtitle"></param>
       /// <param name="strCID"></param>
       /// <param name="strSID"></param>
       /// <returns></returns>
       public bool ExistMSProduct(string strPtitle, string strCID, string strSID)
       {
           string strSql = string.Empty;
           strSql +=" SELECT count(ID) FROM MS_Product ";
           if (strPtitle.Trim() != null && strPtitle.Trim() != "")
           {
               if (strSql.ToString().Trim().ToLower().Contains("where"))
               {
                   strSql += " and ";
               }
               else
               {
                   strSql += " where ";
               }
               strSql += " [Ptitle] ='" + strPtitle + "' ";
           }
           if (strCID.Trim() != null && strCID.Trim() != "")
           {
               if (strSql.ToString().Trim().ToLower().Contains("where"))
               {
                   strSql += " and ";
               }
               else
               {
                   strSql += " where ";
               }
               strSql += " [CID] ='" + strCID + "' ";
           }
           if (strSID.Trim() != null && strSID.Trim() != "")
           {
               if (strSql.ToString().Trim().ToLower().Contains("where"))
               {
                   strSql += " and ";
               }
               else
               {
                   strSql += " where ";
               }
               strSql += " [SID] ='" + strSID + "' ";
           }
           return DbHelperSQL.Exists(strSql.ToString());
       }
       #endregion
       #region 获取有效产品总数
       /// <summary>
       /// 获取有效产品总数
       /// </summary>
       /// <param name="strWhere">条件</param>
       /// <returns></returns>
       public int GetNotPassProductCount(string strWhere)
       {
           string strSql = string.Empty;
           strSql += " SELECT count(ID) FROM MS_Product where Pstate=0 ";
           if (strWhere != null && strWhere != "")
           {
               strSql += strWhere;
           }
           int count = 0;
           try
           {
               count = Convert.ToInt32(DbHelperSQL.GetSingle(strSql.ToString()));
           }
           catch (Exception)
           {
              count = 0;
           }
           return count;
       }
       #endregion
       #region 获取产品用户列表
       /// <summary>
       /// 获取产品用户列表
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetProductCustomerList(string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select distinct(b.NickName) NickName,b.ID,b.Phone,b.UserPwd,b.Email,b.QQnum,b.HeadImg,"+
               "b.Pnote,b.IsDel,b.AddTime,case b.Sex when 0 then '男' when 1 then '女' end Sex ");
           strSql.Append(" from MS_Product a,MS_Customers b,MS_ProductCategory c ");
           strSql.Append(" where a.Pstate=0 and b.IsDel=0 and a.CustomerID=b.ID and a.Cid=c.ID ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by b.AddTime desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion

       #region 获取站点产品列表
       /// <summary>
       /// 获取站点产品列表
       /// </summary>
       /// <param name="strSiteCode">站点ID</param>
       /// <returns></returns>
       public DataSet GetProductListBySiteCode(string strSiteCode)
       {
           StringBuilder strSql = new StringBuilder();
           string sql = @"SELECT a.ID,a.Ptitle,a.Price,b.PimgUrl 
                          FROM MS_Product a
                          LEFT JOIN MS_ProductAtlas b ON (b.PID = a.ID)
                          WHERE a.SiteCode = @strSiteCode";
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@strSiteCode", strSiteCode)
                };
           DataSet ds = DbHelperSQL.Query(sql, paras.ToArray());
           return ds;
       }
       #endregion


       #region 获取站点产品列表
       /// <summary>
       /// 获取站点产品列表
       /// </summary>
       /// <param name="strProductID">商品ID</param>
       /// <returns></returns>
       public DataSet GetProductByID(string strProductID)
       {
           StringBuilder strSql = new StringBuilder();
           string sql = @"SELECT a.ID,a.Ptitle,a.Price,b.PimgUrl 
                          FROM MS_Product a
                          LEFT JOIN MS_ProductAtlas b ON (b.PID = a.ID)
                          WHERE a.ID = @strProductID";
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@strProductID", strProductID)
                };
           DataSet ds = DbHelperSQL.Query(sql, paras.ToArray());
           return ds;
       }
       #endregion
    }
}
