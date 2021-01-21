// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
// ER - July 2013 - Small tweaks.
//

using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using Json = Nuqleon.Json.Expressions;

namespace System.Linq.Expressions.Bonsai.Serialization
{
    #region Aliases

    //
    // This makes diffing with the original Bonsai serializer easier.
    //

    using Type = TypeSlim;
    using MemberInfo = MemberInfoSlim;

    using Expression = ExpressionSlim;
    using ParameterExpression = ParameterExpressionSlim;
    using LabelTarget = LabelTargetSlim;

    #endregion

    internal class DeserializationState
    {
        #region Fields

        private readonly DeserializationDomain _domain;
        private readonly Stack<ParameterExpression[]> _params;
        private readonly ParameterExpression[] _globals;
        private readonly LabelTarget[] _labelTagets;

        #endregion

        #region Constructors

        public DeserializationState(Json.Expression state, Version version)
        {
            _domain = new DeserializationDomain(state);
            _params = new Stack<ParameterExpression[]>();
            _globals = GetGlobals(state);
            _labelTagets = GetLabelTargets(state);

            Debug.Assert(_domain.SupportsVersion(version));
        }

        #endregion

        #region Methods

        public Type GetType(Json.Expression expression)
        {
            return _domain.GetType(expression);
        }

        public ParameterExpression Lookup(Json.ArrayExpression expression)
        {
            var n = expression.ElementCount;
            if (n is not 2 and not 3)
                throw new BonsaiParseException("Expected 2 or 3 JSON array elements for an expression of type 'Parameter'.", expression);

            if (n == 2)
            {
                if (expression.GetElement(1) is not Json.ConstantExpression indexJson || !int.TryParse(indexJson.Value.ToString(), out int index))
                    throw new BonsaiParseException("Expected a JSON number in 'node[1]' containing a global parameter reference to bind an expression of type 'Parameter'.", expression);

                if (_globals == null || index < 0 || index >= _globals.Length)
                    throw new BonsaiParseException(string.Format(CultureInfo.InvariantCulture, "A global parameter with index {0} is not defined.", index), expression);

                return _globals[index];
            }
            else
            {
                if (expression.GetElement(1) is not Json.ConstantExpression scopeJson || !int.TryParse(scopeJson.Value.ToString(), out int scope))
                    throw new BonsaiParseException("Expected a JSON number in 'node[1]' containing a parameter scope to bind an expression of type 'Parameter'.", expression);

                var targetScope = GetScope(scope, expression);

                if (expression.GetElement(2) is not Json.ConstantExpression indexJson || !int.TryParse(indexJson.Value.ToString(), out int index))
                    throw new BonsaiParseException("Expected a JSON number in 'node[2]' containing a parameter index to bind an expression of type 'Parameter'.", expression);

                if (index < 0 || index >= targetScope.Length)
                    throw new BonsaiParseException(string.Format(CultureInfo.InvariantCulture, "A parameter with index {0} is not available at scope depth {1}.", index, scope), expression);

                return targetScope[index];
            }
        }

        private ParameterExpression[] GetScope(int index, Json.Expression expression)
        {
            if (index >= 0)
            {
                var idx = index;

                using var e = _params.GetEnumerator();

                while (e.MoveNext())
                {
                    if (idx == 0)
                    {
                        return e.Current;
                    }

                    idx--;
                }
            }

            throw new BonsaiParseException(string.Format(CultureInfo.InvariantCulture, "A scope with index {0} is invalid at the current scope depth in the expression.", index), expression);
        }

        public IEnumerable<ParameterExpression> Push(Json.ArrayExpression parameters)
        {
            var n = parameters.ElementCount;

            var res = new ParameterExpression[n];

            for (var i = 0; i < n; i++)
            {
                var parameter = parameters.GetElement(i);

                if (parameter is not Json.ArrayExpression decl)
                    throw new BonsaiParseException("Expected JSON array expression for the declaration of a 'Parameter' expression.", parameter);

                var count = decl.ElementCount;
                if (count is not 1 and not 2)
                    throw new BonsaiParseException("Expected 1 or 2 JSON array elements for the type reference and an optional name of a 'Parameter' expression node.", parameter);

                ParameterExpression param;

                var type = GetType(decl.GetElement(0));
                if (count == 2)
                {
                    var name = (string)((Json.ConstantExpression)decl.GetElement(1)).Value;
                    param = Expression.Parameter(type, name);
                }
                else
                {
                    param = Expression.Parameter(type);
                }

                res[i] = param;
            }

            _params.Push(res);

            return res;
        }

