using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.PA;
using Mozart.Common;

namespace Mozart.MicroSite
{
    public partial class AlbumList : BasePage
    {
        public string AlbumTypeID { get; set; }
        protected override bool BeforeLoad()
        {
            AlbumTypeID = Common.Common.NoHtml(Request.QueryString["ID"].ToString());

            //取站点信息
            DAL.SYS.AccountDAL dalAccount = new DAL.SYS.AccountDAL();
            DAL.Album.AlbumDAL dalAlbum = new DAL.Album.AlbumDAL();

            DataSet dsAccount = dalAlbum.GetAccountData(AlbumTypeID);

            if (null != dsAccount && dsAccount.Tables.Count > 0 && dsAccount.Tables[0].Rows.Count > 0)
            {
                SiteCode = dsAccount.Tables[0].Rows[0]["SiteCode"].ToString();
                SiteID = dsAccount.Tables[0].Rows[0]["ID"].ToString();
            }
            return base.BeforeLoad();
        }
        protected override void AddTempData(JinianNet.JNTemplate.TemplateContext context)
        {
            //取站点信息
            DAL.SYS.AccountDAL dalAccount = new DAL.SYS.AccountDAL();
            DAL.Album.AlbumDAL dalAlbum = new DAL.Album.AlbumDAL();


            //取站点相册列表
            DataSet dsAlbumList = dalAlbum.GetAlbumList(AlbumTypeID);
            List<PA_Album> liAlbumList = new List<PA_Album>();
            if (null != dsAlbumList && dsAlbumList.Tables.Count > 0 && dsAlbumList.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsAlbumList.Tables[0].Rows)
                {
                    PA_Album model = DataConvert.DataRowToModel<PA_Album>(row);
                    liAlbumList.Add(model);
                }
            }

            context.TempData["albumlist"] = liAlbumList;

        }
    }
}