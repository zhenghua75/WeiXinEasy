using Model.WeiXin;
/* ==============================================================================
 * 类名称：NewsMsgDAL
 * 类描述：
 * 创建人：yhn,51809571@qq.com
 * 创建时间：2014/3/31 21:04:53
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
using Maticsoft.DBUtility;

namespace DAL.WeiXin
{
    public class NewsMsgDAL
    {
        /// <summary>
        /// 根据ID集获取图文消息列表，ID集以逗号分隔
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IList<NewsMsg> GetNewsMsgs(string ids)
        {
            IList<NewsMsg> res = new List<NewsMsg>();
            if (!string.IsNullOrEmpty(ids))
            {
                string sql = @"SELECT * FROM [WX_NewsMsg]
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
                        NewsMsg newsMsg = new NewsMsg()
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
                        res.Add(newsMsg);
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// 根据ID获取图文消息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public NewsMsg GetNewsMsg(string id)
        {
            NewsMsg res = null;
            if (!string.IsNullOrEmpty(id))
            {
                string sql = @"SELECT * FROM [WX_NewsMsg]
                        WHERE ID = @ID";
                IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@ID", id)
                };
                DataSet ds = DbHelperSQL.Query(sql, paras.ToArray());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    res = new NewsMsg()
                    {
                        ID = dr.GetColumnValue("ID", string.Empty),
                        Title = dr.GetColumnValue("Title", string.Empty),
                        WXConfigID = dr.GetColumnValue("WXConfigID", string.Empty),
                        Description = dr.GetColumnValue("Description", string.Empty),
                        PicUrl = dr.GetColumnValue("PicUrl", string.Empty),
                        Url = dr.GetColumnValue("Url", string.Empty),
                        Remark = dr.GetColumnValue("Remark", string.Empty),
                        AddTime = dr.GetColumnValue("AddTime", DateTime.Now)
                    };;
                }
            }
            return res;
        }
    }
}
