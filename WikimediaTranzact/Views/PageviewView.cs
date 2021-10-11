using System;
using System.IO;

using Log                = Wikimedia_Tranzact.Utils.Log;
using Common             = Wikimedia_Tranzact.Utils.Common;
using MenuEnum           = Wikimedia_Tranzact.Utils.MenuEnum;
using MenuController     = Wikimedia_Tranzact.Controllers.MenuController;
using PageviewController = Wikimedia_Tranzact.Controllers.PageviewController;
using PageviewModel      = Wikimedia_Tranzact.Models.PageviewModel;

namespace Wikimedia_Tranzact.Views
{
    /*
     * Representation of the data to be displayed
     */
    class PageviewView
    {
        /*
         * Properties
         */
        private static PageviewView instance;
        private string pageviewDownloadPath;
        private bool isScreenClean;


        /*
         * Constructor
         */
        public PageviewView()
        {
            //File path of pageview download message list
            this.pageviewDownloadPath = PageviewModel.pageviewDownloadPath;

            //If the screen is clean
            this.isScreenClean = false;
        }


        /*
         * Method - Return instance of class
         */
        public static PageviewView Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PageviewView();
                }
                return instance;
            }
        }


        /*
         * Method - Show pageview download message
         * Param  - fileName: file name to download
         * Param  - progress: download progress percentage
         * Param  - isCompleted: download process completed
         * Param  - isError: operation error
         * Param  - left: left coordinate of the screen
         * Param  - top: top coordinate of the screen
         * Param  - error: system error message
         */
        public void PrintDownloadMessage(string fileName = null, int progress = 0, bool isCompleted = false, bool isError = false, int left = 0, int top = 0, Exception error = null)
        {
            bool isExit = false;
            if (error != null)
            {
                Console.WriteLine($"\n{error}");
                isExit = true;
            }

            if (fileName == null)
            {
                Console.Write("\n\t Error. Invalid data entry.");
                Console.WriteLine("\n\n\t Press any key to return to the main menu...");
                isExit = true;
            }

            if (isExit == true)
            {
                Console.ReadKey();
                //Back to main menu
                MenuController.Instance.Run(MenuEnum.MainMenu);
            }

            //If the screen is clean
            if (this.isScreenClean == false)
            {
                Console.Clear();
                this.isScreenClean = true;
            }

            //Set download message
            string message = $"File {fileName} - Downloading: {progress}%";

            if (isCompleted)
            {
                message = $"File {fileName} - Successfully downloaded.";
            }

            if (isError)
            {
                message = $"File {fileName} - Could not be downloaded.";
                message += "\n\t An error occurred while downloading. Check the Internet connection (Retry).";
            }

            //Print message
            Console.SetCursorPosition(left, top);
            Console.WriteLine("\n\t" + message);

            //Save pageview download log
            Log.DownloadLog(message, isCompleted);
        }


        /*
         * Method - Print message of all completed downloads
         * Param  - left: left coordinate of the screen
         * Param  - top: top coordinate of the screen
         */
        public void PrintDownloadsCompleted(string text = null, int left = 0, int top = 0)
        {
            text = (text == null) ? "Download of all pageviews completed." : "";
            string message = $"------------{text}------------";

            Console.SetCursorPosition(left, top);
            Console.WriteLine("\n\n\n\t" + message);            

            Console.WriteLine("\n\n\t Press any key to return to the main menu...");
            Console.ReadKey();

            //Disable flag
            Common.isDownloadingPageview = false;

            //Back to main menu
            MenuController.Instance.Run(MenuEnum.MainMenu);
        }


        /*
         * Method - Print download message list
         */
        public void PrintDownloadMessageList(string filePath = null)
        {
            Console.Clear();
            Console.WriteLine("\n\n Pageview download history: \n\n");

            if (filePath == null)
            {
                filePath = this.pageviewDownloadPath;
            }          

            if (File.Exists(filePath))
            {
                using (StreamReader textStream = File.OpenText(filePath))
                {
                    string text = textStream.ReadToEnd();
                    Console.WriteLine(text);
                }
            }
            else
            {
                Console.Write("\t No records. \n\n");
            }

            //Back to main menu
            Console.WriteLine("\n Press any key to return to the main menu...");
            Console.ReadKey();

            //Back to main menu
            MenuController.Instance.Run(MenuEnum.MainMenu);
        }
    }
}
