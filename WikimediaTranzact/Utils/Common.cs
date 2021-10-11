using System;
using System.Linq;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;

namespace Wikimedia_Tranzact.Utils
{
    /*
     * General purpose functions to reuse throughout the application
     */
    class Common
    {
        /*
         * Properties
         */
        //If program finished
        private static bool _isFinished = false;
        public static bool isFinished { get { return _isFinished; } set { _isFinished = value; } }
        //If pageviews are downloading
        public static bool _isDownloadingPageview = false;
        public static bool isDownloadingPageview { get { return _isDownloadingPageview; } set { _isDownloadingPageview = value; } }


        /*
         * Method - Get value of environment variable home
         */
        public static string GetHomePath()
        {
            //Check platform type
            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
                return System.Environment.GetEnvironmentVariable("HOME");

            return System.Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
        }


        /*
         * Method - Configure console variables
         */
        public static void ConfigureConsole()
        {
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
        }


        /*
         * Method - Invoke method dynamically
         * Param  - instance: object class
         * Param  - method: method name
         * Param  - parameters: method parameters
         */
        public static void InvokeMethod(object instance = null, string method = null, List<object> parameters = null)
        {
            try
            {
                Type classType = instance.GetType();

                if (classType != null && method != null)
                {
                    //An instance of System.MethodInfo is obtained through the name of the method
                    MethodInfo mi = classType.GetMethod(method);

                    //Set parameter object. 
                    //null because by default we assume that the method has no parameters
                    object[] obj = null;

                    //If there are parameters
                    if (parameters != null)
                    {
                        obj = parameters.Cast<object>().ToArray();
                    }

                    //Call method
                    mi.Invoke(instance, obj);
                }
            }
            catch (Exception e)
            {
                Console.Write("Error invoking method dynamically. \n" + e.Message);
            }
        }


        /*
         * Method - Invoke static method 
         * Param  - classType: class type (e.g. typeof(Class))
         * Param  - method: method name
         * Param  - parameters: method parameters
         */
        public static void InvokeStaticClassMethod(Type classType = null, string method = null, List<object> parameters = null)
        {
            try
            {
                if (classType != null && method != null)
                {
                    //An instance of System.MethodInfo is obtained through the name of the method
                    MethodInfo mi = classType.GetMethod(method);

                    //Set parameter object. 
                    //null because by default we assume that the method has no parameters
                    object[] obj = null;

                    //If there are parameters
                    if (parameters != null)
                    {
                        obj = parameters.Cast<object>().ToArray();
                    }

                    //Call method.
                    //the first parameter is null, because the method to be invoked is a class static method 
                    mi.Invoke(null, obj);
                }
            }
            catch (Exception e)
            {
                Console.Write("Error invoking static method. \n" + e.Message);
            }
        }

        /*
         * Method - Get menu enum key by integer
         * Param  - integer: integer number
         */
        public static MenuEnum GetMenuEnumKeyByInt(int integer = 0)
        {
            MenuEnum result = 0;
            if (integer > 0)
            {
                foreach (var value in Enum.GetValues(typeof(MenuEnum)))
                {
                    if (integer == (int)value)
                    {
                        return (MenuEnum)value;
                    }
                }
            }

            return result;
        }


        /*
         * Method - Pause the system until the user chooses to exit
         * Param  - milliseconds: duration of the pause 
         */
        public static void SystemPause(int milliseconds = 60000)
        {
            //if the system is finished
            if (isFinished == true) return;

            //Sleep for x seconds
            Thread.Sleep(milliseconds);

            //Continue pause
            SystemPause(milliseconds);
        }
    }
}