namespace Wikimedia_Tranzact.Interfaces
{
    interface IPageviewQuery 
    {
        /*
         * Method - Get pageview records
         * Param  - recordNum: number of records
         */
        public void Select(int recordNum);
    }
}
