﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Mozart.PalmShop.ShopCode
{
    /// <summary>
    /// ValidateCode 的摘要说明
    /// </summary>
    public class ValidateCode : IHttpHandler, IRequiresSessionState //为要使用session实现的接口
    {
        public void ProcessRequest(HttpContext context)
        {
            HttpRequest Request = context.Request;
            int num = 6; string codname = "validate"; int minute = 2;
            if (Request["num"] != null && Request["num"] != "")
            {
                try
                {
                    num = Convert.ToInt32(Request["num"]);
                }
                catch (Exception)
                {
                }
            }
            if (Request["minute"] != null && Request["minute"] != "")
            {
                try
                {
                    minute = Convert.ToInt32(Request["minute"]);
                }
                catch (Exception)
                {
                }
            }
            if (Request["codname"] != null && Request["codname"] != "")
            {
                codname = Request["codname"].ToString();
            }
            string checkCode = GetValidation(num);  // 产生6位随机验证码字符
            //context.Session["Code"] = checkCode; //将字符串保存到Session中，以便需要时进行验证

            HttpCookie cookies = new HttpCookie(codname, checkCode);
            context.Response.AppendCookie(cookies);
            cookies.Expires = DateTime.Now.AddMinutes(minute);//设置cookie的有效期是分钟

            System.Drawing.Bitmap image = new System.Drawing.Bitmap(60, 20);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                // 画图片的背景噪音线
                int i;
                for (i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                Font font = new System.Drawing.Font("Arial", 12, (System.Drawing.FontStyle.Bold));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2F, true);
                g.DrawString(checkCode, font, brush, 2, 2);
                //画图片的前景噪音点
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                context.Response.ClearContent();
                context.Response.ContentType = "image/Gif";
                context.Response.BinaryWrite(ms.ToArray());
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }
        /// <summary>
        /// 随即获取验证码
        /// </summary>
        /// <param name="num">位数</param>
        /// <returns>返回验证码</returns>
        public string GetValidation(int num)
        {
            //string str = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"; //"或者写汉字也行"
            string str = "0123456789";
            string validatecode = "";
            Random rd = new Random();
            for (int i = 0; i < num; i++)
            {
                validatecode += str.Substring(rd.Next(0, str.Length), 1);
            }
            return validatecode;
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