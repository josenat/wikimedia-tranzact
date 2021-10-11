using System.Collections.Generic;

using IMenu = Wikimedia_Tranzact.Interfaces.IMenu;

namespace Wikimedia_Tranzact.Models
{
    class MainMenuModel : IMenu
    {
        /*
         * Properties
         */
        private List<string> options;


        /*
         * Constructor
         */
        public MainMenuModel()
        {
            this.options = new List<string>();
        }


        /*
         * Interface method 
         */
        public List<string> GetMenu()
        {
            //Reset options
            if (this.options.Count > 0) this.options = new List<string>();

            //Add options
            this.options.Add("1. Download Pageviews");
            this.options.Add("2. Pageviews Query");
            this.options.Add("3. View Download History");
            this.options.Add("4. Exit");
            this.options.Add("Choose an option: ");

            return this.options;
        }
    }
}