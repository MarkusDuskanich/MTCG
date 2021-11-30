using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Http.Status {

    public enum HttpStatus {
        [Description("200 OK")]
        OK,
        [Description("400 Bad Request")]
        BadRequest, 
        [Description("401 Unauthorized")]
        Unauthorized,
        [Description("403 Forbidden")]
        Forbidden,
        [Description("404 Not Found")]
        NotFound,
        [Description("500 Internal Server Error")]
        InternalServerError
    }

    public class HttpStatusConverter {
        public static string From(HttpStatus status) {
            var memInfo = typeof(HttpStatus).GetMember(status.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? (attributes[0] as DescriptionAttribute).Description : null;
        }  

        public static HttpStatus From(string description) {
            foreach(var status in Enum.GetValues(typeof(HttpStatus))){
                if (From((HttpStatus)status) == description)
                    return (HttpStatus)status;
            }
            throw new ArgumentException($"HTTP status code for <{description}> not found");
        }
    }
}
