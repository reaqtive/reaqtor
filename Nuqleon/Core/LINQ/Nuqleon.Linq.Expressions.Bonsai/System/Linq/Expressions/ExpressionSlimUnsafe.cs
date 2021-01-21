// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Globalization;
using System.Linq.CompilerServices;
using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Lightweight representation of expression trees.
    /// </summary>
    [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Lots of factory methods.")]
    internal static partial class ExpressionSlimUnsafe
    {
        #region Fields

        // Do not use the extension method, `ToExpressionSlim()`, as it will ultimately call back into this factory
        private static readonly DefaultExpressionSlim s_void = new(TypeSlimExtensions.VoidType);

        private static readonly TypeSlim s_typeMissing = null;

        #endregion

        #region Factories


        #region ArrayAccess

        public static IndexExpressionSlim ArrayAccess(ExpressionSlim array, params ExpressionSlim[] indexes)
        {
            return ArrayAccess(array, (IEnumerable<ExpressionSlim>)indexes);
        }

        public static IndexExpressionSlim ArrayAccess(ExpressionSlim array, IEnumerable<ExpressionSlim> indexes)
        {
            return new IndexExpressionSlim(array, indexer: null, indexes.ToReadOnlyUnsafe());
        }

        #endregion

        #region Binary

        public static BinaryExpressionSlim MakeBinary(ExpressionType binaryType, ExpressionSlim left, ExpressionSlim right)
        {
            return MakeBinary(binaryType, left, right, liftToNull: false, method: null, conversion: null);
        }

        public static BinaryExpressionSlim MakeBinary(ExpressionType binaryType, ExpressionSlim left, ExpressionSlim right, bool liftToNull, MethodInfoSlim method)
        {
            return MakeBinary(binaryType, left, right, liftToNull, method, conversion: null);
        }

        public static BinaryExpressionSlim MakeBinary(ExpressionType binaryType, ExpressionSlim left, ExpressionSlim right, bool liftToNull, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            return binaryType switch
            {
                ExpressionType.Add => Add(left, right, method),
                ExpressionType.AddChecked => AddChecked(left, right, method),
                ExpressionType.Subtract => Subtract(left, right, method),
                ExpressionType.SubtractChecked => SubtractChecked(left, right, method),
                ExpressionType.Multiply => Multiply(left, right, method),
                ExpressionType.MultiplyChecked => MultiplyChecked(left, right, method),
                ExpressionType.Divide => Divide(left, right, method),
                ExpressionType.Modulo => Modulo(left, right, method),
                ExpressionType.Power => Power(left, right, method),
                ExpressionType.And => And(left, right, method),
                ExpressionType.AndAlso => AndAlso(left, right, method),
                ExpressionType.Or => Or(left, right, method),
                ExpressionType.OrElse => OrElse(left, right, method),
                ExpressionType.LessThan => LessThan(left, right, liftToNull, method),
                ExpressionType.LessThanOrEqual => LessThanOrEqual(left, right, liftToNull, method),
                ExpressionType.GreaterThan => GreaterThan(left, right, liftToNull, method),
                ExpressionType.GreaterThanOrEqual => GreaterThanOrEqual(left, right, liftToNull, method),
                ExpressionType.Equal => Equal(left, right, liftToNull, method),
                ExpressionType.NotEqual => NotEqual(left, right, liftToNull, method),
                ExpressionType.ExclusiveOr => ExclusiveOr(left, right, method),
                ExpressionType.Coalesce => Coalesce(left, right, conversion),
                ExpressionType.ArrayIndex => ArrayIndex(left, right),
                ExpressionType.RightShift => RightShift(left, right, method),
                ExpressionType.LeftShift => LeftShift(left, right, method),
                ExpressionType.Assign => Assign(left, right),
                ExpressionType.AddAssign => AddAssign(left, right, method, conversion),
                ExpressionType.AndAssign => AndAssign(left, right, method, conversion),
                ExpressionType.DivideAssign => DivideAssign(left, right, method, conversion),
                ExpressionType.ExclusiveOrAssign => ExclusiveOrAssign(left, right, method, conversion),
                ExpressionType.LeftShiftAssign => LeftShiftAssign(left, right, method, conversion),
                ExpressionType.ModuloAssign => ModuloAssign(left, right, method, conversion),
                ExpressionType.MultiplyAssign => MultiplyAssign(left, right, method, conversion),
                ExpressionType.OrAssign => OrAssign(left, right, method, conversion),
                ExpressionType.PowerAssign => PowerAssign(left, right, method, conversion),
                ExpressionType.RightShiftAssign => RightShiftAssign(left, right, method, conversion),
                ExpressionType.SubtractAssign => SubtractAssign(left, right, method, conversion),
                ExpressionType.AddAssignChecked => AddAssignChecked(left, right, method, conversion),
                ExpressionType.SubtractAssignChecked => SubtractAssignChecked(left, right, method, conversion),
                ExpressionType.MultiplyAssignChecked => MultiplyAssignChecked(left, right, method, conversion),
                _ => throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Expression type '{0}' is not a valid binary expression type.", binaryType)),
            };
        }

        public static BinaryExpressionSlim Add(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.Add, left, right, liftToNull: true, method: null, conversion: null);
        }

        public static BinaryExpressionSlim Add(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.Add, left, right, liftToNull: true, method, conversion: null);
        }

        public static BinaryExpressionSlim AddAssign(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.AddAssign, left, right, liftToNull: true, method: null, conversion: null);
        }

        public static BinaryExpressionSlim AddAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.AddAssign, left, right, liftToNull: true, method, conversion: null);
        }

        public static BinaryExpressionSlim AddAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            return BinaryExpressionSlim.Make(ExpressionType.AddAssign, left, right, liftToNull: true, method, conversion);
        }

        public static BinaryExpressionSlim AddAssignChecked(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.AddAssignChecked, left, right, liftToNull: true, method: null, conversion: null);
        }

        public static BinaryExpressionSlim AddAssignChecked(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.AddAssignChecked, left, right, liftToNull: true, method, conversion: null);
        }

        public static BinaryExpressionSlim AddAssignChecked(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            return BinaryExpressionSlim.Make(ExpressionType.AddAssignChecked, left, right, liftToNull: true, method, conversion);
        }

        public static BinaryExpressionSlim AddChecked(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.AddChecked, left, right, liftToNull: true, method: null, conversion: null);
        }

        public static BinaryExpressionSlim AddChecked(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.AddChecked, left, right, liftToNull: true, method, conversion: null);
        }

        public static BinaryExpressionSlim And(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.And, left, right, liftToNull: true, method: null, conversion: null);
        }

        public static BinaryExpressionSlim And(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.And, left, right, liftToNull: true, method, conversion: null);
        }

        public static BinaryExpressionSlim AndAssign(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.AndAssign, left, right, liftToNull: true, method: null, conversion: null);
        }

        public static BinaryExpressionSlim AndAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.AndAssign, left, right, liftToNull: true, method, conversion: null);
        }

        public static BinaryExpressionSlim AndAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            return BinaryExpressionSlim.Make(ExpressionType.AndAssign, left, right, liftToNull: true, method, conversion);
        }

        public static BinaryExpressionSlim AndAlso(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.AndAlso, left, right, liftToNull: true, method: null, conversion: null);
        }

        public static BinaryExpressionSlim AndAlso(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.AndAlso, left, right, liftToNull: true, method, conversion: null);
        }

        public static BinaryExpressionSlim ArrayIndex(ExpressionSlim array, ExpressionSlim index)
        {
            return BinaryExpressionSlim.Make(ExpressionType.ArrayIndex, array, index, liftToNull: false, method: null, conversion: null);
        }

        public static BinaryExpressionSlim Assign(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.Assign, left, right, liftToNull: true, method: null, conversion: null);
        }

        public static BinaryExpressionSlim Coalesce(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.Coalesce, left, right, liftToNull: false, method: null, conversion: null);
        }

        public static BinaryExpressionSlim Coalesce(ExpressionSlim left, ExpressionSlim right, LambdaExpressionSlim conversion)
        {
            return BinaryExpressionSlim.Make(ExpressionType.Coalesce, left, right, liftToNull: false, method: null, conversion);
        }

        public static BinaryExpressionSlim Divide(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.Divide, left, right, liftToNull: true, method: null, conversion: null);
        }

        public static BinaryExpressionSlim Divide(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.Divide, left, right, liftToNull: true, method, conversion: null);
        }

        public static BinaryExpressionSlim DivideAssign(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.DivideAssign, left, right, liftToNull: true, method: null, conversion: null);
        }

        public static BinaryExpressionSlim DivideAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.DivideAssign, left, right, liftToNull: true, method, conversion: null);
        }

        public static BinaryExpressionSlim DivideAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            return BinaryExpressionSlim.Make(ExpressionType.DivideAssign, left, right, liftToNull: true, method, conversion);
        }

        public static BinaryExpressionSlim Equal(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.Equal, left, right, liftToNull: false, method: null, conversion: null);
        }

        public static BinaryExpressionSlim Equal(ExpressionSlim left, ExpressionSlim right, bool liftToNull, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.Equal, left, right, liftToNull, method, conversion: null);
        }

        public static BinaryExpressionSlim ExclusiveOr(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.ExclusiveOr, left, right, liftToNull: true, method: null, conversion: null);
        }

        public static BinaryExpressionSlim ExclusiveOr(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.ExclusiveOr, left, right, liftToNull: true, method, conversion: null);
        }

        public static BinaryExpressionSlim ExclusiveOrAssign(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.ExclusiveOrAssign, left, right, liftToNull: true, method: null, conversion: null);
        }

        public static BinaryExpressionSlim ExclusiveOrAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.ExclusiveOrAssign, left, right, liftToNull: true, method, conversion: null);
        }

        public static BinaryExpressionSlim ExclusiveOrAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            return BinaryExpressionSlim.Make(ExpressionType.ExclusiveOrAssign, left, right, liftToNull: true, method, conversion);
        }

        public static BinaryExpressionSlim GreaterThan(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.GreaterThan, left, right, liftToNull: false, method: null, conversion: null);
        }

        public static BinaryExpressionSlim GreaterThan(ExpressionSlim left, ExpressionSlim right, bool liftToNull, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.GreaterThan, left, right, liftToNull, method, conversion: null);
        }

        public static BinaryExpressionSlim GreaterThanOrEqual(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.GreaterThanOrEqual, left, right, liftToNull: false, method: null, conversion: null);
        }

        public static BinaryExpressionSlim GreaterThanOrEqual(ExpressionSlim left, ExpressionSlim right, bool liftToNull, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.GreaterThanOrEqual, left, right, liftToNull, method, conversion: null);
        }

        public static BinaryExpressionSlim LeftShift(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.LeftShift, left, right, liftToNull: true, method: null, conversion: null);
        }

        public static BinaryExpressionSlim LeftShift(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.LeftShift, left, right, liftToNull: true, method, conversion: null);
        }

        public static BinaryExpressionSlim LeftShiftAssign(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.LeftShiftAssign, left, right, liftToNull: true, method: null, conversion: null);
        }

        public static BinaryExpressionSlim LeftShiftAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.LeftShiftAssign, left, right, liftToNull: true, method, conversion: null);
        }

        public static BinaryExpressionSlim LeftShiftAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            return BinaryExpressionSlim.Make(ExpressionType.LeftShiftAssign, left, right, liftToNull: true, method, conversion);
        }

        public static BinaryExpressionSlim LessThan(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.LessThan, left, right, liftToNull: false, method: null, conversion: null);
        }

        public static BinaryExpressionSlim LessThan(ExpressionSlim left, ExpressionSlim right, bool liftToNull, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.LessThan, left, right, liftToNull, method, conversion: null);
        }

        public static BinaryExpressionSlim LessThanOrEqual(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.LessThanOrEqual, left, right, liftToNull: false, method: null, conversion: null);
        }

        public static BinaryExpressionSlim LessThanOrEqual(ExpressionSlim left, ExpressionSlim right, bool liftToNull, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.LessThanOrEqual, left, right, liftToNull, method, conversion: null);
        }

        public static BinaryExpressionSlim Modulo(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.Modulo, left, right, liftToNull: true, method: null, conversion: null);
        }

        public static BinaryExpressionSlim Modulo(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.Modulo, left, right, liftToNull: true, method, conversion: null);
        }

        public static BinaryExpressionSlim ModuloAssign(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.ModuloAssign, left, right, liftToNull: true, method: null, conversion: null);
        }

        public static BinaryExpressionSlim ModuloAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.ModuloAssign, left, right, liftToNull: true, method, conversion: null);
        }

        public static BinaryExpressionSlim ModuloAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            return BinaryExpressionSlim.Make(ExpressionType.ModuloAssign, left, right, liftToNull: true, method, conversion);
        }

        public static BinaryExpressionSlim Multiply(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.Multiply, left, right, liftToNull: true, method: null, conversion: null);
        }

        public static BinaryExpressionSlim Multiply(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.Multiply, left, right, liftToNull: true, method, conversion: null);
        }

        public static BinaryExpressionSlim MultiplyAssign(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.MultiplyAssign, left, right, liftToNull: true, method: null, conversion: null);
        }

        public static BinaryExpressionSlim MultiplyAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.MultiplyAssign, left, right, liftToNull: true, method, conversion: null);
        }

        public static BinaryExpressionSlim MultiplyAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            return BinaryExpressionSlim.Make(ExpressionType.MultiplyAssign, left, right, liftToNull: true, method, conversion);
        }

        public static BinaryExpressionSlim MultiplyChecked(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.MultiplyChecked, left, right, liftToNull: true, method: null, conversion: null);
        }

        public static BinaryExpressionSlim MultiplyChecked(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.MultiplyChecked, left, right, liftToNull: true, method, conversion: null);
        }

        public static BinaryExpressionSlim MultiplyAssignChecked(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.MultiplyAssignChecked, left, right, liftToNull: true, method: null, conversion: null);
        }

        public static BinaryExpressionSlim MultiplyAssignChecked(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.MultiplyAssignChecked, left, right, liftToNull: true, method, conversion: null);
        }

        public static BinaryExpressionSlim MultiplyAssignChecked(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            return BinaryExpressionSlim.Make(ExpressionType.MultiplyAssignChecked, left, right, liftToNull: true, method, conversion);
        }

        public static BinaryExpressionSlim NotEqual(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.NotEqual, left, right, liftToNull: false, method: null, conversion: null);
        }

        public static BinaryExpressionSlim NotEqual(ExpressionSlim left, ExpressionSlim right, bool liftToNull, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.NotEqual, left, right, liftToNull, method, conversion: null);
        }

        public static BinaryExpressionSlim Or(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.Or, left, right, liftToNull: true, method: null, conversion: null);
        }

        public static BinaryExpressionSlim Or(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.Or, left, right, liftToNull: true, method, conversion: null);
        }

        public static BinaryExpressionSlim OrAssign(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.OrAssign, left, right, liftToNull: true, method: null, conversion: null);
        }

        public static BinaryExpressionSlim OrAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.OrAssign, left, right, liftToNull: true, method, conversion: null);
        }

        public static BinaryExpressionSlim OrAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            return BinaryExpressionSlim.Make(ExpressionType.OrAssign, left, right, liftToNull: true, method, conversion);
        }

        public static BinaryExpressionSlim OrElse(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.OrElse, left, right, liftToNull: true, method: null, conversion: null);
        }

        public static BinaryExpressionSlim OrElse(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.OrElse, left, right, liftToNull: true, method, conversion: null);
        }

        public static BinaryExpressionSlim Power(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.Power, left, right, liftToNull: true, method: null, conversion: null);
        }

        public static BinaryExpressionSlim Power(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.Power, left, right, liftToNull: true, method, conversion: null);
        }

        public static BinaryExpressionSlim PowerAssign(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.PowerAssign, left, right, liftToNull: true, method: null, conversion: null);
        }

        public static BinaryExpressionSlim PowerAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.PowerAssign, left, right, liftToNull: true, method, conversion: null);
        }

        public static BinaryExpressionSlim PowerAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            return BinaryExpressionSlim.Make(ExpressionType.PowerAssign, left, right, liftToNull: true, method, conversion);
        }

        public static BinaryExpressionSlim ReferenceEqual(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.Equal, left, right, liftToNull: false, method: null, conversion: null);
        }

        public static BinaryExpressionSlim ReferenceNotEqual(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.NotEqual, left, right, liftToNull: false, method: null, conversion: null);
        }

        public static BinaryExpressionSlim RightShift(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.RightShift, left, right, liftToNull: true, method: null, conversion: null);
        }
        public static BinaryExpressionSlim RightShift(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.RightShift, left, right, liftToNull: true, method, conversion: null);
        }

        public static BinaryExpressionSlim RightShiftAssign(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.RightShiftAssign, left, right, liftToNull: true, method: null, conversion: null);
        }

        public static BinaryExpressionSlim RightShiftAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.RightShiftAssign, left, right, liftToNull: true, method, conversion: null);
        }

        public static BinaryExpressionSlim RightShiftAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            return BinaryExpressionSlim.Make(ExpressionType.RightShiftAssign, left, right, liftToNull: true, method, conversion);
        }

        public static BinaryExpressionSlim Subtract(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.Subtract, left, right, liftToNull: true, method: null, conversion: null);
        }

        public static BinaryExpressionSlim Subtract(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.Subtract, left, right, liftToNull: true, method, conversion: null);
        }

        public static BinaryExpressionSlim SubtractAssign(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.SubtractAssign, left, right, liftToNull: true, method: null, conversion: null);
        }

        public static BinaryExpressionSlim SubtractAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.SubtractAssign, left, right, liftToNull: true, method, conversion: null);
        }

        public static BinaryExpressionSlim SubtractAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            return BinaryExpressionSlim.Make(ExpressionType.SubtractAssign, left, right, liftToNull: true, method, conversion);
        }

        public static BinaryExpressionSlim SubtractChecked(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.SubtractChecked, left, right, liftToNull: true, method: null, conversion: null);
        }

        public static BinaryExpressionSlim SubtractChecked(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.SubtractChecked, left, right, liftToNull: true, method, conversion: null);
        }

        public static BinaryExpressionSlim SubtractAssignChecked(ExpressionSlim left, ExpressionSlim right)
        {
            return BinaryExpressionSlim.Make(ExpressionType.SubtractAssignChecked, left, right, liftToNull: true, method: null, conversion: null);
        }

        public static BinaryExpressionSlim SubtractAssignChecked(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return BinaryExpressionSlim.Make(ExpressionType.SubtractAssignChecked, left, right, liftToNull: true, method, conversion: null);
        }

        public static BinaryExpressionSlim SubtractAssignChecked(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            return BinaryExpressionSlim.Make(ExpressionType.SubtractAssignChecked, left, right, liftToNull: true, method, conversion);
        }

        #endregion

        #region Block

        public static BlockExpressionSlim Block(IEnumerable<ExpressionSlim> expressions)
        {
            return Block(Array.Empty<ParameterExpressionSlim>(), expressions);
        }

        public static BlockExpressionSlim Block(params ExpressionSlim[] expressions)
        {
            return Block(Array.Empty<ParameterExpressionSlim>(), expressions);
        }

        public static BlockExpressionSlim Block(IEnumerable<ParameterExpressionSlim> variables, IEnumerable<ExpressionSlim> expressions)
        {
            var expr = expressions.ToReadOnlyUnsafe();

            return new BlockExpressionSlim(variables.ToReadOnlyUnsafe(), expr, s_typeMissing);
        }

        public static BlockExpressionSlim Block(IEnumerable<ParameterExpressionSlim> variables, params ExpressionSlim[] expressions)
        {
            var expr = expressions.ToReadOnlyUnsafe();

            return new BlockExpressionSlim(variables.ToReadOnlyUnsafe(), expr, s_typeMissing);
        }

        public static BlockExpressionSlim Block(TypeSlim type, IEnumerable<ExpressionSlim> expressions)
        {
            var expr = expressions.ToReadOnlyUnsafe();

            return new BlockExpressionSlim(EmptyReadOnlyCollection<ParameterExpressionSlim>.Instance, expr, type);
        }

        public static BlockExpressionSlim Block(TypeSlim type, params ExpressionSlim[] expressions)
        {
            var expr = expressions.ToReadOnlyUnsafe();

            return new BlockExpressionSlim(EmptyReadOnlyCollection<ParameterExpressionSlim>.Instance, expr, type);
        }

        public static BlockExpressionSlim Block(TypeSlim type, IEnumerable<ParameterExpressionSlim> variables, IEnumerable<ExpressionSlim> expressions)
        {
            var expr = expressions.ToReadOnlyUnsafe();

            return new BlockExpressionSlim(variables.ToReadOnlyUnsafe(), expr, type);
        }

        public static BlockExpressionSlim Block(TypeSlim type, IEnumerable<ParameterExpressionSlim> variables, params ExpressionSlim[] expressions)
        {
            var expr = expressions.ToReadOnlyUnsafe();

            return new BlockExpressionSlim(variables.ToReadOnlyUnsafe(), expr, type);
        }

        #endregion

        #region Break

        public static GotoExpressionSlim Break(LabelTargetSlim target)
        {
            return MakeGoto(GotoExpressionKind.Break, target, value: null, TypeSlimExtensions.VoidType);
        }

        public static GotoExpressionSlim Break(LabelTargetSlim target, ExpressionSlim value)
        {
            return MakeGoto(GotoExpressionKind.Break, target, value, TypeSlimExtensions.VoidType);
        }

        public static GotoExpressionSlim Break(LabelTargetSlim target, TypeSlim type)
        {
            return MakeGoto(GotoExpressionKind.Break, target, value: null, type);
        }

        public static GotoExpressionSlim Break(LabelTargetSlim target, ExpressionSlim value, TypeSlim type)
        {
            return MakeGoto(GotoExpressionKind.Break, target, value, type);
        }

        #endregion

        #region Call

        public static MethodCallExpressionSlim Call(MethodInfoSlim method, params ExpressionSlim[] arguments)
        {
            return Call(instance: null, method, (IEnumerable<ExpressionSlim>)arguments);
        }

        public static MethodCallExpressionSlim Call(MethodInfoSlim method, IEnumerable<ExpressionSlim> arguments)
        {
            return Call(instance: null, method, arguments);
        }

        public static MethodCallExpressionSlim Call(ExpressionSlim instance, MethodInfoSlim method, params ExpressionSlim[] arguments)
        {
            return Call(instance, method, (IEnumerable<ExpressionSlim>)arguments);
        }

        public static MethodCallExpressionSlim Call(ExpressionSlim instance, MethodInfoSlim method, IEnumerable<ExpressionSlim> arguments)
        {
            var argList = arguments as IList<ExpressionSlim>;

            if (instance == null)
            {
                if (argList != null)
                {
                    switch (argList.Count)
                    {
                        case 0:
                            return Call(method);
                        case 1:
                            return Call(method, argList[0]);
                        case 2:
                            return Call(method, argList[0], argList[1]);
                        case 3:
                            return Call(method, argList[0], argList[1], argList[2]);
                        case 4:
                            return Call(method, argList[0], argList[1], argList[2], argList[3]);
                        case 5:
                            return Call(method, argList[0], argList[1], argList[2], argList[3], argList[4]);
                    }
                }

                return new StaticMethodCallExpressionSlimN(method, arguments.ToReadOnlyUnsafe());
            }
            else
            {
                if (argList != null)
                {
                    switch (argList.Count)
                    {
                        case 0:
                            return Call(instance, method);
                        case 1:
                            return Call(instance, method, argList[0]);
                        case 2:
                            return Call(instance, method, argList[0], argList[1]);
                        case 3:
                            return Call(instance, method, argList[0], argList[1], argList[2]);
                        case 4:
                            return Call(instance, method, argList[0], argList[1], argList[2], argList[3]);
                        case 5:
                            return Call(instance, method, argList[0], argList[1], argList[2], argList[3], argList[4]);
                    }
                }

                return new InstanceMethodCallExpressionSlimN(instance, method, arguments.ToReadOnlyUnsafe());
            }
        }

        #endregion

        #region Catch

        public static CatchBlockSlim Catch(TypeSlim type, ExpressionSlim body)
        {
            return MakeCatchBlock(type, variable: null, body, filter: null);
        }

        public static CatchBlockSlim Catch(ParameterExpressionSlim variable, ExpressionSlim body)
        {
            return MakeCatchBlock(variable.Type, variable, body, filter: null);
        }

        public static CatchBlockSlim Catch(TypeSlim type, ExpressionSlim body, ExpressionSlim filter)
        {
            return MakeCatchBlock(type, variable: null, body, filter);
        }

        public static CatchBlockSlim Catch(ParameterExpressionSlim variable, ExpressionSlim body, ExpressionSlim filter)
        {
            return MakeCatchBlock(variable.Type, variable, body, filter);
        }

        #endregion

        #region Condition

        public static ConditionalExpressionSlim Condition(ExpressionSlim test, ExpressionSlim ifTrue, ExpressionSlim ifFalse)
        {
            return new ConditionalExpressionSlim(test, ifTrue, ifFalse, s_typeMissing);
        }

        public static ConditionalExpressionSlim Condition(ExpressionSlim test, ExpressionSlim ifTrue, ExpressionSlim ifFalse, TypeSlim type)
        {
            return new ConditionalExpressionSlim(test, ifTrue, ifFalse, type);
        }

        public static ConditionalExpressionSlim IfThen(ExpressionSlim test, ExpressionSlim ifTrue)
        {
            return new ConditionalExpressionSlim(test, ifTrue, Empty(), TypeSlimExtensions.VoidType);
        }

        public static ConditionalExpressionSlim IfThenElse(ExpressionSlim test, ExpressionSlim ifTrue, ExpressionSlim ifFalse)
        {
            return new ConditionalExpressionSlim(test, ifTrue, ifFalse, TypeSlimExtensions.VoidType);
        }

        #endregion

        #region Constant

        public static ConstantExpressionSlim Constant(ObjectSlim value)
        {
            return new ConstantExpressionSlim(value);
        }

        public static ConstantExpressionSlim Constant(ObjectSlim value, TypeSlim type)
        {
            return ConstantExpressionSlim.Make(value, type);
        }

        #endregion

        #region Continue

        public static GotoExpressionSlim Continue(LabelTargetSlim target)
        {
            return MakeGoto(GotoExpressionKind.Continue, target, value: null, TypeSlimExtensions.VoidType);
        }

        public static GotoExpressionSlim Continue(LabelTargetSlim target, TypeSlim type)
        {
            return MakeGoto(GotoExpressionKind.Continue, target, value: null, type);
        }

        #endregion

        #region Default

        public static DefaultExpressionSlim Default(TypeSlim type)
        {
            if (s_void.Type.Equals(type))
            {
                return s_void;
            }

            return new DefaultExpressionSlim(type);
        }

        public static DefaultExpressionSlim Empty()
        {
            return s_void;
        }

        #endregion

        #region ElementInit

        public static ElementInitSlim ElementInit(MethodInfoSlim addMethod, params ExpressionSlim[] arguments)
        {
            return new ElementInitSlim(addMethod, arguments.ToReadOnlyUnsafe());
        }

        public static ElementInitSlim ElementInit(MethodInfoSlim addMethod, IEnumerable<ExpressionSlim> arguments)
        {
            return new ElementInitSlim(addMethod, arguments.ToReadOnlyUnsafe());
        }

        #endregion

        #region Goto

        public static GotoExpressionSlim Goto(LabelTargetSlim target)
        {
            return MakeGoto(GotoExpressionKind.Goto, target, value: null, TypeSlimExtensions.VoidType);
        }

        public static GotoExpressionSlim Goto(LabelTargetSlim target, TypeSlim type)
        {
            return MakeGoto(GotoExpressionKind.Goto, target, value: null, type);
        }

        public static GotoExpressionSlim Goto(LabelTargetSlim target, ExpressionSlim value)
        {
            return MakeGoto(GotoExpressionKind.Goto, target, value, TypeSlimExtensions.VoidType);
        }

        public static GotoExpressionSlim Goto(LabelTargetSlim target, ExpressionSlim value, TypeSlim type)
        {
            return MakeGoto(GotoExpressionKind.Goto, target, value, type);
        }

        #endregion

        #region Invoke

        public static InvocationExpressionSlim Invoke(ExpressionSlim expression, params ExpressionSlim[] arguments)
        {
            return Invoke(expression, (IEnumerable<ExpressionSlim>)arguments);
        }

        public static InvocationExpressionSlim Invoke(ExpressionSlim expression, IEnumerable<ExpressionSlim> arguments)
        {
            if (arguments is IList<ExpressionSlim> argList)
            {
                switch (argList.Count)
                {
                    case 0:
                        return Invoke(expression);
                    case 1:
                        return Invoke(expression, argList[0]);
                    case 2:
                        return Invoke(expression, argList[0], argList[1]);
                    case 3:
                        return Invoke(expression, argList[0], argList[1], argList[2]);
                    case 4:
                        return Invoke(expression, argList[0], argList[1], argList[2], argList[3]);
                    case 5:
                        return Invoke(expression, argList[0], argList[1], argList[2], argList[3], argList[4]);
                }
            }

            return new InvocationExpressionSlimN(expression, arguments.ToReadOnlyUnsafe());
        }

        #endregion

        #region Label

        public static LabelExpressionSlim Label(LabelTargetSlim target)
        {
            return new LabelExpressionSlim(target, defaultValue: null);
        }

        public static LabelExpressionSlim Label(LabelTargetSlim target, ExpressionSlim defaultValue)
        {
            return new LabelExpressionSlim(target, defaultValue);
        }

        #endregion

        #region LabelTarget

        public static LabelTargetSlim Label()
        {
            return new LabelTargetSlim(TypeSlimExtensions.VoidType, name: null);
        }

        public static LabelTargetSlim Label(string name)
        {
            return new LabelTargetSlim(TypeSlimExtensions.VoidType, name);
        }

        public static LabelTargetSlim Label(TypeSlim type)
        {
            return new LabelTargetSlim(type, name: null);
        }

        public static LabelTargetSlim Label(TypeSlim type, string name)
        {
            return new LabelTargetSlim(type, name);
        }

        #endregion

        #region Lambda

        public static LambdaExpressionSlim Lambda(ExpressionSlim body, params ParameterExpressionSlim[] parameters)
        {
            return new LambdaExpressionSlim(s_typeMissing, body, parameters.ToReadOnlyUnsafe());
        }

        public static LambdaExpressionSlim Lambda(ExpressionSlim body, IEnumerable<ParameterExpressionSlim> parameters)
        {
            return new LambdaExpressionSlim(s_typeMissing, body, parameters.ToReadOnlyUnsafe());
        }

        public static LambdaExpressionSlim Lambda(TypeSlim delegateType, ExpressionSlim body, params ParameterExpressionSlim[] parameters)
        {
            return new LambdaExpressionSlim(delegateType, body, parameters.ToReadOnlyUnsafe());
        }

        public static LambdaExpressionSlim Lambda(TypeSlim delegateType, ExpressionSlim body, IEnumerable<ParameterExpressionSlim> parameters)
        {
            return new LambdaExpressionSlim(delegateType, body, parameters.ToReadOnlyUnsafe());
        }

        #endregion

        #region ListInit

        public static ListInitExpressionSlim ListInit(NewExpressionSlim newExpression, params ElementInitSlim[] initializers)
        {
            return new ListInitExpressionSlim(newExpression, initializers.ToReadOnlyUnsafe());
        }

        public static ListInitExpressionSlim ListInit(NewExpressionSlim newExpression, IEnumerable<ElementInitSlim> initializers)
        {
            return new ListInitExpressionSlim(newExpression, initializers.ToReadOnlyUnsafe());
        }

        public static ListInitExpressionSlim ListInit(NewExpressionSlim newExpression, MethodInfoSlim addMethod, params ExpressionSlim[] initializers)
        {
            var elementInitializers = new ElementInitSlim[initializers.Length];
            for (var i = 0; i < initializers.Length; ++i)
            {
                var arguments = new[] { initializers[i] };
                elementInitializers[i] = new ElementInitSlim(addMethod, new TrueReadOnlyCollection<ExpressionSlim>(/* transfer ownership */ arguments));
            }

            return new ListInitExpressionSlim(newExpression, new TrueReadOnlyCollection<ElementInitSlim>(/* transfer ownership */ elementInitializers));
        }

        public static ListInitExpressionSlim ListInit(NewExpressionSlim newExpression, MethodInfoSlim addMethod, IEnumerable<ExpressionSlim> initializers)
        {
            var initializersList = initializers.ToList();
            var elementInitializers = new ElementInitSlim[initializersList.Count];
            for (var i = 0; i < initializersList.Count; ++i)
            {
                var arguments = new[] { initializersList[i] };
                elementInitializers[i] = new ElementInitSlim(addMethod, new TrueReadOnlyCollection<ExpressionSlim>(/* transfer ownership */ arguments));
            }

            return new ListInitExpressionSlim(newExpression, new TrueReadOnlyCollection<ElementInitSlim>(/* transfer ownership */ elementInitializers));
        }

        #endregion

        #region Loop

        public static LoopExpressionSlim Loop(ExpressionSlim body)
        {
            return new LoopExpressionSlim(body, @break: null, @continue: null);
        }

        public static LoopExpressionSlim Loop(ExpressionSlim body, LabelTargetSlim @break)
        {
            return new LoopExpressionSlim(body, @break, @continue: null);
        }

        public static LoopExpressionSlim Loop(ExpressionSlim body, LabelTargetSlim @break, LabelTargetSlim @continue)
        {
            return new LoopExpressionSlim(body, @break, @continue);
        }

        #endregion

        #region MakeCatchBlock

        public static CatchBlockSlim MakeCatchBlock(TypeSlim type, ParameterExpressionSlim variable, ExpressionSlim body, ExpressionSlim filter)
        {
            return new CatchBlockSlim(type, variable, body, filter);
        }

        #endregion

        #region MakeGoto

        public static GotoExpressionSlim MakeGoto(GotoExpressionKind kind, LabelTargetSlim target, ExpressionSlim value, TypeSlim type)
        {
            return new GotoExpressionSlim(kind, target, value, type);
        }

        #endregion

        #region MakeIndex

        public static IndexExpressionSlim MakeIndex(ExpressionSlim instance, PropertyInfoSlim indexer, IEnumerable<ExpressionSlim> arguments)
        {
            return new IndexExpressionSlim(instance, indexer, arguments.ToReadOnlyUnsafe());
        }

        public static IndexExpressionSlim Property(ExpressionSlim instance, PropertyInfoSlim indexer, params ExpressionSlim[] arguments)
        {
            return new IndexExpressionSlim(instance, indexer, arguments.ToReadOnlyUnsafe());
        }

        public static IndexExpressionSlim Property(ExpressionSlim instance, PropertyInfoSlim indexer, IEnumerable<ExpressionSlim> arguments)
        {
            return new IndexExpressionSlim(instance, indexer, arguments.ToReadOnlyUnsafe());
        }

        #endregion

        #region MakeTry

        public static TryExpressionSlim MakeTry(TypeSlim type, ExpressionSlim body, ExpressionSlim @finally, ExpressionSlim fault, IEnumerable<CatchBlockSlim> handlers)
        {
            var catchBlocks = handlers != null ? handlers.ToReadOnlyUnsafe() : EmptyReadOnlyCollection<CatchBlockSlim>.Instance;

            if (fault != null)
            {
                if (@finally != null || catchBlocks.Count > 0)
                {
                    throw new ArgumentException("A try expression with a fault block can't have a finally block or any catch blocks.");
                }
            }
            else
            {
                if (@finally == null && catchBlocks.Count == 0)
                {
                    throw new ArgumentException("A try expression without any catch blocks should either have a fault block or a finally block.");
                }
            }

            return new TryExpressionSlim(type, body, @finally, fault, catchBlocks);
        }

        #endregion

        #region Member

        public static MemberExpressionSlim Field(ExpressionSlim instance, FieldInfoSlim field)
        {
            return MemberExpressionSlim.Make(instance, field);
        }

        public static MemberExpressionSlim MakeMemberAccess(ExpressionSlim instance, MemberInfoSlim member)
        {
            return MemberExpressionSlim.Make(instance, member);
        }

        public static MemberExpressionSlim Property(ExpressionSlim instance, PropertyInfoSlim property)
        {
            return MemberExpressionSlim.Make(instance, property);
        }

        #endregion

        #region MemberAssignment

        public static MemberAssignmentSlim Bind(MemberInfoSlim member, ExpressionSlim expression)
        {
            return new MemberAssignmentSlim(member, expression);
        }

        #endregion

        #region MemberInit

        public static MemberInitExpressionSlim MemberInit(NewExpressionSlim newExpression, params MemberBindingSlim[] bindings)
        {
            return new MemberInitExpressionSlim(newExpression, bindings.ToReadOnlyUnsafe());
        }

        public static MemberInitExpressionSlim MemberInit(NewExpressionSlim newExpression, IEnumerable<MemberBindingSlim> bindings)
        {
            return new MemberInitExpressionSlim(newExpression, bindings.ToReadOnlyUnsafe());
        }

        #endregion

        #region MemberMemberBind

        public static MemberMemberBindingSlim MemberBind(MemberInfoSlim member, params MemberBindingSlim[] bindings)
        {
            return new MemberMemberBindingSlim(member, bindings.ToReadOnlyUnsafe());
        }

        public static MemberMemberBindingSlim MemberBind(MemberInfoSlim member, IEnumerable<MemberBindingSlim> bindings)
        {
            return new MemberMemberBindingSlim(member, bindings.ToReadOnlyUnsafe());
        }

        #endregion

        #region MemberListBind

        public static MemberListBindingSlim ListBind(MemberInfoSlim member, params ElementInitSlim[] initializers)
        {
            return new MemberListBindingSlim(member, initializers.ToReadOnlyUnsafe());
        }

        public static MemberListBindingSlim ListBind(MemberInfoSlim member, IEnumerable<ElementInitSlim> initializers)
        {
            return new MemberListBindingSlim(member, initializers.ToReadOnlyUnsafe());
        }

        #endregion

        #region New

        public static NewExpressionSlim New(ConstructorInfoSlim constructor, params ExpressionSlim[] arguments)
        {
            return New(constructor, arguments, default(IEnumerable<MemberInfoSlim>));
        }

        public static NewExpressionSlim New(ConstructorInfoSlim constructor, IEnumerable<ExpressionSlim> arguments)
        {
            return New(constructor, arguments, default(IEnumerable<MemberInfoSlim>));
        }

        public static NewExpressionSlim New(ConstructorInfoSlim constructor, IEnumerable<ExpressionSlim> arguments, params MemberInfoSlim[] members)
        {
            return New(constructor, arguments, (IEnumerable<MemberInfoSlim>)members);
        }

        public static NewExpressionSlim New(ConstructorInfoSlim constructor, IEnumerable<ExpressionSlim> arguments, IEnumerable<MemberInfoSlim> members)
        {
            if (members != null)
            {
                return new FullNewReferenceTypeExpressionSlimWithMembers(constructor, arguments.ToReadOnlyUnsafe(), members.ToReadOnlyUnsafe());
            }
            else
            {
                if (arguments is IList<ExpressionSlim> argList)
                {
                    switch (argList.Count)
                    {
                        case 0:
                            return New(constructor);
                        case 1:
                            return New(constructor, argList[0]);
                        case 2:
                            return New(constructor, argList[0], argList[1]);
                        case 3:
                            return New(constructor, argList[0], argList[1], argList[2]);
                        case 4:
                            return New(constructor, argList[0], argList[1], argList[2], argList[3]);
                        case 5:
                            return New(constructor, argList[0], argList[1], argList[2], argList[3], argList[4]);
                    }
                }

                return new FullNewReferenceTypeExpressionSlim(constructor, arguments.ToReadOnlyUnsafe());
            }
        }

        public static NewExpressionSlim New(TypeSlim type)
        {
            return new NewValueTypeExpressionSlim(type);
        }

        #endregion

        #region NewArray

        public static NewArrayExpressionSlim NewArrayBounds(TypeSlim elementType, params ExpressionSlim[] bounds)
        {
            return new NewArrayBoundsExpressionSlim(elementType, bounds.ToReadOnlyUnsafe());
        }

        public static NewArrayExpressionSlim NewArrayBounds(TypeSlim elementType, IEnumerable<ExpressionSlim> bounds)
        {
            return new NewArrayBoundsExpressionSlim(elementType, bounds.ToReadOnlyUnsafe());
        }

        public static NewArrayExpressionSlim NewArrayInit(TypeSlim elementType, params ExpressionSlim[] initializers)
        {
            return new NewArrayInitExpressionSlim(elementType, initializers.ToReadOnlyUnsafe());
        }

        public static NewArrayExpressionSlim NewArrayInit(TypeSlim elementType, IEnumerable<ExpressionSlim> initializers)
        {
            return new NewArrayInitExpressionSlim(elementType, initializers.ToReadOnlyUnsafe());
        }

        #endregion

        #region Parameter

        public static ParameterExpressionSlim Parameter(TypeSlim type)
        {
            return new ParameterExpressionSlim(type, name: null);
        }

        public static ParameterExpressionSlim Parameter(TypeSlim type, string name)
        {
            return new ParameterExpressionSlim(type, name);
        }

        #endregion

        #region Return

        public static GotoExpressionSlim Return(LabelTargetSlim target)
        {
            return MakeGoto(GotoExpressionKind.Return, target, value: null, TypeSlimExtensions.VoidType);
        }

        public static GotoExpressionSlim Return(LabelTargetSlim target, ExpressionSlim value)
        {
            return MakeGoto(GotoExpressionKind.Return, target, value, TypeSlimExtensions.VoidType);
        }

        public static GotoExpressionSlim Return(LabelTargetSlim target, TypeSlim type)
        {
            return MakeGoto(GotoExpressionKind.Return, target, value: null, type);
        }

        public static GotoExpressionSlim Return(LabelTargetSlim target, ExpressionSlim value, TypeSlim type)
        {
            return MakeGoto(GotoExpressionKind.Return, target, value, type);
        }

        #endregion

        #region Switch

        public static SwitchExpressionSlim Switch(ExpressionSlim switchValue, params SwitchCaseSlim[] cases)
        {
            return Switch(s_typeMissing, switchValue, defaultBody: null, comparison: null, (IEnumerable<SwitchCaseSlim>)cases);
        }

        public static SwitchExpressionSlim Switch(ExpressionSlim switchValue, ExpressionSlim defaultBody, params SwitchCaseSlim[] cases)
        {
            return Switch(s_typeMissing, switchValue, defaultBody, comparison: null, (IEnumerable<SwitchCaseSlim>)cases);
        }

        public static SwitchExpressionSlim Switch(ExpressionSlim switchValue, ExpressionSlim defaultBody, MethodInfoSlim comparison, params SwitchCaseSlim[] cases)
        {
            return Switch(s_typeMissing, switchValue, defaultBody, comparison, (IEnumerable<SwitchCaseSlim>)cases);
        }

        public static SwitchExpressionSlim Switch(TypeSlim type, ExpressionSlim switchValue, ExpressionSlim defaultBody, MethodInfoSlim comparison, params SwitchCaseSlim[] cases)
        {
            return Switch(type, switchValue, defaultBody, comparison, (IEnumerable<SwitchCaseSlim>)cases);
        }

        public static SwitchExpressionSlim Switch(ExpressionSlim switchValue, ExpressionSlim defaultBody, MethodInfoSlim comparison, IEnumerable<SwitchCaseSlim> cases)
        {
            return Switch(s_typeMissing, switchValue, defaultBody, comparison, cases);
        }

        public static SwitchExpressionSlim Switch(TypeSlim type, ExpressionSlim switchValue, ExpressionSlim defaultBody, MethodInfoSlim comparison, IEnumerable<SwitchCaseSlim> cases)
        {
            return new SwitchExpressionSlim(type, switchValue, defaultBody, comparison, cases.ToReadOnlyUnsafe());
        }

        #endregion

        #region SwitchCase

        public static SwitchCaseSlim SwitchCase(ExpressionSlim body, params ExpressionSlim[] testValues)
        {
            return SwitchCase(body, (IEnumerable<ExpressionSlim>)testValues);
        }

        public static SwitchCaseSlim SwitchCase(ExpressionSlim body, IEnumerable<ExpressionSlim> testValues)
        {
            var t = testValues.ToReadOnlyUnsafe();

            return new SwitchCaseSlim(body, t);
        }

        #endregion

        #region Try

        public static TryExpressionSlim TryCatch(ExpressionSlim body, params CatchBlockSlim[] handlers)
        {
            return MakeTry(s_typeMissing, body, @finally: null, fault: null, handlers);
        }

        public static TryExpressionSlim TryCatchFinally(ExpressionSlim body, ExpressionSlim @finally, params CatchBlockSlim[] handlers)
        {
            return MakeTry(s_typeMissing, body, @finally, fault: null, handlers);
        }

        public static TryExpressionSlim TryFault(ExpressionSlim body, ExpressionSlim fault)
        {
            return MakeTry(s_typeMissing, body, @finally: null, fault, handlers: null);
        }

        public static TryExpressionSlim TryFinally(ExpressionSlim body, ExpressionSlim @finally)
        {
            return MakeTry(s_typeMissing, body, @finally, fault: null, handlers: null);
        }

        #endregion

        #region TypeEqual

        public static TypeBinaryExpressionSlim TypeEqual(ExpressionSlim expression, TypeSlim type)
        {
            return TypeBinaryExpressionSlim.Make(ExpressionType.TypeEqual, expression, type);
        }

        #endregion

        #region TypeIs

        public static TypeBinaryExpressionSlim TypeIs(ExpressionSlim expression, TypeSlim type)
        {
            return TypeBinaryExpressionSlim.Make(ExpressionType.TypeIs, expression, type);
        }

        #endregion

        #region Unary

        public static UnaryExpressionSlim MakeUnary(ExpressionType unaryType, ExpressionSlim operand, TypeSlim type)
        {
            return MakeUnary(unaryType, operand, type, method: null);
        }

        public static UnaryExpressionSlim MakeUnary(ExpressionType unaryType, ExpressionSlim operand, TypeSlim type, MethodInfoSlim method)
        {
            return unaryType switch
            {
                ExpressionType.Negate => Negate(operand, method),
                ExpressionType.NegateChecked => NegateChecked(operand, method),
                ExpressionType.Not => Not(operand, method),
                ExpressionType.IsFalse => IsFalse(operand, method),
                ExpressionType.IsTrue => IsTrue(operand, method),
                ExpressionType.OnesComplement => OnesComplement(operand, method),
                ExpressionType.ArrayLength => ArrayLength(operand),
                ExpressionType.Convert => Convert(operand, type, method),
                ExpressionType.ConvertChecked => ConvertChecked(operand, type, method),
                ExpressionType.Throw => Throw(operand, type),
                ExpressionType.TypeAs => TypeAs(operand, type),
                ExpressionType.Quote => Quote(operand),
                ExpressionType.UnaryPlus => UnaryPlus(operand, method),
                ExpressionType.Unbox => Unbox(operand, type),
                ExpressionType.Increment => Increment(operand, method),
                ExpressionType.Decrement => Decrement(operand, method),
                ExpressionType.PreIncrementAssign => PreIncrementAssign(operand, method),
                ExpressionType.PostIncrementAssign => PostIncrementAssign(operand, method),
                ExpressionType.PreDecrementAssign => PreDecrementAssign(operand, method),
                ExpressionType.PostDecrementAssign => PostDecrementAssign(operand, method),
                _ => throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Expression type '{0}' is not a valid unary expression type.", unaryType)),
            };
        }

        public static UnaryExpressionSlim ArrayLength(ExpressionSlim array)
        {
            return UnaryExpressionSlim.Make(ExpressionType.ArrayLength, array, method: null, s_typeMissing);
        }

        public static UnaryExpressionSlim Convert(ExpressionSlim expression, TypeSlim type)
        {
            return UnaryExpressionSlim.Make(ExpressionType.Convert, expression, method: null, type);
        }

        public static UnaryExpressionSlim Convert(ExpressionSlim expression, TypeSlim type, MethodInfoSlim method)
        {
            return UnaryExpressionSlim.Make(ExpressionType.Convert, expression, method, type);
        }

        public static UnaryExpressionSlim ConvertChecked(ExpressionSlim expression, TypeSlim type)
        {
            return UnaryExpressionSlim.Make(ExpressionType.ConvertChecked, expression, method: null, type);
        }

        public static UnaryExpressionSlim ConvertChecked(ExpressionSlim expression, TypeSlim type, MethodInfoSlim method)
        {
            return UnaryExpressionSlim.Make(ExpressionType.ConvertChecked, expression, method, type);
        }

        public static UnaryExpressionSlim Decrement(ExpressionSlim expression)
        {
            return UnaryExpressionSlim.Make(ExpressionType.Decrement, expression, method: null, s_typeMissing);
        }

        public static UnaryExpressionSlim Decrement(ExpressionSlim expression, MethodInfoSlim method)
        {
            return UnaryExpressionSlim.Make(ExpressionType.Decrement, expression, method, s_typeMissing);
        }

        public static UnaryExpressionSlim Increment(ExpressionSlim expression)
        {
            return UnaryExpressionSlim.Make(ExpressionType.Increment, expression, method: null, s_typeMissing);
        }

        public static UnaryExpressionSlim Increment(ExpressionSlim expression, MethodInfoSlim method)
        {
            return UnaryExpressionSlim.Make(ExpressionType.Increment, expression, method, s_typeMissing);
        }

        public static UnaryExpressionSlim IsFalse(ExpressionSlim expression)
        {
            return UnaryExpressionSlim.Make(ExpressionType.IsFalse, expression, method: null, s_typeMissing);
        }

        public static UnaryExpressionSlim IsFalse(ExpressionSlim expression, MethodInfoSlim method)
        {
            return UnaryExpressionSlim.Make(ExpressionType.IsFalse, expression, method, s_typeMissing);
        }

        public static UnaryExpressionSlim IsTrue(ExpressionSlim expression)
        {
            return UnaryExpressionSlim.Make(ExpressionType.IsTrue, expression, method: null, s_typeMissing);
        }

        public static UnaryExpressionSlim IsTrue(ExpressionSlim expression, MethodInfoSlim method)
        {
            return UnaryExpressionSlim.Make(ExpressionType.IsTrue, expression, method, s_typeMissing);
        }

        public static UnaryExpressionSlim Negate(ExpressionSlim expression)
        {
            return UnaryExpressionSlim.Make(ExpressionType.Negate, expression, method: null, s_typeMissing);
        }

        public static UnaryExpressionSlim Negate(ExpressionSlim expression, MethodInfoSlim method)
        {
            return UnaryExpressionSlim.Make(ExpressionType.Negate, expression, method, s_typeMissing);
        }

        public static UnaryExpressionSlim NegateChecked(ExpressionSlim expression)
        {
            return UnaryExpressionSlim.Make(ExpressionType.NegateChecked, expression, method: null, s_typeMissing);
        }

        public static UnaryExpressionSlim NegateChecked(ExpressionSlim expression, MethodInfoSlim method)
        {
            return UnaryExpressionSlim.Make(ExpressionType.NegateChecked, expression, method, s_typeMissing);
        }

        public static UnaryExpressionSlim Not(ExpressionSlim expression)
        {
            return UnaryExpressionSlim.Make(ExpressionType.Not, expression, method: null, s_typeMissing);
        }

        public static UnaryExpressionSlim Not(ExpressionSlim expression, MethodInfoSlim method)
        {
            return UnaryExpressionSlim.Make(ExpressionType.Not, expression, method, s_typeMissing);
        }

        public static UnaryExpressionSlim OnesComplement(ExpressionSlim expression)
        {
            return UnaryExpressionSlim.Make(ExpressionType.OnesComplement, expression, method: null, s_typeMissing);
        }

        public static UnaryExpressionSlim OnesComplement(ExpressionSlim expression, MethodInfoSlim method)
        {
            return UnaryExpressionSlim.Make(ExpressionType.OnesComplement, expression, method, s_typeMissing);
        }

        public static UnaryExpressionSlim PreDecrementAssign(ExpressionSlim expression)
        {
            return UnaryExpressionSlim.Make(ExpressionType.PreDecrementAssign, expression, method: null, s_typeMissing);
        }

        public static UnaryExpressionSlim PreDecrementAssign(ExpressionSlim expression, MethodInfoSlim method)
        {
            return UnaryExpressionSlim.Make(ExpressionType.PreDecrementAssign, expression, method, s_typeMissing);
        }

        public static UnaryExpressionSlim PreIncrementAssign(ExpressionSlim expression)
        {
            return UnaryExpressionSlim.Make(ExpressionType.PreIncrementAssign, expression, method: null, s_typeMissing);
        }

        public static UnaryExpressionSlim PreIncrementAssign(ExpressionSlim expression, MethodInfoSlim method)
        {
            return UnaryExpressionSlim.Make(ExpressionType.PreIncrementAssign, expression, method, s_typeMissing);
        }

        public static UnaryExpressionSlim PostDecrementAssign(ExpressionSlim expression)
        {
            return UnaryExpressionSlim.Make(ExpressionType.PostDecrementAssign, expression, method: null, s_typeMissing);
        }

        public static UnaryExpressionSlim PostDecrementAssign(ExpressionSlim expression, MethodInfoSlim method)
        {
            return UnaryExpressionSlim.Make(ExpressionType.PostDecrementAssign, expression, method, s_typeMissing);
        }

        public static UnaryExpressionSlim PostIncrementAssign(ExpressionSlim expression)
        {
            return UnaryExpressionSlim.Make(ExpressionType.PostIncrementAssign, expression, method: null, s_typeMissing);
        }

        public static UnaryExpressionSlim PostIncrementAssign(ExpressionSlim expression, MethodInfoSlim method)
        {
            return UnaryExpressionSlim.Make(ExpressionType.PostIncrementAssign, expression, method, s_typeMissing);
        }

        public static UnaryExpressionSlim Quote(ExpressionSlim expression)
        {
            return UnaryExpressionSlim.Make(ExpressionType.Quote, expression, method: null, s_typeMissing /* TODO - add equivalent of GetType call? */);
        }

        public static UnaryExpressionSlim Rethrow()
        {
            return UnaryExpressionSlim.Make(ExpressionType.Throw, operand: null, method: null, TypeSlimExtensions.VoidType);
        }

        public static UnaryExpressionSlim Rethrow(TypeSlim type)
        {
            return UnaryExpressionSlim.Make(ExpressionType.Throw, operand: null, method: null, type ?? TypeSlimExtensions.VoidType);
        }

        public static UnaryExpressionSlim Throw(ExpressionSlim expression)
        {
            return UnaryExpressionSlim.Make(ExpressionType.Throw, expression, method: null, TypeSlimExtensions.VoidType);
        }

        public static UnaryExpressionSlim Throw(ExpressionSlim expression, TypeSlim type)
        {
            return UnaryExpressionSlim.Make(ExpressionType.Throw, expression, method: null, type ?? TypeSlimExtensions.VoidType);
        }

        public static UnaryExpressionSlim TypeAs(ExpressionSlim expression, TypeSlim type)
        {
            return UnaryExpressionSlim.Make(ExpressionType.TypeAs, expression, method: null, type);
        }

        public static UnaryExpressionSlim UnaryPlus(ExpressionSlim expression)
        {
            return UnaryExpressionSlim.Make(ExpressionType.UnaryPlus, expression, method: null, s_typeMissing);
        }

        public static UnaryExpressionSlim UnaryPlus(ExpressionSlim expression, MethodInfoSlim method)
        {
            return UnaryExpressionSlim.Make(ExpressionType.UnaryPlus, expression, method, s_typeMissing);
        }

        public static UnaryExpressionSlim Unbox(ExpressionSlim expression, TypeSlim type)
        {
            return UnaryExpressionSlim.Make(ExpressionType.Unbox, expression, method: null, type);
        }

        #endregion

#pragma warning restore

        #endregion
    }
}
