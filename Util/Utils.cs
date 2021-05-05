using System;
using System.Collections;

namespace WBAssistantF
{
    internal static class Utils
    {
        public static T2[] FMap<T1, T2>(T1[] t1s, Func<T1, T2> f)
        {
            T2[] t2s = new T2[t1s.Length];
            for (uint i = 0; i < t1s.Length; ++i)
            {
                if (t1s[i] != null)
                    t2s[i] = f(t1s[i]);
            }
            return t2s;
        }

        public static void ForAll<T>(T ts, Action<object> f) where T : IEnumerable
        {
            foreach (object x in ts)
            {
                f(x);
            }
        }


        public static int FirstWhich<T1>(T1[] t1s, Func<T1, bool> f)
        {
            for (int i = 0; i < t1s.Length; ++i)
            {
                if (t1s[i] != null && f(t1s[i]))
                    return i;
            }
            return -1;
        }

        public static T IfThen<T>(T t, Func<T, bool> f, Func<T, T> g)
        {
            return f(t) ? g(t) : t;
        }

        public static string Concat(string[] ts)
        {
            string ret = "";
            foreach (string t in ts)
                ret += t;
            return ret;
        }

        public static int Concat(int[] ts)
        {
            int ret = 0;
            foreach (int t in ts)
                ret += t;
            return ret;
        }


        public static string Intersperse(string[] ts, char ch)
        {
            string ret = "";
            foreach (string t in ts)
                ret += t + ch;
            if (ts.Length >= 1)
                ret = ret[..^1];
            return ret;
        }



    }
}
