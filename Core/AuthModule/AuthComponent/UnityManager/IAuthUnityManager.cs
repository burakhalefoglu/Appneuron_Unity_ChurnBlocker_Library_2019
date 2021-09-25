namespace AppneuronUnity.Core.AuthModule.AuthComponent.UnityManager
{
    using System.Threading.Tasks;
using AppneuronUnity.Core.AuthModule.AuthComponent.DataModel;

    /// <summary>
    /// Defines the <see cref="IAuthUnityManager" />.
    /// </summary>
    internal interface IAuthUnityManager
    {
        /// <summary>
        /// The Login.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        Task Login();

        /// <summary>
        /// The checkTokenOnfile.
        /// </summary>
        /// <returns>The <see cref="Task{TokenDataModel}"/>.</returns>
        Task<TokenDataModel> checkTokenOnfile();

        /// <summary>
        /// The SaveTokenOnfile.
        /// </summary>
        /// <param name="tokenDataModel">The tokenDataModel<see cref="TokenDataModel"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task SaveTokenOnfile(TokenDataModel tokenDataModel);

    }
}
