using MvcAWSElastiCache.Helpers;
using MvcAWSElastiCache.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MvcAWSElastiCache.Services
{
    public class ServiceCacheRedis
    {
        private IDatabase cache;

        public ServiceCacheRedis()
        {
            this.cache = HelperCacheRedis.Connection.GetDatabase();
        }

        public async Task<List<Coche>> GetCochesFavoritosAsync()
        {
            //TENDREMOS ALMACENADO EN CACHE UN JSON DE COLECCION DE COHES
            string jsonCoches = await this.cache.StringGetAsync("favoritos");
            if (jsonCoches == null)
            {
                return null;
            }else
            {
                List<Coche> cars = JsonConvert.DeserializeObject<List<Coche>>(jsonCoches);
                return cars;
            }
        }

        public async Task AddFavoritoAsync(Coche car)
        {
            List<Coche> fav = await this.GetCochesFavoritosAsync();
            if (fav == null)
            {
                fav = new List<Coche>();
            }
            fav.Add(car);
            string jsonCoches = JsonConvert.SerializeObject(fav);
            await this.cache.StringSetAsync("favoritos", jsonCoches, TimeSpan.FromMinutes(30)); //q se guarden 30 min
        }

        public async Task DeleteFavoritoAsync(int idcoche)
        {
            List<Coche> favoritos = await this.GetCochesFavoritosAsync();
            if (favoritos != null)
            {
                Coche carDelete = favoritos.FirstOrDefault(x => x.IdCoche == idcoche);
                favoritos.Remove(carDelete);
                if (favoritos.Count == 0)
                {
                    await this.cache.KeyDeleteAsync("favoritos");
                }
                else
                {
                    string jsonCoches = JsonConvert.SerializeObject(favoritos);
                    await this.cache.StringSetAsync("favoritos", jsonCoches, TimeSpan.FromMinutes(30));
                }
            }
        }
    }
}
