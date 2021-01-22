// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive;
using Reaqtive.Scheduler;

namespace Reaqtor.Remoting.Deployable
{
    public class ConsoleObserver<T> : Observer<T>
    {
        private readonly string _prefix;
        private IScheduler _scheduler;
        private Uri _id;

        public ConsoleObserver()
        {
            _prefix = "";
        }

        public ConsoleObserver(string prefix)
        {
            _prefix = "(" + prefix + ")";
        }

        public override void SetContext(IOperatorContext context)
        {
            _scheduler = context.Scheduler;
            _id = context.InstanceId;

            base.SetContext(context);
        }

        protected override void OnCompletedCore()
        {
            Console.WriteLine("{0:o}> {2} ConsoleObserver{1}.OnCompleted()", _scheduler.Now, _prefix, _id);
        }

        protected override void OnErrorCore(Exception error)
        {
            Console.WriteLine("{0:o}> {3} ConsoleObserver{1}.OnError({2})", _scheduler.Now, _prefix, error, _id);
        }

        protected override void OnNextCore(T value)
        {
            Console.WriteLine("{0:o}> {3} ConsoleObserver{1}.OnNext({2})", _scheduler.Now, _prefix, value, _id);
        }
    }
}
