using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInvoice.Data.AppData.Models;
using WebInvoice.Data.Repository.Repositories;
using WebInvoice.Services;

namespace WebInvoice.Middleware
{
    public class GetCompanyTempDataMiddleware
    {
        private readonly RequestDelegate next;

        public GetCompanyTempDataMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task InvokeAsync(HttpContext context,
                                        UserManager<ApplicationUser> userManager,
                                        IAppDeletableEntityRepository<CompanyApp> companyAppRepository,
                                        IUserCompanyTemp userCompanyTemp)
        {

            if (context.User.Identity.IsAuthenticated)
            {
                var routeValues = context.Request.RouteValues;
                var userId = userManager.GetUserId(context.User);
                var companies = await companyAppRepository.AllAsNoTracking()
                    .Include(c => c.CompanyAppObjects)
                    .Where(u => u.ApplicationUserId == userId).ToListAsync();
                if (companies.Count > 0)
                {
                    userCompanyTemp.UserId = userId;
                    userCompanyTemp.CompanyApps = companies;

                    if (routeValues.Keys.Contains("company"))
                    {
                        var currentCompanySlug = routeValues["company"].ToString();
                        var currentCompany = companies.Where(c => c.CompanySlug == currentCompanySlug).FirstOrDefault();
                        
                        if (currentCompany != null)
                        {
                            userCompanyTemp.CompanyName = currentCompany.CompanyName;
                            userCompanyTemp.CompanySlug = currentCompany.CompanySlug;
                            userCompanyTemp.ConnectionString = currentCompany.ConnStr;
                            userCompanyTemp.CompanyGUID = currentCompany.GUID;
                            userCompanyTemp.IsVatRegistered = currentCompany.IsVatRegistered;

                            if (routeValues.Keys.Contains("companyObject"))
                            {
                                var currentObjectSlug = routeValues["companyObject"].ToString();
                                var objects = currentCompany.CompanyAppObjects;

                                if (objects != null)
                                {
                                    var currentObject = objects.Where(o => o.ObjectSlug == currentObjectSlug).FirstOrDefault();

                                    if (currentObject is null)
                                    {
                                        currentObject = objects.FirstOrDefault();
                                        if (currentObject != null)
                                        {
                                            context.Request.RouteValues["companyObject"] = currentObject.ObjectSlug;
                                        }
                                    }

                                    if (currentObject != null)
                                    { 
                                        
                                        userCompanyTemp.CompanyObjectName = currentObject.ObjectName;
                                        userCompanyTemp.CompanyObjectSlug = currentObject.ObjectSlug;
                                        userCompanyTemp.CompanyObjectGUID = currentObject.GUID;
                                        userCompanyTemp.CurrentCompanyAppObjects = currentCompany.CompanyAppObjects; 
                                    }
                                }
                            }
                        }
                        else
                        {
                            context.Response.Redirect("/home/index?errorUrl");
                            return;
                        }
                       
                    }
                    else
                    {
                        if (!context.Request.Query.ContainsKey("errorUrl"))
                        {
                            var currentCompany = companies.Where(c => c.IsActive == true).FirstOrDefault();

                            if (currentCompany != null)
                            {
                                userCompanyTemp.CompanyName = currentCompany.CompanyName;
                                userCompanyTemp.CompanySlug = currentCompany.CompanySlug;
                                userCompanyTemp.ConnectionString = currentCompany.ConnStr;
                                userCompanyTemp.CompanyGUID = currentCompany.GUID;
                                userCompanyTemp.IsVatRegistered = currentCompany.IsVatRegistered;

                                context.Request.RouteValues.TryAdd("company", currentCompany.CompanySlug);

                                var objects = currentCompany.CompanyAppObjects;
                                if (objects != null)
                                {
                                    var currentObject = objects.Where(o => o.IsActive == true).FirstOrDefault();

                                    if (currentObject is null)
                                    {
                                        currentObject = objects.FirstOrDefault();
                                    }

                                    if (currentObject != null)
                                    {

                                        userCompanyTemp.CompanyObjectName = currentObject.ObjectName;
                                        userCompanyTemp.CompanyObjectSlug = currentObject.ObjectSlug;
                                        userCompanyTemp.CompanyObjectGUID = currentObject.GUID;
                                        userCompanyTemp.CurrentCompanyAppObjects = currentCompany.CompanyAppObjects;
                                        context.Request.RouteValues.TryAdd("companyObject", currentObject.ObjectSlug);
                                    }
                                }
                            }
                        }
                        
                    }
                }
            }

            await next.Invoke(context);

        }

    }
}
