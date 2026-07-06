// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2012
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Nuqleon.Serialization;

namespace Nuqleon.Linq.Expressions.Serialization
{
    using TypeSystem;

    using Json = Nuqleon.Json.Expressions;

    /// <summary>
    /// Rule table for serialization of expression trees.
    /// </summary>
    internal class ExpressionTreeRuleTable : JsonRuleTable<ExpressionJsonSerializationContext>
    {
        #region Singleton pattern

        /// <summary>
        /// Single instance creation lock.
        /// </summary>
        private static readonly object s_singletonLock = new();

        /// <summary>
        /// Singleton instance for the default, read-only expression tree serialization rule table.
        /// </summary>
        private static volatile RuleTableBase<object, Json.Expression, ExpressionJsonSerializationContext> s_instance;

        /// <summary>
        /// Gets the singleton instance for the default, read-only expression tree serialization rule table.
        /// </summary>
        public static RuleTableBase<object, Json.Expression, ExpressionJsonSerializationContext> Instance
        {
            get
            {
                //
                // Double-locking pattern to ensure single initialization even in the face of multiple
                // threads racing for initialization.
                //
                if (s_instance == null)
                {
                    lock (s_singletonLock)
                    {
                        s_instance ??= new ExpressionTreeRuleTable();
                    }
                }

                return s_instance;
            }
        }

        /// <summary>
        /// Creates an rule table with default rules for expression tree serialization.
        /// </summary>
        protected ExpressionTreeRuleTable()
        {
            //
            // Initialize rules.
            //
            AddExpressionRules();
            AddReflectionRules();
            AddEnumRule();
        }

        #endregion

        #region Methods

        #region Rule initialization

        /// <summary>
        /// Adds rules for expression tree nodes (per .NET 3.5 parlance).
        /// </summary>
        private void AddExpressionRules()
        {
            Add(
                "ExpressionType", default(ExpressionType),
                (et, _, ctx) => Json.Expression.String(et.ToString()),
                (jo, _, ctx) => (ExpressionType)Enum.Parse(typeof(ExpressionType), ((Json.ConstantExpression)jo).Value.ToString())
            );
            Add(
                "Expressions", default(ReadOnlyCollection<Expression>),
                (es, serialize, ctx) => Json.Expression.Array(from e in es select serialize(e, ctx)),
                (jo, deserialize, ctx) => (from e in ((Json.ArrayExpression)jo).Elements select (Expression)deserialize(e, ctx)).ToList().AsReadOnly()
            );

            Add((ConstantExpression ce) => Expression.Constant(ce.Value, ce.Type));

            Add((UnaryExpression ue) => Expression.MakeUnary(ue.NodeType, ue.Operand, ue.Type, ue.Method));
            Add((MemberExpression me) => Expression.MakeMemberAccess(me.Expression, me.Member));

            Add((BinaryExpression be) => Expression.MakeBinary(be.NodeType, be.Left, be.Right, be.IsLiftedToNull, be.Method, be.Conversion));
            Add((TypeBinaryExpression te) => Expression.TypeIs(te.Expression, te.TypeOperand));

            Add((ConditionalExpression ce) => Expression.Condition(ce.Test, ce.IfTrue, ce.IfFalse));

            Add((MethodCallExpression mc) => Expression.Call(mc.Object, mc.Method, mc.Arguments));
            Add((InvocationExpression ie) => Expression.Invoke(ie.Expression, ie.Arguments));

            Add((DefaultExpression de) => Expression.Default(de.Type));
            Add((IndexExpression ie) => Expression.MakeIndex(ie.Object, ie.Indexer, ie.Arguments));
            Add((DynamicExpression de) => Expression.MakeDynamic(de.DelegateType, de.Binder, de.Arguments));
            Add((DebugInfoExpression die) => Expression.DebugInfo(die.Document, die.StartLine, die.StartColumn, die.EndLine, die.EndColumn));
            Add((SymbolDocumentInfo sdi) => Expression.SymbolDocument(sdi.FileName, sdi.Language, sdi.LanguageVendor, sdi.DocumentType));

            AddObjectCreationRules();
            AddLambdaAndParameterRules();
        }

