using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeShare.Models.Web
{
    public class AppSettingsDto
    {
        public ConnectionStringsDto ConnectionStrings { get; set; } = new();
        public ApiSettingsDto Api { get; set; } = new();
        public LoggingSettingsDto Logging { get; set; } = new();
        public string AllowedHosts { get; set; } = "*";
    }

    public class ConnectionStringsDto
    {
        public string DefaultConnection { get; set; } = string.Empty;
    }

    public class ApiSettingsDto
    {
        public string BaseUrl { get; set; } = string.Empty;
    }

    public class LoggingSettingsDto
    {
        public LogLevelDto LogLevel { get; set; } = new();
    }

    public class LogLevelDto
    {
        public string Default { get; set; } = "Information";
        public string MicrosoftAspNetCore { get; set; } = "Warning";
    }
}
