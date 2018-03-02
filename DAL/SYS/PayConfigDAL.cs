using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DAL.Common;

namespace DAL.SYS
{
    public class PayConfigDAL
    {
        public const string TABLE_NAME = "SYS_PayConfig";

        public static PayConfigDAL CreateInstance()
        {
            return new PayConfigDAL();
        }


        /// <summary>
        /// 根据ID获取媒体信息
        /// </summary>
        /// <param name="info"></param>
        public Model.SYS.PayConfig GetModelByID(string id)
        {
            Model.SYS.PayConfig res = null;
            if (!string.IsNullOrEmpty(id))
            {
                string sql = string.Format("SELECT * FROM {0} WHERE ID='{1}'",
                    TABLE_NAME, id);
                DataSet ds = DbHelperSQL.Query(sql);
                Model.SYS.PayConfig temp = ds.ConvertToFirstObj<Model.SYS.PayConfig>();
                if (temp != null)
                {
                    if (temp.EncryptParams)
                    {
                        res = temp;
                    }
                    else
                    {
                        res = temp;
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// 根据SiteCode、PayMode获取媒体信息
        /// payMode包括wxpay、alipay
        /// </summary>
        /// <param name="info"></param>
        public Model.SYS.PayConfig GetModel(string siteCode,string payMode)
        {
            Model.SYS.PayConfig res = null;
            if (!string.IsNullOrEmpty(siteCode) && !string.IsNullOrEmpty(payMode))
            {
                string sql = string.Format("SELECT * FROM {0} WHERE SiteCode='{1}' AND PayMode='{2}'", 
                    TABLE_NAME, siteCode,payMode);
                DataSet ds = DbHelperSQL.Query(sql);
                Model.SYS.PayConfig temp = ds.ConvertToFirstObj<Model.SYS.PayConfig>();
                if(temp!=null)
                {
                    if(temp.EncryptParams)
                    {
                        res = temp;
                    }
                    else
                    {
                        res=temp;
                    }
                }
            }
            return res;
        }
    }
}
