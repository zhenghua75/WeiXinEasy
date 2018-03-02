using DAL.Common;
using Model.WeiXin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Maticsoft.DBUtility;

namespace DAL.WeiXin
{
    public class ReceiveTextMsgDAL
    {
        public const string TABLE_NAME = "WX_ReceiveTextMsg";

        /// <summary>
        /// 添加文本消息
        /// </summary>
        /// <param name="info"></param>
        public void Insert(ReceiveTextMsg info)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("ID", info.ID);
            data.Add("WXConfigID", info.WXConfigID);
            data.Add("ToUserName", info.ToUserName);
            data.Add("FromUserName", info.FromUserName);
            data.Add("CreateTime", info.CreateTime);
            data.Add("MsgType", info.MsgType);
            data.Add("Content", info.Content);
            data.Add("AddTime", info.AddTime);
            data.Add("MsgId", info.MsgId);
            SQLHelperExt.Insert(TABLE_NAME, data);
        }

        #region 获取文本消息列表
        /// <summary>
        /// 获取文本消息列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet GetReceiveTextMsgList(string where)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT  a.* from WX_ReceiveTextMsg a left join WX_WXConfig b on(a.WXConfigID=b.ID)");
            if (where.Trim() != null && where.Trim() != "")
            {
                strSql.Append(" WHERE " + where);
            }
            strSql.Append(" ORDER BY AddTime DESC ");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }
        #endregion
    }
}
