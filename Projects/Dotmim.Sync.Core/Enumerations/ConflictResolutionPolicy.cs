﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dotmim.Sync.Enumerations
{
    /// <summary>
    /// Represents the options for the conflict resolution policy to use for synchronization.
    /// Used in the configuration class
    /// </summary>
    public enum ConflictResolutionPolicy
    {
        /// <summary>
        /// Indicates that the change on the server wins in case of a conflict.
        /// </summary>
        ServerWins,

        /// <summary>
        /// Indicates that the change sent by the client wins in case of a conflict.
        /// </summary>
        ClientWins,

        /// <summary>
        /// Changes from both server and client version were merged
        /// </summary>
        Merge
    }
}
