using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP03_WebApp
{
    public class ViewLayoutAttribute : ResultFilterAttribute
    {
        private string layout;

        public ViewLayoutAttribute(string layout)
        {
            Layout = layout;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var viewResult = context.Result as ViewResult;

            if (viewResult != null)
            {
                viewResult.ViewData["Layout"] = Layout;
            }
        }

        public string Layout { get => layout; set => layout = value; }
    }
}
