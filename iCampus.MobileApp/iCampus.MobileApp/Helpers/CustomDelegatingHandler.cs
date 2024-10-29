using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.Forms;

namespace iCampus.MobileApp.Helpers;

 public class CustomDelegatingHandler : DelegatingHandler
    {
        public CustomDelegatingHandler():base(new HttpClientHandler())
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            string requestContentBase64String = string.Empty;
            
            ApiConfigurationView apiConfig = await ApiHelper.GetObject<ApiConfigurationView>(TextResource.SISApiConfigurationUrl);

            string requestUri = System.Web.HttpUtility.UrlEncode(request.RequestUri.AbsoluteUri.ToLower());

            string requestHttpMethod = request.Method.Method;

            //Calculate UNIX time
            DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = DateTime.UtcNow - epochStart;
            string requestTimeStamp = Convert.ToUInt64(timeSpan.TotalSeconds).ToString();

            //create random nonce for each request
            string nonce = Guid.NewGuid().ToString("N");

            //Checking if the request contains body, usually will be null with HTTP GET and DELETE
            if (request.Content != null)
            {
                byte[] content = await request.Content.ReadAsByteArrayAsync();
                MD5 md5 = MD5.Create();
                byte[] requestContentHash = md5.ComputeHash(content);
                requestContentBase64String = Convert.ToBase64String(requestContentHash);
            }

            //Creating the raw signature string
            string signatureRawData = requestContentBase64String;
            var secretKeyByteArray = Encoding.ASCII.GetBytes(apiConfig.SecretKey);

            byte[] signature = Encoding.UTF8.GetBytes(signatureRawData);

            using (HMACSHA256 hmac = new HMACSHA256(secretKeyByteArray))
            {
                byte[] signatureBytes = hmac.ComputeHash(signature);
                string requestSignatureBase64String = Convert.ToBase64String(signatureBytes);
                request.Headers.Authorization = new AuthenticationHeaderValue("amx", string.Format("{0}:{1}:{2}:{3}:{4}", apiConfig.PublicKey, requestSignatureBase64String, nonce, requestTimeStamp, AppSettings.Current.UserId));
            }

            response = await base.SendAsync(request, cancellationToken);

            return response;
        }
    }