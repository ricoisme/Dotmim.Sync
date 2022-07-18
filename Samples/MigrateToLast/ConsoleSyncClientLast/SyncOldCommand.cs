﻿using Dotmim.Sync;
using Dotmim.Sync.Sqlite;
using Dotmim.Sync.SqlServer;
using Dotmim.Sync.Web.Client;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSyncClient
{

    [Command("old", Description = "v0.94: This command will execute a simple sync to the server version 0.94")]
    public class SyncOldCommand
    {

        public SyncOldCommand(IConfiguration configuration, IOptions<ApiOptions> apiOptions)
        {
            ApiOptions = apiOptions.Value;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public ApiOptions ApiOptions { get; }


        [Option("-s")]
        public ScopeNames Scope { get; } = ScopeNames.DefaultScope;

        [Option("-r")]
        public bool Reinitialize { get; set; } = false;


        public async Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            var serverOrchestrator = new WebRemoteOrchestrator(ApiOptions.SyncAddressOld);

            // Second provider is using plain old Sql Server provider, relying on triggers and tracking tables to create the sync environment
            var connectionString = Configuration.GetConnectionString(ProviderType.Sql, "Client");

            var clientProvider = new SqlSyncProvider(connectionString);

            try
            {
                if (Scope == ScopeNames.Logs)
                    await SyncServices.SynchronizeLogsAsync(serverOrchestrator, clientProvider, new SyncOptions(), Reinitialize);
                else
                    await SyncServices.SynchronizeDefaultAsync(serverOrchestrator, clientProvider, new SyncOptions(), Reinitialize);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
            return 1;
        }

    }
}
