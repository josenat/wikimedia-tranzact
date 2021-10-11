using System.Collections.Generic;

using IMenu = Wikimedia_Tranzact.Interfaces.IMenu;

namespace Wikimedia_Tranzact.Models
{
    class QueryMenuModel : IMenu
    {
        /*
         * Properties
         */
        private List<string> options;


        /*
         * Constructor
         */
        public QueryMenuModel()
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
            this.options.Add("Enter how many records you want to see (e.g. 100): ");

            return this.options;
        }
    }
}