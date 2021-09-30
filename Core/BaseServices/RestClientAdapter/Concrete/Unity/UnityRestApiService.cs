using AppneuronUnity.Core.Libraries.LitJson;
using AppneuronUnity.Core.Results;
using System;
using System.Threading.Tasks;
using UnityEngine.Networking;
using AppneuronUnity.Core.Adapters.RestClientAdapter.Abstract;
using AppneuronUnity.Core.AuthModule.AuthComponent.DataModel.Jwt;
using UnityEngine;

// Doc: https://github.com/burakhalefoglu/unitywebrequest-tutorial

namespace AppneuronUnity.Core.Adapters.RestClientAdapter.Concrete.Unity
{
    internal class UnityRestApiService : IRestClientServices
    {
        public async Task<IResult> DeleteAsync(string url, string id)
        {
            try
            {
                using (var www = UnityWebRequest.Delete(url + "/id?=${id}"))
                {
                    www.SetRequestHeader("accept", "application/json");
                    www.SetRequestHeader("content-type", "application/json");
                    www.SetRequestHeader("authorization", "Bearer " +
                        TokenSingletonModel.Instance.Token);

                    var operation = www.SendWebRequest();

                    while (!operation.isDone)
                        await Task.Yield();

                    if (www.isNetworkError)
                        return new ErrorResult((int)www.responseCode);

                    return new SuccessResult((int)www.responseCode);


                }
            }
            catch (Exception ex)
            {
                //TODO: Exception log
                return new ErrorResult(0);
            }
        }

        public async Task<IDataResult<T>> GetAsync<T>(string url)
        {
            try
            {
                using (var www = UnityWebRequest.Get(url))
                {
                    www.SetRequestHeader("accept", "application/json");
                    www.SetRequestHeader("content-type", "application/json");
                    www.SetRequestHeader("authorization", "Bearer " + TokenSingletonModel.Instance.Token);

                    var operation = www.SendWebRequest();

                    while (!operation.isDone)
                        await Task.Yield();
                    var userData = JsonMapper.ToObject<T>(www.downloadHandler.text);
                    if (www.isNetworkError || userData == null)
                        return new ErrorDataResult<T>(statuseCode: (int)www.responseCode);

                    return new SuccessDataResult<T>(userData, statuseCode: (int)www.responseCode);


                }
            }
            catch (Exception ex)
            {
                //TODO: Exception log
                return new ErrorDataResult<T>(statuseCode: 0);
            }
        }

        public async Task<IResult> IsInternetConnectedAsync()
        {
            try
            {
                using (var www = UnityWebRequest.Get("https://www.google.com/"))
                {

                    var operation = www.SendWebRequest();

                    while (!operation.isDone)
                        await Task.Yield();

                    if (www.isNetworkError)
                    {
                        return new SuccessResult();
                    }
                    return new ErrorResult();

                }
            }
            catch (Exception ex)
            {
                //TODO: Exception log
                return new ErrorResult();
            }

        }


        public async Task<IDataResult<T>> PostAsync<T>(string url, object sendObject)
        {
            try
            {
                var jsonObject = JsonMapper.ToJson(sendObject);
                using (var www = UnityWebRequest.Post(url, jsonObject))
                {
                    www.SetRequestHeader("accept", "application/json");
                    www.SetRequestHeader("content-type", "application/json");
                    www.SetRequestHeader("authorization", "Bearer " + TokenSingletonModel.Instance.Token);

                    var operation = www.SendWebRequest();

                    while (!operation.isDone)
                        await Task.Yield();
                    var userData = JsonMapper.ToObject<T>(www.downloadHandler.text);
                    if (www.isNetworkError || userData == null)
                        return new ErrorDataResult<T>(statuseCode: (int)www.responseCode);

                    return new SuccessDataResult<T>(userData, statuseCode: (int)www.responseCode);


                }
            }
            catch (Exception ex)
            {
                //TODO: Exception log
                return new ErrorDataResult<T>(statuseCode: 0);
            }
        }

        public async Task<IDataResult<T>> PutAsync<T>(string url, object sendObject)
        {
            try
            {


                var jsonObject = JsonMapper.ToJson(sendObject);
                using (var www = UnityWebRequest.Put(url, jsonObject))
                {
                    www.SetRequestHeader("accept", "application/json");
                    www.SetRequestHeader("content-type", "application/json");
                    www.SetRequestHeader("authorization", "Bearer " + TokenSingletonModel.Instance.Token);

                    var operation = www.SendWebRequest();

                    while (!operation.isDone)
                        await Task.Yield();

                    var userData = JsonMapper.ToObject<T>(www.downloadHandler.text);
                    if (www.isNetworkError || userData == null)
                        return new ErrorDataResult<T>(statuseCode: (int)www.responseCode);

                    return new SuccessDataResult<T>(userData, statuseCode: (int)www.responseCode);


                }
            }
            catch (Exception ex)
            {
                //TODO: Exception log
                return new ErrorDataResult<T>(statuseCode: 0);
            }
        }
    }
}
