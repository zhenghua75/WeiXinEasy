using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.MiniShop;
using Maticsoft.DBUtility;

namespace DAL.MiniShop
{
    /// <summary>
    /// 产品颜色、尺寸、库存 类
    /// </summary>
    public class MSSizeOrColorDAL
    {
        public MSSizeOrColorDAL() { ;}
        #region 添加产品 颜色尺寸
        /// <summary>
        /// 添加产品 颜色尺寸
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddSizeOrColor(MSSizeOrColor model)
        {
            string sql = @"INSERT INTO [MS_SizeOrColor]
                        ([ID],[SizeColor],[PID],[Scname],[Scimg],[Stock],[IsDel],[AddTime])
                 VALUES
                        (@ID,@SizeColor,@PID,@Scname,@Scimg,@Stock,@IsDel,@AddTime)";
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", model.ID),
                new System.Data.SqlClient.SqlParameter("@SizeColor", (model.SizeColor==0?0:model.SizeColor)),
                new System.Data.SqlClient.SqlParameter("@PID", model.PID),
                new System.Data.SqlClient.SqlParameter("@Scname", model.Scname),
                new System.Data.SqlClient.SqlParameter("@Scimg", model.Scimg),
                new System.Data.SqlClient.SqlParameter("@Stock",(model.Stock==0?0:model.Stock)),
                new System.Data.SqlClient.SqlParameter("@IsDel",(model.IsDel==1?1:0)),
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
        #region 更新产品 颜色尺寸
        /// <summary>
        /// 更新产品 颜色尺寸
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateSizeOrColor(MSSizeOrColor model)
        {
            string safeslq = "";
            safeslq = "UPDATE MS_SizeOrColor SET ";
            if (model.PID != null && model.PID != "")
            {
                safeslq += "PID='" + model.PID + "',";
            }
            if (model.Scname != null && model.Scname != "")
            {
                safeslq += "Scname='" + model.Scname + "',";
            }
            if (model.Scimg != null && model.Scimg != "")
            {
                safeslq += "Scimg='" + model.Scimg + "',";
            }
            safeslq += " SizeColor=" + (model.SizeColor == 0 ? 0 : model.SizeColor) + ", ";
            safeslq += " Stock=" + (model.Stock == 0 ? 0 : model.Stock) + ", ";
            safeslq += " IsDel=" + (model.IsDel == 1 ? 1 : 0) + " ";
            safeslq += " where ID='" + model.ID + "' ";
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
        #region 获取产品 颜色尺寸 属性
        /// <summary>
        /// 获取产品 颜色尺寸 属性
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="strID"></param>
        /// <returns></returns>
        public object GetSizeOrColorValueByID(string strValue, string strID)
        {
            string safesql = "";
            safesql = "select " + strValue + " from MS_SizeOrColor where ID='" + strID + "'";
            return DbHelperSQL.GetSingle(safesql.ToString());
        }
        #endregion
        #region 更新产品 颜色尺寸 状态
        /// <summary>
        /// 更新产品 颜色尺寸 状态
        /// </summary>
        /// <param name="strID"></param>
        /// <returns></returns>
        public bool UpdateSizeOrColorState(string strID)
        {
            StringBuilder strSql = new StringBuilder();
            int state = 0;
            try
            {
                state = Convert.ToInt32(GetSizeOrColorValueByID("IsDel", strID));
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
            strSql.Append(" UPDATE MS_SizeOrColor ");
            strSql.Append(" SET IsDel = @IsDel ");
            strSql.Append(" WHERE ID = @ID ");
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", strID),
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
        #region 获取产品 颜色尺寸 有效列表
        /// <summary>
        /// 获取产品 颜色尺寸 有效列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetSizeOrColorList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select a.ID,a.PID,a.Scname,a.Scimg,a.Stock,a.IsDel,a.AddTime, ");
            strSql.Append(" case a.SizeColor when 0 then '' when 1 then '尺寸' when -1 then '颜色' end  SizeColor, ");
            strSql.Append(" b.Ptitle,c.ShopName,d.Cname ");
            strSql.Append(" from MS_SizeOrColor a,MS_Product b,MS_Shop c,MS_ProductCategory d ");
            strSql.Append(" where a.IsDel=0 and a.PID=b.ID and b.[SID]=c.ID and b.Cid=d.ID ");
            if (strWhere.Trim() != null && strWhere.Trim() != "")
            {
                strSql.Append("  " + strWhere);
            }
            strSql.Append(" order by a.AddTime desc ");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion
        #region 根据产品编号获取有效的 颜色尺寸列表
        /// <summary>
        /// 根据产品编号获取有效的 颜色尺寸列表
        /// </summary>
        /// <param name="strPID">产品编号</param>
        /// <returns></returns>
        public DataSet GetSizeOrColorListByPID(string strPID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM MS_SizeOrColor ");
            strSql.Append(" WHERE IsDel=0 AND [PID] = @PID");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@PID", strPID)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion
        #region 获取产品 颜色尺寸 详细
        /// <summary>
        /// 获取产品 颜色尺寸 详细
        /// </summary>
        /// <param name="strID"></param>
        /// <returns></returns>
        public DataSet GetSizeOrColorDetail(string strID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM MS_SizeOrColor ");
            strSql.Append(" WHERE [ID] = @ID");
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", strID)
                };
            return DbHelperSQL.Query(strSql.ToString(), paras.ToArray());
        }
        #endregion
        #region 判断产品 颜色尺寸 是否存在
        /// <summary>
        /// 判断产品 颜色尺寸 是否存在
        /// </summary>
        /// <param name="SizeOrColor">颜色或尺寸</param>
        /// <param name="strScname">名称</param>
        /// <param name="strPID">产品编号</param>
        /// <returns></returns>
        public bool ExistSizeOrColor(int SizeOrColor, string strScname, string strPID)
        {
            string strSql = string.Empty;
            strSql += " SELECT count(ID) FROM MS_SizeOrColor ";
            if (SizeOrColor.ToString().Trim() != null && SizeOrColor.ToString().Trim() != "")
            {
                if (strSql.ToString().Trim().ToLower().Contains("where"))
                {
                    strSql += " and ";
                }
                else
                {
                    strSql += " where ";
                }
                strSql += " [SizeColor] =" + SizeOrColor + " ";
            }
            if (strScname.Trim() != null && strScname.Trim() != "")
            {
                if (strSql.ToString().Trim().ToLower().Contains("where"))
                {
                    strSql += " and ";
                }
                else
                {
                    strSql += " where ";
                }
                strSql += " [strScname] ='" + strScname + "' ";
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
    }
}
