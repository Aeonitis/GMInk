using System;
using System.IO;
using System.Runtime.InteropServices;
using Ink.Runtime;
using DllExport;

namespace GMWolf.GMInk
{
    using GML;

    public class GMInk
    {
        private static Story? story;

        [DllExport("Load", CallingConvention = CallingConvention.Cdecl)]
        public static double Load([MarshalAs(UnmanagedType.LPStr)] string file)
        {
            try
            {
                string json = File.ReadAllText(file);
                story = new Story(json);
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        [DllExport("CanContinue", CallingConvention = CallingConvention.Cdecl)]
        public static double CanContinue()
        {
            return (story?.canContinue ?? false) ? 1 : 0;
        }

        [DllExport("Continue", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static string Continue()
        {
            return story?.Continue() ?? "";
        }

        [DllExport("CurrentChoicesCount", CallingConvention = CallingConvention.Cdecl)]
        public static double CurrentChoicesCount()
        {
            return story?.currentChoices.Count ?? 0;
        }

        [DllExport("CurrentChoices", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static string CurrentChoice(double i)
        {
            if (story?.currentChoices == null || i < 0 || i >= story.currentChoices.Count)
                return "";
            
            return story.currentChoices[(int)i]?.text ?? "";
        }

        [DllExport("ChooseChoiceIndex", CallingConvention = CallingConvention.Cdecl)]
        public static void ChooseChoiceIndex(double i)
        {
            story?.ChooseChoiceIndex((int)i);
        }

        [DllExport("SaveState", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static string SaveState()
        {
            return story?.state.ToJson() ?? "";
        }

        [DllExport("LoadState", CallingConvention = CallingConvention.Cdecl)]
        public static void LoadState([MarshalAs(UnmanagedType.LPStr)] string json)
        {
            if (story != null && !string.IsNullOrEmpty(json))
            {
                story.state.LoadJson(json);
            }
        }

        [DllExport("TagCount", CallingConvention = CallingConvention.Cdecl)]
        public static double TagCount()
        {
            return TagCountInternal();
        }

        public static double TagCountInternal()
        {
            return story?.currentTags?.Count ?? 0;
        }

        [DllExport("GetTag", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static string GetTag(double i)
        {
            if ((int)i < TagCountInternal() && i >= 0)
            {
                return story?.currentTags?[(int)i] ?? "";
            }
            else
            {
                return "";
            }
        }

        [DllExport("TagForContentAtPathCount", CallingConvention = CallingConvention.Cdecl)]
        public static double TagForContentAtPathCount([MarshalAs(UnmanagedType.LPStr)] string path)
        {
            return story?.TagsForContentAtPath(path)?.Count ?? 0;
        }

        [DllExport("TagForContentAtPath", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static string TagForContentAtPath([MarshalAs(UnmanagedType.LPStr)] string path, double i)
        {
            return story?.TagsForContentAtPath(path)?[(int)i] ?? "";
        }

        [DllExport("GlobalTagCount", CallingConvention = CallingConvention.Cdecl)]
        public static double GlobalTagCount()
        {
            return story?.globalTags?.Count ?? 0;
        }

        [DllExport("GlobalTag", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static string GlobalTag(double i)
        {
            return story?.globalTags?[(int)i] ?? "";
        }

        [DllExport("ChoosePathString", CallingConvention = CallingConvention.Cdecl)]
        public static void ChoosePathString([MarshalAs(UnmanagedType.LPStr)] string path)
        {
            story?.ChoosePathString(path);
        }

        [DllExport("VariableGetReal", CallingConvention = CallingConvention.Cdecl)]
        public static double VariableGetReal([MarshalAs(UnmanagedType.LPStr)] string var)
        {
            object? o = story?.variablesState?[var];
            if (o == null) return 0;
            
            if (double.TryParse(o.ToString(), out double result))
                return result;
            
            return 0;
        }

        [DllExport("VariableGetString", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static string VariableGetString([MarshalAs(UnmanagedType.LPStr)] string var)
        {
            return (story?.variablesState?[var])?.ToString() ?? "";
        }

        [DllExport("VariableSetReal", CallingConvention = CallingConvention.Cdecl)]
        public static void VariableSetReal([MarshalAs(UnmanagedType.LPStr)] string var, double value)
        {
            if (story != null)
            {
                story.variablesState[var] = value;
            }
        }

        [DllExport("VariableSetString", CallingConvention = CallingConvention.Cdecl)]
        public static void VariableSetString([MarshalAs(UnmanagedType.LPStr)] string var, [MarshalAs(UnmanagedType.LPStr)] string value)
        {
            if (story != null)
            {
                story.variablesState[var] = value;
            }
        }

        [DllExport("VisitCountAtPathString", CallingConvention = CallingConvention.Cdecl)]
        public static double VisitCountAtPathString([MarshalAs(UnmanagedType.LPStr)] string path)
        {
            return story?.state?.VisitCountAtPathString(path) ?? 0;
        }

        [DllExport("ObserveVariable", CallingConvention = CallingConvention.Cdecl)]
        public static void ObserveVariable([MarshalAs(UnmanagedType.LPStr)] string name, double script)
        {
            story?.ObserveVariable(name, (string varName, object value) =>
            {
                GML.CallScript(script, varName, value);
            });
        }

        [DllExport("BindExternal", CallingConvention = CallingConvention.Cdecl)]
        public static void BindExternal([MarshalAs(UnmanagedType.LPStr)] string name, double script)
        {
            story?.BindExternalFunctionGeneral(name, (object[] args) =>
            {
                GML.CallScript(script, args);
                return null; // Changed from 0 to null for proper void return
            });
        }
    }
}