using srj.Domain.Enums;

namespace srj.Domain.Models;

public class ItemCategory
{
    public long Id { get; set; }

    public string Name { get; set; }

    public Metal Metal { get; set; }
}