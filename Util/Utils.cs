using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WBAssistantF
{
    internal static class Utils
    {
        public static T2[] FMap<T1, T2>(T1[] t1s, Func<T1, T2> f)
        {
            var t2s = new T2[t1s.Length];
            for (uint i = 0; i < t1s.Length; ++i)
                if (t1s[i] != null)
                    t2s[i] = f(t1s[i]);
            return t2s;
        }

        public static void ForAll<T>(T ts, Action<object> f) where T : IEnumerable
        {
            foreach (var x in ts) f(x);
        }


        public static int FirstWhich<T1>(T1[] t1s, Func<T1, bool> f)
        {
            for (var i = 0; i < t1s.Length; ++i)
                if (t1s[i] != null && f(t1s[i]))
                    return i;
            return -1;
        }

        public static T IfThen<T>(T t, Func<T, bool> f, Func<T, T> g)
        {
            return f(t) ? g(t) : t;
        }

        public static string Concat(IEnumerable<string> ts)
        {
            var ret = "";
            foreach (var t in ts)
                ret += t;
            return ret;
        }
        
        public static string[] Concat(string[][] ts)
        {
            List<string> ret = new List<string>();
            foreach (var t in ts)
                foreach (var t1 in t)
                    ret.Add((t1));
                
            return ret.ToArray();
        }

        public static string[] Merge(string[] ts1, string[] ts2)
        {
            return ts1.Union(ts2).ToArray();
        }

        public static int Concat(int[] ts)
        {
            var ret = 0;
            foreach (var t in ts)
                ret += t;
            return ret;
        }


        public static string Intersperse(string[] ts, char ch)
        {
            var ret = ts.Aggregate("", (current, t) => current + (t + ch));
            if (ts.Length >= 1)
                ret = ret[..^1];
            return ret;
        }
    }
}