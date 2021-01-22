// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System;

namespace Reaqtor.Remoting.Protocol
{
    [Serializable]
    public class StorageEntityProperty
    {
        public int Type
        {
            get;
            set;
        }

        public string Data
        {
            get;
            set;
        }
    }
}
