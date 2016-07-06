Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports System.Text
Imports System.Configuration
Imports System.Data
Imports System.Collections.Specialized


Public MustInherit Class DBFunctionality : Implements IDisposable
    
    Public Sub New()

    End Sub
    
    Public Property FieldList As StringDictionary
        Get
            Return _fieldList
        End Get
        Set(value As StringDictionary)
            _fieldList = value
        End Set
    End Property
    Private Property _fieldList As StringDictionary

     Private Property _cn As OracleConnection
    Public Property CN(ByVal strDSrc As String, ByVal strUId As String, strPwd As String) As OracleConnection
        Get
            Return CxBuilder(strDSrc, strUId, strPwd)
        End Get
        Set(value As OracleConnection)
            _cn = value
        End Set
    End Property
    Private Property _sql As String
    Public Property SQL() As String
        Get
            Return _sql
        End Get
        Set(value As String)
            _sql = value
        End Set
    End Property
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="View"></param>
    ''' <param name="Year"></param>
    ''' <param name="SortDir"></param>
    ''' <remarks></remarks>
    Public Sub CallDB(ByVal View As String, ByVal Year As String, ByVal SortDir As String)

        Dim dSet As New DataSet()
        Dim lstSortItems As List(Of String) = New List(Of String)()
        Dim strErrMsg, strQual As String
        'Getting fields for selected view from app.config
        Dim fieldList As New List(Of String)(ConfigurationManager.AppSettings(View).ToString().Split(","c))
        Dim DSConfig, UIDConfig, PWConfig, DS, UID, PW As String
        DSConfig = "pvtDS"
        UIDConfig = "PvtUID"
        PWConfig = "PvtPW"
        DS = ConfigurationManager.AppSettings(DSConfig)
        UID = ConfigurationManager.AppSettings(UIDConfig)
        PW = ConfigurationManager.AppSettings(PWConfig)

        ' default sort is MilePost Begin, ascending
        lstSortItems.Add("MP_BEGIN" + " " + SortDir)
        If Year.Length > 0 Then
            strQual = "Year=" + Year '+ " AND ATTRIBUTE_DATE IS NOT NULL"
        Else : strQual = "" '"ATTRIBUTE_DATE IS NOT NULL"
        End If
        ' Using sqlstringbuilder function to build query/cmd string from app.config and user-defined parameters
        Dim cmdstr As String = NullHandlerSQLStringBuilder(View, fieldList, strQual, lstSortItems).ToString()
        Dim cmd As OracleCommand = New OracleCommand(cmdstr, Me.CN(DS, UID, PW)) 'need to dim here but then 
        Dim da As OracleDataAdapter = New OracleDataAdapter(cmd) 'need to dim here but then 
        Dim dSchema As System.Data.SchemaType = SchemaType.Source


        ' This setting prevents .Net from incorrectly converting Oracle number type to .Net decimal type
        da.ReturnProviderSpecificTypes = True

        dSet.Tables.Add("dtView")
        _dt = dSet.Tables("dtView")
        da.FillSchema(dSet, dSchema, "dtView")
       

        Try
            Me.CN(DS, UID, PW).Open()


            'Filling data table which will be available (as property of this object) to Excel functionality
            da.Fill(_dt)
            
        Catch adoEx As DataException

            strErrMsg = "Data exception while acquiring data:  " & adoEx.Source & adoEx.Message

            Utilities.WriteError(adoEx, strErrMsg)
            Exit Sub
        Catch orEx As OracleException
            strErrMsg = "Oracle exception while acquiring data:  " & orEx.Source & orEx.Message

            Utilities.WriteError(orEx, strErrMsg)
            Exit Sub
        Catch ex As Exception
            strErrMsg = "Exception while acquiring data:  " & ex.Source & ex.Message

            Utilities.WriteError(ex, strErrMsg)
            Exit Sub
        Finally
            'CN.Dispose()
            da.Dispose()
        End Try
    End Sub

    Private Const delim As String = ", "
    Public Property DT() As DataTable
        Get
            Return _dt
        End Get
        Set(value As DataTable)
            _dt = value
        End Set
    End Property

    Private Property _dt As DataTable
    Public Property DTYears() As DataTable
        Get
            Return _dtYrs
        End Get
        Set(value As DataTable)
            _dtYrs = value
        End Set
    End Property

    Private Property _dtYrs As DataTable


    Public Property DR() As IDataReader
        Get
            Return _dr
        End Get
        Set(value As IDataReader)
            _dr = value
        End Set
    End Property
    Private Property _dr As OracleDataReader
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strDSrc"></param>
    ''' <param name="strUId"></param>
    ''' <param name="strPwd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CxStringBuilder(ByVal strDSrc As String, ByVal strUId As String, strPwd As String) As String
        Dim sbCxStringBldr As StringBuilder = New StringBuilder(String.Empty)
        sbCxStringBldr.Append("Data Source=" + strDSrc + ";")
        sbCxStringBldr.Append("User Id=" + strUId + ";")
        sbCxStringBldr.Append("Password=" + strPwd + ";")
        Return sbCxStringBldr.ToString()
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strDSrc"></param>
    ''' <param name="strUId"></param>
    ''' <param name="strPwd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CxBuilder(ByVal strDSrc As String, ByVal strUId As String, strPwd As String) As OracleConnection

        'Using stringbuilder function to get connection string
        Dim strCx As String = CxStringBuilder(strDSrc, strUId, strPwd)
        Dim cn As OracleConnection = New OracleConnection(strCx)
        Return cn

    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="tblName"></param>
    ''' <param name="items"></param>
    ''' <param name="qualifiers"></param>
    ''' <param name="sortItems"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function NullHandlerSQLStringBuilder(ByVal tblName As String, ByVal items As List(Of String),
                                        ByVal qualifiers As String, Optional ByVal sortItems As List(Of String) = Nothing) As String
        'Dim strSQL As String

        Dim sbSqlStrBldr As StringBuilder = New StringBuilder(String.Empty)
        Dim vals As New Text.StringBuilder
        Dim sorts As New Text.StringBuilder("")

        If qualifiers IsNot Nothing AndAlso qualifiers.Length > 0 AndAlso
            Not qualifiers.Trim.ToUpper.StartsWith("WHERE") Then qualifiers = "WHERE " & qualifiers


        If items Is Nothing OrElse items.Count = 0 Then
            vals.Append("*")
        Else
            For Each item As String In items
                vals.AppendFormat("{0}{1}", item, delim)
            Next
            vals.Length = vals.Length - delim.Length  'removes last delimiter from string 
        End If

        If Not sortItems Is Nothing AndAlso sortItems.Count > 0 Then
            sorts.Append("ORDER BY ")
            For Each item As String In sortItems
                sorts.AppendFormat("{0}{1}", item, delim)
            Next
            sorts.Length = sorts.Length - delim.Length  'removes last delimiter from string 
        End If
        Return String.Format("SELECT {0} FROM {1} {2} {3}", vals.ToString, tblName, qualifiers, sorts).Trim

    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="tblName"></param>
    ''' <param name="items"></param>
    ''' <param name="qualifiers"></param>
    ''' <param name="sortItems"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function SQLStringBuilder(ByVal tblName As String, ByVal items As List(Of String),
                                        ByVal qualifiers As String, ByVal sortItems As List(Of String)) As String


        Dim sbSqlStrBldr As StringBuilder = New StringBuilder(String.Empty)
        Dim vals As New Text.StringBuilder
        Dim sorts As New Text.StringBuilder("")

        If qualifiers IsNot Nothing AndAlso qualifiers.Length > 0 AndAlso
            Not qualifiers.Trim.ToUpper.StartsWith("WHERE") Then qualifiers = "WHERE " & qualifiers


        If items Is Nothing OrElse items.Count = 0 Then
            vals.Append("*")
        Else
            For Each item As String In items
                vals.AppendFormat("{0}{1}", item, delim)
            Next
            vals.Length = vals.Length - delim.Length  'removes last delimiter from string 
        End If

        If Not sortItems Is Nothing AndAlso sortItems.Count > 0 Then
            sorts.Append("ORDER BY ")
            For Each item As String In sortItems
                sorts.AppendFormat("{0}{1}", item, delim)
            Next
            sorts.Length = sorts.Length - delim.Length  'removes last delimiter from string 
        End If
        Return String.Format("SELECT {0} FROM {1} {2} {3}", vals.ToString, tblName, qualifiers, sorts).Trim


        'Return strSQL
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="cxBuilder"></param>
    ''' <param name="cmd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function fnDataReader(ByVal cxBuilder As OracleConnection, ByVal cmd As OracleCommand) As OracleDataReader

        cxBuilder.Open()
        Dim dr As OracleDataReader = cmd.ExecuteReader
        cxBuilder.Close()
        Return dr

    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="View"></param>
    ''' <param name="filter"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function fnFilteredDataTable(ByVal View As String, ByVal filter As String) As DataTable
        Dim strErrmsg, DS, UID, PW, DSConfig, UIDConfig, PWConfig As String
        Dim dvdt As DataTable = New DataTable
        Dim dv As DataView
        DSConfig = "pvtDS"
        UIDConfig = "PvtUID"
        PWConfig = "PvtPW"
        DS = ConfigurationManager.AppSettings(DSConfig)
        UID = ConfigurationManager.AppSettings(UIDConfig)
        PW = ConfigurationManager.AppSettings(PWConfig)

        dv = New DataView(fnDataTable(View, DS, UID, PW))
        Try

            dvdt = dv.ToTable(True, filter)

        Catch ex As Exception

            strErrmsg = "Error while creating internal data table"
            Utilities.WriteError(ex, strErrmsg)

        End Try
        Return dvdt

    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="View"></param>
    ''' <param name="DS"></param>
    ''' <param name="UID"></param>
    ''' <param name="PW"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function fnDataTable(ByVal View As String, ByVal DS As String, ByVal UID As String, ByVal PW As String) As DataTable
        Dim strErrmsg As String
        Dim dSet As New DataSet()
        Dim dtYrs As New DataTable()
        Dim yearfieldlist As List(Of String) = New List(Of String)()
        Dim fieldList As New List(Of String)(ConfigurationManager.AppSettings(View).ToString().Split(","c))
        Dim lstSortItems As List(Of String) = New List(Of String)()

        'Using stringbuilder function to get connection string
        'Using connectionbuilder function to get db connection
        Dim cn As OracleConnection = CxBuilder(DS, UID, PW)
        'Getting fields for selected view from app.config
        yearfieldlist.Add("'" + View + "'" + " as ViewName")
        yearfieldlist.Add("YEAR")
        Dim cmdStrYrs As String = SQLStringBuilder(View, yearfieldlist, "", lstSortItems)
        Dim cmd As OracleCommand = New OracleCommand(cmdStrYrs, cn)
        Dim da As OracleDataAdapter = New OracleDataAdapter(cmd)
        ' This setting prevents .Net from incorrectly converting Oracle number type to .Net decimal type
        da.ReturnProviderSpecificTypes = True
        da.SafeMapping().Add("YEAR", GetType(String))
        dSet.Tables.Add("dtYears")
        dtYrs = dSet.Tables("dtYears")

        Try
            cn.Open()
            'Filling data table which will be available (as property of this object) to Excel functionality
            da.Fill(dtYrs)
        Catch adoEx As DataException
            strErrmsg = "Data error while creating internal data table"
            Utilities.WriteError(adoEx, strErrmsg)

        Catch orEx As OracleException

            strErrmsg = "Oracle error while creating internal data table"
            Utilities.WriteError(orEx, strErrmsg)

        Catch ex As Exception
            strErrmsg = "Error while creating internal data table"
            Utilities.WriteError(ex, strErrmsg)

        Finally

            cn.Dispose()
            da.Dispose()
        End Try
        Return dtYrs

    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sql"></param>
    ''' <param name="cxStr"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetDataTable(ByVal sql As String, ByVal cxStr As String) As DataTable
        Dim dt As New DataTable

        GetData(dt, sql, cxStr)
        Return dt
    End Function
    Public Shared Function GetDataTableWithDateFormats(ByVal sql As String, ByVal cxStr As String) As DataTable
        Dim dt As New DataTable

        GetData(dt, sql, cxStr)
        Return dt
    End Function

    Public Shared Function GetScalar(ByVal sql As String) As Object

        Dim DSConfig, UIDConfig, PWConfig, DS, UID, PW As String
        DSConfig = "bucDS"
        UIDConfig = "bucUID"
        PWConfig = "bucPW"
        DS = ConfigurationManager.AppSettings(DSConfig)
        UID = ConfigurationManager.AppSettings(UIDConfig)
        PW = ConfigurationManager.AppSettings(PWConfig)

        Dim cxStr As String = CxStringBuilder(DS, UID, PW)

        Using conn As OracleConnection = New OracleConnection(cxStr),
            cmd As New OracleCommand(sql, conn)
            Try
                conn.Open()
                GetScalar = cmd.ExecuteScalar

            Catch ex As Exception
                Utilities.WriteError(ex, ex.Message)
            End Try

        End Using
        


    End Function


    'generic method for all SQL query methods
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="dobj"></param>
    ''' <param name="sql"></param>
    ''' <param name="cxStr"></param>
    ''' <remarks></remarks>
    Private Shared Sub GetData(ByRef dobj As Object, ByVal sql As String, ByVal cxStr As String)
        Dim strErrmsg As String = String.Empty
        Using conn As OracleConnection = New OracleConnection(cxStr),
            da As New OracleDataAdapter(sql, conn)
            '*** https://www.google.com/?gws_rd=ssl#safe=active&q=ORA-00911%3A+invalid+character+.net+oracle+dataadapter ***
            Try
                da.Fill(dobj)
            Catch ex As Exception

                strErrmsg = "Oracle error while creating internal data table"
                Utilities.WriteError(ex, strErrmsg)

            Finally
                conn.Close()
            End Try
        End Using
    End Sub
   
    ' Flag: Has Dispose already been called? 
    Dim disposed As Boolean = False
    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub
    Public Sub Dispose() _
           Implements IDisposable.Dispose
        ' Public implementation of Dispose pattern callable by consumers. 
        ' Dispose of unmanaged resources.
        Dispose(True)
        ' Suppress finalization.
        GC.SuppressFinalize(Me)
    End Sub
    ' Protected implementation of Dispose pattern. 
    Protected Overridable Sub Dispose(disposing As Boolean)
        If disposed Then Return
        ' Protected implementation of Dispose pattern. 
        If disposing Then
            ' Free any other managed objects here. 
            ' 
        End If

        ' Free any unmanaged objects here. 
        ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
        ' TODO: set large fields to null.
        '
        disposed = True
    End Sub
    ''' <summary>Creates Oracle Update SQL string based on input values.</summary>
    ''' <param name="tblName">REQUIRED. Name of the table to Update.</param>
    ''' <param name="items">REQUIRED. Generic list of key-value pairs identifying columns and values to update.  
    ''' Can include parameters and functional manipulations (e.g., Upper()) as necessary.</param>
    ''' <param name="qualifiers">String that acts as the qualifier for the UPDATE statement, or Nothing if no qualifiers needed.  
    ''' Automatically adds the WHERE keyword if not included.</param>
    ''' <returns>Oracle Update SQL string.</returns>
    Public Function GetUpdateSQL(ByVal tblName As String, ByVal items As List(Of KeyValuePair(Of String, String)), ByVal qualifiers As String) As String
        If qualifiers IsNot Nothing AndAlso qualifiers.Length > 0 AndAlso
            Not qualifiers.Trim.ToUpper.StartsWith("WHERE") Then qualifiers = "WHERE " & qualifiers
        Dim vals As New Text.StringBuilder
        For i = 0 To items.Count - 1
            vals.AppendFormat("{0} = {1}{2}", items(i).Key, items(i).Value, delim)
        Next
        vals.Length = vals.Length - delim.Length  'removes last delimiter from string 
        Return String.Format("UPDATE {0} SET {1} {2}", tblName, vals.ToString, qualifiers).Trim
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateIIISWebCommandObject() As Oracle.DataAccess.Client.OracleCommand
        '***********************************************************************************
        '* FUNCTION:    CreateCPMSCommandObject
        '* PARAMETERS:  (none)
        '* RETURNS:     Oracle Command Object
        '*
        '* DESCRIPTION: This function creates a command object from the global connection
        '*              object.  All commands from the same connection object use the same
        '*              transaction as well as the connection.
        '***********************************************************************************

        'command object to return
        Dim retCmd As Oracle.DataAccess.Client.OracleCommand

        'assign the command object
        retCmd = gsOracleConnection.CreateCommand()

        Return retCmd

    End Function
    Public gsOracleConnection As Oracle.DataAccess.Client.OracleConnection = New Oracle.DataAccess.Client.OracleConnection
    Public gsOracleTransaction As Oracle.DataAccess.Client.OracleTransaction
End Class

