Imports System
Imports System.Configuration
Imports System.Data
Imports Oracle.DataAccess.Client
Imports System.Collections

''' <summary>
''' A helper class used to build and execute SQL against an Oracle database. REQUIRED: set the ConnectString property before executing SQL.
''' </summary>
Public MustInherit Class OracleHelper  'TODO: MustInherit doesn't seem to be working

    ''' <summary>
    ''' Represents available sort types. Use string values, for example, to specify sort direction in SQL.
    ''' </summary>
    Public Enum SortDirection
        ASC
        DESC
    End Enum

    ''' <summary>
    ''' Represents available data types for Oracle interaction. Use, for example, in specifying parameter types.
    ''' </summary>
    Public Enum DBType  'TODO: actually, only ones currently in use are included
        [Char] = OracleDbType.Char
        [Date] = OracleDbType.Date
        [Decimal] = OracleDbType.Decimal
        [Double] = OracleDbType.Double
        [Short] = OracleDbType.Int16
        [Int32] = OracleDbType.Int32
        [Long] = OracleDbType.Long
        [Single] = OracleDbType.Single
        VarChar2 = OracleDbType.Varchar2
    End Enum

    Public Enum DBTypeXWalk  'TODO: actually, only ones currently in use are included
        [Char] = OracleDbType.Char
        [Date] = OracleDbType.Date
        [DateTime] = OracleDbType.Date
        [Decimal] = OracleDbType.Decimal
        [Double] = OracleDbType.Double
        [Int16] = OracleDbType.Int32
        [Int32] = OracleDbType.Int32
        [Int64] = OracleDbType.Long
        [Integer] = OracleDbType.Int32
        [Long] = OracleDbType.Long
        [Single] = OracleDbType.Single
        [String] = OracleDbType.Varchar2
    End Enum


#Region "    *****    SQL Helpers    *****    "

    Private Const delim As String = ", "
    Private Const shortDatePattern As String = "MM/dd/yyyy" 'not quite same as Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern
    Private Const shortDateTimePattern As String = "MM/dd/yyyy hh:mm tt"
    Private Const shortDateTimePatternOracle As String = "MM/DD/YYYY HH:MI AM"
    Private Const NLS_DATE_FORMAT As String = "dd-MMM-yyyy"  'default format of dates from Oracle


    ' ''' <summary>Users of this class MUST set the connect string before executing SQL.</summary>
    'Public WriteOnly Property ConnectString As String  'TODO: maybe the Shared is overriding the MustInherit? Make all apps create their own class to set the DB conection, and inherit this for all the functionality. Can it be done as module, so no need to instantiate classes?
    '    Set(value As String)
    '        mConnectionString = value
    '    End Set
    'End Property

    ''' <summary>Provides a parameter object for use in executing SQL with parameters. Wrapper saves caller having to reference Oracle.DataAccess</summary>
    Public Structure Param
        Friend OracleParam As OracleParameter  'local property holds Oracle.DataAccess version of parameter

        ''' <summary>Creates a new input parameter for use in execution of SQL with this library.</summary>
        ''' <param name="name">Name of the parameter.</param>
        ''' <param name="value">Value for the parameter.</param>
        ''' <returns>Creates and loads new parameter instance.</returns>
        Public Function LoadSingleton(name As String, value As Object, Optional direction As ParameterDirection = ParameterDirection.Input) As Param  'for nullable values, skip type to avoid conversion errors
            OracleParam = New OracleParameter(name, value) With {.Direction = direction}
            Return (Me)
        End Function

        ''' <summary>Creates a new input parameter for use in execution of SQL with this library.</summary>
        ''' <param name="name">Name of the parameter.</param>
        ''' <param name="type">Type (from enum) of the parameter.</param>
        ''' <param name="values">Array of values for the parameter.</param>
        ''' <returns>Creates and loads new parameter instance, defaulting to input direction.</returns>
        Public Function Load(Of T)(name As String, type As DBType, values() As T) As Param  'default to input param
            Return Load(Of T)(name, type, values, ParameterDirection.Input)
        End Function
        ''' <summary>Creates a new parameter for use in execution of SQL with this library.</summary>
        ''' <param name="name">Name of the parameter.</param>
        ''' <param name="type">Type (from enum) of the parameter.</param>
        ''' <param name="values">Array of values for the parameter.</param>
        ''' <param name="direction">Indicator for parameter direction (input/output)</param>
        ''' <returns>Creates and loads new parameter instance, defaulting to input direction.</returns>
        Public Function Load(Of T)(name As String, type As DBType, values() As T, direction As ParameterDirection) As Param
            OracleParam = New OracleParameter(name, type, values, direction)
            Return (Me)
        End Function

        ''' <summary>Gets value of parameter object (for output parameters)</summary>
        ''' <returns>Value of parameter</returns>
        Public Function GetValue() As Object
            Return OracleParam.Value
        End Function

    End Structure

    ''' <summary>Creates a new key-value pair with the indicated key and value.</summary>
    ''' <param name="key">A string with the key name.</param>
    ''' <param name="val">A string with the value.</param>
    ''' <returns>Input items combined into new key-value pair.</returns>
    Public Function GetKVP(ByVal key As String, ByVal val As String) As KeyValuePair(Of String, String)
        Return New KeyValuePair(Of String, String)(key, val)
    End Function

    ''' <summary>Formats the input value as a string for inclusion in a SQL string.  Returns NULL for DBNull and non-numeric values.</summary>
    ''' <param name="val">A numeric object to be converted to a SQL string value.</param>
    ''' <returns>Input value as string, or NULL if input is DBNull or non-numeric.</returns>
    Public Function GetNumberSQL(val As Object) As String
        If IsDBNull(val) OrElse Not IsNumeric(val) Then
            'Return "NULL"
            Return ""
        Else
            Return val
        End If
    End Function

    ''' <summary>Formats the input value as a string for inclusion in a SQL string, wrapping in single quotes and escaping existing single quotes.  
    ''' Returns NULL for DBNull and empty strings.</summary>
    ''' <param name="val">An object to be converted to a SQL string value.</param>
    ''' <returns>Input string enclosed in single quotes, with existing single quotes escaped, or NULL if input is DBNull or empty string.</returns>
    Public Function GetStringSQL(ByVal val As Object) As String  'TODO: need a version that allows an empty string?
        If IsDBNull(val) OrElse val.ToString.Length = 0 Then
            'Return "NULL"
            Return ""
        Else
            Return String.Format("'{0}'", val.ToString.Replace("'", "''"))
        End If
    End Function

    ''' <summary>Creates Oracle Select SQL string based on input values.</summary>
    ''' <param name="tblName">REQUIRED. Name of the table to Select from.  Can be a full statement including JOINs if necessary.</param>
    ''' <param name="items">Generic list of string values identifying columns to Select.  Can include literals (properly escaped), 'As' clauses for naming, 
    ''' and other functional manipulations (e.g., Upper()) as necessary.  If nothing, defaults to wildcard (*).</param>
    ''' <param name="qualifiers">String that acts as the qualifier for the SELECT statement, or Nothing if no qualifiers needed.  
    ''' Automatically adds the WHERE keyword if not included.</param>
    ''' <param name="sortItems">Generic list of string values to use in an Order By statment, or Nothing if no sort needed.</param>
    ''' <returns>Oracle Select SQL string.</returns>
    Public Function GetSelectSQL(ByVal tblName As String, ByVal items As List(Of String),
                                        ByVal qualifiers As String, ByVal sortItems As List(Of String)) As String
        If qualifiers IsNot Nothing AndAlso qualifiers.Length > 0 AndAlso
            Not qualifiers.Trim.ToUpper.StartsWith("WHERE") Then qualifiers = "WHERE " & qualifiers
        Dim vals As New Text.StringBuilder
        If items Is Nothing OrElse items.Count = 0 Then
            vals.Append("*")
        Else
            For Each item As String In items
                vals.AppendFormat("{0}{1}", item, delim)
            Next
            vals.Length = vals.Length - delim.Length  'removes last delimiter from string 
        End If
        Dim sorts As New Text.StringBuilder("")
        If Not sortItems Is Nothing AndAlso sortItems.Count > 0 Then
            sorts.Append("ORDER BY ")
            For Each item As String In sortItems
                sorts.AppendFormat("{0}{1}", item, delim)
            Next
            sorts.Length = sorts.Length - delim.Length  'removes last delimiter from string 
        End If
        Return String.Format("SELECT {0} FROM {1} {2} {3}", vals.ToString, tblName, qualifiers, sorts).Trim
    End Function

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
    ''' <summary>Formats the input value as a 'To_Date()' string for inclusion in a SQL string.</summary>
    ''' <param name="dt">A date to be converted to a SQL string value.</param>
    ''' <returns>Input value as To_Date() string.</returns>
    Public Function GetDateSQL(ByVal dt As Date) As String
        Return String.Format("TO_DATE('{0}', '{1}')", dt.ToString(shortDatePattern), shortDatePattern)
    End Function

    ''' <summary>Formats the input value as a 'To_Date()' string for inclusion in a SQL string.</summary>
    ''' <param name="dt">A datetime to be converted to a SQL string value.</param>
    ''' <returns>Input value as To_Date() string.</returns>
    Public Function GetDateTimeSQL(ByVal dt As DateTime) As String
        Return String.Format("TO_DATE('{0}', '{1}')", dt.ToString(shortDateTimePattern), shortDateTimePatternOracle)
    End Function

    ''' <summary>Creates Oracle Insert SQL string based on input values.</summary>
    ''' <param name="tblName">REQUIRED. Name of the table to Insert into.</param>
    ''' <param name="items">REQUIRED. Generic list of key-value pairs identifying columns and data to insert. 
    ''' Can include parameters and functional manipulations (e.g., Upper()) as necessary.</param>
    ''' <returns>Oracle Insert SQL string.</returns>
    Public Function GetInsertSQL(ByVal tblName As String, ByVal items As List(Of KeyValuePair(Of String, String))) As String
        Dim keys As New Text.StringBuilder
        Dim vals As New Text.StringBuilder
        For i = 0 To items.Count - 1
            keys.AppendFormat("{0}{1}", items(i).Key, delim)
            vals.AppendFormat("{0}{1}", items(i).Value, delim)
        Next
        keys.Length = keys.Length - delim.Length  'removes last delimiter from string 
        vals.Length = vals.Length - delim.Length  'removes last delimiter from string 
        Return String.Format("INSERT INTO {0} ({1}) VALUES ({2})", tblName, keys.ToString, vals.ToString)
    End Function

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

    ''' <summary>Creates Oracle Mere (Insert or Update) SQL string based on input values. 
    ''' NOTE: automatically appends suffix of _I and _U to insert and update parameters, respectively.</summary>
    ''' <param name="tblName">REQUIRED. Name of the table to Merge to.</param>
    ''' <param name="links">REQUIRED. Generic list of key-value pairs identifying qualifier columns and their values.
    ''' These values are used in the SQL to determine if the row already exists in the table. 
    ''' Can include parameters and functional manipulations (e.g., Upper()) as necessary.</param>
    ''' <param name="inserts">REQUIRED. Generic list of key-value pairs identifying columns and data to insert if row does not already exist. 
    ''' Can include parameters and functional manipulations (e.g., Upper()) as necessary.</param>
    ''' <param name="updates">REQUIRED. Generic list of key-value pairs identifying columns and values to update if the row already exists. 
    ''' Can include parameters and functional manipulations (e.g., Upper()) as necessary.</param>
    ''' <returns>Oracle Merge (Insert or Update) SQL string.</returns>
    Public Function GetMergeSQL(tblName As String, links As List(Of KeyValuePair(Of String, String)),
                                       inserts As List(Of KeyValuePair(Of String, String)),
                                       updates As List(Of KeyValuePair(Of String, String))) As String
        Dim duals As New List(Of String), i As Integer
        For i = 0 To links.Count - 1
            duals.Add(links(i).Key & " = " & links(i).Value)
        Next
        Dim iKeys As New List(Of String), iVals As New List(Of String), val As String
        For i = 0 To inserts.Count - 1
            iKeys.Add(inserts(i).Key)
            val = inserts(i).Value
            If val.StartsWith(":") Then  'parameter--add suffix automatically
                iVals.Add(val & "_I")
            Else
                iVals.Add(val)
            End If
        Next
        Dim upds As New List(Of String)
        For i = 0 To updates.Count - 1
            val = updates(i).Value
            If val.StartsWith(":") Then  'parameter--add suffix automatically
                upds.Add(updates(i).Key & " = " & val & "_U")
            Else
                upds.Add(updates(i).Key & " = " & val)
            End If
        Next

        Dim sql As New Text.StringBuilder("MERGE into ")
        sql.AppendLine(tblName)
        sql.AppendFormat("using dual on ({0}) ", String.Join(" AND ", duals.ToArray)).AppendLine()
        sql.AppendLine("when not matched then ")
        sql.AppendFormat("INSERT ({0}) VALUES ({1}) ", String.Join(delim, iKeys.ToArray), String.Join(delim, iVals.ToArray)).AppendLine()
        sql.AppendLine("when matched then ")
        sql.AppendFormat("UPDATE SET {0} ", String.Join(delim, upds.ToArray))

        Return sql.ToString
    End Function

#End Region

    ''' <summary>Filter for DBNull value, so DB values can be safely used in code.</summary>
    ''' <param name="val">Object representing value from the database.</param>
    ''' <returns>Input value or Nothing, if value is DBNull.</returns>
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

    ''' <summary>converts Nothing or an empty string to DBNull for posting to DB</summary>
    ''' <param name="val">Object representing value from the database.</param>
    ''' <returns>Input value or DBNull.</returns>
    Public Function UnNVL(val As String) As Object
        If val Is Nothing OrElse val.Length = 0 Then
            Return DBNull.Value
        Else
            Return val
        End If
    End Function

    ''' <summary>converts Nothing or an empty string to DBNull for posting to DB, otherwise formats date to default DB string format</summary>
    ''' <param name="val">Object representing date value.</param>
    ''' <returns>Formatted input value or DBNull.</returns>
    Public Function UnNVLDate(val As String) As Object
        Dim tmp As Date
        If val Is Nothing OrElse val.Length = 0 OrElse Not Date.TryParse(val, tmp) Then
            Return DBNull.Value
        Else
            Return Date.Parse(val).ToString(shortDatePattern)
        End If
    End Function

