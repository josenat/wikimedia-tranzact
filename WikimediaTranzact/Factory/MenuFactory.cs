using System.Collections.Generic;

using IMenu                 = Wikimedia_Tranzact.Interfaces.IMenu;
using MenuEnum              = Wikimedia_Tranzact.Utils.MenuEnum;
using MainMenuModel         = Wikimedia_Tranzact.Models.MainMenuModel;
using ConsultPageviewModel  = Wikimedia_Tranzact.Models.QueryMenuModel;
using DownloadPageviewModel = Wikimedia_Tranzact.Models.DownloadMenuModel;

namespace Wikimedia_Tranzact.Factory
{
    class MenuFactory
    {
        /*
         * Properties
         */
        private static MenuFactory instance;
        private Dictionary<MenuEnum,   IMenu> menuDictionary;


        /*
         * Constructor
         */
        private MenuFactory()
        {
            menuDictionary = new Dictionary<MenuEnum, IMenu>();

            menuDictionary.Add(MenuEnum.MainMenu, new MainMenuModel());
            menuDictionary.Add(MenuEnum.DownloadPageviewMenu, new DownloadPageviewModel());
            menuDictionary.Add(MenuEnum.PageviewQueryMenu, new ConsultPageviewModel());
        }


        /*
         * Method - Return instance of class
         */
        public static MenuFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MenuFactory();
                }
                return instance;
            }
        }


        /*
         * Method - Menu interface
         * Param  - menu: menu type
         */
        public IMenu GetMenu(MenuEnum menu)
        {

            IMenu result = null;

            if (menuDictionary.ContainsKey(menu))
            {
                //Get interface with selected object according to type of enumeration (menu)
                result = menuDictionary[menu];
            }

            return result;
        }
    }
}
