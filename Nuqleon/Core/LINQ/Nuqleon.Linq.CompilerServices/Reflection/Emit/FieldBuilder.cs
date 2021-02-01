// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2018 - Created this file.
//

//
// NB: This file contains a late bound accessor for System.Reflection.Emit types which are unavailable on .NET Standard 2.0.
//

#if NETSTANDARD2_0

#pragma warning disable CS1591 // XML docs missing for late bound accessor mirror image of System.Reflection.Emit functionality.

using System;
using System.Globalization;
using System.Reflection;

namespace Nuqleon.Reflection.Emit
{
    public class FieldBuilder : FieldInfo
    {
        internal readonly FieldInfo _field;

        public FieldBuilder(object builder) => _field = (FieldInfo)builder;

        public override FieldAttributes Attributes => _field.Attributes;

        public override RuntimeFieldHandle FieldHandle => _field.FieldHandle;

        public override Type FieldType => _field.FieldType.Unwrap();

        public override Type DeclaringType => _field.DeclaringType.Unwrap();

        public override string Name => _field.Name;

        public override Type ReflectedType => _field.ReflectedType.Unwrap();

        public override object[] GetCustomAttributes(bool inherit) => _field.GetCustomAttributes(inherit);

        public override object[] GetCustomAttributes(Type attributeType, bool inherit) => _field.GetCustomAttributes(attributeType.Unwrap(), inherit);

        public override object GetValue(object obj) => _field.GetValue(obj);

        public override bool IsDefined(Type attributeType, bool inherit) => _field.IsDefined(attributeType.Unwrap(), inherit);

        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture) => _field.SetValue(obj, value, invokeAttr, binder, culture);
    }
}

#endif
