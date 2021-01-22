// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

using Json = Nuqleon.Json.Expressions;

namespace System.Linq.Expressions.Bonsai.Serialization
{
    internal static class Discriminators
    {
        public static class Expression
        {
            public const string Constant = ":";
            public const string Default = "default";
            public const string OnesComplement = "~";
            public const string Decrement = "--";
            public const string Increment = "++";
            public const string Not = "!";
            public const string Convert = "<:";
            public const string ConvertChecked = "<:$";
            public const string TypeAs = "as";
            public const string Quote = "`";
            public const string Plus = "+";
            public const string PlusDollar = "+$";
            public const string Add = Plus;
            public const string AddChecked = PlusDollar;
            public const string UnaryPlus = Plus;
            public const string Minus = "-";
            public const string MinusDollar = "-$";
            public const string Subtract = Minus;
            public const string SubtractChecked = MinusDollar;
            public const string Negate = Minus;
            public const string NegateChecked = MinusDollar;
            public const string Multiply = "*";
            public const string MultiplyChecked = "*$";
            public const string Divide = "/";
            public const string Modulo = "%";
            public const string Power = "^^";
            public const string RightShift = ">>";
            public const string LeftShift = "<<";
            public const string LessThan = "<";
            public const string LessThanOrEqual = "<=";
            public const string GreaterThan = ">";
            public const string GreaterThanOrEqual = ">=";
            public const string Equal = "==";
            public const string NotEqual = "!=";
            public const string And = "&";
            public const string AndAlso = "&&";
            public const string Or = "|";
            public const string OrElse = "||";
            public const string ExclusiveOr = "^";
            public const string Coalesce = "??";
            public const string TypeIs = "is";
            public const string TypeEqual = "=:";
            public const string Conditional = "?:";
            public const string Lambda = "=>";
            public const string Parameter = "$";
            public const string Index = ".[]";
            public const string Invocation = "()";
            public const string MemberAccess = ".";
            public const string MethodCall = ".()";
            public const string New = "new";
            public const string MemberInit = "{.}";
            public const string ListInit = "{+}";
            public const string NewArrayInit = "new[]";
            public const string NewArrayBounds = "new[*]";
            public const string ArrayLength = "#";
            public const string ArrayIndex = "[]";
            public const string Block = "{...}";
            public const string Goto = "goto";
            public const string Label = "label";
            public const string Loop = "loop";
            public const string Switch = "switch";
            public const string Try = "try";
            public const string Assign = "=";
            public const string AddAssign = "+=";
            public const string AddAssignChecked = "+$=";
            public const string AndAssign = "&=";
            public const string DivideAssign = "/=";
            public const string ExclusiveOrAssign = "^=";
            public const string LeftShiftAssign = "<<=";
            public const string ModuloAssign = "%=";
            public const string MultiplyAssign = "*=";
            public const string MultiplyAssignChecked = "*$=";
            public const string OrAssign = "|=";
            public const string PowerAssign = "^^=";
            public const string RightShiftAssign = ">>=";
            public const string SubtractAssign = "-=";
            public const string SubtractAssignChecked = "-$=";
            public const string PostDecrementAssign = "a--";
            public const string PostIncrementAssign = "a++";
            public const string PreDecrementAssign = "--a";
            public const string PreIncrementAssign = "++a";
            public const string Throw = "throw";
            public const string Unbox = "unbox";
            public const string IsFalse = "F?";
            public const string IsTrue = "T?";

