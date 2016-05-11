using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;

namespace DSS.MoHra.Resolver
{
    public static class ResolverHelper
    {
        public static bool Evaluate(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                throw new ArgumentNullException("expression");

            expression = expression.ToLower();
            expression = expression.Trim();
            expression = expression.Replace(" ", "");

            var method = @"
                using System;
            
                namespace DSS.MoHra.Resolver
                {                
                    public class BooleanFunction
                    {                
                        public static bool Evaluate()
                        {
                            return {expression};
                        }
                    }
                }
            ";
            method = method.Replace("{expression}", expression);
            
            var provider = new CSharpCodeProvider();
            var results = provider.CompileAssemblyFromSource(new CompilerParameters(), method);
            if (results.Errors != null && results.Errors.Count > 0)
            {
                var exceptionString = string.Empty;
                foreach (CompilerError error in results.Errors)
                    exceptionString += (string.IsNullOrEmpty(exceptionString) ? string.Empty : Environment.NewLine) + error.ErrorText;
                throw new ArgumentException(exceptionString);
            }
            var booleanFunction = results.CompiledAssembly.GetType("DSS.MoHra.Resolver.BooleanFunction");
            var compiledMethod = booleanFunction.GetMethod("Evaluate");
            return (bool)compiledMethod.Invoke(null, null);
        }
    }
}
