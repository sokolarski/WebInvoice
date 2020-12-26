namespace WebInvoice.Data.Repository
{
    using System;
    using System.Threading.Tasks;

    public interface ICompanyDbQueryRunner : IDisposable
    {
        Task RunQueryAsync(string query, params object[] parameters);
    }
}
