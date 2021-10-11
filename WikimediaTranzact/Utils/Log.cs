using System.IO;
using PageviewModel = Wikimedia_Tranzact.Models.PageviewModel;

namespace Wikimedia_Tranzact.Utils
{
    class Log
    {
        /*
         * Method - Pageview download log
         * Param  - message: text to save
         * Param  - isCompleted: if the download was completed
         * Param  - filePath: file path
         */
        public static void DownloadLog(string message = null, bool isCompleted = false, string filePath = null)
        {
            if (isCompleted)
            {
                if (filePath == null)
                {
                    filePath = PageviewModel.pageviewDownloadPath;
                }

                //Format message
                message = PageviewModel.logDatetime + "\t" + message + "\n\n";

                //Save log file
                File.AppendAllText(filePath, message);
            }
        }
    }
}
