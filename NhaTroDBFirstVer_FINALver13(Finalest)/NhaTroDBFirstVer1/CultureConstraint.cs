using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web;

namespace NhaTroDBFirstVer1
{
    public class CultureConstraint : IRouteConstraint
    {
        private string[] _values;
        public CultureConstraint(params string[] values)
        {
            this._values = values;
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName,
                            RouteValueDictionary values, RouteDirection routeDirection)
        {
            // lấy giá trị parameterName từ route
            string value = values[parameterName].ToString();
            // kiểm tra ràng buộc
            return _values.Contains(value);

        }

    }
}