        /// <summary>
        /// Adds rules for object creation expression trees.
        /// </summary>
        private void AddObjectCreationRules()
        {
            Add(
                default(NewExpression), ne => ne.Members == null,
                ne => Expression.New(ne.Constructor, ne.Arguments)
            );
            Add(
                "NewExpressionWithMembers", default(NewExpression), ne => ne.Members != null,
                ne => Expression.New(ne.Constructor, ne.Arguments, ne.Members)
            );

            Add(
                "NewArray", default(NewArrayExpression),
                (na, serialize, ctx) =>
                    Json.Expression.Object(
                        new Dictionary<string, Json.Expression>
                        {
                            { "NodeType", serialize(na.NodeType, ctx) },
                            { "ElementType", serialize(na.Type.GetElementType(), ctx) },
                            { "Expressions", serialize(na.Expressions, ctx) }
                        }
                    ),
                (jo, deserialize, ctx) =>
                    ((Json.ObjectExpression)jo).Let(o =>
                        ((Type)deserialize(o.Members["ElementType"], ctx)).Let(elementType =>
                            ((ReadOnlyCollection<Expression>)deserialize(o.Members["Expressions"], ctx)).Let(args =>
                                (ExpressionType)deserialize(o.Members["NodeType"], ctx) == ExpressionType.NewArrayBounds
                                ? Expression.NewArrayBounds(elementType, args)
                                : Expression.NewArrayInit(elementType, args)
                            )
                        )
                    )
            );

            Add((MemberInitExpression me) => Expression.MemberInit(me.NewExpression, me.Bindings));

            Add((MemberAssignment ma) => Expression.Bind(ma.Member, ma.Expression));
            Add((MemberMemberBinding mm) => Expression.MemberBind(mm.Member, mm.Bindings));
            Add(
                "MemberBindings", default(ReadOnlyCollection<MemberBinding>),
                (ms, serialize, ctx) => Json.Expression.Array(from m in ms select serialize(m, ctx)),
                (jo, deserialize, ctx) => (from e in ((Json.ArrayExpression)jo).Elements select (MemberBinding)deserialize(e, ctx)).ToList().AsReadOnly()
            );
            Add((MemberListBinding ml) => Expression.ListBind(ml.Member, ml.Initializers));
            Add((ListInitExpression le) => Expression.ListInit(le.NewExpression, le.Initializers));
            Add((ElementInit ei) => Expression.ElementInit(ei.AddMethod, ei.Arguments));
            Add(
                "ElementInits", default(ReadOnlyCollection<ElementInit>),
                (ms, serialize, ctx) => Json.Expression.Array(from m in ms select serialize(m, ctx)),
                (jo, deserialize, ctx) => (from e in ((Json.ArrayExpression)jo).Elements select (ElementInit)deserialize(e, ctx)).ToList().AsReadOnly()
            );
        }

        /// <summary>
        /// Adds rules for lambda expressions and parameter expressions with proper scoping support.
        /// </summary>
        private void AddLambdaAndParameterRules()
        {
            Add(
                "Lambda", default(LambdaExpression),
                (le, serialize, ctx) =>
                {
                    var frame = ctx.Scope.Serialization.PushFrame(le.Parameters);

                    var parameters = frame.ToJson(ctx.Types);
                    var body = serialize(le.Body, ctx);
                    var type = serialize(le.Type, ctx);

                    ctx.Scope.Serialization.PopFrame();

                    return Json.Expression.Object(
                        new Dictionary<string, Json.Expression>
                        {
                            { "Parameters", parameters },
                            { "Body", body },
                            { "Type", type },
                        }
                    );
                },
                (jo, deserialize, ctx) =>
                {
                    var o = (Json.ObjectExpression)jo;

                    var frame = ctx.Scope.Deserialization.PushFrame(o.Members["Parameters"], ctx.Types);

                    var parameters = frame.Parameters;
                    var body = (Expression)deserialize(o.Members["Body"], ctx);
                    var type = (Type)deserialize(o.Members["Type"], ctx);

                    ctx.Scope.Deserialization.PopFrame();

                    return Expression.Lambda(type, body, parameters);
                }
            );
            Add(
                "Parameter", default(ParameterExpression),
                (pe, _, ctx) => Json.Expression.String(ctx.Scope.Serialization.GetParameter(pe)),
                (jo, _, ctx) => ctx.Scope.Deserialization.GetParameter((string)((Json.ConstantExpression)jo).Value)
            );
            Add(
                "Parameters", default(ReadOnlyCollection<ParameterExpression>),
                (ps, serialize, ctx) => Json.Expression.Array(from p in ps select serialize(p, ctx)),
                (jo, deserialize, ctx) => (from e in ((Json.ArrayExpression)jo).Elements select (ParameterExpression)deserialize(e, ctx)).ToList().AsReadOnly()
            );
        }

        /// <summary>
        /// Adds rules for reflection objects.
        /// </summary>
        private void AddReflectionRules()
        {
            Add(
                "Type", default(Type),
                (tp, _, ctx) => ctx.Types.Register(tp).ToJson(),
                (jo, _, ctx) => ctx.Types.Lookup(TypeRef.FromJson(jo))
            );
            Add((PropertyInfo pi) => pi.DeclaringType.GetProperty(pi.Name, pi.PropertyType));
            Add((FieldInfo fi) => fi.DeclaringType.GetField(fi.Name));
            Add(
                "MemberInfos", default(ReadOnlyCollection<MemberInfo>),
                (ms, serialize, ctx) => Json.Expression.Array(from m in ms select serialize(m, ctx)),
                (jo, deserialize, ctx) => (from e in ((Json.ArrayExpression)jo).Elements select (MemberInfo)deserialize(e, ctx)).ToList().AsReadOnly()
            );
            AddConstructorRule();
            AddMethodRule();
        }

