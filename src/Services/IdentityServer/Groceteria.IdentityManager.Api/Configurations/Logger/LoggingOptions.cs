﻿namespace Groceteria.IdentityManager.Api.Configurations.Logger
{
    public class LoggingOptions
    {
        public bool IncludeScopes { get; set; }
        public string LogOutputTemplate { get; set; }
        public Console Console { get; set; }
        public Elastic Elastic { get; set; }    
    }

    public class Console
    {
        public bool Enabled { get; set; }
        public string LogLevel { get; set; }
    }

    public class Elastic
    {
        public bool Enabled { get; set; }
        public string LogLevel { get; set; }
    }
}
