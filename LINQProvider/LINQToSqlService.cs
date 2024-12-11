using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider
{
    public class LINQToSqlService
    {
        public Expression Translate(string query)
        { 
            ExpressionType.GreaterThan = 
        
        }

        
        public string GetSqlOperator(ExpressionType operationSighn)
        {
            string sqlOperator;
            switch (operationSighn) {
                case ExpressionType.GreaterThan:
                    sqlOperator = ">";
                    break;

                case ExpressionType.Equal:
                    sqlOperator = "=";
                    break;

                case ExpressionType.LessThan:
                    sqlOperator = "<";
                    break;
                default:
                    sqlOperator = String.Empty;
                    break;
                }
            return sqlOperator;
        }

        public string 
    }
}
