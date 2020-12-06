using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInvoice.Data.Repository.Models;

namespace WebInvoice.Data.CompanyData.Models
{
    public class Company : BaseDeletableModel<int>
    {
        public Company()
        {
            this.Partners = new HashSet<Partner>();
            this.Products = new HashSet<Product>();
            this.Employees = new HashSet<Employee>();
            this.BankAccounts = new HashSet<BankAccount>();
            this.NonVatDocuments = new HashSet<NonVatDocument>();
            this.VatDocuments = new HashSet<VatDocument>();
        }
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string Address { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        [MaxLength(50)]
        public string Country { get; set; }

        [MaxLength(9)]
        public string EIK { get; set; }

        [MaxLength(11)]
        public string VatId { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(50)]
        public string MOL { get; set; }

        public bool IsVatRegistered { get; set; }

        public string LogoPath { get; set; }

        public bool IsActive { get; set; }

        public string GUID { get; set; }

        public virtual ICollection<Partner> Partners { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }

        public virtual ICollection<BankAccount> BankAccounts { get; set; }

        public virtual ICollection<NonVatDocument> NonVatDocuments { get; set; }

        public virtual ICollection<VatDocument> VatDocuments { get; set; }

        public virtual ICollection<CompanyObject> CompanyObjects { get; set; }

    }
}
