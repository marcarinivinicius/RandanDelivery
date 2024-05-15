using Microsoft.EntityFrameworkCore;
using Vehicle.Domain.Entities;
using Vehicle.Infra.Context;
using Vehicle.Infra.Interfaces;
using Vehicle.Infra.Models;

namespace Vehicle.Infra.Repositories
{
    public class MotoRepository(VehicleContext context) : BaseRepository<Moto>(context), IMotoRepository
    {
        public async Task<List<Moto>> GetAll(MotoFilters? filters = null)
        {
            IQueryable<Moto> query = context.Motocycles.AsNoTracking();

            if (filters != null)
            {
                if (!filters.AllRecords)
                {
                    if (filters.Active)
                    {
                        query = query.Where(moto => moto.Active);
                    }
                    else
                    {
                        query = query.Where(moto => moto.Active == false);
                    }
                }
                if (filters.AllLocated)
                {
                    if (filters.Located)
                    {
                         query = query.Where(moto => moto.Active);
                    }
                }

                if (!string.IsNullOrEmpty(filters.PlateCode))
                {
                    query = query.Where(moto => moto.PlateCode.Contains(filters.PlateCode));
                }
                
            }

            var motos = await query.ToListAsync();
            return motos;
        }
    }
}
