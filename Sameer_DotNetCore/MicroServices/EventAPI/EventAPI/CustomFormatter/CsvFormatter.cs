using EventAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAPI.CustomFormatter
{
    public class CsvFormatter : TextOutputFormatter
    {
        public CsvFormatter()
        {
            this.SupportedEncodings.Add(Encoding.UTF8);
            this.SupportedEncodings.Add(Encoding.Unicode);
            this.SupportedMediaTypes.Add("text/csv");
            this.SupportedMediaTypes.Add("application/csv");
        }

        protected override bool CanWriteType(Type type)
        {
            if(typeof(EventInfo).IsAssignableFrom(type) || typeof(IEnumerable<EventInfo>).IsAssignableFrom(type))
            {
                return true;
            }
            return false;
        }


        public async override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var buffer = new StringBuilder();
            var response = context.HttpContext.Response;

            if(context.Object is EventInfo)//if item is single eventinfo object
            {

                var ev = context.Object as EventInfo;
                buffer.AppendLine("Id,Title,Speaker,Organizer,StartDate,EndDate,Location,RegistrationUrl");
                buffer.AppendLine($"{ev.Id},{ev.Title},{ev.Speaker},{ev.Organizer},{ev.StartDate},{ev.EndDate},{ev.Location},{ev.RegistrationURL}");
            }
            else if(context.Object is IEnumerable<EventInfo>)//if array of eventinfo object
            {
                var events = context.Object as IEnumerable<EventInfo>;
                buffer.AppendLine("Id,Title,Speaker,Organizer,StartDate,EndDate,Location,RegistrationUrl");
                foreach (var ev in events)
                {
                    buffer.AppendLine($"{ev.Id},{ev.Title},{ev.Speaker},{ev.Organizer},{ev.StartDate},{ev.EndDate},{ev.Location},{ev.RegistrationURL}");
                }
            }
            await response.WriteAsync(buffer.ToString());
        }
    }
}
