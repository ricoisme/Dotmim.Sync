﻿using Dotmim.Sync.Batch;
using Dotmim.Sync.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Dotmim.Sync.Enumerations;
using System.Runtime.Serialization;
using System.Data.Common;
using Dotmim.Sync.Web.Client.BackwardCompatibility;

namespace Dotmim.Sync.Web.Client
{

    public interface IScopeMessage
    {
        SyncContext SyncContext { get; set; }
    }

    [DataContract(Name = "changesres"), Serializable]
    public class HttpMessageSendChangesResponse : IScopeMessage
    {

        public HttpMessageSendChangesResponse()
        {

        }

        public HttpMessageSendChangesResponse(SyncContext context)
            => this.SyncContext = context ?? throw new ArgumentNullException(nameof(context));

        /// <summary>
        /// Gets or Sets the Server HttpStep
        /// </summary>
        [DataMember(Name = "ss", IsRequired = true, Order = 1)]

        public HttpStep ServerStep { get; set; }

        /// <summary>
        /// Gets or Sets the SyncContext
        /// </summary>
        [DataMember(Name = "sc", IsRequired = true, Order = 2)]
        public SyncContext SyncContext { get; set; }

        /// <summary>
        /// Gets the current batch index, send from the server 
        /// </summary>
        [DataMember(Name = "bi", IsRequired = true, Order = 3)]
        public int BatchIndex { get; set; }

        /// <summary>
        /// Gets the number of batch to send
        /// </summary>
        [DataMember(Name = "bc", IsRequired = false, Order = 4)]
        public int BatchCount { get; set; }

        /// <summary>
        /// Gets or Sets if this is the last Batch send from the server 
        /// </summary>
        [DataMember(Name = "islb", IsRequired = true, Order = 5)]
        public bool IsLastBatch { get; set; }

        /// <summary>
        /// The remote client timestamp generated by the server database
        /// </summary>
        [DataMember(Name = "rct", IsRequired = true, Order = 6)]
        public long RemoteClientTimestamp { get; set; }

        /// <summary>
        /// Gets the BatchParInfo send from the server 
        /// </summary>
        [DataMember(Name = "changes", IsRequired = true, Order = 7)]
        public ContainerSet Changes { get; set; } // BE CAREFUL: If changes the order, change it too in "ContainerSetBoilerPlate" !

        /// <summary>
        /// Gets the changes stats from the server
        /// </summary>
        [DataMember(Name = "scs", IsRequired = true, Order = 8)]
        public DatabaseChangesSelected ServerChangesSelected { get; set; }

        /// <summary>
        /// Gets the changes stats from the server
        /// </summary>
        [DataMember(Name = "cca", IsRequired = true, Order = 9)]
        public DatabaseChangesApplied ClientChangesApplied { get; set; }

        /// <summary>
        /// Gets or Sets the conflict resolution policy from the server
        /// </summary>

        [DataMember(Name = "policy", IsRequired = true, Order = 10)]
        public ConflictResolutionPolicy ConflictResolutionPolicy { get; set; }


    }

    [DataContract(Name = "morechangesreq"), Serializable]
    public class HttpMessageGetMoreChangesRequest : IScopeMessage
    {
        public HttpMessageGetMoreChangesRequest() { }

        public HttpMessageGetMoreChangesRequest(SyncContext context, int batchIndexRequested)
        {
            this.BatchIndexRequested = batchIndexRequested;
            this.SyncContext = context ?? throw new ArgumentNullException(nameof(context));
        }
        [DataMember(Name = "sc", IsRequired = true, Order = 1)]
        public SyncContext SyncContext { get; set; }

        [DataMember(Name = "bireq", IsRequired = true, Order = 2)]
        public int BatchIndexRequested { get; set; }

    }

    [DataContract(Name = "changesreq"), Serializable]
    public class HttpMessageSendChangesRequest : IScopeMessage
    {
        public HttpMessageSendChangesRequest()
        {

        }

        public HttpMessageSendChangesRequest(SyncContext context, ScopeInfoClient cScopeInfoClient)
        {
            this.SyncContext = context;
            this.ScopeInfoClient = cScopeInfoClient;
        }

