using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.RC;
using Model.RC;
using System.IO;
using System.Text;

namespace Mozart.WXWall
{
    public partial class MoveRace : System.Web.UI.Page
    {
        public static string strID = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string action = string.Empty;
                if (Request["action"] != null && Request["action"] != "")
                {
                    action = Common.Common.NoHtml(Request["action"].ToString());
                }
                else
                {
                    return;
                }
                if (Request["rid"] != null && Request["rid"] != "")
                {
                    strID = Common.Common.NoHtml(Request["rid"].ToString());
                }
                else
                {
                    return;
                }
                switch (action.ToLower().Trim())
                {
                    case "detail":
                        GetRaceDetaile();
                        break;
                    case "raceuserlist":
                        GetRaceUserList();
                        break;
                    case "newrace":
                        AddNewRace();
                        break;
                    case "getmovenum":
                        GetMoveNum();
                        break;
                }
                Response.End();
            }
        }
        #region 获取赛事详细
        /// <summary>
        /// 获取赛事详细
        /// </summary>
        void GetRaceDetaile()
        {
            if (strID != null && strID != "")
            {
                RC_RaceDAL dal = new RC_RaceDAL();
                Response.Write(Dataset2Json(dal.GetRaceDetail(strID)));
            }
        }
        #endregion
        #region 获取用户列表
        /// <summary>
        /// 获取用户列表
        /// </summary>
        void GetRaceUserList()
        {
            if (strID != null && strID != "")
            {
                RC_UsersDAL dal = new RC_UsersDAL();
                Response.Write(Dataset2Json(dal.GetTopRcUserList(10," and RaceID='" + strID + "' ")));
            }
        }
        #endregion
        #region 添加或更新用户速度
        /// <summary>
        /// 添加或更新用户速度
        /// </summary>
        void AddNewRace()
        {
            string nickname = string.Empty;
            string speed = string.Empty;
            if (Request["nickname"] != null && Request["nickname"] != "")
            {
                nickname = Common.Common.NoHtml(Request["nickname"].ToString());
            }
            if (Request["speed"] != null && Request["speed"] != "")
            {
                speed = Common.Common.NoHtml(Request["speed"].ToString());
            }
            RC_Users model = new RC_Users();
            RC_UsersDAL dal = new RC_UsersDAL();
            if (nickname.Trim() != null && nickname.Trim() != "")
            {
                if (dal.ExistUser(nickname, strID))
                {
                    string uid = string.Empty;
                    uid = dal.GetRaceUserValue("ID", nickname, strID).ToString();
                    if (speed.Trim() != null && speed.Trim() != "")
                    {
                        if (dal.UpdateUserSpeedByOpenID(uid, speed, strID))
                        {
                            Response.Write("{\"success\":\"true\"}");
                        }
                        else
                        {
                            Response.Write("{\"success\":\"操作失败\"}");
                        }
                    }
                }
                else
                {
                    model.ID = Guid.NewGuid().ToString("N").ToUpper();
                    model.OpenID = nickname;
                    if (speed.Trim() != null && speed.Trim() != "")
                    {
                        model.Speed = speed;
                    }
                    model.IsDel = 0;
                    model.IsWin = 0;
                    model.RaceID = strID;
                    if (dal.AddRCUser(model))
                    {
                        Response.Write("{\"success\":\"true\"}");
                    }
                    else
                    {
                        Response.Write("{\"success\":\"操作失败\"}");
                    }
                }
            }
        }
        #endregion
        #region 获取重力速度
        /// <summary>
        /// 获取重力速度
        /// </summary>
        void GetMoveNum()
        {
            RC_RaceDAL dal = new RC_RaceDAL();
            string movenum = string.Empty;
            movenum = dal.GetRCRaceValueByID("MoveNum", strID).ToString();
            Response.Write("{\"movenum\":\"" + movenum + "\"}");
        }
        #endregion
        public static string Dataset2Json(DataSet ds)
        {
            StringBuilder json = new StringBuilder();
            json.Append("[");
            foreach (System.Data.DataTable dt in ds.Tables)
            {
                json.Append(DataTable2Json(dt));
                json.Append(",");
            }
            json.Remove(json.Length - 1, 1);
            json.Append("]");
            return json.ToString();
        }
        public static string DataTable2Json(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString().Replace("\"", "\\\"")); //对于特殊字符，还应该进行特别的处理。 
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            return jsonBuilder.ToString();
        } 
    }
}