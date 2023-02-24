﻿namespace Groceteria.Catalogue.Api.Entities
{
    public class Category: BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}