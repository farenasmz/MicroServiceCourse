using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace TiendaServicios.Apil.Libro.Tests
{
    public class AsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider Inner;
        public AsyncQueryProvider(IQueryProvider inner)
        {
            Inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new AsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new AsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return Inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return Inner.Execute<TResult>(expression);
        }

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
        {
            Type resultadoTipo = typeof(TResult).GetGenericArguments()[0];
            object ejecucionResultado = typeof(IQueryProvider).GetMethod(
                name: nameof(IQueryProvider.Execute),
                genericParameterCount: 1,
                types: new[] { typeof(Expression) }
                ).MakeGenericMethod(resultadoTipo).Invoke(this, new[] { expression });

            return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))?.MakeGenericMethod(resultadoTipo).Invoke(null, new[] { ejecucionResultado });
        }
    }
}
