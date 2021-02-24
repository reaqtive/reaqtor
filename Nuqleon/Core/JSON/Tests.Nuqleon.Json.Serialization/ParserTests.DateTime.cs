// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/05/2016 - Created fast JSON deserializer functionality.
//

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nuqleon.Json.Serialization;

namespace Tests
{
    public partial class ParserTests
    {
        [TestMethod]
        public void FastParser_DateTime()
        {
            AssertParse(Parser.ParseDateTime, Literal("2016-02-11T12:34:56Z"), new DateTime(2016, 2, 11, 12, 34, 56));
            AssertParse(Parser.ParseDateTime, Literal("2016-02-11t12:34:56Z"), new DateTime(2016, 2, 11, 12, 34, 56));
            AssertParse(Parser.ParseDateTime, Literal("2016-02-11T12:34:56.789Z"), new DateTime(2016, 2, 11, 12, 34, 56, 789));

            for (var year = 1970; year <= 2070; year++)
            {
                AssertParse(Parser.ParseDateTime, Literal(year + "-02-11T12:34:56.789Z"), new DateTime(year, 2, 11, 12, 34, 56, 789));
            }

            for (var month = 1; month <= 12; month++)
            {
                AssertParse(Parser.ParseDateTime, Literal("2016-" + month.ToString().PadLeft(2, '0') + "-11T12:34:56.789Z"), new DateTime(2016, month, 11, 12, 34, 56, 789));
            }

            for (var day = 1; day <= 31; day++)
            {
                AssertParse(Parser.ParseDateTime, Literal("2016-03-" + day.ToString().PadLeft(2, '0') + "T12:34:56.789Z"), new DateTime(2016, 03, day, 12, 34, 56, 789));
            }

            for (var hour = 0; hour <= 23; hour++)
            {
                AssertParse(Parser.ParseDateTime, Literal("2016-02-11T" + hour.ToString().PadLeft(2, '0') + ":34:56.789Z"), new DateTime(2016, 02, 11, hour, 34, 56, 789));
            }

            for (var minute = 0; minute <= 59; minute++)
            {
                AssertParse(Parser.ParseDateTime, Literal("2016-02-11T12:" + minute.ToString().PadLeft(2, '0') + ":56.789Z"), new DateTime(2016, 02, 11, 12, minute, 56, 789));
            }

            for (var second = 0; second <= 59; second++)
            {
                AssertParse(Parser.ParseDateTime, Literal("2016-02-11T12:34:" + second.ToString().PadLeft(2, '0') + ".789Z"), new DateTime(2016, 02, 11, 12, 34, second, 789));
            }

            for (var millisecond = 0; millisecond <= 999; millisecond++)
            {
                AssertParse(Parser.ParseDateTime, Literal("2016-02-11T12:34:56." + millisecond.ToString() + "Z"), new DateTime(2016, 02, 11, 12, 34, 56, int.Parse(millisecond.ToString().PadRight(3, '0'))));
            }

            AssertParseFail(Parser.ParseDateTime, "null");

            AssertParseFail(Parser.ParseDateTime, "");

            // no leap second support
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:34:60Z\"");

            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:34:56+08:00");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:34:56+08:0");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:34:56+08:");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:34:56+08");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:34:56+0");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:34:56+");

            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:34:56.789");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:34:56.\"");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:34:56.");

            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:34:56Z");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:34:56");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:34:5");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:34:");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:34");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:3");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T1");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-1");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02");
            AssertParseFail(Parser.ParseDateTime, "\"2016-0");
            AssertParseFail(Parser.ParseDateTime, "\"2016-");
            AssertParseFail(Parser.ParseDateTime, "\"2016");
            AssertParseFail(Parser.ParseDateTime, "\"201");
            AssertParseFail(Parser.ParseDateTime, "\"20");
            AssertParseFail(Parser.ParseDateTime, "\"2");
            AssertParseFail(Parser.ParseDateTime, "\"");

            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:34:56+08:0X\"");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:34:56+08:X0\"");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:34:56+08X00\"");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:34:56+0X:00\"");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:34:56+X8:00\"");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:34:56X08:00\"");

            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:34:56.78X\"");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:34:56.7X9\"");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:34:56.X89\"");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:34:56X789\"");

            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:34:56X\"");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:34:5XZ\"");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:34:X6Z\"");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:34X56Z\"");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:3X:56Z\"");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12:X4:56Z\"");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T12X34:56Z\"");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11T1X:34:56Z\"");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11TX2:34:56Z\"");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-11X12:34:56Z\"");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-1XT12:34:56Z\"");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02-X1T12:34:56Z\"");
            AssertParseFail(Parser.ParseDateTime, "\"2016-02X11T12:34:56Z\"");
            AssertParseFail(Parser.ParseDateTime, "\"2016-0X-11T12:34:56Z\"");
            AssertParseFail(Parser.ParseDateTime, "\"2016-X2-11T12:34:56Z\"");
            AssertParseFail(Parser.ParseDateTime, "\"2016X02-11T12:34:56Z\"");
            AssertParseFail(Parser.ParseDateTime, "\"201X-02-11T12:34:56Z\"");
            AssertParseFail(Parser.ParseDateTime, "\"20X6-02-11T12:34:56Z\"");
            AssertParseFail(Parser.ParseDateTime, "\"2X16-02-11T12:34:56Z\"");
            AssertParseFail(Parser.ParseDateTime, "\"X016-02-11T12:34:56Z\"");

            AssertParseFail(Parser.ParseDateTime, "true");

            AssertParseFail(Parser.ParseDateTime, "42");
            AssertParseFail(Parser.ParseDateTime, "-42");

            AssertParseFail(Parser.ParseDateTime, "[");
            AssertParseFail(Parser.ParseDateTime, "]");
            AssertParseFail(Parser.ParseDateTime, "{");
            AssertParseFail(Parser.ParseDateTime, "}");
            AssertParseFail(Parser.ParseDateTime, ",");
            AssertParseFail(Parser.ParseDateTime, ":");

            AssertParseFail(Parser.ParseDateTime, "null");
        }

        [TestMethod]
        public void FastParser_NullableDateTime()
        {
            AssertParse(Parser.ParseNullableDateTime, "null", null);

            AssertParse(Parser.ParseNullableDateTime, Literal("2016-02-11T12:34:56Z"), new DateTime(2016, 2, 11, 12, 34, 56));
            AssertParse(Parser.ParseNullableDateTime, Literal("2016-02-11t12:34:56Z"), new DateTime(2016, 2, 11, 12, 34, 56));
            AssertParse(Parser.ParseNullableDateTime, Literal("2016-02-11T12:34:56.789Z"), new DateTime(2016, 2, 11, 12, 34, 56, 789));

            AssertParseFail(Parser.ParseNullableDateTime, "");
            AssertParseFail(Parser.ParseNullableDateTime, "n");
            AssertParseFail(Parser.ParseNullableDateTime, "nu");
            AssertParseFail(Parser.ParseNullableDateTime, "nul");

            AssertParseFail(Parser.ParseNullableDateTime, "Null");
            AssertParseFail(Parser.ParseNullableDateTime, "nUll");
            AssertParseFail(Parser.ParseNullableDateTime, "nuLl");
            AssertParseFail(Parser.ParseNullableDateTime, "nulL");
        }

        [TestMethod]
        public void FastParser_DateTimeOffset()
        {
            AssertParse(Parser.ParseDateTimeOffset, Literal("2016-02-11T12:34:56+08:00"), new DateTimeOffset(2016, 2, 11, 12, 34, 56, new TimeSpan(8, 0, 0)));
            AssertParse(Parser.ParseDateTimeOffset, Literal("2016-02-11t12:34:56+08:00"), new DateTimeOffset(2016, 2, 11, 12, 34, 56, new TimeSpan(8, 0, 0)));
            AssertParse(Parser.ParseDateTimeOffset, Literal("2016-02-11T12:34:56-05:30"), new DateTimeOffset(2016, 2, 11, 12, 34, 56, -new TimeSpan(5, 30, 0)));
            AssertParse(Parser.ParseDateTimeOffset, Literal("2016-02-11t12:34:56-05:30"), new DateTimeOffset(2016, 2, 11, 12, 34, 56, -new TimeSpan(5, 30, 0)));
            AssertParse(Parser.ParseDateTimeOffset, Literal("2016-02-11T12:34:56.789+08:00"), new DateTimeOffset(2016, 2, 11, 12, 34, 56, 789, new TimeSpan(8, 0, 0)));
            AssertParse(Parser.ParseDateTimeOffset, Literal("2016-02-11T12:34:56.789-05:30"), new DateTimeOffset(2016, 2, 11, 12, 34, 56, 789, -new TimeSpan(5, 30, 0)));

            AssertParseFail(Parser.ParseDateTimeOffset, "null");
        }

        [TestMethod]
        public void FastParser_NullableDateTimeOffset()
        {
            AssertParse(Parser.ParseNullableDateTimeOffset, "null", null);

            AssertParse(Parser.ParseNullableDateTimeOffset, Literal("2016-02-11T12:34:56+08:00"), new DateTimeOffset(2016, 2, 11, 12, 34, 56, new TimeSpan(8, 0, 0)));
            AssertParse(Parser.ParseNullableDateTimeOffset, Literal("2016-02-11t12:34:56+08:00"), new DateTimeOffset(2016, 2, 11, 12, 34, 56, new TimeSpan(8, 0, 0)));
            AssertParse(Parser.ParseNullableDateTimeOffset, Literal("2016-02-11T12:34:56-05:30"), new DateTimeOffset(2016, 2, 11, 12, 34, 56, -new TimeSpan(5, 30, 0)));
            AssertParse(Parser.ParseNullableDateTimeOffset, Literal("2016-02-11t12:34:56-05:30"), new DateTimeOffset(2016, 2, 11, 12, 34, 56, -new TimeSpan(5, 30, 0)));
            AssertParse(Parser.ParseNullableDateTimeOffset, Literal("2016-02-11T12:34:56.789+08:00"), new DateTimeOffset(2016, 2, 11, 12, 34, 56, 789, new TimeSpan(8, 0, 0)));
            AssertParse(Parser.ParseNullableDateTimeOffset, Literal("2016-02-11T12:34:56.789-05:30"), new DateTimeOffset(2016, 2, 11, 12, 34, 56, 789, -new TimeSpan(5, 30, 0)));

            AssertParseFail(Parser.ParseNullableDateTimeOffset, "");
            AssertParseFail(Parser.ParseNullableDateTimeOffset, "n");
            AssertParseFail(Parser.ParseNullableDateTimeOffset, "nu");
            AssertParseFail(Parser.ParseNullableDateTimeOffset, "nul");

            AssertParseFail(Parser.ParseNullableDateTimeOffset, "Null");
            AssertParseFail(Parser.ParseNullableDateTimeOffset, "nUll");
            AssertParseFail(Parser.ParseNullableDateTimeOffset, "nuLl");
            AssertParseFail(Parser.ParseNullableDateTimeOffset, "nulL");
        }
    }
}