        public void Pop()
        {
            _params.Pop();
        }

        public MemberInfo GetMember(Json.Expression expression)
        {
            return _domain.GetMember(expression);
        }

        public LabelTarget GetLabelTarget(Json.Expression expression)
        {
            if (expression is not Json.ConstantExpression indexJson || !int.TryParse(indexJson.Value.ToString(), out int index))
                throw new BonsaiParseException("Expected a JSON number containing a label target reference.", expression);

            if (_labelTagets == null || index < 0 || index >= _labelTagets.Length)
                throw new BonsaiParseException(string.Format(CultureInfo.InvariantCulture, "A label target with index {0} is not defined.", index), expression);

            return _labelTagets[index];
        }

        private ParameterExpression[] GetGlobals(Json.Expression state)
        {
            if (state is not Json.ObjectExpression obj)
                throw new BonsaiParseException("Expected JSON object expression for the Context property of the Bonsai expression.", state);

            if (obj.Members.TryGetValue("Globals", out Json.Expression res))
            {
                return DeserializeGlobals(res);
            }

            return null;
        }

        private ParameterExpression[] DeserializeGlobals(Json.Expression globals)
        {
            if (globals is not Json.ArrayExpression globalTable)
                throw new BonsaiParseException("Expected JSON array expression for the Globals property of the Bonsai expression context.", globals);

            var n = globalTable.ElementCount;

            var d = new ParameterExpression[n];

            for (var i = 0; i < n; i++)
            {
                var globalRow = globalTable.GetElement(i);

                if (globalRow is not Json.ArrayExpression globalJson)
                    throw new BonsaiParseException("Expected JSON array expression for the declaration of a global 'Parameter' node.", globalRow);

                var count = globalJson.ElementCount;

                if (count is not 1 and not 2)
                    throw new BonsaiParseException("Expected 1 or 2 JSON array elements for the type reference and an optional name of a global 'Parameter' expression node.", globalRow);

                var parameterType = _domain.GetType(globalJson.GetElement(0));

                var parameterName = default(string);
                if (count == 2)
                {
                    parameterName = ((Json.ConstantExpression)globalJson.GetElement(1)).Value.ToString();
                }

                d[i] = Expression.Parameter(parameterType, parameterName);
            }

            return d;
        }

        private LabelTarget[] GetLabelTargets(Json.Expression state)
        {
            if (state is not Json.ObjectExpression obj)
                throw new BonsaiParseException("Expected JSON object expression for the Context property of the Bonsai expression.", state);

            if (obj.Members.TryGetValue("LabelTargets", out Json.Expression res))
            {
                return DeserializeLabelTargets(res);
            }

            return null;
        }

        private LabelTarget[] DeserializeLabelTargets(Json.Expression labelTargets)
        {
            if (labelTargets is not Json.ArrayExpression labelTable)
                throw new BonsaiParseException("Expected JSON array expression for the LabelTargets property of the Bonsai expression context.", labelTargets);

            var n = labelTable.ElementCount;

            var d = new LabelTarget[n];

            for (var i = 0; i < n; i++)
            {
                var labelTargetRow = labelTable.GetElement(i);

                if (labelTargetRow is not Json.ArrayExpression labelTargetJson)
                    throw new BonsaiParseException("Expected JSON array expression for the declaration of a 'LabelTarget' node.", labelTargetRow);

                var count = labelTargetJson.ElementCount;

                if (count is not 1 and not 2)
                    throw new BonsaiParseException("Expected 1 or 2 JSON array elements for the type reference and an optional name of a 'LabelTarget' node.", labelTargetRow);

                var labelTargetType = _domain.GetType(labelTargetJson.GetElement(0));

                var labelTargetName = default(string);
                if (count == 2)
                {
                    labelTargetName = ((Json.ConstantExpression)labelTargetJson.GetElement(1)).Value.ToString();
                }

                d[i] = Expression.Label(labelTargetType, labelTargetName);
            }

            return d;
        }

        #endregion
    }
}
