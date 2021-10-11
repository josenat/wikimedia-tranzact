using System.Collections.Generic;

using IMenu = Wikimedia_Tranzact.Interfaces.IMenu;

namespace Wikimedia_Tranzact.Models
{
    class DownloadMenuModel : IMenu
    {
        /*
         * Properties
         */
        private List<string> options;


        /*
         * Constructor
         */
        public DownloadMenuModel()
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
            this.options.Add("Enter the number of past hours you want to download (e.g. 5): ");

            return this.options;
        }
    }
}