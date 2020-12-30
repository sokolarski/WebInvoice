using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.AppData.Models;

namespace WebInvoice.Services
{
    public class UserCompanyTemp : IUserCompanyTemp
    {
        public UserCompanyTemp()
        {
            
        }

        public string UserId { get; set; }

        public string CompanyName { get; set; }
        public bool IsSetCompany => !string.IsNullOrEmpty(this.CompanyName);
        public string CompanySlug { get; set; }

        public string CompanyGUID { get; set; }

        public string CompanyObjectName { get; set; }

        public bool IsSetObject => !string.IsNullOrEmpty(this.CompanyObjectName);

        public string CompanyObjectSlug { get; set; }

        public string CompanyObjectGUID { get; set; }

        public string ConnectionString { get; set; }

        public bool IsSetConnectionString => !string.IsNullOrEmpty(this.ConnectionString);

        public Dictionary<string, string> RouteData
        {
            get
            {
                var temp = new Dictionary<string, string>();
                temp.Add("company", this.CompanySlug);
                temp.Add("object", this.CompanyObjectSlug);
                return temp;
            }
            set { this.RouteData = value; }
        }

        public ICollection<CompanyAppObject> CurrentCompanyAppObjects { get; set; }
        public ICollection<CompanyApp> CompanyApps { get; set; }


    }
}
