using System;
using System.Linq;
using System.Linq.Expressions;

public class SqlQueryable<T> : IQueryable<T>
{
    private readonly SqlProvider _provider;
    private readonly Expression _expression;

    public SqlQueryable(SqlProvider provider, Expression expression = null)
    {
        _provider = provider;
        _expression = expression ?? Expression.Constant(this);
    }

    public Type ElementType => typeof(T);
    public Expression Expression => _expression;
    public IQueryProvider Provider => _provider;

    public IEnumerator<T> GetEnumerator() 
    {
        var result = _provider.Execute<IEnumerable<T>>(_expression);
            return result.GetEnumerator();
    } 
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}
