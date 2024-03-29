﻿using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Acme.Energy.Backend.Controllers
{
    [ApiController]
    [Route("/spa-config")]
    public class SpaConfigController : ControllerBase
    {
        private readonly IConfiguration _config;
        public SpaConfigController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public ActionResult<SpaConfigResponse> SpaConfiguration()
        {
            var host = Request.Host.ToString();
            var origin = Request.Headers.Origin.FirstOrDefault() ?? "";
            var userAgent = Request.Headers.UserAgent.FirstOrDefault();
            var acceptedLangs = Request.Headers.AcceptLanguage.FirstOrDefault() ?? "";

            var favoriteLang = GetPreferredLang(acceptedLangs);
            var langResources = $"/i18n/{favoriteLang}.json";

            var ip = Request.HttpContext.Connection.RemoteIpAddress?.ToString();

            var flavour = GetBrand(origin);
            Console.WriteLine(origin + " resolved to flavour: " + flavour);

            return new SpaConfigResponse
            {
                Version = GetVersion(),
                Environment = _config.GetValue<string>("Env"),
                Audience = _config.GetValue<string>($"Spa:{flavour}:Audience"),
                ClientId = _config.GetValue<string>($"Spa:{flavour}:ClientId"),
                Brand = _config.GetValue<string>($"Spa:{flavour}:Brand"),

                Host = host,
                Origin = origin,
                UserAgent = userAgent,
                AcceptedLang = acceptedLangs,
                LangResources = langResources,
                Lang = favoriteLang,
                Ip = ip,
            };
        }
        private static string GetBrand(string host)
        {
            if (host.Contains("electron.")) 
            {
                return "electron";
            }
            return "photon";
        }
        private static string GetVersion()
        {
            var version = Assembly.GetExecutingAssembly()?.GetName().Version ?? new Version();
            return $"{version.Major}.{version.Minor}.{version.Build}";
        }
        private static string GetPreferredLang(string langs)
        {
            var f1 = langs.Split(';').FirstOrDefault() ?? "";
            var f2 = f1.Split(',').FirstOrDefault() ?? "";
            return f2 ?? "en";
        }
    }

    public class SpaConfigResponse
    {
        public string Version { get; set; } = "";
        public string Environment { get; set; } = "";
        public string Audience { get; set; } = "";
        public string ClientId { get; set; } = "";
        public string Brand { get; set; } = "";
        public string? Host { get; set; }
        public string? Origin { get; set; }
        public string? UserAgent { get; set; }
        public string? AcceptedLang { get; set; }
        public string? Lang { get; set; }
        public string? LangResources { get; set; }
        public string? Ip { get; set; }
    }
}
