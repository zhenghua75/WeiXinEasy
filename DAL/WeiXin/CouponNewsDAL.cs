using Maticsoft.DBUtility;
using Model.WeiXin;
/* ==============================================================================
 * 类名称：CouponNewsDAL
 * 类描述：
 * 创建人：yhn,51809571@qq.com
 * 创建时间：2014/4/5 11:09:42
 * 修改人：
 * 修改时间：
 * 修改备注：
 * @version 1.0
 * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBUtility;

namespace DAL.WeiXin
{
    public class CouponNewsDAL
    {
        /// <summary>
        /// 根据ID集获取优惠券图文消息列表，ID集以逗号分隔
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IList<CouponNews> GetCouponNewsList(string ids)
        {
            IList<CouponNews> res = new List<CouponNews>();
            if (!string.IsNullOrEmpty(ids))
            {
                string sql = @"SELECT * FROM [WX_CouponNews]
                        WHERE ID IN @IDs";
                IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@IDs", ids)
                };
                DataSet ds = DbHelperSQL.Query(sql, paras.ToArray());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        CouponNews couponNews = new CouponNews()
                        {
                            ID = dr.GetColumnValue("ID", string.Empty),
                            Title = dr.GetColumnValue("Title", string.Empty),
                            WXConfigID = dr.GetColumnValue("WXConfigID", string.Empty),
                            Description = dr.GetColumnValue("Description", string.Empty),
                            PicUrl = dr.GetColumnValue("PicUrl", string.Empty),
                            Url = dr.GetColumnValue("Url", string.Empty),
                            Remark = dr.GetColumnValue("Remark", string.Empty),
                            AddTime = dr.GetColumnValue("AddTime", DateTime.Now)
                        };
                        res.Add(couponNews);
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// 根据ID获取优惠券图文消息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CouponNews GetCouponNews(string id)
        {
            CouponNews res = null;
            if (!string.IsNullOrEmpty(id))
            {
                string sql = @"SELECT * FROM [WX_CouponNews]
                        WHERE ID = @ID";
                IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", id)
                };
                DataSet ds = DbHelperSQL.Query(sql, paras.ToArray());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    res = new CouponNews()
                    {
                        ID = dr.GetColumnValue("ID", string.Empty),
                        Title = dr.GetColumnValue("Title", string.Empty),
                        WXConfigID = dr.GetColumnValue("WXConfigID", string.Empty),
                        Description = dr.GetColumnValue("Description", string.Empty),
                        PicUrl = dr.GetColumnValue("PicUrl", string.Empty),
                        Url = dr.GetColumnValue("Url", string.Empty),
                        Remark = dr.GetColumnValue("Remark", string.Empty),
                        AddTime = dr.GetColumnValue("AddTime", DateTime.Now)
                    }; ;
                }
            }
            return res;
        }
    }
}
