using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using Assets.Appneuron.Core.CoreServices.DataStorageService.Abstract;
using System.Threading.Tasks;
using System;

namespace Assets.Appneuron.Core.CoreServices.DataStorageService.Concrete.BinaryType
{
    public class BinaryTypeRepositoryBase<T> : IRepositoryService<T>
        where T : class, new()
    {


        public Task<T> SelectAsync(string filePath)
        {
            var binaryFormatter = new BinaryFormatter();
            string savePath = filePath + ".data";
            if (!File.Exists(savePath))
            {
                T entity = new T();
                return Task.FromResult<T>(entity);
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
