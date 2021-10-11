using Common         = Wikimedia_Tranzact.Utils.Common;
using MenuEnum       = Wikimedia_Tranzact.Utils.MenuEnum;
using MenuController = Wikimedia_Tranzact.Controllers.MenuController;

namespace Wikimedia_Tranzact
{
    class Program
    {
        /*
         * Application main method
         */
        static void Main()
        {
            //Configure console 
            Common.ConfigureConsole();

            //Run main menu
            MenuController.Instance.Run(MenuEnum.MainMenu);

            //Pause the system
            Common.SystemPause();                         
        }
    }
}