        [DataMember(Name = "sc", IsRequired = true, Order = 1)]
        public SyncContext SyncContext { get; set; }


        // IsRequired = false to preserve backward compat
        [DataMember(Name = "scopeclient", IsRequired = false, Order = 2)]
        public ScopeInfoClient ScopeInfoClient { get; set; }

        /// <summary>
        /// Get the current batch index 
        /// </summary>
        [DataMember(Name = "bi", IsRequired = true, Order = 3)]
        public int BatchIndex { get; set; }

        /// <summary>
        /// Get the current batch count
        /// </summary>
        [DataMember(Name = "bc", IsRequired = false, Order = 4)]
        public int BatchCount { get; set; }

        /// <summary>
        /// Gets or Sets if this is the last Batch to sent to server 
        /// </summary>
        [DataMember(Name = "islb", IsRequired = true, Order = 5)]
        public bool IsLastBatch { get; set; }

        /// <summary>
        /// Changes to send
        /// </summary>
        [DataMember(Name = "changes", IsRequired = true, Order = 6)]
        public ContainerSet Changes { get; set; }

        /// <summary>
        /// Client last sync timestamp
        /// </summary>
        [DataMember(Name = "clst", IsRequired = false, Order = 7)] // IsRequired = false to preserve backward compat
        public long ClientLastSyncTimestamp { get; set; }

        /// <summary>
        /// Pre 0.9.6 old ScopeInfo object
        /// </summary>
        [DataMember(Name = "scope", IsRequired = false, Order = 8)] // IsRequired = false to preserve backward compat
        public OldScopeInfo OldScopeInfo { get; set; }

    }

    [DataContract(Name = "ensureschemares"), Serializable]
    public class HttpMessageEnsureSchemaResponse : IScopeMessage
    {
        public HttpMessageEnsureSchemaResponse()
        {

        }
        public HttpMessageEnsureSchemaResponse(SyncContext context, ScopeInfo sScopeInfo)
        {
            this.SyncContext = context ?? throw new ArgumentNullException(nameof(context));
            this.ServerScopeInfo = sScopeInfo ?? throw new ArgumentNullException(nameof(sScopeInfo));
            this.Schema = sScopeInfo.Schema;
        }

        [DataMember(Name = "sc", IsRequired = true, Order = 1)]
        public SyncContext SyncContext { get; set; }

        /// <summary>
        /// Gets or Sets the schema because the ServerScopeInfo won't have it since it's marked (on purpose) as IgnoreDataMember (and then not serialized)
        /// </summary>
        [DataMember(Name = "schema", IsRequired = true, Order = 2)]
        public SyncSet Schema { get; set; }

        /// <summary>
        /// Gets or Sets the server scope info, from server
        /// </summary>
        [DataMember(Name = "ssi", IsRequired = true, Order = 3)]
        public ScopeInfo ServerScopeInfo { get; set; }

    }


    [DataContract(Name = "ensurescopesres"), Serializable]
    public class HttpMessageEnsureScopesResponse : IScopeMessage
    {
        public HttpMessageEnsureScopesResponse()
        {

        }
        public HttpMessageEnsureScopesResponse(SyncContext context, ScopeInfo sScopeInfo)
        {
            this.SyncContext = context ?? throw new ArgumentNullException(nameof(context));
            this.ServerScopeInfo = sScopeInfo ?? throw new ArgumentNullException(nameof(sScopeInfo));
        }

        [DataMember(Name = "sc", IsRequired = true, Order = 1)]
        public SyncContext SyncContext { get; set; }

        /// <summary>
        /// Gets or Sets the schema option (without schema itself, that is not serializable)
        /// </summary>
        [DataMember(Name = "serverscope", IsRequired = true, Order = 2)]
        public ScopeInfo ServerScopeInfo { get; set; }
    }


    [DataContract(Name = "ensurereq"), Serializable]
    public class HttpMessageEnsureScopesRequest : IScopeMessage
    {
        public HttpMessageEnsureScopesRequest() { }

