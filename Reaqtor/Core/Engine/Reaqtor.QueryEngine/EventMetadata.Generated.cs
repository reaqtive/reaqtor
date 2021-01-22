// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

// <WARNING>
//   This file was auto-generated on 01/15/2021 09:32:45.
//   To make changes, edit the .tt file.
// </WARNING>

namespace Reaqtor.QueryEngine
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Diagnostics;
    using System.Diagnostics.Tracing;

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class Tracing
    {
        public static void Checkpoint_ContinueScheduler_Unloaded(this TraceSource source, System.Uri queryEngineId, System.String caller)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceCheckpoint_ContinueScheduler_Unloaded;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = caller;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 1, "Scheduler of query engine '{0}' wasn't resumed from {1} because an unload operation is pending.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Checkpoint_ContinueScheduler_Unloaded(arg0, arg1);
                }
            }
        }

        public static void Checkpoint_DeleteCompleted(this TraceSource source, System.Uri queryEngineId, System.String category, System.Int32 count)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceCheckpoint_DeleteCompleted;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = category;
                var arg2 = count;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 2, "Query engine '{0}' finished deleting items in category '{1}'. Total = {2}", arg0, arg1, arg2);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Checkpoint_DeleteCompleted(arg0, arg1, arg2);
                }
            }
        }

        public static void Checkpoint_DeleteStarted(this TraceSource source, System.Uri queryEngineId, System.String category)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceCheckpoint_DeleteStarted;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = category;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 3, "Query engine '{0}' deleting items in category '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Checkpoint_DeleteStarted(arg0, arg1);
                }
            }
        }

        public static void Checkpoint_SavingDefinitionsFailure(this TraceSource source, System.Uri queryEngineId, System.String category, System.String key, System.String errorMsg)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceCheckpoint_SavingDefinitionsFailure;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = category;
                var arg2 = key;
                var arg3 = errorMsg;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Error, 4, "Query engine '{0}' failed to save entity in category '{1}' with key '{2}'. Error: {3}", arg0, arg1, arg2, arg3);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Checkpoint_SavingDefinitionsFailure(arg0, arg1, arg2, arg3);
                }
            }
        }

        public static void Checkpoint_SavingDefinitionsCompleted(this TraceSource source, System.Uri queryEngineId, System.String category, System.Int32 total, System.Int32 skipped, System.Int32 failed)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceCheckpoint_SavingDefinitionsCompleted;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = category;
                var arg2 = total;
                var arg3 = skipped;
                var arg4 = failed;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 5, "Query engine '{0}' finished saving definitions in category '{1}'. Total = {2}, Skipped = {3}, Failed = {4}", arg0, arg1, arg2, arg3, arg4);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Checkpoint_SavingDefinitionsCompleted(arg0, arg1, arg2, arg3, arg4);
                }
            }
        }

        public static void Checkpoint_SavingDefinitionsStarted(this TraceSource source, System.Uri queryEngineId, System.String category)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceCheckpoint_SavingDefinitionsStarted;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = category;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 6, "Query engine '{0}' saving definitions in category '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Checkpoint_SavingDefinitionsStarted(arg0, arg1);
                }
            }
        }

        public static void Checkpoint_SavingStateFailure(this TraceSource source, System.Uri queryEngineId, System.String category, System.String key, System.String errorMsg)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceCheckpoint_SavingStateFailure;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = category;
                var arg2 = key;
                var arg3 = errorMsg;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Error, 7, "Query engine '{0}' failed to save entity runtime state in category '{1}' with key '{2}'. Error: {3}", arg0, arg1, arg2, arg3);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Checkpoint_SavingStateFailure(arg0, arg1, arg2, arg3);
                }
            }
        }

        public static void Checkpoint_SavingStateCompleted(this TraceSource source, System.Uri queryEngineId, System.String category, System.Int32 total, System.Int32 skipped, System.Int32 failed)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceCheckpoint_SavingStateCompleted;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = category;
                var arg2 = total;
                var arg3 = skipped;
                var arg4 = failed;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 8, "Query engine '{0}' finished saving runtime state in category '{1}'. Total = {2}, Skipped = {3}, Failed = {4}", arg0, arg1, arg2, arg3, arg4);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Checkpoint_SavingStateCompleted(arg0, arg1, arg2, arg3, arg4);
                }
            }
        }

        public static void Checkpoint_SavingStateStarted(this TraceSource source, System.Uri queryEngineId, System.String category)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceCheckpoint_SavingStateStarted;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = category;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 9, "Query engine '{0}' saving runtime state in category '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Checkpoint_SavingStateStarted(arg0, arg1);
                }
            }
        }

        public static void FailSafe_Exception(this TraceSource source, System.String method, System.Exception exception)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceFailSafe_Exception;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = method;
                var arg1 = exception.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Error, 10, "Action invoked by '{0}' failed with exception '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.FailSafe_Exception(arg0, arg1);
                }
            }
        }

        public static void LazyStream_Created(this TraceSource source, System.String key, System.Uri queryEngineId, System.String expression)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceLazyStream_Created;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = key;
                var arg1 = queryEngineId.ToTraceString();
                var arg2 = expression;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 11, "Stream for key '{0}' does not exist on query engine '{1}'. Creating stream from remote definition '{2}'.", arg0, arg1, arg2);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.LazyStream_Created(arg0, arg1, arg2);
                }
            }
        }

        public static void LazyStream_TypeRewrite(this TraceSource source, System.String key, System.Type streamType, System.Type newStreamType, System.Uri queryEngineId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceLazyStream_TypeRewrite;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = key;
                var arg1 = streamType.ToTraceString();
                var arg2 = newStreamType.ToTraceString();
                var arg3 = queryEngineId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 12, "Rewriting stream definition for '{0}' from '{1}' to '{2}' on query engine '{3}'.", arg0, arg1, arg2, arg3);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.LazyStream_TypeRewrite(arg0, arg1, arg2, arg3);
                }
            }
        }

        public static void Registry_AddEntityPlaceholder(this TraceSource source, System.Uri queryEngineId, System.String kind, System.String key)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceRegistry_AddEntityPlaceholder;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = kind;
                var arg2 = key;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 13, "Added invalid entity entry to query engine '{0}' registry for '{1}' '{2}'.", arg0, arg1, arg2);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Registry_AddEntityPlaceholder(arg0, arg1, arg2);
                }
            }
        }

        public static void Registry_RemoveEntity(this TraceSource source, System.String kind, System.String key, System.Uri queryEngineId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceRegistry_RemoveEntity;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = kind;
                var arg1 = key;
                var arg2 = queryEngineId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 14, "Removed '{0}' '{1}' from query engine '{2}'.", arg0, arg1, arg2);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Registry_RemoveEntity(arg0, arg1, arg2);
                }
            }
        }

        public static void RegistryGarbageCollection_LiveDependencies(this TraceSource source, System.Uri queryEngineId, System.Int32 count, System.Int32 markTime)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceRegistryGarbageCollection_LiveDependencies;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = count;
                var arg2 = markTime;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 15, "Garbage collector on query engine '{0}' has found {1} live dependencies. Mark time = {2}ms", arg0, arg1, arg2);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.RegistryGarbageCollection_LiveDependencies(arg0, arg1, arg2);
                }
            }
        }

        public static void RegistryGarbageCollection_NotEnabled(this TraceSource source, System.Uri queryEngineId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceRegistryGarbageCollection_NotEnabled;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 16, "Garbage collector on query engine '{0}' is not enabled.", arg0);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.RegistryGarbageCollection_NotEnabled(arg0);
                }
            }
        }

        public static void RegistryGarbageCollection_Started(this TraceSource source, System.Uri queryEngineId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceRegistryGarbageCollection_Started;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 17, "Garbage collector on query engine '{0}' has started.", arg0);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.RegistryGarbageCollection_Started(arg0);
                }
            }
        }

        public static void RegistryGarbageCollection_SweepCompleted(this TraceSource source, System.Uri queryEngineId, System.Int32 count, System.Int32 iterations, System.Int32 duration)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceRegistryGarbageCollection_SweepCompleted;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = count;
                var arg2 = iterations;
                var arg3 = duration;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 18, "Garbage collector on query engine '{0}' has no further sweep work. Performed {1} sweep operations over {2} iterations. Total sweep time = {3}ms", arg0, arg1, arg2, arg3);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.RegistryGarbageCollection_SweepCompleted(arg0, arg1, arg2, arg3);
                }
            }
        }

        public static void RegistryGarbageCollection_SweepDisabled(this TraceSource source, System.Uri queryEngineId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceRegistryGarbageCollection_SweepDisabled;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 19, "Garbage collector on query engine '{0}' is running without sweeping enabled. No further actions will be taken.", arg0);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.RegistryGarbageCollection_SweepDisabled(arg0);
                }
            }
        }

        public static void RegistryGarbageCollection_SweepStarted(this TraceSource source, System.Uri queryEngineId, System.Int32 count)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceRegistryGarbageCollection_SweepStarted;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = count;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 20, "Garbage collector on query engine '{0}' is starting the sweep phase. Remaining entities = {1}.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.RegistryGarbageCollection_SweepStarted(arg0, arg1);
                }
            }
        }

        public static void RegistryGarbageCollection_SweepStopped(this TraceSource source, System.Uri queryEngineId, System.Int32 count, System.Int32 remaining, System.Int32 duration)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceRegistryGarbageCollection_SweepStopped;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = count;
                var arg2 = remaining;
                var arg3 = duration;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 21, "Garbage collector on query engine '{0}' has finished sweeping {1} artifacts. Remaining entities = {2}. Sweep time = {3}ms", arg0, arg1, arg2, arg3);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.RegistryGarbageCollection_SweepStopped(arg0, arg1, arg2, arg3);
                }
            }
        }

        public static void RegistryGarbageCollection_UnreachableObservables(this TraceSource source, System.Uri queryEngineId, System.Int32 count, System.Int32 total, System.Int32 scanTime)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceRegistryGarbageCollection_UnreachableObservables;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = count;
                var arg2 = total;
                var arg3 = scanTime;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 22, "Garbage collector on query engine '{0}' has found {1} unreachable observable definitions out of a total {2}. Scan time = {3}ms", arg0, arg1, arg2, arg3);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.RegistryGarbageCollection_UnreachableObservables(arg0, arg1, arg2, arg3);
                }
            }
        }

        public static void StateOperation_Error(this TraceSource source, Reaqtor.QueryEngine.TraceVerb verb, Reaqtor.QueryEngine.TraceNoun noun, System.Uri uri, System.Exception error)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceStateOperation_Error;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = verb.ToTraceString();
                var arg1 = noun.ToTraceString();
                var arg2 = uri.ToTraceString();
                var arg3 = error.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Error, 23, "Failed to {0} {1} '{2}'. Error: {3}", arg0, arg1, arg2, arg3);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.StateOperation_Error(arg0, arg1, arg2, arg3);
                }
            }
        }

        public static void StateOperation_Executed(this TraceSource source, Reaqtor.QueryEngine.TraceVerb verb, Reaqtor.QueryEngine.TraceNoun noun, System.Uri entityId, System.Int64 duration)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceStateOperation_Executed;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = verb.ToTraceString();
                var arg1 = noun.ToTraceString();
                var arg2 = entityId.ToTraceString();
                var arg3 = duration;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Verbose, 24, "Executed {0} {1} '{2}' in {3} ms.", arg0, arg1, arg2, arg3);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.StateOperation_Executed(arg0, arg1, arg2, arg3);
                }
            }
        }

        public static void StateOperation_Warning(this TraceSource source, Reaqtor.QueryEngine.TraceVerb verb, Reaqtor.QueryEngine.TraceNoun noun, System.Uri entityId, System.Exception error)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceStateOperation_Warning;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = verb.ToTraceString();
                var arg1 = noun.ToTraceString();
                var arg2 = entityId.ToTraceString();
                var arg3 = error.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Warning, 25, "Failed to {0} {1} '{2}'. Error: {3}", arg0, arg1, arg2, arg3);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.StateOperation_Warning(arg0, arg1, arg2, arg3);
                }
            }
        }

        public static void TemplateMigration_Completed(this TraceSource source, System.Uri queryEngineId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceTemplateMigration_Completed;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 26, "Template migration task for query engine '{0}' has completed.", arg0);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.TemplateMigration_Completed(arg0);
                }
            }
        }

        public static void TemplateMigration_Execute(this TraceSource source, System.Uri queryEngineId, System.Uri entityId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceTemplateMigration_Execute;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = entityId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 27, "Template migration task for query engine '{0}' templatized expression for entity '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.TemplateMigration_Execute(arg0, arg1);
                }
            }
        }

        public static void TemplateMigration_RegexInvalid(this TraceSource source, System.Uri queryEngineId, System.String regex)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceTemplateMigration_RegexInvalid;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = regex;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Error, 28, "Template migration task for query engine '{0}' could not parse regular expression '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.TemplateMigration_RegexInvalid(arg0, arg1);
                }
            }
        }

        public static void TemplateMigration_ResetQuota(this TraceSource source, System.Uri queryEngineId, System.Int32 quota, System.Int32 count)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceTemplateMigration_ResetQuota;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = quota;
                var arg2 = count;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 29, "Template migration task for query engine '{0}' resetting quota to '{1}' after templatizing '{2}' entities.", arg0, arg1, arg2);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.TemplateMigration_ResetQuota(arg0, arg1, arg2);
                }
            }
        }

        public static void TemplateMigration_Started(this TraceSource source, System.Uri queryEngineId, System.String regex)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceTemplateMigration_Started;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = regex;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 30, "Template migration task for query engine '{0}' has started for entity keys matching '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.TemplateMigration_Started(arg0, arg1);
                }
            }
        }

        public static void BlobLogs_Created(this TraceSource source, System.String path, System.Uri queryEngineId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceBlobLogs_Created;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = path;
                var arg1 = queryEngineId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 31, "Recovery state blob log created at '{0}' on query engine '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.BlobLogs_Created(arg0, arg1);
                }
            }
        }

        public static void BlobLogs_CreateFailed(this TraceSource source, System.String path, System.Uri queryEngineId, System.Exception error)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceBlobLogs_CreateFailed;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = path;
                var arg1 = queryEngineId.ToTraceString();
                var arg2 = error.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Warning, 32, "Recovery state blob log could not be created at '{0}' on query engine '{1}'. Error = {2}", arg0, arg1, arg2);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.BlobLogs_CreateFailed(arg0, arg1, arg2);
                }
            }
        }

        public static void Canary_NotRecovered_Observable(this TraceSource source, System.Uri canaryId, System.Uri queryEngineId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceCanary_NotRecovered_Observable;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = canaryId.ToTraceString();
                var arg1 = queryEngineId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Warning, 33, "Canary observable '{0}' not recovered for query engine '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Canary_NotRecovered_Observable(arg0, arg1);
                }
            }
        }

        public static void Canary_NotRecovered_Observer(this TraceSource source, System.Uri canaryId, System.Uri queryEngineId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceCanary_NotRecovered_Observer;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = canaryId.ToTraceString();
                var arg1 = queryEngineId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Warning, 34, "Canary observer '{0}' not recovered for query engine '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Canary_NotRecovered_Observer(arg0, arg1);
                }
            }
        }

        public static void Canary_NotRecovered_Subscription(this TraceSource source, System.Uri canaryId, System.Uri queryEngineId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceCanary_NotRecovered_Subscription;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = canaryId.ToTraceString();
                var arg1 = queryEngineId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Warning, 35, "Canary subscription '{0}' not recovered for query engine '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Canary_NotRecovered_Subscription(arg0, arg1);
                }
            }
        }

        public static void Canary_Created_Observable(this TraceSource source, System.Uri canaryId, System.Uri queryEngineId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceCanary_Created_Observable;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = canaryId.ToTraceString();
                var arg1 = queryEngineId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 36, "Canary observable '{0}' defined on query engine '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Canary_Created_Observable(arg0, arg1);
                }
            }
        }

        public static void Canary_Created_Observer(this TraceSource source, System.Uri canaryId, System.Uri queryEngineId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceCanary_Created_Observer;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = canaryId.ToTraceString();
                var arg1 = queryEngineId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 37, "Canary observer '{0}' defined on query engine '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Canary_Created_Observer(arg0, arg1);
                }
            }
        }

        public static void Canary_Created_Subscription(this TraceSource source, System.Uri canaryId, System.Uri queryEngineId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceCanary_Created_Subscription;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = canaryId.ToTraceString();
                var arg1 = queryEngineId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 38, "Canary subscription '{0}' defined on query engine '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Canary_Created_Subscription(arg0, arg1);
                }
            }
        }

        public static void Mitigation_Execute(this TraceSource source, System.Uri entityId, Reaqtor.QueryEngine.Events.ReactiveEntityRecoveryFailureMitigation mitigation)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceMitigation_Execute;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = entityId.ToTraceString();
                var arg1 = mitigation.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 39, "Recovery of '{0}' failed. Executing mitigation '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Mitigation_Execute(arg0, arg1);
                }
            }
        }

        public static void Mitigation_NotAccepted(this TraceSource source, Reaqtor.QueryEngine.Events.ReactiveEntityRecoveryFailureMitigation mitigation, System.Uri entityId, System.Exception error)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceMitigation_NotAccepted;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = mitigation.ToTraceString();
                var arg1 = entityId.ToTraceString();
                var arg2 = error.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 40, "Mitigation '{0}' for recovery failure of '{1}' was not accepted. Exception: '{2}'.", arg0, arg1, arg2);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Mitigation_NotAccepted(arg0, arg1, arg2);
                }
            }
        }

        public static void Mitigation_Failure(this TraceSource source, Reaqtor.QueryEngine.Events.ReactiveEntityRecoveryFailureMitigation mitigation, System.Uri entityId, System.Exception error)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceMitigation_Failure;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = mitigation.ToTraceString();
                var arg1 = entityId.ToTraceString();
                var arg2 = error.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 41, "Mitigation '{0}' for recovery failure of '{1}' threw exception: '{2}'.", arg0, arg1, arg2);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Mitigation_Failure(arg0, arg1, arg2);
                }
            }
        }

        public static void Recovery_LoadingDefinitionsFailure(this TraceSource source, System.Uri queryEngineId, System.String category, System.String key, System.String errorMsg)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceRecovery_LoadingDefinitionsFailure;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = category;
                var arg2 = key;
                var arg3 = errorMsg;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Error, 42, "Query engine '{0}' failed to load entity in category '{1}' with key '{2}'. Error: {3}", arg0, arg1, arg2, arg3);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Recovery_LoadingDefinitionsFailure(arg0, arg1, arg2, arg3);
                }
            }
        }

        public static void Recovery_LoadingDefinitionsCompleted(this TraceSource source, System.Uri queryEngineId, System.String category, System.Int32 total, System.Int32 failed)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceRecovery_LoadingDefinitionsCompleted;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = category;
                var arg2 = total;
                var arg3 = failed;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 43, "Query engine '{0}' finished loading definitions in category '{1}'. Total = {2}, Failed = {3}", arg0, arg1, arg2, arg3);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Recovery_LoadingDefinitionsCompleted(arg0, arg1, arg2, arg3);
                }
            }
        }

        public static void Recovery_LoadingDefinitionsStarted(this TraceSource source, System.Uri queryEngineId, System.String category)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceRecovery_LoadingDefinitionsStarted;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = category;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 44, "Query engine '{0}' loading definitions in category '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Recovery_LoadingDefinitionsStarted(arg0, arg1);
                }
            }
        }

        public static void Recovery_LoadingStateFailure(this TraceSource source, System.Uri queryEngineId, System.String category, System.String stateCategory, System.String key, System.String errorMsg)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceRecovery_LoadingStateFailure;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = category;
                var arg2 = stateCategory;
                var arg3 = key;
                var arg4 = errorMsg;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Error, 45, "Query engine '{0}' failed to load entity state in category '{1}'/'{2}' with key '{3}'. Error: {4}", arg0, arg1, arg2, arg3, arg4);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Recovery_LoadingStateFailure(arg0, arg1, arg2, arg3, arg4);
                }
            }
        }

        public static void Recovery_LoadCompleted(this TraceSource source, System.Uri queryEngineId, System.Int64 duration)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceRecovery_LoadCompleted;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = duration;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 46, "Query engine '{0}' finished loading artifacts. Elapsed time = {1}ms", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Recovery_LoadCompleted(arg0, arg1);
                }
            }
        }

        public static void Recovery_LoadStarted(this TraceSource source, System.Uri queryEngineId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceRecovery_LoadStarted;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 47, "Query engine '{0}' loading artifacts.", arg0);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Recovery_LoadStarted(arg0);
                }
            }
        }

        public static void Recovery_LoadSubjectWithoutState(this TraceSource source, System.Uri subjectId, System.Uri queryEngineId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceRecovery_LoadSubjectWithoutState;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = subjectId.ToTraceString();
                var arg1 = queryEngineId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 48, "Recovering subject '{0}' without state on query engine '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Recovery_LoadSubjectWithoutState(arg0, arg1);
                }
            }
        }

        public static void Recovery_LoadSubscriptionWithoutState(this TraceSource source, System.Uri subjectId, System.Uri queryEngineId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceRecovery_LoadSubscriptionWithoutState;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = subjectId.ToTraceString();
                var arg1 = queryEngineId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 49, "Recovering subscription '{0}' without state on query engine '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Recovery_LoadSubscriptionWithoutState(arg0, arg1);
                }
            }
        }

        public static void Recovery_StartCompleted(this TraceSource source, System.Uri queryEngineId, System.Int64 duration)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceRecovery_StartCompleted;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = duration;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 50, "Query engine '{0}' finished starting artifacts. Elapsed time = {1}ms", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Recovery_StartCompleted(arg0, arg1);
                }
            }
        }

        public static void Recovery_StartReliableSubscriptions(this TraceSource source, System.Uri queryEngineId, System.Int32 count)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceRecovery_StartReliableSubscriptions;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = count;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 51, "Query engine '{0}' starting {1} reliable subscriptions.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Recovery_StartReliableSubscriptions(arg0, arg1);
                }
            }
        }

        public static void Recovery_StartStarted(this TraceSource source, System.Uri queryEngineId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceRecovery_StartStarted;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 52, "Query engine '{0}' loading artifacts.", arg0);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Recovery_StartStarted(arg0);
                }
            }
        }

        public static void Recovery_StartSubjects(this TraceSource source, System.Uri queryEngineId, System.Int32 count)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceRecovery_StartSubjects;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = count;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 53, "Query engine '{0}' starting {1} subjects.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Recovery_StartSubjects(arg0, arg1);
                }
            }
        }

        public static void Recovery_StartSubscriptions(this TraceSource source, System.Uri queryEngineId, System.Int32 count)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceRecovery_StartSubscriptions;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = count;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 54, "Query engine '{0}' starting {1} subscriptions.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Recovery_StartSubscriptions(arg0, arg1);
                }
            }
        }

        public static void Recovery_Summary(this TraceSource source, System.String category, System.Uri queryEngineId, System.Collections.Generic.IEnumerable<Reaqtor.Metadata.IReactiveResource> summarySource, Func<System.Collections.Generic.IEnumerable<Reaqtor.Metadata.IReactiveResource>, string> summaryToString)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceRecovery_Summary;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var summary = summaryToString(summarySource);
                var arg0 = category;
                var arg1 = queryEngineId.ToTraceString();
                var arg2 = summary;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 55, "Recovery summary for {0} on query engine {1} - {2}", arg0, arg1, arg2);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Recovery_Summary(arg0, arg1, arg2);
                }
            }
        }

        public static void ReliableSubscriptionInput_OnStart(this TraceSource source, System.Uri instanceId, System.Int64 sequenceId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceReliableSubscriptionInput_OnStart;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = instanceId.ToTraceString();
                var arg1 = sequenceId;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 56, "Reliable subscription input for '{0}' sent Start({1}).", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.ReliableSubscriptionInput_OnStart(arg0, arg1);
                }
            }
        }

        public static void ReliableSubscriptionInput_OnStateSaved(this TraceSource source, System.Uri instanceId, System.Int64 sequenceId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceReliableSubscriptionInput_OnStateSaved;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = instanceId.ToTraceString();
                var arg1 = sequenceId;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 57, "Reliable subscription input for '{0}' sent AcknowledgeRange({1}).", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.ReliableSubscriptionInput_OnStateSaved(arg0, arg1);
                }
            }
        }

        public static void CheckpointableStateManager_TransitionCompleted(this TraceSource source, System.Uri queryEngineId, System.String caller, Reaqtor.QueryEngine.QueryEngineStatus oldStatus, Reaqtor.QueryEngine.QueryEngineStatus newStatus)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceCheckpointableStateManager_TransitionCompleted;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = caller;
                var arg2 = oldStatus.ToTraceString();
                var arg3 = newStatus.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 58, "Query engine '{0}' finished '{1}' request. Transitioning from state '{2}' to state '{3}'.", arg0, arg1, arg2, arg3);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.CheckpointableStateManager_TransitionCompleted(arg0, arg1, arg2, arg3);
                }
            }
        }

        public static void CheckpointableStateManager_TransitionStarted(this TraceSource source, System.Uri queryEngineId, System.String caller, Reaqtor.QueryEngine.QueryEngineStatus oldStatus, Reaqtor.QueryEngine.QueryEngineStatus newStatus)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceCheckpointableStateManager_TransitionStarted;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = caller;
                var arg2 = oldStatus.ToTraceString();
                var arg3 = newStatus.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 59, "Query engine '{0}' processing '{1}' request. Transitioning from state '{2}' to state '{3}'.", arg0, arg1, arg2, arg3);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.CheckpointableStateManager_TransitionStarted(arg0, arg1, arg2, arg3);
                }
            }
        }

        public static void Bridge_CreatingUpstreamObservable(this TraceSource source, System.Uri observableId, System.Uri bridgeId, System.Linq.Expressions.Expression expressionSource, Func<System.Linq.Expressions.Expression, string> expressionToString)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceBridge_CreatingUpstreamObservable;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var expression = expressionToString(expressionSource);
                var arg0 = observableId.ToTraceString();
                var arg1 = bridgeId.ToTraceString();
                var arg2 = expression;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 60, "Defining upstream observable '{0}' for bridge '{1}'. Definition = '{2}'.", arg0, arg1, arg2);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Bridge_CreatingUpstreamObservable(arg0, arg1, arg2);
                }
            }
        }

        public static void Bridge_CreatingUpstreamSubscription(this TraceSource source, System.Uri subscriptionId, System.Uri bridgeId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceBridge_CreatingUpstreamSubscription;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = subscriptionId.ToTraceString();
                var arg1 = bridgeId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 61, "Creating upstream subscription '{0}' for bridge '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Bridge_CreatingUpstreamSubscription(arg0, arg1);
                }
            }
        }

        public static void Bridge_DisposingUpstreamObservable(this TraceSource source, System.Uri observableId, System.Uri bridgeId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceBridge_DisposingUpstreamObservable;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = observableId.ToTraceString();
                var arg1 = bridgeId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 62, "Disposing upstream observable '{0}' for bridge '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Bridge_DisposingUpstreamObservable(arg0, arg1);
                }
            }
        }

        public static void Bridge_DisposingUpstreamSubscription(this TraceSource source, System.Uri subscriptionId, System.Uri bridgeId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceBridge_DisposingUpstreamSubscription;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = subscriptionId.ToTraceString();
                var arg1 = bridgeId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 63, "Disposing upstream subscription '{0}' for bridge '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Bridge_DisposingUpstreamSubscription(arg0, arg1);
                }
            }
        }

        public static void Transaction_Log_Replay(this TraceSource source, System.Uri queryEngineId, String operation, String category, System.Uri entityId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceTransaction_Log_Replay;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = operation;
                var arg2 = category;
                var arg3 = entityId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 64, "Recovery of query engine '{0}' induced a replay of a '{1}' operation for a(n) '{2}' artifact with id '{3}' from the transaction log.", arg0, arg1, arg2, arg3);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Transaction_Log_Replay(arg0, arg1, arg2, arg3);
                }
            }
        }

        public static void Transaction_Log_Replay_Failure(this TraceSource source, System.Uri queryEngineId, String operation, String category, System.Uri entityId, System.Exception exception)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceTransaction_Log_Replay_Failure;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = operation;
                var arg2 = category;
                var arg3 = entityId.ToTraceString();
                var arg4 = exception.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Warning, 65, "Replay of a '{1}' operation for a(n) '{2}' artifact with id '{3}' from the transaction log on query engine '{0}' failed. {4}", arg0, arg1, arg2, arg3, arg4);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Transaction_Log_Replay_Failure(arg0, arg1, arg2, arg3, arg4);
                }
            }
        }

        public static void Transaction_Log_Garbage_Collection(this TraceSource source, System.Uri queryEngineId, Int64 count, Int64 currentVersion, Int64 activeCount)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceTransaction_Log_Garbage_Collection;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = count;
                var arg2 = currentVersion;
                var arg3 = activeCount;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 66, "Reclaimed '{1}' old versions of the transaction log on query engine '{0}'. Current version: '{2}', Active count: '{3}'.", arg0, arg1, arg2, arg3);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Transaction_Log_Garbage_Collection(arg0, arg1, arg2, arg3);
                }
            }
        }

        public static void Transaction_Log_Coalesce(this TraceSource source, System.Uri queryEngineId, String entityId, String coalescesStateSoFar, String nextState)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceTransaction_Log_Coalesce;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = entityId;
                var arg2 = coalescesStateSoFar;
                var arg3 = nextState;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 67, "Multiple occurences of artifact with id '{1}' appeared in the transaction log on query engine '{0}'. Coalesced state so far: '{2}', next state '{3}'.", arg0, arg1, arg2, arg3);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Transaction_Log_Coalesce(arg0, arg1, arg2, arg3);
                }
            }
        }

        public static void Transaction_Log_Initialization(this TraceSource source, System.Uri queryEngineId, Int64 latest, Int64 activeCount, Int64 heldCount)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceTransaction_Log_Initialization;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = latest;
                var arg2 = activeCount;
                var arg3 = heldCount;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 68, "Transaction log on query engine '{0}' initializing with latest version '{1}', active count '{2}', and held count '{3}'.", arg0, arg1, arg2, arg3);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Transaction_Log_Initialization(arg0, arg1, arg2, arg3);
                }
            }
        }

        public static void Transaction_Log_Snapshot(this TraceSource source, System.Uri queryEngineId, Int64 latest, Int64 activeCount, Int64 heldCount)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceTransaction_Log_Snapshot;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = latest;
                var arg2 = activeCount;
                var arg3 = heldCount;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 69, "Transaction log on query engine '{0}' shapshotted with latest version '{1}', active count '{2}', and held count '{3}'.", arg0, arg1, arg2, arg3);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Transaction_Log_Snapshot(arg0, arg1, arg2, arg3);
                }
            }
        }

        public static void Transaction_Log_Lost_Reference(this TraceSource source, System.Uri queryEngineId, Int64 latest, Int64 activeCount, Int64 heldCount)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceTransaction_Log_Lost_Reference;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = latest;
                var arg2 = activeCount;
                var arg3 = heldCount;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 70, "Transaction log on query engine '{0}' unreferenced old version. Current latest version '{1}', active count '{2}', and held count '{3}'.", arg0, arg1, arg2, arg3);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Transaction_Log_Lost_Reference(arg0, arg1, arg2, arg3);
                }
            }
        }

        public static void Transaction_Log_Garbage_Collection_Start(this TraceSource source, System.Uri queryEngineId, Int64 latest, Int64 activeCount, Int64 heldCount)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceTransaction_Log_Garbage_Collection_Start;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = latest;
                var arg2 = activeCount;
                var arg3 = heldCount;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 71, "Transaction log on query engine '{0}' starting garbage collection. Current latest version '{1}', active count '{2}', and held count '{3}'.", arg0, arg1, arg2, arg3);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Transaction_Log_Garbage_Collection_Start(arg0, arg1, arg2, arg3);
                }
            }
        }

        public static void Transaction_Log_Garbage_Collection_End(this TraceSource source, System.Uri queryEngineId, Int64 latest, Int64 activeCount, Int64 heldCount)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceTransaction_Log_Garbage_Collection_End;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = latest;
                var arg2 = activeCount;
                var arg3 = heldCount;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 72, "Transaction log on query engine '{0}' finished garbage collection. Current latest version '{1}', active count '{2}', and held count '{3}'.", arg0, arg1, arg2, arg3);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Transaction_Log_Garbage_Collection_End(arg0, arg1, arg2, arg3);
                }
            }
        }

        public static void Transaction_Log_Garbage_Collection_Failed(this TraceSource source, System.Uri queryEngineId, System.Exception exception)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceTransaction_Log_Garbage_Collection_Failed;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = queryEngineId.ToTraceString();
                var arg1 = exception.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Error, 73, "Transaction log on query engine '{0}' garbage collection failed. {1}", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Transaction_Log_Garbage_Collection_Failed(arg0, arg1);
                }
            }
        }

        public static void Create_Artifact_Unexpected_Transaction_Disposal_Exception(this TraceSource source, System.Uri entityId, System.Exception exception)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceCreate_Artifact_Unexpected_Transaction_Disposal_Exception;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = entityId.ToTraceString();
                var arg1 = exception.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Error, 74, "Creation of artifact '{0}' failed after adding to engine and transaction log. This is most likely due to an error in disposing the transaction. Exception: '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Create_Artifact_Unexpected_Transaction_Disposal_Exception(arg0, arg1);
                }
            }
        }

        public static void BlobLogs_Done_Success(this TraceSource source, System.String path, System.Uri queryEngineId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceBlobLogs_Done_Success;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = path;
                var arg1 = queryEngineId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 75, "Recovery state blob log finished writing to '{0}' on query engine '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.BlobLogs_Done_Success(arg0, arg1);
                }
            }
        }

        public static void BlobLogs_Done_Canceled(this TraceSource source, System.String path, System.Uri queryEngineId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceBlobLogs_Done_Canceled;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = path;
                var arg1 = queryEngineId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 76, "Recovery state blob log canceled writing to '{0}' on query engine '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.BlobLogs_Done_Canceled(arg0, arg1);
                }
            }
        }

        public static void BlobLogs_Done_Error(this TraceSource source, System.String path, System.Uri queryEngineId, System.Exception error)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceBlobLogs_Done_Error;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = path;
                var arg1 = queryEngineId.ToTraceString();
                var arg2 = error.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Warning, 77, "Recovery state blob log failed writing to '{0}' on query engine '{1}'. Error = {2}", arg0, arg1, arg2);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.BlobLogs_Done_Error(arg0, arg1, arg2);
                }
            }
        }

        public static void HigherOrderOperator_CreatingBridge(this TraceSource source, System.Uri bridgeId, System.Uri subscriptionId, System.Linq.Expressions.Expression definition)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceHigherOrderOperator_CreatingBridge;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = bridgeId.ToTraceString();
                var arg1 = subscriptionId.ToTraceString();
                var arg2 = definition.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 78, "Creating bridge '{0}' for subscription '{1}'. Definition = '{2}'.", arg0, arg1, arg2);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.HigherOrderOperator_CreatingBridge(arg0, arg1, arg2);
                }
            }
        }

        public static void HigherOrderOperator_StartingInnerSubscription(this TraceSource source, System.Uri bridgeId, System.Uri subscriptionId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceHigherOrderOperator_StartingInnerSubscription;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = bridgeId.ToTraceString();
                var arg1 = subscriptionId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 79, "Starting inner subscription to bridge '{0}' for higher order subscription '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.HigherOrderOperator_StartingInnerSubscription(arg0, arg1);
                }
            }
        }

        public static void HigherOrderOperator_DisposingInnerSubscription(this TraceSource source, System.Uri bridgeId, System.Uri subscriptionId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceHigherOrderOperator_DisposingInnerSubscription;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = bridgeId.ToTraceString();
                var arg1 = subscriptionId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 80, "Disposing inner subscription to bridge '{0}' for higher order subscription '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.HigherOrderOperator_DisposingInnerSubscription(arg0, arg1);
                }
            }
        }

        public static void HigherOrderOperator_DeletingBridge(this TraceSource source, System.Uri bridgeId, System.Uri subscriptionId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceHigherOrderOperator_DeletingBridge;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = bridgeId.ToTraceString();
                var arg1 = subscriptionId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 81, "Removing stream for bridge '{0}' for higher order subscription '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.HigherOrderOperator_DeletingBridge(arg0, arg1);
                }
            }
        }

        public static void InputEdge_ExternalSubscription_Dispose_Failed(this TraceSource source, System.Uri externalSubscriptionId, System.Uri edgeId, System.Exception exception)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceInputEdge_ExternalSubscription_Dispose_Failed;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = externalSubscriptionId.ToTraceString();
                var arg1 = edgeId.ToTraceString();
                var arg2 = exception.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Warning, 82, "Deleting external subscription '{0}' on input edge '{1}' failed. Exception: '{2}'.", arg0, arg1, arg2);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.InputEdge_ExternalSubscription_Dispose_Failed(arg0, arg1, arg2);
                }
            }
        }


        [EventSource(Name = "Nuqleon-Reactive-QueryEngine")]
        private class Impl : EventSource
        {
            internal static Impl Instance = new();

            private Impl()
                : base()
            {
            }

            /// <summary>
            /// Class providing keywords to annotate events with in order to allow filtering.
            /// </summary>
            public static class Keywords
            {
                /// <summary>
                /// Underlying value for keyword BlobLogs
                /// </summary>
                public const EventKeywords BlobLogs = (EventKeywords)1UL;

                /// <summary>
                /// Underlying value for keyword Canary
                /// </summary>
                public const EventKeywords Canary = (EventKeywords)2UL;

                /// <summary>
                /// Underlying value for keyword Checkpoint
                /// </summary>
                public const EventKeywords Checkpoint = (EventKeywords)4UL;

                /// <summary>
                /// Underlying value for keyword Recovery
                /// </summary>
                public const EventKeywords Recovery = (EventKeywords)8UL;

                /// <summary>
                /// Underlying value for keyword Mitigation
                /// </summary>
                public const EventKeywords Mitigation = (EventKeywords)16UL;

                /// <summary>
                /// Underlying value for keyword StateOperation
                /// </summary>
                public const EventKeywords StateOperation = (EventKeywords)32UL;

                /// <summary>
                /// Underlying value for keyword FailSafe
                /// </summary>
                public const EventKeywords FailSafe = (EventKeywords)64UL;

                /// <summary>
                /// Underlying value for keyword LazyStream
                /// </summary>
                public const EventKeywords LazyStream = (EventKeywords)128UL;

                /// <summary>
                /// Underlying value for keyword Registry
                /// </summary>
                public const EventKeywords Registry = (EventKeywords)256UL;

                /// <summary>
                /// Underlying value for keyword GarbageCollection
                /// </summary>
                public const EventKeywords GarbageCollection = (EventKeywords)512UL;

                /// <summary>
                /// Underlying value for keyword TemplateMigration
                /// </summary>
                public const EventKeywords TemplateMigration = (EventKeywords)1024UL;

                /// <summary>
                /// Underlying value for keyword ReliableSubscriptionInput
                /// </summary>
                public const EventKeywords ReliableSubscriptionInput = (EventKeywords)2048UL;

                /// <summary>
                /// Underlying value for keyword CheckpointableStateManager
                /// </summary>
                public const EventKeywords CheckpointableStateManager = (EventKeywords)4096UL;

                /// <summary>
                /// Underlying value for keyword Bridge
                /// </summary>
                public const EventKeywords Bridge = (EventKeywords)8192UL;

                /// <summary>
                /// Underlying value for keyword TransactionLog
                /// </summary>
                public const EventKeywords TransactionLog = (EventKeywords)16384UL;

                /// <summary>
                /// Underlying value for keyword HigherOrderOperator
                /// </summary>
                public const EventKeywords HigherOrderOperator = (EventKeywords)32768UL;

                /// <summary>
                /// Underlying value for keyword Edge
                /// </summary>
                public const EventKeywords Edge = (EventKeywords)65536UL;

            }

            public static class Tasks
            {
                public const EventTask None = (EventTask)1;
            }

            public static class Opcodes
            {
            }

            [Event(1,
                Level = EventLevel.Informational,
                Keywords = Keywords.Checkpoint,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Scheduler of query engine '{0}' wasn't resumed from {1} because an unload operation is pending.",
                Version = 1)]
            internal unsafe void Checkpoint_ContinueScheduler_Unloaded(String queryEngineId, String caller)
            {
                queryEngineId ??= string.Empty;
                caller ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                fixed (char* callerBytes = caller)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)callerBytes;
                    descrs[1].Size = checked((caller.Length + 1) * 2);

                    WriteEventCore(1, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceCheckpoint_ContinueScheduler_Unloaded => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Checkpoint);

            [Event(2,
                Level = EventLevel.Informational,
                Keywords = Keywords.Checkpoint,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Query engine '{0}' finished deleting items in category '{1}'. Total = {2}",
                Version = 1)]
            internal unsafe void Checkpoint_DeleteCompleted(String queryEngineId, String category, Int32 count)
            {
                queryEngineId ??= string.Empty;
                category ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                fixed (char* categoryBytes = category)
                {
                    var dataCount = 3;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)categoryBytes;
                    descrs[1].Size = checked((category.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)(&count);
                    descrs[2].Size = sizeof(Int32);

                    WriteEventCore(2, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceCheckpoint_DeleteCompleted => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Checkpoint);

            [Event(3,
                Level = EventLevel.Informational,
                Keywords = Keywords.Checkpoint,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Query engine '{0}' deleting items in category '{1}'.",
                Version = 1)]
            internal unsafe void Checkpoint_DeleteStarted(String queryEngineId, String category)
            {
                queryEngineId ??= string.Empty;
                category ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                fixed (char* categoryBytes = category)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)categoryBytes;
                    descrs[1].Size = checked((category.Length + 1) * 2);

                    WriteEventCore(3, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceCheckpoint_DeleteStarted => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Checkpoint);

            [Event(4,
                Level = EventLevel.Error,
                Keywords = Keywords.Checkpoint,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Query engine '{0}' failed to save entity in category '{1}' with key '{2}'. Error: {3}",
                Version = 1)]
            internal unsafe void Checkpoint_SavingDefinitionsFailure(String queryEngineId, String category, String key, String errorMsg)
            {
                queryEngineId ??= string.Empty;
                category ??= string.Empty;
                key ??= string.Empty;
                errorMsg ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                fixed (char* categoryBytes = category)
                fixed (char* keyBytes = key)
                fixed (char* errorMsgBytes = errorMsg)
                {
                    var dataCount = 4;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)categoryBytes;
                    descrs[1].Size = checked((category.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)keyBytes;
                    descrs[2].Size = checked((key.Length + 1) * 2);

                    descrs[3].DataPointer = (IntPtr)errorMsgBytes;
                    descrs[3].Size = checked((errorMsg.Length + 1) * 2);

                    WriteEventCore(4, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceCheckpoint_SavingDefinitionsFailure => Impl.Instance.IsEnabled(EventLevel.Error, Keywords.Checkpoint);

            [Event(5,
                Level = EventLevel.Informational,
                Keywords = Keywords.Checkpoint,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Query engine '{0}' finished saving definitions in category '{1}'. Total = {2}, Skipped = {3}, Failed = {4}",
                Version = 1)]
            internal unsafe void Checkpoint_SavingDefinitionsCompleted(String queryEngineId, String category, Int32 total, Int32 skipped, Int32 failed)
            {
                queryEngineId ??= string.Empty;
                category ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                fixed (char* categoryBytes = category)
                {
                    var dataCount = 5;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)categoryBytes;
                    descrs[1].Size = checked((category.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)(&total);
                    descrs[2].Size = sizeof(Int32);

                    descrs[3].DataPointer = (IntPtr)(&skipped);
                    descrs[3].Size = sizeof(Int32);

                    descrs[4].DataPointer = (IntPtr)(&failed);
                    descrs[4].Size = sizeof(Int32);

                    WriteEventCore(5, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceCheckpoint_SavingDefinitionsCompleted => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Checkpoint);

            [Event(6,
                Level = EventLevel.Informational,
                Keywords = Keywords.Checkpoint,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Query engine '{0}' saving definitions in category '{1}'.",
                Version = 1)]
            internal unsafe void Checkpoint_SavingDefinitionsStarted(String queryEngineId, String category)
            {
                queryEngineId ??= string.Empty;
                category ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                fixed (char* categoryBytes = category)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)categoryBytes;
                    descrs[1].Size = checked((category.Length + 1) * 2);

                    WriteEventCore(6, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceCheckpoint_SavingDefinitionsStarted => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Checkpoint);

            [Event(7,
                Level = EventLevel.Error,
                Keywords = Keywords.Checkpoint,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Query engine '{0}' failed to save entity runtime state in category '{1}' with key '{2}'. Error: {3}",
                Version = 1)]
            internal unsafe void Checkpoint_SavingStateFailure(String queryEngineId, String category, String key, String errorMsg)
            {
                queryEngineId ??= string.Empty;
                category ??= string.Empty;
                key ??= string.Empty;
                errorMsg ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                fixed (char* categoryBytes = category)
                fixed (char* keyBytes = key)
                fixed (char* errorMsgBytes = errorMsg)
                {
                    var dataCount = 4;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)categoryBytes;
                    descrs[1].Size = checked((category.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)keyBytes;
                    descrs[2].Size = checked((key.Length + 1) * 2);

                    descrs[3].DataPointer = (IntPtr)errorMsgBytes;
                    descrs[3].Size = checked((errorMsg.Length + 1) * 2);

                    WriteEventCore(7, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceCheckpoint_SavingStateFailure => Impl.Instance.IsEnabled(EventLevel.Error, Keywords.Checkpoint);

            [Event(8,
                Level = EventLevel.Informational,
                Keywords = Keywords.Checkpoint,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Query engine '{0}' finished saving runtime state in category '{1}'. Total = {2}, Skipped = {3}, Failed = {4}",
                Version = 1)]
            internal unsafe void Checkpoint_SavingStateCompleted(String queryEngineId, String category, Int32 total, Int32 skipped, Int32 failed)
            {
                queryEngineId ??= string.Empty;
                category ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                fixed (char* categoryBytes = category)
                {
                    var dataCount = 5;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)categoryBytes;
                    descrs[1].Size = checked((category.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)(&total);
                    descrs[2].Size = sizeof(Int32);

                    descrs[3].DataPointer = (IntPtr)(&skipped);
                    descrs[3].Size = sizeof(Int32);

                    descrs[4].DataPointer = (IntPtr)(&failed);
                    descrs[4].Size = sizeof(Int32);

                    WriteEventCore(8, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceCheckpoint_SavingStateCompleted => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Checkpoint);

            [Event(9,
                Level = EventLevel.Informational,
                Keywords = Keywords.Checkpoint,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Query engine '{0}' saving runtime state in category '{1}'.",
                Version = 1)]
            internal unsafe void Checkpoint_SavingStateStarted(String queryEngineId, String category)
            {
                queryEngineId ??= string.Empty;
                category ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                fixed (char* categoryBytes = category)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)categoryBytes;
                    descrs[1].Size = checked((category.Length + 1) * 2);

                    WriteEventCore(9, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceCheckpoint_SavingStateStarted => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Checkpoint);

            [Event(10,
                Level = EventLevel.Error,
                Keywords = Keywords.FailSafe,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Action invoked by '{0}' failed with exception '{1}'.",
                Version = 1)]
            internal unsafe void FailSafe_Exception(String method, String exception)
            {
                method ??= string.Empty;
                exception ??= string.Empty;
                fixed (char* methodBytes = method)
                fixed (char* exceptionBytes = exception)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)methodBytes;
                    descrs[0].Size = checked((method.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)exceptionBytes;
                    descrs[1].Size = checked((exception.Length + 1) * 2);

                    WriteEventCore(10, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceFailSafe_Exception => Impl.Instance.IsEnabled(EventLevel.Error, Keywords.FailSafe);

            [Event(11,
                Level = EventLevel.Informational,
                Keywords = Keywords.LazyStream,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Stream for key '{0}' does not exist on query engine '{1}'. Creating stream from remote definition '{2}'.",
                Version = 1)]
            internal unsafe void LazyStream_Created(String key, String queryEngineId, String expression)
            {
                key ??= string.Empty;
                queryEngineId ??= string.Empty;
                expression ??= string.Empty;
                fixed (char* keyBytes = key)
                fixed (char* queryEngineIdBytes = queryEngineId)
                fixed (char* expressionBytes = expression)
                {
                    var dataCount = 3;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)keyBytes;
                    descrs[0].Size = checked((key.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[1].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)expressionBytes;
                    descrs[2].Size = checked((expression.Length + 1) * 2);

                    WriteEventCore(11, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceLazyStream_Created => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.LazyStream);

            [Event(12,
                Level = EventLevel.Informational,
                Keywords = Keywords.LazyStream,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Rewriting stream definition for '{0}' from '{1}' to '{2}' on query engine '{3}'.",
                Version = 1)]
            internal unsafe void LazyStream_TypeRewrite(String key, String streamType, String newStreamType, String queryEngineId)
            {
                key ??= string.Empty;
                streamType ??= string.Empty;
                newStreamType ??= string.Empty;
                queryEngineId ??= string.Empty;
                fixed (char* keyBytes = key)
                fixed (char* streamTypeBytes = streamType)
                fixed (char* newStreamTypeBytes = newStreamType)
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 4;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)keyBytes;
                    descrs[0].Size = checked((key.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)streamTypeBytes;
                    descrs[1].Size = checked((streamType.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)newStreamTypeBytes;
                    descrs[2].Size = checked((newStreamType.Length + 1) * 2);

                    descrs[3].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[3].Size = checked((queryEngineId.Length + 1) * 2);

                    WriteEventCore(12, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceLazyStream_TypeRewrite => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.LazyStream);

            [Event(13,
                Level = EventLevel.Informational,
                Keywords = Keywords.Registry,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Added invalid entity entry to query engine '{0}' registry for '{1}' '{2}'.",
                Version = 1)]
            internal unsafe void Registry_AddEntityPlaceholder(String queryEngineId, String kind, String key)
            {
                queryEngineId ??= string.Empty;
                kind ??= string.Empty;
                key ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                fixed (char* kindBytes = kind)
                fixed (char* keyBytes = key)
                {
                    var dataCount = 3;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)kindBytes;
                    descrs[1].Size = checked((kind.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)keyBytes;
                    descrs[2].Size = checked((key.Length + 1) * 2);

                    WriteEventCore(13, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceRegistry_AddEntityPlaceholder => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Registry);

            [Event(14,
                Level = EventLevel.Informational,
                Keywords = Keywords.Registry,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Removed '{0}' '{1}' from query engine '{2}'.",
                Version = 1)]
            internal unsafe void Registry_RemoveEntity(String kind, String key, String queryEngineId)
            {
                kind ??= string.Empty;
                key ??= string.Empty;
                queryEngineId ??= string.Empty;
                fixed (char* kindBytes = kind)
                fixed (char* keyBytes = key)
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 3;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)kindBytes;
                    descrs[0].Size = checked((kind.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)keyBytes;
                    descrs[1].Size = checked((key.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[2].Size = checked((queryEngineId.Length + 1) * 2);

                    WriteEventCore(14, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceRegistry_RemoveEntity => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Registry);

            [Event(15,
                Level = EventLevel.Informational,
                Keywords = Keywords.Registry | Keywords.GarbageCollection,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Garbage collector on query engine '{0}' has found {1} live dependencies. Mark time = {2}ms",
                Version = 1)]
            internal unsafe void RegistryGarbageCollection_LiveDependencies(String queryEngineId, Int32 count, Int32 markTime)
            {
                queryEngineId ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 3;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)(&count);
                    descrs[1].Size = sizeof(Int32);

                    descrs[2].DataPointer = (IntPtr)(&markTime);
                    descrs[2].Size = sizeof(Int32);

                    WriteEventCore(15, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceRegistryGarbageCollection_LiveDependencies => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Registry | Keywords.GarbageCollection);

            [Event(16,
                Level = EventLevel.Informational,
                Keywords = Keywords.Registry | Keywords.GarbageCollection,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Garbage collector on query engine '{0}' is not enabled.",
                Version = 1)]
            internal unsafe void RegistryGarbageCollection_NotEnabled(String queryEngineId)
            {
                queryEngineId ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 1;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    WriteEventCore(16, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceRegistryGarbageCollection_NotEnabled => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Registry | Keywords.GarbageCollection);

            [Event(17,
                Level = EventLevel.Informational,
                Keywords = Keywords.Registry | Keywords.GarbageCollection,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Garbage collector on query engine '{0}' has started.",
                Version = 1)]
            internal unsafe void RegistryGarbageCollection_Started(String queryEngineId)
            {
                queryEngineId ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 1;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    WriteEventCore(17, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceRegistryGarbageCollection_Started => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Registry | Keywords.GarbageCollection);

            [Event(18,
                Level = EventLevel.Informational,
                Keywords = Keywords.Registry | Keywords.GarbageCollection,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Garbage collector on query engine '{0}' has no further sweep work. Performed {1} sweep operations over {2} iterations. Total sweep time = {3}ms",
                Version = 1)]
            internal unsafe void RegistryGarbageCollection_SweepCompleted(String queryEngineId, Int32 count, Int32 iterations, Int32 duration)
            {
                queryEngineId ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 4;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)(&count);
                    descrs[1].Size = sizeof(Int32);

                    descrs[2].DataPointer = (IntPtr)(&iterations);
                    descrs[2].Size = sizeof(Int32);

                    descrs[3].DataPointer = (IntPtr)(&duration);
                    descrs[3].Size = sizeof(Int32);

                    WriteEventCore(18, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceRegistryGarbageCollection_SweepCompleted => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Registry | Keywords.GarbageCollection);

            [Event(19,
                Level = EventLevel.Informational,
                Keywords = Keywords.Registry | Keywords.GarbageCollection,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Garbage collector on query engine '{0}' is running without sweeping enabled. No further actions will be taken.",
                Version = 1)]
            internal unsafe void RegistryGarbageCollection_SweepDisabled(String queryEngineId)
            {
                queryEngineId ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 1;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    WriteEventCore(19, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceRegistryGarbageCollection_SweepDisabled => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Registry | Keywords.GarbageCollection);

            [Event(20,
                Level = EventLevel.Informational,
                Keywords = Keywords.Registry | Keywords.GarbageCollection,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Garbage collector on query engine '{0}' is starting the sweep phase. Remaining entities = {1}.",
                Version = 1)]
            internal unsafe void RegistryGarbageCollection_SweepStarted(String queryEngineId, Int32 count)
            {
                queryEngineId ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)(&count);
                    descrs[1].Size = sizeof(Int32);

                    WriteEventCore(20, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceRegistryGarbageCollection_SweepStarted => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Registry | Keywords.GarbageCollection);

            [Event(21,
                Level = EventLevel.Informational,
                Keywords = Keywords.Registry | Keywords.GarbageCollection,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Garbage collector on query engine '{0}' has finished sweeping {1} artifacts. Remaining entities = {2}. Sweep time = {3}ms",
                Version = 1)]
            internal unsafe void RegistryGarbageCollection_SweepStopped(String queryEngineId, Int32 count, Int32 remaining, Int32 duration)
            {
                queryEngineId ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 4;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)(&count);
                    descrs[1].Size = sizeof(Int32);

                    descrs[2].DataPointer = (IntPtr)(&remaining);
                    descrs[2].Size = sizeof(Int32);

                    descrs[3].DataPointer = (IntPtr)(&duration);
                    descrs[3].Size = sizeof(Int32);

                    WriteEventCore(21, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceRegistryGarbageCollection_SweepStopped => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Registry | Keywords.GarbageCollection);

            [Event(22,
                Level = EventLevel.Informational,
                Keywords = Keywords.Registry | Keywords.GarbageCollection,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Garbage collector on query engine '{0}' has found {1} unreachable observable definitions out of a total {2}. Scan time = {3}ms",
                Version = 1)]
            internal unsafe void RegistryGarbageCollection_UnreachableObservables(String queryEngineId, Int32 count, Int32 total, Int32 scanTime)
            {
                queryEngineId ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 4;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)(&count);
                    descrs[1].Size = sizeof(Int32);

                    descrs[2].DataPointer = (IntPtr)(&total);
                    descrs[2].Size = sizeof(Int32);

                    descrs[3].DataPointer = (IntPtr)(&scanTime);
                    descrs[3].Size = sizeof(Int32);

                    WriteEventCore(22, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceRegistryGarbageCollection_UnreachableObservables => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Registry | Keywords.GarbageCollection);

            [Event(23,
                Level = EventLevel.Error,
                Keywords = Keywords.StateOperation,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Failed to {0} {1} '{2}'. Error: {3}",
                Version = 1)]
            internal unsafe void StateOperation_Error(String verb, String noun, String uri, String error)
            {
                verb ??= string.Empty;
                noun ??= string.Empty;
                uri ??= string.Empty;
                error ??= string.Empty;
                fixed (char* verbBytes = verb)
                fixed (char* nounBytes = noun)
                fixed (char* uriBytes = uri)
                fixed (char* errorBytes = error)
                {
                    var dataCount = 4;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)verbBytes;
                    descrs[0].Size = checked((verb.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)nounBytes;
                    descrs[1].Size = checked((noun.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)uriBytes;
                    descrs[2].Size = checked((uri.Length + 1) * 2);

                    descrs[3].DataPointer = (IntPtr)errorBytes;
                    descrs[3].Size = checked((error.Length + 1) * 2);

                    WriteEventCore(23, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceStateOperation_Error => Impl.Instance.IsEnabled(EventLevel.Error, Keywords.StateOperation);

            [Event(24,
                Level = EventLevel.Verbose,
                Keywords = Keywords.StateOperation,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Executed {0} {1} '{2}' in {3} ms.",
                Version = 1)]
            internal unsafe void StateOperation_Executed(String verb, String noun, String entityId, Int64 duration)
            {
                verb ??= string.Empty;
                noun ??= string.Empty;
                entityId ??= string.Empty;
                fixed (char* verbBytes = verb)
                fixed (char* nounBytes = noun)
                fixed (char* entityIdBytes = entityId)
                {
                    var dataCount = 4;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)verbBytes;
                    descrs[0].Size = checked((verb.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)nounBytes;
                    descrs[1].Size = checked((noun.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)entityIdBytes;
                    descrs[2].Size = checked((entityId.Length + 1) * 2);

                    descrs[3].DataPointer = (IntPtr)(&duration);
                    descrs[3].Size = sizeof(Int64);

                    WriteEventCore(24, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceStateOperation_Executed => Impl.Instance.IsEnabled(EventLevel.Verbose, Keywords.StateOperation);

            [Event(25,
                Level = EventLevel.Warning,
                Keywords = Keywords.StateOperation,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Failed to {0} {1} '{2}'. Error: {3}",
                Version = 1)]
            internal unsafe void StateOperation_Warning(String verb, String noun, String entityId, String error)
            {
                verb ??= string.Empty;
                noun ??= string.Empty;
                entityId ??= string.Empty;
                error ??= string.Empty;
                fixed (char* verbBytes = verb)
                fixed (char* nounBytes = noun)
                fixed (char* entityIdBytes = entityId)
                fixed (char* errorBytes = error)
                {
                    var dataCount = 4;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)verbBytes;
                    descrs[0].Size = checked((verb.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)nounBytes;
                    descrs[1].Size = checked((noun.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)entityIdBytes;
                    descrs[2].Size = checked((entityId.Length + 1) * 2);

                    descrs[3].DataPointer = (IntPtr)errorBytes;
                    descrs[3].Size = checked((error.Length + 1) * 2);

                    WriteEventCore(25, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceStateOperation_Warning => Impl.Instance.IsEnabled(EventLevel.Warning, Keywords.StateOperation);

            [Event(26,
                Level = EventLevel.Informational,
                Keywords = Keywords.TemplateMigration,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Template migration task for query engine '{0}' has completed.",
                Version = 1)]
            internal unsafe void TemplateMigration_Completed(String queryEngineId)
            {
                queryEngineId ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 1;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    WriteEventCore(26, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceTemplateMigration_Completed => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.TemplateMigration);

            [Event(27,
                Level = EventLevel.Informational,
                Keywords = Keywords.TemplateMigration,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Template migration task for query engine '{0}' templatized expression for entity '{1}'.",
                Version = 1)]
            internal unsafe void TemplateMigration_Execute(String queryEngineId, String entityId)
            {
                queryEngineId ??= string.Empty;
                entityId ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                fixed (char* entityIdBytes = entityId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)entityIdBytes;
                    descrs[1].Size = checked((entityId.Length + 1) * 2);

                    WriteEventCore(27, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceTemplateMigration_Execute => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.TemplateMigration);

            [Event(28,
                Level = EventLevel.Error,
                Keywords = Keywords.TemplateMigration,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Template migration task for query engine '{0}' could not parse regular expression '{1}'.",
                Version = 1)]
            internal unsafe void TemplateMigration_RegexInvalid(String queryEngineId, String regex)
            {
                queryEngineId ??= string.Empty;
                regex ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                fixed (char* regexBytes = regex)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)regexBytes;
                    descrs[1].Size = checked((regex.Length + 1) * 2);

                    WriteEventCore(28, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceTemplateMigration_RegexInvalid => Impl.Instance.IsEnabled(EventLevel.Error, Keywords.TemplateMigration);

            [Event(29,
                Level = EventLevel.Informational,
                Keywords = Keywords.TemplateMigration,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Template migration task for query engine '{0}' resetting quota to '{1}' after templatizing '{2}' entities.",
                Version = 1)]
            internal unsafe void TemplateMigration_ResetQuota(String queryEngineId, Int32 quota, Int32 count)
            {
                queryEngineId ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 3;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)(&quota);
                    descrs[1].Size = sizeof(Int32);

                    descrs[2].DataPointer = (IntPtr)(&count);
                    descrs[2].Size = sizeof(Int32);

                    WriteEventCore(29, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceTemplateMigration_ResetQuota => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.TemplateMigration);

            [Event(30,
                Level = EventLevel.Informational,
                Keywords = Keywords.TemplateMigration,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Template migration task for query engine '{0}' has started for entity keys matching '{1}'.",
                Version = 1)]
            internal unsafe void TemplateMigration_Started(String queryEngineId, String regex)
            {
                queryEngineId ??= string.Empty;
                regex ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                fixed (char* regexBytes = regex)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)regexBytes;
                    descrs[1].Size = checked((regex.Length + 1) * 2);

                    WriteEventCore(30, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceTemplateMigration_Started => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.TemplateMigration);

            [Event(31,
                Level = EventLevel.Informational,
                Keywords = Keywords.BlobLogs,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Recovery state blob log created at '{0}' on query engine '{1}'.",
                Version = 2)]
            internal unsafe void BlobLogs_Created(String path, String queryEngineId)
            {
                path ??= string.Empty;
                queryEngineId ??= string.Empty;
                fixed (char* pathBytes = path)
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)pathBytes;
                    descrs[0].Size = checked((path.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[1].Size = checked((queryEngineId.Length + 1) * 2);

                    WriteEventCore(31, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceBlobLogs_Created => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.BlobLogs);

            [Event(32,
                Level = EventLevel.Warning,
                Keywords = Keywords.BlobLogs,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Recovery state blob log could not be created at '{0}' on query engine '{1}'. Error = {2}",
                Version = 1)]
            internal unsafe void BlobLogs_CreateFailed(String path, String queryEngineId, String error)
            {
                path ??= string.Empty;
                queryEngineId ??= string.Empty;
                error ??= string.Empty;
                fixed (char* pathBytes = path)
                fixed (char* queryEngineIdBytes = queryEngineId)
                fixed (char* errorBytes = error)
                {
                    var dataCount = 3;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)pathBytes;
                    descrs[0].Size = checked((path.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[1].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)errorBytes;
                    descrs[2].Size = checked((error.Length + 1) * 2);

                    WriteEventCore(32, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceBlobLogs_CreateFailed => Impl.Instance.IsEnabled(EventLevel.Warning, Keywords.BlobLogs);

            [Event(33,
                Level = EventLevel.Warning,
                Keywords = Keywords.Canary,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Canary observable '{0}' not recovered for query engine '{1}'.",
                Version = 1)]
            internal unsafe void Canary_NotRecovered_Observable(String canaryId, String queryEngineId)
            {
                canaryId ??= string.Empty;
                queryEngineId ??= string.Empty;
                fixed (char* canaryIdBytes = canaryId)
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)canaryIdBytes;
                    descrs[0].Size = checked((canaryId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[1].Size = checked((queryEngineId.Length + 1) * 2);

                    WriteEventCore(33, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceCanary_NotRecovered_Observable => Impl.Instance.IsEnabled(EventLevel.Warning, Keywords.Canary);

            [Event(34,
                Level = EventLevel.Warning,
                Keywords = Keywords.Canary,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Canary observer '{0}' not recovered for query engine '{1}'.",
                Version = 1)]
            internal unsafe void Canary_NotRecovered_Observer(String canaryId, String queryEngineId)
            {
                canaryId ??= string.Empty;
                queryEngineId ??= string.Empty;
                fixed (char* canaryIdBytes = canaryId)
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)canaryIdBytes;
                    descrs[0].Size = checked((canaryId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[1].Size = checked((queryEngineId.Length + 1) * 2);

                    WriteEventCore(34, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceCanary_NotRecovered_Observer => Impl.Instance.IsEnabled(EventLevel.Warning, Keywords.Canary);

            [Event(35,
                Level = EventLevel.Warning,
                Keywords = Keywords.Canary,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Canary subscription '{0}' not recovered for query engine '{1}'.",
                Version = 1)]
            internal unsafe void Canary_NotRecovered_Subscription(String canaryId, String queryEngineId)
            {
                canaryId ??= string.Empty;
                queryEngineId ??= string.Empty;
                fixed (char* canaryIdBytes = canaryId)
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)canaryIdBytes;
                    descrs[0].Size = checked((canaryId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[1].Size = checked((queryEngineId.Length + 1) * 2);

                    WriteEventCore(35, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceCanary_NotRecovered_Subscription => Impl.Instance.IsEnabled(EventLevel.Warning, Keywords.Canary);

            [Event(36,
                Level = EventLevel.Informational,
                Keywords = Keywords.Canary,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Canary observable '{0}' defined on query engine '{1}'.",
                Version = 1)]
            internal unsafe void Canary_Created_Observable(String canaryId, String queryEngineId)
            {
                canaryId ??= string.Empty;
                queryEngineId ??= string.Empty;
                fixed (char* canaryIdBytes = canaryId)
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)canaryIdBytes;
                    descrs[0].Size = checked((canaryId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[1].Size = checked((queryEngineId.Length + 1) * 2);

                    WriteEventCore(36, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceCanary_Created_Observable => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Canary);

            [Event(37,
                Level = EventLevel.Informational,
                Keywords = Keywords.Canary,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Canary observer '{0}' defined on query engine '{1}'.",
                Version = 1)]
            internal unsafe void Canary_Created_Observer(String canaryId, String queryEngineId)
            {
                canaryId ??= string.Empty;
                queryEngineId ??= string.Empty;
                fixed (char* canaryIdBytes = canaryId)
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)canaryIdBytes;
                    descrs[0].Size = checked((canaryId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[1].Size = checked((queryEngineId.Length + 1) * 2);

                    WriteEventCore(37, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceCanary_Created_Observer => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Canary);

            [Event(38,
                Level = EventLevel.Informational,
                Keywords = Keywords.Canary,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Canary subscription '{0}' defined on query engine '{1}'.",
                Version = 1)]
            internal unsafe void Canary_Created_Subscription(String canaryId, String queryEngineId)
            {
                canaryId ??= string.Empty;
                queryEngineId ??= string.Empty;
                fixed (char* canaryIdBytes = canaryId)
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)canaryIdBytes;
                    descrs[0].Size = checked((canaryId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[1].Size = checked((queryEngineId.Length + 1) * 2);

                    WriteEventCore(38, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceCanary_Created_Subscription => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Canary);

            [Event(39,
                Level = EventLevel.Informational,
                Keywords = Keywords.Mitigation,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Recovery of '{0}' failed. Executing mitigation '{1}'.",
                Version = 1)]
            internal unsafe void Mitigation_Execute(String entityId, String mitigation)
            {
                entityId ??= string.Empty;
                mitigation ??= string.Empty;
                fixed (char* entityIdBytes = entityId)
                fixed (char* mitigationBytes = mitigation)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)entityIdBytes;
                    descrs[0].Size = checked((entityId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)mitigationBytes;
                    descrs[1].Size = checked((mitigation.Length + 1) * 2);

                    WriteEventCore(39, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceMitigation_Execute => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Mitigation);

            [Event(40,
                Level = EventLevel.Informational,
                Keywords = Keywords.Mitigation,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Mitigation '{0}' for recovery failure of '{1}' was not accepted. Exception: '{2}'.",
                Version = 1)]
            internal unsafe void Mitigation_NotAccepted(String mitigation, String entityId, String error)
            {
                mitigation ??= string.Empty;
                entityId ??= string.Empty;
                error ??= string.Empty;
                fixed (char* mitigationBytes = mitigation)
                fixed (char* entityIdBytes = entityId)
                fixed (char* errorBytes = error)
                {
                    var dataCount = 3;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)mitigationBytes;
                    descrs[0].Size = checked((mitigation.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)entityIdBytes;
                    descrs[1].Size = checked((entityId.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)errorBytes;
                    descrs[2].Size = checked((error.Length + 1) * 2);

                    WriteEventCore(40, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceMitigation_NotAccepted => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Mitigation);

            [Event(41,
                Level = EventLevel.Informational,
                Keywords = Keywords.Mitigation,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Mitigation '{0}' for recovery failure of '{1}' threw exception: '{2}'.",
                Version = 1)]
            internal unsafe void Mitigation_Failure(String mitigation, String entityId, String error)
            {
                mitigation ??= string.Empty;
                entityId ??= string.Empty;
                error ??= string.Empty;
                fixed (char* mitigationBytes = mitigation)
                fixed (char* entityIdBytes = entityId)
                fixed (char* errorBytes = error)
                {
                    var dataCount = 3;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)mitigationBytes;
                    descrs[0].Size = checked((mitigation.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)entityIdBytes;
                    descrs[1].Size = checked((entityId.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)errorBytes;
                    descrs[2].Size = checked((error.Length + 1) * 2);

                    WriteEventCore(41, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceMitigation_Failure => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Mitigation);

            [Event(42,
                Level = EventLevel.Error,
                Keywords = Keywords.Recovery,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Query engine '{0}' failed to load entity in category '{1}' with key '{2}'. Error: {3}",
                Version = 1)]
            internal unsafe void Recovery_LoadingDefinitionsFailure(String queryEngineId, String category, String key, String errorMsg)
            {
                queryEngineId ??= string.Empty;
                category ??= string.Empty;
                key ??= string.Empty;
                errorMsg ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                fixed (char* categoryBytes = category)
                fixed (char* keyBytes = key)
                fixed (char* errorMsgBytes = errorMsg)
                {
                    var dataCount = 4;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)categoryBytes;
                    descrs[1].Size = checked((category.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)keyBytes;
                    descrs[2].Size = checked((key.Length + 1) * 2);

                    descrs[3].DataPointer = (IntPtr)errorMsgBytes;
                    descrs[3].Size = checked((errorMsg.Length + 1) * 2);

                    WriteEventCore(42, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceRecovery_LoadingDefinitionsFailure => Impl.Instance.IsEnabled(EventLevel.Error, Keywords.Recovery);

            [Event(43,
                Level = EventLevel.Informational,
                Keywords = Keywords.Recovery,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Query engine '{0}' finished loading definitions in category '{1}'. Total = {2}, Failed = {3}",
                Version = 1)]
            internal unsafe void Recovery_LoadingDefinitionsCompleted(String queryEngineId, String category, Int32 total, Int32 failed)
            {
                queryEngineId ??= string.Empty;
                category ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                fixed (char* categoryBytes = category)
                {
                    var dataCount = 4;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)categoryBytes;
                    descrs[1].Size = checked((category.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)(&total);
                    descrs[2].Size = sizeof(Int32);

                    descrs[3].DataPointer = (IntPtr)(&failed);
                    descrs[3].Size = sizeof(Int32);

                    WriteEventCore(43, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceRecovery_LoadingDefinitionsCompleted => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Recovery);

            [Event(44,
                Level = EventLevel.Informational,
                Keywords = Keywords.Recovery,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Query engine '{0}' loading definitions in category '{1}'.",
                Version = 1)]
            internal unsafe void Recovery_LoadingDefinitionsStarted(String queryEngineId, String category)
            {
                queryEngineId ??= string.Empty;
                category ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                fixed (char* categoryBytes = category)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)categoryBytes;
                    descrs[1].Size = checked((category.Length + 1) * 2);

                    WriteEventCore(44, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceRecovery_LoadingDefinitionsStarted => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Recovery);

            [Event(45,
                Level = EventLevel.Error,
                Keywords = Keywords.Recovery,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Query engine '{0}' failed to load entity state in category '{1}'/'{2}' with key '{3}'. Error: {4}",
                Version = 1)]
            internal unsafe void Recovery_LoadingStateFailure(String queryEngineId, String category, String stateCategory, String key, String errorMsg)
            {
                queryEngineId ??= string.Empty;
                category ??= string.Empty;
                stateCategory ??= string.Empty;
                key ??= string.Empty;
                errorMsg ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                fixed (char* categoryBytes = category)
                fixed (char* stateCategoryBytes = stateCategory)
                fixed (char* keyBytes = key)
                fixed (char* errorMsgBytes = errorMsg)
                {
                    var dataCount = 5;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)categoryBytes;
                    descrs[1].Size = checked((category.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)stateCategoryBytes;
                    descrs[2].Size = checked((stateCategory.Length + 1) * 2);

                    descrs[3].DataPointer = (IntPtr)keyBytes;
                    descrs[3].Size = checked((key.Length + 1) * 2);

                    descrs[4].DataPointer = (IntPtr)errorMsgBytes;
                    descrs[4].Size = checked((errorMsg.Length + 1) * 2);

                    WriteEventCore(45, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceRecovery_LoadingStateFailure => Impl.Instance.IsEnabled(EventLevel.Error, Keywords.Recovery);

            [Event(46,
                Level = EventLevel.Informational,
                Keywords = Keywords.Recovery,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Query engine '{0}' finished loading artifacts. Elapsed time = {1}ms",
                Version = 1)]
            internal unsafe void Recovery_LoadCompleted(String queryEngineId, Int64 duration)
            {
                queryEngineId ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)(&duration);
                    descrs[1].Size = sizeof(Int64);

                    WriteEventCore(46, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceRecovery_LoadCompleted => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Recovery);

            [Event(47,
                Level = EventLevel.Informational,
                Keywords = Keywords.Recovery,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Query engine '{0}' loading artifacts.",
                Version = 1)]
            internal unsafe void Recovery_LoadStarted(String queryEngineId)
            {
                queryEngineId ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 1;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    WriteEventCore(47, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceRecovery_LoadStarted => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Recovery);

            [Event(48,
                Level = EventLevel.Informational,
                Keywords = Keywords.Recovery,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Recovering subject '{0}' without state on query engine '{1}'.",
                Version = 1)]
            internal unsafe void Recovery_LoadSubjectWithoutState(String subjectId, String queryEngineId)
            {
                subjectId ??= string.Empty;
                queryEngineId ??= string.Empty;
                fixed (char* subjectIdBytes = subjectId)
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)subjectIdBytes;
                    descrs[0].Size = checked((subjectId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[1].Size = checked((queryEngineId.Length + 1) * 2);

                    WriteEventCore(48, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceRecovery_LoadSubjectWithoutState => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Recovery);

            [Event(49,
                Level = EventLevel.Informational,
                Keywords = Keywords.Recovery,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Recovering subscription '{0}' without state on query engine '{1}'.",
                Version = 1)]
            internal unsafe void Recovery_LoadSubscriptionWithoutState(String subjectId, String queryEngineId)
            {
                subjectId ??= string.Empty;
                queryEngineId ??= string.Empty;
                fixed (char* subjectIdBytes = subjectId)
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)subjectIdBytes;
                    descrs[0].Size = checked((subjectId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[1].Size = checked((queryEngineId.Length + 1) * 2);

                    WriteEventCore(49, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceRecovery_LoadSubscriptionWithoutState => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Recovery);

            [Event(50,
                Level = EventLevel.Informational,
                Keywords = Keywords.Recovery,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Query engine '{0}' finished starting artifacts. Elapsed time = {1}ms",
                Version = 1)]
            internal unsafe void Recovery_StartCompleted(String queryEngineId, Int64 duration)
            {
                queryEngineId ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)(&duration);
                    descrs[1].Size = sizeof(Int64);

                    WriteEventCore(50, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceRecovery_StartCompleted => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Recovery);

            [Event(51,
                Level = EventLevel.Informational,
                Keywords = Keywords.Recovery,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Query engine '{0}' starting {1} reliable subscriptions.",
                Version = 1)]
            internal unsafe void Recovery_StartReliableSubscriptions(String queryEngineId, Int32 count)
            {
                queryEngineId ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)(&count);
                    descrs[1].Size = sizeof(Int32);

                    WriteEventCore(51, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceRecovery_StartReliableSubscriptions => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Recovery);

            [Event(52,
                Level = EventLevel.Informational,
                Keywords = Keywords.Recovery,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Query engine '{0}' loading artifacts.",
                Version = 1)]
            internal unsafe void Recovery_StartStarted(String queryEngineId)
            {
                queryEngineId ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 1;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    WriteEventCore(52, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceRecovery_StartStarted => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Recovery);

            [Event(53,
                Level = EventLevel.Informational,
                Keywords = Keywords.Recovery,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Query engine '{0}' starting {1} subjects.",
                Version = 1)]
            internal unsafe void Recovery_StartSubjects(String queryEngineId, Int32 count)
            {
                queryEngineId ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)(&count);
                    descrs[1].Size = sizeof(Int32);

                    WriteEventCore(53, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceRecovery_StartSubjects => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Recovery);

            [Event(54,
                Level = EventLevel.Informational,
                Keywords = Keywords.Recovery,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Query engine '{0}' starting {1} subscriptions.",
                Version = 1)]
            internal unsafe void Recovery_StartSubscriptions(String queryEngineId, Int32 count)
            {
                queryEngineId ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)(&count);
                    descrs[1].Size = sizeof(Int32);

                    WriteEventCore(54, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceRecovery_StartSubscriptions => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Recovery);

            [Event(55,
                Level = EventLevel.Informational,
                Keywords = Keywords.Recovery,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Recovery summary for {0} on query engine {1} - {2}",
                Version = 1)]
            internal unsafe void Recovery_Summary(String category, String queryEngineId, String summary)
            {
                category ??= string.Empty;
                queryEngineId ??= string.Empty;
                summary ??= string.Empty;
                fixed (char* categoryBytes = category)
                fixed (char* queryEngineIdBytes = queryEngineId)
                fixed (char* summaryBytes = summary)
                {
                    var dataCount = 3;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)categoryBytes;
                    descrs[0].Size = checked((category.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[1].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)summaryBytes;
                    descrs[2].Size = checked((summary.Length + 1) * 2);

                    WriteEventCore(55, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceRecovery_Summary => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Recovery);

            [Event(56,
                Level = EventLevel.Informational,
                Keywords = Keywords.ReliableSubscriptionInput,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Reliable subscription input for '{0}' sent Start({1}).",
                Version = 1)]
            internal unsafe void ReliableSubscriptionInput_OnStart(String instanceId, Int64 sequenceId)
            {
                instanceId ??= string.Empty;
                fixed (char* instanceIdBytes = instanceId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)instanceIdBytes;
                    descrs[0].Size = checked((instanceId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)(&sequenceId);
                    descrs[1].Size = sizeof(Int64);

                    WriteEventCore(56, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceReliableSubscriptionInput_OnStart => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.ReliableSubscriptionInput);

            [Event(57,
                Level = EventLevel.Informational,
                Keywords = Keywords.ReliableSubscriptionInput,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Reliable subscription input for '{0}' sent AcknowledgeRange({1}).",
                Version = 1)]
            internal unsafe void ReliableSubscriptionInput_OnStateSaved(String instanceId, Int64 sequenceId)
            {
                instanceId ??= string.Empty;
                fixed (char* instanceIdBytes = instanceId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)instanceIdBytes;
                    descrs[0].Size = checked((instanceId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)(&sequenceId);
                    descrs[1].Size = sizeof(Int64);

                    WriteEventCore(57, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceReliableSubscriptionInput_OnStateSaved => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.ReliableSubscriptionInput);

            [Event(58,
                Level = EventLevel.Informational,
                Keywords = Keywords.CheckpointableStateManager,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Query engine '{0}' finished '{1}' request. Transitioning from state '{2}' to state '{3}'.",
                Version = 1)]
            internal unsafe void CheckpointableStateManager_TransitionCompleted(String queryEngineId, String caller, String oldStatus, String newStatus)
            {
                queryEngineId ??= string.Empty;
                caller ??= string.Empty;
                oldStatus ??= string.Empty;
                newStatus ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                fixed (char* callerBytes = caller)
                fixed (char* oldStatusBytes = oldStatus)
                fixed (char* newStatusBytes = newStatus)
                {
                    var dataCount = 4;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)callerBytes;
                    descrs[1].Size = checked((caller.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)oldStatusBytes;
                    descrs[2].Size = checked((oldStatus.Length + 1) * 2);

                    descrs[3].DataPointer = (IntPtr)newStatusBytes;
                    descrs[3].Size = checked((newStatus.Length + 1) * 2);

                    WriteEventCore(58, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceCheckpointableStateManager_TransitionCompleted => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.CheckpointableStateManager);

            [Event(59,
                Level = EventLevel.Informational,
                Keywords = Keywords.CheckpointableStateManager,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Query engine '{0}' processing '{1}' request. Transitioning from state '{2}' to state '{3}'.",
                Version = 1)]
            internal unsafe void CheckpointableStateManager_TransitionStarted(String queryEngineId, String caller, String oldStatus, String newStatus)
            {
                queryEngineId ??= string.Empty;
                caller ??= string.Empty;
                oldStatus ??= string.Empty;
                newStatus ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                fixed (char* callerBytes = caller)
                fixed (char* oldStatusBytes = oldStatus)
                fixed (char* newStatusBytes = newStatus)
                {
                    var dataCount = 4;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)callerBytes;
                    descrs[1].Size = checked((caller.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)oldStatusBytes;
                    descrs[2].Size = checked((oldStatus.Length + 1) * 2);

                    descrs[3].DataPointer = (IntPtr)newStatusBytes;
                    descrs[3].Size = checked((newStatus.Length + 1) * 2);

                    WriteEventCore(59, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceCheckpointableStateManager_TransitionStarted => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.CheckpointableStateManager);

            [Event(60,
                Level = EventLevel.Informational,
                Keywords = Keywords.Bridge,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Defining upstream observable '{0}' for bridge '{1}'. Definition = '{2}'.",
                Version = 1)]
            internal unsafe void Bridge_CreatingUpstreamObservable(String observableId, String bridgeId, String expression)
            {
                observableId ??= string.Empty;
                bridgeId ??= string.Empty;
                expression ??= string.Empty;
                fixed (char* observableIdBytes = observableId)
                fixed (char* bridgeIdBytes = bridgeId)
                fixed (char* expressionBytes = expression)
                {
                    var dataCount = 3;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)observableIdBytes;
                    descrs[0].Size = checked((observableId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)bridgeIdBytes;
                    descrs[1].Size = checked((bridgeId.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)expressionBytes;
                    descrs[2].Size = checked((expression.Length + 1) * 2);

                    WriteEventCore(60, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceBridge_CreatingUpstreamObservable => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Bridge);

            [Event(61,
                Level = EventLevel.Informational,
                Keywords = Keywords.Bridge,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Creating upstream subscription '{0}' for bridge '{1}'.",
                Version = 1)]
            internal unsafe void Bridge_CreatingUpstreamSubscription(String subscriptionId, String bridgeId)
            {
                subscriptionId ??= string.Empty;
                bridgeId ??= string.Empty;
                fixed (char* subscriptionIdBytes = subscriptionId)
                fixed (char* bridgeIdBytes = bridgeId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[0].Size = checked((subscriptionId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)bridgeIdBytes;
                    descrs[1].Size = checked((bridgeId.Length + 1) * 2);

                    WriteEventCore(61, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceBridge_CreatingUpstreamSubscription => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Bridge);

            [Event(62,
                Level = EventLevel.Informational,
                Keywords = Keywords.Bridge,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Disposing upstream observable '{0}' for bridge '{1}'.",
                Version = 1)]
            internal unsafe void Bridge_DisposingUpstreamObservable(String observableId, String bridgeId)
            {
                observableId ??= string.Empty;
                bridgeId ??= string.Empty;
                fixed (char* observableIdBytes = observableId)
                fixed (char* bridgeIdBytes = bridgeId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)observableIdBytes;
                    descrs[0].Size = checked((observableId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)bridgeIdBytes;
                    descrs[1].Size = checked((bridgeId.Length + 1) * 2);

                    WriteEventCore(62, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceBridge_DisposingUpstreamObservable => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Bridge);

            [Event(63,
                Level = EventLevel.Informational,
                Keywords = Keywords.Bridge,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Disposing upstream subscription '{0}' for bridge '{1}'.",
                Version = 1)]
            internal unsafe void Bridge_DisposingUpstreamSubscription(String subscriptionId, String bridgeId)
            {
                subscriptionId ??= string.Empty;
                bridgeId ??= string.Empty;
                fixed (char* subscriptionIdBytes = subscriptionId)
                fixed (char* bridgeIdBytes = bridgeId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[0].Size = checked((subscriptionId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)bridgeIdBytes;
                    descrs[1].Size = checked((bridgeId.Length + 1) * 2);

                    WriteEventCore(63, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceBridge_DisposingUpstreamSubscription => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Bridge);

            [Event(64,
                Level = EventLevel.Informational,
                Keywords = Keywords.TransactionLog,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Recovery of query engine '{0}' induced a replay of a '{1}' operation for a(n) '{2}' artifact with id '{3}' from the transaction log.",
                Version = 2)]
            internal unsafe void Transaction_Log_Replay(String queryEngineId, String operation, String category, String entityId)
            {
                queryEngineId ??= string.Empty;
                operation ??= string.Empty;
                category ??= string.Empty;
                entityId ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                fixed (char* operationBytes = operation)
                fixed (char* categoryBytes = category)
                fixed (char* entityIdBytes = entityId)
                {
                    var dataCount = 4;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)operationBytes;
                    descrs[1].Size = checked((operation.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)categoryBytes;
                    descrs[2].Size = checked((category.Length + 1) * 2);

                    descrs[3].DataPointer = (IntPtr)entityIdBytes;
                    descrs[3].Size = checked((entityId.Length + 1) * 2);

                    WriteEventCore(64, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceTransaction_Log_Replay => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.TransactionLog);

            [Event(65,
                Level = EventLevel.Warning,
                Keywords = Keywords.TransactionLog,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Replay of a '{1}' operation for a(n) '{2}' artifact with id '{3}' from the transaction log on query engine '{0}' failed. {4}",
                Version = 3)]
            internal unsafe void Transaction_Log_Replay_Failure(String queryEngineId, String operation, String category, String entityId, String exception)
            {
                queryEngineId ??= string.Empty;
                operation ??= string.Empty;
                category ??= string.Empty;
                entityId ??= string.Empty;
                exception ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                fixed (char* operationBytes = operation)
                fixed (char* categoryBytes = category)
                fixed (char* entityIdBytes = entityId)
                fixed (char* exceptionBytes = exception)
                {
                    var dataCount = 5;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)operationBytes;
                    descrs[1].Size = checked((operation.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)categoryBytes;
                    descrs[2].Size = checked((category.Length + 1) * 2);

                    descrs[3].DataPointer = (IntPtr)entityIdBytes;
                    descrs[3].Size = checked((entityId.Length + 1) * 2);

                    descrs[4].DataPointer = (IntPtr)exceptionBytes;
                    descrs[4].Size = checked((exception.Length + 1) * 2);

                    WriteEventCore(65, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceTransaction_Log_Replay_Failure => Impl.Instance.IsEnabled(EventLevel.Warning, Keywords.TransactionLog);

            [Event(66,
                Level = EventLevel.Informational,
                Keywords = Keywords.TransactionLog,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Reclaimed '{1}' old versions of the transaction log on query engine '{0}'. Current version: '{2}', Active count: '{3}'.",
                Version = 2)]
            internal unsafe void Transaction_Log_Garbage_Collection(String queryEngineId, Int64 count, Int64 currentVersion, Int64 activeCount)
            {
                queryEngineId ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 4;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)(&count);
                    descrs[1].Size = sizeof(Int64);

                    descrs[2].DataPointer = (IntPtr)(&currentVersion);
                    descrs[2].Size = sizeof(Int64);

                    descrs[3].DataPointer = (IntPtr)(&activeCount);
                    descrs[3].Size = sizeof(Int64);

                    WriteEventCore(66, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceTransaction_Log_Garbage_Collection => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.TransactionLog);

            [Event(67,
                Level = EventLevel.Informational,
                Keywords = Keywords.TransactionLog,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Multiple occurences of artifact with id '{1}' appeared in the transaction log on query engine '{0}'. Coalesced state so far: '{2}', next state '{3}'.",
                Version = 2)]
            internal unsafe void Transaction_Log_Coalesce(String queryEngineId, String entityId, String coalescesStateSoFar, String nextState)
            {
                queryEngineId ??= string.Empty;
                entityId ??= string.Empty;
                coalescesStateSoFar ??= string.Empty;
                nextState ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                fixed (char* entityIdBytes = entityId)
                fixed (char* coalescesStateSoFarBytes = coalescesStateSoFar)
                fixed (char* nextStateBytes = nextState)
                {
                    var dataCount = 4;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)entityIdBytes;
                    descrs[1].Size = checked((entityId.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)coalescesStateSoFarBytes;
                    descrs[2].Size = checked((coalescesStateSoFar.Length + 1) * 2);

                    descrs[3].DataPointer = (IntPtr)nextStateBytes;
                    descrs[3].Size = checked((nextState.Length + 1) * 2);

                    WriteEventCore(67, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceTransaction_Log_Coalesce => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.TransactionLog);

            [Event(68,
                Level = EventLevel.Informational,
                Keywords = Keywords.TransactionLog,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Transaction log on query engine '{0}' initializing with latest version '{1}', active count '{2}', and held count '{3}'.",
                Version = 2)]
            internal unsafe void Transaction_Log_Initialization(String queryEngineId, Int64 latest, Int64 activeCount, Int64 heldCount)
            {
                queryEngineId ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 4;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)(&latest);
                    descrs[1].Size = sizeof(Int64);

                    descrs[2].DataPointer = (IntPtr)(&activeCount);
                    descrs[2].Size = sizeof(Int64);

                    descrs[3].DataPointer = (IntPtr)(&heldCount);
                    descrs[3].Size = sizeof(Int64);

                    WriteEventCore(68, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceTransaction_Log_Initialization => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.TransactionLog);

            [Event(69,
                Level = EventLevel.Informational,
                Keywords = Keywords.TransactionLog,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Transaction log on query engine '{0}' shapshotted with latest version '{1}', active count '{2}', and held count '{3}'.",
                Version = 2)]
            internal unsafe void Transaction_Log_Snapshot(String queryEngineId, Int64 latest, Int64 activeCount, Int64 heldCount)
            {
                queryEngineId ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 4;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)(&latest);
                    descrs[1].Size = sizeof(Int64);

                    descrs[2].DataPointer = (IntPtr)(&activeCount);
                    descrs[2].Size = sizeof(Int64);

                    descrs[3].DataPointer = (IntPtr)(&heldCount);
                    descrs[3].Size = sizeof(Int64);

                    WriteEventCore(69, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceTransaction_Log_Snapshot => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.TransactionLog);

            [Event(70,
                Level = EventLevel.Informational,
                Keywords = Keywords.TransactionLog,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Transaction log on query engine '{0}' unreferenced old version. Current latest version '{1}', active count '{2}', and held count '{3}'.",
                Version = 2)]
            internal unsafe void Transaction_Log_Lost_Reference(String queryEngineId, Int64 latest, Int64 activeCount, Int64 heldCount)
            {
                queryEngineId ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 4;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)(&latest);
                    descrs[1].Size = sizeof(Int64);

                    descrs[2].DataPointer = (IntPtr)(&activeCount);
                    descrs[2].Size = sizeof(Int64);

                    descrs[3].DataPointer = (IntPtr)(&heldCount);
                    descrs[3].Size = sizeof(Int64);

                    WriteEventCore(70, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceTransaction_Log_Lost_Reference => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.TransactionLog);

            [Event(71,
                Level = EventLevel.Informational,
                Keywords = Keywords.TransactionLog,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Transaction log on query engine '{0}' starting garbage collection. Current latest version '{1}', active count '{2}', and held count '{3}'.",
                Version = 2)]
            internal unsafe void Transaction_Log_Garbage_Collection_Start(String queryEngineId, Int64 latest, Int64 activeCount, Int64 heldCount)
            {
                queryEngineId ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 4;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)(&latest);
                    descrs[1].Size = sizeof(Int64);

                    descrs[2].DataPointer = (IntPtr)(&activeCount);
                    descrs[2].Size = sizeof(Int64);

                    descrs[3].DataPointer = (IntPtr)(&heldCount);
                    descrs[3].Size = sizeof(Int64);

                    WriteEventCore(71, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceTransaction_Log_Garbage_Collection_Start => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.TransactionLog);

            [Event(72,
                Level = EventLevel.Informational,
                Keywords = Keywords.TransactionLog,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Transaction log on query engine '{0}' finished garbage collection. Current latest version '{1}', active count '{2}', and held count '{3}'.",
                Version = 2)]
            internal unsafe void Transaction_Log_Garbage_Collection_End(String queryEngineId, Int64 latest, Int64 activeCount, Int64 heldCount)
            {
                queryEngineId ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 4;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)(&latest);
                    descrs[1].Size = sizeof(Int64);

                    descrs[2].DataPointer = (IntPtr)(&activeCount);
                    descrs[2].Size = sizeof(Int64);

                    descrs[3].DataPointer = (IntPtr)(&heldCount);
                    descrs[3].Size = sizeof(Int64);

                    WriteEventCore(72, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceTransaction_Log_Garbage_Collection_End => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.TransactionLog);

            [Event(73,
                Level = EventLevel.Error,
                Keywords = Keywords.TransactionLog,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Transaction log on query engine '{0}' garbage collection failed. {1}",
                Version = 3)]
            internal unsafe void Transaction_Log_Garbage_Collection_Failed(String queryEngineId, String exception)
            {
                queryEngineId ??= string.Empty;
                exception ??= string.Empty;
                fixed (char* queryEngineIdBytes = queryEngineId)
                fixed (char* exceptionBytes = exception)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[0].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)exceptionBytes;
                    descrs[1].Size = checked((exception.Length + 1) * 2);

                    WriteEventCore(73, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceTransaction_Log_Garbage_Collection_Failed => Impl.Instance.IsEnabled(EventLevel.Error, Keywords.TransactionLog);

            [Event(74,
                Level = EventLevel.Error,
                Keywords = Keywords.TransactionLog,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Creation of artifact '{0}' failed after adding to engine and transaction log. This is most likely due to an error in disposing the transaction. Exception: '{1}'.",
                Version = 1)]
            internal unsafe void Create_Artifact_Unexpected_Transaction_Disposal_Exception(String entityId, String exception)
            {
                entityId ??= string.Empty;
                exception ??= string.Empty;
                fixed (char* entityIdBytes = entityId)
                fixed (char* exceptionBytes = exception)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)entityIdBytes;
                    descrs[0].Size = checked((entityId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)exceptionBytes;
                    descrs[1].Size = checked((exception.Length + 1) * 2);

                    WriteEventCore(74, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceCreate_Artifact_Unexpected_Transaction_Disposal_Exception => Impl.Instance.IsEnabled(EventLevel.Error, Keywords.TransactionLog);

            [Event(75,
                Level = EventLevel.Informational,
                Keywords = Keywords.BlobLogs,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Recovery state blob log finished writing to '{0}' on query engine '{1}'.",
                Version = 1)]
            internal unsafe void BlobLogs_Done_Success(String path, String queryEngineId)
            {
                path ??= string.Empty;
                queryEngineId ??= string.Empty;
                fixed (char* pathBytes = path)
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)pathBytes;
                    descrs[0].Size = checked((path.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[1].Size = checked((queryEngineId.Length + 1) * 2);

                    WriteEventCore(75, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceBlobLogs_Done_Success => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.BlobLogs);

            [Event(76,
                Level = EventLevel.Informational,
                Keywords = Keywords.BlobLogs,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Recovery state blob log canceled writing to '{0}' on query engine '{1}'.",
                Version = 1)]
            internal unsafe void BlobLogs_Done_Canceled(String path, String queryEngineId)
            {
                path ??= string.Empty;
                queryEngineId ??= string.Empty;
                fixed (char* pathBytes = path)
                fixed (char* queryEngineIdBytes = queryEngineId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)pathBytes;
                    descrs[0].Size = checked((path.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[1].Size = checked((queryEngineId.Length + 1) * 2);

                    WriteEventCore(76, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceBlobLogs_Done_Canceled => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.BlobLogs);

            [Event(77,
                Level = EventLevel.Warning,
                Keywords = Keywords.BlobLogs,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Recovery state blob log failed writing to '{0}' on query engine '{1}'. Error = {2}",
                Version = 1)]
            internal unsafe void BlobLogs_Done_Error(String path, String queryEngineId, String error)
            {
                path ??= string.Empty;
                queryEngineId ??= string.Empty;
                error ??= string.Empty;
                fixed (char* pathBytes = path)
                fixed (char* queryEngineIdBytes = queryEngineId)
                fixed (char* errorBytes = error)
                {
                    var dataCount = 3;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)pathBytes;
                    descrs[0].Size = checked((path.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)queryEngineIdBytes;
                    descrs[1].Size = checked((queryEngineId.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)errorBytes;
                    descrs[2].Size = checked((error.Length + 1) * 2);

                    WriteEventCore(77, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceBlobLogs_Done_Error => Impl.Instance.IsEnabled(EventLevel.Warning, Keywords.BlobLogs);

            [Event(78,
                Level = EventLevel.Informational,
                Keywords = Keywords.HigherOrderOperator,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Creating bridge '{0}' for subscription '{1}'. Definition = '{2}'.",
                Version = 1)]
            internal unsafe void HigherOrderOperator_CreatingBridge(String bridgeId, String subscriptionId, String definition)
            {
                bridgeId ??= string.Empty;
                subscriptionId ??= string.Empty;
                definition ??= string.Empty;
                fixed (char* bridgeIdBytes = bridgeId)
                fixed (char* subscriptionIdBytes = subscriptionId)
                fixed (char* definitionBytes = definition)
                {
                    var dataCount = 3;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)bridgeIdBytes;
                    descrs[0].Size = checked((bridgeId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[1].Size = checked((subscriptionId.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)definitionBytes;
                    descrs[2].Size = checked((definition.Length + 1) * 2);

                    WriteEventCore(78, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceHigherOrderOperator_CreatingBridge => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.HigherOrderOperator);

            [Event(79,
                Level = EventLevel.Informational,
                Keywords = Keywords.HigherOrderOperator,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Starting inner subscription to bridge '{0}' for higher order subscription '{1}'.",
                Version = 1)]
            internal unsafe void HigherOrderOperator_StartingInnerSubscription(String bridgeId, String subscriptionId)
            {
                bridgeId ??= string.Empty;
                subscriptionId ??= string.Empty;
                fixed (char* bridgeIdBytes = bridgeId)
                fixed (char* subscriptionIdBytes = subscriptionId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)bridgeIdBytes;
                    descrs[0].Size = checked((bridgeId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[1].Size = checked((subscriptionId.Length + 1) * 2);

                    WriteEventCore(79, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceHigherOrderOperator_StartingInnerSubscription => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.HigherOrderOperator);

            [Event(80,
                Level = EventLevel.Informational,
                Keywords = Keywords.HigherOrderOperator,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Disposing inner subscription to bridge '{0}' for higher order subscription '{1}'.",
                Version = 1)]
            internal unsafe void HigherOrderOperator_DisposingInnerSubscription(String bridgeId, String subscriptionId)
            {
                bridgeId ??= string.Empty;
                subscriptionId ??= string.Empty;
                fixed (char* bridgeIdBytes = bridgeId)
                fixed (char* subscriptionIdBytes = subscriptionId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)bridgeIdBytes;
                    descrs[0].Size = checked((bridgeId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[1].Size = checked((subscriptionId.Length + 1) * 2);

                    WriteEventCore(80, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceHigherOrderOperator_DisposingInnerSubscription => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.HigherOrderOperator);

            [Event(81,
                Level = EventLevel.Informational,
                Keywords = Keywords.HigherOrderOperator,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Removing stream for bridge '{0}' for higher order subscription '{1}'.",
                Version = 1)]
            internal unsafe void HigherOrderOperator_DeletingBridge(String bridgeId, String subscriptionId)
            {
                bridgeId ??= string.Empty;
                subscriptionId ??= string.Empty;
                fixed (char* bridgeIdBytes = bridgeId)
                fixed (char* subscriptionIdBytes = subscriptionId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)bridgeIdBytes;
                    descrs[0].Size = checked((bridgeId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[1].Size = checked((subscriptionId.Length + 1) * 2);

                    WriteEventCore(81, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceHigherOrderOperator_DeletingBridge => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.HigherOrderOperator);

            [Event(82,
                Level = EventLevel.Warning,
                Keywords = Keywords.Edge,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Deleting external subscription '{0}' on input edge '{1}' failed. Exception: '{2}'.",
                Version = 1)]
            internal unsafe void InputEdge_ExternalSubscription_Dispose_Failed(String externalSubscriptionId, String edgeId, String exception)
            {
                externalSubscriptionId ??= string.Empty;
                edgeId ??= string.Empty;
                exception ??= string.Empty;
                fixed (char* externalSubscriptionIdBytes = externalSubscriptionId)
                fixed (char* edgeIdBytes = edgeId)
                fixed (char* exceptionBytes = exception)
                {
                    var dataCount = 3;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)externalSubscriptionIdBytes;
                    descrs[0].Size = checked((externalSubscriptionId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)edgeIdBytes;
                    descrs[1].Size = checked((edgeId.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)exceptionBytes;
                    descrs[2].Size = checked((exception.Length + 1) * 2);

                    WriteEventCore(82, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceInputEdge_ExternalSubscription_Dispose_Failed => Impl.Instance.IsEnabled(EventLevel.Warning, Keywords.Edge);
        }

        private static string ToTraceString<T>(this T obj) => obj?.ToString();
    }
}



