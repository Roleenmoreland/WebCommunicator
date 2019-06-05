using System.Collections.Generic;
using System.Threading.Tasks;
using WebCommunicator.Interface;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace WebCommunicator.Classes
{
    public class WebCommunicator : IWebCommunicator
    {
        private readonly string jsonMediaType = "application/json";
        private readonly HttpClient _httpClient;

        public WebCommunicator()
        {
            _httpClient = new HttpClient();
        }

        public async Task<TResponse> SendGetRequest<TResponse>(string urlPath, IDictionary<string, string> headers = null, IDictionary<string, string> authenticationHeader = null)
        {
            try
            {
                //Creating a Http request
                using(var request = new HttpRequestMessage(HttpMethod.Get, urlPath))
                {
                    //Set the  Authentication Header
                    if (authenticationHeader != null)
                    {
                        foreach (var header in authenticationHeader)
                        {
                            var authentication = new AuthenticationHeaderValue(header.Key, header.Value);
                            request.Headers.Authorization = authentication;
                        }
                    }

                    //Set the Headers
                    if (headers != null)
                    {
                        foreach (var header in headers)
                        {
                            request.Headers.Add(header.Key, header.Value);
                        }
                    }
                    
                    //Send the Request
                    var responseMessage = await _httpClient.SendAsync(request);

                    //Return as the expected type
                    return JsonConvert.DeserializeObject<TResponse>(await responseMessage.Content.ReadAsStringAsync());
                }
            }   
            catch (System.Exception)
            {
                //Throw exception to client.
                throw;
            }
        }

        public async Task<TResponse> SendPatchRequest<TResponse, TRequest>(string urlPath, TRequest requestBody, IDictionary<string, string> headers = null, IDictionary<string, string> authenticationHeader = null)
        {
            return await SendRequest<TResponse, TRequest>(new HttpMethod("Patch"), urlPath, requestBody, headers, authenticationHeader);
        }

        public async Task<TResponse> SendPostRequest<TResponse, TRequest>(string urlPath, TRequest requestBody, IDictionary<string, string> headers = null, IDictionary<string, string> authenticationHeader = null)
        {
            return await SendRequest<TResponse, TRequest>(HttpMethod.Post, urlPath, requestBody, headers, authenticationHeader);
        }

        public async Task<TResponse> SendPutRequest<TResponse, TRequest>(string urlPath, TRequest requestBody, IDictionary<string, string> headers = null, IDictionary<string, string> authenticationHeader = null)
        {
            return await SendRequest<TResponse, TRequest>(HttpMethod.Put, urlPath, requestBody, headers, authenticationHeader);
        }

        public async Task<TResponse> SendDeleteRequest<TResponse, TRequest>(string urlPath, TRequest requestBody, IDictionary<string, string> headers = null, IDictionary<string, string> authenticationHeader = null)
        {
            return await SendRequest<TResponse, TRequest>(HttpMethod.Delete, urlPath, requestBody, headers, authenticationHeader);
        }

        private async Task<TResponse> SendRequest<TResponse, TRequest>(HttpMethod methodType, string urlPath, TRequest requestBody, IDictionary<string, string> headers = null, IDictionary<string, string> authenticationHeader = null)
        {
            try
            {
                //Creating a Http request
                using(var request = new HttpRequestMessage(methodType, urlPath))
                {
                    //Set the  Authentication Header
                    if (authenticationHeader != null)
                    {
                        foreach (var header in authenticationHeader)
                        {
                            var authentication = new AuthenticationHeaderValue(header.Key, header.Value);
                            request.Headers.Authorization = authentication;
                        }
                    }

                    //Set the Headers
                    if (headers != null)
                    {
                        foreach (var header in headers)
                        {
                            request.Headers.Add(header.Key, header.Value);
                        }
                    }

                    //Add the Request Body
                    request.Content = new StringContent(
                        JsonConvert.SerializeObject(requestBody, new JsonSerializerSettings 
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        }), Encoding.UTF8, jsonMediaType);

                    //Send the Request
                    var responseMessage = await _httpClient.SendAsync(request);

                    //Return as the expected type
                    return JsonConvert.DeserializeObject<TResponse>(await responseMessage.Content.ReadAsStringAsync());
                }
            }   
            catch (System.Exception)
            {
                //Throw exception to client.
                throw;
            }
        }
    }
}
