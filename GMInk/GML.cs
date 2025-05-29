using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DllExport;

namespace GMWolf.GML
{
    public static class GML
    {
        private delegate void GmlEventPerformAsyncDelegate(int a, int b);
        private delegate int GmlDSMapCreateDelegate(int num);
        private delegate bool GmlDSMapAddDoubleDelegate(int index, [MarshalAs(UnmanagedType.LPStr)] string key, double value);
        private delegate bool GmlDSMapAddStringDelegate(int index, [MarshalAs(UnmanagedType.LPStr)] string key, [MarshalAs(UnmanagedType.LPStr)] string value);

        private static GmlEventPerformAsyncDelegate? GmlEventPerformAsync;
        private static GmlDSMapCreateDelegate? GmlDSMapCreate;
        private static GmlDSMapAddDoubleDelegate? GmlDSMapAddDouble;
        private static GmlDSMapAddStringDelegate? GmlDSMapAddString;

        private const int EVENT_OTHER_SOCIAL = 70;

        [DllExport("RegisterCallbacks", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe double RegisterCallbacks(IntPtr arg1, IntPtr arg2, IntPtr arg3, IntPtr arg4)
        {
            try
            {
                GmlEventPerformAsync = Marshal.GetDelegateForFunctionPointer<GmlEventPerformAsyncDelegate>(arg1);
                GmlDSMapCreate = Marshal.GetDelegateForFunctionPointer<GmlDSMapCreateDelegate>(arg2);
                GmlDSMapAddDouble = Marshal.GetDelegateForFunctionPointer<GmlDSMapAddDoubleDelegate>(arg3);
                GmlDSMapAddString = Marshal.GetDelegateForFunctionPointer<GmlDSMapAddStringDelegate>(arg4);
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public static void EventPerformAsync(Dictionary<string, object>? dictionary)
        {
            if (GmlEventPerformAsync != null && GmlDSMapCreate != null)
            {
                GmlEventPerformAsync(dictionary?.ToGmlMap() ?? GmlDSMapCreate(0), EVENT_OTHER_SOCIAL);
            }
        }

        private static int ToGmlMap(this Dictionary<string, object> dictionary)
        {
            if (GmlDSMapCreate == null) return 0;
            
            int id = GmlDSMapCreate(0);

            foreach (string key in dictionary.Keys)
            {
                GmlDSMapAdd(id, key, dictionary[key]);
            }

            return id;
        }

        private static void GmlDSMapAdd(int id, string key, object value)
        {
            switch (value)
            {
                case int intValue:
                    GmlDSMapAddDouble?.Invoke(id, key, intValue);
                    break;
                case double doubleValue:
                    GmlDSMapAddDouble?.Invoke(id, key, doubleValue);
                    break;
                case float floatValue:
                    GmlDSMapAddDouble?.Invoke(id, key, floatValue);
                    break;
                case string stringValue:
                    GmlDSMapAddString?.Invoke(id, key, stringValue);
                    break;
                case Dictionary<string, object> dictValue:
                    GmlDSMapAddDouble?.Invoke(id, key, dictValue.ToGmlMap());
                    break;
                default:
                    GmlDSMapAddString?.Invoke(id, key, value?.ToString() ?? "");
                    break;
            }
        }

        private static int ToGmlMap(this Dictionary<string, string> d)
        {
            if (GmlDSMapCreate == null) return 0;
            
            int id = GmlDSMapCreate(0);

            foreach (string key in d.Keys)
            {
                GmlDSMapAddString?.Invoke(id, key, d[key]);
            }

            return id;
        }

        private static int ToGmlMap(this Dictionary<string, Dictionary<string, object>> d)
        {
            if (GmlDSMapCreate == null) return 0;
            
            int id = GmlDSMapCreate(0);

            foreach (string key in d.Keys)
            {
                GmlDSMapAddDouble?.Invoke(id, key, d[key].ToGmlMap());
            }

            return id;
        }

        public static object CallScript(double script, params object[] args)
        {
            Dictionary<string, object> map = new Dictionary<string, object>()
            {
                {"type", "script" },
                {"script",  script}
            };
            
            int n = 0;
            foreach (object arg in args)
            {
                map[n++.ToString()] = arg;
            }

            EventPerformAsync(map);

            return 0;
        }
    }
}