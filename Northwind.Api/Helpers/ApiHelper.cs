using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.Routing;
using Northwind.Api.Controllers;
using Northwind.Api.Models;
using Northwind.Data;
using Northwind.Model;

namespace Northwind.Api.Helpers
{
    public static class ApiHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="context"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static ICollection<TEntity> AddPaginationToHeader<TEntity>(this IOrderedQueryable<TEntity> collection,
            ApiController controller, int page, int pageSize, string resource = null) where TEntity : class 
        {
            var resourceName = resource ??
                               controller.ActionContext.ControllerContext.ControllerDescriptor.ControllerName;
            var totalCount = collection.Count();
            var totalPages = (int) Math.Ceiling((double) totalCount/pageSize);
            var urlHelper = new UrlHelper(controller.Request);
            var prevLink = page > 0 ? urlHelper.Link(resourceName, new {page = page - 1, size = pageSize}) : "";
            var nextLink = page < totalPages - 1
                ? urlHelper.Link(resourceName, new {page = page + 1, pageSize = pageSize})
                : "";

            var pagingHeader = new
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                PrevPageLink = prevLink,
                NextPageLink = nextLink
            };

            var response = HttpContext.Current.Response;

            response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(pagingHeader));

            return collection.Skip(pageSize*page).Take(pageSize).ToList();
        }

        public static ICollection<TEntity> AddPaginationToHeader<TEntity>(this IEnumerable<TEntity> collection,
            ApiController controller, int page, int pageSize, string resource = null) where TEntity : class
        {
            return collection.Skip(pageSize * page).Take(pageSize).ToList();
        }
    }
}