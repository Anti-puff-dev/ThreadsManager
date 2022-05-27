using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ThreadsManager
{
    public struct FN
    {
        public Func<string, bool> func;
        public String args;
    }

    public struct FNObj
    {
        public Func<Object, bool> func;
        public Object args;
    }

    public class Functions
    {
        public delegate void ThreadDelegate();

        public Functions(int p)
        {
            ThreadPool.SetMaxThreads(p, p);
        }

        public void AddFunction(Func<bool> MyFunc, Func<bool> CallBack)
        {
            Func<bool>[] fn = new Func<bool>[2];
            fn[0] = MyFunc;
            fn[1] = CallBack;
            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessThread), fn);
        }

        public void AddFunction(Func<string, bool> MyFunc, String args)
        {
            FN fn = new FN();
            fn.func = MyFunc;
            fn.args = args;
            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessThreadArgs), fn);
        }


        public void AddFunction(Func<Object, bool> MyFunc, Object args)
        {
            FNObj fn = new FNObj();
            fn.func = MyFunc;
            fn.args = args;
            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessThreadObj), fn);
        }


        private void ProcessThread(object a)
        {
            Func<bool>[] fn = (Func<bool>[])a;
            fn[0]();
            fn[1]();
        }

        private void ProcessThreadArgs(object a)
        {
            FN fn = (FN)a;
            fn.func(fn.args);
        }

        private void ProcessThreadObj(object a)
        {
            FNObj fn = (FNObj)a;
            fn.func(fn.args);
        }
    }
}
