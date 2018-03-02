using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.MiniShop;
using Maticsoft.DBUtility;

namespace DAL.MiniShop
{
   public class MSProductParaDAL
    {
       public MSProductParaDAL() { ;}
       #region 添加产品参数
       /// <summary>
       /// 添加产品参数
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddMSPPara(MSProductPara model)
       {
           string sql = @"INSERT INTO [MS_ProductPara]
                        ([ID],[PID],[ParName],[Price],[Stock],[ParState],[AddTime])
                 VALUES
                        (@ID,@PID,@ParName,@Price,@Stock,@ParState,@AddTime)";
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@PID", model.PID),
                new System.Data.SqlClient.SqlParameter("@ParName", model.ParName),
                new System.Data.SqlClient.SqlParameter("@Price", model.Price),
                new System.Data.SqlClient.SqlParameter("@Stock", model.Stock>0?model.Stock:0),
                new System.Data.SqlClient.SqlParameter("@ParState",(model.ParState==1?1:0)),
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
       #region 更新产品参数
       /// <summary>
       /// 更新产品参数
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool UpdateMSPPara(MSProductPara model)
       {
           string safeslq = "";
           safeslq = "UPDATE MS_ProductPara SET ";
           if (model.PID != null && model.PID != "")
           {
               safeslq += "PID='" + model.PID + "',";
           }
           if (model.ParName != null && model.ParName != "")
           {
               safeslq += "ParName='" + model.ParName + "',";
           }
           if (model.Price != null && model.Price.ToString() != "")
           {
               safeslq += "Price='" + model.Price + "',";
           }
           safeslq += " Stock=" + (model.Stock>0?model.Stock:0) + ", ";
           safeslq += " ParState=" + (model.ParState == 1 ? 1 : 0) + " ";
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
       #region 获取产品参数值信息
       /// <summary>
       /// 获取产品参数值信息
       /// </summary>
       /// <param name="strValue"></param>
       /// <param name="strID"></param>
       /// <returns></returns>
       public object GetMSPParaValueByID(string strValue, string strID)
       {
           string safesql = "";
           safesql = "select " + strValue + " from MS_ProductPara where ID='" + strID + "'";
           return DbHelperSQL.GetSingle(safesql.ToString());
       }
       #endregion
       #region 更新参数状态
       /// <summary>
       /// 更新参数状态
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public bool UpdateMSPParaState(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           int state = 0;
           try
           {
               state = Convert.ToInt32(GetMSPParaValueByID("ParState", strID));
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
           strSql.Append(" UPDATE MS_ProductPara ");
           strSql.Append(" SET ParState = @ParState ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strID),
                new System.Data.SqlClient.SqlParameter("@ParState", state)
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
       #region 更新库存
       /// <summary>
       /// 更新库存
       /// </summary>
       /// <param name="strStock"></param>
       /// <param name="strID"></param>
       /// <returns></returns>
       public bool UpdateStock(int strStock, string strID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" UPDATE MS_ProductPara ");
           strSql.Append(" SET Stock = @Stock ");
           strSql.Append(" WHERE ID = @ID ");
           System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strID),
                new System.Data.SqlClient.SqlParameter("@Stock", strStock)
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
       #region 获取有效的参数列表
       /// <summary>
       /// 获取有效的参数列表
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetMSPParaList(string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select a.*,b.ShopName,c.Ptitle ");
           strSql.Append(" from MS_ProductPara a,MS_Shop b,MS_Product c,MS_ProductCategory d ");
           strSql.Append(" where a.ParState=0 and a.PID=c.ID and c.[SID]=b.ID ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by a.AddTime desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 获取有效的产品参数(产品)
       /// <summary>
       /// 获取有效的产品参数(产品)
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public DataSet GetParaList(string strWhere)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select *");
           strSql.Append(" from MS_ProductPara  ");
           strSql.Append(" where ParState=0 ");
           if (strWhere.Trim() != null && strWhere.Trim() != "")
           {
               strSql.Append("  " + strWhere);
           }
           strSql.Append(" order by AddTime desc ");
           DataSet ds = DbHelperSQL.Query(strSql.ToString());
           return ds;
       }
       #endregion
       #region 获取参数详细
       /// <summary>
       /// 获取参数详细
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       public DataSet GetParaDetail(string strID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM MS_ProductPara ");
           strSql.Append(" WHERE [ID] = @ID");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", strID)
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion
       #region 根据产品编号获取有效的产品参数
       /// <summary>
       /// 获取有效的产品参数
       /// </summary>
       /// <param name="strPID">产品编号</param>
       /// <returns></returns>
       public DataSet GetProductParamByPID(string strPID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" SELECT * ");
           strSql.Append(" FROM MS_ProductPara ");
           strSql.Append(" WHERE [PID] = @PID AND ParState=0 ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@PID", strPID)
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion
       #region 根据参数名称及编号判断信息是否存在
       /// <summary>
       /// 根据参数名称及编号判断信息是否存在
       /// </summary>
       /// <param name="strParName"></param>
       /// <param name="strPID"></param>
       /// <returns></returns>
       public bool ExistMSPPara(string strParName, string strPID)
       {
           string strSql = string.Empty;
           strSql += " SELECT count(ID) FROM MS_ProductPara ";
           if (strParName.Trim() != null && strParName.Trim() != "")
           {
               if (strSql.ToString().Trim().ToLower().Contains("where"))
               {
                   strSql += " and ";
               }
               else
               {
                   strSql += " where ";
               }
               strSql += " [ParName] ='" + strParName + "' ";
           }
           if (strPID.Trim() != null && strPID.Trim() != "")
           {
               if (strSql.ToString().Trim().ToLower().Contains("where"))
               {
                   strSql += " and ";
               }
               else
               {
                   strSql += " where ";
               }
               strSql += " [PID] ='" + strPID + "' ";
           }
           return DbHelperSQL.Exists(strSql.ToString());
       }
       #endregion
       #region 根据产品编号获取最大/最小 价格
       /// <summary>
       /// 根据产品编号获取最大/最小 价格
       /// </summary>
       /// <param name="strPID">产品编号</param>
       /// <returns></returns>
       public DataSet GetMaxMinPrice(string strPID)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" select PID,MAX(Price)Mprice,MIN(Price)Sprice from MS_ProductPara ");
           strSql.Append(" WHERE [PID] = @PID ");
           strSql.Append(" group by PID ");
           IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@PID", strPID)
                };
           return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
       }
       #endregion
    }
}
