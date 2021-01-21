// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2014
//

.assembly Tests.Nuqleon.Runtime.ExceptionServices.Extensions
{
  .ver 1:0:0:0
}

.module Tests.Nuqleon.Runtime.ExceptionServices.Extensions.dll

.assembly extern mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89)
  .ver 4:0:0:0
}

.assembly extern Nuqleon.Runtime.ExceptionServices.Extensions
{
  .ver 1:0:0:0
}

.assembly extern Microsoft.VisualStudio.QualityTools.UnitTestFramework
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A)
  .ver 10:0:0:0
}

.class public Tests.Nuqleon.Runtime.ExceptionServices.Helpers
{
  .custom instance void [Microsoft.VisualStudio.QualityTools.UnitTestFramework]Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute::.ctor()

  .method public specialname void .ctor()
  {
    ldarg.0
    call         instance void [mscorlib]System.Object::.ctor()

    ret
  }

  //   _____           _____           _ _   
  //  |_   _| __ _   _|  ___|_ _ _   _| | |_ 
  //    | || '__| | | | |_ / _` | | | | | __|
  //    | || |  | |_| |  _| (_| | |_| | | |_ 
  //    |_||_|   \__, |_|  \__,_|\__,_|_|\__|
  //             |___/                       
  //  

  //         _                               _        _           _       
  //        /_\  _ _ __ _ _  _ _ __  ___ _ _| |_   __| |_  ___ __| |__ ___
  //       / _ \| '_/ _` | || | '  \/ -_) ' \  _| / _| ' \/ -_) _| / /(_-<
  //      /_/ \_\_| \__, |\_,_|_|_|_\___|_||_\__| \__|_||_\___\__|_\_\/__/
  //                |___/                                                 
  //      

  .method public void ExceptionServices_Fault_ArgumentChecks()
  {
    .custom      instance void [Microsoft.VisualStudio.QualityTools.UnitTestFramework]Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute::.ctor()

    //
    // 'try' argument should not be null.
    //

    ldnull
    ldftn        void Tests.Nuqleon.Runtime.ExceptionServices.Helpers::ExceptionServices_Fault_ArgumentChecks_TryIsNull()
    newobj       instance void class [mscorlib]System.Action::.ctor(object, native int)

    ldstr        "try"

    call         void Tests.Nuqleon.Runtime.ExceptionServices.Helpers::AssertThrows_ArgumentNullException(class [mscorlib]System.Action, string)

    //
    // 'fault' argument should not be null.
    //

    ldnull
    ldftn        void Tests.Nuqleon.Runtime.ExceptionServices.Helpers::ExceptionServices_Fault_ArgumentChecks_FaultIsNull()
    newobj       instance void class [mscorlib]System.Action::.ctor(object, native int)

    ldstr        "fault"

    call         void Tests.Nuqleon.Runtime.ExceptionServices.Helpers::AssertThrows_ArgumentNullException(class [mscorlib]System.Action, string)

    ret
  }

  .method static void ExceptionServices_Fault_ArgumentChecks_TryIsNull()
  {
    ldnull

    ldnull
    ldftn        void Tests.Nuqleon.Runtime.ExceptionServices.Helpers::Nop()
    newobj       instance void class [mscorlib]System.Action::.ctor(object, native int)

    call         void [Nuqleon.Runtime.ExceptionServices.Extensions]Nuqleon.Runtime.ExceptionServices.Helpers::TryFault(class [mscorlib]System.Action, class [mscorlib]System.Action)

    ret
  }

  .method static void ExceptionServices_Fault_ArgumentChecks_FaultIsNull()
  {
    ldnull
    ldftn        void Tests.Nuqleon.Runtime.ExceptionServices.Helpers::Nop()
    newobj       instance void class [mscorlib]System.Action::.ctor(object, native int)

    ldnull
	
    call         void [Nuqleon.Runtime.ExceptionServices.Extensions]Nuqleon.Runtime.ExceptionServices.Helpers::TryFault(class [mscorlib]System.Action, class [mscorlib]System.Action)

    ret
  }

  //       _____                  _ _   _                                     _   _          
  //      |_   _| _ _  _  __ __ _(_) |_| |_    _ _  ___   _____ ____ ___ _ __| |_(_)___ _ _  
  //        | || '_| || | \ V  V / |  _| ' \  | ' \/ _ \ / -_) \ / _/ -_) '_ \  _| / _ \ ' \ 
  //        |_||_|  \_, |  \_/\_/|_|\__|_||_| |_||_\___/ \___/_\_\__\___| .__/\__|_\___/_||_|
  //                |__/                                                |_|                  
  //      

  .method public void ExceptionServices_Fault_Try_NoError()
  {
    .custom      instance void [Microsoft.VisualStudio.QualityTools.UnitTestFramework]Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute::.ctor()

    .locals init (class Closure1 'closure')

    //
    // try
    // {
    //   Nop();
    //

    ldnull
    ldftn        void Tests.Nuqleon.Runtime.ExceptionServices.Helpers::Nop()
    newobj       instance void class [mscorlib]System.Action::.ctor(object, native int)

    //
    // }
    // fault
    // {
    //   closure.called = true;
    //

    newobj       instance void class Closure1::.ctor()
    stloc        'closure'
    ldloc        'closure'
    ldftn        instance void Closure1::Set()
    newobj       instance void class [mscorlib]System.Action::.ctor(object, native int)

    //
    // }
    //

    call         void [Nuqleon.Runtime.ExceptionServices.Extensions]Nuqleon.Runtime.ExceptionServices.Helpers::TryFault(class [mscorlib]System.Action, class [mscorlib]System.Action)

    //
    // Assert.IsFalse(!closure.called);
    //

    ldloc        'closure'
    ldfld        bool Closure1::called
    
    call         void [Microsoft.VisualStudio.QualityTools.UnitTestFramework]Microsoft.VisualStudio.TestTools.UnitTesting.Assert::IsFalse(bool)

    ret
  }

  //       _____                  _ _   _                          _   _          
  //      |_   _| _ _  _  __ __ _(_) |_| |_    _____ ____ ___ _ __| |_(_)___ _ _  
  //        | || '_| || | \ V  V / |  _| ' \  / -_) \ / _/ -_) '_ \  _| / _ \ ' \ 
  //        |_||_|  \_, |  \_/\_/|_|\__|_||_| \___/_\_\__\___| .__/\__|_\___/_||_|
  //                |__/                                     |_|                  
  //      

  .method public void ExceptionServices_Fault_Try_Error()
  {
    .custom      instance void [Microsoft.VisualStudio.QualityTools.UnitTestFramework]Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute::.ctor()

    .locals init (class Closure1 'closure', bool 'hasError')

    //
    // try
    // {

    .try
    {
      //
      // try
      // {
      //   Fail();
      //

      ldnull
      ldftn      void Tests.Nuqleon.Runtime.ExceptionServices.Helpers::Fail()
      newobj     instance void class [mscorlib]System.Action::.ctor(object, native int)

      //
      // }
      // fault
      // {
      //   closure.called = true;
      //

      newobj     instance void class Closure1::.ctor()
      stloc      'closure'
      ldloc      'closure'
      ldftn      instance void Closure1::Set()
      newobj     instance void class [mscorlib]System.Action::.ctor(object, native int)

      //
      // }
      //

      call       void [Nuqleon.Runtime.ExceptionServices.Extensions]Nuqleon.Runtime.ExceptionServices.Helpers::TryFault(class [mscorlib]System.Action, class [mscorlib]System.Action)
      leave      Assert
    }

    //
    // }
    // catch
    // {
    //

    catch [mscorlib]System.Object
    {
      //
      // hasError = true;
      //

      pop
      ldc.i4.1
      stloc      'hasError'
      leave      Assert
    }

    //
    // }
    //

Assert:

    //
    // Assert.IsTrue(closure.called & hasError);
    //

    ldloc        'closure'
    ldfld        bool Closure1::called
    ldloc        'hasError'
    and
    
    call         void [Microsoft.VisualStudio.QualityTools.UnitTestFramework]Microsoft.VisualStudio.TestTools.UnitTesting.Assert::IsTrue(bool)

    ret
  }


  //   _____           _____ _ _ _            
  //  |_   _| __ _   _|  ___(_) | |_ ___ _ __ 
  //    | || '__| | | | |_  | | | __/ _ \ '__|
  //    | || |  | |_| |  _| | | | ||  __/ |   
  //    |_||_|   \__, |_|   |_|_|\__\___|_|   
  //             |___/                        
  //  

  //         _                               _        _           _       
  //        /_\  _ _ __ _ _  _ _ __  ___ _ _| |_   __| |_  ___ __| |__ ___
  //       / _ \| '_/ _` | || | '  \/ -_) ' \  _| / _| ' \/ -_) _| / /(_-<
  //      /_/ \_\_| \__, |\_,_|_|_|_\___|_||_\__| \__|_||_\___\__|_\_\/__/
  //                |___/                                                 
  //      

  .method public void ExceptionServices_Filter_ArgumentChecks()
  {
    .custom      instance void [Microsoft.VisualStudio.QualityTools.UnitTestFramework]Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute::.ctor()

    //
    // 'try' argument should not be null.
    //

    ldnull
    ldftn        void Tests.Nuqleon.Runtime.ExceptionServices.Helpers::ExceptionServices_Filter_ArgumentChecks_TryIsNull()
    newobj       instance void class [mscorlib]System.Action::.ctor(object, native int)

    ldstr        "try"

    call         void Tests.Nuqleon.Runtime.ExceptionServices.Helpers::AssertThrows_ArgumentNullException(class [mscorlib]System.Action, string)

    //
    // 'filter' argument should not be null.
    //

    ldnull
    ldftn        void Tests.Nuqleon.Runtime.ExceptionServices.Helpers::ExceptionServices_Filter_ArgumentChecks_FilterIsNull()
    newobj       instance void class [mscorlib]System.Action::.ctor(object, native int)

    ldstr        "if"

    call         void Tests.Nuqleon.Runtime.ExceptionServices.Helpers::AssertThrows_ArgumentNullException(class [mscorlib]System.Action, string)

    //
    // 'handler' argument should not be null.
    //

    ldnull
    ldftn        void Tests.Nuqleon.Runtime.ExceptionServices.Helpers::ExceptionServices_Filter_ArgumentChecks_HandlerIsNull()
    newobj       instance void class [mscorlib]System.Action::.ctor(object, native int)

    ldstr        "catch"

    call         void Tests.Nuqleon.Runtime.ExceptionServices.Helpers::AssertThrows_ArgumentNullException(class [mscorlib]System.Action, string)

    ret
  }

  .method static void ExceptionServices_Filter_ArgumentChecks_TryIsNull()
  {
    ldnull

    ldnull
    ldftn        bool Tests.Nuqleon.Runtime.ExceptionServices.Helpers::True(class [mscorlib]System.Exception)
    newobj       instance void class [mscorlib]System.Func`2<class [mscorlib]System.Exception,bool>::.ctor(object, native int)

    ldnull
    ldftn        void Tests.Nuqleon.Runtime.ExceptionServices.Helpers::Nop(class [mscorlib]System.Exception)
    newobj       instance void class [mscorlib]System.Action`1<class [mscorlib]System.Exception>::.ctor(object, native int)

    call         void [Nuqleon.Runtime.ExceptionServices.Extensions]Nuqleon.Runtime.ExceptionServices.Helpers::TryFilter<class [mscorlib]System.Exception>(class [mscorlib]System.Action, class [mscorlib]System.Func`2<!!0,bool>, class [mscorlib]System.Action`1<!!0>)

    ret
  }

  .method static void ExceptionServices_Filter_ArgumentChecks_FilterIsNull()
  {
    ldnull
    ldftn        void Tests.Nuqleon.Runtime.ExceptionServices.Helpers::Nop()
    newobj       instance void class [mscorlib]System.Action::.ctor(object, native int)

    ldnull

    ldnull
    ldftn        void Tests.Nuqleon.Runtime.ExceptionServices.Helpers::Nop(class [mscorlib]System.Exception)
    newobj       instance void class [mscorlib]System.Action`1<class [mscorlib]System.Exception>::.ctor(object, native int)
	
    call         void [Nuqleon.Runtime.ExceptionServices.Extensions]Nuqleon.Runtime.ExceptionServices.Helpers::TryFilter<class [mscorlib]System.Exception>(class [mscorlib]System.Action, class [mscorlib]System.Func`2<!!0,bool>, class [mscorlib]System.Action`1<!!0>)

    ret
  }

  .method static void ExceptionServices_Filter_ArgumentChecks_HandlerIsNull()
  {
    ldnull
    ldftn        void Tests.Nuqleon.Runtime.ExceptionServices.Helpers::Nop()
    newobj       instance void class [mscorlib]System.Action::.ctor(object, native int)

    ldnull
    ldftn        bool Tests.Nuqleon.Runtime.ExceptionServices.Helpers::True(class [mscorlib]System.Exception)
    newobj       instance void class [mscorlib]System.Func`2<class [mscorlib]System.Exception,bool>::.ctor(object, native int)

    ldnull
	
    call         void [Nuqleon.Runtime.ExceptionServices.Extensions]Nuqleon.Runtime.ExceptionServices.Helpers::TryFilter<class [mscorlib]System.Exception>(class [mscorlib]System.Action, class [mscorlib]System.Func`2<!!0,bool>, class [mscorlib]System.Action`1<!!0>)

    ret
  }

  //       _____                  _ _   _                                     _   _          
  //      |_   _| _ _  _  __ __ _(_) |_| |_    _ _  ___   _____ ____ ___ _ __| |_(_)___ _ _  
  //        | || '_| || | \ V  V / |  _| ' \  | ' \/ _ \ / -_) \ / _/ -_) '_ \  _| / _ \ ' \ 
  //        |_||_|  \_, |  \_/\_/|_|\__|_||_| |_||_\___/ \___/_\_\__\___| .__/\__|_\___/_||_|
  //                |__/                                                |_|                  
  //      

  .method public void ExceptionServices_Filter_Try_NoError()
  {
    .custom      instance void [Microsoft.VisualStudio.QualityTools.UnitTestFramework]Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute::.ctor()

    .locals init (class Closure2 'closureFilter', class Closure3 'closureHandler')

    //
    // try
    // {
    //   Nop();
    //

    ldnull
    ldftn        void Tests.Nuqleon.Runtime.ExceptionServices.Helpers::Nop()
    newobj       instance void class [mscorlib]System.Action::.ctor(object, native int)

    //
    // }
    // catch (Exception ex) if (closureFilter.Set(ex))
    // {
    //

    newobj       instance void class Closure2::.ctor()
    stloc        'closureFilter'
    ldloc        'closureFilter'
    ldftn         instance bool Closure2::Set(class [mscorlib]System.Exception)
    newobj       instance void class [mscorlib]System.Func`2<class [mscorlib]System.Exception, bool>::.ctor(object, native int)

    //
    //   closureHandler.Set(ex);
    //

    newobj       instance void class Closure3::.ctor()
    stloc        'closureHandler'
    ldloc        'closureHandler'
    ldftn        instance void Closure3::Set(class [mscorlib]System.Exception)
    newobj       instance void class [mscorlib]System.Action`1<class [mscorlib]System.Exception>::.ctor(object, native int)

    //
    // }
    //

    call         void [Nuqleon.Runtime.ExceptionServices.Extensions]Nuqleon.Runtime.ExceptionServices.Helpers::TryFilter<class [mscorlib]System.Exception>(class [mscorlib]System.Action, class [mscorlib]System.Func`2<!!0,bool>, class [mscorlib]System.Action`1<!!0>)

    //
    // Assert.IsFalse(closureFilter.called);
    //

    ldloc        'closureFilter'
    ldfld        bool Closure2::called
    
    call         void [Microsoft.VisualStudio.QualityTools.UnitTestFramework]Microsoft.VisualStudio.TestTools.UnitTesting.Assert::IsFalse(bool)

    //
    // Assert.IsFalse(closureHandler.called);
    //

    ldloc        'closureHandler'
    ldfld        bool Closure3::called
    
    call         void [Microsoft.VisualStudio.QualityTools.UnitTestFramework]Microsoft.VisualStudio.TestTools.UnitTesting.Assert::IsFalse(bool)

    ret
  }

  //       _____                  _ _   _       __ _ _ _                  _                       _   _          
  //      |_   _| _ _  _  __ __ _(_) |_| |_    / _(_) | |_ ___ _ _ ___ __| |  _____ ____ ___ _ __| |_(_)___ _ _  
  //        | || '_| || | \ V  V / |  _| ' \  |  _| | |  _/ -_) '_/ -_) _` | / -_) \ / _/ -_) '_ \  _| / _ \ ' \ 
  //        |_||_|  \_, |  \_/\_/|_|\__|_||_| |_| |_|_|\__\___|_| \___\__,_| \___/_\_\__\___| .__/\__|_\___/_||_|
  //                |__/                                                                    |_|                  
  //      

  .method public void ExceptionServices_Filter_Try_Filtered()
  {
    .custom      instance void [Microsoft.VisualStudio.QualityTools.UnitTestFramework]Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute::.ctor()

    .locals init (class Closure4 'closureFilter', class Closure3 'closureHandler')

    //
    // try
    // {
    //   Fail();
    //

    ldnull
    ldftn        void Tests.Nuqleon.Runtime.ExceptionServices.Helpers::Fail()
    newobj       instance void class [mscorlib]System.Action::.ctor(object, native int)

    //
    // }
    // catch (Exception ex) if (closureFilter.Set(ex) /* ex.Contains("Oops") */)
    // {
    //

    newobj       instance void class Closure4::.ctor()
    stloc        'closureFilter'
    ldloc        'closureFilter'
    ldftn        instance bool Closure4::Set(class [mscorlib]System.Exception)
    newobj       instance void class [mscorlib]System.Func`2<class [mscorlib]System.Exception, bool>::.ctor(object, native int)

    //
    //   closureHandler.Set(ex);
    //

    newobj       instance void class Closure3::.ctor()
    stloc        'closureHandler'
    ldloc        'closureHandler'
    ldftn        instance void Closure3::Set(class [mscorlib]System.Exception)
    newobj       instance void class [mscorlib]System.Action`1<class [mscorlib]System.Exception>::.ctor(object, native int)

    //
    // }
    //

    call         void [Nuqleon.Runtime.ExceptionServices.Extensions]Nuqleon.Runtime.ExceptionServices.Helpers::TryFilter<class [mscorlib]System.Exception>(class [mscorlib]System.Action, class [mscorlib]System.Func`2<!!0,bool>, class [mscorlib]System.Action`1<!!0>)

    //
    // Assert.IsTrue(closureFilter.called);
    //

    ldloc        'closureFilter'
    ldfld        bool Closure4::called
    
    call         void [Microsoft.VisualStudio.QualityTools.UnitTestFramework]Microsoft.VisualStudio.TestTools.UnitTesting.Assert::IsTrue(bool)

    //
    // Assert.IsTrue(closureHandler.called);
    //

    ldloc        'closureHandler'
    ldfld        bool Closure3::called
    
    call         void [Microsoft.VisualStudio.QualityTools.UnitTestFramework]Microsoft.VisualStudio.TestTools.UnitTesting.Assert::IsTrue(bool)

    //
    // Assert.IsTrue(closureHandler.err.Message.Contains("Oops"));
    //

    ldloc        'closureHandler'
    ldfld        class [mscorlib]System.Exception Closure3::err
    callvirt     instance string class [mscorlib]System.Exception::get_Message()
    ldstr        "Oops"
    callvirt     instance bool class [mscorlib]System.String::Contains(string)
    
    call         void [Microsoft.VisualStudio.QualityTools.UnitTestFramework]Microsoft.VisualStudio.TestTools.UnitTesting.Assert::IsTrue(bool)

    ret
  }

  //       _____                  _ _   _                 __ _ _ _                  _                       _   _          
  //      |_   _| _ _  _  __ __ _(_) |_| |_    _  _ _ _  / _(_) | |_ ___ _ _ ___ __| |  _____ ____ ___ _ __| |_(_)___ _ _  
  //        | || '_| || | \ V  V / |  _| ' \  | || | ' \|  _| | |  _/ -_) '_/ -_) _` | / -_) \ / _/ -_) '_ \  _| / _ \ ' \ 
  //        |_||_|  \_, |  \_/\_/|_|\__|_||_|  \_,_|_||_|_| |_|_|\__\___|_| \___\__,_| \___/_\_\__\___| .__/\__|_\___/_||_|
  //                |__/                                                                              |_|                  
  //      

  //            _____             ___ __  _         
  //           / ___/__  ___  ___/ (_) /_(_)__  ___ 
  //          / /__/ _ \/ _ \/ _  / / __/ / _ \/ _ \
  //          \___/\___/_//_/\_,_/_/\__/_/\___/_//_/
  //          

  .method public void ExceptionServices_Filter_Try_Unfiltered_Condition()
  {
    .custom      instance void [Microsoft.VisualStudio.QualityTools.UnitTestFramework]Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute::.ctor()

    .locals init (class Closure4 'closureFilter', class Closure3 'closureHandler', bool 'hasError')

    //
    // try
    // {
    //

    .try
    {

      //
      // try
      // {
      //   Liaf();
      //

      ldnull
      ldftn      void Tests.Nuqleon.Runtime.ExceptionServices.Helpers::Liaf()
      newobj     instance void class [mscorlib]System.Action::.ctor(object, native int)

      //
      // }
      // catch (Exception ex) if (closureFilter.Set(ex) /* ex.Contains("Oops") */)
      // {
      //

      newobj     instance void class Closure4::.ctor()
      stloc      'closureFilter'
      ldloc      'closureFilter'
      ldftn      instance bool Closure4::Set(class [mscorlib]System.Exception)
      newobj     instance void class [mscorlib]System.Func`2<class [mscorlib]System.Exception, bool>::.ctor(object, native int)

      //
      //   closureHandler.Set(ex);
      //

      newobj     instance void class Closure3::.ctor()
      stloc      'closureHandler'
      ldloc      'closureHandler'
      ldftn      instance void Closure3::Set(class [mscorlib]System.Exception)
      newobj     instance void class [mscorlib]System.Action`1<class [mscorlib]System.Exception>::.ctor(object, native int)

      //
      // }
      //

      call       void [Nuqleon.Runtime.ExceptionServices.Extensions]Nuqleon.Runtime.ExceptionServices.Helpers::TryFilter<class [mscorlib]System.Exception>(class [mscorlib]System.Action, class [mscorlib]System.Func`2<!!0,bool>, class [mscorlib]System.Action`1<!!0>)

      leave      Assert
    }

    //
    // }
    // catch
    // {
    //

    catch [mscorlib]System.Object
    {
      //
      // hasError = true;
      //

      pop
      ldc.i4.1
      stloc      'hasError'
      leave      Assert
    }

    //
    // }
    //

Assert:

    //
    // Assert.IsTrue(closureFilter.called);
    //

    ldloc        'closureFilter'
    ldfld        bool Closure4::called
    
    call         void [Microsoft.VisualStudio.QualityTools.UnitTestFramework]Microsoft.VisualStudio.TestTools.UnitTesting.Assert::IsTrue(bool)

    //
    // Assert.IsFalse(closureHandler.called);
    //

    ldloc        'closureHandler'
    ldfld        bool Closure3::called
    
    call         void [Microsoft.VisualStudio.QualityTools.UnitTestFramework]Microsoft.VisualStudio.TestTools.UnitTesting.Assert::IsFalse(bool)

    //
    // Assert.IsTrue(hasError);
    //

    ldloc        'hasError'
    
    call         void [Microsoft.VisualStudio.QualityTools.UnitTestFramework]Microsoft.VisualStudio.TestTools.UnitTesting.Assert::IsTrue(bool)

    ret
  }

  //           ______             
  //          /_  __/_ _____  ___ 
  //           / / / // / _ \/ -_)
  //          /_/  \_, / .__/\__/ 
  //              /___/_/         
  //          

  .method public void ExceptionServices_Filter_Try_Unfiltered_Type()
  {
    .custom      instance void [Microsoft.VisualStudio.QualityTools.UnitTestFramework]Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute::.ctor()

    .locals init (class Closure4 'closureFilter', class Closure3 'closureHandler', bool 'hasError')

    //
    // try
    // {
    //

    .try
    {

      //
      // try
      // {
      //   Fail();
      //

      ldnull
      ldftn      void Tests.Nuqleon.Runtime.ExceptionServices.Helpers::Fail()
      newobj     instance void class [mscorlib]System.Action::.ctor(object, native int)

      //
      // }
      // catch (IOException ex) if (closureFilter.Set(ex) /* ex.Contains("Oops") */)
      // {
      //

      newobj     instance void class Closure4::.ctor()
      stloc      'closureFilter'
      ldloc      'closureFilter'
      ldftn      instance bool Closure4::Set(class [mscorlib]System.Exception)
      newobj     instance void class [mscorlib]System.Func`2<class [mscorlib]System.Exception, bool>::.ctor(object, native int)

      //
      //   closureHandler.Set(ex);
      //

      newobj     instance void class Closure3::.ctor()
      stloc      'closureHandler'
      ldloc      'closureHandler'
      ldftn      instance void Closure3::Set(class [mscorlib]System.Exception)
      newobj     instance void class [mscorlib]System.Action`1<class [mscorlib]System.Exception>::.ctor(object, native int)

      //
      // }
      //

      call       void [Nuqleon.Runtime.ExceptionServices.Extensions]Nuqleon.Runtime.ExceptionServices.Helpers::TryFilter<class [mscorlib]System.IO.IOException>(class [mscorlib]System.Action, class [mscorlib]System.Func`2<!!0,bool>, class [mscorlib]System.Action`1<!!0>)

      leave      Assert
    }

    //
    // }
    // catch
    // {
    //

    catch [mscorlib]System.Object
    {
      //
      // hasError = true;
      //

      pop
      ldc.i4.1
      stloc      'hasError'
      leave      Assert
    }

    //
    // }
    //

Assert:

    //
    // Assert.IsFalse(closureFilter.called);
    //

    ldloc        'closureFilter'
    ldfld        bool Closure4::called
    
    call         void [Microsoft.VisualStudio.QualityTools.UnitTestFramework]Microsoft.VisualStudio.TestTools.UnitTesting.Assert::IsFalse(bool)

    //
    // Assert.IsFalse(closureHandler.called);
    //

    ldloc        'closureHandler'
    ldfld        bool Closure3::called
    
    call         void [Microsoft.VisualStudio.QualityTools.UnitTestFramework]Microsoft.VisualStudio.TestTools.UnitTesting.Assert::IsFalse(bool)

    //
    // Assert.IsTrue(hasError);
    //

    ldloc        'hasError'
    
    call         void [Microsoft.VisualStudio.QualityTools.UnitTestFramework]Microsoft.VisualStudio.TestTools.UnitTesting.Assert::IsTrue(bool)

    ret
  }


  //   _   _      _                     
  //  | | | | ___| |_ __   ___ _ __ ___ 
  //  | |_| |/ _ \ | '_ \ / _ \ '__/ __|
  //  |  _  |  __/ | |_) |  __/ |  \__ \
  //  |_| |_|\___|_| .__/ \___|_|  |___/
  //               |_|                 
  // 

  .method static void AssertThrows_ArgumentNullException(class [mscorlib]System.Action 'action', string 'argName')
  {
    .try
    {
      ldarg      'action'
      callvirt   instance void [mscorlib]System.Action::Invoke()
	  
      call       void [Microsoft.VisualStudio.QualityTools.UnitTestFramework]Microsoft.VisualStudio.TestTools.UnitTesting.Assert::Fail()

      leave      Exit
    }
    catch [mscorlib]System.ArgumentNullException
    {
      callvirt   instance string [mscorlib]System.ArgumentException::get_ParamName()
      ldarg      'argName'

      call       void [Microsoft.VisualStudio.QualityTools.UnitTestFramework]Microsoft.VisualStudio.TestTools.UnitTesting.Assert::AreEqual(object, object)

      leave      Exit
    }

  Exit:
    ret
  } 

  .method private static void Fail()
  {
    ldstr        "Oops!"
    newobj       instance void class [mscorlib]System.Exception::.ctor(class [mscorlib]System.String)
    throw

    ret
  }

  .method private static void Liaf()
  {
    ldstr        "!spoO"
    newobj       instance void class [mscorlib]System.Exception::.ctor(class [mscorlib]System.String)
    throw

    ret
  }

  .method private static void Nop()
  {
    ret
  }

  .method private static void Nop(class [mscorlib]System.Exception 'ex')
  {
    ret
  }

  .method private static bool True(class [mscorlib]System.Exception 'ex')
  {
    ldc.i4.1
    ret
  }

  .method private static bool False(class [mscorlib]System.Exception 'ex')
  {
    ldc.i4.0
    ret
  }
}

