using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiXinCore.Models;

namespace Mozart.WeiXin
{
    public interface IRequestMsgHandle
    {
        Dictionary<string, object> Params{get;set;}
        string Process(RequestMsgModel msg);
    }
}
