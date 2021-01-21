// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

namespace System.Linq.Expressions
{
    partial class ParameterTable
    {
        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="T2">The second type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, T2>(Expression<Action<TParameter, T2>> expression) => Add((LambdaExpression)expression);

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="T2">The second type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T3">The third type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, T2, T3>(Expression<Action<TParameter, T2, T3>> expression) => Add((LambdaExpression)expression);

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="T2">The second type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T3">The third type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T4">The fourth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, T2, T3, T4>(Expression<Action<TParameter, T2, T3, T4>> expression) => Add((LambdaExpression)expression);

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="T2">The second type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T3">The third type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T4">The fourth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T5">The fifth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, T2, T3, T4, T5>(Expression<Action<TParameter, T2, T3, T4, T5>> expression) => Add((LambdaExpression)expression);

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="T2">The second type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T3">The third type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T4">The fourth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T5">The fifth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T6">The sixth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, T2, T3, T4, T5, T6>(Expression<Action<TParameter, T2, T3, T4, T5, T6>> expression) => Add((LambdaExpression)expression);

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="T2">The second type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T3">The third type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T4">The fourth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T5">The fifth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T6">The sixth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T7">The seventh type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, T2, T3, T4, T5, T6, T7>(Expression<Action<TParameter, T2, T3, T4, T5, T6, T7>> expression) => Add((LambdaExpression)expression);

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="T2">The second type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T3">The third type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T4">The fourth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T5">The fifth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T6">The sixth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T7">The seventh type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T8">The eighth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, T2, T3, T4, T5, T6, T7, T8>(Expression<Action<TParameter, T2, T3, T4, T5, T6, T7, T8>> expression) => Add((LambdaExpression)expression);

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="T2">The second type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T3">The third type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T4">The fourth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T5">The fifth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T6">The sixth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T7">The seventh type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T8">The eighth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T9">The ninth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, T2, T3, T4, T5, T6, T7, T8, T9>(Expression<Action<TParameter, T2, T3, T4, T5, T6, T7, T8, T9>> expression) => Add((LambdaExpression)expression);

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="T2">The second type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T3">The third type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T4">The fourth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T5">The fifth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T6">The sixth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T7">The seventh type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T8">The eighth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T9">The ninth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T10">The tenth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Expression<Action<TParameter, T2, T3, T4, T5, T6, T7, T8, T9, T10>> expression) => Add((LambdaExpression)expression);

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="T2">The second type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T3">The third type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T4">The fourth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T5">The fifth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T6">The sixth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T7">The seventh type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T8">The eighth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T9">The ninth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T10">The tenth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T11">The eleventh type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Expression<Action<TParameter, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> expression) => Add((LambdaExpression)expression);

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="T2">The second type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T3">The third type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T4">The fourth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T5">The fifth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T6">The sixth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T7">The seventh type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T8">The eighth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T9">The ninth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T10">The tenth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T11">The eleventh type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T12">The twelfth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Expression<Action<TParameter, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> expression) => Add((LambdaExpression)expression);

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="T2">The second type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T3">The third type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T4">The fourth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T5">The fifth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T6">The sixth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T7">The seventh type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T8">The eighth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T9">The ninth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T10">The tenth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T11">The eleventh type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T12">The twelfth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T13">The thirteenth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Expression<Action<TParameter, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> expression) => Add((LambdaExpression)expression);

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="T2">The second type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T3">The third type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T4">The fourth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T5">The fifth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T6">The sixth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T7">The seventh type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T8">The eighth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T9">The ninth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T10">The tenth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T11">The eleventh type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T12">The twelfth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T13">The thirteenth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T14">The fourteenth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Expression<Action<TParameter, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> expression) => Add((LambdaExpression)expression);

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="T2">The second type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T3">The third type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T4">The fourth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T5">The fifth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T6">The sixth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T7">The seventh type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T8">The eighth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T9">The ninth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T10">The tenth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T11">The eleventh type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T12">The twelfth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T13">The thirteenth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T14">The fourteenth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T15">The fifteenth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Expression<Action<TParameter, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> expression) => Add((LambdaExpression)expression);

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="T2">The second type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T3">The third type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T4">The fourth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T5">The fifth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T6">The sixth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T7">The seventh type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T8">The eighth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T9">The ninth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T10">The tenth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T11">The eleventh type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T12">The twelfth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T13">The thirteenth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T14">The fourteenth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T15">The fifteenth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T16">The sixteenth type parameter of the action passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Expression<Action<TParameter, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>> expression) => Add((LambdaExpression)expression);

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="R">The result type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, R>(Expression<Func<TParameter, R>> expression) => Add((LambdaExpression)expression);

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="T2">The second type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="R">The result type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, T2, R>(Expression<Func<TParameter, T2, R>> expression) => Add((LambdaExpression)expression);

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="T2">The second type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T3">The third type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="R">The result type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, T2, T3, R>(Expression<Func<TParameter, T2, T3, R>> expression) => Add((LambdaExpression)expression);

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="T2">The second type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T3">The third type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T4">The fourth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="R">The result type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, T2, T3, T4, R>(Expression<Func<TParameter, T2, T3, T4, R>> expression) => Add((LambdaExpression)expression);

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="T2">The second type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T3">The third type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T4">The fourth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T5">The fifth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="R">The result type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, T2, T3, T4, T5, R>(Expression<Func<TParameter, T2, T3, T4, T5, R>> expression) => Add((LambdaExpression)expression);

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="T2">The second type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T3">The third type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T4">The fourth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T5">The fifth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T6">The sixth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="R">The result type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, T2, T3, T4, T5, T6, R>(Expression<Func<TParameter, T2, T3, T4, T5, T6, R>> expression) => Add((LambdaExpression)expression);

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="T2">The second type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T3">The third type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T4">The fourth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T5">The fifth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T6">The sixth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T7">The seventh type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="R">The result type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, T2, T3, T4, T5, T6, T7, R>(Expression<Func<TParameter, T2, T3, T4, T5, T6, T7, R>> expression) => Add((LambdaExpression)expression);

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="T2">The second type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T3">The third type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T4">The fourth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T5">The fifth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T6">The sixth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T7">The seventh type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T8">The eighth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="R">The result type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, T2, T3, T4, T5, T6, T7, T8, R>(Expression<Func<TParameter, T2, T3, T4, T5, T6, T7, T8, R>> expression) => Add((LambdaExpression)expression);

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="T2">The second type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T3">The third type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T4">The fourth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T5">The fifth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T6">The sixth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T7">The seventh type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T8">The eighth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T9">The ninth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="R">The result type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, T2, T3, T4, T5, T6, T7, T8, T9, R>(Expression<Func<TParameter, T2, T3, T4, T5, T6, T7, T8, T9, R>> expression) => Add((LambdaExpression)expression);

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="T2">The second type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T3">The third type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T4">The fourth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T5">The fifth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T6">The sixth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T7">The seventh type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T8">The eighth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T9">The ninth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T10">The tenth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="R">The result type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, T2, T3, T4, T5, T6, T7, T8, T9, T10, R>(Expression<Func<TParameter, T2, T3, T4, T5, T6, T7, T8, T9, T10, R>> expression) => Add((LambdaExpression)expression);

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="T2">The second type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T3">The third type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T4">The fourth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T5">The fifth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T6">The sixth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T7">The seventh type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T8">The eighth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T9">The ninth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T10">The tenth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T11">The eleventh type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="R">The result type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R>(Expression<Func<TParameter, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R>> expression) => Add((LambdaExpression)expression);

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="T2">The second type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T3">The third type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T4">The fourth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T5">The fifth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T6">The sixth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T7">The seventh type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T8">The eighth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T9">The ninth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T10">The tenth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T11">The eleventh type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T12">The twelfth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="R">The result type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R>(Expression<Func<TParameter, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R>> expression) => Add((LambdaExpression)expression);

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="T2">The second type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T3">The third type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T4">The fourth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T5">The fifth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T6">The sixth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T7">The seventh type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T8">The eighth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T9">The ninth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T10">The tenth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T11">The eleventh type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T12">The twelfth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T13">The thirteenth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="R">The result type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R>(Expression<Func<TParameter, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R>> expression) => Add((LambdaExpression)expression);

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="T2">The second type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T3">The third type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T4">The fourth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T5">The fifth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T6">The sixth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T7">The seventh type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T8">The eighth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T9">The ninth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T10">The tenth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T11">The eleventh type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T12">The twelfth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T13">The thirteenth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T14">The fourteenth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="R">The result type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R>(Expression<Func<TParameter, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R>> expression) => Add((LambdaExpression)expression);

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="T2">The second type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T3">The third type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T4">The fourth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T5">The fifth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T6">The sixth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T7">The seventh type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T8">The eighth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T9">The ninth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T10">The tenth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T11">The eleventh type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T12">The twelfth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T13">The thirteenth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T14">The fourteenth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T15">The fifteenth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="R">The result type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R>(Expression<Func<TParameter, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R>> expression) => Add((LambdaExpression)expression);

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned to by the <typeparamref name="TParameter"/> parameter in the specified <paramref name="expression" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter to add to the table.</typeparam>
        /// <typeparam name="T2">The second type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T3">The third type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T4">The fourth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T5">The fifth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T6">The sixth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T7">The seventh type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T8">The eighth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T9">The ninth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T10">The tenth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T11">The eleventh type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T12">The twelfth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T13">The thirteenth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T14">The fourteenth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T15">The fifteenth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="T16">The sixteenth type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <typeparam name="R">The result type parameter of the function passed in <paramref name="expression" />.</typeparam>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add<TParameter, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R>(Expression<Func<TParameter, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R>> expression) => Add((LambdaExpression)expression);

    }
}
