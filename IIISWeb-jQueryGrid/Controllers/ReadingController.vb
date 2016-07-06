Imports System.Web.Mvc
Imports IIISWeb_jQueryGrid.Models

Namespace Controllers
    Public Class ReadingController
        Inherits Controller

        ' GET: Reading
        <HttpGet>
        Function Index() As ActionResult

            Dim rm As ReadingModel = New ReadingModel()
            rm.Divisions = GetDivisions()


            Return View(rm)
        End Function


        Private Function GetDivisions() As IDictionary(Of String, String)
            Dim divs As IDictionary(Of String, String) = New Dictionary(Of String, String)()

            Dim cmdstr As String
            Dim strErrMsg As String = String.Empty

            cmdstr = "select '%' AS DIVISION_KEY, ' (ALL) ' AS DIVISION_DESC FROM DUAL "
            cmdstr &= "UNION ALL "
            cmdstr &= "SELECT DISTINCT DIVISION_DESC AS DIVISION_KEY, "
            cmdstr &= "DIVISION_DESC AS DIVISION_DESC "
            cmdstr &= "FROM IIISWEB.V_IIIS_BRIDGE_UNDERCLEARANCE "
            cmdstr &= " ORDER BY DIVISION_DESC ASC "

            Dim rd As ReadingsData = New ReadingsData
            rd.dbOpenConnection()
            Dim cmd As IDbCommand = rd.IIISWebDb.CreateCommand

            Try
                'cmd.Connection.Open()
                cmd.CommandText = cmdstr
                Dim dr As IDataReader = cmd.ExecuteReader
                'Dim lstReadings As IDictionary(Of String, String) = New IDictionary(Of String, String)

                'Dim readings As IOrderedQueryable(Of ReadingModel)
                While dr.Read

                    divs.Add(dr(0), dr(1))
                End While


            Catch ex As Exception
                strErrMsg = "Exception while acquiring data:  " + ex.Source + ex.Message
                Utilities.WriteError(ex, strErrMsg)
                Exit Function

            Finally
                rd.dbCloseConnection()
                cmd.Dispose()
            End Try
            'makes.Add("1", "Acura")
            'makes.Add("2", "Audi")
            'makes.Add("3", "BMW")

            Return divs
        End Function

#Region "JSON endpoints"

        ' All JSON endpoints require [HttpPost] to prevent JSON hijacking.
        ' With [HttpPost], returning arrays Is allowed.  
        ' See http://haacked.com/archive/2009/06/25/json-hijacking.aspx

        <HttpGet()>
        Public Function ReadingList(page As System.Nullable(Of Integer), limit As System.Nullable(Of Integer), sortBy As String, direction As String, Optional searchString As String = Nothing) As JsonResult


            'Dim readings As Object = Using<GetReadingsListForUser>()
            '    .Execute(CurrentUserId)

            'Dim model As Object = New ReadingListViewModel(readings)
            Dim total As Integer

            Dim readings As IOrderedQueryable(Of ReadingModel)
            Try

                readings = New ReadingModel().GetAllReadings(page, limit, sortBy, direction, searchString, total)

                ' this version works! TVO 6/24/2016
                Return Json(New JsonReadingListViewModel With {
                            .JsonReadings = readings,
                            .JsonTotal = total
                            }, JsonRequestBehavior.AllowGet)

            Catch ex As Exception
                Utilities.WriteError(ex, "GetAllReadings")
            End Try

        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Reading"></param>
        ''' <returns></returns>
        <HttpPost()>
        Public Function Save(Reading As ReadingModel) As JsonResult
            Dim model As New ReadingModel()
            Reading.ADD_Date = System.DateTime.Now
            Reading.ADD_ID = "21179"
            Reading.Update_Date = System.DateTime.Now
            Reading.UPDATE_ID = "21179"
            model.Save(Reading)
            Return Json(True)

        End Function


        '[HttpPost]
        'Public JsonResult Remove(int id)
        '{
        '    New GridModel().Remove(id);
        '    Return Json(True);
        '}

        '<HttpPost()>
        'Public Function JsonReadingList() As JsonResult

        '    Dim listReadings As GetReadingsListForUser = New GetReadingsListForUser

        '    Dim list As Object = listReadings.Execute().Select(Function(x) As JsonReadingsViewModel
        '                                                           Return ToJsonReadingsViewModel(x)
        '                                                       End Function).ToList()

        '    '.Select(x >= ToJsonReadingsViewModel(x))
        '    '.ToList();

        '    Return Json(list)
        'End Function

        Private Function ToJsonReadingsViewModel(reading As ReadingModel) As JsonReadingsViewModel

            Dim jrvm As New JsonReadingsViewModel
            With jrvm
                .Milepost = reading.Milepost
                .INSP_ID = reading.INSP_ID
                .UC_Read_Date = reading.UC_Read_Date
                .Inspect_Date = reading.Inspect_Date
                '.Bin = reading.Bin
                '.BRIDGE_LOCATION = reading.BRIDGE_LOCATION


            End With
            Return jrvm

            'VehicleId = vehicle.VehicleId,
            '               Name = vehicle.Name,
            '               SortOrder = vehicle.SortOrder,
            '               Year = vehicle.Year,
            '               MakeName = vehicle.MakeName,
            '               ModelName = vehicle.ModelName,
            '               Odometer = vehicle.Odometer,
            '               PhotoId = vehicle.PhotoId,
            '               LifeTimeStatistics = New JsonStatisticsViewModel(),
            '               //Not used
            '               LastTwelveMonthsStatistics = last12Stats,
            '               OverdueReminders = overdue ?? New List<ReminderSummaryModel>()

        End Function

#End Region
    End Class
End Namespace