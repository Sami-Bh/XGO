using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGOMobile.Services.Models
{
    internal class LoggingHandler : DelegatingHandler
    {
        public LoggingHandler() : base()
        {
        }

        public LoggingHandler(HttpMessageHandler innerHandler) : base(innerHandler)
        {
        }
    }
}
