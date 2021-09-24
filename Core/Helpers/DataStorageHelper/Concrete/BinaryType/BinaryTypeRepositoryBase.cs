namespace AppneuronUnity.Core.Helpers.DataStorageHelper.Concrete.BinaryType
{
    using AppneuronUnity.Core.Helpers.DataStorageHelper.Abstract;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading.Tasks;
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="BinaryTypeRepositoryBase{T}" />.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    internal class BinaryTypeRepositoryBase<T> : IRepositoryService<T>
        where T : class, new()
    {
        /// <summary>
        /// The SelectAsync.
        /// </summary>
        /// <param name="filePath">The filePath<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{T}"/>.</returns>
        public Task<T> SelectAsync(string filePath)
        {
            var binaryFormatter = new BinaryFormatter();
            string savePath = filePath + ".data";
            if (!File.Exists(savePath))
            {
                T entity = new T();
                return Task.FromResult(entity);
            }
            using (var fileStream = File.Open(savePath,
            FileMode.OpenOrCreate,
            FileAccess.ReadWrite,
            FileShare.None))
            {
                T dataModel = (T)binaryFormatter.Deserialize(fileStream);
                return Task.FromResult(dataModel);
            }
        }

        /// <summary>
        /// The InsertAsync.
        /// </summary>
        /// <param name="filePath">The filePath<see cref="string"/>.</param>
        /// <param name="dataModel">The dataModel<see cref="T"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task InsertAsync(string filePath, T dataModel)
        {
            var binaryFormatter = new BinaryFormatter();
            string savePath = filePath + ".data";
            await Task.Run(() =>
            {
                using (var fileStream = File.Create(savePath))
                {
                    binaryFormatter.Serialize(fileStream, dataModel);
                }
            });
        }

        /// <summary>
        /// The DeleteAsync.
        /// </summary>
        /// <param name="filePath">The filePath<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task DeleteAsync(string filePath)
        {
            string saveFilePath = filePath + ".data";
            await Task.Run(() =>
            {
                File.Delete(saveFilePath);
            });
            Debug.Log("File Deleted");
        }
    }
}
