using srj.Domain.Enums;

namespace srj.Application.Dtos.Response.Items;

public class ItemCategoryResponse
{
    public long Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public Metal Metal { get; set; }
}