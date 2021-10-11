using System.Collections.Generic;

using IPageviewQuery       = Wikimedia_Tranzact.Interfaces.IPageviewQuery;
using QueryEnum            = Wikimedia_Tranzact.Utils.QueryEnum;
using PageviewQueryService = Wikimedia_Tranzact.Services.Pageview.PageviewQueryService;

namespace Wikimedia_Tranzact.Factory
{
    class PageviewQueryFactory
    {
        /*
         * Properties
         */
        private static PageviewQueryFactory instance;
        private Dictionary<QueryEnum, IPageviewQuery> queryDictionary;


        /*
         * Constructor
         */
        private PageviewQueryFactory()
        {
            queryDictionary = new Dictionary<QueryEnum, IPageviewQuery>();

            queryDictionary.Add(QueryEnum.PageviewQuery, new PageviewQueryService());
        }


        /*
         * Method - Return instance of class
         */
        public static PageviewQueryFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PageviewQueryFactory();
                }
                return instance;
            }
        }


        /*
         * Method - Query interface
         * Param  - query: query type
         */
        public IPageviewQuery Select(QueryEnum query)
        {
            IPageviewQuery result = null;

            if (queryDictionary.ContainsKey(query))
            {
                //Get interface with selected object according to type of enumeration (query)
                result = queryDictionary[query];
            }

            return result;
        }
    }
}
