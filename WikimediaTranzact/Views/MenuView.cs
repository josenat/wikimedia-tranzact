using System;
using System.Collections.Generic;

using Common             = Wikimedia_Tranzact.Utils.Common;
using QueryEnum          = Wikimedia_Tranzact.Utils.QueryEnum;
using MenuController     = Wikimedia_Tranzact.Controllers.MenuController;
using PageviewController = Wikimedia_Tranzact.Controllers.PageviewController;

namespace Wikimedia_Tranzact.Views
{
    class MenuView
    {
        /*
         * Properties
         */
        private static string title = "Wikimedia Tranzact Project.";
        private string separator = new string('-', 100);
        private string menuName;
        private List<object> menuParam;
        private List<string> options;
        private int choice;
        private string errorText;


        /*
         * Constructor
         */
        public MenuView(string name = null, List<string> options = null, bool isError = false)
        {
            //Menu name
            this.menuName = name;

            //Menu options
            this.options = options;

            //Default parameters of menu methods
            this.menuParam = new List<object>();
            this.menuParam.Add(isError);

            //Default error message
            this.errorText = "Invalid option. Please try again!";
        }


        /*
         * Method - Return instance of class
         */
        public static MenuView Instance(string name = null, List<string> options = null, bool isError = false)
        {
            return new MenuView(name, options, isError);
        }


        /*
         * Method - Print main menu         
         */
        public void PrintMenu()
        {
            this.InvokeMethod(this.menuName, this.menuParam);
        }


        /*
         * Method - Print main menu
         * Param  - isError: option invalid
         */
        public void MainMenu(bool isError = false)
        {
            bool isRepeat;
            do
            {
                Console.Clear();
                Console.WriteLine($"\n\t {title} \n {separator}");

                isRepeat = false;
                if (isError)
                {
                    Console.WriteLine($"\n {this.errorText}");
                    isError = false;
                }

                for (int option = 0; option < this.options.Count; option++)
                {
                    if (option < (this.options.Count - 1))
                        Console.WriteLine($"\n\t {this.options[option]}");
                    else
                        Console.Write($"\n\t {this.options[option]}");
                }

                //Validate chosen option (integers only)
                Action getInput = () =>
                {
                    int.TryParse(Console.ReadLine(), out this.choice);
                };
                getInput();

                if (this.choice < 1 || this.choice > (this.options.Count - 1))
                {
                    isError = true;
                    isRepeat = true;
                }

            } while (isRepeat);

            //Update choice
            MenuController.Instance.choice = this.choice;
            //Run menu option
            MenuController.Instance.Run(Common.GetMenuEnumKeyByInt(this.choice));
        }


        /*
         * Method - Print download menu
         * Param  - isError: option invalid
         */
        public void DownloadPageviewMenu(bool isError = false)
        {
            if (isError)
            {
                Console.WriteLine($"\n\t\t {this.errorText}");
            }

            for (int option = 0; option < this.options.Count; option++)
            {
                if (option < (this.options.Count - 1))
                    Console.WriteLine($"\n {this.options[option]}");
                else
                    Console.Write($"\n {this.options[option]}");
            }

            //Validate chosen option (integers only)
            Action getInput = () =>
            {
                int.TryParse(Console.ReadLine(), out this.choice);
            };
            getInput();

            Console.Clear();
            Console.Write("\n Wait a moment...");

            //Download pageview
            PageviewController.Instance.DownloadPageview(this.choice);
        }


        /*
         * Method - Pageview query menu
         * Param  - isError: option invalid
         */
        public void PageviewQueryMenu(bool isError = false)
        {
            if (isError)
            {
                Console.WriteLine($"\n\t\t {this.errorText}");
            }

            for (int option = 0; option < this.options.Count; option++)
            {
                if (option < (this.options.Count - 1))
                    Console.WriteLine($"\n {this.options[option]}");
                else
                    Console.Write($"\n {this.options[option]}");
            }

            //Validate chosen option (integers only)
            Action getInput = () =>
            {
                int.TryParse(Console.ReadLine(), out this.choice);
            };
            getInput();

            //Consult pageviews
            PageviewController.Instance.Select(QueryEnum.PageviewQuery, this.choice);
        }


        /*
         * Method - Invoke method dynamically
         * Param  - method: method name
         * Param  - parameters: method parameters
         */
        public void InvokeMethod(string method = null, List<object> parameters = null)
        {
            //Call method
            Common.InvokeMethod(this, method, parameters);
        }
    }
}
