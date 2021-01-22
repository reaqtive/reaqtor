// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Globalization;

using Reaqtive;
using Reaqtive.Scheduler;

using Reaqtor.Hosting.Shared.Serialization;

namespace Reaqtor.Remoting.Deployable
{
    public class TraceObserver<T> : StatefulObserver<T>
    {
        private readonly string _prefix;
        private IScheduler _scheduler;
        private Uri _id;
        private TraceSource _tracer;

        public TraceObserver()
        {
            _prefix = "";
        }

        public TraceObserver(string prefix)
        {
            _prefix = "(" + prefix + ")";
        }

        public override string Name => "rcr:Trace/v";

        public override Version Version => new(1, 0, 0, 0);

        public override void SetContext(IOperatorContext context)
        {
            _scheduler = context.Scheduler;
            _id = context.InstanceId;
            _tracer = context.TraceSource;

            base.SetContext(context);
        }

        protected override void LoadStateCore(IOperatorStateReader reader)
        {
            _tracer.TraceInformation(string.Format(CultureInfo.InvariantCulture, "{0:o}> {2} TraceObserver{1}.LoadStateCore()", _scheduler.Now, _prefix, _id));
        }

        protected override void SaveStateCore(IOperatorStateWriter writer)
        {
            _tracer.TraceInformation(string.Format(CultureInfo.InvariantCulture, "{0:o}> {2} TraceObserver{1}.SaveStateCore()", _scheduler.Now, _prefix, _id));
        }

        protected override void OnStart()
        {
            _tracer.TraceInformation(string.Format(CultureInfo.InvariantCulture, "{0:o}> {2} TraceObserver{1}.OnStart()", _scheduler.Now, _prefix, _id));
            base.OnStart();
        }

        protected override void OnDispose()
        {
            _tracer.TraceInformation(string.Format(CultureInfo.InvariantCulture, "{0:o}> {2} TraceObserver{1}.OnDispose()", _scheduler.Now, _prefix, _id));
            base.OnDispose();
        }

        protected override void OnCompletedCore()
        {
            _tracer.TraceInformation(string.Format(CultureInfo.InvariantCulture, "{0:o}> {2} TraceObserver{1}.OnCompleted()", _scheduler.Now, _prefix, _id));
        }

        protected override void OnErrorCore(Exception error)
        {
            _tracer.TraceInformation(string.Format(CultureInfo.InvariantCulture, "{0:o}> {3} TraceObserver{1}.OnError({2})", _scheduler.Now, _prefix, error, _id));
        }

        protected override void OnNextCore(T value)
        {
            var helpers = new SerializationHelpers();
            _tracer.TraceInformation(string.Format(CultureInfo.InvariantCulture, "{0:o}> {3} TraceObserver{1}.OnNext({2})", _scheduler.Now, _prefix, helpers.Serialize<T>(value), _id));
        }
    }
}
