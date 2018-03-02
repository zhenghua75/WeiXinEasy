using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.CMS
{
    /// <summary>
    /// 评论模型
    /// </summary>
    public class CMS_Comment
    {
        private int iD;
        private string articleID = String.Empty;
        private string userName = String.Empty;
        private string content = String.Empty;
        private DateTime createTime;
        private int replyCount;
        public int ID
        {
            get { return this.iD; }
            set { this.iD = value; }
        }
        public string ArticleID
        {
            get { return this.articleID; }
            set { this.articleID = value; }
        }
        public string UserName
        {
            get { return this.userName; }
            set { this.userName = value; }
        }
        public string Content
        {
            get { return this.content; }
            set { this.content = value; }
        }
        public DateTime CreateTime
        {
            get { return this.createTime; }
            set { this.createTime = value; }
        }
        public int ReplyCount
        {
            get { return this.replyCount; }
            set { this.replyCount = value; }
        }
    }
}
