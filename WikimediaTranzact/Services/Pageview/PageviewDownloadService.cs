using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Collections.Generic;

using Common        = Wikimedia_Tranzact.Utils.Common;
using PageviewModel = Wikimedia_Tranzact.Models.PageviewModel;
using PageviewView  = Wikimedia_Tranzact.Views.PageviewView;

namespace Wikimedia_Tranzact.Services.Pageview
{
    class PageviewDownloadService
    {
        /*
         * Properties
         */
        public static PageviewDownloadService instance;
        private int downloadCount;
        private int _lastHourPageviews;
        private PageviewView pageviewView;
        private List<string> _pageviewAddressList;
        private List<string> _pageviewNameList;
        private List<string> _downloadStatusList;
        private Dictionary<string, string> _statusList;

        public int lastHourPageviews { get { return _lastHourPageviews; } set { _lastHourPageviews = value; } }
        public List<string> pageviewAddressList { get { return _pageviewAddressList; } set { _pageviewAddressList = new List<string>(); } }
        public List<string> pageviewNameList { get { return _pageviewNameList; } set { _pageviewNameList = new List<string>(); } }
        public List<string> downloadStatusList { get { return _downloadStatusList; } set { _downloadStatusList = new List<string>(); } }
        public IDictionary<string, string> statusList { get { return _statusList; } set { _statusList = new Dictionary<string, string>(); } }


        /*
         * Constructor
         */
        public PageviewDownloadService()
        {
            //Create root directory
            this.createRootDirectory();

            //Page view download counter
            this.downloadCount = 0;

            //Total number of page views based on your time
            this.lastHourPageviews = 0;

            //Instantiate pageview view class  
            this.pageviewView = new Views.PageviewView();

            //Set page views address variable
            this.pageviewAddressList = new List<string>();

            //Set page views name variable
            this.pageviewNameList = new List<string>();

            //Pageview download status list
            this.downloadStatusList = new List<string>();

            //Status list
            this.statusList = new Dictionary<string, string>();
            this.statusList.Add("pending", "Pending");
            this.statusList.Add("completed", "Completed");
        }


