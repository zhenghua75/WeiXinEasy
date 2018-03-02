using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using DAL;
using Newtonsoft.Json;

namespace Mozart.WebService
{
    /// <summary>
    /// wsHappyPhoto 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class wsHappyPhoto : System.Web.Services.WebService
    {
        #region 生成打印码
        /// <summary>
        /// 生成指定个数的打印串
        /// </summary>
        /// <param name="iAmount">指定的数字串个数</param>
        /// <param name="strClientCode">指定的终端代码</param>
        /// <param name="strStart">有效开始时间</param>
        /// <param name="strEnd">有效的结束时间</param>
        /// <returns></returns>
        [WebMethod(Description = "生成打印码")]
        public string CreatePrintCode(int iAmount,string strSIteCode,string strClientCode,string strStart,string strEnd)
        {
            DAL.HP.PrintCodeDAL dal = new DAL.HP.PrintCodeDAL();
            DataSet ds = dal.CreatePrintCode(iAmount,strSIteCode, strClientCode, strStart, strEnd);
            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //return Common.Common.DataTableToJson("ClientCode", ds.Tables[0]);
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                return "0";
            }
        }
        #endregion

        #region 获取所需要打印的图片
        /// <summary>
        /// 获取所需要打印的图片
        /// </summary>
        /// <param name="strSiteCode">站点代码</param>
        /// <param name="strClientCode">指定的终端代码</param>
        /// <returns></returns>
        [WebMethod(Description = "获取打印信息")]
        public DataSet GetPrintPhoto(string strSiteCode, string strClientCode)
        {
            DAL.HP.PrintCodeDAL dal = new DAL.HP.PrintCodeDAL();
            return dal.getPrintPhoto(strSiteCode, strClientCode);
        }
        #endregion

        #region 修改打印码与照片的状态
        /// <summary>
        /// 修改打印码与照片的状态
        /// </summary>
        /// <param name="strPhotoID">照片ID</param>
        /// <param name="strState">修改后状态</param>
        /// <returns></returns>
        [WebMethod(Description = "修改打印码与照片的状态")]
        public bool UpdatePrintState(string strPhotoID, string strState)
        {
            DAL.HP.PrintCodeDAL dal = new DAL.HP.PrintCodeDAL();
            return dal.UpdatePrintState(strPhotoID, strState);
        }
        #endregion

        #region 获取批量打印码
        /// <summary>
        /// 获取批量打印码
        /// </summary>
        /// <param name="strSiteCode">站点代码</param>
        /// <param name="strState">指定的终端代码</param>
        /// <returns></returns>
        [WebMethod(Description = "获取打印信息")]
        public DataSet GetPrintCode(string strSiteCode, string strState)
        {
            DAL.HP.HPClientDAL dal = new DAL.HP.HPClientDAL();
            return dal.GetPrintCode(strSiteCode, strState);
        }
        #endregion

        #region 修改批量打印码状态
        /// <summary>
        /// 修改批量打印码状态
        /// </summary>
        /// <param name="strPrintCode">批量打印码</param>
        /// <param name="strState">修改后状态</param>
        /// <returns></returns>
        [WebMethod(Description = "修改打印码与照片的状态")]
        public bool UpdatePrintCodeState(string strPrintCode, string strState)
        {
            DAL.HP.HPClientDAL dal = new DAL.HP.HPClientDAL();
            return dal.UpdatePrintCodeState(strPrintCode, strState);
        }
        #endregion


    }
}
