using Maticsoft.DBUtility;
using Model.HP;
/* ==============================================================================
 * 类名称：PhotoDAL
 * 类描述：
 * 创建人：yhn,51809571@qq.com
 * 创建时间：2014/4/5 16:41:10
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
using DAL.Common;

namespace DAL.HP
{
    public class PhotoDAL
    {
        /// <summary>
        /// 插入欢乐照片信息
        /// </summary>
        /// <param name="info"></param>
        public void InsertInfo(Photo info)
        {
            string sql = @"INSERT INTO [HP_Photo]
                       ([ID]
                       ,[SiteCode]
                       ,[OpenId]
                       ,[ClientID]
                       ,[PrintCode]
                       ,[Img]
                       ,[State]
                       ,[AttachText] )
                 VALUES
                       (@ID
                        ,@SiteCode
                        ,@OpenId
                        ,@ClientID
                        ,@PrintCode
                        ,@Img
                        ,@State
                        ,@AttachText)";
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", info.ID),
                new System.Data.SqlClient.SqlParameter("@SiteCode", info.SiteCode),
                new System.Data.SqlClient.SqlParameter("@OpenId", info.OpenId),
                new System.Data.SqlClient.SqlParameter("@ClientID", info.ClientID),
                new System.Data.SqlClient.SqlParameter("@PrintCode", info.PrintCode),
                new System.Data.SqlClient.SqlParameter("@Img", info.Img),
                new System.Data.SqlClient.SqlParameter("@State", info.State),
                new System.Data.SqlClient.SqlParameter("@AttachText", info.AttachText)
            };
            DbHelperSQL.ExecuteSql(sql, paras);
        }

        /// <summary>
        /// 更新欢乐照片信息
        /// </summary>
        /// <param name="info"></param>
        public void UpdateInfo(Photo info,string id)
        {
            string sql = @"UPDATE [HP_Photo] SET
                       [SiteCode]=@SiteCode
                       ,[OpenId]=@OpenId
                       ,[ClientID]=@ClientID
                       ,[PrintCode]=@PrintCode
                       ,[Img]=@Img
                       ,[State]=@State
                    WHERE
                        ID=@ID";
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ID", info.ID),
                new System.Data.SqlClient.SqlParameter("@SiteCode", info.SiteCode),
                new System.Data.SqlClient.SqlParameter("@OpenId", info.OpenId),
                new System.Data.SqlClient.SqlParameter("@ClientID", info.ClientID),
                new System.Data.SqlClient.SqlParameter("@PrintCode", info.PrintCode),
                new System.Data.SqlClient.SqlParameter("@Img", info.Img),
                new System.Data.SqlClient.SqlParameter("@State", info.State)
            };
            DbHelperSQL.ExecuteSql(sql, paras);
        }

        /// <summary>
        /// 更新欢乐照片打印信息
        /// </summary>
        /// <param name="printCode"></param>
        /// <param name="clientID"></param>
        /// <param name="siteCode"></param>
        /// <param name="openID"></param>
        public void UpdatePrintInfo(string printCode, string clientID,string siteCode, string openID)
        {
            string sql = @"UPDATE [HP_Photo] SET
                       [ClientID]=@ClientID
                       ,[PrintCode]=@PrintCode
                    WHERE SiteCode=@SiteCode 
                        AND REPLACE(OpenID,'-','')=@OpenID
                        AND State=0";
            System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@ClientID", clientID),
                new System.Data.SqlClient.SqlParameter("@PrintCode", printCode),
                new System.Data.SqlClient.SqlParameter("@SiteCode", siteCode),
                new System.Data.SqlClient.SqlParameter("@OpenId", openID),
            };
            DbHelperSQL.ExecuteSql(sql, paras);
        }

        /// <summary>
        /// 保存欢乐照片信息，如站点用户已存在图片，则覆盖，否则插入
        /// </summary>
        /// <param name="info"></param>
        public void SaveInfo(Photo info)
        {
            //如里存在，则更新
            if (ExistPhoto(info.SiteCode, info.OpenId,0))
            {
                string sql = @"UPDATE [HP_Photo] SET
                       [Img]=@Img
                    WHERE
                        [SiteCode]=@SiteCode 
                        AND REPLACE(OpenId,'-','')=@OpenId
                        AND State=@State";
                System.Data.SqlClient.SqlParameter[] paras = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@SiteCode", info.SiteCode),
                new System.Data.SqlClient.SqlParameter("@OpenId", info.OpenId),
                new System.Data.SqlClient.SqlParameter("@Img", info.Img),
                new System.Data.SqlClient.SqlParameter("@State", info.State)
            };
                DbHelperSQL.ExecuteSql(sql, paras);
                info = GetInfo(info.SiteCode, info.OpenId, 0);
            }
            else
            {
                InsertInfo(info);
            }
        }

        /// <summary>
        /// 更新照片附加信息
        /// </summary>
        /// <param name="siteCode"></param>
        /// <param name="openID"></param>
        public void UpdateAttachText(string siteCode, string openID,string attachText)
        {
            Dictionary<string,object> dict=new Dictionary<string,object>();
            dict.Add("AttachText", attachText);
            string where=string.Format(" SiteCode = '{0}' AND OpenId='{1}' AND State=0",siteCode,openID);
            SQLHelperExt.Update("HP_Photo", dict, where);
        }

        /// <summary>
        /// 判断是否已存在此站点用户的图片信息
        /// </summary>
        /// <param name="siteCode"></param>
        /// <param name="openID"></param>
        /// <returns></returns>
        public Photo GetInfo(string siteCode, string openID,int state)
        {
            Photo res = null;
            if (!string.IsNullOrEmpty(siteCode) && !string.IsNullOrEmpty(openID))
            {
                string sql = @"SELECT * FROM [HP_Photo]
                        WHERE SiteCode = @SiteCode AND REPLACE(OpenId,'-','')=@OpenId
                            AND State=@State";
                IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", siteCode),
                    new System.Data.SqlClient.SqlParameter("@OpenId", openID),
                    new System.Data.SqlClient.SqlParameter("@State", state)
                };
                DataSet ds = DbHelperSQL.Query(sql, paras.ToArray());
                res=ds.ConvertToFirstObj<Photo>();
            }
            return res;
        }

        /// <summary>
        /// 判断是否已存在此站点用户的图片信息
        /// </summary>
        /// <param name="siteCode"></param>
        /// <param name="openID"></param>
        /// <returns></returns>
        public bool ExistPhoto(string siteCode, string openID,int state)
        {
            string sql = @"SELECT count(ID) FROM HP_Photo 
                    WHERE SiteCode=@SiteCode 
                    AND REPLACE(OpenID,'-','')=@OpenID
                    AND State=@State";
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@SiteCode", siteCode),
                    new System.Data.SqlClient.SqlParameter("@OpenID", openID),
                    new System.Data.SqlClient.SqlParameter("@State", state)
                };
            return DbHelperSQL.Exists(sql, paras.ToArray());
        }

        /// <summary>
        /// 返回已经提交打印的数量
        /// </summary>
        /// <param name="openID"></param>
        /// <returns></returns>
        public int OpenIDPhotoCount(string openID)
        {
            string sql = @"SELECT count(ID) AS PhotoCount FROM HP_Photo 
                    WHERE OpenID = @OpenID";
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@OpenID", openID)
                };
            DataSet ds = DbHelperSQL.Query(sql, paras.ToArray());
            return int.Parse(ds.Tables[0].Rows[0]["PhotoCount"].ToString());
        }

        /// <summary>
        /// 返回已经提交打印的数量
        /// </summary>
        /// <param name="openID"></param>
        /// <param name="strPrintCode">打印码</param>
        /// <returns></returns>
        public int OpenIDPhotoCount(string openID,string strPrintCode)
        {
            string sql = @"SELECT count(ID) AS PhotoCount FROM HP_Photo 
                    WHERE OpenID = @OpenID AND PrintCode = @PrintCode ";
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@OpenID", openID),
                    new System.Data.SqlClient.SqlParameter("@PrintCode", strPrintCode)
                };
            DataSet ds = DbHelperSQL.Query(sql, paras.ToArray());
            return int.Parse(ds.Tables[0].Rows[0]["PhotoCount"].ToString());
        }
    }
}