        /*
         * Method - Return instance of class
         */
        public static PageviewDownloadService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PageviewDownloadService();
                }                
                return instance; 
            }
        }


        /*
         * Method - Create root directory
         */
        public void createRootDirectory()
        {
            string folderPath = PageviewModel.rootPath;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }


        /*
         * Method - Configure pageview datetime data
         * Param  - hourPageview: It's a page view number based on so many hours ago
         */
        public void ConfigureDatetimeData(int hourPageview = 0)
        {
            //Set the date and time of the page view based on the number of hours passed 
            PageviewDatetimeService.Instance.SetLastDatetimePageview(hourPageview);
        }


        /*
         * Method - Get pageview address
         */
        public string GetPageviewAddress()
        {
            return PageviewModel.pageviewUrl + PageviewDatetimeService.Instance.year + "/" + PageviewDatetimeService.Instance.year + "-" + PageviewDatetimeService.Instance.month + "/pageviews-" + PageviewDatetimeService.Instance.year + PageviewDatetimeService.Instance.month + PageviewDatetimeService.Instance.day + "-" + PageviewDatetimeService.Instance.hour + "0000.gz";
        }


        /*
         * Method - Get the custom name of the page view
         */
        public string GetPageviewName()
        {
            return PageviewModel.rootPath + @"\pageviews-" + PageviewDatetimeService.Instance.year + PageviewDatetimeService.Instance.month + PageviewDatetimeService.Instance.day + "-" + PageviewDatetimeService.Instance.hour + "0000.gz";
        }


        /*
         * Method - Get web page source code from pageviews
         */
        public void GetSourceCode()
        {
            PageviewDatetimeService.Instance.GetSourceCode();
        }


        /*
         * Method - Get the short name of the page view without the file path
         * Param  - fullName: full page view name including file path
         */
        public string GetPageviewShortName(string fullName = "")
        {
            //Split full name
            string[] split = fullName.Split("\\");

            //Return last part of long name
            return split[split.Length - 1];
        }


        /*
         * Method - Download a pageview 
         * Param  - hourPageview: It's a page view number based on your time
         */
        public void DownloadPageview(int hourPageview = 0)
        {
            if (this.lastHourPageviews > 0)
            {
                //Set pageview short name 
                string shortFileName = this.GetPageviewShortName(this.pageviewNameList[hourPageview]);

                try
                {
                    //Set variables for the download process
                    using (WebClient client = new WebClient())
                    {
                        Object LockObject                    = new Object();
                        TimeSpan DesiredProgressUpdatePeriod = TimeSpan.FromMilliseconds(1500);
                        long LastProgressUpdatePosition      = -1;
                        DateTime LastProgressUpdateTime      = DateTime.MinValue;

                        //Download progress
                        client.DownloadProgressChanged += (o, e) =>
                        {
                            //Prevent multipe concurrent thread to update progress at once
                            lock (LockObject)
                            {
                                //This prevents updating progress to value that is lower than what we already printed.
                                //This could happen when threads enters lock out of order.
                                if (LastProgressUpdatePosition > e.BytesReceived)
                                    return;

                                //This is not neccessary, but prevents you to miss 100% progress event ()
                                var isCompleted = e.TotalBytesToReceive != 0 && e.BytesReceived == e.TotalBytesToReceive;

                                //Check if desired time elapsed since last update
                                bool UpdatePeriodElapsed = DateTime.Now >= LastProgressUpdateTime + DesiredProgressUpdatePeriod;

                                if (isCompleted || UpdatePeriodElapsed)
                                {
                                    //If the download was complete
                                    if (isCompleted)
                                    {
                                        this.downloadStatusList[hourPageview] = this.statusList["completed"];
                                    }

                                    //Print message                                
                                    this.pageviewView.PrintDownloadMessage(shortFileName, e.ProgressPercentage, isCompleted, false, 0, hourPageview);

                                    LastProgressUpdatePosition = e.BytesReceived;
                                    LastProgressUpdateTime = DateTime.Now;
                                }
                            }
                        };

                        //Download process finished
                        client.DownloadFileCompleted += (sender, e) =>
                        {
                            //If the download was canceled
                            if (e.Cancelled)
                            {
                                //Print message
                                this.pageviewView.PrintDownloadMessage(shortFileName, 0, false, true, 0, hourPageview);
                            }

                            //If there was an error
                            if (e.Error != null)
                            {
                                //Print message
                                this.pageviewView.PrintDownloadMessage(shortFileName, 0, false, true, 0, hourPageview, e.Error);
                            }

                            //Update download counter
                            this.downloadCount++;

                            //If there are pending downloads
                            if (this.downloadCount < this.lastHourPageviews)
                            {
                                //Download next pageview
                                this.DownloadPageview(this.downloadCount);
                            }
                            else
                            {
                                //As long as the progress of the last download (client.DownloadProgressChanged)
                                //has not been updated, we will not show the final message of the downloads completed 
                                do
                                {
                                    Thread.Sleep(1000);
                                    if (this.downloadStatusList[hourPageview] == this.statusList["completed"])
                                    {
                                        //Print download completed message
                                        this.pageviewView.PrintDownloadsCompleted(null, 0, hourPageview);
                                    } 

                                } while (this.downloadStatusList[hourPageview] == this.statusList["pending"]);
                            }
                        };

                        //Download pageview to a local file as an asynchronous operation
                        client.DownloadFileAsync(new Uri(this.pageviewAddressList[hourPageview]), this.pageviewNameList[hourPageview]);

                        //Enable flag
                        Common.isDownloadingPageview = true;
                    }
                }
                catch
                {
                    //Print message
                    this.pageviewView.PrintDownloadMessage(shortFileName, 0, false, true, 0, hourPageview);
                }
            }
            else
            {
                this.pageviewView.PrintDownloadMessage(null, 0, false, true, 0, hourPageview);
            }
        }
    }
}