//
// Closure for `new Action(() => { called = true; })`.
//

.class Closure1
{
  .field public bool called

  .method public specialname void .ctor()
  {
    ldarg.0
    call         instance void [mscorlib]System.Object::.ctor()

    ret
  }

  .method public void Set()
  {
    ldarg.0
    ldc.i4.1
    stfld        bool Closure1::called

    ret
  }
}

//
// Closure for `new Func<Exception, bool>(ex => { called = true; return false; })`.
//

.class Closure2
{
  .field public bool called

  .method public specialname void .ctor()
  {
    ldarg.0
    call         instance void [mscorlib]System.Object::.ctor()

    ret
  }

  .method public bool Set(class [mscorlib]System.Exception 'ex')
  {
    ldarg.0
    ldc.i4.1
    stfld        bool Closure2::called

    ldc.i4.0
    ret
  }
}

//
// Closure for `new Action<Exception>(ex => { called = true; err = ex; })`.
//

.class Closure3
{
  .field public class [mscorlib]System.Exception err
  .field public bool called

  .method public specialname void .ctor()
  {
    ldarg.0
    call         instance void [mscorlib]System.Object::.ctor()

    ret
  }

  .method public void Set(class [mscorlib]System.Exception 'ex')
  {
    ldarg.0
    ldc.i4.1
    stfld        bool Closure3::called

    ldarg.0
    ldarg        'ex'
    stfld        class [mscorlib]System.Exception Closure3::err

    ret
  }
}

//
// Closure for `new Func<Exception, bool>(ex => { called = true; return ex.Message.Contains("Oops"); })`.
//

.class Closure4
{
  .field public bool called

  .method public specialname void .ctor()
  {
    ldarg.0
    call         instance void [mscorlib]System.Object::.ctor()

    ret
  }

  .method public bool Set(class [mscorlib]System.Exception 'ex')
  {
    ldarg.0
    ldc.i4.1
    stfld        bool Closure4::called

    ldarg        'ex'
    callvirt     instance string class [mscorlib]System.Exception::get_Message()
    ldstr        "Oops"
    callvirt     instance bool class [mscorlib]System.String::Contains(string)

    ret
  }
}