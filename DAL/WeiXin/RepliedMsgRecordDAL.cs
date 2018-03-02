using Maticsoft.DBUtility;
using Model.WeiXin;
/* ==============================================================================
 * 类名称：RepliedMsgRecordDAL
 * 类描述：
 * 创建人：yhn,51809571@qq.com
 * 创建时间：2014/3/15 16:43:38
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
    public class RepliedMsgRecordDAL
    {
        /// <summary>
        /// 添加回复消息记录
        /// </summary>
        /// <param name="info"></param>
        public void InsertInfo(RepliedMsgRecord info)
        {
            string sql = @"INSERT INTO [WX_RepliedMsgRecord]
                       ([WXConfigID]
                       ,[MsgType]
                       ,[MsgBody]
                       ,[ToUserName]
                       ,[FromUserName]
                       ,[CreateTime])
                 VALUES
                       (@WXConfigID
                        ,@MsgType
                        ,@MsgBody
                        ,@ToUserName
                        ,@FromUserName
                        ,@CreateTime)";
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@WXConfigID", info.WXConfigID),
                new System.Data.SqlClient.SqlParameter("@MsgType", info.MsgType),
                new System.Data.SqlClient.SqlParameter("@MsgBody", info.MsgBody),
                new System.Data.SqlClient.SqlParameter("@ToUserName", info.ToUserName),
                new System.Data.SqlClient.SqlParameter("@FromUserName", info.FromUserName),
                new System.Data.SqlClient.SqlParameter("@CreateTime", info.CreateTime)
            };
            DbHelperSQL.ExecuteSql(sql, paras);
        }
    }
}
