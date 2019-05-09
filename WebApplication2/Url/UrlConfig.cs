using System.Collections.Generic;

namespace WebApplication2.Url
{
    public class UrlConfig
    {
        public int MaxUrlChunkSize { get; } = 260;
        public IEnumerable<string> ValidIssuers { get; } = new[] {"http://localhost:51501"};
        public IEnumerable<string> ValidAudiences { get; } = new[] {"http://localhost:51501"};
    }
}