#Region "    *****    DB Methods    *****    "

    Private Enum ExecuteType
        NonQuery
        Scalar
        Reader
    End Enum

    Private mConnectionString As String
    ''' <summary>Instantiates OracleHlper library.</summary>
    ''' <param name="connectString">REQUIRED: specifies the connection string.</param>
    Public Sub New(connectString As String)      'Protected Friend 
        mConnectionString = connectString
    End Sub

    ''' <summary>Executes multiple SQL statements in one session, with transaction wrapper to rollback in case of error.</summary>
    ''' <param name="sql">Array of SQL strings to execute.</param>
    ''' <returns>Array of integers representing result of ExecuteNonQuery calls for each input SQL (number of rows affected for each SQL).</returns>
    Public Function ExecuteInTransaction(sql() As String) As Integer()
        Dim result As New List(Of Integer)
        Using conn = New OracleConnection(mConnectionString)
            Try
                conn.Open()
                Using trans As Oracle.DataAccess.Client.OracleTransaction = conn.BeginTransaction,
                    objCmd = New OracleCommand With {.Transaction = trans,
                                                     .Connection = conn}
                    Try
                        For i As Integer = 0 To sql.Length - 1
                            objCmd.CommandText = sql(i)
                            result.Add(objCmd.ExecuteNonQuery())
                        Next
                        trans.Commit()
                    Catch ex As Exception
                        trans.Rollback()
                        Throw
                    Finally
                        objCmd.Connection.Close()
                    End Try
                End Using
            Catch ex As Exception
                Throw
            Finally
                conn.Close()
            End Try
        End Using
        Return result.ToArray
    End Function

    'TODO: untested
    ''' <summary>Executes multiple SQL statements in one session, with transaction wrapper to rollback in case of error.  Note that all input args must be parallel arrays.</summary>
    ''' <param name="sql">Array of SQL strings to execute.</param>
    ''' <param name="params">Jagged array of parameters to use in each SQL.  First dimension must be parallel to sql array.</param>
    ''' <param name="recordCount">Array of integers with number of records in parameter data.  Must be parallel to sql and params arrays.</param>
    ''' <param name="cmdType">Array of CommandTypes.  Must be parallel to sql array.</param>
    ''' <returns>Array of integers representing result of ExecuteNonQuery calls for each input SQL (number of rows affected for each SQL).</returns>
    Public Function ExecuteInTransaction(sql() As String, params()() As Param, recordCount() As Integer, cmdType() As CommandType) As Integer()
        Dim result As New List(Of Integer)
        Using conn = New OracleConnection(mConnectionString)
            Try
                conn.Open()
                Using trans As Oracle.DataAccess.Client.OracleTransaction = conn.BeginTransaction,
                    objCmd = New OracleCommand With {.Transaction = trans,
                                                     .Connection = conn}
                    Try
                        'sql, params, recordCount and cmdType must be parallel
                        For i As Integer = 0 To sql.Length - 1
                            objCmd.CommandType = cmdType(i)
                            objCmd.CommandText = sql(i)
                            objCmd.ArrayBindCount = recordCount(i)
                            If params IsNot Nothing AndAlso params.Length > 0 AndAlso
                                params(i) IsNot Nothing AndAlso params(i).Length > 0 Then
                                objCmd.Parameters.AddRange(params(i))
                            End If

                            result.Add(objCmd.ExecuteNonQuery())
                            objCmd.Parameters.Clear()
                        Next
                        trans.Commit()
                    Catch ex As Exception
                        trans.Rollback()
                        Throw
                    Finally
                        objCmd.Connection.Close()
                    End Try
                End Using
            Catch ex As Exception
                Throw
            Finally
                conn.Close()
            End Try
        End Using
        Return result.ToArray
    End Function

    ''' <summary>Executes SQL statement.</summary>
    ''' <param name="sql">SQL string to execute.</param>
    ''' <returns>Integer representing result of ExecuteNonQuery (number of rows affected).</returns>
    Public Function ExecuteNonQuery(ByVal sql As String) As Integer
        Return ExecuteNonQuery(sql, Nothing, 0)
    End Function
    ''' <summary>Executes SQL statement with parameters.</summary>
    ''' <param name="sql">SQL string to execute.</param>
    ''' <param name="params">Array of Param structure object representing data parameters referenced in SQL, or Nothing.</param>
    ''' <param name="recordCount">Number of data items in each parameter, or 0 if no parameters.</param>
    ''' <returns>Integer representing result of ExecuteNonQuery (number of rows affected).</returns>
    Public Function ExecuteNonQuery(ByVal sql As String, params() As Param, recordCount As Integer) As Integer
        Return ExecuteNonQuery(sql, params, recordCount, CommandType.Text)
    End Function
    ''' <summary>Executes SQL proc/statement with parameters.</summary>
    ''' <param name="sql">SQL proc/string to execute.</param>
    ''' <param name="params">Array of Param structure object representing data parameters referenced in SQL, or Nothing.</param>
    ''' <param name="recordCount">Number of data items in each parameter, or 0 if no parameters.</param>
    ''' <param name="CmdType">CommandType of SQL.</param>
    ''' <returns>Integer representing result of ExecuteNonQuery (number of rows affected).</returns>
    Public Function ExecuteNonQuery(ByVal sql As String, params() As Param, recordCount As Integer, CmdType As CommandType) As Integer
        If params IsNot Nothing Then
            Dim ps As New List(Of OracleParameter)
            For i As Integer = 0 To params.Count - 1
                ps.Add(params(i).OracleParam)
            Next
            Return Execute(Of Integer)(ExecuteType.NonQuery, sql, ps.ToArray, recordCount, CmdType)
        Else
            Return Execute(Of Integer)(ExecuteType.NonQuery, sql, Nothing, 0, CmdType)
        End If
    End Function

    ''' <summary>Executes SQL query for fetching discrete data item and returns indicated data type.</summary>
    ''' <typeparam name="T">The type of the result</typeparam>
    ''' <param name="sql">SQL query to execute.</param>
    ''' <returns>Result of query cast to indicated type.</returns>
    Public Function ExecuteScalar(Of T)(ByVal sql As String) As T
        Return Execute(Of T)(ExecuteType.Scalar, sql, Nothing, 0, CommandType.Text)
    End Function

    ''' <summary>Executes SQL query and returns data reader.</summary>
    ''' <param name="sql">SQL query to execute.</param>
    ''' <returns>Result of query as data reader.</returns>
    Public Function ExecuteReader(ByVal sql As String) As IDataReader
        Return Execute(Of IDataReader)(ExecuteType.Reader, sql, Nothing, 0, CommandType.Text)
    End Function

    'generic method for all SQL execution methods
    Private Function Execute(Of T)(ByVal method As ExecuteType, ByVal sql As String, params() As OracleParameter, recordCount As Integer, cmdType As CommandType) As T
        Using conn As OracleConnection = New OracleConnection(mConnectionString),
            cmd As New OracleCommand(sql, conn)
            Try
                cmd.CommandType = cmdType
                cmd.Connection.Open()
                If params IsNot Nothing AndAlso params.Count > 0 Then
                    If recordCount > 0 Then cmd.ArrayBindCount = recordCount
                    For i As Integer = 0 To params.Length - 1
                        cmd.Parameters.Add(params(i))
                    Next
                End If
                Select Case method
                    Case ExecuteType.NonQuery
                        Return CType(cmd.ExecuteNonQuery, Object)  'bit of a hack, but allows use of this generic method
                    Case ExecuteType.Scalar
                        Return cmd.ExecuteScalar
                    Case ExecuteType.Reader
                        Return CType(cmd.ExecuteReader, Object)  'bit of a hack, but allows use of this generic method
                End Select
            Catch ex As Exception
                Utilities.WriteError(ex, ex.Message)

            Finally
                'TODO: are params still available after disposal of the connection and command? if not, need to repackage all output params for return
                ''can't do this if there are output params
                'For p As Integer = 0 To cmd.Parameters.Count - 1
                '    cmd.Parameters(p).Dispose()
                'Next
                conn.Close()
            End Try
        End Using
    End Function

    ''' <summary>Executes SQL query and returns data set.</summary>
    ''' <param name="sql">SQL query to execute.</param>
    ''' <returns>Result of query as data set.</returns>
    Public Function GetDataSet(ByVal sql As String) As DataSet
        Dim ds As New DataSet
        GetData(ds, sql)
        Return ds
    End Function

    ''' <summary>Executes SQL query and returns data table.</summary>
    ''' <param name="sql">SQL query to execute.</param>
    ''' <returns>Result of query as data table.</returns>
    Public Function GetDataTable(ByVal sql As String) As DataTable
        Dim dt As New DataTable
        GetData(dt, sql)
        Return dt
    End Function

    ''' <summary>Executes SQL query and returns data view.</summary>
    ''' <param name="sql">SQL query to execute.</param>
    ''' <returns>Result of query as data view.</returns>
    Public Function GetDataView(ByVal sql As String) As DataView
        Return New DataView(GetDataTable(sql))
    End Function

    'generic method for all SQL query methods
    Private Sub GetData(ByRef dobj As Object, ByVal sql As String)
        Using conn As OracleConnection = New OracleConnection(mConnectionString),
            da As New OracleDataAdapter(sql, conn)
            Try
                da.Fill(dobj)
            Catch ex As Exception
                Throw
            Finally
                conn.Close()
            End Try
        End Using
    End Sub

#End Region

End Class

