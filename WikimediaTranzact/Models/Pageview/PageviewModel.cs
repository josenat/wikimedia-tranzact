using System;
using System.IO;

using Common = Wikimedia_Tranzact.Utils.Common;

namespace Wikimedia_Tranzact.Models
{
    class PageviewModel
    {
        /*
         * Properties
         */
        public static char separator              = Path.AltDirectorySeparatorChar;
        public static string pageviewUrl          = "https://dumps.wikimedia.org/other/pageviews/";
        public static string rootPath             = Common.GetHomePath() + separator + "tranzact-wikimedia-pageview" + separator;
        public static string pageviewDownloadPath = rootPath + separator + "download-log.txt";        
        public static string logDatetime          = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
