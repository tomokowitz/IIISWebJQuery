Imports System.Collections.Generic
Imports IIISWeb_jQueryGrid.Models

Public Interface IReadingsRepository

    Sub Create(userId As Int16, reading As ReadingModel)

    Function GetReading(ReadingID As Integer) As ReadingModel

    Function GetAllReadings() As IOrderedQueryable(Of ReadingModel)


End Interface
