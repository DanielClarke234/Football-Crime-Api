using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Football_Crime_Api.Models.Paging
{
    public static class PagingExtension
    { 
        //Method for filtering return models for use on a table style page
        public static List<T> Limit<T>(this List<T> objects, PagingModel model)
        {
            return objects.AsQueryable().Skip((model.PageNum - 1) * model.PerPage).Take(model.PerPage).ToList();
        }
    }
    public class PagingModel
    {
        [FromQuery(Name = "pagenum")]
        public int PageNum { get; set; } = 1;
        [FromQuery(Name = "perpage")]
        public int PerPage { get; set; } = 9999999;
    }
}
