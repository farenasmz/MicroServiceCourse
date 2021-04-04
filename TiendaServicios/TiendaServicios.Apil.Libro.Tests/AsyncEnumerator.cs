using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TiendaServicios.Apil.Libro.Tests
{
    public class AsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private IEnumerator<T> Enumerator { get; set; }
        public T Current => Enumerator.Current;

        public AsyncEnumerator(IEnumerator<T> enumerator) => this.Enumerator = enumerator ?? throw new ArgumentNullException();

        public async ValueTask DisposeAsync()
        {
            await Task.CompletedTask;
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            return await Task.FromResult(Enumerator.MoveNext());
        }
    }
}
