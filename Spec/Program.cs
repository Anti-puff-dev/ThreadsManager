using System;
using System.Collections.Specialized;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Spec
{
    class Program
    {
        static int thread_count = 100;
        static ThreadsManager.Functions tm = new ThreadsManager.Functions(thread_count);

        static List<string> list = new List<string>();


        static void Main(string[] args)
        {
            Test1();
            Console.ReadKey();
        }




        static void Test1()
        {
            Console.WriteLine("Test1");
            //Populate list
            for (int i=0; i<1000; i++)
            {
                list.Add(i.ToString());
            }

            new Task(async() => await Init()).Start();
        }


        static async Task Init()
        {
            Console.WriteLine("init " + list.Count());
            if (list.Count() == 0)
            {
                Thread.Sleep(250);
                await Init();
            }
            else
            {
                for (int i = 0; i < 1000; i++)
                {
                    tm.AddFunction(Proc, "");
                }
            }
        }


        static bool Proc(string args)
        {
            string _queue = null;

            try
            {
                if (list.Count() == 0)
                {
                    Thread.Sleep(250);
                    new Task(async () => await Init()).Start();
                }
                else
                {
                    lock (list)
                    {
                        _queue = list[0];
                        list.RemoveAt(0);
                    }

                    if (_queue != null)
                    {
                        Int32 a = Convert.ToInt32(_queue) * 2;
                        Console.WriteLine(a.ToString());
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception err)
            {
                
            }

            return true;
        }
    }
}