        /// <summary>
        /// Adds a rule for ConstructorInfo serialization.
        /// </summary>
        private void AddConstructorRule()
        {
            Add(
                "Constructor", default(ConstructorInfo),
                (ci, serialize, ctx) =>
                    Json.Expression.Object(
                        new Dictionary<string, Json.Expression>
                        {
                            { "DeclaringType", serialize(ci.DeclaringType, ctx) },
                            { "Parameters", Json.Expression.Array(from p in ci.GetParameters() select serialize(p.ParameterType, ctx)) }
                        }
                    ),
                (jo, deserialize, ctx) =>
                    ((Json.ObjectExpression)jo).Let(o =>
                        ((Type)deserialize(o.Members["DeclaringType"], ctx)).GetConstructor(
                            (from e in ((Json.ArrayExpression)o.Members["Parameters"]).Elements
                             select (Type)deserialize(e, ctx))
                            .ToArray()
                        )
                    )
            );
        }

        /// <summary>
        /// Adds a rule for MethodInfo serialization.
        /// </summary>
        private void AddMethodRule()
        {
            Add(
                "Method", default(MethodInfo),
                (mi, serialize, ctx) =>
                {
                    var members = new Dictionary<string, Json.Expression>
                    {
                        { "DeclaringType", serialize(mi.DeclaringType, ctx) },
                        { "Name", Json.Expression.String(mi.Name) },
                        { "ParameterTypes", Json.Expression.Array(mi.GetParameters().Select(p => serialize(p.ParameterType, ctx))) },
                    };

                    if (mi.IsGenericMethod)
                        members.Add("GenericArguments", Json.Expression.Array(mi.GetGenericArguments().Select(a => serialize(a, ctx))));
                    if (mi.IsSpecialName)
                        members.Add("ReturnType", serialize(mi.ReturnType, ctx));

                    return Json.Expression.Object(members);
                },
                (jo, deserialize, ctx) =>
                {
                    var o = (Json.ObjectExpression)jo;

                    var declaringType = (Type)deserialize(o.Members["DeclaringType"], ctx);
                    var name = (string)((Json.ConstantExpression)o.Members["Name"]).Value;
                    var parameterTypes = ((Json.ArrayExpression)o.Members["ParameterTypes"]).Elements.Select(p => (Type)deserialize(p, ctx)).ToArray();

                    if (o.Members.TryGetValue("GenericArguments", out Json.Expression args))
                    {
                        var genArg = (from p in ((Json.ArrayExpression)args).Elements
                                      select (Type)deserialize(p, ctx))
                                     .ToArray();

                        var candidates = from m in declaringType.GetMethods()
                                         where m.IsGenericMethodDefinition
                                         where m.Name == name
                                         where m.GetGenericArguments().Length == genArg.Length
                                         select m;

                        foreach (var candidate in candidates)
                        {
                            var test = candidate.MakeGenericMethod(genArg);
                            if (test.GetParameters().Select(p => p.ParameterType).SequenceEqual(parameterTypes))
                                return test;
                        }

                        throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Cannot find method '{0}.{1}({2})'.", declaringType.Name, name, string.Join(", ", parameterTypes.Select(t => t.Name).ToArray())));
                    }

                    if (o.Members.TryGetValue("ReturnType", out Json.Expression ret))
                    {
                        var retType = (Type)deserialize(ret, ctx);
                        return declaringType.GetMethods().Single(m => m.Name == name && m.GetParameters().Select(p => p.ParameterType).SequenceEqual(parameterTypes) && m.ReturnType == retType);
                    }
                    else
                    {
                        return declaringType.GetMethod(name, parameterTypes);
                    }
                }
            );
        }

        /// <summary>
        /// Adds a rule for serialization of enum values.
        /// </summary>
        /// <remarks>This rule isn't present in the base type because the JsonSerializer base class doesn't have rules for type tables. This may be reconsidered in the future.</remarks>
        private void AddEnumRule()
        {
            Add<Enum>(
                "Enum", default(Enum),
                (en, serialize, ctx) =>
                {
                    var u = Enum.GetUnderlyingType(en.GetType());
                    var o = Convert.ChangeType(en, u, CultureInfo.InvariantCulture);
                    var t = ctx.Types.Register(en.GetType()).ToJson();

                    return Json.Expression.Object(new Dictionary<string, Json.Expression>
                    {
                        { "Type", t },
                        { "Value", serialize(o, ctx) },
                    });
                },
                (jo, deserialize, ctx) =>
                {
                    var o = (Json.ObjectExpression)jo;

                    var t = ctx.Types.Lookup(TypeRef.FromJson(o.Members["Type"]));
                    var u = Enum.GetUnderlyingType(t);
                    var v = deserialize(o.Members["Value"], ctx);

                    var toObj = typeof(Enum).GetMethod("ToObject", BindingFlags.Public | BindingFlags.Static, binder: null, new[] { typeof(Type), u }, modifiers: null);
                    return (Enum)toObj.Invoke(obj: null, new[] { t, v });
                }
            );
        }

        #endregion

        #endregion
    }
}
