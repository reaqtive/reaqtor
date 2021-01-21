// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2014
//

#pragma warning disable CS0618 // NB: Ignore the Obsolete attribute on TryFilter for testing purposes.

using System;
using System.Collections.Generic;
using Nuqleon.Runtime.ExceptionServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Nuqleon.Runtime.ExceptionServices
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void TryFault_NoError()
        {
            Helpers.TryFault(
                () => { },
                () => { Assert.Fail(); }
            );
        }

        [TestMethod]
        public void TryFault_Error()
        {
            var faulted = false;

            var ex = new Exception("Oops!");

            try
            {
                Helpers.TryFault(
                    () => { throw ex; },
                    () => { faulted = true; }
                );

                Assert.Fail();
            }
            catch (Exception err)
            {
                Assert.IsTrue(faulted);
                Assert.AreSame(ex, err);
            }
        }

        [TestMethod]
        public void TryFault_HandlerThrows()
        {
            var ex = new Exception("Oops, ");
            var xe = new Exception("I did it again!");

            try
            {
                Helpers.TryFault(
                    () => { throw ex; },
                    () => { throw xe; }
                );

                Assert.Fail();
            }
            catch (Exception err)
            {
                Assert.AreSame(xe, err);
            }
        }

        [TestMethod]
        public void TryFilter_NoError()
        {
            Helpers.TryFilter<Exception>(
                () => { },
                ex => { Assert.Fail(); return false; },
                ex => { Assert.Fail(); }
            );
        }

        [TestMethod]
        public void TryFilter_Error_Filtered()
        {
            var filtered = false;
            var handled = false;

            var ae = new ArgumentException("myArg");

            Helpers.TryFilter<ArgumentException>(
                () => { throw ae; },
                ex => { Assert.AreSame(ae, ex); filtered = true; return true; },
                ex => { Assert.AreSame(ae, ex); handled = true; }
            );

            Assert.IsTrue(filtered && handled);
        }

        [TestMethod]
        public void TryFilter_Error_NotFiltered_Type()
        {
            var filtered = false;
            var handled = false;
            var failed = false;

            try
            {
                Helpers.TryFilter<ArgumentException>(
                    () => { Console.WriteLine(1 / "".Length); },
                    ex => { filtered = true; return true; },
                    ex => { handled = true; }
                );

                Assert.Fail();
            }
            catch (DivideByZeroException)
            {
                failed = true;
            }

            Assert.IsTrue(failed);
            Assert.IsTrue(!filtered && !handled);
        }

        [TestMethod]
        public void TryFilter_Error_NotFiltered_Condition()
        {
            var filtered = false;
            var handled = false;

            var ae = new ArgumentException("myArg");

            try
            {
                Helpers.TryFilter<ArgumentException>(
                    () => { throw ae; },
                    ex => { Assert.AreSame(ae, ex); filtered = true; return ex.ParamName == "hisArg"; },
                    ex => { handled = true; }
                );

                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.AreSame(ae, ex);
            }

            Assert.IsTrue(filtered);
            Assert.IsFalse(handled);
        }

        [TestMethod]
        public void TryFilter_FilterThrows()
        {
            var ae = new ArgumentException("myArg");
            var up = new Exception("up");

            var handled = false;

            Helpers.TryFilter<Exception>(
                () =>
                {
                    Helpers.TryFilter<ArgumentException>(
                        () => { throw ae; },
                        ex => { throw up; },
                        ex => { Assert.Fail(); }
                    );
                },
                ex => ex == ae,
                ex =>
                {
                    handled = true;
                }
            );

            Assert.IsTrue(handled);
        }

        [TestMethod]
        public void TryFilter_Handlerhrows()
        {
            var ae = new ArgumentException("myArg");
            var up = new Exception("up");

            var handled = false;

            try
            {
                Helpers.TryFilter<ArgumentException>(
                    () =>
                    {
                        throw ae;
                    },
                    ex => ex == ae,
                    ex =>
                    {
                        handled = true;
                        throw up;
                    }
                );
            }
            catch (Exception ex)
            {
                Assert.AreSame(up, ex);
            }

            Assert.IsTrue(handled);
        }

        [TestMethod]
        public void TryEverything_Climax()
        {
            var log = new List<string>();

                                                           Helpers.TryFilter<Exception>(
            @try: () =>
            {
                log.Add("A");
                try
                {
                    log.Add("B");
                                                               Helpers.TryFault(
                    @try: () =>
                    {
                        log.Add("C");
                        throw new Exception("Oops!");
                    },
                    @fault: () =>
                    {
                        log.Add("D");
                    }
                                                               );
                }
                finally
                {
                    log.Add("E");
                }
            },
            @catch: ex =>
            {
                log.Add("F");
            },
            @if: ex =>
            {
                log.Add("G");
                return ex.Message.Contains("Oops");
            }
                                                           );

            var res = string.Join("", log);

            Assert.AreEqual("ABCGDEF", res);
        }

        [TestMethod]
        public void TryEverything_AntiClimax()
        {
            var log = new List<string>();

            try
            {
                log.Add("0");
                                                           Helpers.TryFilter<Exception>(
                @try: () =>
                {
                    log.Add("A");
                    try
                    {
                        log.Add("B");
                                                               Helpers.TryFault(
                        @try: () =>
                        {
                            log.Add("C");
                            throw new Exception("!spoO");
                        },
                        @fault: () =>
                        {
                            log.Add("D");
                        }
                                                               );
                    }
                    finally
                    {
                        log.Add("E");
                    }
                },
                @catch: ex =>
                {
                    log.Add("F");
                },
                @if: ex =>
                {
                    log.Add("G");
                    return ex.Message.Contains("Oops");
                }
                                                           );
            }
            catch
            {
                log.Add("Z");
            }

            var res = string.Join("", log);

            Assert.AreEqual("0ABCGDEZ", res);
        }

        [TestMethod]
        public void TryEverything_WhoWillHandleIt()
        {
            var log = new List<string>();

            var hot = new Exception("potato");


                                                           Helpers.TryFilter<Exception>(
            @try: () =>
            {
                log.Add("A");
                                                               Helpers.TryFault(
                @try: () =>
                {
                    log.Add("B");
                                                                   Helpers.TryFilter<Exception>(
                    @try: () =>
                    {
                        log.Add("C");
                                                                       Helpers.TryFault(
                        @try: () =>
                        {
                            log.Add("D");
                            try
                            {
                                log.Add("E");
                                throw hot;
                            }
                            finally
                            {
                                log.Add("F");
                            }
                        },
                        @fault: () =>
                        {
                            log.Add("G");
                        }
                                                                       );
                    },
                    @catch: ex =>
                    {
                        log.Add("H");
                    },
                    @if: ex =>
                    {
                        log.Add("I");
                        return ex.Message.Contains("tomato");
                    }
                                                                   );
                },
                @fault: () =>
                {
                    log.Add("J");
                }
                                                               );
            },
            @catch: ex =>
            {
                log.Add("K");
            },
            @if: ex =>
            {
                log.Add("L");
                return ex.Message.Contains("potato");
            }
                                                           );

            var res = string.Join("", log);
            Assert.AreEqual("ABCDEILFGJK", res);
        }
    }
}