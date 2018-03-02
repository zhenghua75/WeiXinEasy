using Maticsoft.DBUtility;
using Model.WeiXin;
/* ==============================================================================
 * 类名称：ReceiveMsgDAL
 * 类描述：
 * 创建人：yhn,51809571@qq.com
 * 创建时间：2014/3/15 16:43:23
 * 修改人：
 * 修改时间：
 * 修改备注：
 * @version 1.0
 * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.WeiXin
{
    public class ReceiveMsgDAL
    {
        /// <summary>
        /// 插入微信接收消息
        /// </summary>
        /// <param name="info"></param>
        public void InsertInfo(ReceiveMsg info)
        {
            string sql = @"INSERT INTO [WX_ReceiveMsg]
                       ([WXConfigID]
                       ,[ToUserName]
                       ,[FromUserName]
                       ,[CreateTime]
                       ,[MsgType]
                       ,[MsgId]
                       ,[Event]
                       ,[EventKey]
                       ,[MsgBody]
                       ,[ReceiveTime]
                       ,[Remark])
                 VALUES
                       (@WXConfigID
                        ,@ToUserName
                        ,@FromUserName
                        ,@CreateTime
                        ,@MsgType
                        ,@MsgId
                        ,@Event
                        ,@EventKey
                        ,@MsgBody
                        ,@ReceiveTime
                        ,@Remark)";
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@WXConfigID", info.WXConfigID),
                new System.Data.SqlClient.SqlParameter("@ToUserName", info.ToUserName),
                new System.Data.SqlClient.SqlParameter("@FromUserName", info.FromUserName),
                new System.Data.SqlClient.SqlParameter("@CreateTime", info.CreateTime),
                new System.Data.SqlClient.SqlParameter("@MsgType", info.MsgType),
                new System.Data.SqlClient.SqlParameter("@MsgId", info.MsgId),
                new System.Data.SqlClient.SqlParameter("@Event", info.Event),
                new System.Data.SqlClient.SqlParameter("@EventKey", info.EventKey),
                new System.Data.SqlClient.SqlParameter("@MsgBody", info.MsgBody),
                new System.Data.SqlClient.SqlParameter("@ReceiveTime", info.ReceiveTime),
                new System.Data.SqlClient.SqlParameter("@Remark", info.Remark)
            };
            DbHelperSQL.ExecuteSql(sql, paras);
        }
    }
}
