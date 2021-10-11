using System;

using MenuEnum                = Wikimedia_Tranzact.Utils.MenuEnum;
using QueryEnum               = Wikimedia_Tranzact.Utils.QueryEnum;
using IPageviewQuery          = Wikimedia_Tranzact.Interfaces.IPageviewQuery;
using PageviewQueryFactory    = Wikimedia_Tranzact.Factory.PageviewQueryFactory;
using PageviewDownloadService = Wikimedia_Tranzact.Services.Pageview.PageviewDownloadService;

namespace Wikimedia_Tranzact.Controllers
{
    /*
     * Class - Page views for Wikipedia site
     */
    class PageviewController
    {
        /*
         * Properties
         */
        private static PageviewController instance;
        private int _lastHourPageviews;
        public int lastHourPageviews { get => _lastHourPageviews; set => _lastHourPageviews = value; }


        /*
         * Constructor
         */
        public PageviewController()
        {
            //Set default number of all page views to download
            this.lastHourPageviews = 5;
        }


        /*
         * Method - Return instance of class
         */
        public static PageviewController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PageviewController();
                }
                return instance; 
            }
        }


        /*
         * Method - Request the download of page views according to the last hours ()
         * Param  - lastHours: It's the number of all the page views to download.
         */
        public void DownloadPageview(int lastHours = 5)
        {
            //Validate
            if (lastHours > 0)
            {
                //Get number of page views to download
                this.lastHourPageviews = lastHours;

                //Reset object
                PageviewDownloadService.instance = null;

                //Get total number of page views based on your time
                PageviewDownloadService.Instance.lastHourPageviews = this.lastHourPageviews;

                //Get web page source code from pageviews
                PageviewDownloadService.Instance.GetSourceCode();

                //Configure data required to download each pageviews
                for (int hourPageview = 1; hourPageview <= this.lastHourPageviews; hourPageview++)
                {                    
                    //Configure time data of pageview
                    PageviewDownloadService.Instance.ConfigureDatetimeData(hourPageview);

                    //Get pageview address from "n" hours ago
                    PageviewDownloadService.Instance.pageviewAddressList.Add(PageviewDownloadService.Instance.GetPageviewAddress());

                    //Get pageview name from "n" hours ago
                    PageviewDownloadService.Instance.pageviewNameList.Add(PageviewDownloadService.Instance.GetPageviewName());

                    //Set the default status of pageview from "n" hours
                    PageviewDownloadService.Instance.downloadStatusList.Add(PageviewDownloadService.Instance.statusList["pending"]);
                } 

                //Run pageview downloads 
                PageviewDownloadService.Instance.DownloadPageview();
            }
            else
            {
                Console.Clear();
                Console.Write("\n\t Error. Invalid data entry.");
                Console.WriteLine("\n\n\t Press any key to return to the main menu...");
                Console.ReadKey();

                //Back to main menu
                MenuController.Instance.Run(MenuEnum.MainMenu);
            }
        }


        /*
         * Method - Consult pageviews
         * Param  - queryEnum: type of query
         * Param  - recordNum: number of records
         */
        public void Select(QueryEnum queryEnum, int recordNum = 0)
        {
            if (recordNum > 0)
            {
                //Get interface with the desired object based on the given enum
                IPageviewQuery query = PageviewQueryFactory.Instance.Select(queryEnum);

                //Get implementation of the desired method
                query.Select(recordNum);
            }
            else
            {
                Console.Clear();
                Console.Write("\n\t Error. Invalid data entry.");
                Console.WriteLine("\n\n\t Press any key to return to the main menu...");
                Console.ReadKey();

                //Back to main menu
                MenuController.Instance.Run(MenuEnum.MainMenu);
            }

        }
    }
}
