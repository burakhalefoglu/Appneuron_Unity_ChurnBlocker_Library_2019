namespace AppneuronUnity.Core.CoreServices.RestClientServices.Abstract
{
    using AppneuronUnity.Core.Results;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IRestClientServices" />.
    /// </summary>
    internal interface IRestClientServices
    {
        Task<IDataResult<T>> GetAsync<T>(string url);

        Task<IDataResult<T>> PostAsync<T>(string url, object sendObject);

        Task<IDataResult<T>> PutAsync<T>(string url, object sendObject);

        Task<IResult> DeleteAsync(string url, string id);
        
        Task<IResult> IsInternetConnectedAsync();
    }
}
