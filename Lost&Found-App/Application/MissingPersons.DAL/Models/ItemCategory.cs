using System;
using System.Collections.Generic;

namespace MissingPersons.DAL.Models;

public partial class ItemCategory
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? ParentCategoryId { get; set; }

    public virtual ICollection<ItemCategory> InverseParentCategory { get; set; } = new List<ItemCategory>();

    public virtual ICollection<ItemDetail> ItemDetails { get; set; } = new List<ItemDetail>();

    public virtual ItemCategory? ParentCategory { get; set; }
}
