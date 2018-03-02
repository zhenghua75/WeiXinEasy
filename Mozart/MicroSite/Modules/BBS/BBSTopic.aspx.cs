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
    public partial class BBSTopic : BasePage
    {
        private string TopicID;
        private BBS_Topic model;
        private DAL.BBS.BBSSectionDAL dalBBS;
        protected override bool BeforeLoad()
        {
            TopicID = Common.Common.NoHtml(Request.QueryString["ID"].ToString());

            //取社区主题信息
            dalBBS = new DAL.BBS.BBSSectionDAL();
            model = new BBS_Topic();
            DataSet dsTopic = dalBBS.GetTopicInfo(TopicID);

            if (null != dsTopic && dsTopic.Tables.Count > 0 && dsTopic.Tables[0].Rows.Count > 0)
            {
                model = DataConvert.DataRowToModel<BBS_Topic>(dsTopic.Tables[0].Rows[0]);
            }
            string strSection = model.SID;

            //取站点信息
            DataSet dsBBS = dalBBS.GetAccountData(strSection);

            if (null != dsBBS && dsBBS.Tables.Count > 0 && dsBBS.Tables[0].Rows.Count > 0)
            {
                SiteCode = dsBBS.Tables[0].Rows[0]["SiteCode"].ToString();
                SiteID = dsBBS.Tables[0].Rows[0]["ID"].ToString();
            }
            return base.BeforeLoad();
        }
        protected override void AddTempData(JinianNet.JNTemplate.TemplateContext context)
        {
            //取主题回复信息
            DataSet dsReply = dalBBS.GetReplyList(TopicID);
            List<BBS_Reply> liReply = new List<BBS_Reply>();
            if (null != dsReply && dsReply.Tables.Count > 0 && dsReply.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsReply.Tables[0].Rows)
                {
                    BBS_Reply modelR = DataConvert.DataRowToModel<BBS_Reply>(row);
                    liReply.Add(modelR);
                }
            }
            context.TempData["topic"] = model;
            context.TempData["replylist"] = liReply;
        }
    }
}