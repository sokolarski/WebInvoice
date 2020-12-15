namespace WebInvoice.Services
{
    public interface ICompanyDbContextConnStrProvider
    {
        string GetConnectionString();
    }
}