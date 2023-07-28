// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Expressions.Tests;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ExpressionJit
{
    // REVIEW: Runtime generated thunk types require delegate types to be public.
    public delegate TResult F<TInput, TResult>(TInput t);

    public static class Program
    {
        public static void Main()
        {
            {
                Expression<F<string, int>> f = s => s.Length;
                Test(f, d => d("bar"));
            }

            {
                Expression<Func<int>> f = () => 42;
                Test(f, d => d());
            }

            {
                Expression<Func<int, int>> f = x => x;
                Test(f, d => d(42));
            }

            {
                Expression<Func<int, Func<int>>> f = x => () => x;
                Test(f, d => d(42)());
            }

            {
                Expression<Func<int, Func<int, int>>> f = x => y => x * y;
                Test(f, d => d(21)(2));
            }

            {
                Expression<Func<IEnumerable<int>, int, IEnumerable<int>>> f = (xs, a) => xs.Where(x => x > a).Select(x => x * x);
                Test(f, d => string.Join(", ", d(new[] { -2, -1, 0, 1, 2, 3 }, 0)));
            }

            // Tiered recompilation
            {
                Expression<F<int, F<int, int>>> f = x => y => x * y;
                var d = f.Compile(CompilationOptions.EnableJustInTimeCompilation | CompilationOptions.TieredCompilation);

                for (int i = 0; i < 8; i++)
                {
                    object target = ((dynamic)d).Target.Parent.Target.Method;
                    Console.WriteLine(target);

                    d(1)(2);
                }
            }

            //ThunkFactory.GetThunkType(typeof(AssemblyLoadEventHandler), typeof(int));
            //var t = ThunkFactory.GetThunkType(typeof(EventHandler<AssemblyLoadEventArgs>), typeof(int));
            //var l = (Expression<Action<int, object, AssemblyLoadEventArgs>>)((c, o, e) => Console.WriteLine("{0} - {1} - {2}", c, o, e.LoadedAssembly));
            //var p = t.GetConstructors()[0].GetParameters()[0].ParameterType.GetGenericArguments()[0];
            //var f = Expression.Lambda(p, l.Body, l.Parameters);
            //var i = Activator.CreateInstance(t, new object[] { f });
            //var q = (Expression<Func<int, Func<int, int>>>)(x => y => x + y);
            //Thunks();
        }

        private static void Test<T>(Expression<T> expression, Func<T, object> invoke) where T : Delegate
        {
            Console.WriteLine("================================================================================");
            Console.WriteLine(expression);
            Console.WriteLine("--------------------------------------------------------------------------------");

            PrintDebugView(expression);

            PrintTitle("Regular - Compiled", ConsoleColor.Red);

            PrintIL(expression);
            Console.WriteLine(invoke(expression.Compile()));
            Console.WriteLine();

            PrintTitle("Regular - Interpreted", ConsoleColor.Red);

            PrintInterpreterInstructions(expression);
            Console.WriteLine(invoke(expression.Compile(preferInterpretation: true)));
            Console.WriteLine();

            PrintTitle("JIT - Compiled", ConsoleColor.Red);

            var d1 = expression.Compile(CompilationOptions.EnableJustInTimeCompilation);
            PrintJitInfo(d1);
            Console.WriteLine(invoke(d1));
            Console.WriteLine();

            PrintTitle("JIT - Interpreted", ConsoleColor.Red);

            var d2 = expression.Compile(CompilationOptions.EnableJustInTimeCompilation | CompilationOptions.PreferInterpretation);
            PrintJitInfo(d2, preferInterpretation: true);
            Console.WriteLine(invoke(d2));
            Console.WriteLine();

            Console.WriteLine("================================================================================");
            Console.WriteLine();
        }

        private static void PrintJitInfo(Delegate d, bool preferInterpretation = false)
        {
            var mt = (FunctionContext<MethodTable>)d.Target;

            int i = 0;

            foreach (Thunk t in mt.Closure.Thunks.Cast<Thunk>())
            {
                PrintTitle($"Thunk {i}", ConsoleColor.Cyan);
                Console.WriteLine();

                PrintDebugView(t.Lambda);

                if (preferInterpretation)
                {
                    PrintInterpreterInstructions(t.Lambda);
                }
                else
                {
                    PrintIL(t.Lambda);
                }

                i++;
            }
        }

        private static readonly PropertyInfo s_debugView = typeof(Expression).GetProperty("DebugView", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        private static void PrintDebugView(LambdaExpression e)
        {
            PrintTitle("DebugView", ConsoleColor.Green);
            var s = (string)s_debugView.GetValue(e);
            Console.WriteLine(s);
            Console.WriteLine();
        }

        private static void PrintIL(LambdaExpression e)
        {
            PrintTitle("IL", ConsoleColor.Green);
            Console.WriteLine(ILPrinter.GetIL(e));
            Console.WriteLine();
        }

        private static void PrintInterpreterInstructions(LambdaExpression e)
        {
            PrintTitle("Instructions", ConsoleColor.Green);
            dynamic dt = e.Compile(preferInterpretation: true);
            try
            {
                object o = dt.Target.Target;
                var debugView2 = o.GetType().GetProperty("DebugView", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                var s2 = (string)debugView2.GetValue(o);
                Console.WriteLine(s2);
            }
            catch
            {
                Console.WriteLine("(Not supported)");
            }
            Console.WriteLine();
        }

        private static void PrintTitle(string s, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(s);
            Console.ResetColor();
        }
    }
}
