
namespace Microsoft.OpenApi
{
    internal class Models
    {
        internal class OpenApiInfo : OpenApi.OpenApiInfo
        {
            public string Title { get; set; }
            public string Version { get; set; }
        }
    }
}