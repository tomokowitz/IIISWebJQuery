Imports System.Web.Mvc
Imports IIISWeb_jQueryGrid.Models


Namespace Controllers
    Public Class HomeController
        Inherits Controller

        ' GET: Reading
        <HttpGet>
        Public Function Index() As ActionResult
            '      Public Function GetAllReadings() As ActionResult


            Try
                'Dim rm As ReadingModel = New ReadingModel()
                'rm.Divisions = GetDivisions()
                'Dim readings As IEnumerable(Of ReadingModel) = rm.GetAllReadings
                'Return View(readings)
                Return View()

            Catch ex As Exception
                Utilities.WriteError(ex, "GetAllReadings")
            End Try

        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>

        '<HttpPost()>
        'Public Function 

        'End Function

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

        'Public Function GetPlayers(page As System.Nullable(Of Integer), limit As System.Nullable(Of Integer), sortBy As String, direction As String, Optional searchString As String = Nothing) As JsonResult
        '    Dim total As Integer
        '    Dim records = New GridModel().GetPlayers(page, limit, sortBy, direction, searchString, total)
        '    Return Json(New From {records, total}, JsonRequestBehavior.AllowGet)

        'End Function


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

    <AttributeUsage(AttributeTargets.[Class] Or AttributeTargets.Method)>
    Public NotInheritable Class NoCacheAttribute
        Inherits ActionFilterAttribute
        Public Overrides Sub OnResultExecuting(filterContext As ResultExecutingContext)
            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1))
            filterContext.HttpContext.Response.Cache.SetValidUntilExpires(False)
            filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches)
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache)
            filterContext.HttpContext.Response.Cache.SetNoStore()

            MyBase.OnResultExecuting(filterContext)
        End Sub
    End Class

End Namespace