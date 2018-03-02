using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.DC;
using Model.DC;
using Mozart.Common;
using System.IO;

namespace Mozart.MicroSite
{
    public partial class BuildingList : BasePage
    {
        protected override void AddTempData(JinianNet.JNTemplate.TemplateContext context)
        {
            base.AddTempData(context);
            List<DC_Building> BuildingList = new List<DC_Building>();
            DC_BuildingsDAL dal = new DC_BuildingsDAL();
            DataSet BuildingListds = dal.GetDCBuildingListBySiteCode(SiteCode);
            string BuildingListcount = BuildingListds.Tables[0].Rows.Count.ToString();
            foreach (DataRow row in BuildingListds.Tables[0].Rows)
            {
                DC_Building model = DataConvert.DataRowToModel<DC_Building>(row);
                BuildingList.Add(model);
            }
            context.TempData["BuildingListcount"] = BuildingListcount;
            context.TempData["BuildingList"] = BuildingList;
        }
    }
}