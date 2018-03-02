using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    public static class Helper
    {
        public static string GetJson(HttpContext context, string fileName)
        {
            string path = context.Server.MapPath(string.Format("json/{0}.json", fileName));
            return File.ReadAllText(path, System.Text.Encoding.UTF8);
        }
        public static string GetHtmnlFragment(HttpContext context, string fileName)
        {
            string path = context.Server.MapPath(string.Format("HtmlFragment/{0}.html", fileName));
            return File.ReadAllText(path, System.Text.Encoding.UTF8);
        }
        public static void ProcessContext(HttpContext context,string text)
        {
            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.Write(text);
        }
        public static byte[] GetAppTemplateFile(int FileID)
        {
            string path = "";// Server.MapPath(FileID.ToString() + ".png");
            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }
            else
            {
                path = "";// Server.MapPath(FileID.ToString() + ".jpg");
                return File.ReadAllBytes(path);
            }
        }

        public static string GetRequestJsonObj(HttpContext context)
        {
            var jsonString = String.Empty;

            context.Request.InputStream.Position = 0;
            using (var inputStream = new StreamReader(context.Request.InputStream))
            {
                jsonString = inputStream.ReadToEnd();
            }
            return jsonString;
        }
    }
}