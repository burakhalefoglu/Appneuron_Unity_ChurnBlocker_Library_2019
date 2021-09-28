namespace AppneuronUnity.Core.ObjectBases.DataStorageHelper.Concrete.BinaryType
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading.Tasks;
    using UnityEngine;
    using AppneuronUnity.Core.ObjectBases.DataStorageHelper.Abstract;

    internal class BinaryTypeRepositoryBase<T> : IRepositoryService<T>
        where T : class, new()
    {

        public Task<T> SelectAsync(string fileName)
        {
            var directoryName = Application.persistentDataPath + $"/Appneuron/{typeof(T).Name}/";

            var binaryFormatter = new BinaryFormatter();
            string savePath = directoryName + fileName + ".data";

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

        public async Task InsertAsync(string fileName, T dataModel)
        {
            var directoryName = Application.persistentDataPath + $"/Appneuron/{typeof(T).Name}/";
            Directory.CreateDirectory(directoryName);

            var binaryFormatter = new BinaryFormatter();
            string savePath = directoryName + fileName + ".data";
            await Task.Run(() =>
            {
                using (var fileStream = File.Create(savePath))
                {
                    binaryFormatter.Serialize(fileStream, dataModel);
                }
            });
        }

        public async Task DeleteAsync(string fileName)
        {
            var directoryName = Application.persistentDataPath + $"/Appneuron/{typeof(T).Name}/";
            string saveFilePath = directoryName + fileName + ".data";
            await Task.Run(() =>
            {
                File.Delete(saveFilePath);
            });
            Debug.Log("File Deleted");
        }
    }
}
