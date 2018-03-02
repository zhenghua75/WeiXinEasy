/* ==============================================================================
 * 类名称：CommonHelper
 * 类描述：
 * 创建人：yhn,51809571@qq.com
 * 创建时间：2014/3/15 11:20:05
 * 修改人：
 * 修改时间：
 * 修改备注：
 * @version 1.0
 * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBUtility
{
    public class CommonHelper
    {
        public static T GetDataCellValue<T>(object obj, T defaultValue)
        {
            T res = defaultValue;
            if (obj != null && obj != DBNull.Value)
            {
                res = (T)obj;
            }
            return res;
        }
    }
}
