Imports System.Linq.Expressions

'
' Copyright Microsoft Corporation.  All rights reserved.
' bartde - May 2013
'

Public Module VisualBasicModule

    Public Function GetAnonymousObject() As Object
        Return New With {.Bar = 42}
    End Function

    Public Function GetExpressionWithClosure() As Expression(Of Func(Of Integer))
        Dim x As Integer
        Return Function() x
    End Function

    Public Function GetAnonymousTypeWithKeysExpression() As Expression
        Return GetAnonymousTypeWithKeysExpression_Impl(Function() New With {Key .Bar = 42, .Foo = 43})
    End Function

    Private Function GetAnonymousTypeWithKeysExpression_Impl(Of T)(e As Expression(Of Func(Of T))) As Expression
        Return e
    End Function

End Module
