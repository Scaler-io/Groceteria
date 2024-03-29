﻿namespace Groceteria.IdentityManager.Api.Configurations.ElasticSearch
{
    public class ElasticSearchConfiguration
    {
        public string Uri { get; set; }
        public string IdetityClientIndex { get; set; }
        public string IdentityScopeIndex { get; set; }
        public string IdentityApiResourceIndex { get; set; }
        public string IdentityResourceIndex { get; set; }
    }
}
