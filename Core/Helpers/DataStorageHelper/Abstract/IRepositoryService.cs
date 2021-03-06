namespace AppneuronUnity.Core.ObjectBases.DataStorageHelper.Abstract
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IRepositoryService{T}" />.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    internal interface IRepositoryService<T>
    {
        /// <summary>
        /// The InsertAsync.
        /// </summary>
        /// <param name="filePath">The filePath<see cref="string"/>.</param>
        /// <param name="dataModel">The dataModel<see cref="T"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task InsertAsync(string fileName, T dataModel);

        /// <summary>
        /// The SelectAsync.
        /// </summary>
        /// <param name="filePath">The filePath<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{T}"/>.</returns>
        Task<T> SelectAsync(string fileName);

        /// <summary>
        /// The DeleteAsync.
        /// </summary>
        /// <param name="filePath">The filePath<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task DeleteAsync(string fileName);
    }
}
