using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.MiniShop;
using Model.MiniShop;
using Mozart.Common;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;

namespace Mozart.CMSAdmin.MiniShop
{
    public partial class wfmPhotoSubmitUpdate : System.Web.UI.Page
    {
        static string strID = string.Empty;
        string strMessage = string.Empty;
        string strAction = string.Empty;
        public static string imgsrc1 = string.Empty;
        public static string imgsrc2 = string.Empty;
        string strSFilePath = string.Empty;
        string strFilePath = string.Empty;
        public static string userid = string.Empty;
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["strLoginName"].ToString() != null && Session["strLoginName"].ToString() != ""&&
                    Session["strSiteCode"].ToString() != null && Session["strSiteCode"].ToString() != "")
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
                    showdetailinfo();
                    #endregion
                }
                else
                {
                    return;
                }
            }
        }
        void showdetailinfo()
        {
            printimg.Items.Clear();
            printimg.Items.Insert(0, new ListItem("--请选择需打印的照片--", ""));
            MSPhotoSubmitDAL photoDal = new MSPhotoSubmitDAL();
            DataSet photods = photoDal.GetPhotoSubmitDetail(strID);
            MSPhotoSubmit photomodel = DataConvert.DataRowToModel<MSPhotoSubmit>(photods.Tables[0].Rows[0]);
            ordernum.Text = photomodel.OrderNum;
            imgsrc1 = "../../PalmShop/ShopCode/" + photomodel.Img1;
            imgsrc2 = "../../PalmShop/ShopCode/" + photomodel.Img2;
            img1.Src = imgsrc1;
            img2.Src = imgsrc2;
            userid = photomodel.UID;
            printimg.Items.Insert(1, new ListItem("照片1", photomodel.Img1));
            printimg.Items.Insert(2, new ListItem("照片2", photomodel.Img2));
            printimg.DataBind();
            if (photomodel.Reivew == 0)
            {
                rno.Checked = true;
            }
            else
            {
                ryes.Checked = true;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Session["strLoginName"].ToString() != null && Session["strLoginName"].ToString() != "")
            {
                if (strID != null && strID != "")
                {
                    MSPhotoSubmitDAL photoDal = new MSPhotoSubmitDAL();
                    int pass = rno.Checked ? 0 : 1;
                    photoDal.UpdatePhotoSubmitRv(strID, pass);
                    if (printimg.SelectedValue != null && printimg.SelectedValue != "")
                    {
                        PrintPhoto(printimg.SelectedValue);
                    }
                    MessageBox.Show(this, "操作成功！");
                }
                else
                {
                    MessageBox.Show(this, "操作失败，请重新操作");
                }
            }
            else
            {
                MessageBox.Show(this, "操作失败，请重新操作！");
            }
        }
        void PrintPhoto(string imgsrc)
        {
            string imgname = string.Empty;
            string[] imgarray = imgsrc.Split('.');
            imgname = imgarray[0];
            string openid = string.Empty;
            if (userid != null && userid != "")
            {
                MSCustomersDAL customerDal = new MSCustomersDAL();
                try
                {
                    openid = customerDal.GetCustomerValueByID("OpenID", userid).ToString();
                }
                catch (Exception)
                {
                }
            }
            if (openid == null || openid == "")
            {
                openid = strID;
            }

            DAL.HP.PhotoDAL dalPhoto = new DAL.HP.PhotoDAL();
            //照片处理
            strSFilePath = "../../PalmShop/ShopCode/" + imgsrc;
            strFilePath = imgsrc;
            string[] urls = strSFilePath.Split('.');
            string _url = openid + "." + urls.Last();
            string saveurl = "../../HP_Photo/";
            saveurl = Server.MapPath(saveurl);
            if (!Directory.Exists(saveurl))
            {
                Directory.CreateDirectory(saveurl);
            }
            string inputurl = Server.MapPath("../../PalmShop/ShopCode/") + strFilePath;
            string outputurl = Server.MapPath("../../HP_Photo/") + _url;

            int width = 260;
            int height =310;

            System.IO.FileStream fs = new System.IO.FileStream(inputurl, System.IO.FileMode.Open);
            //ZoomAuto(fs, outputurl,width, height, "", "");
            CutForCustom(fs, outputurl, width, height, 100);
            fs.Close();

            string SiteCode = "VYIGO";
            if (Session["strSiteCode"].ToString() != null && Session["strSiteCode"].ToString() != "")
            {
                SiteCode = Session["strSiteCode"].ToString();
            }

            // DAL.HP.PrintCodeDAL dalPrintCode = new DAL.HP.PrintCodeDAL();
            // DataSet printds = dalPrintCode.AddPrintCode(1, SiteCode,"0000", "2014-01-01", "2019-12-31");
            //string strPID = string.Empty;
            //if (printds != null && printds.Tables.Count > 0 && printds.Tables[0].Rows.Count > 0)
            //{
            //    strPID = printds.Tables[0].Rows[0]["ID"].ToString();
            //}

            string strPID = Guid.NewGuid().ToString("N");
            Model.HP.Photo modelPhoto = new Model.HP.Photo()
            {
                ID = strPID,
                OpenId = openid,
                SiteCode = SiteCode,
                ClientID = "WSY01",
                PrintCode = "1111",
                Img = openid + "." + imgsrc.Split('.').Last(),
                AttachText = AttachText.Text + "\r\n"
            };
            dalPhoto.InsertInfo(modelPhoto);

            //插入V币记录
            if (userid != null && userid != "" && strPID != null && strPID != "")
            {
                int award = GetAwardChance();
                MSVAcct msvModel = new MSVAcct();
                MSVAcctDAL msvDal = new MSVAcctDAL();
                MSVAcctDetail msvdetailModel = new MSVAcctDetail();
                MSVAcctDetailDAL msvdetailDal = new MSVAcctDetailDAL();
                if (!msvDal.ExistMSVAcct(userid, SiteCode))
                {
                    msvModel.CustID = userid;
                    msvModel.SiteCode = SiteCode;
                    msvModel.V_Amont = award;
                    msvDal.AddMSVAcct(msvModel);
                }
                else
                {

                    int count = Convert.ToInt32(msvDal.GetMSVAcct("V_Amont", userid).ToString());
                    count = count + award;
                    msvModel.CustID = userid;
                    msvModel.SiteCode = SiteCode;
                    msvModel.V_Amont = count;
                    msvDal.UpdateMSVAcct(msvModel);
                }
                msvdetailModel.CustID = userid;
                msvdetailModel.Amount = award;
                msvdetailModel.ChargeType = "首次购物";
                msvdetailModel.Ext_Fld1 = strPID;
                msvdetailModel.SiteCode = SiteCode;
                msvdetailDal.AddMSVAcctDetail(msvdetailModel);
            }
            //插入活动券
            if (!string.IsNullOrEmpty(openid))
            {
                string strGuid = Guid.NewGuid().ToString("N");
                DAL.ACT.CouponDAL cdal = new DAL.ACT.CouponDAL();
                if (!cdal.ExistCoupon(SiteCode, "56DBFD79AFF94FD6B0FE7E72CE7589E6", openid))
                {
                    Model.ACT.Coupon coupon = null;
                    coupon = new Model.ACT.Coupon()
                    {
                        ID = strGuid,
                        SiteCode = SiteCode,
                        SiteActivityID = "56DBFD79AFF94FD6B0FE7E72CE7589E6",
                        OpenID = openid,
                        CouponStatus = 0
                    };
                    cdal.InsertInfo(coupon);
                }
            }
        }
        #region 自定义裁剪并缩放

        /// <summary>
        /// 指定长宽裁剪
        /// 按模版比例最大范围的裁剪图片并缩放至模版尺寸
        /// </summary>
        /// <remarks>吴剑 2012-08-08</remarks>
        /// <param name="fromFile">原图Stream对象</param>
        /// <param name="fileSaveUrl">保存路径</param>
        /// <param name="maxWidth">最大宽(单位:px)</param>
        /// <param name="maxHeight">最大高(单位:px)</param>
        /// <param name="quality">质量（范围0-100）</param>
        public static void CutForCustom(System.IO.Stream fromFile, string fileSaveUrl, int maxWidth, int maxHeight, int quality)
        {
            //从文件获取原始图片，并使用流中嵌入的颜色管理信息
            System.Drawing.Image initImage = System.Drawing.Image.FromStream(fromFile, true);

            //原图宽高均小于模版，不作处理，直接保存
            if (initImage.Width <= maxWidth && initImage.Height <= maxHeight)
            {
                initImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
            {
                //模版的宽高比例
                double templateRate = (double)maxWidth / maxHeight;
                //原图片的宽高比例
                double initRate = (double)initImage.Width / initImage.Height;

                //原图与模版比例相等，直接缩放
                if (templateRate == initRate)
                {
                    //按模版大小生成最终图片
                    System.Drawing.Image templateImage = new System.Drawing.Bitmap(maxWidth, maxHeight);
                    System.Drawing.Graphics templateG = System.Drawing.Graphics.FromImage(templateImage);
                    templateG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    templateG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    templateG.Clear(Color.White);
                    templateG.DrawImage(initImage, new System.Drawing.Rectangle(0, 0, maxWidth, maxHeight), new System.Drawing.Rectangle(0, 0, initImage.Width, initImage.Height), System.Drawing.GraphicsUnit.Pixel);
                    templateImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                //原图与模版比例不等，裁剪后缩放
                else
                {
                    //裁剪对象
                    System.Drawing.Image pickedImage = null;
                    System.Drawing.Graphics pickedG = null;

                    //定位
                    Rectangle fromR = new Rectangle(0, 0, 0, 0);//原图裁剪定位
                    Rectangle toR = new Rectangle(0, 0, 0, 0);//目标定位

                    //宽为标准进行裁剪
                    if (templateRate > initRate)
                    {
                        //裁剪对象实例化
                        pickedImage = new System.Drawing.Bitmap(initImage.Width, (int)System.Math.Floor(initImage.Width / templateRate));
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);

                        //裁剪源定位
                        fromR.X = 0;
                        fromR.Y = (int)System.Math.Floor((initImage.Height - initImage.Width / templateRate) / 2);
                        fromR.Width = initImage.Width;
                        fromR.Height = (int)System.Math.Floor(initImage.Width / templateRate);

                        //裁剪目标定位
                        toR.X = 0;
                        toR.Y = 0;
                        toR.Width = initImage.Width;
                        toR.Height = (int)System.Math.Floor(initImage.Width / templateRate);
                    }
                    //高为标准进行裁剪
                    else
                    {
                        pickedImage = new System.Drawing.Bitmap((int)System.Math.Floor(initImage.Height * templateRate), initImage.Height);
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);

                        fromR.X = (int)System.Math.Floor((initImage.Width - initImage.Height * templateRate) / 2);
                        fromR.Y = 0;
                        fromR.Width = (int)System.Math.Floor(initImage.Height * templateRate);
                        fromR.Height = initImage.Height;

                        toR.X = 0;
                        toR.Y = 0;
                        toR.Width = (int)System.Math.Floor(initImage.Height * templateRate);
                        toR.Height = initImage.Height;
                    }

                    //设置质量
                    pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    //裁剪
                    pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);

                    //按模版大小生成最终图片
                    System.Drawing.Image templateImage = new System.Drawing.Bitmap(maxWidth, maxHeight);
                    System.Drawing.Graphics templateG = System.Drawing.Graphics.FromImage(templateImage);
                    templateG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    templateG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    templateG.Clear(Color.White);
                    templateG.DrawImage(pickedImage, new System.Drawing.Rectangle(0, 0, maxWidth, maxHeight), new System.Drawing.Rectangle(0, 0, pickedImage.Width, pickedImage.Height), System.Drawing.GraphicsUnit.Pixel);

                    //关键质量控制
                    //获取系统编码类型数组,包含了jpeg,bmp,png,gif,tiff
                    ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
                    ImageCodecInfo ici = null;
                    foreach (ImageCodecInfo i in icis)
                    {
                        if (i.MimeType == "image/jpeg" || i.MimeType == "image/bmp" || i.MimeType == "image/png" || i.MimeType == "image/gif")
                        {
                            ici = i;
                        }
                    }
                    EncoderParameters ep = new EncoderParameters(1);
                    ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);

                    //保存缩略图
                    templateImage.Save(fileSaveUrl, ici, ep);
                    //templateImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);

                    //释放资源
                    templateG.Dispose();
                    templateImage.Dispose();

                    pickedG.Dispose();
                    pickedImage.Dispose();
                }
            }

            //释放资源
            initImage.Dispose();
        }
        #endregion

        #region 按概率取中奖
        /// <summary>
        /// 获取奖项金额
        /// </summary>
        /// <returns></returns>
        int GetAwardChance()
        {
            int award = 0;
            Random random = new Random();
            int r1 = (int)(new Random(GetRandomSeed()).Next(0, 100));
            if (10 < r1 && r1 < 60)
            {
                award = 1;
            }
            if (61 < r1 && r1 < 80)
            {
                award = 2;
            }
            if (81 < r1 && r1 < 90)
            {
                award = 3;
            }
            if (91 < r1 && r1 < 97)
            {
                award = 4;
            }
            if (98 < r1 && r1 < 100)
            {
                award = 5;
            }
            return award;
        }

        public int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        #endregion
    }
}