using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.DC;
using DAL.DC;
using Mozart.Common;
using System.IO;

namespace Mozart.MicroSite
{
    public partial class OldHome : BasePage
    {
        protected override void AddTempData(JinianNet.JNTemplate.TemplateContext context)
        {
            List<DC_House> HouseList = new List<DC_House>();
            DC_HouseDAL dal = new DC_HouseDAL();
            DataSet Houseds = dal.GetDCHouseList(SiteCode, "0");
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