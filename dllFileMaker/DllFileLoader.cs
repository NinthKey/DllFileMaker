﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dllFileMaker
{
    class DllFileLoader
    {
        static Assembly Assemblies;
        public ArrayList[] SOPClasses;

        public static dynamic SOP;
        public static string SOPTargetClassName;

        public static dynamic SOPRecord;
        public static string RecordTargetClassName;

        public DllFileLoader()
        {
            Debug.WriteLine("DllFileLoad Initialize");
            // Print all dll class and method
            
        }

        public static ArrayList loadDLLFile(string path)
        {

            Assemblies = Assembly.LoadFrom(path);

            ArrayList List = new ArrayList();

            Type[] AllTypes = Assemblies.GetTypes();

            foreach (Type t in AllTypes)
            {
                List.Add(t.ToString());
                Debug.WriteLine(t.ToString());

                PropertyInfo[] properties = t.GetProperties();
                
                foreach(PropertyInfo x in properties)
                {
                    List.Add(x.Name + " " + x.PropertyType);
                    Debug.WriteLine(x.Name + " " + x.PropertyType );
                }
            }


            return List;

        }

        

        /// <summary>
        /// This function loads all the dll files in DEBUG folder and print 
        /// the class and type name to the Console.
        /// </summary>
        /// <returns></returns>
        public ArrayList[] LoadLibrary()
        {
            string filepath = Environment.CurrentDirectory;
            Console.WriteLine(filepath);
            DirectoryInfo d = new DirectoryInfo(filepath);
            int dllCount = d.GetFiles("*.dll").Count();

            ArrayList[] Classes = new ArrayList[dllCount];
            ArrayList[] Methods = new ArrayList[dllCount];

            int i = 0;
            foreach (var file in d.GetFiles("*.dll"))
            {
                string dllName = file.ToString().Split('.').First();

                // Writing all the Class + Method name
                Console.WriteLine(file.ToString());
                if (dllName.Contains("SOP"))
                {
                    Classes[i] = GetAllTypesFromDLLstring(dllName);

                    foreach (var ii in Classes[i])
                    {
                        Methods[i] = GetAllTypesFromClass(dllName,
                                                          ii.ToString());
                        // PRINT--------------------------------------
                        Console.WriteLine("----Classes: " + ii.ToString());
                        foreach (var jj in Methods[i])
                            Console.WriteLine("----Methods: " + jj.ToString());
                        // -------------------------------------------
                    }
                }
                i++;
            }
            return Classes;
        }
        public ArrayList GetAllTypesFromDLLstring(string dllName)
        {
            try
            {
                Assemblies = Assembly.Load(dllName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "\n\nError - couldn't obtain assemblies from " +
                    dllName);
                Console.WriteLine("EXCEPTION OUTPUT\n" +
                    ex.Message + "\n" +
                    ex.InnerException);
                ArrayList _Quit = new ArrayList(1);
                _Quit.Add("QUIT");
                return _Quit;
            }

            Type[] AllTypes = Assemblies.GetTypes();

            ArrayList _Temp = new ArrayList();

            foreach (Type t in AllTypes)
            {
                _Temp.Add(t.ToString());
            }

            return _Temp;
        }
        public ArrayList GetAllTypesFromClass(string dllName, string className)
        {
            //Assembly _Assemblies = Assembly.Load(dllName);

            Type _Type;

            ArrayList _Temp = new ArrayList();
            try
            {
                _Type = Assemblies.GetType(className);
                MethodInfo[] _Methods = _Type.GetMethods();

                foreach (MethodInfo meth in _Methods)
                {
                    _Temp.Add(meth.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\nError - couldn't obtain methods from " +
                                  dllName);
                Console.WriteLine("EXCEPTION:\n" + ex.Message + "\n" +
                                  ex.InnerException);
                _Temp.Clear();
                _Temp.Capacity = 1;
                _Temp.Add("QUIT");
            }
            return _Temp;
        }

        public static object FindClass(string targetClassName)
        {
            try
            {
                Assemblies = Assembly.LoadFrom(targetClassName + ".dll");
                return Assemblies.CreateInstance(targetClassName +
                                                "." + targetClassName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }

        }
    }
}
