// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2014
//

.assembly Nuqleon.Runtime.ExceptionServices.Extensions
{
  .ver 1:0:0:0
}

.module Nuqleon.Runtime.ExceptionServices.Extensions.dll

.assembly extern mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89)
  .ver 4:0:0:0
}

.class public sealed Nuqleon.Runtime.ExceptionServices.Helpers
{

  //   _____           _____           _ _   
  //  |_   _| __ _   _|  ___|_ _ _   _| | |_ 
  //    | || '__| | | | |_ / _` | | | | | __|
  //    | || |  | |_| |  _| (_| | |_| | | |_ 
  //    |_||_|   \__, |_|  \__,_|\__,_|_|\__|
  //             |___/                       
  //  

  .method public static void TryFault(class [mscorlib]System.Action 'try', class [mscorlib]System.Action 'fault')
  {
    //
    // Argument null checks.
    //

    ldarg        'try'
    brfalse.s    FailArgTry
    ldarg        'fault'
    brfalse.s    FailArgFault

    //
    // Invocation of 'try' and 'fault' delegates in a .try/fault handler.
    //

    .try
    {
      ldarg      'try'
      callvirt   instance void [mscorlib]System.Action::Invoke()
      leave.s    Exit
    }
    fault
    {
      ldarg      'fault'
      callvirt   instance void [mscorlib]System.Action::Invoke()
      endfault
    }

  Exit:
    ret

    //
    // Exceptions for argument null checks.
    //

  FailArgTry:
    ldstr        "try"
    br.s         ThrowArgNull

  FailArgFault:
    ldstr        "fault"

  ThrowArgNull:
    newobj       instance void [mscorlib]System.ArgumentNullException::.ctor(string)
    throw
  }


  //   _____           _____ _ _ _            
  //  |_   _| __ _   _|  ___(_) | |_ ___ _ __ 
  //    | || '__| | | | |_  | | | __/ _ \ '__|
  //    | || |  | |_| |  _| | | | ||  __/ |   
  //    |_||_|   \__, |_|   |_|_|\__\___|_|   
  //             |___/                        
  //  

  .method public static void TryFilter<([mscorlib]System.Exception)T>(class [mscorlib]System.Action 'try', class [mscorlib]System.Func`2<!!T,bool> 'if', class [mscorlib]System.Action`1<!!T> 'catch')
  {
    .custom instance void [mscorlib]System.ObsoleteAttribute::.ctor(string) = { string('Consider using language support for exception filters (e.g. `when` in C#).') }
    .locals init (!!T 'ex')

    //
    // Argument null checks.
    //

    ldarg        'try'
    brfalse.s    FailArgTry
    ldarg        'if'
    brfalse.s    FailArgFilter
    ldarg        'catch'
    brfalse.s    FailArgHandler

    .try
    {
      ldarg      'try'
      callvirt   instance void [mscorlib]System.Action::Invoke()
      leave.s    Exit
    }
    filter
    {
      isinst     !!T
      dup
      brtrue.s   RunFilter
      pop
      ldc.i4.0
      br.s       EndFilter

    RunFilter:
      unbox.any  !!T
      stloc      'ex'
      ldarg      'if'
      ldloc      'ex'
      callvirt   instance !1 class [mscorlib]System.Func`2<!!T,bool>::Invoke(!0)

    EndFilter:
      endfilter
    }
    {
      pop
      ldarg      'catch'
      ldloc      'ex'
      callvirt   instance void class [mscorlib]System.Action`1<!!T>::Invoke(!0)
      leave.s    Exit
    }

  Exit:
    ret

    //
    // Exceptions for argument null checks.
    //

  FailArgTry:
    ldstr        "try"
    br.s         ThrowArgNull

  FailArgFilter:
    ldstr        "if"
    br.s         ThrowArgNull

  FailArgHandler:
    ldstr        "catch"

  ThrowArgNull:
    newobj       instance void [mscorlib]System.ArgumentNullException::.ctor(string)
    throw
  }
}