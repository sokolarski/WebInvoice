namespace WebInvoice.Services
{
    public interface IConnectionStringGenerator
    {
        string GenerateSlug(string str);
        string GetConnectionString(string dbName, string GUID);
    }
}