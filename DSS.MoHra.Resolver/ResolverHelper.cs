using System;

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
            string currentStepExpression = expression;

            do
            {
                currentStepExpression = expression;

                expression = expression.Replace("!0", "1");
                expression = expression.Replace("!1", "0");

                expression = expression.Replace("(0)", "0");
                expression = expression.Replace("(1)", "1");

                expression = expression.Replace("0*0", "0");
                expression = expression.Replace("0*1", "0");
                expression = expression.Replace("1*0", "0");
                expression = expression.Replace("1*1", "1");

                expression = expression.Replace("0+0", "0");
                expression = expression.Replace("0+1", "1");
                expression = expression.Replace("1+0", "1");
                expression = expression.Replace("1+1", "1");
            }
            while (currentStepExpression != expression);

            if (expression == "0")
                return false;
            if (expression == "1")
                return true;

            throw new ArgumentException("expression");
        }
    }
}
