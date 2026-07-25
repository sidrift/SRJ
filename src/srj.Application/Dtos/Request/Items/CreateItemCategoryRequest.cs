using System.ComponentModel.DataAnnotations;
using srj.Domain.Enums;

namespace srj.Application.Dtos.Request.Items;

public class CreateItemCategoryRequest
{
    [Required] public string Name { get; set; } = string.Empty;

    [EnumDataType(typeof(Metal))] public Metal Metal { get; set; }
}