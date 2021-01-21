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
using System.Collections.ObjectModel;
using System.Linq.CompilerServices;
using System.Reflection;

using Json = Nuqleon.Json.Expressions;

namespace System.Linq.Expressions.Bonsai.Serialization
{
    #region Aliases

    //
    // This makes diffing with the original Bonsai serializer easier.
    //

    using LabelTarget = LabelTargetSlim;
    using MemberInfo = MemberInfoSlim;
    using ParameterExpression = ParameterExpressionSlim;
    using Type = TypeSlim;

    #endregion

    internal class SerializationState
    {
        #region Fields

        private readonly SerializationDomain _domain;
        private readonly Stack<Dictionary<ParameterExpression, int>> _params;
        private readonly Dictionary<ParameterExpression, int> _globals;
        private readonly List<ParameterExpression> _globalsDef;
        private readonly Dictionary<LabelTarget, int> _labelTargets;
        private readonly List<LabelTarget> _labelTargetsDef;
        private static readonly Json.Expression s_parameterDiscriminator = Json.Expression.String("$");

        #endregion

        #region Constructors

        public SerializationState(Version version)
        {
            _domain = new SerializationDomain(version);
            _params = new Stack<Dictionary<ParameterExpression, int>>();
            _globals = new Dictionary<ParameterExpression, int>();
            _globalsDef = new List<ParameterExpression>();
            _labelTargets = new Dictionary<LabelTarget, int>();
            _labelTargetsDef = new List<LabelTarget>();
        }

        #endregion

        #region Properties

        public bool IsV08 => _domain.IsV08;

        #endregion

        #region Methods

        public Json.Expression AddMember(MemberInfo member)
        {
            return _domain.AddMember(member);
        }

        public Json.Expression AddType(Type type)
        {
            return _domain.AddType(type).ToJson();
        }

        public Json.Expression AddLabelTarget(LabelTarget labelTarget)
        {
            if (labelTarget == null)
                throw new ArgumentNullException(nameof(labelTarget));

            if (!_labelTargets.TryGetValue(labelTarget, out int index))
            {
                index = _labelTargets.Count;
                _labelTargets.Add(labelTarget, index);
                _labelTargetsDef.Add(labelTarget);
            }

            return index.ToJsonNumber();
        }

        public Json.Expression Lookup(ParameterExpression parameter)
        {
            var scope = 0;
            foreach (var frame in _params)
            {
                if (frame.TryGetValue(parameter, out int index))
                {
                    return Json.Expression.Array(
                        s_parameterDiscriminator,
                        scope.ToJsonNumber(),
                        index.ToJsonNumber()
                    );
                }

                scope++;
            }

            {
                if (!_globals.TryGetValue(parameter, out int index))
                {
                    index = _globals.Count;
                    _globals[parameter] = index;
                    _globalsDef.Add(parameter);
                }

                return Json.Expression.Array(
                    s_parameterDiscriminator,
                    index.ToJsonNumber()
                );
            }
        }

        public ReadOnlyCollection<Json.Expression> Push(IList<ParameterExpression> parameters)
        {
            var count = parameters.Count;

            var res = new Json.Expression[count];

            var env = new Dictionary<ParameterExpression, int>(count);

            for (var i = 0; i < count; ++i)
            {
                var parameter = parameters[i];

                env[parameter] = i;

                var type = _domain.AddType(parameter.Type).ToJson();

                Json.Expression par;

                if (parameter.Name != null)
                {
                    par = Json.Expression.Array(
                        type,
                        Json.Expression.String(parameter.Name)
                    );
                }
                else
                {
                    par = Json.Expression.Array(
                        type
                    );
                }

                res[i] = par;
            }

            _params.Push(env);

            return new TrueReadOnlyCollection<Json.Expression>(/* transfer ownership */ res);
        }

        public void Pop()
        {
            _params.Pop();
        }

        public Json.Expression ToJson()
        {
            var globals = default(Json.Expression);
            if (_globals.Count != 0)
            {
                globals = SerializeGlobals();
            }

            var labelTargets = default(Json.Expression);
            if (_labelTargets.Count != 0)
            {
                labelTargets = SerializeLabelTargets();
            }

            var res = _domain.GetDomainContext();

            if (_globals.Count != 0)
            {
                res.Add("Globals", globals);
            }

            if (_labelTargets.Count != 0)
            {
                res.Add("LabelTargets", labelTargets);
            }

            return Json.Expression.Object(res);
        }

        private Json.Expression SerializeGlobals()
        {
            return Json.Expression.Array(EnumerateGlobals());
        }

        private Json.Expression[] EnumerateGlobals()
        {
            var globals = new Json.Expression[_globalsDef.Count];

            var i = 0;

            foreach (var global in _globalsDef)
            {
                if (global.Name == null)
                {
                    globals[i] = Json.Expression.Array(
                        _domain.AddType(global.Type).ToJson()
                    );
                }
                else
                {
                    globals[i] = Json.Expression.Array(
                        _domain.AddType(global.Type).ToJson(),
                        Json.Expression.String(global.Name)
                    );
                }

                i++;
            }

            return globals;
        }

        private Json.Expression SerializeLabelTargets()
        {
            return Json.Expression.Array(EnumerateLabelTargets());
        }

        private Json.Expression[] EnumerateLabelTargets()
        {
            var labels = new Json.Expression[_labelTargetsDef.Count];

            var i = 0;

            foreach (var labelTarget in _labelTargetsDef)
            {
                if (labelTarget.Name == null)
                {
                    labels[i] = Json.Expression.Array(
                        _domain.AddType(labelTarget.Type).ToJson()
                    );
                }
                else
                {
                    labels[i] = Json.Expression.Array(
                        _domain.AddType(labelTarget.Type).ToJson(),
                        Json.Expression.String(labelTarget.Name)
                    );
                }

                i++;
            }

            return labels;
        }

        #endregion
    }
}
