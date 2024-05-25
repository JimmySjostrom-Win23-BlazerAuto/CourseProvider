using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;

namespace CourseProvider.Functions;

public class Playground(ILogger<Playground> logger)
{
    private readonly ILogger<Playground> _logger = logger;

	[Function("Playground")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "graphql")] HttpRequestData req)
    {
        var response = req.CreateResponse();
        response.Headers.Add("Content-Type", "text/html; charset=utf-8");
        await response.WriteStringAsync(PlaygroundPage());
        return response;
    }

    private string PlaygroundPage()
    {
        return @"
            <!DOCTYOE html>
            <html>
            <head>
                <title>Hotchocolate GraphQL Playground</title>
                <link rel=""stylesheet"" href=""https://cdn.jsdelivr.net/npm/graphql-playground-react/build/static/css/index.css"" />
                <link rel=""shortcut icon"" href=""https://cdn.jsdelivr.net/npm/graphql-playground-react/build/favicon.png"" />
                <script src=""https://cdn.jsdelivr.net/npm/graphql-playground-react/build/static/js/middleware.js""></script>
            </head>
            <body>
                <div id=""root""></div>
                <script>
                    window.addEventListener('load', function (event) {
                        GraphQLPlayground.init(document.getElementById('root'), 7
                            endpoint: '/api/graphql'
                        })
                    })
                </script>
            </body>
            </html>";
    }
}