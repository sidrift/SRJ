using srj.Application.Dtos.Commons;
using srj.Application.Dtos.Request.Jewelery;
using srj.Application.Dtos.Response.Silver;
using srj.Application.Interface.Repository;
using srj.Application.Interface.Services;
using srj.Application.Interface.Services.Jewelery;

namespace srj.Application.Services.Jewelery;

public class JewelryQueryService : IJewelryQueryService
{
    private readonly IIndiaDateTimeService _indiaDateTimeService;
    private readonly IJewelryItemRepository _jewelryItemRepository;

    public JewelryQueryService(IJewelryItemRepository  jewelryItemRepository, IIndiaDateTimeService indiaDateTimeService)
    {
        _jewelryItemRepository = jewelryItemRepository;
        _indiaDateTimeService = indiaDateTimeService;
    }

    public async Task<PagedResponse<JewelryListResponse>> GetAllAsync(JewelrySearchRequest request)
    {
        var result = await _jewelryItemRepository.SearchAsync(request);

        foreach (var item in result.Items)
        {
            item.CreatedAt = _indiaDateTimeService.ConvertFromUtc(item.CreatedAt);
        }

        return result;
    }
}