﻿namespace Groceteria.Catalogue.Api.Models.Requests
{
    public class BrandUpsertRequest
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
