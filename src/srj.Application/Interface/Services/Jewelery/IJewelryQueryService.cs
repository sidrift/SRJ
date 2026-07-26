using srj.Application.Dtos.Commons;
using srj.Application.Dtos.Request.Jewelery;
using srj.Application.Dtos.Response.Silver;

namespace srj.Application.Interface.Services.Jewelery;

public interface IJewelryQueryService
{
    Task<PagedResponse<JewelryListResponse>> GetAllAsync(JewelrySearchRequest request);
}