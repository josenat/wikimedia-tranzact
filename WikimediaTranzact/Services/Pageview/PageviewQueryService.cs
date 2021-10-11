using System;
using System.IO;
using System.Linq;
using System.IO.Compression;
using System.Collections.Generic;

using MenuEnum       = Wikimedia_Tranzact.Utils.MenuEnum;
using PageviewModel  = Wikimedia_Tranzact.Models.PageviewModel;
using MenuController = Wikimedia_Tranzact.Controllers.MenuController;
using PageController = Wikimedia_Tranzact.Controllers.PageviewController;
using IPageviewQuery = Wikimedia_Tranzact.Interfaces.IPageviewQuery;

namespace Wikimedia_Tranzact.Services.Pageview
{
    class PageviewQueryService : IPageviewQuery
    {
        /*
         * Properties
         */
        private List<Dictionary<string, int>> dataTable;
        private string fileExt;
        private bool isDescendingOrder;


        /*
         * Constructor
         */
        public PageviewQueryService()
        {
            this.dataTable         = new List<Dictionary<string, int>>();
            this.fileExt           = ".gz";
            this.isDescendingOrder = true;
        }


        /*
         * Interface method - Get pageview records
         * Param  - recordNum: number of records
         */
        public void Select(int recordNum)
        {
            Console.Clear();

            //Download directory
            string rootPath = PageviewModel.rootPath;

            if (Directory.Exists(rootPath))
            {
                //Get all files from directory
                var directory = new DirectoryInfo(rootPath);

                //Get number of files to evaluate according to hours
                int lastHourNum = PageController.Instance.lastHourPageviews;

                //Get list of files
                List<FileInfo> fileList = directory
                                            .GetFiles("*" + this.fileExt)
                                            .OrderByDescending(f => f.LastWriteTime)
                                            .Take(lastHourNum)
                                            .ToList();                

                //If files exist
                if (fileList.Count > 0)
                {
                    List<StreamReader> unZipReaderList = new List<StreamReader>();
                     
                    for (int x = 0; x < fileList.Count; x++)
                    {
                        FileInfo fileToDecompress = fileList[x];
                        FileStream fileStream     = fileToDecompress.OpenRead();
                        GZipStream unZip          = new GZipStream(fileStream, CompressionMode.Decompress);
                        StreamReader unZipReader  = new StreamReader(unZip);
                        unZipReaderList.Add(unZipReader); 
                    }

                    int loop  = 0;
                    bool stop = false;
                    List<Dictionary<string, int>> table = new List<Dictionary<string, int>>();

                    do
                    {
                        int value      = 0;
                        string key     = "";
                        string[] split = {""};

                        for (int i = 0; i < unZipReaderList.Count; i++)
                        {
                            //Get and format new file line data
                            //----------------------------------------
                            //Get domain and title
                            var line = unZipReaderList[i].ReadLine(); 
                            if (line == null) continue;
                            split = line.Split(' ', 4);                                                        
                            key   = split[0] + " " + split[1];
                            //Get count views
                            value = Int32.Parse(split[2]);
                            //----------------------------------------

                            //Search in data table rows
                            foreach (var item in table)
                            {
                                //If the key already exists in our data table
                                if (item.ContainsKey(key))
                                {
                                    //Sum new value (count views)
                                    value += item[key];
                                    //Delete current row
                                    item.Remove(key);
                                }

                            }

                            //Add new row to table             
                            table.Add(new Dictionary<string, int>()
                            {
                                { key, value }
                            });
                        }

                        loop++;

                        //If the limit of records to get is reached
                        if (loop == recordNum) stop = true;

                    } while (stop == false);

                    //Update general table
                    this.dataTable = table;

                    //Sort data table
                    this.SortDataTableByNumberViews();

                    //Print data
                    this.printDataTable();

                    //Close the underlying stream
                    for (int i = 0; i < unZipReaderList.Count; i++)
                    {
                        unZipReaderList[i].Dispose();
                    }                    
                }
                else
                {
                    Console.Write("\n\t There are no page views. Please download.");
                }
            }
            else
            {
                Console.Write("\n\t Directory does not exist.");
            }

            Console.WriteLine("\n\n\t Press any key to return to the main menu...");
            Console.ReadKey();

            //Back to main menu
            MenuController.Instance.Run(MenuEnum.MainMenu);
        }


        /*
         * Method - Sort data table by number of views
         */
        public void SortDataTableByNumberViews()
        {
            if (this.dataTable.Count > 0)
            {
                //Auxiliary data table
                List<Dictionary<string, int>> auxDataTable = new List<Dictionary<string, int>>();

                //Auxiliary dictionary
                Dictionary<string, int> auxDictionary = new Dictionary<string, int>();

                //Reset indices
                int index = 0;
                this.dataTable.ForEach(row =>
                {
                    foreach (var column in row)
                    {
                        auxDataTable.Add(new Dictionary<string, int>());
                        auxDataTable[index].Add(column.Key, column.Value);
                        index++; 
                    }
                });
                 
                //Sort data table
                for (int i = 0; i < auxDataTable.Count; i++)
                {
                    for (int j = 0; j < (auxDataTable.Count-1); j++)
                    {
                        var currentRow   = auxDataTable[j];
                        int currentValue = 0;

                        //Get current value
                        foreach (var column in currentRow)
                        {                            
                            currentValue = column.Value;
                        }

                        var nextRow = auxDataTable[j + 1];
                        int nextValue = 0;

                        //Get next value
                        foreach (var column in nextRow)
                        {                            
                            nextValue = column.Value;
                        }
                        
                        //If the order is descending
                        if (this.isDescendingOrder == true)
                        {
                            if (currentValue < nextValue)
                            {
                                auxDictionary       = auxDataTable[j];
                                auxDataTable[j]     = auxDataTable[j + 1];
                                auxDataTable[j + 1] = auxDictionary;
                            }
                        } 
                        else
                        {
                            if (currentValue > nextValue)
                            {
                                auxDictionary       = auxDataTable[j];
                                auxDataTable[j]     = auxDataTable[j + 1];
                                auxDataTable[j + 1] = auxDictionary;
                            }
                        }

                    }
                }

                //Update ordered data table
                this.dataTable = auxDataTable; 
            }
        }


        /*
         * Method - Print data table
         */
        public void printDataTable()
        {
            if (this.dataTable.Count > 0)
            {
                Console.WriteLine("\n\t DOMAIN_CODE | PAGE_TITEL | CNT \n");

                this.dataTable.ForEach(row => 
                {
                    foreach (var colum in row)
                    {
                        Console.WriteLine("\t " + colum.Key + " " + colum.Value);
                    }
                });
            }
            else
            {
                Console.WriteLine("\n No data to print");
            }
        }
    }
}
