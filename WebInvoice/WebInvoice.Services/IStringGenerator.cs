namespace WebInvoice.Services
{
    public interface IStringGenerator
    {
        string GenerateSlug(string str);
        string GetConnectionString(string dbName, string GUID);
    }
}