using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.BBS;
using Mozart.Common;

namespace Mozart.MicroSite
{
    public partial class BBSList : BasePage
    {
        public string Section { get; set; }
        protected override bool BeforeLoad()
        {
            if (null == Request.QueryString["ID"])
            {
                Section = "CFC502D7-1FB6-4F3B-9827-6921B426A0CD";
            }
            Section = Common.Common.NoHtml(Request.QueryString["ID"].ToString());

            //取站点信息
            DAL.BBS.BBSSectionDAL dalBBS = new DAL.BBS.BBSSectionDAL();
            DataSet dsBBS = dalBBS.GetAccountData(Section);

            if (null != dsBBS && dsBBS.Tables.Count > 0 && dsBBS.Tables[0].Rows.Count > 0)
            {
                SiteCode = dsBBS.Tables[0].Rows[0]["SiteCode"].ToString();
                SiteID = dsBBS.Tables[0].Rows[0]["ID"].ToString();
            }

            return base.BeforeLoad();
        }
        protected override void AddTempData(JinianNet.JNTemplate.TemplateContext context)
        {
            DAL.BBS.BBSSectionDAL dalBBS = new DAL.BBS.BBSSectionDAL();
            //取社区主题
            DataSet dsTopic = dalBBS.GetTopicList(Section);

            List<BBS_TopicInfo> liTopic = new List<BBS_TopicInfo>();
            if (null != dsTopic && dsTopic.Tables.Count > 0 && dsTopic.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsTopic.Tables[0].Rows)
                {
                    BBS_TopicInfo model = DataConvert.DataRowToModel<BBS_TopicInfo>(row);
                    liTopic.Add(model);
                }
            }

            context.TempData["topiclist"] = liTopic;
        }
    }
}