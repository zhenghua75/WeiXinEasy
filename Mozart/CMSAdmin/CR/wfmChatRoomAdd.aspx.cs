﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Model.CR;
using DAL.CR;
using Mozart.Common;

namespace Mozart.CMSAdmin.CR
{
    public partial class wfmChatRoomAdd : System.Web.UI.Page
    {
        static string strID = string.Empty;
        string strMessage = string.Empty;
        string strAction = string.Empty;
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Session["strSiteCode"].ToString()) && Session["strRoleCode"].ToString() != "ADMIN")
            {
                Response.Write("<script language=JavaScript>parent.location.href='../Index.aspx';</script>");
                Response.End();
            }
            if (!IsPostBack)
            {
                #region 初始化界面

                if (null != Common.Common.NoHtml(Request.QueryString["action"]))
                {
                    strAction = Common.Common.NoHtml(Request.QueryString["action"]);
                }
                if (null != Common.Common.NoHtml(Request.QueryString["id"]))
                {
                    strID = Common.Common.NoHtml(Request.QueryString["id"]);
                }
                #endregion
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (null == Session["strSiteName"] || null == Session["strSiteCode"] || null == Session["strLoginName"])
            {
                Response.Write("<script language=JavaScript>;parent.location.href='../Index.aspx';</script>");
                Response.End();
            }
            if (RoomName.Text.Trim() != null && RoomName.Text.Trim() != "")
            {
                ChatRoomDAL dal = new ChatRoomDAL();
                int roommaxnum = Convert.ToInt32(dal.GetMaxRoomNum());
                roommaxnum = roommaxnum + 1;
                string strSiteCode=Session["strSiteCode"].ToString();
                if (dal.ExistChatRoom(strSiteCode, RoomName.Text))
                {
                    MessageBox.Show(this, "该房间已经存在！");
                }
                else if (dal.ExistChatRoomNum(strSiteCode, roommaxnum))
                {
                    MessageBox.Show(this, "该房间已经存在！");
                }
                else
                {
                    //上传图像
                    string strIconFileName = string.Empty;//图像路径
                    string strIconSaveFileName = string.Empty;//网址路径
                    try
                    {
                        if (this.file0.PostedFile.FileName == "")
                        {
                            strIconSaveFileName = "";
                        }
                        else
                        {
                            if (!System.IO.Directory.Exists(Server.MapPath("~") + @"/Images"))
                            {
                                System.IO.Directory.CreateDirectory(Server.MapPath("~") + @"/Images");
                            }
                            if (!System.IO.Directory.Exists(String.Format(@"{0}/WXWall/images/{1}", Server.MapPath("~"), Session["strSiteCode"].ToString())))
                            {
                                System.IO.Directory.CreateDirectory(String.Format(@"{0}/WXWall/images/{1}", Server.MapPath("~"), Session["strSiteCode"].ToString()));
                            }
                            string orignalName = this.file0.PostedFile.FileName;//获取客户机上传文件的文件名
                            string extendName = orignalName.Substring(orignalName.LastIndexOf("."));//获取扩展名

                            if (extendName != ".gif" && extendName != ".jpg" && extendName != ".jpeg" && extendName != ".png")
                            {
                                MessageBox.Show(this, "文件格式有误！");
                                return;

                            }//检查文件格式
                            string newName = String.Format("{0}_{1}{2}", DateTime.Now.Millisecond, file0.PostedFile.ContentLength, extendName);//对文件进行重命名
                            strIconFileName = String.Format(@"{0}WXWall/images/{1}/{2}", Server.MapPath("~"), Session["strSiteCode"].ToString(), newName);
                            strIconSaveFileName = String.Format(@"images/{0}/{1}", Session["strSiteCode"].ToString(), newName);
                            file0.PostedFile.SaveAs(strIconFileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "上传发生错误！原因是：" + ex.ToString());
                    }

                    //上传二维码图像
                    string strcodeIconFileName = string.Empty;//图像路径
                    string strcodeIconSaveFileName = string.Empty;//网址路径
                    try
                    {
                        if (this.file1.PostedFile.FileName == "")
                        {
                            strcodeIconSaveFileName = "";
                        }
                        else
                        {
                            if (!System.IO.Directory.Exists(Server.MapPath("~") + @"/Images"))
                            {
                                System.IO.Directory.CreateDirectory(Server.MapPath("~") + @"/Images");
                            }
                            if (!System.IO.Directory.Exists(String.Format(@"{0}/WXWall/images/{1}", Server.MapPath("~"), Session["strSiteCode"].ToString())))
                            {
                                System.IO.Directory.CreateDirectory(String.Format(@"{0}/WXWall/images/{1}", Server.MapPath("~"), Session["strSiteCode"].ToString()));
                            }
                            string orignalName = this.file1.PostedFile.FileName;//获取客户机上传文件的文件名
                            string extendName = orignalName.Substring(orignalName.LastIndexOf("."));//获取扩展名

                            if (extendName != ".gif" && extendName != ".jpg" && extendName != ".jpeg" && extendName != ".png")
                            {
                                MessageBox.Show(this, "文件格式有误！");
                                return;

                            }//检查文件格式
                            string newName = String.Format("{0}_{1}{2}", DateTime.Now.Millisecond, file1.PostedFile.ContentLength, extendName);//对文件进行重命名
                            strcodeIconFileName = String.Format(@"{0}WXWall/images/{1}/{2}", Server.MapPath("~"), Session["strSiteCode"].ToString(), newName);
                            strcodeIconSaveFileName = String.Format(@"images/{0}/{1}", Session["strSiteCode"].ToString(), newName);
                            file1.PostedFile.SaveAs(strcodeIconFileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "上传发生错误！原因是：" + ex.ToString());
                    }

                    ChatRoom model = new ChatRoom();
                    model.RoomName = RoomName.Text;
                    model.RoomDesc = hd_content.Value;
                    model.RoomImg = strIconSaveFileName;
                    model.WebCodeImg = strcodeIconSaveFileName;
                    if (phonenum.Text.Trim() != null && phonenum.Text.Trim() != "")
                    {
                        model.PhoneNum = Convert.ToInt32(phonenum.Text);
                    }
                    if (appid.Text.Trim() != null && appid.Text.Trim() != "")
                    {
                        model.AppID = appid.Text;
                    }
                    model.SiteCode = Session["strSiteCode"].ToString();
                    model.IsDel = 0;
                    model.ID = roommaxnum;
                    if (dal.AddChatRoom(model))
                    {
                        MessageBox.Show(this, "操作成功！");
                    }
                    else
                    {
                        MessageBox.Show(this, "操作失败！");
                    }
                }
            }
            else
            {
                MessageBox.Show(this, "请输入相应标题名称！");
            }

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            RoomName.Text = ""; phonenum.Text = "";
            hd_content.Value = "";
        }
    }
}