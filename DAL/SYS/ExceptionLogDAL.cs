using DAL.Common;
using Model.SYS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.SYS
{
    public class ExceptionLogDAL
    {


        /// <summary>
        /// 插入异常日志信息
        /// </summary>
        /// <param name="log"></param>
        public static void InsertExceptionLog(Exception ex)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("Message", ex.Message);
            dict.Add("Source", ex.Source);
            dict.Add("StackTrace", ex.StackTrace);
            SQLHelperExt.Insert("SYS_ExceptionLog", dict);
        }

        /// <summary>
        /// 插入异常日志信息
        /// </summary>
        /// <param name="log"></param>
        public static void InsertExceptionLog(ExceptionLog ex)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("Message", ex.Message);
            dict.Add("Source", ex.Source);
            dict.Add("StackTrace", ex.StackTrace);
            SQLHelperExt.Insert("SYS_ExceptionLog", dict);
        }

        /// <summary>
        /// 插入异常日志信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="source">消息源</param>
        /// <param name="stackTrace">消息堆栈信息</param>
        public static void InsertExceptionInfo(string message, string source="", string stackTrace="")
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("Message", message);
            dict.Add("Source", source);
            dict.Add("StackTrace", stackTrace);
            SQLHelperExt.Insert("SYS_ExceptionLog", dict);
        }
    }
}
