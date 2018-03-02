using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mozart.Common;

namespace Mozart.MicroSite
{
    public partial class MyThumbDetail : BasePage
    {
        public Model.Album.UserPhoto model { get; set; }
        protected override bool BeforeLoad()
        {
            string strProductID = string.Empty;
            if (null == Request.QueryString["id"])
            {
                return false;
            }
            strProductID = Common.Common.NoHtml(Request.QueryString["id"].ToString());
            DAL.Album.UserPhotoDAL dal = new DAL.Album.UserPhotoDAL();
            DataSet ds = dal.GetMyThumb(strProductID);

            model = new Model.Album.UserPhoto();
            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                model = DataConvert.DataRowToModel<Model.Album.UserPhoto>(ds.Tables[0].Rows[0]);
            }
            SiteCode = model.SiteCode;
            return base.BeforeLoad();
        }
        protected override void AddTempData(JinianNet.JNTemplate.TemplateContext context)
        {
            context.TempData["pDetail"] = model;
        }
    }
}