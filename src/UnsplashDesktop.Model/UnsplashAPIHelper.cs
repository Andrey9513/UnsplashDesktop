using Flurl;
using RestSharp;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;
using System;

namespace UnsplashDesktopBusinessLogic
{
    public static class UnslashAPIHelper
    {

        private const string UpsplashUri = @"https://source.unsplash.com";

        static UnslashAPIHelper()
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.Console(new CompactJsonFormatter())
            .CreateLogger();
        }

        public static bool TryGetUnslashPhoto(RequestModel model, out byte[] image)
        {
            try
            {
                string fullUri = String.Empty;

                fullUri = model.Mode switch
                {
                    Modes.user => model.Orientation is Orientations.Any ? Url.Combine(UpsplashUri, model.Mode.ToString(), model.User, model.Size) 
                                                                        : Url.Combine(UpsplashUri, model.Mode.ToString(), model.User, model.Size, model.Orientation.ToString()),
                    Modes.collection => model.Orientation is Orientations.Any ? Url.Combine(UpsplashUri, model.Mode.ToString(), model.Collections, model.Size) 
                                                                              : Url.Combine(UpsplashUri, model.Mode.ToString(), model.Collections, model.Size, model.Orientation.ToString()),
                    Modes.featured => model.Orientation is Orientations.Any ? (String.IsNullOrEmpty(model.Size) ? Url.Combine(UpsplashUri, model.Mode.ToString(), model.Features)
                                                                                                                : Url.Combine(UpsplashUri, model.Size, model.Features)) 
                                                                            : (String.IsNullOrEmpty(model.Size) ? Url.Combine(UpsplashUri, model.Mode.ToString(), model.Features, model.Orientation.ToString())
                                                                                                                : Url.Combine(UpsplashUri, model.Size, model.Features, model.Orientation.ToString()))
                                                        ,
                    _ => throw new ArgumentException(message: "invalid enum value", paramName: nameof(model.Mode))
                };

                Log.Information($"Uri: {fullUri}");

                var client = new RestClient(fullUri);
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    image = response.RawBytes;
                    return true;
                }
                else
                {
                    Log.Warning("Response status code {StatusCode}", response.StatusCode);
                    image = null;
                    return false;
                }

            }
            catch (Exception exc)
            {
                Log.Error(exc, "No request was sent");
                image = null;
                return false;
            }

        }
    }
}
