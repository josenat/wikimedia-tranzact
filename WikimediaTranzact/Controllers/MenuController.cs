using System;
using System.Collections.Generic;

using Common       = Wikimedia_Tranzact.Utils.Common;
using IMenu        = Wikimedia_Tranzact.Interfaces.IMenu;
using MenuEnum     = Wikimedia_Tranzact.Utils.MenuEnum;
using MenuFactory  = Wikimedia_Tranzact.Factory.MenuFactory;
using MenuView     = Wikimedia_Tranzact.Views.MenuView;
using PageviewView = Wikimedia_Tranzact.Views.PageviewView;

namespace Wikimedia_Tranzact.Controllers
{
    /*
     * Class - System menus
     */
    class MenuController
    {
        /*
         * Properties
         */
        private static MenuController instance;
        private int _choice;
        private string _menuName;
        private MenuEnum _menuNum;
        private List<string> _options;
        public int choice { get => _choice; set => _choice = value; }
        public string menuName { get => _menuName; set => _menuName = value; }
        public MenuEnum menuEnum { get => _menuNum; set => _menuNum = value; }
        public List<string> options { get => _options; set => _options = value; }


        /* 
         * Constructor
         */
        public MenuController()
        {
            //Menu name
            this.menuName = MenuEnum.MainMenu.ToString();

            //Menu number
            this.menuEnum = MenuEnum.MainMenu;

            //Menu options
            this.options = new List<string>();

            //Choice of menu
            this.choice = 0;
        }


        /*
         * Method - Return instance of class
         */
        public static MenuController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MenuController();
                }
                return instance;
            }
        }


        /*
         * Method - Run menu get process
         * Param  - menuEnum: It's the type of menu
         */
        public void Run(MenuEnum menuEnum = 0)
        {
            /*----------------------------------------------------------------------------*
             * For the purposes of this test project, we will not implement an interface 
             * for the following options:
             */
            if (menuEnum == MenuEnum.MainMenu)
            {
                //If option three was chosen from the main menu
                if (this.choice == 3)
                {
                    //Reset choice
                    this.choice = 0;
                    //Run page view download history menu
                    PageviewView.Instance.PrintDownloadMessageList();
                }
                else if (this.choice == 4)
                {
                    //Finished system
                    Environment.Exit(0);
                }
            }
            /*----------------------------------------------------------------------------*/

            //Menu name
            this.menuName = menuEnum.ToString();

            //Update menu number
            this.menuEnum = menuEnum;

            //Get menu options
            this.options = this.GetMenu(menuEnum);

            //Print menu
            this.PrintMenu(this.menuName, this.options);
        }


        /*
         * Method - Get menu options
         * Param  - menuEnum: type of menu
         */
        public List<string> GetMenu(MenuEnum menuEnum = 0)
        {
            //Get interface with the desired object based on the given enum
            IMenu menu = MenuFactory.Instance.GetMenu(menuEnum);
            //Get implementation of the desired method
            List<string> result = menu.GetMenu();

            return result;
        }


        /*
         * Method - Print menu
         * Param  - menuName: menu name
         * Param  - options: menu options
         * Param  - isError: option invalid
         */
        public void PrintMenu(string menuName = null, List<string> options = null, bool isError = false)
        {
            //If pageviews are downloading
            if (Common.isDownloadingPageview == true)
            {
                //Don't print any menu
                return;
            }

            //Print menu
            MenuView.Instance(menuName, options, isError).PrintMenu();
        }
    }
}
