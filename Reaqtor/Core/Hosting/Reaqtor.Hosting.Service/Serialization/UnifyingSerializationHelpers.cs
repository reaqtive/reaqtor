// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Nuqleon.DataModel;
using Reaqtor.Hosting.Shared.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Reaqtor.Hosting.Shared.Serialization
{
    /// <summary>
    /// Helper class to serialize and deserialize expressions and data model-
    /// compliant objects.
    /// </summary>
    /// <remarks>
    /// This serializer unifies slim types against CLR types for unbound parameters
    /// sourced from definitions in <see cref="IReactiveMetadata"/>.
    /// </remarks>
    public class UnifyingSerializationHelpers : SerializationHelpers
    {
        /// <summary>
        /// The metadata definitions interface.
        /// </summary>
        private readonly IReactiveMetadata _metadata;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnifyingSerializationHelpers"/>
        /// class to serialize and deserialize expressions and data model-compliant objects.
        /// </summary>
        /// <param name="metadata">
        /// The source of unbound parameter definitions for unification.
        /// </param>
        public UnifyingSerializationHelpers(IReactiveMetadata metadata)
        {
            _metadata = metadata;
        }

        /// <summary>
        /// Create an expression serializer.
        /// </summary>
        /// <returns>An expression serializer.</returns>
        protected override IExpressionSerializer CreateExpressionSerializer()
        {
            return new TypeUnifyingExpressionSerializer(this);
        }

        /// <summary>
        /// Looks up the free variable name in the Reactive metadata and attempts to unify entity types.
        /// </summary>
        /// <param name="free">The free variable.</param>
        /// <param name="metadata">The metadata to unify aginst.</param>
        /// <returns>
        /// Unifications between the free variable type and the known resource type.
        /// </returns>
        internal static IEnumerable<KeyValuePair<TypeSlim, Type>> FindAndUnify(ParameterExpressionSlim free, IReactiveMetadata metadata)
        {
            var reactiveType = ReactiveEntityTypeExtensions.FromTypeSlim(free.Type);
            if ((reactiveType & ReactiveEntityType.Func) == ReactiveEntityType.Func)
            {
                reactiveType ^= ReactiveEntityType.Func;
            }

            var metadataType = default(Type);
            switch (reactiveType)
            {
                case ReactiveEntityType.Observer:
                    if (metadata.Observers.TryGetValue(new Uri(free.Name), out var observer))
                    {
                        metadataType = observer.Expression.Type;
                    }
                    break;
                case ReactiveEntityType.Observable:
                    if (metadata.Observables.TryGetValue(new Uri(free.Name), out var observable))
                    {
                        metadataType = observable.Expression.Type;
                    }
                    break;
                case ReactiveEntityType.SubscriptionFactory:
                    if (metadata.SubscriptionFactories.TryGetValue(new Uri(free.Name), out var subscriptionFactory))
                    {
                        metadataType = subscriptionFactory.Expression.Type;
                    }
                    break;
                case ReactiveEntityType.Subscription:
                    if (metadata.Subscriptions.TryGetValue(new Uri(free.Name), out var subscription))
                    {
                        metadataType = subscription.Expression.Type;
                    }
                    break;
                case ReactiveEntityType.StreamFactory:
                    if (metadata.StreamFactories.TryGetValue(new Uri(free.Name), out var streamFactory))
                    {
                        metadataType = streamFactory.Expression.Type;
                    }
                    break;
                case ReactiveEntityType.Stream:
                    if (metadata.Streams.TryGetValue(new Uri(free.Name), out var stream))
                    {
                        metadataType = stream.Expression.Type;
                    }
                    break;
            }

            if (metadataType != null)
            {
                var unifier = new DataModelTypeUnifier();
                if (unifier.Unify(metadataType, free.Type))
                {
                    return unifier.Entries;
                }
            }

            return Array.Empty<KeyValuePair<TypeSlim, Type>>();
        }

        /// <summary>
        /// Inverted type space to convert slim representations of reflection
        /// objects into the corresponding CLR reflection objects with data
        /// model awareness.
        /// </summary>
        /// TODO: Remove this once the DataModel project is updated with this implementation
        private sealed class DataModelInvertedTypeSpace : InvertedTypeSpace
        {
            /// <summary>
            /// Gets the CLR property with the specified "signature"
            /// corresponding to the specified slim property representation.
            /// This method has built-in data model awareness to map properties
            /// based on their mapped identity using MappingAttribute.
            /// </summary>
            /// <param name="propertySlim">
            /// Slim property representation to convert to the corresponding
            /// CLR property.
            /// </param>
            /// <param name="declaringType">
            /// Declaring type to locate the property on.
            /// </param>
            /// <param name="propertyType">Type of the property.</param>
            /// <param name="indexParameterTypes">
            /// Parameter types of the indexer, if any.
            /// </param>
            /// <returns>
            /// CLR property corresponding to the specified slim property
            /// representation.
            /// </returns>
            /// <exception cref="ArgumentNullException">
            /// <c>declaringType</c> is null</exception>
            protected override PropertyInfo GetPropertyCore(PropertyInfoSlim propertySlim, Type declaringType, Type propertyType, Type[] indexParameterTypes)
            {
                if (propertySlim.DeclaringType.Kind == TypeSlimKind.Structural)
                {
                    var propertyMatch = declaringType.GetProperties().SingleOrDefault(prop =>
                    {
                        var attribute = prop.GetCustomAttribute<MappingAttribute>(inherit: false);
                        if (attribute == null)
                        {
                            return prop.Name == propertySlim.Name;
                        }
                        else
                        {
                            return attribute.Uri == propertySlim.Name || prop.Name == propertySlim.Name;
                        }
                    });

                    if (propertyMatch == default)
                    {
                        throw new InvalidOperationException(
                            string.Format(
                                CultureInfo.InvariantCulture,
                                "No property with name or mapping attribute '{0}' was found on type '{1}'.",
                                propertySlim.Name,
                                declaringType));
                    }

                    return propertyMatch;
                }

                return base.GetPropertyCore(propertySlim, declaringType, propertyType, indexParameterTypes);
            }
        }

        /// <summary>
        /// Converter for slim expressions to LINQ expressions, with built-in
        /// awareness of data model projection peculiarities.
        /// </summary>
        /// TODO: Remove this once the DataModel project is updated with this implementation
        private sealed class DataModelExpressionSlimToExpressionConverter : ExpressionSlimToExpressionConverter
        {
            /// <summary>
            /// Creates a new expression converter using the type space obtained
            /// from unification.
            /// </summary>
            /// <param name="typeSpace">Type space containing type mappings.</param>
            public DataModelExpressionSlimToExpressionConverter(DataModelInvertedTypeSpace typeSpace)
                : base(typeSpace)
            {
            }

            /// <summary>
            /// Visits a member assignment slim tree node to produce a member assignment,
            /// taking implicit conversions for primitive data model types into account.
            /// </summary>
            /// <param name="node">Slim expression representation of the assignment.</param>
            /// <param name="expression">Expression assigned to the member, obtained from recursive conversion steps.</param>
            /// <returns>Member assignment node.</returns>
            protected override MemberAssignment MakeMemberAssignment(MemberAssignmentSlim node, Expression expression)
            {
                var member = TypeSpace.GetMember(node.Member);

                var lhsType = GetMemberAssignmentMemberType(member);
                var rhsType = expression.Type;

                // This case occurs when an enum is used in a known type and the underlying type of the enum
                // is used during anonymization. For example:
                //
                //    xs.Select(x => new Person { Sex = Sex.Male, Age = x })
                //
                // Erasure of the enum results in the following anonymized expression:
                //
                //    xs.Select(x => new <>__Record { entity://person/sex = 1, entity://person/age = x })
                //
                // Upon unification and reconstruction of the expression, the record type is unified against
                // the Person type, and an assignment incompatibility occurs for Sex = 1.
                //
                // A similar case exists during recursive rewriting of a subexpression whose type, declared
                // on a known type, is an enum. For example:
                //
                //    xs.Select(x => new Person { Sex = x.Sex, Age = 21 })
                //
                // Erasure of the enum results in the following anonymized expression:
                //
                //    xs.Select(x => new <>__Record { entity://person/sex = x.entity://ape/sex, entity://person/age = 21 })
                //
                // This time around, as the type of "x" gets resolved, the x.entity://ape/sex subexpression is
                // turned into x.Sex on the known type, which is an enum. Assignment to the record type's int
                // property now fails because of lack of conversion.
                if (lhsType != rhsType && (IsEnumAssignableToUnderlyingType(lhsType, rhsType) || IsEnumAssignableToUnderlyingType(rhsType, lhsType)))
                {
                    expression = Expression.Convert(expression, lhsType);
                }

                return base.MakeMemberAssignment(node, expression);
            }

            /// <summary>
            /// Returns whether the specified enum candidate type is an enum, and if so, it has the specified underlying type.
            /// </summary>
            /// <param name="enumCandidate">Candidate type to check to be an enum.</param>
            /// <param name="underlyingCandidate">Candidate underlying type to check on the enum.</param>
            /// <returns>true if the enum candidate is an enum and its underlying type is the underlying candidate; otherwise, false.</returns>
            private static bool IsEnumAssignableToUnderlyingType(Type enumCandidate, Type underlyingCandidate)
            {
                return enumCandidate.IsEnum && enumCandidate.GetEnumUnderlyingType() == underlyingCandidate;
            }

            /// <summary>
            /// Gets the return type of the specified member, used in a MemberExpression or a MemberAssignment.
            /// </summary>
            /// <param name="member">Member to get the return type for. The member should be a property of a field.</param>
            /// <returns>Return type of the specified member.</returns>
            private static Type GetMemberAssignmentMemberType(MemberInfo member)
            {
                Debug.Assert(member.MemberType is MemberTypes.Property or MemberTypes.Field);

                if (member is PropertyInfo prop)
                {
                    return prop.PropertyType;
                }

                var field = (FieldInfo)member;
                return field.FieldType;
            }
        }

        /// <summary>
        /// An expression serializer that performs type unification against
        /// free variable definitions before reducing to Linq expressions.
        /// </summary>
        private sealed class TypeUnifyingExpressionSerializer : SerializationHelpersExpressionSerializer
        {
            /// <summary>
            /// The parent helpers with a field for the metadata definitions interface.
            /// </summary>
            private readonly UnifyingSerializationHelpers _parent;

            /// <summary>
            /// The inverted type space for this serializer instance.
            /// </summary>
            private DataModelInvertedTypeSpace _invertedTypeSpace;

            /// <summary>
            /// Instantiates the expression serializer.
            /// </summary>
            /// <param name="parent">
            /// A reference to the parent serialization helpers, which has a reference
            /// to the Reactive metadata.
            /// </param>
            public TypeUnifyingExpressionSerializer(UnifyingSerializationHelpers parent)
                : base(parent)
            {
                _parent = parent;
            }

            /// <summary>
            /// Creates a slim expression to expression reducer.
            /// </summary>
            /// <returns>A slim expression to expression reducer.</returns>
            protected override ExpressionSlimToExpressionConverter CreateReducer()
            {
                return new DataModelExpressionSlimToExpressionConverter(_invertedTypeSpace);
            }

            /// <summary>
            /// Reduces a slim expression to a Linq expression, first mapping slim
            /// types to types from the expanded definition of the free variables in
            /// the expression.
            /// </summary>
            /// <param name="expression">The expression to reduce.</param>
            /// <returns>The reduced expression.</returns>
            public override Expression Reduce(ExpressionSlim expression)
            {
                EnsureInvertedTypeSpace();

                var freeVariables = FreeVariableScannerSlim.Find(expression);
                foreach (var free in freeVariables)
                {
                    var unifications = FindAndUnify(free, _parent._metadata);

                    foreach (var unification in unifications)
                    {
                        _invertedTypeSpace.MapType(unification.Key, unification.Value);
                    }
                }

                return base.Reduce(expression);
            }

            private void EnsureInvertedTypeSpace()
            {
                _invertedTypeSpace ??= new DataModelInvertedTypeSpace();
            }
        }
    }
}
