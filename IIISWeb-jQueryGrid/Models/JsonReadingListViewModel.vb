
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations

Namespace Models
    Public Class JsonReadingListViewModel

        Public Sub New()

            JsonReadings = _jsonReadings
            JsonTotal = _jsonTotal
            ' JsonSelectedRdgID = _selectedID

        End Sub


        Public Property JsonReadings() As IOrderedQueryable(Of ReadingModel)
            Get
                Return _jsonReadings
            End Get
            Set(value As IOrderedQueryable(Of ReadingModel))
                _jsonReadings = value
            End Set

        End Property

        Private Property _jsonReadings As IOrderedQueryable(Of ReadingModel)

        Public Property JsonTotal As Integer
            Get
                Return _jsonTotal
            End Get
            Set(value As Integer)
                _jsonTotal = value
            End Set
        End Property

        Private Property _jsonTotal As Integer

        'Public Property JsonSelectedRdgID As Integer
        '    Get
        '        Return _selectedID
        '    End Get
        '    Set(value As Integer)
        '        _selectedID = value
        '    End Set
        'End Property

        'Private Property _selectedID As Integer

    End Class

End Namespace
