using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersAPI.Provider
{
    public class BaseRepository
    {
        public readonly IConfiguration Configuration;
        private readonly string ConnectionString;
        public MongoClient client;
        public BaseRepository(IConfiguration config, IWebHostEnvironment environment)
        {
            Configuration = config;

            if (environment.IsDevelopment())
            {
                ConnectionString = Configuration["MongoDB:ConnectionString"];
            }
            else
            {
                var client = new SecretClient(new Uri(uriString: $"https://vgflow.vault.azure.net/"), new DefaultAzureCredential());
                var mongoDBsecret = client.GetSecretAsync("MongoDBConnectionString").Result;
                ConnectionString = mongoDBsecret.Value.Value.ToString();
            }

            var settings = MongoClientSettings.FromConnectionString(ConnectionString);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            client = new MongoClient(settings);
        }
    }
}
