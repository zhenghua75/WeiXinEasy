using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Model.CR;
using DAL.CR;

namespace Mozart.WXWall
{
    public partial class MessageWall : System.Web.UI.Page
    {
       public string strSiteCode = string.Empty;
       public string walltitle = string.Empty;
       public string msglist = string.Empty;
       public string strRoomID = string.Empty;
       public string strUserCount = string.Empty;
       public string script = string.Empty;
       public string bgimgurl = string.Empty;
       public string msgcount = string.Empty;
       public string urlparm = string.Empty;
       public string codimg = string.Empty;
       public string appid = string.Empty;
       ChatRoomDAL rmdal = new ChatRoomDAL();
       ChatUsersDAL userdal = new ChatUsersDAL();
       ChatMessageDAL msgdal = new ChatMessageDAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["SiteCode"] != null && Request["SiteCode"] != "")
                {
                    if (Request["SiteCode"].ToString().Length < 10)
                    {
                        strSiteCode = Common.Common.NoHtml(Request["SiteCode"].ToString().ToLower());
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
                if (Request["rmid"] != null && Request["rmid"] != "")
                {
                    strRoomID = Common.Common.NoHtml(Request["rmid"].ToString());
                }
                else
                {
                    return;
                }
                MsgDetail(); SubStringUrl();
                strUserCount = userdal.GetUserCount(Convert.ToInt32(strRoomID)).ToString();
                string action = string.Empty;
                if (Request["action"] != null && Request["action"] != "")
                {
                    action = Request["action"].ToString();
                    switch (action.ToLower().Trim())
                    {
                        case"msg":
                            ShowMsgList();
                            break;
                        case "photo":
                            ShowPhotoList();
                            break;
                        case "lottery":
                            getLotteryUserList();
                            break;
                        case "savewin":
                            SaveWin();
                            break;
                        case "delwin":
                            SaveWin();
                            break;
                        default:
                            ShowMsgList();
                            break;
                    }
                }
                else
                {
                    ShowMsgList();
                }
            }
        }
        #region 聊天室详细
        /// <summary>
        /// 聊天室详细
        /// </summary>
        void MsgDetail()
        {
            DataSet rmdetailds = rmdal.GetChatRoomDetail(Convert.ToInt32(strRoomID),strSiteCode);
            ChatRoom rmdetailmodel = new ChatRoom();
            if (null != rmdetailds && rmdetailds.Tables.Count > 0 && rmdetailds.Tables[0].Rows.Count > 0)
            {
                walltitle = rmdetailds.Tables[0].Rows[0]["RoomName"].ToString();
                var rmbgimgurl = rmdetailds.Tables[0].Rows[0]["RoomImg"].ToString();
                var rmwebcodeimg = rmdetailds.Tables[0].Rows[0]["WebCodeImg"].ToString();
                if (rmbgimgurl.Trim() != null && rmbgimgurl.Trim() != "")
                {
                    bgimgurl = "style=\"background-image: url(" + rmbgimgurl + ");\"";
                }
                if (rmwebcodeimg.Trim() != null && rmwebcodeimg.Trim() != "")
                {
                    codimg = rmwebcodeimg;
                }
                else
                {
                    codimg = "images/photo-default.png";
                }
                appid = rmdetailds.Tables[0].Rows[0]["AppID"].ToString();
                if (appid.Trim() != null && appid.Trim() != "")
                {
                    appid = "无法扫描二维码的用户，请关注微信号:<br/><span class=\"redfont\">" + appid+"</span>";
                }
            }
            msgcount = msgdal.GetChatMsgCount(strRoomID).ToString();
        }
        #endregion
        #region URL设置
        /// <summary>
        /// URL设置
        /// </summary>
        void SubStringUrl()
        {
            urlparm = Request.Url.ToString();
            if (urlparm.ToLower().Contains("?"))
            {
                if (urlparm.ToLower().Contains("?action="))
                {
                    string suburl = urlparm.Substring(urlparm.IndexOf("?action"));
                    urlparm = urlparm.Substring(0, urlparm.Length - suburl.Length)+"?";
                }
                else if (urlparm.ToLower().Contains("&action="))
                {
                    string suburl = urlparm.Substring(urlparm.IndexOf("&action"));
                    urlparm = urlparm.Substring(0, urlparm.Length - suburl.Length) + "&";
                }
                else
                {
                    urlparm = urlparm + "&";
                }
            }
            else
            {
                urlparm = urlparm + "?";
            }
        }
        #endregion
        #region 留言列表
        /// <summary>
        /// 留言列表
        /// </summary>
        void ShowMsgList()
        {
            ChatMessageDAL dal = new ChatMessageDAL();
            DataSet ChatMsgDs = dal.GetChatMsgByTop(20, strSiteCode,strRoomID);
            string housecount = ChatMsgDs.Tables[0].Rows.Count.ToString();
            if (null != ChatMsgDs && ChatMsgDs.Tables.Count > 0 && ChatMsgDs.Tables[0].Rows.Count > 0)
            {
                msglist = "\r\n<div data-role=\"page\" id=\"chatsPage\" data-url=\"chatsPage\" tabindex=\"0\"" +
                    " class=\"ui-page ui-page-theme-a ui-page-header-fixed ui-page-footer-fixed ui-page-active\"" +
                    " style=\"min-height: 529px; padding-top: 69px; padding-bottom: 29px;\">";
                msglist += "\r\n<div data-role=\"content\" class=\"ui-content\" role=\"main\">\r\n";
                msglist += "\r\n<div class=\"divContent\" id=\"divChats\">\r\n";
                for (int i = 0; i < ChatMsgDs.Tables[0].Rows.Count; i++)
                {
                    string nickname = ChatMsgDs.Tables[0].Rows[i]["NickName"].ToString();
                    string headimgurl = ChatMsgDs.Tables[0].Rows[i]["HeadImgUrl"].ToString();
                    string msgContent = ChatMsgDs.Tables[0].Rows[i]["MsgText"].ToString();
                    string addtime = ChatMsgDs.Tables[0].Rows[i]["AddTime"].ToString();
                    addtime = Convert.ToDateTime(addtime).ToString("MM-dd HH:mm:ss");
                    int countNum = ChatMsgDs.Tables[0].Rows.Count-i;
                    string msgID = ChatMsgDs.Tables[0].Rows[i]["ID"].ToString();
                    string msgtype = ChatMsgDs.Tables[0].Rows[i]["MsgType"].ToString();
                    switch (msgtype.ToLower().Trim())
                    {
                        case "image":
                            msgContent = "<img src=" + msgContent + ">";
                            break;
                    }
                    msglist += "\r\n<div class=\"divChat\" id=\"chat_" + msgID+ "\">" +
                "\r\n<table>" +
                    "\r\n<tbody>" +
                        "\r\n<tr>" +
                            "\r\n<td class=\"tdFace\" rowspan=\"2\">" +
                                "\r\n<a  class=\"aUserInfo\"><span class=\"pName\">" + nickname + "</span>" +
                                    "\r\n<img src=\"" + headimgurl+ "\"></a>" +
                            "\r\n</td>" +
                            "\r\n<td class=\"tdChat\">" +
                                "\r\n<div class=\"article\">" +
                                    "\r\n<a>" + msgContent + "</a>" +
                                "\r\n</div>" +
                            "\r\n</td>" +
                        "\r\n</tr>" +
                        "\r\n<tr>" +
                            "\r\n<td class=\"pTime\">" + addtime + "&nbsp;&nbsp;&nbsp;&nbsp;" + countNum + "楼</td>" +
                        "\r\n</tr>" +
                    "\r\n</tbody>" +
                "\r\n</table>" +
            "\r\n</div>";
                }
                msglist += "\r\n</div>\r\n</div>\r\n</div>\r\n";
            }
        }
        #endregion
        #region 图片列表
        /// <summary>
        /// 图片列表
        /// </summary>
        void ShowPhotoList()
        {
            ChatMessageDAL dal = new ChatMessageDAL();
            DataSet ChatMsgDs = dal.GetChatMsgImgList(strSiteCode, strRoomID);
            string housecount = ChatMsgDs.Tables[0].Rows.Count.ToString();
            if (null != ChatMsgDs && ChatMsgDs.Tables.Count > 0 && ChatMsgDs.Tables[0].Rows.Count > 0)
            {
                msglist = "\r\n<div data-role=\"page\" id=\"chatsPage\" data-url=\"chatsPage\" tabindex=\"0\"" +
                    " class=\"ui-page ui-page-theme-a ui-page-header-fixed ui-page-footer-fixed ui-page-active\"" +
                    " style=\"min-height: 529px; padding-top: 69px; padding-bottom: 29px;\">";
                msglist += "\r\n<div data-role=\"content\" class=\"ui-content\" role=\"main\">\r\n";
                msglist += "\r\n<div class=\"divChatPhotos\" id=\"divChatPhotos\">\r\n";
                msglist += "\r\n<ul>\r\n";
                for (int i = 0; i < ChatMsgDs.Tables[0].Rows.Count; i++)
                {
                    string msgID = ChatMsgDs.Tables[0].Rows[i]["ID"].ToString();
                    string msgContent = ChatMsgDs.Tables[0].Rows[i]["MsgText"].ToString();
                    string nickname = ChatMsgDs.Tables[0].Rows[i]["NickName"].ToString();
                    string addtime = ChatMsgDs.Tables[0].Rows[i]["AddTime"].ToString();
                    addtime = Convert.ToDateTime(addtime).ToString("yyyy-MM-dd");
                    msglist += "<li><a data-name=\"" + nickname + "\" data-time=\"" + addtime + "\" href=\"" + msgContent + "\"><img src=\"" + msgContent + "\"  /></a></li>\r\n";
                }
                msglist += "\r\n</ul>\r\n";
                msglist += "\r\n</div>\r\n</div>\r\n</div>\r\n";
            }
        }
        #endregion
        #region 获取聊天室用户用于抽奖
        /// <summary>
        /// 获取聊天室用户用于抽奖
        /// </summary>
        void getLotteryUserList()
        {
            DataSet ChatUserDs = userdal.GetChatUsersList(" AND a.RoomID=" + strRoomID + "  and a.IsWin=0  ");
            if (null != ChatUserDs && ChatUserDs.Tables.Count > 0 && ChatUserDs.Tables[0].Rows.Count > 0)
            {
                msglist = "\r\n<div data-role=\"page\" id=\"chatsPage\" data-url=\"chatsPage\" tabindex=\"0\"" +
                    " class=\"ui-page ui-page-theme-a ui-page-header-fixed ui-page-footer-fixed ui-page-active\"" +
                    " style=\"min-height: 529px; padding-top: 69px; padding-bottom: 29px;\">";
                msglist += "\r\n<div data-role=\"content\" class=\"ui-content\" role=\"main\">";
                msglist += "\r\n<div class=\"divContent\" id=\"divChats\">";
                msglist += "\r\n<ul class=\"lotteryul\">";
                msglist += "\r\n<li class=\"lotteryulli\">";
                    msglist += "\r\n<div class=\"divLottery\">";
                        msglist += "\r\n<div class=\"topboxdiv\">";
                            msglist += "\r\n<span class=\"lott-wt\">现场抽奖</span>";
                            msglist += "\r\n<p class=\"lott-w\"><span>参加抽奖人数</span><em class=\"join-num lotteryUserNum\">"+strUserCount+"</em></p>";
                        msglist += "\r\n</div>";
                        msglist += "\r\n<div class=\"rock-box\">";
                            msglist += "\r\n<span class=\"rock-head\">";
                            msglist += "\r\n<img id=\"headimgurl\" src=\"images/lottery-default.jpg\">";
                            msglist += "\r\n</span>";
                            msglist += "\r\n<span id=\"rockname\" data-id=\"\" data-o=\"\" data-rmid=\"\" class=\"rock-name\"></span>";
                            msglist += "<input type=\"text\" id=\"uid\" name=\"name\" value=\"\" style=\"display:none;\" />";
                        msglist += "\r\n</div>";
                        msglist += "\r\n<div class=\"btn-start\">";
                        msglist += "\r\n<span>一次抽取</span>";
                       msglist += "\r\n<select name=\"\" id=\"lotteryNumSel\" onchange=\"setTimeCount()\"><option value=\"1\">1</option>";
                           msglist += "\r\n<option value=\"2\">2</option><option value=\"3\">3</option>";
                            msglist += "\r\n<option value=\"4\">4</option><option value=\"5\">5</option>";
                            msglist += "\r\n<option value=\"6\">6</option><option value=\"7\">7</option>";
                            msglist += "\r\n<option value=\"8\">8</option><option value=\"9\">9</option>";
                            msglist += "\r\n<option value=\"10\">10</option><option value=\"20\">20</option>";
                            msglist += "\r\n<option value=\"30\">30</option><option value=\"50\">50</option>";
                            msglist += "\r\n<option value=\"100\">100</option>\r\n</select>人";
                            msglist += "\r\n<a href=\"javascript:void(0);\" data-disabled=\"no\">";
                            msglist += "\r\n<span style=\"display: inline;\" id=\"startLottery\" >开始抽奖</span>";
                            msglist += "\r\n<span style=\"display: none;\" id=\"endLottery\" >停止</span>";
                        msglist += "\r\n</a>";
                        msglist += "\r\n</div>";
                   msglist += "\r\n</div>";
                msglist += "\r\n</li>";
                msglist += "\r\n<li class=\"lotterullastli\">";
                    msglist += "\r\n<div class=\"divWinner\">";
                      msglist += "\r\n<div class=\"list-top\">";
                          msglist += "\r\n<p class=\"pro-num\">获奖人数<em class=\"winUserNum\"></em></p>";
                          msglist += "\r\n<span>获奖名单</span>";
                      msglist += "\r\n</div>";
                        msglist += "\r\n<div class=\"list-tit\">";
                            msglist += "\r\n<span>获奖序号</span>";
                             msglist += "\r\n<span>用户昵称</span>";
                        msglist += "\r\n</div>";
                        msglist += "\r\n<div class=\"priname-box\">";
                        msglist += "\r\n<ul id=\"prize-list\" class=\"prize-list\">";
                        msglist += "\r\n</ul>";
                        msglist += "\r\n</div>";
                        msglist += "\r\n<div class=\"und-btn\">";
                        msglist += "\r\n<span>重新抽奖</span></div>";
                    msglist += "\r\n</div>";
                msglist += "\r\n</li>";
            msglist += "\r\n</ul>";
                script = "[";
                for (int i = 0; i < ChatUserDs.Tables[0].Rows.Count; i++)
                {
                    string UserID = ChatUserDs.Tables[0].Rows[i]["ID"].ToString();
                    string OpenID = ChatUserDs.Tables[0].Rows[i]["OpenID"].ToString();
                    string NickName = ChatUserDs.Tables[0].Rows[i]["NickName"].ToString();
                    string HeadImgUrl = ChatUserDs.Tables[0].Rows[i]["HeadImgUrl"].ToString();
                    script += "{ \"id\": \"" + UserID + "\",\"rmid\":\"" + strRoomID+ "\",\"name\": \"" + NickName +
                        "\",\"oid\":\""+OpenID+"\", \"headimg\": \"" + HeadImgUrl + "\" }";
                    if (i != ChatUserDs.Tables[0].Rows.Count - 1)
                    {
                        script += ",";
                    }
                }
                script  += "]";
                msglist += "\r\n</div>\r\n</div>\r\n</div>\r\n";
            }
        }
        #endregion
        #region 更新中奖用户状态
        /// <summary>
        /// 更新中奖用户状态
        /// </summary>
        void SaveWin()
        {
            string uid = string.Empty; string oid = string.Empty; string rmid = string.Empty;
            if (Request["uid"] != null && Request["uid"] != "")
            {
                uid= Common.Common.NoHtml(Request["uid"]);
            }
            if (Request["oid"] != null && Request["oid"] != "")
            {
                oid = Common.Common.NoHtml(Request["oid"]);
            }
            if (Request["rmid"] != null && Request["rmid"] != "")
            {
                rmid = Common.Common.NoHtml(Request["rmid"]);
            }
            if (uid.Trim() != null && uid.Trim() != "" && oid.Trim() != null && oid.Trim() != "" &&
                rmid.Trim() != null && rmid.Trim() != "")
            {
                if (userdal.UpdateUserWin(uid, Convert.ToInt32(rmid)))
                {
                    Response.Write("{\"success\":\"true\"}");
                }
                else
                {
                    Response.Write("{\"success\":\"操作失败\"}");
                }
            }
            else
            {
                Response.Write("{\"success\":\"操作失败\"}");
            }
            Response.End();
        }
        #endregion
    }
}