using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Repository;

public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
{
    private readonly HotelListingDbContext _hotelListingDbContext;
    
    public CountriesRepository(HotelListingDbContext hotelListingDbContext, IMapper mapper) : base(hotelListingDbContext, mapper)
    {
        this._hotelListingDbContext = hotelListingDbContext;
    }

    public async Task<Country> GetDetails(int id)
    {
        return await _hotelListingDbContext.Countries.Include(q => q.Hotels)
            .FirstOrDefaultAsync(q => q.Id == id);
    }
}