            public static readonly Json.Expression ConstantDiscriminator = Json.Expression.String(Constant);
            public static readonly Json.Expression DefaultDiscriminator = Json.Expression.String(Default);
            public static readonly Json.Expression OnesComplementDiscriminator = Json.Expression.String(OnesComplement);
            public static readonly Json.Expression DecrementDiscriminator = Json.Expression.String(Decrement);
            public static readonly Json.Expression IncrementDiscriminator = Json.Expression.String(Increment);
            public static readonly Json.Expression NotDiscriminator = Json.Expression.String(Not);
            public static readonly Json.Expression ConvertDiscriminator = Json.Expression.String(Convert);
            public static readonly Json.Expression ConvertCheckedDiscriminator = Json.Expression.String(ConvertChecked);
            public static readonly Json.Expression TypeAsDiscriminator = Json.Expression.String(TypeAs);
            public static readonly Json.Expression QuoteDiscriminator = Json.Expression.String(Quote);
            public static readonly Json.Expression PlusDiscriminator = Json.Expression.String(Plus);
            public static readonly Json.Expression PlusDollarDiscriminator = Json.Expression.String(PlusDollar);
            public static readonly Json.Expression AddDiscriminator = Json.Expression.String(Add);
            public static readonly Json.Expression AddCheckedDiscriminator = Json.Expression.String(AddChecked);
            public static readonly Json.Expression UnaryPlusDiscriminator = Json.Expression.String(UnaryPlus);
            public static readonly Json.Expression MinusDiscriminator = Json.Expression.String(Minus);
            public static readonly Json.Expression MinusDollarDiscriminator = Json.Expression.String(MinusDollar);
            public static readonly Json.Expression SubtractDiscriminator = Json.Expression.String(Subtract);
            public static readonly Json.Expression SubtractCheckedDiscriminator = Json.Expression.String(SubtractChecked);
            public static readonly Json.Expression NegateDiscriminator = Json.Expression.String(Negate);
            public static readonly Json.Expression NegateCheckedDiscriminator = Json.Expression.String(NegateChecked);
            public static readonly Json.Expression MultiplyDiscriminator = Json.Expression.String(Multiply);
            public static readonly Json.Expression MultiplyCheckedDiscriminator = Json.Expression.String(MultiplyChecked);
            public static readonly Json.Expression DivideDiscriminator = Json.Expression.String(Divide);
            public static readonly Json.Expression ModuloDiscriminator = Json.Expression.String(Modulo);
            public static readonly Json.Expression PowerDiscriminator = Json.Expression.String(Power);
            public static readonly Json.Expression RightShiftDiscriminator = Json.Expression.String(RightShift);
            public static readonly Json.Expression LeftShiftDiscriminator = Json.Expression.String(LeftShift);
            public static readonly Json.Expression LessThanDiscriminator = Json.Expression.String(LessThan);
            public static readonly Json.Expression LessThanOrEqualDiscriminator = Json.Expression.String(LessThanOrEqual);
            public static readonly Json.Expression GreaterThanDiscriminator = Json.Expression.String(GreaterThan);
            public static readonly Json.Expression GreaterThanOrEqualDiscriminator = Json.Expression.String(GreaterThanOrEqual);
            public static readonly Json.Expression EqualDiscriminator = Json.Expression.String(Equal);
            public static readonly Json.Expression NotEqualDiscriminator = Json.Expression.String(NotEqual);
            public static readonly Json.Expression AndDiscriminator = Json.Expression.String(And);
            public static readonly Json.Expression AndAlsoDiscriminator = Json.Expression.String(AndAlso);
            public static readonly Json.Expression OrDiscriminator = Json.Expression.String(Or);
            public static readonly Json.Expression OrElseDiscriminator = Json.Expression.String(OrElse);
            public static readonly Json.Expression ExclusiveOrDiscriminator = Json.Expression.String(ExclusiveOr);
            public static readonly Json.Expression CoalesceDiscriminator = Json.Expression.String(Coalesce);
            public static readonly Json.Expression TypeIsDiscriminator = Json.Expression.String(TypeIs);
            public static readonly Json.Expression TypeEqualDiscriminator = Json.Expression.String(TypeEqual);
            public static readonly Json.Expression ConditionalDiscriminator = Json.Expression.String(Conditional);
            public static readonly Json.Expression LambdaDiscriminator = Json.Expression.String(Lambda);
            public static readonly Json.Expression ParameterDiscriminator = Json.Expression.String(Parameter);
            public static readonly Json.Expression IndexDiscriminator = Json.Expression.String(Index);
            public static readonly Json.Expression InvocationDiscriminator = Json.Expression.String(Invocation);
            public static readonly Json.Expression MemberAccessDiscriminator = Json.Expression.String(MemberAccess);
            public static readonly Json.Expression MethodCallDiscriminator = Json.Expression.String(MethodCall);
            public static readonly Json.Expression NewDiscriminator = Json.Expression.String(New);
            public static readonly Json.Expression MemberInitDiscriminator = Json.Expression.String(MemberInit);
            public static readonly Json.Expression ListInitDiscriminator = Json.Expression.String(ListInit);
            public static readonly Json.Expression NewArrayInitDiscriminator = Json.Expression.String(NewArrayInit);
            public static readonly Json.Expression NewArrayBoundsDiscriminator = Json.Expression.String(NewArrayBounds);
            public static readonly Json.Expression ArrayLengthDiscriminator = Json.Expression.String(ArrayLength);
            public static readonly Json.Expression ArrayIndexDiscriminator = Json.Expression.String(ArrayIndex);
            public static readonly Json.Expression BlockDiscriminator = Json.Expression.String(Block);
            public static readonly Json.Expression GotoDiscriminator = Json.Expression.String(Goto);
            public static readonly Json.Expression LabelDiscriminator = Json.Expression.String(Label);
            public static readonly Json.Expression LoopDiscriminator = Json.Expression.String(Loop);
            public static readonly Json.Expression SwitchDiscriminator = Json.Expression.String(Switch);
            public static readonly Json.Expression TryDiscriminator = Json.Expression.String(Try);
            public static readonly Json.Expression AssignDiscriminator = Json.Expression.String(Assign);
            public static readonly Json.Expression AddAssignDiscriminator = Json.Expression.String(AddAssign);
            public static readonly Json.Expression AddAssignCheckedDiscriminator = Json.Expression.String(AddAssignChecked);
            public static readonly Json.Expression AndAssignDiscriminator = Json.Expression.String(AndAssign);
            public static readonly Json.Expression DivideAssignDiscriminator = Json.Expression.String(DivideAssign);
            public static readonly Json.Expression ExclusiveOrAssignDiscriminator = Json.Expression.String(ExclusiveOrAssign);
            public static readonly Json.Expression LeftShiftAssignDiscriminator = Json.Expression.String(LeftShiftAssign);
            public static readonly Json.Expression ModuloAssignDiscriminator = Json.Expression.String(ModuloAssign);
            public static readonly Json.Expression MultiplyAssignDiscriminator = Json.Expression.String(MultiplyAssign);
            public static readonly Json.Expression MultiplyAssignCheckedDiscriminator = Json.Expression.String(MultiplyAssignChecked);
            public static readonly Json.Expression OrAssignDiscriminator = Json.Expression.String(OrAssign);
            public static readonly Json.Expression PowerAssignDiscriminator = Json.Expression.String(PowerAssign);
            public static readonly Json.Expression RightShiftAssignDiscriminator = Json.Expression.String(RightShiftAssign);
            public static readonly Json.Expression SubtractAssignDiscriminator = Json.Expression.String(SubtractAssign);
            public static readonly Json.Expression SubtractAssignCheckedDiscriminator = Json.Expression.String(SubtractAssignChecked);
            public static readonly Json.Expression PostDecrementAssignDiscriminator = Json.Expression.String(PostDecrementAssign);
            public static readonly Json.Expression PostIncrementAssignDiscriminator = Json.Expression.String(PostIncrementAssign);
            public static readonly Json.Expression PreDecrementAssignDiscriminator = Json.Expression.String(PreDecrementAssign);
            public static readonly Json.Expression PreIncrementAssignDiscriminator = Json.Expression.String(PreIncrementAssign);
            public static readonly Json.Expression ThrowDiscriminator = Json.Expression.String(Throw);
            public static readonly Json.Expression UnboxDiscriminator = Json.Expression.String(Unbox);
            public static readonly Json.Expression IsFalseDiscriminator = Json.Expression.String(IsFalse);
            public static readonly Json.Expression IsTrueDiscriminator = Json.Expression.String(IsTrue);
        }

