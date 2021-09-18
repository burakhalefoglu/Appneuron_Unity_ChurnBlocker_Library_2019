using System.Collections;
using System.Collections.Generic;
using RestSharp;
using UnityEngine;
using Newtonsoft.Json;
using System.Net;
using Assets.Appneuron.Core.CoreServices.RestClientServices.Abstract;
using Assets.Appneuron.Core.CoreServices.ResultService;
using System.Threading.Tasks;
using Appneuron.Core.DataModel.Concrete;

namespace Assets.Appneuron.Core.CoreServices.RestClientServices.Concrete.RestSharp
{
    public class RestSharpServices : IRestClientServices
    {


        public async Task<IDataResult<T>> GetAsync<T>(string url)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("accept", "application/json");
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + TokenSingletonModel.Instance.Token);

            IRestResponse response = await client.ExecuteAsync(request);

            var userData = JsonConvert.DeserializeObject<T>(response.Content,
                    new JsonSerializerSettings
                    {
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects
                    });

            HttpStatusCode statusCode = response.StatusCode;
            int numericStatusCode = (int)statusCode;
            var isSuccess = response.IsSuccessful;
            if (isSuccess)
            {
                return new SuccessDataResult<T>(userData, statuseCode: numericStatusCode);
            }
            return new ErrorDataResult<T>(userData, statuseCode: numericStatusCode);

        }

        public async Task<IDataResult<T>> PostAsync<T>(string url, object sendObject)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("accept", "application/json");
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + TokenSingletonModel.Instance.Token);
            var jsonObject = JsonConvert.SerializeObject(sendObject);
            request.AddParameter("application/json", jsonObject, ParameterType.RequestBody);

            IRestResponse response = await client.ExecuteAsync(request);


            var data = JsonConvert.DeserializeObject<T>(response.Content,
                              new JsonSerializerSettings
                              {
                                  PreserveReferencesHandling = PreserveReferencesHandling.Objects
                              });

            HttpStatusCode statusCode = response.StatusCode;
            int numericStatusCode = (int)statusCode;
            Debug.Log(response.Content);
            var isSuccess = response.IsSuccessful;
            if (isSuccess)
            {
                return new SuccessDataResult<T>(data, statuseCode: numericStatusCode);
            }

            return new ErrorDataResult<T>(data, numericStatusCode);
        }

        public async Task<IDataResult<T>> PutAsync<T>(string url, object sendObject)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.PUT);
            request.AddHeader("accept", "application/json");
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + TokenSingletonModel.Instance.Token);
            var jsonObject = JsonConvert.SerializeObject(sendObject);
            request.AddParameter("application/json", jsonObject, ParameterType.RequestBody);

            IRestResponse response = await client.ExecuteAsync(request);


            var data = JsonConvert.DeserializeObject<T>(response.Content,
                              new JsonSerializerSettings
                              {
                                  PreserveReferencesHandling = PreserveReferencesHandling.Objects
                              });

            HttpStatusCode statusCode = response.StatusCode;
            int numericStatusCode = (int)statusCode;
            Debug.Log(response.Content);
            var isSuccess = response.IsSuccessful;
            if (isSuccess)
            {
                return new SuccessDataResult<T>(data, statuseCode: numericStatusCode);
            }

            return new ErrorDataResult<T>(data, numericStatusCode);
        }


        public async Task<IResult> DeleteAsync(string url, string id)
        {
            var client = new RestClient(url + "/id?=${id}");
            var request = new RestRequest(Method.DELETE);
            IRestResponse response = await client.ExecuteAsync(request);

            HttpStatusCode statusCode = response.StatusCode;
            int numericStatusCode = (int)statusCode;
            var isSuccess = response.IsSuccessful;
            if (isSuccess)
            {
                return new SuccessResult(numericStatusCode);
            }
            return new ErrorResult(numericStatusCode);
        }

        public async Task<IResult> IsInternetConnectedAsync()
        {
            var client = new RestClient("https://www.google.com/");
            var request = new RestRequest(Method.GET);
            IRestResponse response = await client.ExecuteAsync(request);

            var isSuccess = response.IsSuccessful;
            if (isSuccess)
            {
                return new SuccessResult();
            }
            return new ErrorResult();
        }
    }
}
