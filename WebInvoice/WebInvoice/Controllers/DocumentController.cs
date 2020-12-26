using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Data;
using WebInvoice.Data.CompanyData.Models;
using WebInvoice.Data.Repository.Repositories;

namespace WebInvoice.Controllers
{
    [Authorize]
    public class DocumentController : Controller
    {
        private readonly ICompanyDeletableEntityRepository<Company> companyRepository;

        public DocumentController(ICompanyDeletableEntityRepository<Company> companyRepository)
        {
            this.companyRepository = companyRepository;
        }
        public IActionResult Index()
        {
            var name = companyRepository.All().FirstOrDefault();
            return Json(name);
        }
    }
}
