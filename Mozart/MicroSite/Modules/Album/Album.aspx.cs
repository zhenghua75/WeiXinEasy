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
    public partial class Album : BasePage
    {
        protected override bool BeforeLoad()
        {
            SiteCode = Common.Common.NoHtml(Request.QueryString["ID"].ToString());
            return base.BeforeLoad();
        }
        protected override void AddTempData(JinianNet.JNTemplate.TemplateContext context)
        {            
            //取站点信息
            DAL.SYS.AccountDAL dalAccount = new DAL.SYS.AccountDAL();
            DAL.Album.AlbumDAL dalAlbum = new DAL.Album.AlbumDAL();

            //取站点相册列表
            DataSet dsAlbumType = dalAlbum.GetAlbumTypeList(SiteCode);
            List<PA_AlbumType> liAlbumType = new List<PA_AlbumType>();
            if (null != dsAlbumType && dsAlbumType.Tables.Count > 0 && dsAlbumType.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsAlbumType.Tables[0].Rows)
                {
                    PA_AlbumType model = DataConvert.DataRowToModel<PA_AlbumType>(row);
                    liAlbumType.Add(model);
                }
            }

            context.TempData["AlbumTypelist"] = liAlbumType;
        }
    }
}