using System;
using System.Net;
using System.Collections.Generic;

namespace Wikimedia_Tranzact.Services.Pageview
{
    class PageviewDatetimeService
    {
        /*
         * Properties
         */
        private static PageviewDatetimeService instance;
        private Dictionary<string, List<string>> dateFormatList;
        private Dictionary<string, List<string>> timeFormatList;
        private string _year;
        private string _month;
        private string _day;
        private string _hour;
        private int _lastHour;
        private string sourceCode;
        private List<string> dateTimeList;

        public string year { get { return _year; } set { _year = value; } }
        public string month { get { return _month; } set { _month = value; } }
        public string day { get { return _day; } set { _day = value; } }
        public string hour { get { return _hour; } set { _hour = value; } }
        public int lastHour { get { return _lastHour; } set { _lastHour = value; } }


        /*
         * Constructor
         */
        public PageviewDatetimeService()
        {
            //Initialize list of date formats            
            this.InitializeDateFormatList();

            //Set date format list
            this.SetDefaultDateFormat();

            //Initialize list of time formats
            this.InitializeTimeFormatList();

            //Set time format list
            this.SetDefaultTimeFormat();

            //Set current date 
            this.year  = DateTime.UtcNow.ToString(this.dateFormatList["year"][0]);
            this.month = DateTime.UtcNow.ToString(this.dateFormatList["month"][0]);
            this.day   = DateTime.UtcNow.ToString(this.dateFormatList["day"][0]);

            //Set current hour
            this.hour = DateTime.UtcNow.ToString(this.timeFormatList["hour"][0]);

            //Set last hour of pageview
            this.lastHour = 0;

            //Date time list
            this.dateTimeList = new List<string>();
        }


        /*
         * Method - Return instance of class
         */
        public static PageviewDatetimeService Instance
        {
            get
            { 
                if (instance == null)
                {
                    instance = new PageviewDatetimeService();
                }
                return instance;  
            }
        }


        /*
         * Method - Initialize list of date formats 
         */
        public void InitializeDateFormatList()
        {
            this.dateFormatList          = new Dictionary<string, List<string>>();
            this.dateFormatList["year"]  = new List<string>();
            this.dateFormatList["month"] = new List<string>();
            this.dateFormatList["day"]   = new List<string>();

        }


        /*
         * Method - Initialize list of time formats 
         */
        public void InitializeTimeFormatList()
        {
            this.timeFormatList           = new Dictionary<string, List<string>>();
            this.timeFormatList["hour"]   = new List<string>();
            this.timeFormatList["minute"] = new List<string>();
            this.timeFormatList["second"] = new List<string>();

        }


        /*
         * Method - Set date format
         * Param  - format: It's the date format
         */
        public void SetDateFormat(string format = "default")
        {
            //Reset list of date formats
            this.InitializeDateFormatList();

            //Evaluate format
            switch (format)
            {
                case "default":
                    this.SetDefaultDateFormat();
                    break;
                default:
                    this.SetDefaultDateFormat();
                    break;
            }
        }


        /*
         * Method - Get default date format
         */
        public void SetDefaultDateFormat()
        {
            this.dateFormatList["year"].Add("yyyy");
            this.dateFormatList["month"].Add("MM");
            this.dateFormatList["day"].Add("dd");
        }


        /*
         * Method - Set time format
         * Param  - format: It's the time format
         */
        public void SetTimeFormat(string format = "default")
        {
            //Reset list of time formats
            this.InitializeTimeFormatList();

            //Evaluate format
            switch (format)
            {
                case "default":
                    this.SetDefaultTimeFormat();
                    break;
                default:
                    this.SetDefaultTimeFormat();
                    break;
            }
        }


        /*
         * Method - Get default time format
         */
        public void SetDefaultTimeFormat()
        {
            this.timeFormatList["hour"].Add("HH");
            this.timeFormatList["minute"].Add("mm");
            this.timeFormatList["second"].Add("ss");
        }


        /*
         * Method - Get web page source code from pageviews
         */
        public void GetSourceCode()
        {
            string pageviewUrl = this.GetDefaultPageviewUrl();
            using (WebClient client = new WebClient())
            {
                this.sourceCode = client.DownloadString(pageviewUrl);
            };

            this.getDateTimeList();
        }


        /*
         * Method - Get datetime list of pageviews
         */
        public void getDateTimeList()
        {
            //Get datetime text
            this.dateTimeList.Clear();
            string text = GetDefaultTextSearchFormat();
            string[] split = this.sourceCode.Split(text);
            for (int n = 1; n < split.Length; n++)
            {
                this.dateTimeList.Add(split[n].Substring(11, 11));
            }
        }


        /*
         * Method - Set the date and time of the page view based on the number of hours passed 
         * Param  - hoursNum: number of the last hours of pageviews
         */
        public void SetLastDatetimePageview(int hoursNum = 1)
        {
            int count = this.dateTimeList.Count;
            if (count > 0)
            {
                for (int i = 0; i < hoursNum; i++)
                {
                    count--; 
                    //Set datetime 
                    this.year  = this.dateTimeList[count].Substring(0, 4);
                    this.month = this.dateTimeList[count].Substring(4, 2);
                    this.day   = this.dateTimeList[count].Substring(6, 2);
                    this.hour  = this.dateTimeList[count].Substring(9, 2);

                    //Set last hour
                    this.lastHour = Int32.Parse(this.hour);
                }                
            }
        }


        /*
         * Method - Get default pageview url
         */
        public string GetDefaultPageviewUrl()
        {
            return "https://dumps.wikimedia.org/other/pageviews/2021/2021-10/";
        }


        /*
         * Method - Get default text search format
         */
        public string GetDefaultTextSearchFormat()
        {
            //return "<a href=\"pageviews-";
            return "0000.gz\"";
        }


        /*
         * Method - Set past hour of page view
         * Param  - hours: It's the number of hours passed
         */
        public void SetPastHour(int hours = 0)
        {
            //Set time format
            this.SetTimeFormat("default");

            //Update number of page view
            hours++;

            this.hour = DateTime.UtcNow.AddHours(-hours).ToString(this.timeFormatList["hour"][0]);
        }

    }
}