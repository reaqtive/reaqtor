// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

#if SUPPORT_SYS_CONFIG
using System.Configuration;
#endif

using Reaqtor.Expressions;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Exposes a set of configuration options for query engines.
    /// </summary>
    public sealed class ConfigurationOptions
    {
#if SUPPORT_SYS_CONFIG
        private const string DumpRecoveryStateBlobsKey = "DumpRecoveryStateBlobs";
        private const string DumpRecoveryStatePathKey = "DumpRecoveryStatePath";
        private const string CheckpointDegreeOfParallelismKey = "CheckpointDegreeOfParallelism";
        private const string RecoveryDegreeOfParallelismKey = "RecoveryDegreeOfParallelism";
        private const string TemplatizeExpressionsKey = "TemplatizeExpressions";
        private const string SerializationPolicyVersionKey = "SerializationPolicyVersion";
        private const string GarbageCollectionEnabledKey = "GarbageCollectionEnabled";
        private const string GarbageCollectionSweepEnabledKey = "GarbageCollectionSweepEnabled";
        private const string GarbageCollectionSweepBudgetPerCheckpointKey = "GarbageCollectionSweepBudgetPerCheckpoint";
        private const string TemplatizeRecoveredEntitiesLimitKey = "TemplatizeRecoveredEntitiesLimit";
        private const string TemplatizeRecoveredEntitiesRegexKey = "TemplatizeRecoveredEntitiesRegex";
#endif

        private bool? _dumpRecoveryStateBlobs;
        private string _dumpRecoveryStatePath;
        private int? _checkpointDegreeOfParallelism;
        private int? _recoveryDegreeOfParallelism;
        private bool? _templatizeExpressions;
        private int? _templatizeRecoveredEntitiesLimit;
        private string _templatizeRecoveredEntitiesRegex;
        private bool? _garbageCollectionEnabled;
        private bool? _garbageCollectionSweepEnabled;
        private int? _garbageCollectionSweepBudgetPerCheckpoint;

        //private volatile ISerializationPolicy _serializationPolicy;

        /// <summary>
        /// Initializes the configuration options for a query engine.
        /// </summary>
        internal ConfigurationOptions()
        {
            //var version = SerializationPolicyVersion;
            ExpressionPolicy = new ExpressionPolicy();
            Quoter = new Quoter(ExpressionPolicy);
            //_serializationPolicy = version != null
            //    ? new SerializationPolicy(_expressionPolicy, version)
            //    : QueryEngine.SerializationPolicy.Default;
        }

        /// <summary>
        /// Gets or sets the path where recovery state dumps are written to. This setting only takes effect if <see cref="DumpRecoveryStateBlobs"/> is set to <c>true</c>.
        /// </summary>
        public string DumpRecoveryStatePath
        {
            get
            {
                if (_dumpRecoveryStatePath != null)
                {
                    return _dumpRecoveryStatePath;
                }
                else
                {
#if SUPPORT_SYS_CONFIG
                    return ConfigurationManager.AppSettings[DumpRecoveryStatePathKey];
#else
                    return null;
#endif
                }
            }
            set => _dumpRecoveryStatePath = value;
        }

        /// <summary>
        /// Gets or sets the flag enabling persistence of recovery state blobs which can be used to debug recovery issues. See <see cref="DumpRecoveryStatePath"/> to configure the path to write the blobs to.
        /// </summary>
        public bool DumpRecoveryStateBlobs
        {
            get
            {
                if (_dumpRecoveryStateBlobs.HasValue)
                {
                    return _dumpRecoveryStateBlobs.Value;
                }
                else
                {
#if SUPPORT_SYS_CONFIG
                    var appSetting = ConfigurationManager.AppSettings[DumpRecoveryStateBlobsKey];
                    if (appSetting != null && bool.TryParse(appSetting, out bool result))
                    {
                        return result;
                    }
#endif
                    return false;
                }
            }
            set => _dumpRecoveryStateBlobs = value;
        }

        /// <summary>
        /// Gets or sets the degree of parallelism for checkpointing operations.
        /// </summary>
        public int CheckpointDegreeOfParallelism
        {
            get
            {
                if (_checkpointDegreeOfParallelism.HasValue)
                {
                    return _checkpointDegreeOfParallelism.Value;
                }
                else
                {
#if SUPPORT_SYS_CONFIG
                    var appSetting = ConfigurationManager.AppSettings[CheckpointDegreeOfParallelismKey];
                    if (appSetting != null && int.TryParse(appSetting, out int result))
                    {
                        return result;
                    }
#endif
                    return 1;
                }
            }
            set => _checkpointDegreeOfParallelism = value;
        }

        /// <summary>
        /// Gets or sets the degree of parallelism for recovery operations.
        /// </summary>
        public int RecoveryDegreeOfParallelism
        {
            get
            {
                if (_recoveryDegreeOfParallelism.HasValue)
                {
                    return _recoveryDegreeOfParallelism.Value;
                }
                else
                {
#if SUPPORT_SYS_CONFIG
                    var appSetting = ConfigurationManager.AppSettings[RecoveryDegreeOfParallelismKey];
                    if (appSetting != null && int.TryParse(appSetting, out int result))
                    {
                        return result;
                    }
#endif
                    return Math.Max(Environment.ProcessorCount / 2, 1);
                }
            }
            set => _recoveryDegreeOfParallelism = value;
        }

        /// <summary>
        /// Gets or sets the flag enabling templatization of expressions. By templatizing expressions, checkpoint and recovery state can be significantly reduced if many expressions have the same shape.
        /// </summary>
        public bool TemplatizeExpressions
        {
            get
            {
                if (_templatizeExpressions.HasValue)
                {
                    return _templatizeExpressions.Value;
                }
                else
                {
#if SUPPORT_SYS_CONFIG
                    var appSetting = ConfigurationManager.AppSettings[TemplatizeExpressionsKey];
                    if (appSetting != null && bool.TryParse(appSetting, out bool result))
                    {
                        return result;
                    }
#endif
                    return false;
                }
            }
            set => _templatizeExpressions = value;
        }

        /// <summary>
        /// Gets or sets the maximum number of entities that will be migrated to templatized form in a single checkpoint. If not set, the default value is 0.
        /// </summary>
        public int TemplatizeRecoveredEntitiesQuota
        {
            get
            {
                if (_templatizeRecoveredEntitiesLimit.HasValue)
                {
                    return _templatizeRecoveredEntitiesLimit.Value;
                }
                else
                {
#if SUPPORT_SYS_CONFIG
                    var appSetting = ConfigurationManager.AppSettings[TemplatizeRecoveredEntitiesLimitKey];
                    if (appSetting != null && int.TryParse(appSetting, out int result))
                    {
                        return result;
                    }
#endif
                    return 0;
                }
            }
            set => _templatizeRecoveredEntitiesLimit = value;
        }

        /// <summary>
        /// Gets or sets the regular expression applied to the entity key to determine if the entity should be migrated to templatized form. If not set, all entities can be migrated.
        /// </summary>
        public string TemplatizeRecoveredEntitiesRegex
        {
            get
            {
                if (_templatizeRecoveredEntitiesRegex != null)
                {
                    return _templatizeRecoveredEntitiesRegex;
                }
                else
                {
#if SUPPORT_SYS_CONFIG
                    return ConfigurationManager.AppSettings[TemplatizeRecoveredEntitiesRegexKey];
#else
                    return null;
#endif
                }
            }
            set => _templatizeRecoveredEntitiesRegex = value;
        }

#if DEPRECATED // NB: This was removed in favor of specifying a serialization policy in the constructor.
        /// <summary>
        /// Gets or sets the serialization policy version.
        /// </summary>
        public Version SerializationPolicyVersion
        {
            get
            {
#if SUPPORT_SYS_CONFIG
                var appSetting = ConfigurationManager.AppSettings[SerializationPolicyVersionKey];
                if (appSetting != null)
                {
                    return Version.Parse(appSetting);
                }
#endif
                return null;
            }
            set
            {
                _serializationPolicy = value != null 
                    ? new SerializationPolicy(_expressionPolicy, value) 
                    : QueryEngine.SerializationPolicy.Default;
            }
        }

        /// <summary>
        /// Gets the serialization policy for the query engine.
        /// </summary>
        internal ISerializationPolicy SerializationPolicy => _serializationPolicy;
#endif

        /// <summary>
        /// Gets the quoter used to quote expressions in the query engine.
        /// </summary>
        internal Quoter Quoter { get; private set; }

        /// <summary>
        /// Gets or sets the flag enabling garbage collection of orphaned artifacts.
        /// This flag does not enable sweeping by default. See <see cref="GarbageCollectionSweepEnabled"/> to configure sweeping of orphaned artifacts.
        /// </summary>
        public bool GarbageCollectionEnabled
        {
            get
            {
                if (_garbageCollectionEnabled != null)
                {
                    return _garbageCollectionEnabled.Value;
                }
                else
                {
#if SUPPORT_SYS_CONFIG
                    var appSetting = ConfigurationManager.AppSettings[GarbageCollectionEnabledKey];
                    if (appSetting != null && bool.TryParse(appSetting, out bool result))
                    {
                        return result;
                    }
#endif
                    return false;
                }
            }
            set => _garbageCollectionEnabled = value;
        }

        /// <summary>
        /// Gets or sets the flag enabling garbage collection of orphaned artifacts by undefining these during the sweep phase.
        /// For this flag to take effect, garbage collection has to be enabled. See <see cref="GarbageCollectionEnabled"/> to turn on the garbage collector.
        /// </summary>
        public bool GarbageCollectionSweepEnabled
        {
            get
            {
                if (_garbageCollectionSweepEnabled != null)
                {
                    return _garbageCollectionSweepEnabled.Value;
                }
                else
                {
#if SUPPORT_SYS_CONFIG
                    var appSetting = ConfigurationManager.AppSettings[GarbageCollectionSweepEnabledKey];
                    if (appSetting != null && bool.TryParse(appSetting, out bool result))
                    {
                        return result;
                    }
#endif
                    return false;
                }
            }
            set => _garbageCollectionSweepEnabled = value;
        }

        /// <summary>
        /// Gets or sets the maximum number of sweep (delete) operations the garbage collector is allowed to commit per checkpoint.
        /// For this flag to take effect, garbage collection and sweeping have to be enabled.
        /// See <see cref="GarbageCollectionEnabled"/> to turn on the garbage collector.
        /// See <see cref="GarbageCollectionSweepEnabled"/> to turn on sweeping.
        /// </summary>
        public int GarbageCollectionSweepBudgetPerCheckpoint
        {
            get
            {
                if (_garbageCollectionSweepBudgetPerCheckpoint != null)
                {
                    return _garbageCollectionSweepBudgetPerCheckpoint.Value;
                }
                else
                {
#if SUPPORT_SYS_CONFIG
                    var appSetting = ConfigurationManager.AppSettings[GarbageCollectionSweepBudgetPerCheckpointKey];
                    if (appSetting != null && int.TryParse(appSetting, out int result))
                    {
                        return result;
                    }
#endif
                    return 0;
                }
            }
            set => _garbageCollectionSweepBudgetPerCheckpoint = value;
        }

        /// <summary>
        /// Gets the expression options for the query engine, including options related to caching of compiled delegates for 
        /// evaluation and caching of expressions for compact storage in memory.
        /// </summary>
        public ExpressionPolicy ExpressionPolicy { get; private set; }

        /// <summary>
        /// Gets the foreign function binder used to resolve foreign functions during expression binding.
        /// </summary>
        public Func<string, Expression> ForeignFunctionBinder
        {
            get; set;
        }

        /// <summary>
        /// Clears the configuration options set 
        /// </summary>
        public void Clear()
        {
            _dumpRecoveryStateBlobs = null;
            _dumpRecoveryStatePath = null;
            _checkpointDegreeOfParallelism = null;
            _recoveryDegreeOfParallelism = null;
            _templatizeExpressions = null;
            _garbageCollectionEnabled = null;
            _garbageCollectionSweepEnabled = null;

#if DEPRECATED
            // Needed to ensure side-effect of resetting the serialization policy instance
            SerializationPolicyVersion = null;
#endif

            ForeignFunctionBinder = null;
        }
    }
}
