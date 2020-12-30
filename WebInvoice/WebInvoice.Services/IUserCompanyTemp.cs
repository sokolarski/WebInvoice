using System.Collections.Generic;
using WebInvoice.Data.AppData.Models;

namespace WebInvoice.Services
{
    public interface IUserCompanyTemp
    {
        
        string CompanyName { get; set; }
        string CompanyObjectName { get; set; }
        bool IsSetCompany { get; }
        string CompanyObjectSlug { get; set; }
        string CompanyGUID { get; set; }
        bool IsSetObject { get; }
        string CompanySlug { get; set; }
        string CompanyObjectGUID { get; set; }
        string ConnectionString { get; set; }
        bool IsSetConnectionString { get; }
        Dictionary<string, string> RouteData { get; set; }
        string UserId { get; set; }
        public ICollection<CompanyAppObject> CurrentCompanyAppObjects { get; set; }
        ICollection<CompanyApp> CompanyApps { get; set; }
    }
}