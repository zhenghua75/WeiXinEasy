﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    /// <summary>
    /// SetReleaseSettings 的摘要说明
    /// </summary>
    public class SetReleaseSettings : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            ReturnObj<string> ro = JsonConvert.DeserializeObject<ReturnObj<string>>(Helper.GetJson(context, "SetReleaseSettings"));

            Helper.ProcessContext(context, JsonConvert.SerializeObject(ro));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}