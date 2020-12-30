using DbLunch.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbLunch.Data
{
    public abstract class BaseRepository<T> : IRepository<T> where T : BaseModel
    {
        private string _filePath;

        public BaseRepository(string filePath)
        {
            _filePath = $"Data/Json/{filePath}";
        }

        public async Task<IEnumerable<T>> All()
        {
            var jsonString = await File.ReadAllTextAsync(_filePath, Encoding.UTF8 );
            var json = JsonConvert.DeserializeObject<T[]>(jsonString);
            return json;
        }

        public async Task Delete(int id)
        {
            List<T> modelArray = (await All()).ToList();
            T removed = modelArray.Find(u => u.Id == id);
            if (removed != null)
            {
                modelArray.Remove(removed);
                var jsonString = JsonConvert.SerializeObject(modelArray);
                await File.WriteAllTextAsync(_filePath, jsonString);
                return;
            }
            throw new ArgumentException($"{typeof(T)} with id {id} was not found.");
        }

        public async Task<T> Find(int id)
        {
            var modelArray = await All();
            return modelArray.First(u => u.Id == id);
        }

        public async Task Insert(T modelInsert)
        {
            var modelArray = await All();
            modelInsert.Id = NextId(modelArray);

            var newList = modelArray.ToList();
            newList.Add(modelInsert);

            string jsonString = JsonConvert.SerializeObject(newList, Formatting.Indented);
            await File.WriteAllTextAsync(_filePath, jsonString, Encoding.UTF8);
            return;
        }

        public Task Update(T model)
        {
            throw new NotImplementedException();
        }

        private bool IdExists(IEnumerable<T> modelArray, int id)
        {
            return modelArray.Any(x => x.Id == id);
        }

        private int NextId(IEnumerable<T> modelArray)
        {
            return modelArray.Max(x => x.Id) + 1;
        }
    }
}
