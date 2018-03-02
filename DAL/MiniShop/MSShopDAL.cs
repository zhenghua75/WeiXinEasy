using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.MiniShop;
using Maticsoft.DBUtility;

namespace DAL.MiniShop
{
   public class MSShopDAL
    {
       public MSShopDAL() { ;}
       #region 店铺添加
       /// <summary>
       /// 店铺添加
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddMSShop(MSShop model)
       {
           string sql = @"INSERT INTO [MS_Shop]
                        ([ID],[UID],[ShopName],[WXName],[WXNum],[ShopLogo],[ShopBackImg],[ShopDesc],[ShopState],[AddTime])
                 VALUES
                        (@ID,@UID,@ShopName,@WXName,@WXNum,@ShopLogo,@ShopBackImg,@ShopDesc,@ShopState,@AddTime)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@UID", model.UID),
                new System.Data.SqlClient.SqlParameter("@ShopName", model.ShopName),
                new System.Data.SqlClient.SqlParameter("@WXName", model.WXName),
                new System.Data.SqlClient.SqlParameter("@WXNum", model.WXNum),
                new System.Data.SqlClient.SqlParameter("@ShopLogo", model.ShopLogo),
                new System.Data.SqlClient.SqlParameter("@ShopBackImg", model.ShopBackImg),
                new System.Data.SqlClient.SqlParameter("@ShopDesc", model.ShopDesc),
                new System.Data.SqlClient.SqlParameter("@ShopState",(model.ShopState==1?1:0)),
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
       #region 店铺更新
       /// <summary>
       /// 店铺更新
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateMSShop(MSShop model)
       {
           string safeslq = "";
           safeslq = "UPDATE MS_Shop SET ";
           if (model.UID != null && model.UID != "")
           {
               safeslq += "[UID]='" + model.UID + "',";
           }
           if (model.ShopName != null && model.ShopName != "")
           {
               safeslq += "ShopName='" + model.ShopName + "',";
           }
           if (model.WXName != null && model.WXName != "")
           {
               safeslq += "WXName='" + model.WXName + "',";
           }
           if (model.WXNum != null && model.WXNum != "")
           {
               safeslq += "WXNum='" + model.WXNum + "',";
           }
           if (model.ShopLogo != null && model.ShopLogo.ToString() != "")
           {
               safeslq += "ShopLogo='" + model.ShopLogo + "',";
           }
           if (model.ShopBackImg != null && model.ShopBackImg.ToString() != "")
           {
               safeslq += "ShopBackImg='" + model.ShopBackImg + "',";
           }
           if (model.ShopDesc != null && model.ShopDesc != "")
           {
               safeslq += "ShopDesc='" + model.ShopDesc + "',";
           }
           safeslq += " ShopState=" + (model.ShopState == 1 ? 1 : 0) + " ";
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
       #region 获取店铺属性值
       /// <summary>
       /// 获取店铺属性值
       /// </summary>
       /// <param name="strValue"></param>
       /// <param name="strID"></param>
       /// <returns></returns>
       public object GetMSShopValueByID(string strValue, string strID)
       {
           string safesql = "";
           safesql = "select " + strValue + " from MS_Shop where ID='" + strID + "'";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion
       #region 更新店铺状态
       /// <summary>
       /// 更新店铺状态
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public bool UpdateMSShopState(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetMSShopValueByID("ShopState", strID));
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
           strSql.Append(" UPDATE MS_Shop ");
           strSql.Append(" SET ShopState = @ShopState ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strID),
                new System.Data.SqlClient.SqlParameter("@ShopState", state)
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
       #region 获取有效店铺列表(包括用户电话)
       /// <summary>
       /// 获取有效店铺列表(包括用户电话)
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetMSShopList(string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select a.*,b.Phone from MS_Shop a,MS_Customers b where a.[UID]=b.ID ");
           strSql.Append(" and a.ShopState=0 and b.IsDel=0 ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by a.AddTime desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 获取有效店铺列表
       /// <summary>
       /// 获取有效店铺列表
       /// </summary>
       /// <param name="top">条数:0表示列出所有</param>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetShopList(int top,string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select  ");
           if (top > 0)
           {
               strSql.Append(" top "+top);
           }
           strSql.Append(" a.*,b.Phone from MS_Shop a,MS_Customers b where a.[UID]=b.ID  ");
           strSql.Append(" and a.ShopState=0 and b.IsDel=0 ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by a.AddTime desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 获取店铺详细
       /// <summary>
       /// 获取店铺详细
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public DataSet GetMSShopDetail(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM MS_Shop ");
           strSql.Append(" WHERE [ID] = @ID");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", strID)
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion
       #region 根据用户登录信息获取店铺详细
       /// <summary>
       /// 根据用户登录信息获取店铺详细
       /// </summary>
       /// <param name="strUID">用户编号</param>
       /// <returns></returns>
       public DataSet GetShopDetailByUID(string strUID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM MS_Shop ");
           strSql.Append(" WHERE [UID] = @UID");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@UID", strUID)
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion
       #region 判断店铺是否存在
       /// <summary>
       /// 判断店铺是否存在
       /// </summary>
       /// <param name="strShopName">店铺名称</param>
       /// <param name="strUID">用户编号</param>
       /// <returns></returns>
       public bool ExistMSShop(string strShopName,string strUID)
       {
           string strSql = string.Empty;
           //strSql += " SELECT count(a.ID) FROM MS_Shop a,MS_Customers b ";
           //strSql += " where a.ShopState=0 and b.IsDel=0 and a.[UID]=b.ID ";
           //if (strShopName.Trim() != null && strShopName.Trim() != "")
           //{
           //    strSql += " and a.[ShopName] ='" + strShopName + "' ";
           //}
           //if (strPhone.Trim() != null && strPhone.Trim() != "")
           //{
           //    strSql += " and b.[Phone] ='" + strPhone + "' ";
           //}

           strSql += " SELECT count(ID) FROM MS_Shop  ";
           strSql += " where ShopState=0  ";
           if (strShopName.Trim() != null && strShopName.Trim() != "")
           {
               strSql += " and [ShopName] ='" + strShopName + "' ";
           }
           if (strUID.Trim() != null && strUID.Trim() != "")
           {
               strSql += " and [UID] ='" + strUID + "' ";
           }
           return DbHelperSQL.Exists(strSql.ToString());
       }
       #endregion
       #region 根据用户编号获取店铺属性数据
       /// <summary>
       /// 根据用户编号获取店铺属性数据
       /// </summary>
       /// <param name="strValue"></param>
       /// <param name="strUID"></param>
       /// <returns></returns>
       public object GetSidByUid(string strValue,string strUID)
       {
           string safesql = "";
           safesql = "select " + strValue + " from MS_Shop where UID='" + strUID + "' and ShopState=0 ";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion
    }
}
