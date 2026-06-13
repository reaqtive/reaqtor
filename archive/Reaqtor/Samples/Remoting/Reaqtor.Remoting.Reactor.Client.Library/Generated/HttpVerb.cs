// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor
{
    /// <summary>
    /// Valid http verbs
    /// </summary>
    public enum HttpVerb
    {
        [Mapping("reactor://platform.bing.com/http/verbs/undefined")]
        Unknown = 0,

        [Mapping("reactor://platform.bing.com/http/verbs/get")]
        Get = 1,

        [Mapping("reactor://platform.bing.com/http/verbs/post")]
        Post = 2,

        [Mapping("reactor://platform.bing.com/http/verbs/put")]
        Put = 3,

        [Mapping("reactor://platform.bing.com/http/verbs/delete")]
        Delete = 4,

        [Mapping("reactor://platform.bing.com/http/verbs/head")]
        Head = 5,

        [Mapping("reactor://platform.bing.com/http/verbs/patch")]
        Patch = 6,

        [Mapping("reactor://platform.bing.com/http/verbs/options")]
        Options = 7,

        [Mapping("reactor://platform.bing.com/http/verbs/trace")]
        Trace = 8,

        [Mapping("reactor://platform.bing.com/http/verbs/connect")]
        Connect = 9,

        [Mapping("reactor://platform.bing.com/http/verbs/propfind")]
        PropFind = 10,

        [Mapping("reactor://platform.bing.com/http/verbs/proppatch")]
        PropPatch = 11,

        [Mapping("reactor://platform.bing.com/http/verbs/mkcol")]
        MkCol = 12,

        [Mapping("reactor://platform.bing.com/http/verbs/copy")]
        Copy = 13,

        [Mapping("reactor://platform.bing.com/http/verbs/move")]
        Move = 14,

        [Mapping("reactor://platform.bing.com/http/verbs/lock")]
        Lock = 15,

        [Mapping("reactor://platform.bing.com/http/verbs/unlock")]
        Unlock = 16,
    }
}
