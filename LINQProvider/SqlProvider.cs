using LINQProvider.Models;
using System.Collections;
using System.Data.SqlClient;
using System.Linq.Expressions;

public class SqlProvider : IQueryProvider
{
    string _connectionString;
    public SqlProvider(string connectionString)
    {
        _connectionString = connectionString;
    }
    public IQueryable CreateQuery(Expression expression)
    {
        Type elementType = expression.Type.GetGenericArguments().First();
        return (IQueryable)Activator.CreateInstance(typeof(SqlQueryable<>).MakeGenericType(elementType), this, expression);
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        return new SqlQueryable<TElement>(this, expression);
    }

    public object Execute(Expression expression)
    {
        return Execute<IEnumerable<object>>(expression);
    }

    public TResult Execute<TResult>(Expression expression)
    {
        string sql = TranslateToSql<TResult>(expression);
        Console.WriteLine("Generated SQL: " + sql);

        var resultType = typeof(TResult).GetGenericArguments().First();
        var listType = typeof(List<>).MakeGenericType(resultType);
        var results = (IList)Activator.CreateInstance(listType);

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                  
                    var properties = resultType.GetProperties();
                    while (reader.Read())
                    {
                        var instance = Activator.CreateInstance(resultType);

                        foreach (var prop in properties)
                        {
                            var val = reader[prop.Name];
                            prop.SetValue(instance, val == DBNull.Value ? null : val);
                        }
                        results.Add(instance);
                        Console.WriteLine(instance.GetType());

                    }
                    Console.WriteLine(typeof(TResult));
                    Console.WriteLine(results.GetType());
                    return (TResult)(object)results;
                }
            }
        }
    }

    private string TranslateToSql<T>(Expression expression)
    {
        string ProcessExpression(Expression expr)
        {
            if (expr is BinaryExpression binaryExpr)
            {
                string left = ProcessExpression(binaryExpr.Left);
                string right = ProcessExpression(binaryExpr.Right);
                string @operator = GetSqlOperator(binaryExpr.NodeType);

                return $"({left} {@operator} {right})";
            }
            else if (expr is MemberExpression memberExpr)
            {
                return memberExpr.Member.Name;
            }
            else if (expr is ConstantExpression constantExpr)
            {
                return constantExpr.Value?.ToString() ?? "NULL";
            }
            else if (expr is UnaryExpression unaryExpr)
            {
                return ProcessExpression(unaryExpr.Operand);
            }

            throw new NotSupportedException($"Expression type {expr.GetType()} is not supported.");
        }

        if (expression is MethodCallExpression methodCall && methodCall.Method.Name == "Where")
        {
            var lambda = (LambdaExpression)((UnaryExpression)methodCall.Arguments[1]).Operand;
            var resultType = typeof(T).GetGenericArguments()[0];

            string whereClause = ProcessExpression(lambda.Body);

            return $"SELECT * FROM {resultType.Name}s WHERE {whereClause}";
        }

        throw new NotSupportedException("Only simple Where clauses are supported in this demo.");
    }

    private string GetSqlOperator(ExpressionType nodeType)
    {
        return nodeType switch
        {
            ExpressionType.Equal => "=",
            ExpressionType.GreaterThan => ">",
            ExpressionType.LessThan => "<",
            ExpressionType.And => "AND",
            ExpressionType.AndAlso => "AND",
            _ => throw new NotSupportedException($"Operator {nodeType} is not supported.")
        };
    }
}
