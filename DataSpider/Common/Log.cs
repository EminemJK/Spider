using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSpider.Common
{ 
    public static class Log
    {
        private static readonly log4net.ILog logMng = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void Info(string msg)
        {
            logMng.Info(msg);
        }
        public static void Error(string msg)
        {
            logMng.Error(msg);
        }
    }
}
