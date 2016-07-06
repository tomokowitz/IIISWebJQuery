Option Explicit On
Option Strict Off
Imports Microsoft.VisualBasic
Imports System.Data
Imports Oracle.DataAccess
Imports Oracle.DataAccess.Client
Imports System.IO
Imports System.Configuration
Imports System.Text.RegularExpressions
Imports System.Web.UI
Imports System.Reflection
Imports IIISWeb_jQueryGrid.Models

Public Class ReadingsData

    Public IIISWebDb As OracleConnection
    Public IIISDevDb As OracleConnection 'TVO 2/24/2015
    Public sErrMsg As String
    Public sWinID As String

    'Protected DSConfig, UIDConfig, PWConfig, DS, UID, PW As String
    Protected DSConfig As String = "bucDS"
    Protected UIDConfig As String = "bucUID"
    Protected PWConfig As String = "bucPW"
    Protected DS As String = ConfigurationManager.AppSettings(DSConfig)
    Protected UID As String = ConfigurationManager.AppSettings(UIDConfig)
    Protected PW As String = ConfigurationManager.AppSettings(PWConfig)

    Private Const shortDatePattern As String = "MM/dd/yyyy" 'not quite same as Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern
    Private Const shortDateTimePattern As String = "MM/dd/yyyy hh:mm tt"
    Private Const shortDateTimePatternOracle As String = "MM/DD/YYYY HH:MI AM"
    Private Const NLS_DATE_FORMAT As String = "dd-MMM-yyyy"  'default format of dates from Oracle
    Public Sub New()
        IIISWebDb = New OracleConnection
        IIISWebDb.ConnectionString = ConfigurationManager.ConnectionStrings("IIISWebDB").ConnectionString
        'TVO 2/24/2015
        IIISDevDb = New OracleConnection
        IIISDevDb.ConnectionString = ConfigurationManager.ConnectionStrings("IIISDB").ConnectionString


    End Sub
    Public Function dbOpenConnection() As Boolean
        dbOpenConnection = True
        Try
            If IIISWebDb.State <> ConnectionState.Open Then
                IIISWebDb.Open()
                dbOpenConnection = True
            End If
        Catch ex As Exception
            dbOpenConnection = False
            sErrMsg = sErrMsg & "failed"
            sErrMsg = sErrMsg & String.Concat(ex.Message, "IIIS clsBridgeDBMgr.dbOpenConnection")
        End Try
    End Function
    ' TVO 2/24/2015
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function dbOpenIIISConnection() As Boolean
        dbOpenIIISConnection = True
        Try
            If IIISDevDb.State <> ConnectionState.Open Then
                IIISDevDb.Open()
                dbOpenIIISConnection = True
            End If
        Catch ex As Exception
            dbOpenIIISConnection = False
            sErrMsg = sErrMsg & "failed"
            sErrMsg = sErrMsg & String.Concat(ex.Message, "IIIS clsBridgeDBMgr.dbOpenConnection")
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    Public Sub dbCloseConnection()
        Try
            If IIISWebDb.State = ConnectionState.Open Then
                IIISWebDb.Close()
            End If
        Catch ex As Exception
            sErrMsg = sErrMsg & String.Concat(ex.Message, "IIISWeb dbManager.dbCloseConnection")
        End Try
    End Sub

    ''' <summary>Formats the input value as a 'To_Date()' string for inclusion in a SQL string.  Returns NULL for DBNull and non-date values.</summary>
    ''' <param name="dt">A date object to be converted to a SQL string value.</param>
    ''' <returns>Input value as string, or NULL if input is DBNull or not a date.</returns>
    Public Function GetDateSQL(ByVal dt As Object) As String  'object version checks for NULL and invalid values
        If IsDBNull(dt) OrElse Not IsDate(dt) Then
            'Return "NULL"
            Return ""
        Else
            Return GetDateSQL(Date.Parse(dt))
        End If
    End Function




    ''' <summary>converts Nothing or an empty string to DBNull for posting to DB, otherwise formats date to default DB string format</summary>
    ''' <param name="val">Object representing date value.</param>
    ''' <returns>Formatted input value or DBNull.</returns>
    Public Function UnNVLDate(val As String) As Object
        Dim tmp As Date



        If val Is Nothing OrElse val.Length = 0 OrElse Not Date.TryParse(val, tmp) OrElse Date.Parse(val).ToString(shortDatePattern) = "01/01/1001" OrElse Date.Parse(val) = #1/1/1001# Then
            Return DBNull.Value

        Else
            Return Date.Parse(val).ToString(shortDatePattern)
        End If
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="val"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function NVL(val As Object) As Object
        Return NVL(val, Nothing)
    End Function
    ''' <summary>Filter for DBNull value, so DB values can be safely used in code.</summary>
    ''' <param name="val">Object representing value from the database.</param>
    ''' <param name="defaultVal">Value to return if input value is DBNull</param>
    ''' <returns>Input value or defaultVal, if value is DBNull.</returns>
    Public Function NVL(val As Object, defaultVal As Object) As Object
        If IsDBNull(val) Then
            Return defaultVal
        Else
            Return val
        End If
    End Function
    ''' <summary>Filter for DBNull value, so DB values can be safely used in code.</summary>
    ''' <param name="val">Object representing value from the database, and indicated type.</param>
    ''' <returns>Input value or default value for type, if input value is DBNull.</returns>
    Public Function NVL(Of T)(val As Object) As T
        If IsDBNull(val) Then
            Dim x As T
            Return x
        Else
            Return val
        End If
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Friend Class MyDB
        Inherits OracleHelper

        Public Sub New()

            MyBase.New(System.Configuration.ConfigurationManager.ConnectionStrings("IIISDB").ConnectionString)

        End Sub

    End Class

    Public Sub Map(ReadingRecord As IDataRecord, Reading As ReadingModel)


        Try

            Reading.Milepost = ReadingRecord("Milepost").ToString
            Reading.INSP_ID = ReadingRecord("INSP_ID").ToString
            Reading.UC_Read_Date = ReadingRecord("UC_Read_Date")
            Reading.Inspect_Date = ReadingRecord("Inspect_Date")

        Catch ex As Exception
            Utilities.WriteError(ex, "Error mapping record to model")

        End Try


    End Sub


    Public Sub Map(DivRecord As IDataRecord, list As DivisionList)


    End Sub



    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="entUC"></param>
    ''' <returns></returns>




    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strMP"></param>
    ''' <param name="strDivision"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetBrdgUCDT(ByVal strMP As String, Optional ByVal strDivision As String = "%") As DataTable


        Dim strCx As String
        Dim strErrMsg As String = String.Empty

        Dim dt As DataTable = New DataTable("dtUCMP")


        'Dim cmdstr As String = GetBrdgUCSql(strMP, strBrType)
        Dim cmdstr As String = GetBrdgUCSql(strMP, , strDivision)
        strCx = DBFunctionality.CxStringBuilder(DS, UID, PW)


        Try

            dt = DBFunctionality.GetDataTable(cmdstr, strCx)


        Catch orEx As OracleException
            strErrMsg = "Oracle exception while acquiring data:  " + orEx.Source + orEx.Message

            Utilities.WriteError(orEx, strErrMsg)
            Exit Function
        Catch adoEx As DataException

            strErrMsg = "Data exception while acquiring data:  " + adoEx.Source + adoEx.Message

            Utilities.WriteError(adoEx, strErrMsg)
            Exit Function

        Catch ex As Exception
            strErrMsg = "Exception while acquiring data:  " + ex.Source + ex.Message

            Utilities.WriteError(ex, strErrMsg)
            Exit Function

        End Try
        Return dt

    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strMP"></param>
    ''' <param name="strSortStr"></param>
    ''' <param name="strDivision"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetBrdgUCSql(ByVal strMP As String, Optional strSortStr As String = "PAGE_INDEX, UC_READ_DATE DESC", Optional strDivision As String = "%") As String
        Const delim As String = ", "
        Dim sortArgs() As String = strSortStr.Split(delim)
        Dim sbSqlStrBldr1 As StringBuilder = New StringBuilder(String.Empty)
        Dim sbSqlTables1 As StringBuilder = New StringBuilder(String.Empty)
        Dim sbSqlQualifiers1 As StringBuilder = New StringBuilder(String.Empty)
        Dim sbSqlStrBldr2 As StringBuilder = New StringBuilder(String.Empty)
        Dim sbSqlTables2 As StringBuilder = New StringBuilder(String.Empty)
        Dim sbSqlQualifiers2 As StringBuilder = New StringBuilder(String.Empty)
        Dim sbSqlSorts1 As StringBuilder = New StringBuilder(String.Empty)
        Dim cmdstr As String
        Dim strErrMsg As String = String.Empty
        Dim listSQLStr As List(Of String) = New List(Of String)
        Dim listSQLStr2 As List(Of String) = New List(Of String)

        Dim listSortStr As List(Of String) = New List(Of String)

        Dim strFilter As String = String.Empty

        ' Calculating boundaries of milepost select range for grid view caching 

        ' What about division change which limits range of mp's
        strFilter = GetMPRangeString(strMP, strDivision)



        Dim Sql As String = String.Empty
        Try
            Sql = " SELECT DISTINCT  "
            Sql = Sql & "TO_CHAR(PI.MILEPOST,'FM9999999.90') AS MILEPOST,  "
            Sql = Sql & "VBU.MEASUREMENT_DESCRIPTION as FEATURE_CROSSED,  "
            Sql = Sql & "IIIS.FN_IIIS_GET_BRDG_FEATS_CARRIED(PI.MILEPOST) AS FEATURE_CARRIED, "
            Sql = Sql & "TO_DATE(to_char(VBU.UC_READ_DATE, 'MM/DD/YYYY'),'MM/DD/YYYY') as UC_READ_DATE,  "
            Sql = Sql & "TO_DATE(to_char(VBU.INSPECT_DATE, 'MM/DD/YYYY'),'MM/DD/YYYY') as INSPECT_DATE,  "
            Sql = Sql & "VBU.ID AS INSP_ID, VBU.OVERHEAD_OR_MAINLINE_BRIDGE, "
            Sql = Sql & "VBU.a_measure, VBU.b_measure,VBU.c_measure, VBU.d_measure, VBU.e_measure,  "
            Sql = Sql & "VBU.f_measure, VBU.g_measure, "
            Sql = Sql & "VBU.h_measure, VBU.aa_measure, VBU.bb_measure, VBU.cc_measure,  "
            Sql = Sql & "VBU.dd_measure, VBU.ee_measure, "
            Sql = Sql & "VBU.ff_measure, VBU.gg_measure, VBU.hh_measure, PI.bin, PI.BRIDGE_LOCATION ,  "
            Sql = Sql & "VBU.ADD_DATE, VBU.ADD_ID, PI.DIVISION_DESC,  "
            Sql = Sql & "VBU.ADD_DATE, VBU.ADD_ID,   "
            Sql = Sql & " CASE PI.DIR_ORIENT WHEN 'N.' THEN 'NORTH' WHEN 'N.E.' THEN 'NORTH EAST'  "
            Sql = Sql & "        WHEN 'S.E.' THEN 'SOUTH EAST' WHEN 'S.' THEN 'SOUTH' "
            Sql = Sql & "        WHEN 'S.W.' THEN 'SOUTH WEST' WHEN 'W.' THEN 'WEST'  "
            Sql = Sql & "        WHEN 'N.W.' THEN 'NORTH WEST' WHEN 'D.L.' THEN 'D.L.'  "
            Sql = Sql & " END AS DIR_ORIENT, "
            Sql = Sql & " PI.DIVISION_DESC,  "
            Sql = Sql & " VBU.remarks, VBU.TWY_TRAFFIC_DIRECTION,  "
            Sql = Sql & "VBU.NORTH_OR_WEST_BOUND || '/' || VBU.SOUTH_OR_EAST_BOUND AS NS_OR_EW_BOUND,  "
            'Sql = Sql & " --VBI.PRIMARY_INSP_RESPONSIBL_CODE,  "
            Sql = Sql & " PI.PAGE_INDEX  "
            Sql = Sql & " "
            Sql = Sql & "FROM "
            Sql = Sql & " "
            Sql = Sql & "IIIS.V_IIIS_BRIDGE_UNDERCLEARANCE VBU,  "
            Sql = Sql & "    (SELECT ROW_NUMBER() OVER  "
            Sql = Sql & "        (ORDER BY MILEPOST) AS PAGE_INDEX, "
            Sql = Sql & "        MILEPOST, "
            Sql = Sql & "        DECODE (direction_orient_code, "
            Sql = Sql & "                  '1', 'N.', "
            Sql = Sql & "                  '2', 'N.E.', "
            Sql = Sql & "                  '3', 'E.', "
            Sql = Sql & "                  '4', 'S.E.', "
            Sql = Sql & "                  '5', 'S.', "
            Sql = Sql & "                  '6', 'S.W.', "
            Sql = Sql & "                  '7', 'W.', "
            Sql = Sql & "                  '8', 'N.W.', "
            Sql = Sql & "                  'D.L.') AS DIR_ORIENT, "
            Sql = Sql & "                  DIVISION_DESC, "
            Sql = Sql & "                  BIN, "
            Sql = Sql & "                  BRIDGE_LOCATION FROM  "
            Sql = Sql & "        ( "
            Sql = Sql & "            SELECT DISTINCT  "
            Sql = Sql & "                BBUC.MILEPOST,  "
            Sql = Sql & "                BI2.DIRECTION_ORIENT_CODE,  "
            Sql = Sql & "                bi2.bin, "
            Sql = Sql & "                bi2.bridge_location, "
            Sql = Sql & "                RD.DIVISION_DESC "
            Sql = Sql & "            FROM  "
            Sql = Sql & "                IIIS.V_IIIS_BRIDGE_UNDERCLEARANCE BBUC, "
            Sql = Sql & "                iiis.iiis_bridge_inventory bi2, "
            Sql = Sql & "                REF.ref_milepost_inventory rmi, "
            Sql = Sql & "                REF.ref_division rd "
            Sql = Sql & "            WHERE  "
            Sql = Sql & "                BBUC.milepost = rmi.milepost AND  "
            Sql = Sql & "                BBUC.milepost = bi2.milepost AND  "
            Sql = Sql & "                rmi.division_code = rd.division_code AND  "
            Sql = Sql & "                BBUC.MILEPOST IN "
            Sql = Sql & "                (" & strFilter & ") AND "
            Sql = Sql & "                BBUC.OVERHEAD_OR_MAINLINE_BRIDGE = 1 "
            Sql = Sql & "            UNION "
            Sql = Sql & "                SELECT DISTINCT  "
            Sql = Sql & "                BBUC.MILEPOST,  "
            Sql = Sql & "                BI2.DIRECTION_ORIENT_CODE,  "
            Sql = Sql & "                bi2.bin, "
            Sql = Sql & "                bi2.bridge_location, "
            Sql = Sql & "                RD.DIVISION_DESC "
            Sql = Sql & "            FROM  "
            Sql = Sql & "                IIIS.V_IIIS_BRIDGE_UNDERCLEARANCE BBUC, "
            Sql = Sql & "                iiis.iiis_bridge_inventory bi2, "
            Sql = Sql & "                REF.ref_milepost_inventory rmi, "
            Sql = Sql & "                REF.ref_division rd "
            Sql = Sql & "            WHERE  "
            Sql = Sql & "                BBUC.milepost = rmi.milepost AND  "
            Sql = Sql & "                BBUC.milepost = bi2.milepost AND  "
            Sql = Sql & "                rmi.division_code = rd.division_code AND  "
            Sql = Sql & "                BBUC.MILEPOST IN "
            Sql = Sql & "                (" & strFilter & ") AND "
            Sql = Sql & "                BBUC.OVERHEAD_OR_MAINLINE_BRIDGE = 2 "
            Sql = Sql & "                 "
            Sql = Sql & "        )BI "
            Sql = Sql & "    )PI "
            Sql = Sql & "WHERE  "
            Sql = Sql & "VBU.MILEPOST = PI.MILEPOST  "
            Sql = Sql & "ORDER BY "

            For Each Str As String In sortArgs
                listSortStr.Add(Str)

            Next
            Sql = Sql & strSortStr


        Catch ex As Exception
            strErrMsg = "Exception while acquiring data:  " + ex.Source + ex.Message

            Utilities.WriteError(ex, strErrMsg)
            'Exit Function

        End Try

        Return Sql

    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strSortStr"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAllUCSql(Optional strSortStr As String = "PAGE_INDEX, UC_READ_DATE DESC")
        Const delim As String = ", "
        Dim sortArgs() As String = strSortStr.Split(delim)
        Dim sbSqlStrBldr1 As StringBuilder = New StringBuilder(String.Empty)
        Dim sbSqlTables1 As StringBuilder = New StringBuilder(String.Empty)
        Dim sbSqlQualifiers1 As StringBuilder = New StringBuilder(String.Empty)
        Dim sbSqlStrBldr2 As StringBuilder = New StringBuilder(String.Empty)
        Dim sbSqlTables2 As StringBuilder = New StringBuilder(String.Empty)
        Dim sbSqlQualifiers2 As StringBuilder = New StringBuilder(String.Empty)
        Dim sbSqlSorts1 As StringBuilder = New StringBuilder(String.Empty)
        Dim cmdstr As String
        Dim strErrMsg As String = String.Empty
        Dim listSQLStr As List(Of String) = New List(Of String)
        Dim listSQLStr2 As List(Of String) = New List(Of String)

        Dim listSortStr As List(Of String) = New List(Of String)

        'Dim strFilter As String = String.Empty

        ' Calculating boundaries of milepost select range for grid view caching 

        ' What about division change which limits range of mp's
        'strFilter = GetMPRangeString(strMP, strDivision)



        Dim Sql As String = String.Empty
        Try
            Sql = " SELECT DISTINCT  "
            Sql = Sql & "TO_CHAR(PI.MILEPOST,'FM9999999.90') AS MILEPOST,  "
            Sql = Sql & "VBU.MEASUREMENT_DESCRIPTION as FEATURE_CROSSED,  "
            Sql = Sql & "IIIS.FN_IIIS_GET_BRDG_FEATS_CARRIED(PI.MILEPOST) AS FEATURE_CARRIED, "
            Sql = Sql & "TO_DATE(to_char(VBU.UC_READ_DATE, 'MM/DD/YYYY'),'MM/DD/YYYY') as UC_READ_DATE,  "
            Sql = Sql & "TO_DATE(to_char(VBU.INSPECT_DATE, 'MM/DD/YYYY'),'MM/DD/YYYY') as INSPECT_DATE,  "
            Sql = Sql & "VBU.ID AS INSP_ID, VBU.OVERHEAD_OR_MAINLINE_BRIDGE, "
            Sql = Sql & "VBU.a_measure, VBU.b_measure,VBU.c_measure, VBU.d_measure, VBU.e_measure,  "
            Sql = Sql & "VBU.f_measure, VBU.g_measure, "
            Sql = Sql & "VBU.h_measure, VBU.aa_measure, VBU.bb_measure, VBU.cc_measure,  "
            Sql = Sql & "VBU.dd_measure, VBU.ee_measure, "
            Sql = Sql & "VBU.ff_measure, VBU.gg_measure, VBU.hh_measure, PI.bin, PI.BRIDGE_LOCATION ,  "
            Sql = Sql & "VBU.ADD_DATE, VBU.ADD_ID, PI.DIVISION_DESC,  "
            Sql = Sql & "VBU.ADD_DATE, VBU.ADD_ID,   "
            Sql = Sql & " CASE PI.DIR_ORIENT WHEN 'N.' THEN 'NORTH' WHEN 'N.E.' THEN 'NORTH EAST'  "
            Sql = Sql & "        WHEN 'S.E.' THEN 'SOUTH EAST' WHEN 'S.' THEN 'SOUTH' "
            Sql = Sql & "        WHEN 'S.W.' THEN 'SOUTH WEST' WHEN 'W.' THEN 'WEST'  "
            Sql = Sql & "        WHEN 'N.W.' THEN 'NORTH WEST' WHEN 'D.L.' THEN 'D.L.'  "
            Sql = Sql & " END AS DIR_ORIENT, "
            Sql = Sql & " PI.DIVISION_DESC,  "
            Sql = Sql & " VBU.remarks, VBU.TWY_TRAFFIC_DIRECTION,  "
            Sql = Sql & "VBU.NORTH_OR_WEST_BOUND || '/' || VBU.SOUTH_OR_EAST_BOUND AS NS_OR_EW_BOUND,  "
            'Sql = Sql & " --VBI.PRIMARY_INSP_RESPONSIBL_CODE,  "
            Sql = Sql & " PI.PAGE_INDEX  "
            Sql = Sql & " "
            Sql = Sql & "FROM "
            Sql = Sql & " "
            Sql = Sql & "IIIS.V_IIIS_BRIDGE_UNDERCLEARANCE VBU,  "
            Sql = Sql & "    (SELECT ROW_NUMBER() OVER  "
            Sql = Sql & "        (ORDER BY MILEPOST) AS PAGE_INDEX, "
            Sql = Sql & "        MILEPOST, "
            Sql = Sql & "        DECODE (direction_orient_code, "
            Sql = Sql & "                  '1', 'N.', "
            Sql = Sql & "                  '2', 'N.E.', "
            Sql = Sql & "                  '3', 'E.', "
            Sql = Sql & "                  '4', 'S.E.', "
            Sql = Sql & "                  '5', 'S.', "
            Sql = Sql & "                  '6', 'S.W.', "
            Sql = Sql & "                  '7', 'W.', "
            Sql = Sql & "                  '8', 'N.W.', "
            Sql = Sql & "                  'D.L.') AS DIR_ORIENT, "
            Sql = Sql & "                  DIVISION_DESC, "
            Sql = Sql & "                  BIN, "
            Sql = Sql & "                  BRIDGE_LOCATION FROM  "
            Sql = Sql & "        ( "
            Sql = Sql & "            SELECT DISTINCT  "
            Sql = Sql & "                BBUC.MILEPOST,  "
            Sql = Sql & "                BI2.DIRECTION_ORIENT_CODE,  "
            Sql = Sql & "                bi2.bin, "
            Sql = Sql & "                bi2.bridge_location, "
            Sql = Sql & "                RD.DIVISION_DESC "
            Sql = Sql & "            FROM  "
            Sql = Sql & "                IIIS.V_IIIS_BRIDGE_UNDERCLEARANCE BBUC, "
            Sql = Sql & "                iiis.iiis_bridge_inventory bi2, "
            Sql = Sql & "                REF.ref_milepost_inventory rmi, "
            Sql = Sql & "                REF.ref_division rd "
            Sql = Sql & "            WHERE  "
            Sql = Sql & "                BBUC.milepost = rmi.milepost AND  "
            Sql = Sql & "                BBUC.milepost = bi2.milepost AND  "
            Sql = Sql & "                rmi.division_code = rd.division_code AND  "
            'Sql = Sql & "                BBUC.MILEPOST IN "
            'Sql = Sql & "                (" & strFilter & ") AND "
            Sql = Sql & "                BBUC.OVERHEAD_OR_MAINLINE_BRIDGE = 1 "
            Sql = Sql & "            UNION "
            Sql = Sql & "                SELECT DISTINCT  "
            Sql = Sql & "                BBUC.MILEPOST,  "
            Sql = Sql & "                BI2.DIRECTION_ORIENT_CODE,  "
            Sql = Sql & "                bi2.bin, "
            Sql = Sql & "                bi2.bridge_location, "
            Sql = Sql & "                RD.DIVISION_DESC "
            Sql = Sql & "            FROM  "
            Sql = Sql & "                IIIS.V_IIIS_BRIDGE_UNDERCLEARANCE BBUC, "
            Sql = Sql & "                iiis.iiis_bridge_inventory bi2, "
            Sql = Sql & "                REF.ref_milepost_inventory rmi, "
            Sql = Sql & "                REF.ref_division rd "
            Sql = Sql & "            WHERE  "
            Sql = Sql & "                BBUC.milepost = rmi.milepost AND  "
            Sql = Sql & "                BBUC.milepost = bi2.milepost AND  "
            Sql = Sql & "                rmi.division_code = rd.division_code AND  "
            'Sql = Sql & "                BBUC.MILEPOST IN "
            'Sql = Sql & "                (" & strFilter & ") AND "
            Sql = Sql & "                BBUC.OVERHEAD_OR_MAINLINE_BRIDGE = 2 "
            Sql = Sql & "                 "
            Sql = Sql & "        )BI "
            Sql = Sql & "    )PI "
            Sql = Sql & "WHERE  "
            Sql = Sql & "VBU.MILEPOST = PI.MILEPOST  "
            Sql = Sql & "ORDER BY "

            For Each Str As String In sortArgs
                listSortStr.Add(Str)

            Next
            Sql = Sql & strSortStr


        Catch ex As Exception
            strErrMsg = "Exception while acquiring data:  " + ex.Source + ex.Message

            Utilities.WriteError(ex, strErrMsg)
            'Exit Function

        End Try

        Return Sql

    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strBT"></param>
    ''' <param name="strDivision"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFirstRecSql(ByVal strBT As String, ByVal strDivision As String) As String


        Dim SQL As String

        Try

            SQL = " select DISTINCT  "
            SQL = SQL & "TO_CHAR(VBU.MILEPOST,'FM9999999.90') AS MILEPOST, "
            SQL = SQL & "to_char(VBU.UC_READ_DATE, 'MM/DD/YYYY') as UC_READ_DATE, "
            SQL = SQL & "to_char(VBU.INSPECT_DATE, 'MM/DD/YYYY') as INSPECT_DATE, "
            SQL = SQL & "VBU.OVERHEAD_OR_MAINLINE_BRIDGE, "
            SQL = SQL & "PI.PAGE_INDEX, VBU.DIVISION_DESC, CASE WHEN VBU.MEASUREMENT_DESCRIPTION IS NULL THEN 'BLANK' "
            SQL = SQL & "ELSE VBU.MEASUREMENT_DESCRIPTION END AS MEASUREMENT_DESCRIPTION "
            SQL = SQL & "FROM "
            SQL = SQL & "IIIS.V_IIIS_BRIDGE_UNDERCLEARANCE VBU, IIIS.V_IIIS_BRIDGE_INVENTORY VBI, "
            SQL = SQL & "( "
            SQL = SQL & "    SELECT ROW_NUMBER() OVER (ORDER BY MILEPOST) AS PAGE_INDEX,MILEPOST, UC_READ_DATE "
            SQL = SQL & "    FROM  "
            SQL = SQL & "        ( "
            SQL = SQL & "       SELECT DISTINCT BUC.MILEPOST,BUC.UC_READ_DATE, BUC.DIVISION_DESC              "
            SQL = SQL & "        FROM IIIS.V_IIIS_BRIDGE_UNDERCLEARANCE BUC "
            SQL = SQL & "        WHERE BUC.DIVISION_DESC LIKE'" & strDivision & "' AND "
            SQL = SQL & "        BUC.OVERHEAD_OR_MAINLINE_BRIDGE = " & strBT
            SQL = SQL & "        ORDER BY BUC.MILEPOST, BUC.UC_READ_DATE          "
            SQL = SQL & "        ) "
            SQL = SQL & ")PI "
            SQL = SQL & " "
            SQL = SQL & "WHERE VBU.MILEPOST = VBI.MILEPOST AND  "
            SQL = SQL & "VBU.MILEPOST = PI.MILEPOST AND  "
            SQL = SQL & "VBU.UC_READ_DATE = PI.UC_READ_DATE AND "
            SQL = SQL & "PI.PAGE_INDEX = 1 AND "
            SQL = SQL & "VBU.OVERHEAD_OR_MAINLINE_BRIDGE = " & strBT & " "
            SQL = SQL & "ORDER BY PAGE_INDEX "

            'strCx = DBFunctionality.CxStringBuilder(DS, UID, PW)


            'Try
            'If DBFunctionality.GetDataTable(SQL, strCx) IsNot Nothing Then
            '    GetFirstRec = DBFunctionality.GetDataTable(SQL, strCx)

            'Else
            '    Throw New Exception("Error fetching first underclearance record")
            'End If
            Return SQL

        Catch ex As Exception
            Utilities.WriteError(ex, "GetFirstRec")
            'ClsLogUtility.LogException(ex, UID, "GetFirstRec")

        End Try

    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strMP"></param>
    ''' <param name="strDivision"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetMPRangeString(ByVal strMP As String, Optional strDivision As String = "%") As String

        'Dim objBrdgDBMgr As ClsBridgeDBMgr = New ClsBridgeDBMgr
        Dim lMPRange As List(Of Double) = New List(Of Double)
        Dim lstRangeStrings As List(Of String) = New List(Of String)
        Dim strFilter As String = String.Empty
        ' this currently gets list of MP's for the bridge type; changing to get all
        Try
            'Dim lMPList As List(Of Double) = objBrdgDBMgr.GetMPList()
            Dim lMPList As List(Of Double) = GetMPListForDivision(strDivision)
            Dim intMPIndex As Int32 = lMPList.FindIndex(Function(ind As Double)
                                                            Return ind = Math.Round(Convert.ToDouble(strMP), 2)
                                                        End Function)

            Dim intPost As Int32 = If(lMPList.Count - intMPIndex < 5, lMPList.Count, intMPIndex + 5)
            Dim intPre As Int32 = If(intPost < 10, 0, intPost - 9)

            Dim i As Int32 = intPre

            strFilter &= Convert.ToString(lMPList(i))
            Do Until i + 1 = intPost
                i += 1
                ' get range of mileposts with indices
                'lMPRange.Add(lMPList(i))
                'i += 1
                strFilter &= ", " & Convert.ToString(lMPList(i))

            Loop


            'Do Until i = intPost
            '    ' get range of mileposts with indices
            '    i += 1
            '    strFilter &= ", " & Convert.ToString(lMPList(i))

            'Loop


            Return strFilter
        Catch ex As Exception
            Utilities.WriteError(ex, ex.Message)

        End Try

    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strDivision"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetMPListForDivision(ByVal strDivision As String) As List(Of Double)

        Dim lMPIndex As List(Of Double) = New List(Of Double)
        Dim strSql As String = "select distinct vbu.milepost "
        strSql &= "FROM IIIS.V_IIIS_BRIDGE_UNDERCLEARANCE VBU "
        'strSql &= "IIIS.IIIS_WEB_UNDRCLR BBUC "
        'strSql &= "WHERE VBU.Milepost = BBUC.MilePost AND "
        'strSql &= "VBU.ID = BBUC.ID AND "
        strSql &= " WHERE VBU.DIVISION_DESC LIKE '" & strDivision & "' "

        strSql &= "order by vbu.milepost"

        Dim strCx As String = DBFunctionality.CxStringBuilder(DS, UID, PW)
        Dim dt As DataTable = DBFunctionality.GetDataTable(strSql, strCx)
        'Dim i As Integer = 0
        For Each dr As DataRow In dt.Rows
            lMPIndex.Add(Math.Round(Convert.ToDouble(dr(0)), 2))
            'lMPIndex(i) =  '"No default member found for type 'Double'."
            'i += 1
        Next
        Return lMPIndex

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strMP"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDivisionForMP(ByVal strMP As String) As String

        Dim strTbl As String = "IIISWEB.V_IIIS_BRIDGE_UNDERCLEARANCE"
        Dim lstFields As New List(Of String)
        lstFields.Add("DIVISION_DESC")
        Dim strQuals As String = "MILEPOST = " & strMP
        Dim lstSort As New List(Of String)
        lstSort.Add("UC_READ_DATE")

        Try
            Dim strDivSql As String = DBFunctionality.SQLStringBuilder(strTbl, lstFields, strQuals, lstSort)
            GetDivisionForMP = DBFunctionality.GetScalar(strDivSql)

        Catch ex As Exception
            Utilities.WriteError(ex, ex.Message)
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDDLMilepostSql(ByVal strDiv As String) As String


        Dim cmdstr As String
        Dim strErrMsg As String = String.Empty
        Try

            'cmdstr = " select DISTINCT TO_CHAR(VBU.MILEPOST,'FM9999999.90') AS MILEPOST, TO_CHAR(VBU.Milepost) as mp_desc,"
            cmdstr = " select DISTINCT VBU.MILEPOST, TO_CHAR(VBU.Milepost) as mp_desc,"
            cmdstr &= "PI.PAGE_INDEX  "
            cmdstr &= "FROM IIISWEB.V_IIIS_BRIDGE_UNDERCLEARANCE VBU, "
            'cmdstr &= "IIIS.IIIS_WEB_UNDRCLR BBUC, "
            cmdstr &= "(SELECT ROW_NUMBER() OVER (ORDER BY MILEPOST) AS PAGE_INDEX,"
            cmdstr &= "MILEPOST FROM (SELECT DISTINCT MILEPOST FROM "
            cmdstr &= "IIISWEB.V_IIIS_BRIDGE_UNDERCLEARANCE WHERE DIVISION_DESC LIKE '" & strDiv & "'))PI "
            'cmdstr &= "WHERE VBU.Milepost = BBUC.MilePost AND VBU.ID = BBUC.ID "
            cmdstr &= "WHERE VBU.MILEPOST = PI.MILEPOST "
            cmdstr &= "ORDER BY VBU.MILEPOST"



        Catch ex As Exception
            strErrMsg = "Exception while acquiring data:  " + ex.Source + ex.Message

            Utilities.WriteError(ex, strErrMsg)
            Exit Function

        End Try
        Return cmdstr

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strMP"></param>
    ''' <param name="strBrType"></param>
    ''' <param name="strFeature"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUCDateDDLSql(ByVal strMP As String, ByVal strBrType As String, ByVal strFeature As String) As String

        Dim strSql As String
        Dim strErrMsg As String = String.Empty
        Try

            strSql = " select distinct TO_DATE('12/31/9999','MM/DD/YYYY') as IDI, 'select' as UC_READ_DATE from dual  "
            strSql = strSql & "union all "
            strSql = strSql & "SELECT DISTINCT VBU.UC_READ_DATE AS IDI, to_char(VBU.UC_READ_DATE,'mm/dd/yyyy') as UC_READ_DATE FROM "
            strSql = strSql & "IIISWEB.V_IIIS_BRIDGE_UNDERCLEARANCE VBU "
            'strSql = strSql & "IIIS.IIIS_WEB_UNDRCLR BBUC "
            'strSql = strSql & "WHERE VBU.MILEPOST = BBUC.MILEPOST AND VBU.UC_READ_DATE = BBUC.UC_READ_DATE AND "
            ' strSql = strSql & "VBU.ID = BBUC.ID AND "
            strSql = strSql & " WHERE VBU.MILEPOST = " & strMP & " AND "
            strSql = strSql & "VBU.OVERHEAD_OR_MAINLINE_BRIDGE= " & strBrType & " AND "
            strSql = strSql & "(VBU.MEASUREMENT_DESCRIPTION LIKE '" & strFeature & "' OR VBU.MEASUREMENT_DESCRIPTION IS NULL) "
            strSql = strSql & "ORDER BY IDI DESC "
        Catch ex As Exception
            strErrMsg = "Exception while acquiring data:  " + ex.Source + ex.Message

            Utilities.WriteError(ex, strErrMsg)
            Exit Function

        End Try

        Return strSql

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDDLFeatureSql(ByVal strMP As String, ByVal strBT As String) As String


        Dim cmdstr As String
        Dim strErrMsg As String = String.Empty
        Try
            cmdstr = "select '%' AS FEATURES_KEY, ' (ALL) ' AS FEATURES_CROSSED FROM DUAL "
            cmdstr &= "UNION ALL "
            cmdstr &= "SELECT DISTINCT CASE WHEN MEASUREMENT_DESCRIPTION IS NULL THEN 'BLANK' "
            cmdstr &= "ELSE MEASUREMENT_DESCRIPTION END AS FEATURES_KEY, "
            cmdstr &= "CASE WHEN MEASUREMENT_DESCRIPTION IS NULL THEN 'BLANK' "
            cmdstr &= "ELSE MEASUREMENT_DESCRIPTION END AS FEATURES_CROSSED "
            cmdstr &= "FROM IIISWEB.V_IIIS_BRIDGE_UNDERCLEARANCE "
            cmdstr &= "WHERE milepost = " & strMP & " AND OVERHEAD_OR_MAINLINE_BRIDGE = " & strBT
            cmdstr &= " ORDER BY FEATURES_CROSSED ASC "
        Catch ex As Exception
            strErrMsg = "Exception while acquiring data:  " + ex.Source + ex.Message
            Utilities.WriteError(ex, strErrMsg)
            Exit Function

        End Try
        Return cmdstr


    End Function


End Class

