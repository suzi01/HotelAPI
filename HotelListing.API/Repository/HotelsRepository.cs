using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;

namespace HotelListing.API.Repository;

public class HotelsRepository : GenericRepository<Hotel>, IHotelsRepository
{
    public HotelsRepository(HotelListingDbContext hotelListingDbContext, IMapper mapper) : base(hotelListingDbContext, mapper)
    {
    }
    
}