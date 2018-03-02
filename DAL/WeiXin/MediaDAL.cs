using DAL.Common;
using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DAL.WeiXin
{
    public class MediaDAL
    {
        public const string TABLE_NAME = "WX_Media";

        public static MediaDAL CreateInstance()
        {
            return new MediaDAL();
        }

        /// <summary>
        /// 插入媒体数据
        /// </summary>
        /// <param name="info"></param>
        public void InsertInfo(Model.WeiXin.Media info)
        {
            Dictionary<string, object> datas = new Dictionary<string, object>();
            datas.Add("ID", info.ID);
            datas.Add("MediaName", info.MediaName);
            datas.Add("MediaFile", info.MediaFile);
            datas.Add("UploadTime", info.UploadTime);
            datas.Add("IsAutoSyn", info.IsAutoSyn);
            datas.Add("MediaType", info.MediaType);
            SQLHelperExt.Insert(TABLE_NAME, datas);
        }

        /// <summary>
        /// 删除指定ID的媒体数据
        /// </summary>
        /// <param name="id"></param>
        public void Delete(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string where = string.Format(" ID={0}",id);
                SQLHelperExt.Delete(TABLE_NAME, where);
            }
        }

        /// <summary>
        /// 更新媒体数据
        /// </summary>
        /// <param name="info"></param>
        public void UpdateInfo(Model.WeiXin.Media info)
        {
            string where = string.Format(" ID={0}",info.ID);
            Dictionary<string, object> datas = new Dictionary<string, object>();
            datas.Add("ID", info.ID);
            datas.Add("MediaName", info.MediaName);
            datas.Add("MediaFile", info.MediaFile);
            datas.Add("UploadTime", info.UploadTime);
            datas.Add("IsAutoSyn", info.IsAutoSyn);
            datas.Add("LastSynTime", info.LastSynTime);
            datas.Add("MediaID", info.MediaID);
            datas.Add("MediaType", info.MediaType);
            SQLHelperExt.Update(TABLE_NAME, datas,where);
        }

        /// <summary>
        /// 更新媒体同步数据
        /// </summary>
        /// <param name="info"></param>
        public void UpdateSynInfo(string id,DateTime lastSynTime,string mediaID)
        {
            string where = string.Format(" ID={0}", id);
            Dictionary<string, object> datas = new Dictionary<string, object>();
            datas.Add("LastSynTime", lastSynTime);
            datas.Add("MediaID", mediaID);
            SQLHelperExt.Update(TABLE_NAME, datas, where);
        }

        /// <summary>
        /// 根据ID获取媒体信息
        /// </summary>
        /// <param name="info"></param>
        public Model.WeiXin.Media GetMediaByID(string id)
        {
            Model.WeiXin.Media res = null;
            if (!string.IsNullOrEmpty(id))
            {
                string sql = string.Format("SELECT * FROM {0} WHERE ID='{1}'", TABLE_NAME, id);
                DataSet ds = DbHelperSQL.Query(sql);
                res = ds.ConvertToFirstObj<Model.WeiXin.Media>();
            }
            return res;
        }
    }
}
