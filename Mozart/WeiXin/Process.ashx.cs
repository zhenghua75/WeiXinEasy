using DAL.ACT;
using DAL.CMS;
using DAL.WeiXin;
using Model.ACT;
using Model.WeiXin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml;
using WeiXinCore.CUrl;
using WeiXinCore.Models;
using WeiXinCore.Models.RequestMsgModels;
using WeiXinCore.Models.ResponseMsgModels;
using System.Text.RegularExpressions;
using DAL.HP;
using Model.HP;
using System.Net;
using System.IO;
using DAL.SYS;
using Model.SYS;
using Model.Album;
using DAL.Album;
using WeiXinCore;

namespace Mozart.WeiXin
{
    /// <summary>
    /// Process 的摘要说明
    /// </summary>
    public class Process : IHttpHandler
    {
        private string siteCode = string.Empty;
        private WXConfig wxConfig = null;
        WeiXinCore.WeiXin weixin;

        public void ProcessRequest(HttpContext context)
        {
            siteCode = context.Request.QueryString["id"];

            if (!string.IsNullOrEmpty(siteCode))
            {
                WXConfigDAL dal = new WXConfigDAL();
                wxConfig = dal.GetWXConfigBySiteCode(siteCode);
                if (wxConfig != null)
                {
                    WeiXinCore.Models.WeiXinConfig weixinConfig = new WeiXinCore.Models.WeiXinConfig()
                    {
                        ID = wxConfig.WXID,
                        Name = wxConfig.WXName,
                        Token = wxConfig.WXToken,
                        AppId = wxConfig.WXAppID,
                        AppSecret = wxConfig.WXAppSecret
                    };
                    weixin = new WeiXinCore.WeiXin(weixinConfig);
                    weixin.OnMsgReceiving = MsgReceiving;
                    weixin.OnTextMsgReceived = TextMsgReceive;
                    weixin.OnImageMsgReceived = ImageMsgReceive;
                    weixin.OnSubscribeEvent = SubscribeProcess;
                    weixin.OnMenuClick = MenuClickProcess;

                    //MenuDAL.CreateWeiXinMenu(wxConfig.ID);
                    //string json=WeiXinHelper.GetCustomMenu(weixin.GetAccessToken().Access_Token);
                    //string fileDir = HttpContext.Current.Server.MapPath("/HP_PHOTO/");
                    //if (!Directory.Exists(fileDir))
                    //{
                    //    Directory.CreateDirectory(fileDir);
                    //}
                    //string fileName = weixin.SaveAsMedia("WVNXJOLqpySbvX1T7zGCKYjboBbcv2x4YOgz9e0Avbq_xks5mGujx10avfPQ3ZlW", fileDir);
                    //string file = HttpContext.Current.Server.MapPath("/HP_PHOTO/");
                    //if (!Directory.Exists(file))
                    //{
                    //    Directory.CreateDirectory(file);
                    //}
                    //string tt=weixin.SaveAsMedia("d3VvIIQU7kaxd-NPw8a5qmDPkxLLY1NlBPLmCGNA_40K9DmDN4bbBEBJywfx2JFx",
                    //   file);
                    //List<UserInfo> list= weixin.GetUserInfos();

                    context.Response.ContentType = "text/plain";
                    context.Response.Write(weixin.AcceptMsg());
                    return;
                }
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write("ERROR");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 接收消息时，记录消息信息
        /// </summary>
        /// <param name="doc"></param>
        public void MsgReceiving(XmlDocument doc)
        {
            if (doc != null)
            {
                ReceiveMsgDAL dal = new ReceiveMsgDAL();
                ReceiveMsg info = new ReceiveMsg()
                {
                    WXConfigID = wxConfig.ID,
                    CreateTime=doc.DocumentElement.SelectSingleNode("CreateTime").InnerText,
                    FromUserName = doc.DocumentElement.SelectSingleNode("FromUserName").InnerText,
                    ToUserName = doc.DocumentElement.SelectSingleNode("ToUserName").InnerText,
                    MsgType = doc.DocumentElement.SelectSingleNode("MsgType").InnerText,
                    MsgBody = doc.InnerXml
                };
                dal.InsertInfo(info);

                string msgType = doc.DocumentElement.SelectSingleNode("MsgType").InnerText;
                if (msgType.ToLower() == "text")
                {
                    ReceiveTextMsgDAL receiveTextMsgDAL = new ReceiveTextMsgDAL();
                    ReceiveTextMsg receiveTextMsg = new ReceiveTextMsg()
                    {
                        WXConfigID = wxConfig.ID,
                        CreateTime = doc.DocumentElement.SelectSingleNode("CreateTime").InnerText,
                        FromUserName = doc.DocumentElement.SelectSingleNode("FromUserName").InnerText,
                        ToUserName = doc.DocumentElement.SelectSingleNode("ToUserName").InnerText,
                        MsgType = doc.DocumentElement.SelectSingleNode("MsgType").InnerText,
                        Content = doc.DocumentElement.SelectSingleNode("Content").InnerText,
                    };
                    receiveTextMsgDAL.Insert(receiveTextMsg);
                }

                string msgcontent = string.Empty;
                string openid = string.Empty;
                DAL.CR.ChatRoomDAL roomdal = new DAL.CR.ChatRoomDAL();
                DAL.CR.ChatUsersDAL userdal = new DAL.CR.ChatUsersDAL();
                Model.CR.ChatUsers usermodel = new Model.CR.ChatUsers();
                DAL.CR.ChatMessageDAL msgdal = new DAL.CR.ChatMessageDAL();
                Model.CR.ChatMessage msgmodel = new Model.CR.ChatMessage();
                UserDAL wx_userdal = new UserDAL();
                User wx_usermodel = new User();
                openid = doc.DocumentElement.SelectSingleNode("FromUserName").InnerText;
                string ChatUserName = string.Empty;
                if (openid != null && openid != "")
                {
                    if (!wx_userdal.ExistUserByOpenID(openid))
                    {
                        UserInfo userinfo = weixin.GetUserInfo(openid);//获取用户信息
                        wx_usermodel.ID = Guid.NewGuid().ToString("N").ToUpper();
                        wx_usermodel.City = userinfo.City;
                        wx_usermodel.Country = userinfo.Country;
                        wx_usermodel.HeadImgUrl = userinfo.Headimgurl;
                        wx_usermodel.Language = userinfo.Language;
                        wx_usermodel.NickName = userinfo.NickName;
                        wx_usermodel.OpenID = userinfo.OpenId;
                        wx_usermodel.Province = userinfo.Province;
                        wx_usermodel.Sex = userinfo.Sex;
                        wx_usermodel.Subscribe = userinfo.Subscribe;
                        wx_usermodel.SubscribeTime = userinfo.Subscribe_Time;
                        ChatUserName = userinfo.NickName;
                        if (null != wx_usermodel.ID)
                        {
                            wx_userdal.Insert(wx_usermodel);
                        }
                    }
                    else
                    {
                        ChatUserName = wx_userdal.GetUserValueByOpenID("NickName", openid).ToString();
                    }
                }
                else////OpenID为空
                {
 
                }
                int strRoomID = 1;
                #region 上墙信息
                if (msgType.ToLower() == "text")
                {
                    msgcontent = doc.DocumentElement.SelectSingleNode("Content").InnerText;
                    if (msgcontent.Contains("#"))
                    {
                        if (openid != null && openid != "")
                        {
                            #region 服务号取用户加入信息
                            if (msgcontent.Length <= 1)//判断是否单独发送 #
                            {
                                if (userdal.ExistChatUser(Convert.ToInt32(msgcontent), openid))//如果存在该用户
                                {
                                    string uid = userdal.GetChatUserValueByOpenID("ID", openid, Convert.ToInt32(msgcontent)).ToString();
                                    if (uid.Trim() != null && uid.Trim() != "")
                                    {
                                        userdal.UpdateChatUserIsDel(uid);//更新用户状态  提示表示用户离开房间
                                    }
                                }
                            }
                            else
                            {
                                msgcontent = msgcontent.Substring(1);
                                if (Regex.IsMatch(msgcontent, @"^\d+$"))//判断是否为数字
                                {
                                    string roomsitecode = roomdal.GetChatRoomValueByID("SiteCode", Convert.ToInt32(msgcontent)).ToString();
                                    if (roomsitecode.Trim() != null && roomsitecode.Trim() != "")
                                    {
                                        if (roomdal.ExistChatRoomNum(roomsitecode, Convert.ToInt32(msgcontent)))
                                        {
                                            //提示房间存在
                                            if (userdal.ExistChatUser(Convert.ToInt32(msgcontent), openid))
                                            {
                                                //提示该用户已进入该房间
                                            }
                                            else
                                            {
                                                usermodel.ID = Guid.NewGuid().ToString("N").ToUpper();
                                                usermodel.OpenID = openid;
                                                usermodel.RoomID = Convert.ToInt32(msgcontent);
                                                usermodel.IsDel = 0;
                                                usermodel.IsWin = 0;
                                                userdal.AddChatUsers(usermodel);
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region 订阅号获取上墙信息
                            string[] sArray = msgcontent.Split('#');
                            string username = string.Empty;
                            string msginfo = string.Empty;
                            try
                            {
                                username = sArray[0].ToString();
                            }
                            catch (Exception)
                            {
                            }
                            try
                            {
                                msginfo = sArray[1].ToString();
                            }
                            catch (Exception)
                            {
                            }
                            if (username.Trim() != null && username.Trim() != "")
                            {
                                if (userdal.ExistChatUser(username))//判断用户名是否已存在
                                {
                                    //已存在用户
                                    if (msginfo.Trim() == null || msginfo.Trim() == "")
                                    {
                                        userdal.UpdateChatUserIsDel(username);//删除用户
                                    }
                                }
                                else
                                {
                                    usermodel.ID = Guid.NewGuid().ToString("N").ToUpper();
                                    usermodel.OpenID = username;
                                    usermodel.RoomID = strRoomID;
                                    usermodel.IsDel = 0;
                                    usermodel.IsWin = 0;
                                    userdal.AddChatUsers(usermodel);
                                }
                                if (msginfo.Trim() != null && msginfo.Trim() != "")
                                {
                                    msgmodel.ID = Guid.NewGuid().ToString("N").ToUpper();
                                    msgmodel.UserID = username;
                                    msgmodel.RoomID = strRoomID;
                                    msgmodel.MsgType = "text";
                                    msgmodel.MsgText = msginfo;
                                    msgmodel.MsgState = 0;//1表示通过 0表示不通过
                                    msgmodel.IsDel = 0;
                                    msgdal.AddChatMessage(msgmodel);
                                }
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        #region 服务号取上墙信息
                        if (userdal.ExistChatUser(strRoomID, openid))
                        {
                            //提示已经存在该用户
                            string userid = userdal.GetChatUserValueByOpenID("ID",openid,strRoomID).ToString();
                            msgmodel.ID = Guid.NewGuid().ToString("N").ToUpper();
                            msgmodel.UserID = userid;
                            msgmodel.RoomID = strRoomID;
                            msgmodel.MsgType = "text";
                            msgmodel.MsgText = msgcontent;
                            msgmodel.MsgState = 0;//1表示通过 0表示不通过
                            msgmodel.IsDel = 0;
                            msgdal.AddChatMessage(msgmodel);
                        }
                        #endregion
                    }
                }
                else if (msgType.ToLower() == "image")
                {
                    #region 服务号取上墙信息
                    if (userdal.ExistChatUser(strRoomID, openid))
                    {
                        //提示已经存在该用户
                        string userid = userdal.GetChatUserValueByOpenID("ID", openid, strRoomID).ToString();
                        msgmodel.ID = Guid.NewGuid().ToString("N").ToUpper();
                        msgmodel.UserID = userid;
                        msgmodel.RoomID = strRoomID;
                        msgmodel.MsgType = "image";
                        msgmodel.MsgText = msgcontent;
                        msgmodel.MsgState = 0;//1表示通过 0表示不通过
                        msgmodel.IsDel = 0;
                        msgdal.AddChatMessage(msgmodel);
                    }
                    #endregion
                }
                #endregion
            }
        }

        /// <summary>
        /// 文本消息处理
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string TextMsgReceive(TextRequestMsgModel msg)
        {
            string res = string.Empty;
            MsgAutoRuleDAL dal = new MsgAutoRuleDAL();
            MsgAutoRule rule = dal.GetKeywordsRule(wxConfig.ID,msg.Content);
            if (rule != null)
            {
                res = ProcessReply(msg, rule.MsgType, rule.MsgValue);
            }
            else
            {
                rule = dal.GetRegexRule(wxConfig.ID, msg.Content);
                if (rule != null)
                {
                    res = ProcessReply(msg, rule.MsgType, rule.MsgValue);
                }
                else
                {
                    rule = dal.GetDefaultRule(wxConfig.ID);
                    if (rule != null)
                    {
                        res = ProcessReply(msg, rule.MsgType, rule.MsgValue);
                    }
                }
            }//启用默认消息回复
            return res;
        }

        /// <summary>
        /// 图片消息处理
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string ImageMsgReceive(ImageRequestMsgModel msg)
        {
            string res = string.Empty;
            MsgAutoRuleDAL dal = new MsgAutoRuleDAL();
            MsgAutoRule rule = dal.GetImageRule(wxConfig.ID);
            if (rule != null)
            {
                switch (rule.MsgType.ToLower())
                {
                    case "hp_photo":
                        try
                        {
                            string fileDir = HttpContext.Current.Server.MapPath("/HP_PHOTO/");
                            if (!Directory.Exists(fileDir))
                            {
                                Directory.CreateDirectory(fileDir);
                            }
                            ExceptionLog log = new ExceptionLog();
                            log.Message = weixin.WeiXinConfig.AppId + "}{" + weixin.WeiXinConfig.AppSecret + "}{" + msg.MediaId;
                            ExceptionLogDAL.InsertExceptionLog(log);

                            //string fileName = weixin.SaveAsMedia(msg.MediaId, fileDir);
                            string fileName = weixin.SaveAsFile(msg.PicUrl, fileDir);
                            if (!string.IsNullOrEmpty(fileName))
                            {
                                Photo p = new Photo()
                                {
                                    SiteCode = siteCode,
                                    OpenId = msg.FromUserName,
                                    Img = fileName
                                };
                                PhotoDAL photoDal = new PhotoDAL();
                                photoDal.SaveInfo(p);

                                //回复文本消息
                                string url = string.Format("{0}/WebService/ImageEdit.aspx?id={1}", GetSiteUrl(), p.ID);
                                string content = string.Format("<a href='{0}'>点击编辑</a>",url);
                                TextResponseMsgModel textMsg = new TextResponseMsgModel()
                                {
                                    ToUserName = msg.FromUserName,
                                    FromUserName = msg.ToUserName,
                                    CreateTime = WeiXinHelper.ConvertDateTimeInt(DateTime.Now).ToString(),
                                    Content = content
                                };
                                res = textMsg.ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            ExceptionLogDAL.InsertExceptionLog(ex);
                        }
                        break;
                    case "user_photo":
                        try
                        {
                            string fileDir = HttpContext.Current.Server.MapPath("/USER_PHOTO/");
                            if (!Directory.Exists(fileDir))
                            {
                                Directory.CreateDirectory(fileDir);
                            }

                            //string fileName = weixin.SaveAsMedia(msg.MediaId, fileDir);
                            string fileName = weixin.SaveAsFile(msg.PicUrl, fileDir);
                            if (!string.IsNullOrEmpty(fileName))
                            {
                                UserPhoto photo = new UserPhoto()
                                {
                                    Name = fileName,
                                    SiteCode = siteCode,
                                    OpenId = msg.FromUserName,
                                    FilePath = fileName
                                };
                                UserPhotoDAL uPhotoDal = new UserPhotoDAL();
                                uPhotoDal.Insert(photo);

                                //回复文本消息
                                TextResponseMsgModel textMsg = new TextResponseMsgModel()
                                {
                                    ToUserName = msg.FromUserName,
                                    FromUserName = msg.ToUserName,
                                    CreateTime = WeiXinHelper.ConvertDateTimeInt(DateTime.Now).ToString(),
                                    Content = rule.MsgValue == null ? string.Empty : TransformText(rule.MsgValue,msg)
                                };
                                res = textMsg.ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            ExceptionLogDAL.InsertExceptionLog(ex);
                        }
                        break;
                    case "user_photo_url":
                        try
                        {
                            string fileDir = HttpContext.Current.Server.MapPath("/USER_PHOTO/");
                            if (!Directory.Exists(fileDir))
                            {
                                Directory.CreateDirectory(fileDir);
                            }
                            string fileName = weixin.SaveAsFile(msg.PicUrl, fileDir);
                            if (!string.IsNullOrEmpty(fileName))
                            {
                                UserPhoto photox = new UserPhoto()
                                {
                                    Name = fileName,
                                    SiteCode = siteCode,
                                    OpenId = msg.FromUserName,
                                    FilePath = fileName
                                };
                                UserPhotoDAL uPhotoDal = new UserPhotoDAL();
                                uPhotoDal.Insert(photox);

                                //回复文本消息
                                string url = string.Format("{0}/MicroSite/PhotoPrint.aspx?id={1}", GetSiteUrl(), photox.ID);
                                string content = string.Format("<a href='{0}'>开始打印</a>",url);
                                TextResponseMsgModel textUrlMsg = new TextResponseMsgModel()
                                {
                                    ToUserName = msg.FromUserName,
                                    FromUserName = msg.ToUserName,
                                    CreateTime = WeiXinHelper.ConvertDateTimeInt(DateTime.Now).ToString(),
                                    Content = content
                                };
                                res = textUrlMsg.ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            ExceptionLogDAL.InsertExceptionLog(ex);
                        }
                        break;
                    case "user_printphoto_stepone":
                        try
                        {
                            string fileDir = HttpContext.Current.Server.MapPath("/USER_PHOTO/");
                            if (!Directory.Exists(fileDir))
                            {
                                Directory.CreateDirectory(fileDir);
                            }
                            string fileName = weixin.SaveAsFile(msg.PicUrl, fileDir);
                            if (!string.IsNullOrEmpty(fileName))
                            {
                                UserPhoto photox = new UserPhoto()
                                {
                                    Name = fileName,
                                    SiteCode = siteCode,
                                    OpenId = msg.FromUserName,
                                    FilePath = fileName
                                };
                                UserPhotoDAL uPhotoDal = new UserPhotoDAL();
                                uPhotoDal.Insert(photox);

                                //回复文本消息
                                string url = string.Format("{0}/MicroSite/PhotoPrintStepOne.aspx?id={1}", GetSiteUrl(), photox.ID);
                                string content = string.Format("<a href='{0}'>开始打印</a>", url);
                                TextResponseMsgModel textUrlMsg = new TextResponseMsgModel()
                                {
                                    ToUserName = msg.FromUserName,
                                    FromUserName = msg.ToUserName,
                                    CreateTime = WeiXinHelper.ConvertDateTimeInt(DateTime.Now).ToString(),
                                    Content = content
                                };
                                res = textUrlMsg.ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            ExceptionLogDAL.InsertExceptionLog(ex);
                        }
                        break;
                    default:
                        rule = dal.GetDefaultRule(wxConfig.ID);
                        if (rule != null)
                        {
                            res = ProcessReply(msg, rule.MsgType, rule.MsgValue);
                        }
                        break;
                }
            }
            return res;
        }

        /// <summary>
        /// 订阅消息处理
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string SubscribeProcess(SubscribeEventRequestMsgModel msg)
        {
            string res = string.Empty;
            MsgAutoRuleDAL dal = new MsgAutoRuleDAL();
            MsgAutoRule rule = dal.GetSubscribeRule(wxConfig.ID);
            if (rule != null)
            {
                res = ProcessReply(msg, rule.MsgType, rule.MsgValue);
            }
            else
            {
                rule = dal.GetDefaultRule(wxConfig.ID);
                if (rule != null)
                {
                    res = ProcessReply(msg, rule.MsgType, rule.MsgValue);
                }
            }//启用默认消息回复
            return res;
        }

        /// <summary>
        /// 事件单击事件消息处理
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string MenuClickProcess(ClickEventRequestMsgModel msg)
        {
            string res = string.Empty;
            MsgAutoRuleDAL dal = new MsgAutoRuleDAL();
            MsgAutoRule rule = dal.GetClickEventRule(wxConfig.ID,msg.EventKey);
            if (rule != null)
            {
                res = ProcessReply(msg, rule.MsgType, rule.MsgValue);
            }
            else
            {
                rule = dal.GetDefaultRule(wxConfig.ID);
                if (rule != null)
                {
                    res = ProcessReply(msg, rule.MsgType, rule.MsgValue);
                }
            }//启用默认消息回复
            return res;
        }

        /// <summary>
        /// 处理回复消息
        /// "text":回复文本消息处理,MsgValue对应回复的文本
        /// "sub_auto_coupon":回复订阅自动优惠券处理，MsgValue对应回复的图文消息ID
        /// "auto_news_article":根据文章自动生成图文消息进行回复,MsgValue为类别ID
        /// "news":回复图文表中的消息,MsgValue为图文消息表ID集，用逗号分隔
        /// </summary>
        /// <param name="replyMsgType"></param>
        /// <param name="replyMsgValue"></param>
        /// <param name="customParams"></param>
        /// <returns></returns>
        private string ProcessReply(RequestMsgModel msgModel, string replyMsgType, string replyMsgValue)
        {
            string res = string.Empty;
            try
            {
                switch (replyMsgType.ToLower())
                {
                    case "text":
                        //回复文本消息处理,MsgValue对应回复的文本
                        TextResponseMsgModel textMsg = new TextResponseMsgModel()
                            {
                                ToUserName = msgModel.FromUserName,
                                FromUserName = msgModel.ToUserName,
                                CreateTime = WeiXinHelper.ConvertDateTimeInt(DateTime.Now).ToString(),
                                Content = replyMsgValue == null ? string.Empty : TransformText(replyMsgValue, msgModel)
                            };
                        res = textMsg.ToString();
                        break;
                    case "voice":
                        //回复语音消息处理,MsgValue对应回复的文本
                        VoiceResponseMsgModel voiceMsg = new VoiceResponseMsgModel()
                        {
                            ToUserName = msgModel.FromUserName,
                            FromUserName = msgModel.ToUserName,
                            CreateTime = WeiXinHelper.ConvertDateTimeInt(DateTime.Now).ToString(),
                        };
                        Media media = MediaDAL.CreateInstance().GetMediaByID(replyMsgValue);
                        if (media != null && !string.IsNullOrEmpty(media.MediaID))
                        {
                            voiceMsg.MediaId = media.MediaID;
                        }
                        res = voiceMsg.ToString();
                        break;
                    case "wxpay_test":
                        //用于微信支付测试
                        TextResponseMsgModel textMsgx = new TextResponseMsgModel()
                            {
                                ToUserName = msgModel.FromUserName,
                                FromUserName = msgModel.ToUserName,
                                CreateTime = WeiXinHelper.ConvertDateTimeInt(DateTime.Now).ToString(),
                                Content = string.Format("<a href='{0}/Payment/wxpay/wxpayDemo.aspx?openid={1}'>微信支付测试</a>", GetSiteUrl(), msgModel.FromUserName)
                            };
                        res = textMsgx.ToString();
                        break;
                    case "transfer_customer_service":
                        //将消息转发到多客服
                        TransferCustomerServiceResponseMsgModel transferMsg = new TransferCustomerServiceResponseMsgModel()
                        {
                            ToUserName = msgModel.FromUserName,
                            FromUserName = msgModel.ToUserName,
                            CreateTime = WeiXinHelper.ConvertDateTimeInt(DateTime.Now).ToString(),
                        };
                        res = transferMsg.ToString();
                        break;
                    case "sub_auto_coupon":
                        //回复订阅自动优惠券处理，MsgValue对应回复的优惠券图文消息ID
                        //SubscribeCouponActHandle sch = new SubscribeCouponActHandle();
                        SiteActivityDAL dal = new SiteActivityDAL();
                        SiteActivity activity = dal.GetSiteAct(siteCode, "Coupon");
                        if (activity != null)
                        {
                            CouponDAL cdal = new CouponDAL();
                            if (!cdal.ExistCoupon(siteCode, activity.ID, msgModel.FromUserName))
                            {
                                Coupon coupon = new Coupon()
                                {
                                    SiteCode = siteCode,
                                    SiteActivityID = activity.ID,
                                    OpenID = msgModel.FromUserName,
                                    //CouponCode = msgModel.FromUserName,
                                    CouponStatus = 0
                                };
                                cdal.InsertInfo(coupon);
                            }
                        }
                        CouponNewsDAL nmDAL = new CouponNewsDAL();
                        CouponNews nm = nmDAL.GetCouponNews(replyMsgValue);
                        if (nm != null)
                        {
                            NewsResponseMsgModel newsModel = new NewsResponseMsgModel()
                            {
                                ToUserName = msgModel.FromUserName,
                                FromUserName = msgModel.ToUserName,
                                CreateTime = WeiXinHelper.ConvertDateTimeInt(DateTime.Now).ToString()
                            };
                            List<Article> articles = new List<Article>();
                            articles.Add(new Article()
                            {
                                Title = nm.Title,
                                Description = nm.Description,
                                PicUrl = GetPicUrl(nm.PicUrl),
                                Url = TransformUrl(nm.Url, msgModel)
                            });
                            newsModel.Articles = articles;
                            res = newsModel.ToString();
                        }
                        break;
                    case "auto_coupon_category":
                        SiteActivityDAL dalCatList = new SiteActivityDAL();
                        SiteActivity activityCat = dalCatList.GetSiteAct(siteCode, "Coupon");
                        if (activityCat != null)
                        {
                            CouponDAL cdal = new CouponDAL();
                            if (!cdal.ExistCoupon(siteCode, activityCat.ID, msgModel.FromUserName))
                            {
                                Coupon coupon = new Coupon()
                                {
                                    SiteCode = siteCode,
                                    SiteActivityID = activityCat.ID,
                                    OpenID = msgModel.FromUserName,
                                    //CouponCode = msgModel.FromUserName,
                                    CouponStatus = 0
                                };
                                cdal.InsertInfo(coupon);
                            }
                        }
                        ArticleDAL catDal = new ArticleDAL();
                        DataSet cdsCat = catDal.GetCategoryList(siteCode, replyMsgValue);
                        if (cdsCat != null && cdsCat.Tables.Count > 0 && cdsCat.Tables[0] != null && cdsCat.Tables[0].Rows.Count > 0)
                        {
                            int i = 0;
                            NewsResponseMsgModel newsModel = new NewsResponseMsgModel()
                            {
                                ToUserName = msgModel.FromUserName,
                                FromUserName = msgModel.ToUserName,
                                CreateTime = WeiXinHelper.ConvertDateTimeInt(DateTime.Now).ToString()
                            };
                            List<Article> articles = new List<Article>();
                            foreach (DataRow dr in cdsCat.Tables[0].Rows)
                            {
                                if (++i > 4)
                                {
                                    break;
                                }
                                articles.Add(new Article()
                                {
                                    Title = dr["Title"].ToString(),
                                    Description = dr["Summary"].ToString(),
                                    //Description = RemoveHtmlTag(dr["Content"].ToString(), 30),
                                    PicUrl = GetPicUrl(dr["Pic"].ToString()),
                                    Url = GetArticleUrl(dr["ID"].ToString())
                                });
                            }
                            newsModel.Articles = articles;
                            res = newsModel.ToString();
                        }
                        break;
                    case "auto_news_article":
                        //根据文章自动生成图文消息进行回复,MsgValue为文章ID
                        ArticleDAL aDal = new ArticleDAL();
                        DataSet ds = aDal.GetArticleDetail(replyMsgValue);
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            int i = 0;
                            NewsResponseMsgModel newsModel = new NewsResponseMsgModel()
                            {
                                ToUserName = msgModel.FromUserName,
                                FromUserName = msgModel.ToUserName,
                                CreateTime = WeiXinHelper.ConvertDateTimeInt(DateTime.Now).ToString()
                            };
                            List<Article> articles = new List<Article>();
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                if (++i > 4)
                                {
                                    break;
                                }
                                articles.Add(new Article()
                                {
                                    Title = dr["Title"].ToString(),
                                    Description = dr["Summary"].ToString(),
                                    //Description = RemoveHtmlTag(dr["Content"].ToString(), 100),
                                    PicUrl = GetPicUrl(dr["Pic"].ToString()),
                                    Url = GetArticleUrl(dr["ID"].ToString())
                                });
                            }
                            newsModel.Articles = articles;
                            res = newsModel.ToString();
                        }
                        break;
                    case "auto_news_category":
                        //根据类别自动生成图文消息进行回复,MsgValue为类别ID
                        ArticleDAL cDal = new ArticleDAL();
                        DataSet cds = cDal.GetCategoryList(siteCode, replyMsgValue);
                        if (cds != null && cds.Tables.Count > 0 && cds.Tables[0] != null && cds.Tables[0].Rows.Count > 0)
                        {
                            int i = 0;
                            NewsResponseMsgModel newsModel = new NewsResponseMsgModel()
                            {
                                ToUserName = msgModel.FromUserName,
                                FromUserName = msgModel.ToUserName,
                                CreateTime = WeiXinHelper.ConvertDateTimeInt(DateTime.Now).ToString()
                            };
                            List<Article> articles = new List<Article>();
                            foreach (DataRow dr in cds.Tables[0].Rows)
                            {
                                if (++i > 4)
                                {
                                    break;
                                }
                                articles.Add(new Article()
                                {
                                    Title = dr["Title"].ToString(),
                                    Description = dr["Summary"].ToString(),
                                    //Description = RemoveHtmlTag(dr["Content"].ToString(), 30),
                                    PicUrl = GetPicUrl(dr["Pic"].ToString()),
                                    Url = GetArticleUrl(dr["ID"].ToString())
                                });
                            }
                            newsModel.Articles = articles;
                            res = newsModel.ToString();
                        }
                        break;
                    case "coupon":
                        //回复图文表中的消息,MsgValue为图文消息表ID集，用逗号分隔
                        NewsMsgDAL nmDAL1 = new NewsMsgDAL();
                        NewsMsg nms = nmDAL1.GetNewsMsg(replyMsgValue);
                        if (nms != null)
                        {
                            NewsResponseMsgModel newsModel = new NewsResponseMsgModel()
                            {
                                ToUserName = msgModel.FromUserName,
                                FromUserName = msgModel.ToUserName,
                                CreateTime = WeiXinHelper.ConvertDateTimeInt(DateTime.Now).ToString()
                            };
                            List<Article> articles = new List<Article>();
                            articles.Add(new Article()
                            {
                                Title = nms.Title,
                                Description = nms.Description,
                                PicUrl = GetPicUrl(nms.PicUrl),
                                Url = TransformUrl(nms.Url, msgModel)
                            });
                            newsModel.Articles = articles;
                            res = newsModel.ToString();
                        }
                        break;
                    case "news":
                        //回复图文表中的消息,MsgValue为图文消息表ID集，用逗号分隔
                        NewsMsgDAL nmDALs = new NewsMsgDAL();
                        IList<NewsMsg> newsMsgs = nmDALs.GetNewsMsgs(replyMsgValue);
                        if (newsMsgs != null)
                        {
                            NewsResponseMsgModel newsModel = new NewsResponseMsgModel()
                            {
                                ToUserName = msgModel.FromUserName,
                                FromUserName = msgModel.ToUserName,
                                CreateTime = WeiXinHelper.ConvertDateTimeInt(DateTime.Now).ToString()
                            };
                            List<Article> articles = new List<Article>();
                            foreach (NewsMsg msg in newsMsgs)
                            {
                                articles.Add(new Article()
                                {
                                    Title = msg.Title,
                                    Description = msg.Description,
                                    PicUrl = GetPicUrl(msg.PicUrl),
                                    Url = TransformUrl(msg.Url, msgModel)
                                });
                            }
                            newsModel.Articles = articles;
                            res = newsModel.ToString();
                        }
                        break;
                    case "url":
                        //根据文章自动生成图文消息进行回复,MsgValue为文章ID
                        DAL.SYS.AccountDAL dalUrl = new DAL.SYS.AccountDAL();
                        DataSet dsUrl = dalUrl.GetAccountExtData(replyMsgValue);
                        if (dsUrl != null && dsUrl.Tables.Count > 0 && dsUrl.Tables[0] != null && dsUrl.Tables[0].Rows.Count > 0)
                        {
                            int i = 0;
                            NewsResponseMsgModel newsModel = new NewsResponseMsgModel()
                            {
                                ToUserName = msgModel.FromUserName,
                                FromUserName = msgModel.ToUserName,
                                CreateTime = WeiXinHelper.ConvertDateTimeInt(DateTime.Now).ToString()
                            };
                            List<Article> articles = new List<Article>();
                            foreach (DataRow dr in dsUrl.Tables[0].Rows)
                            {
                                if (++i > 4)
                                {
                                    break;
                                }
                                articles.Add(new Article()
                                {
                                    Title = dr["Name"].ToString(),
                                    Description = dr["Summary"].ToString(),
                                    PicUrl = GetPicUrl(dr["Photo"].ToString()),
                                    Url = GetSiteInfo(dr["ID"].ToString())
                                });
                            }
                            newsModel.Articles = articles;
                            res = newsModel.ToString();
                        }
                        break;
                    case "hp_photo_text":
                        //为当前hp_photo对应的照片附加文字信息
                        PhotoDAL photoDal = new PhotoDAL();
                        if (photoDal.ExistPhoto(siteCode, msgModel.FromUserName, 0))
                        {
                            TextRequestMsgModel temp = msgModel as TextRequestMsgModel;
                            string text = temp.Content.Replace("#ms", "");
                            //附加图片文字
                            photoDal.UpdateAttachText(siteCode, msgModel.FromUserName, text);
                            TextResponseMsgModel textMsg2 = new TextResponseMsgModel()
                            {
                                ToUserName = msgModel.FromUserName,
                                FromUserName = msgModel.ToUserName,
                                CreateTime = WeiXinHelper.ConvertDateTimeInt(DateTime.Now).ToString(),
                                Content = replyMsgValue == null ? string.Empty : TransformText(replyMsgValue, msgModel)
                            };
                            res = textMsg2.ToString();
                        }
                        else
                        {
                            TextResponseMsgModel textMsg2 = new TextResponseMsgModel()
                            {
                                ToUserName = msgModel.FromUserName,
                                FromUserName = msgModel.ToUserName,
                                CreateTime = WeiXinHelper.ConvertDateTimeInt(DateTime.Now).ToString(),
                                Content = "对不起，您暂未参加图片打印活动！"
                            };
                            res = textMsg2.ToString();
                        }
                        break;
                    case "hp_photo_ticket":
                        //对当前hp_photo对应的照片进行打印认证
                        PrintCodeDAL printCodeDAL = new PrintCodeDAL();
                        TextRequestMsgModel temp1 = msgModel as TextRequestMsgModel;
                        string printCode = temp1.Content.Replace("#dy", "");
                        string clientID = printCodeDAL.GetClientIDByPrintCode(printCode,siteCode);
                        ExceptionLogDAL.InsertExceptionLog(new ExceptionLog() { Message=clientID});
                        if (!string.IsNullOrEmpty(clientID))
                        {
                            PhotoDAL photoDal1 = new PhotoDAL();
                            photoDal1.UpdatePrintInfo(printCode, clientID, siteCode, msgModel.FromUserName);
                            TextResponseMsgModel textMsg2 = new TextResponseMsgModel()
                            {
                                ToUserName = msgModel.FromUserName,
                                FromUserName = msgModel.ToUserName,
                                CreateTime = WeiXinHelper.ConvertDateTimeInt(DateTime.Now).ToString(),
                                Content = replyMsgValue == null ? string.Empty : TransformText(replyMsgValue, msgModel)
                            };
                            res = textMsg2.ToString();
                            //TextResponseMsgModel textMsg2 = new TextResponseMsgModel()
                            //{
                            //    ToUserName = msgModel.FromUserName,
                            //    FromUserName = msgModel.ToUserName,
                            //    CreateTime = WeiXinHelper.ConvertDateTimeInt(DateTime.Now).ToString(),
                            //    Content = "照片打印中，请稍侯..."
                            //};
                            //res = textMsg2.ToString();
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogDAL.InsertExceptionLog(ex);
            }
            return res;
        }

        public string GetSiteUrl()
        {
            return HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery, "");
        }

        public string GetPicUrl(string path)
        {
            //return string.Format("{0}/MicroSite/{1}", GetSiteUrl(), path);\
            return string.Format("{0}/{1}", GetSiteUrl(), path.Replace("\\","/"));
        }

        public string GetArticleUrl(string id)
        {
            return string.Format("{0}/MicroSite/ArticleDetail.aspx?ID={1}", GetSiteUrl(), id);
        }

        public string GetSiteInfo(string id)
        {
            return string.Format("{0}/MicroSite/Index.aspx?ID={1}", GetSiteUrl(), id);
        }

        public string TransformUrl(string url, RequestMsgModel msgModel)
        {
            string res = url;
            if (!url.Contains("http"))
            {
                res=string.Format("{0}{1}", GetSiteUrl(), url);
            }
            //替换命名常量
            res=res.Replace("{OpenID}", msgModel.FromUserName);
            res=res.Replace("{MsgType}", msgModel.MsgType.ToString());
            res = res.Replace("{SiteCode}", siteCode);
            return res;
        }

        /// <summary>
        /// 替换文本中的命名常量
        /// </summary>
        /// <param name="text"></param>
        /// <param name="msgModel"></param>
        /// <returns></returns>
        public string TransformText(string text, RequestMsgModel msgModel)
        {
            string res = text;
            //替换命名常量
            res = res.Replace("{OpenID}", msgModel.FromUserName);
            res = res.Replace("{MsgType}", msgModel.MsgType.ToString());
            res = res.Replace("{SiteCode}", siteCode);
            return res;
        }

        public string RemoveHtmlTag(string strHTML,int iTxtAmount )
        {
            string strSHTML = Regex.Replace(strHTML, "<[^>]*>", "");
            strSHTML = strSHTML.Replace("&nbsp;", "");
            strSHTML = strSHTML.Replace("\r", "");
            strSHTML = strSHTML.Replace("\n", "");
            //if (strSHTML.Length > iTxtAmount)
            //{
            //    return strSHTML.Substring(0, iTxtAmount) + "……";
            //}
            //else
            //{
            //    return strSHTML.Substring(0, strSHTML.Length);
            //}
            return strSHTML;
        }

    }
}