        /// <summary>
        /// Create a new message to web remote server.
        /// Scope info table name is not provided since we do not care about it on the server side
        /// </summary>
        public HttpMessageEnsureScopesRequest(SyncContext context)
        {
            this.SyncContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        [DataMember(Name = "sc", IsRequired = true, Order = 1)]
        public SyncContext SyncContext { get; set; }


    }


    [DataContract(Name = "opreq"), Serializable]
    public class HttpMessageOperationRequest : IScopeMessage
    {
        public HttpMessageOperationRequest() { }

        public HttpMessageOperationRequest(SyncContext context, ScopeInfo cScopeInfo, ScopeInfoClient cScopeInfoClient)
        {
            this.SyncContext = context ?? throw new ArgumentNullException(nameof(context));
            this.ScopeInfoFromClient = cScopeInfo;
            this.ScopeInfoClient = cScopeInfoClient;
        }

        [DataMember(Name = "sc", IsRequired = true, Order = 1)]
        public SyncContext SyncContext { get; set; }

        /// <summary>
        /// Gets or Sets the reference scope for local repository, stored on server
        /// </summary>
        [DataMember(Name = "scope", IsRequired = true, Order = 2)]
        public ScopeInfo ScopeInfoFromClient { get; set; }

        /// <summary>
        /// Gets or Sets the reference scope for local repository, stored on server
        /// </summary>
        [DataMember(Name = "scopeclient", IsRequired = true, Order = 3)]
        public ScopeInfoClient ScopeInfoClient { get; set; }


    }

    [DataContract(Name = "opres"), Serializable]
    public class HttpMessageOperationResponse : IScopeMessage
    {
        public HttpMessageOperationResponse() { }

        public HttpMessageOperationResponse(SyncContext context, SyncOperation syncOperation)
        {
            this.SyncContext = context ?? throw new ArgumentNullException(nameof(context));
            this.SyncOperation = syncOperation;
        }

        [DataMember(Name = "sc", IsRequired = true, Order = 1)]
        public SyncContext SyncContext { get; set; }

        [DataMember(Name = "so", IsRequired = true, Order = 2)]
        public SyncOperation SyncOperation { get; set; }
    }


    [DataContract(Name = "remotetsres"), Serializable]
    public class HttpMessageRemoteTimestampResponse : IScopeMessage
    {
        public HttpMessageRemoteTimestampResponse()
        {

        }
        public HttpMessageRemoteTimestampResponse(SyncContext context, long remoteClientTimestamp)
        {
            this.SyncContext = context ?? throw new ArgumentNullException(nameof(context));
            this.RemoteClientTimestamp = remoteClientTimestamp;
        }

        [DataMember(Name = "sc", IsRequired = true, Order = 1)]
        public SyncContext SyncContext { get; set; }

        /// <summary>
        /// The remote client timestamp generated by the server database
        /// </summary>
        [DataMember(Name = "rct", IsRequired = true, EmitDefaultValue = true, Order = 2)]
        public long RemoteClientTimestamp { get; set; }
    }


    [DataContract(Name = "remotetsreq"), Serializable]
    public class HttpMessageRemoteTimestampRequest : IScopeMessage
    {
        public HttpMessageRemoteTimestampRequest() { }

        /// <summary>
        /// Create a new message to web remote server.
        /// </summary>
        public HttpMessageRemoteTimestampRequest(SyncContext context)
        {
            this.SyncContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        [DataMember(Name = "sc", IsRequired = true, Order = 1)]
        public SyncContext SyncContext { get; set; }
    }


    [DataContract(Name = "summary"), Serializable]
    public class HttpMessageSummaryResponse : IScopeMessage
    {
        public HttpMessageSummaryResponse()
        {

        }

        public HttpMessageSummaryResponse(SyncContext context)
            => this.SyncContext = context ?? throw new ArgumentNullException(nameof(context));

        /// <summary>
        /// Gets or Sets the SyncContext
        /// </summary>
        [DataMember(Name = "sc", IsRequired = true, Order = 1)]
        public SyncContext SyncContext { get; set; }

        /// <summary>
        /// Gets or Sets the conflict resolution policy from the server
        /// </summary>

        [DataMember(Name = "bi", IsRequired = false, EmitDefaultValue = false, Order = 2)]
        public BatchInfo BatchInfo { get; set; }

