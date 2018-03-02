using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Model.DC;
using DAL.DC;
using Mozart.Common;

namespace Mozart.MicroSite
{
    public partial class NewHome : BasePage
    {
        protected override void AddTempData(JinianNet.JNTemplate.TemplateContext context)
        {
            base.AddTempData(context);

            List<DC_House> HouseList = new List<DC_House>();
            DC_HouseDAL dal = new DC_HouseDAL();
            DataSet Houseds = dal.GetDCHouseList(this.SiteCode, "1");
            string housecount = Houseds.Tables[0].Rows.Count.ToString();
            foreach (DataRow row in Houseds.Tables[0].Rows)
            {
                DC_House model = DataConvert.DataRowToModel<DC_House>(row);
                HouseList.Add(model);
            }

            context.TempData["HouseCount"] = housecount;
            context.TempData["HouseList"] = HouseList;
        }
    }
}