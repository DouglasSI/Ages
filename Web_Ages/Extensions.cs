using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_Ages
{

    public class LocationData
    {
        public string ActionName { get; set; }

        public string ControllerName { get; set; }
    }

    public static class LocationHelper
    {
        public static LocationData GetLocationData<TModel>(this WebViewPage<TModel> page)
        {
            // TODO: validate page, ViewContext, RouteData, Values
            //      for:
            //          not null, contain values
            return new LocationData()
            {
                ActionName = (string)page.ViewContext.RouteData.Values["action"],
                ControllerName = (string)page.ViewContext.RouteData.Values["controller"]
                // TODO: get area name
            };
        }
    }
}