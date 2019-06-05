using System.Threading.Tasks;
using System.Collections.Generic;   

namespace WebCommunicator.Interface
{
    public interface IWebCommunicator
    {
         Task<TResponse> SendGetRequest<TResponse> (string urlPath, IDictionary<string, string> headers = null, IDictionary<string, string> authenticationHeader = null);
         Task<TResponse> SendPostRequest<TResponse, TRequest> (string urlPath, TRequest requestBody, IDictionary<string, string> headers = null, IDictionary<string, string> authenticationHeader = null);
         Task<TResponse> SendPutRequest<TResponse, TRequest> (string urlPath, TRequest requestBody, IDictionary<string, string> headers = null, IDictionary<string, string> authenticationHeader = null);
         Task<TResponse> SendPatchRequest<TResponse, TRequest> (string urlPath, TRequest requestBody, IDictionary<string, string> headers = null, IDictionary<string, string> authenticationHeader = null);
         Task<TResponse> SendDeleteRequest<TResponse, TRequest> (string urlPath, TRequest requestBody, IDictionary<string, string> headers = null, IDictionary<string, string> authenticationHeader = null);
    }
}