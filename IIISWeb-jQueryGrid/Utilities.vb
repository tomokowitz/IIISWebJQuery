Imports System.IO
Imports System.Text
'Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Configuration
Imports Microsoft.VisualBasic.Logging
Imports System.Collections.Specialized

Public Class Utilities

    Public Sub StartErrorLog()

        Trace.Listeners.Add(New TextWriterTraceListener("ErrorOutput.log"))

    End Sub

    Public Shared Sub WriteError(ByVal ex As System.Exception, ByVal strErr As String)

        Dim errorlog As IIIS_Web_ErrorLog
        'Instantiating trace listener, passing in exception and description parameters
        errorlog = New IIIS_Web_ErrorLog(ex, strErr)
        'Generating message for user to be displayed in popup message box
        MsgBox("Sorry ... a(n) " + ex.GetType().Name.ToString() + " error has occurred.  Please notify the Engineering Systems application development team.")
 
    End Sub
End Class
Public Class IIIS_Web_ErrorLog
    Inherits FileLogTraceListener

    Public Sub New(ByVal ex As System.Exception, ByVal strErr As String)
        Dim lnrEventLog As New FileLogTraceListener("lnrErrorLog")
        Dim strErrMsg As String = String.Empty
        Dim strMetaErrMsg As String = String.Empty
        Dim strMsgTitle As String = String.Empty
        Dim strStackTrace As String = String.Empty
        Dim strDir As String = String.Empty

        ' Getting info from exception and description parameters
        strMsgTitle = strErr + ex.GetType().ToString()
        strErrMsg = ex.Source + ex.Message + ex.TargetSite.ToString()
        strStackTrace = ex.ToString()
        'Getting location of log directory from app config
        strDir = ConfigurationManager.AppSettings("ErrorLogPath").ToString()
        'Checking if directory exists
        If Not System.IO.Directory.Exists(strDir) Then
            System.IO.Directory.CreateDirectory(strDir)

        End If
        ' executing trace listener
        With lnrEventLog
            .AutoFlush = True
            .CustomLocation = strDir
            .Location = LogFileLocation.Custom
            .LogFileCreationSchedule = LogFileCreationScheduleOption.Daily
            '.BaseFileName = "IIISWebErrorLog"
            .BaseFileName = ConfigurationManager.AppSettings("LogFileBaseName")
            .WriteLine("Title: " & strMsgTitle & vbCrLf)
            .WriteLine("Message: " & strErrMsg & vbCrLf)
            .WriteLine("StackTrace: " & strStackTrace & vbCrLf)
            .WriteLine("Date/Time: " & DateTime.Now.ToString() & vbCrLf)
            .WriteLine("================================================" & vbCrLf)
        End With
        lnrEventLog.Close()
    End Sub
End Class
