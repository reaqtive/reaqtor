// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/16/2016 - Created fast JSON serializer functionality.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nuqleon.Json.Serialization;
using System;

namespace Tests
{
    public partial class EmitterTests
    {
        [TestMethod]
        public void FastEmitter_DateTime()
        {
            AssertEmit(Emitter.EmitDateTime, new DateTime(1983, 2, 11, 12, 34, 56, DateTimeKind.Utc), "\"1983-02-11T12:34:56Z\"");
            AssertEmit(Emitter.EmitDateTime, new DateTime(1983, 2, 11, 12, 34, 56, 789, DateTimeKind.Utc), "\"1983-02-11T12:34:56.789Z\"");
        }

        [TestMethod]
        public void FastEmitter_NullableDateTime()
        {
            AssertEmit<DateTime?>(Emitter.EmitNullableDateTime, null, "null");

            AssertEmit<DateTime?>(Emitter.EmitNullableDateTime, new DateTime(1983, 2, 11, 12, 34, 56, DateTimeKind.Utc), "\"1983-02-11T12:34:56Z\"");
            AssertEmit<DateTime?>(Emitter.EmitNullableDateTime, new DateTime(1983, 2, 11, 12, 34, 56, 789, DateTimeKind.Utc), "\"1983-02-11T12:34:56.789Z\"");
        }

        [TestMethod]
        public void FastEmitter_DateTimeOffset()
        {
            AssertEmit(Emitter.EmitDateTimeOffset, new DateTimeOffset(1983, 2, 11, 12, 34, 56, TimeSpan.Zero), "\"1983-02-11T12:34:56Z\"");
            AssertEmit(Emitter.EmitDateTimeOffset, new DateTimeOffset(1983, 2, 11, 12, 34, 56, 789, TimeSpan.Zero), "\"1983-02-11T12:34:56.789Z\"");

            AssertEmit(Emitter.EmitDateTimeOffset, new DateTimeOffset(1983, 2, 11, 12, 34, 56, TimeSpan.FromHours(8)), "\"1983-02-11T12:34:56+08:00\"");
            AssertEmit(Emitter.EmitDateTimeOffset, new DateTimeOffset(1983, 2, 11, 12, 34, 56, 789, TimeSpan.FromHours(8)), "\"1983-02-11T12:34:56.789+08:00\"");

            AssertEmit(Emitter.EmitDateTimeOffset, new DateTimeOffset(1983, 2, 11, 12, 34, 56, TimeSpan.FromMinutes(45)), "\"1983-02-11T12:34:56+00:45\"");
            AssertEmit(Emitter.EmitDateTimeOffset, new DateTimeOffset(1983, 2, 11, 12, 34, 56, 789, TimeSpan.FromMinutes(45)), "\"1983-02-11T12:34:56.789+00:45\"");

            AssertEmit(Emitter.EmitDateTimeOffset, new DateTimeOffset(1983, 2, 11, 12, 34, 56, -TimeSpan.FromHours(11)), "\"1983-02-11T12:34:56-11:00\"");
            AssertEmit(Emitter.EmitDateTimeOffset, new DateTimeOffset(1983, 2, 11, 12, 34, 56, 789, -TimeSpan.FromHours(11)), "\"1983-02-11T12:34:56.789-11:00\"");
        }

        [TestMethod]
        public void FastEmitter_NullableDateTimeOffset()
        {
            AssertEmit<DateTimeOffset?>(Emitter.EmitNullableDateTimeOffset, null, "null");

            AssertEmit<DateTimeOffset?>(Emitter.EmitNullableDateTimeOffset, new DateTimeOffset(1983, 2, 11, 12, 34, 56, TimeSpan.Zero), "\"1983-02-11T12:34:56Z\"");
            AssertEmit<DateTimeOffset?>(Emitter.EmitNullableDateTimeOffset, new DateTimeOffset(1983, 2, 11, 12, 34, 56, 789, TimeSpan.Zero), "\"1983-02-11T12:34:56.789Z\"");

            AssertEmit<DateTimeOffset?>(Emitter.EmitNullableDateTimeOffset, new DateTimeOffset(1983, 2, 11, 12, 34, 56, TimeSpan.FromHours(8)), "\"1983-02-11T12:34:56+08:00\"");
            AssertEmit<DateTimeOffset?>(Emitter.EmitNullableDateTimeOffset, new DateTimeOffset(1983, 2, 11, 12, 34, 56, 789, TimeSpan.FromHours(8)), "\"1983-02-11T12:34:56.789+08:00\"");

            AssertEmit<DateTimeOffset?>(Emitter.EmitNullableDateTimeOffset, new DateTimeOffset(1983, 2, 11, 12, 34, 56, TimeSpan.FromMinutes(45)), "\"1983-02-11T12:34:56+00:45\"");
            AssertEmit<DateTimeOffset?>(Emitter.EmitNullableDateTimeOffset, new DateTimeOffset(1983, 2, 11, 12, 34, 56, 789, TimeSpan.FromMinutes(45)), "\"1983-02-11T12:34:56.789+00:45\"");

            AssertEmit<DateTimeOffset?>(Emitter.EmitNullableDateTimeOffset, new DateTimeOffset(1983, 2, 11, 12, 34, 56, -TimeSpan.FromHours(11)), "\"1983-02-11T12:34:56-11:00\"");
            AssertEmit<DateTimeOffset?>(Emitter.EmitNullableDateTimeOffset, new DateTimeOffset(1983, 2, 11, 12, 34, 56, 789, -TimeSpan.FromHours(11)), "\"1983-02-11T12:34:56.789-11:00\"");
        }
    }
}