        public static class MemberBinding
        {
            public const string MemberAssignment = "=";
            public const string MemberMemberBinding = ".";
            public const string MemberListBinding = "+";

            public static readonly Json.Expression MemberAssignmentDiscriminator = Json.Expression.String(MemberAssignment);
            public static readonly Json.Expression MemberMemberBindingDiscriminator = Json.Expression.String(MemberMemberBinding);
            public static readonly Json.Expression MemberListBindingDiscriminator = Json.Expression.String(MemberListBinding);
        }

        public static class Type
        {
            public const string Simple = "::";
            public const string Generic = "<>";
            public const string Array = "[]";
            public const string Anonymous = "{}";
            public const string Record = "{;}";

            public static readonly Json.Expression SimpleDiscriminator = Json.Expression.String(Simple);
            public static readonly Json.Expression GenericDiscriminator = Json.Expression.String(Generic);
            public static readonly Json.Expression ArrayDiscriminator = Json.Expression.String(Array);
            public static readonly Json.Expression AnonymousDiscriminator = Json.Expression.String(Anonymous);
            public static readonly Json.Expression RecordDiscriminator = Json.Expression.String(Record);
        }

        public static class MemberInfo
        {
            public const string Constructor = "C";
            public const string Field = "F";
            public const string Property = "P";
            public const string SimpleMethod = "M";
            public const string OpenGenericMethod = "M`";
            public const string ClosedGenericMethod = "M<>";

            public static readonly Json.Expression ConstructorDiscriminator = Json.Expression.String(Constructor);
            public static readonly Json.Expression FieldDiscriminator = Json.Expression.String(Field);
            public static readonly Json.Expression PropertyDiscriminator = Json.Expression.String(Property);
            public static readonly Json.Expression SimpleMethodDiscriminator = Json.Expression.String(SimpleMethod);
            public static readonly Json.Expression OpenGenericMethodDiscriminator = Json.Expression.String(OpenGenericMethod);
            public static readonly Json.Expression ClosedGenericMethodDiscriminator = Json.Expression.String(ClosedGenericMethod);
        }
    }
}
