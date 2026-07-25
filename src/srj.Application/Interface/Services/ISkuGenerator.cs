using srj.Domain.Enums;
using srj.Domain.Models;

namespace srj.Application.Interface.Services;

public interface ISkuGenerator
{
    Task<string> GenerateAsync(Metal metal, ItemCategory category);
}