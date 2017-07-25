using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace dllFileMaker
{
    class DllClassmaker
    {
        public static void CreateDllFile(string ClassName, Dictionary<string,string> typeNames)
        {
            Debug.WriteLine("Entering Creating Dll File");

            string typeToString = "";
            //Generating types
            foreach(KeyValuePair<string,string> x in typeNames)
            {
                typeToString += "   public " + x.Value + " " + x.Key +
                    " { get; set; }\n";
            }


            //Generating Dll
            var provider = new CSharpCodeProvider();
            var options = new CompilerParameters
            {
                OutputAssembly = ClassName + "SOP.dll"
            };
            string source =           
            "public class " + ClassName + " {\n" +
                   typeToString +             
            "}";
            Debug.WriteLine(source);

            CompilerResults result =  provider.CompileAssemblyFromSource(options, new[] { source });
            if(result.Errors.Count > 0)
            {
                for(int i = 0; i < result.Errors.Count; i++)
                {
                    Debug.WriteLine(result.Errors[i]);
                }
            }
        }

    }
}