        /// <summary>
        /// The remote client timestamp generated by the server database
        /// </summary>
        [DataMember(Name = "rct", IsRequired = false, EmitDefaultValue = false, Order = 3)]
        public long RemoteClientTimestamp { get; set; }


        [DataMember(Name = "step", IsRequired = true, Order = 4)]
        public HttpStep Step { get; set; }

        /// <summary>
        /// Gets or Sets the container changes when in memory requested by the client
        /// </summary>
        [DataMember(Name = "changes", IsRequired = false, EmitDefaultValue = false, Order = 5)]
        public ContainerSet Changes { get; set; }

        [DataMember(Name = "scs", IsRequired = false, EmitDefaultValue = false, Order = 6)]
        public DatabaseChangesSelected ServerChangesSelected { get; set; }

        [DataMember(Name = "cca", IsRequired = false, EmitDefaultValue = false, Order = 7)]
        public DatabaseChangesApplied ClientChangesApplied { get; set; }

        [DataMember(Name = "crp", IsRequired = false, EmitDefaultValue = false, Order = 8)]
        public ConflictResolutionPolicy ConflictResolutionPolicy { get; set; }

    }



    [DataContract(Name = "endsessionreq"), Serializable]
    public class HttpMessageEndSessionRequest : IScopeMessage
    {
        public HttpMessageEndSessionRequest()
        {

        }

        public HttpMessageEndSessionRequest(SyncContext context)
            => this.SyncContext = context ?? throw new ArgumentNullException(nameof(context));

        /// <summary>
        /// Gets or Sets the SyncContext
        /// </summary>
        [DataMember(Name = "sc", IsRequired = true, Order = 1)]
        public SyncContext SyncContext { get; set; }

        /// <summary>
        /// <summary>Gets or sets the time when a sync sessionn started.
        /// </summary>
        [DataMember(Name = "st", IsRequired = true, Order = 2)]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the time when a sync session ended.
        /// </summary>
        [DataMember(Name = "ct", IsRequired = true, Order = 3)]
        public DateTime CompleteTime { get; set; }

        /// <summary>
        /// Gets or Sets the summary of client changes that where applied on the server
        /// </summary>
        [DataMember(Name = "cas", IsRequired = false, Order = 4, EmitDefaultValue = false)]
        public DatabaseChangesApplied ChangesAppliedOnServer { get; set; }

        /// <summary>
        /// Gets or Sets the summary of server changes that where applied on the client
        /// </summary>
        [DataMember(Name = "cac", IsRequired = false, Order = 5, EmitDefaultValue = false)]
        public DatabaseChangesApplied ChangesAppliedOnClient { get; set; }

        /// <summary>
        /// Gets or Sets the summary of snapshot changes that where applied on the client
        /// </summary>
        [DataMember(Name = "scac", IsRequired = false, Order = 6, EmitDefaultValue = false)]
        public DatabaseChangesApplied SnapshotChangesAppliedOnClient { get; set; }

        /// <summary>
        /// Gets or Sets the summary of client changes to be applied on the server
        /// </summary>
        [DataMember(Name = "ccs", IsRequired = false, Order = 7, EmitDefaultValue = false)]
        public DatabaseChangesSelected ClientChangesSelected { get; set; }

        /// <summary>
        /// Gets or Sets the summary of server changes selected to be applied on the client
        /// </summary>
        [DataMember(Name = "scs", IsRequired = false, Order = 8, EmitDefaultValue = false)]
        public DatabaseChangesSelected ServerChangesSelected { get; set; }

        /// <summary>
        /// Gets or Sets the exception occured on client if any
        /// </summary>
        [DataMember(Name = "exc", IsRequired = false, Order = 9, EmitDefaultValue = false)]
        public string SyncExceptionMessage { get; set; }

    }


    [DataContract(Name = "endsessionres"), Serializable]
    public class HttpMessageEndSessionResponse : IScopeMessage
    {
        public HttpMessageEndSessionResponse() { }

        public HttpMessageEndSessionResponse(SyncContext context)
        {
            this.SyncContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        [DataMember(Name = "sc", IsRequired = true, Order = 1)]
        public SyncContext SyncContext { get; set; }
    }

}
