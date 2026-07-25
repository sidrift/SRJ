using System.ComponentModel.DataAnnotations;

namespace srj.Application.Dtos.Request.Items;

public class UpdateItemCategoryRequest
{
    [Required] public string Name { get; set; } = string.Empty;
}