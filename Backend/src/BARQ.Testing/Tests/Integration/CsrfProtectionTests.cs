using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using BARQ.Testing.Framework;

namespace BARQ.Testing.Tests.Integration
{
    public class CsrfProtectionTests : ApiTestFramework
    {
        [Fact(Skip = "CSRF is not enforced in Testing environment; enable when running against non-Testing env")]
        public async Task UnsafePost_WithoutCsrfHeader_Returns403_WhenCookieAuthEnabled()
        {
            var client = CreateClient();

            var loginPayload = new
            {
                Email = "test@acme.com",
                Password = "Password123!"
            };

            var content = new StringContent(JsonSerializer.Serialize(loginPayload), Encoding.UTF8, "application/json");
            var loginResp = await client.PostAsync("/api/auth/login", content);
            Assert.True(loginResp.StatusCode == HttpStatusCode.OK, "Login should succeed to obtain auth cookie");

            var reqBody = new { name = "csrf-probe" };
            var req = new HttpRequestMessage(HttpMethod.Post, "/api/projects");
            req.Content = new StringContent(JsonSerializer.Serialize(reqBody), Encoding.UTF8, "application/json");

            var resp = await client.SendAsync(req);
            Assert.Equal(HttpStatusCode.Forbidden, resp.StatusCode);
        }
    }
}
