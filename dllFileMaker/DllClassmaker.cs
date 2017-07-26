using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Collections;

namespace dllFileMaker
{
    class DllClassmaker
    {
        public class VariableItem{

            public string name;
            public string type;

            public bool HasDefault;
            public string defaultValue;

            public VariableItem(string name, string type, string defaultValue, bool HasDefault)
            {
                this.name = name;
                this.type = type;
                this.HasDefault = HasDefault;
                this.defaultValue = defaultValue;
                
            }
        }

        public static void CreateDllFile(string ClassName, ArrayList list)
        {
            Debug.WriteLine("Entering Creating Dll File");

            string typeToString = "";
            string variableConstructor = "";
            //Generating types
            foreach(VariableItem x in list)
            {
                typeToString += "   public " + x.type + " " + x.name +
                    " { get; set; }\n";


                if (x.HasDefault)
                {
                    string value = x.defaultValue;
                    if (x.type.Equals("string"))
                    {
                        value = "\"" + value + "\"";
                    }
                    variableConstructor += "      " + x.name + " = " + value +
                        ";\n";
                }
            }

            string Constructor = "   public " + ClassName + " (){\n" +
                variableConstructor +
                "}";

            //Generating Dll
            var provider = new CSharpCodeProvider();
            var options = new CompilerParameters
            {
                OutputAssembly = ClassName + "SOP.dll"
            };
            string source =
            "using System;\n" +           
            "public class " + ClassName + " {\n" +
                   typeToString +   
                   Constructor +          
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
