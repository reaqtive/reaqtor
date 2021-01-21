// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Implementation of the Where operator using fusion.
//
// BD - October 2014
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace OperatorFusion
{
    internal sealed class WhereFactory : IFusionOperator
    {
        private Dictionary<int, Span> _index;
        private SymbolDocumentInfo _file;

        public Type OutputType => Predicate.Parameters[0].Type;

        public HoistOperations Hoist => HoistOperations.OnError;

        public LambdaExpression Predicate { get; set; }

        public void Initialize(ParameterExpression state, Func<Type, FieldInfo> defineField, Action<Expression> appendToCtor, Expression result, Expression disposable)
        {
            // TODO: finish debugging experiment; maybe introduce variable declaration service to create unique names and substitute "holes" in the DebugText template
            CompileDebugText();
        }

        private static string DebugText
        {
            get
            {
                return @"
class Where<T>
{
    public void OnNext(T value)
    {
        #1#var p = false;#-1#
        try
        {
            #2#p = predicate(value);#-2#
        }
        catch (Exception ex)
        {
            #3#observer.OnError(ex);#-3#
            Dispose();
            return;
        }

        if (p)
        {
            #4#observer.OnNext(value);#-4#
        }
    }

    public void OnError(error)
    {
        #5#observer.OnError(error);#-5#
        Dispose();
    }

    public void OnCompleted()
    {
        #6#observer.OnCompleted();#-6#
        Dispose();
    }
}
";
            }
        }

        private void CompileDebugText()
        {
            var dbg = DebugText;

            var code = new StringBuilder();

            var index = new Dictionary<int, Span>();

            var line = 1;
            var col = 1;

            var i = 0;
            while (i < dbg.Length)
            {
                var c = dbg[i];

                if (c == '\r')
                {
                    code.Append(c);
                    i++;

                    if (i < dbg.Length)
                    {
                        if (dbg[i] == '\n')
                        {
                            code.Append(dbg[i]);
                            i++;
                        }
                    }

                    line++;
                    col = 1;
                    continue;
                }

                if (c == '#')
                {
                    if (i >= dbg.Length)
                        throw new InvalidOperationException("Non-terminated sequence point marker.");

                    var e = false;

                    i++;

                    if (dbg[i] == '-')
                    {
                        e = true;

                        i++;

                        if (i >= dbg.Length)
                            throw new InvalidOperationException("Non-terminated sequence point marker.");
                    }

                    var n = 0;

                    while (i < dbg.Length && dbg[i] != '#')
                    {
                        if (!char.IsDigit(dbg[i]))
                            throw new InvalidOperationException("Expected digit in sequence point marker.");

                        n = n * 10 + dbg[i] - '0';

                        i++;
                    }

                    if (i >= dbg.Length)
                        throw new InvalidOperationException("Non-terminated sequence point marker.");

                    i++;

                    if (e)
                    {
                        index[n].EndLine = line;
                        index[n].EndColumn = col;
                    }
                    else
                    {
                        index.Add(n, new Span { StartLine = line, StartColumn = col });
                    }
                }
                else
                {
                    code.Append(c);
                    i++;
                    col++;
                }
            }

            var cc = code.ToString();

            //var lines = cc.Split('\n');

            //foreach (var kv in index)
            //{
            //    // quick test, assuming start and end line is the same
            //    var fragment = lines[kv.Value.StartLine - 1].Substring(kv.Value.StartColumn - 1, kv.Value.EndColumn - kv.Value.StartColumn);
            //    Console.WriteLine(kv.Key + " = " + fragment);
            //}

            var file = "Where.generated.cs";
            _index = index;
            _file = Expression.SymbolDocument(file);

            File.WriteAllText(file, cc);
        }

        private DebugInfoExpression GetDebugInfo(int seqId)
        {
            var span = _index[seqId];
            return Expression.DebugInfo(_file, span.StartLine, span.StartColumn, span.EndLine, span.EndColumn);
        }

        private sealed class Span
        {
            public int StartLine;
            public int StartColumn;
            public int EndLine;
            public int EndColumn;

            public override string ToString()
            {
                return string.Format("[{0},{1}]..[{2},{3}]", StartLine, StartColumn, EndLine, EndColumn);
            }
        }

        public Func<Expression, Expression> CreateOnNext(Func<Expression, Expression> createOnNext, Func<Expression, Expression> createOnError, Expression onCompleted, ParameterExpression state, Expression markDirty)
        {
            return value =>
            {
                var ret = Expression.Label();

                var ex = Expression.Parameter(typeof(Exception), "ex");
                var p = Expression.Parameter(typeof(bool), "p");

                return
                    Expression.Block(
                        typeof(void),
                        new[] { p },
                        GetDebugInfo(1),
                        Expression.Assign(p, Expression.Constant(false)),
                        Expression.TryCatch(
                            Expression.Block(
                                GetDebugInfo(2),
                                Expression.Assign(p, Expression.Invoke(Predicate, value)),
                                Expression.Default(typeof(void))
                            ),
                            Expression.MakeCatchBlock(
                                typeof(Exception), ex,
                                Expression.Block(
                                    GetDebugInfo(3),
                                    createOnError(ex),
                                    Expression.Return(ret)
                                ),
                                null
                            )
                        ),
                        Expression.IfThen(
                            p,
                            Expression.Block(
                                GetDebugInfo(4),
                                createOnNext(value)
                            )
                        ),
                        Expression.Label(ret)
                    );
            };
        }

        public Func<Expression, Expression> CreateOnError(Func<Expression, Expression> createOnNext, Func<Expression, Expression> createOnError, Expression onCompleted, ParameterExpression state, Expression markDirty)
        {
            return error =>
            {
                return 
                    Expression.Block(
                        GetDebugInfo(5),
                        createOnError(error)
                    );
            };
        }

        public Expression CreateOnCompleted(Func<Expression, Expression> createOnNext, Func<Expression, Expression> createOnError, Expression onCompleted, ParameterExpression state, Expression markDirty)
        {
            return 
                Expression.Block(
                    GetDebugInfo(6),
                    onCompleted
                );
        }
    }
}
