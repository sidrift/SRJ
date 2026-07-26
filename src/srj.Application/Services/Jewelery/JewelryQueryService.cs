using srj.Application.Dtos.Commons;
using srj.Application.Dtos.Request.Jewelery;
using srj.Application.Dtos.Response.Silver;
using srj.Application.Interface.Repository;
using srj.Application.Interface.Services.Jewelery;

namespace srj.Application.Services.Jewelery;

public class JewelryQueryService : IJewelryQueryService
{
    private readonly IJewelryItemRepository _jewelryItemRepository;

    public JewelryQueryService(IJewelryItemRepository  jewelryItemRepository)
    {
        _jewelryItemRepository = jewelryItemRepository;
    }

    public async Task<PagedResponse<JewelryListResponse>> GetAllAsync(JewelrySearchRequest request)
    {
        return await _jewelryItemRepository.SearchAsync(request);
   }
}