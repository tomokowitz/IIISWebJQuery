Imports System
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Web
Imports System.Linq
Imports System.Linq.Expressions
Imports System.Runtime.CompilerServices.ExtensionAttribute
Namespace Models
    Public Class SortHelper


        Public Shared Function OrderBy(Of T)(source As IQueryable(Of T), [property] As String) As IOrderedQueryable(Of T)
            Return ApplyOrder(Of T)(source, [property], "OrderBy")
        End Function

        Public Shared Function OrderByDescending(Of T)(source As IQueryable(Of T), [property] As String) As IOrderedQueryable(Of T)
            Return ApplyOrder(Of T)(source, [property], "OrderByDescending")
        End Function

        Public Shared Function ThenBy(Of T)(source As IOrderedQueryable(Of T), [property] As String) As IOrderedQueryable(Of T)
            Return ApplyOrder(Of T)(source, [property], "ThenBy")
        End Function

        Public Shared Function ThenByDescending(Of T)(source As IOrderedQueryable(Of T), [property] As String) As IOrderedQueryable(Of T)
            Return ApplyOrder(Of T)(source, [property], "ThenByDescending")
        End Function

        Private Shared Function ApplyOrder(Of T)(source As IQueryable(Of T), [property] As String, methodName As String) As IOrderedQueryable(Of T)
            Dim props As String() = [property].Split("."c)
            Dim type As Type = GetType(T)
            Dim arg As ParameterExpression = Expression.Parameter(type, "x")
            Dim expr As Expression = arg

            Try
                For Each prop As String In props
                    ' use reflection (not ComponentModel) to mirror LINQ
                    Dim pi As PropertyInfo = type.GetProperty(prop)
                    expr = Expression.[Property](expr, pi)
                    type = pi.PropertyType
                Next
                Dim delegateType As Type = GetType(Func(Of , )).MakeGenericType(GetType(T), type)
                Dim lambda As LambdaExpression = Expression.Lambda(delegateType, expr, arg)

                Dim result As Object = GetType(Queryable).GetMethods().[Single](Function(method) method.Name = methodName AndAlso method.IsGenericMethodDefinition AndAlso method.GetGenericArguments().Length = 2 AndAlso method.GetParameters().Length = 2).MakeGenericMethod(GetType(T), type).Invoke(Nothing, New Object() {source, lambda})
                Return DirectCast(result, IOrderedQueryable(Of T))
            Catch ex As Exception
                Utilities.WriteError(ex, "GetAllReadings")
            End Try

        End Function


    End Class
End Namespace