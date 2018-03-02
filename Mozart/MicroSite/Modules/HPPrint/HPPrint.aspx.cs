using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Web.UI.WebControls;
using System.Data;
using DAL.SYS;

namespace Mozart.HPPrint
{
    public partial class HPPrint : System.Web.UI.Page
    {
        public string sitecode = string.Empty;
        public string strcodeimg = string.Empty;
        string name = string.Empty;
        string printimg1 = string.Empty;
        string printimg2 = string.Empty;
        string printimg3 = string.Empty;
        string printimg4 = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["sitecode"] != null && Request["sitecode"] != "")
                {
                    WriteXml();
                }
                else
                {
                    return;
                }
            }
        }
        #region IO流把字符串写入文件
        /// <summary>
        /// IO流把字符串写入文件
        /// </summary>
        void WriteXml()
        {
            string word = string.Empty;
            word = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n"+
                    "<data>\r\n";
            #region ---------------settings-------------------
            word += "<settings>	\r\n"+
		                "<auto_play>\r\n"+
			                "<defaults symbol=\"circular\" /><!--倒计时按钮的位置-->\r\n"+
			                "<tweenIn x=\"895\" y=\"45\" width=\"30\" height=\"30\" tint=\"0xFFFFFF\" alpha=\"0.5\"/>\r\n"+
			                "<tweenOver alpha=\"1\"/>\r\n"+
		                "</auto_play>\r\n"+
		                "<prev_button><!--左右翻页按钮的位置-->	\r\n"+			
			                "<tweenIn x=\"865\" y=\"300\" width=\"30\" height=\"30\" alpha=\"0\" />\r\n"+
			                "<tweenOver alpha=\"0\" />\r\n"+
		                "</prev_button>\r\n"+
		                "<next_button>		\r\n"+	
			                "<tweenIn x=\"895\" y=\"300\" width=\"30\" height\"30\" alpha=\"0\" />\r\n"+
			                "<tweenOver alpha=\"0\" />\r\n"+
		                "</next_button>\r\n"+
		                "<prev_symbol>\r\n"+
			                "<defaults type=\"3\" />\r\n"+
			                "<tweenIn x=\"865\" y=\"300\" alpha=\"0.5\"  />\r\n"+
			                "<tweenOver time=\"0.15\" x=\"860\" scaleX=\"1.1\" scaleY=\"1.1\" />\r\n"+
		                "</prev_symbol>\r\n"+
		                "<next_symbol>\r\n"+
			                "<defaults type=\"3\" />\r\n"+
			                "<tweenIn x=\"895\" y=\"300\" alpha=\"0.5\"/>\r\n"+
			                "<tweenOver time=\"0.15\" x=\"900\" scaleX=\"1.1\" scaleY=\"1.1\" />\r\n"+
		                "</next_symbol>		\r\n"+
		                "<description>\r\n"+
			                "<defaults round_corners=\"10, 10, 10, 10\"	heading_text_size=\"22\" heading_text_color=\"0xfc9900\" paragraph_text_size=\"13\" paragraph_text_color=\"0xFFFFFF\"/>\r\n"+
			                "<tweenIn x=\"200\" y=\"240\" width=\"560\" height=\"90\" alpha=\"0.15\" />\r\n"+
			                "<tweenOver alpha=\"0.3\"/>\r\n"+
		                "</description>	\r\n"+
		                "<transitions slicing=\"vertical\"  direction=\"down\" duration=\"0.6\"  delay=\"0.2\" cube_color=\"0x611811\"/>\r\n"+
                    "</settings>\r\n";
            #endregion
            word += GetSlides();
            word += "</data>";
            string xmlurl = HttpContext.Current.Server.MapPath("/HPPrint/");
            string xmlname = @""+sitecode+ "config.xml";
            #region ----------------------文件写入------------------
            StreamWriter sr = null;
            try
            {
                if (!Directory.Exists(xmlurl))
                {
                    Directory.CreateDirectory(xmlurl);
                }
                if (!File.Exists(xmlurl + xmlname))
                {
                    sr = File.CreateText(xmlurl +xmlname);
                }
                else
                {
                    sr = File.AppendText(xmlurl + xmlname);
                }
                sr.Close();
                sr = new
                    StreamWriter(HttpContext.Current.Server.MapPath("/HPPrint/"+ xmlname), false, System.Text.Encoding.Default);
                sr.Write(word);
                sr.Close();
            }
            catch
            {

            }
            finally
            {
                sr.Close();
            }
            #endregion
            if (strcodeimg.Trim() != null && strcodeimg.Trim() != "")
            {
                strcodeimg = "<img src=\"" + strcodeimg + "\" alt=\"" + name + "\" />";
            }
        }
        #endregion
        #region 获取xml格式图片连接地址
        /// <summary>
        /// 获取xml格式图片连接地址
        /// </summary>
        /// <returns></returns>
        string GetSlides()
        {
            string word = string.Empty;
            int xmlLine = 10;
            word += "<slides>\r\n";
            sitecode=Common.Common.NoHtml(Request["sitecode"]);
            Account_ExtDAL dal = new Account_ExtDAL();
            DataSet ds = dal.GetPrintImgBySiteCode(sitecode);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                name = ds.Tables[0].Rows[0]["Name"].ToString();
                strcodeimg = ds.Tables[0].Rows[0]["CodeImg"].ToString();
                printimg1 = ds.Tables[0].Rows[0]["PrintImg1"].ToString();
                printimg2 = ds.Tables[0].Rows[0]["PrintImg2"].ToString();
                printimg3 = ds.Tables[0].Rows[0]["PrintImg3"].ToString();
                printimg4 = ds.Tables[0].Rows[0]["PrintImg4"].ToString();
                #region ---------------图片列表排列-------------
                for (int i = 0; i < xmlLine; i++)
                {
                    switch (i)
                    {
                        case 0:
                            word += "<slide>\r\n" +
                                "<url>" + GetPrintImg(printimg1,1) + "</url>\r\n" +
                                        "<link target=\"_blank\">#</link>\r\n" +
                                    "</slide>\r\n" +
                                    "<transition duration=\"0.6\" delay=\".2\" direction=\"down\"/>\r\n";
                            break;
                        case 1:
                            word += "<slide>\r\n" +
                                        "<url>" + GetPrintImg(printimg2, 2) + "</url>\r\n" +
                                        "<link target=\"_blank\">#</link>\r\n" +
                                    "</slide>\r\n" +
                                    "<transition num=\"3\" slicing=\"horizontal\" direction=\"left\" delay=\"0.05\"/>\r\n";
                            break;
                        case 2:
                            word += "<slide>\r\n" +
                                        "<url>" + GetPrintImg(printimg3, 3) + "</url>\r\n" +
                                        "<link target=\"_blank\">#</link>\r\n" +
                                    "</slide>\r\n" +
                                    "<transition num=\"3\"/>\r\n";
                            break;
                        case 3:
                            word += "<slide>\r\n" +
                                        "<url>" + GetPrintImg(printimg4, 4) + "</url>\r\n" +
                                        "<link target=\"_blank\">#</link>\r\n" +
                                    "</slide>\r\n" +
                                    "<transition  num=\"6\" slicing=\"horizontal\" direction=\"right\" duration=\"0.8\"  delay=\"0.05\" z_multiplier=\"5\"/>\r\n";
                            break;
                        case 4:
                            word += "<slide>\r\n" +
                                        "<url>" + GetPrintImg(printimg1, 1) + "</url>\r\n" +
                                        "<link target=\"_blank\">#</link>\r\n" +
                                    "</slide>\r\n" +
                                    "<transition  num=\"6\" slicing=\"vertical\" direction=\"down\" shader=\"phong\" delay=\"0.05\"/>\r\n";
                            break;
                        case 5:
                            word += "<slide>\r\n" +
                                        "<url>" + GetPrintImg(printimg2, 2) + "</url>\r\n" +
                                        "<link target=\"_blank\">#</link>\r\n" +
                                    "</slide>\r\n" +
                                    "<transition  num=\"4\" direction=\"down\" slicing=\"horizontal\" z_multiplier=\"6\" delay=\"0.1\"/>\r\n";
                            break;
                        case 6:
                            word += "<slide>\r\n" +
                                        "<url>" + GetPrintImg(printimg3, 3) + "</url>\r\n" +
                                        "<link target=\"_blank\">#</link>\r\n" +
                                    "</slide>\r\n" +
                                    "<transition  num=\"4\" direction=\"up\" z_multiplier=\"2.5\" delay=\"0.03\"/>\r\n";
                            break;
                        case 7:
                            word += "<slide>\r\n" +
                                        "<url>" + GetPrintImg(printimg4, 4) + "</url>\r\n" +
                                        "<link target=\"_blank\">#</link>\r\n" +
                                    "</slide>\r\n" +
                                    "<transition  direction=\"right\"/>\r\n";
                            break;
                        case 8:
                            word += "<slide>\r\n" +
                                        "<url>" + GetPrintImg(printimg1, 1) + "</url>\r\n" +
                                        "<link target=\"_blank\">#</link>\r\n" +
                                    "</slide>\r\n" +
                                    "<transition  direction=\"up\"/>\r\n";
                            break;
                        case 9:
                            word += "<slide>\r\n" +
                                        "<url>" + GetPrintImg(printimg2, 2) + "</url>\r\n" +
                                        "<link target=\"_blank\">#</link>\r\n" +
                                    "</slide>\r\n";
                            break;
                    }
                }
                #endregion
            }
            word += "</slides>\r\n";
            return word;
        }
        #endregion
        #region 判断图片地址
        /// <summary>
        ///  判断图片地址
        /// </summary>
        /// <param name="img"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        string GetPrintImg(string img,int num)
        {
            if (img != null && img != "")
            {
                return img;
            }
            else
            {
                switch (num)
                {
                    case 1:
                        if (printimg2 != null && printimg2 != "")
                        {
                            img = printimg2; break;
                        }
                        else if (printimg3 != null && printimg3 != "")
                        {
                            img = printimg3; break;
                        }
                        else if (printimg4 != null && printimg4 != "")
                        {
                            img = printimg4; break;
                        }
                        else {
                            img = ""; break;
                        }
                    case 2:
                       if (printimg3 != null && printimg3 != "")
                        {
                            img = printimg3; break;
                        }
                        else if (printimg4 != null && printimg4 != "")
                        {
                            img = printimg4; break;
                        }
                       else if (printimg1 != null && printimg1 != "")
                       {
                           img = printimg1; break;
                       }
                       else
                       {
                           img = ""; break;
                       }
                    case 3:
                       if (printimg4 != null && printimg4 != "")
                       {
                           img = printimg4; break;
                       }
                       else if (printimg1 != null && printimg1 != "")
                       {
                           img = printimg1; break;
                       }
                       else if (printimg2 != null && printimg2 != "")
                       {
                           img = printimg2; break;
                       }
                       else
                       {
                           img = ""; break;
                       }
                    case 4:
                       if (printimg1 != null && printimg1 != "")
                       {
                           img = printimg1; break;
                       }
                       else if (printimg2 != null && printimg2 != "")
                       {
                           img = printimg2; break;
                       }
                       else if (printimg3 != null && printimg3 != "")
                       {
                           img = printimg3; break;
                       }
                       else
                       {
                           img = ""; break;
                       }
                }
            }
            return img;
        }
        #endregion
    }
}