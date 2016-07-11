Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization
Imports Oracle.DataAccess.Client
Imports System.Web.SessionState.HttpSessionState

#Region "Models"
Namespace Models
    '<PropertiesMustMatch("NewPassword", "ConfirmPassword", ErrorMessage:="The new password and confirmation password do not match.")> _
    'Public Class ChangePasswordModel
    '    Private oldPasswordValue As String
    '    Private newPasswordValue As String
    '    Private confirmPasswordValue As String

    '    <Required()> _
    '    <DataType(DataType.Password)> _
    '    <DisplayName("Current password")> _
    '    Public Property OldPassword() As String
    '        Get
    '            Return oldPasswordValue
    '        End Get
    '        Set(ByVal value As String)
    '            oldPasswordValue = value
    '        End Set
    '    End Property

    '    <Required()> _
    '    <ValidatePasswordLength()> _
    '    <DataType(DataType.Password)> _
    '    <DisplayName("New password")> _
    '    Public Property NewPassword() As String
    '        Get
    '            Return newPasswordValue
    '        End Get
    '        Set(ByVal value As String)
    '            newPasswordValue = value
    '        End Set
    '    End Property

    '    <Required()> _
    '    <DataType(DataType.Password)> _
    '    <DisplayName("Confirm new password")> _
    '    Public Property ConfirmPassword() As String
    '        Get
    '            Return confirmPasswordValue
    '        End Get
    '        Set(ByVal value As String)
    '            confirmPasswordValue = value
    '        End Set
    '    End Property
    'End Class

    Public Class ReadingModel

        <Required()>
        Public Property Milepost As String
            Get
                Return _milepost
            End Get
            Set(value As String)
                _milepost = value
            End Set
        End Property
        Private Property _milepost As String

        <Required()>
        Public Property INSP_ID As Integer
            Get
                Return _id
            End Get
            Set(value As Integer)
                _id = value
            End Set
        End Property
        Private Property _id As Integer


        Public Property Inspect_Date As Nullable(Of Date)
            Get
                Return _inspect_date
            End Get
            Set(value As Nullable(Of Date))
                _inspect_date = value
            End Set
        End Property
        Private Property _inspect_date As Nullable(Of Date) = Nothing


        <Required()>
        Public Property UC_Read_Date As Date
            Get
                Return _uc_read_date
            End Get
            Set(value As Date)
                _uc_read_date = value
            End Set
        End Property
        Private Property _uc_read_date As Date

        '<Required()>
        'Public Property OVERHEAD_OR_MAINLINE_BRIDGE As Char
        '    Get
        '        Return _oh_or_ml
        '    End Get
        '    Set(value As Char)
        '        _oh_or_ml = value
        '    End Set
        'End Property

        'Private Property _oh_or_ml As Char
        'Public Property H_measure As String
        '    Get
        '        Return _h_measure
        '    End Get
        '    Set(value As String)
        '        _h_measure = value
        '    End Set
        'End Property

        'Private Property _h_measure As String

        'Public Property G_measure As String
        '    Get
        '        Return _g_measure
        '    End Get
        '    Set(value As String)
        '        _g_measure = value
        '    End Set
        'End Property
        'Private Property _g_measure As String

        'Public Property F_measure As String
        '    Get
        '        Return _f_measure
        '    End Get
        '    Set(value As String)
        '        _f_measure = value
        '    End Set
        'End Property
        'Private Property _f_measure As String

        'Public Property E_measure As String
        '    Get
        '        Return _e_measure
        '    End Get
        '    Set(value As String)
        '        _e_measure = value
        '    End Set
        'End Property
        'Private Property _e_measure As String

        'Public Property D_measure As String
        '    Get
        '        Return _d_measure
        '    End Get
        '    Set(value As String)
        '        _d_measure = value
        '    End Set
        'End Property
        'Private Property _d_measure As String

        'Public Property C_measure As String
        '    Get
        '        Return _c_measure
        '    End Get
        '    Set(value As String)
        '        _c_measure = value
        '    End Set
        'End Property
        'Private Property _c_measure As String

        'Public Property B_measure As String
        '    Get
        '        Return _b_measure
        '    End Get
        '    Set(value As String)
        '        _b_measure = value
        '    End Set
        'End Property
        'Private _b_measure As String


        'Public Property A_measure As String
        '    Get
        '        Return _a_measure
        '    End Get
        '    Set(value As String)
        '        _a_measure = value
        '    End Set
        'End Property
        'Private Property _a_measure As String

        'Public Property AA_measure As String
        '    Get
        '        Return _aa_measure
        '    End Get
        '    Set(value As String)
        '        _aa_measure = value
        '    End Set
        'End Property
        'Private Property _aa_measure As String

        'Public Property BB_measure As String
        '    Get
        '        Return _bb_measure
        '    End Get
        '    Set(value As String)
        '        _bb_measure = value
        '    End Set
        'End Property
        'Private Property _bb_measure As String


        'Public Property CC_measure As String
        '    Get
        '        Return _cc_measure
        '    End Get
        '    Set(value As String)
        '        _cc_measure = value
        '    End Set
        'End Property
        'Private Property _cc_measure As String

        'Public Property DD_measure As String
        '    Get
        '        Return _dd_measure
        '    End Get
        '    Set(value As String)
        '        _dd_measure = value
        '    End Set
        'End Property
        'Private Property _dd_measure As String

        'Public Property EE_measure As String
        '    Get
        '        Return _ee_measure
        '    End Get
        '    Set(value As String)
        '        _ee_measure = value
        '    End Set
        'End Property
        'Private Property _ee_measure As String


        'Public Property FF_measure As String
        '    Get
        '        Return _ff_measure
        '    End Get
        '    Set(value As String)
        '        _ff_measure = value
        '    End Set
        'End Property
        'Private Property _ff_measure As String

        'Public Property GG_measure As String
        '    Get
        '        Return _gg_measure
        '    End Get
        '    Set(value As String)
        '        _gg_measure = value
        '    End Set
        'End Property
        'Private Property _gg_measure As String


        'Public Property HH_measure As String
        '    Get
        '        Return _hh_measure
        '    End Get
        '    Set(value As String)
        '        _hh_measure = value
        '    End Set
        'End Property
        'Private Property _hh_measure As String



        'Public Property BRIDGE_LOCATION As String
        '    Get
        '        Return _bLocation
        '    End Get
        '    Set(value As String)
        '        _bLocation = value
        '    End Set
        'End Property

        'Private Property _bLocation As String

        'Public Property NS_OR_EW_BOUND As String
        '    Get
        '        Return _ns_or_ew_bound
        '    End Get
        '    Set(value As String)
        '        _ns_or_ew_bound = value
        '    End Set
        'End Property
        'Private Property _ns_or_ew_bound As String

        'Public Property SOUTH_OR_EAST_BOUND As String
        '    Get
        '        Return _s_or_e_bound
        '    End Get
        '    Set(value As String)
        '        _s_or_e_bound = value
        '    End Set
        'End Property
        'Private Property _s_or_e_bound As String

        'Public Property NORTH_OR_WEST_BOUND As String
        '    Get
        '        Return _n_or_w_bound
        '    End Get
        '    Set(value As String)
        '        _n_or_w_bound = value
        '    End Set
        'End Property
        'Private Property _n_or_w_bound As String

        'Public Property TWY_TRAFFIC_DIRECTION As String
        '    Get
        '        Return _twy_traffic_dir
        '    End Get
        '    Set(value As String)
        '        _twy_traffic_dir = value
        '    End Set
        'End Property
        'Private Property _twy_traffic_dir As String

        'Public Property Measurement_description As String
        '    Get
        '        Return _measurement_description
        '    End Get
        '    Set(value As String)
        '        _measurement_description = value
        '    End Set
        'End Property
        'Private Property _measurement_description As String

        'Public Property Remarks As String
        '    Get
        '        Return _remarks
        '    End Get
        '    Set(value As String)
        '        _remarks = value
        '    End Set
        'End Property
        'Private Property _remarks As String


        'Public Property PRIMARY_INSP_RESPONSIBL_CODE As String
        '    Get
        '        Return _rc
        '    End Get
        '    Set(value As String)
        '        _rc = value
        '    End Set
        'End Property
        'Private Property _rc As String

        'Public Property Page_Index As Integer
        '    Get
        '        Return _page_index
        '    End Get
        '    Set(value As Integer)
        '        _page_index = value
        '    End Set
        'End Property
        'Private Property _page_index As Integer


        Public Property ADD_ID As String
            Get
                Return _addID
            End Get
            Set(value As String)
                _addID = value
            End Set
        End Property
        Private Property _addID As String

        Public Property ADD_Date As Date
            Get
                Return _add_date
            End Get
            Set(value As Date)
                _add_date = value
            End Set
        End Property
        Private Property _add_date As Date

        Public Property UPDATE_ID As String
            Get
                Return _updateID
            End Get
            Set(value As String)
                _updateID = value
            End Set
        End Property
        Private Property _updateID As String

        Public Property Update_Date As Date
            Get
                Return _update_date
            End Get
            Set(value As Date)
                _update_date = value
            End Set
        End Property
        Private Property _update_date As Date

        'Public Property Division As String
        '    Get
        '        Return _division
        '    End Get
        '    Set(value As String)
        '        _division = value
        '    End Set
        'End Property
        'Private Property _division As String

        ''******* 11/23/2015 ***********************
        'Public Property ThruwayDirection As String
        '    Get
        '        Return _thruwayDirection
        '    End Get
        '    Set(value As String)
        '        _thruwayDirection = value
        '    End Set
        'End Property
        'Private Property _thruwayDirection As String
        ''*******************************************
        ''******* 12/2/2015 ***********************
        'Public Property DIR_OF_ORIENT As String
        '    Get
        '        Return _dirOrient
        '    End Get
        '    Set(value As String)
        '        _dirOrient = value
        '    End Set
        'End Property
        'Private Property _dirOrient As String

        'Public Property Bin As String
        '    Get
        '        Return _bin
        '    End Get
        '    Set(value As String)
        '        _bin = value
        '    End Set
        'End Property
        'Private Property _bin As String


        Public Property Divisions() As IDictionary(Of String, String)
            Get
                Return d_Divisions
            End Get
            Set(value As IDictionary(Of String, String))
                d_Divisions = value
            End Set
        End Property

        Private d_Divisions As IDictionary(Of String, String)


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="page"></param>
        ''' <param name="limit"></param>
        ''' <param name="sortBy"></param>
        ''' <param name="direction"></param>
        ''' <param name="searchString"></param>
        ''' <returns></returns>
        Public Function GetAllReadings(page As System.Nullable(Of Integer), limit As System.Nullable(Of Integer), sortBy As String, direction As String, Optional searchString As String = Nothing, Optional ByRef total As Integer = 0) As IOrderedQueryable(Of ReadingModel) 'Implements IReadingRepository.GetAllReadings


            Dim rd As ReadingsData = New ReadingsData

            Try
                rd.dbOpenConnection()
                'connection.Open()
                Dim cmd As IDbCommand = rd.IIISDevDb.CreateCommand
                'connection.CreateCommand()
                cmd.CommandText = rd.GetAllUCSql

                Dim readings As IOrderedQueryable(Of ReadingModel) '= New IOrderedQueryable(Of ReadingModel)

                readings = GetParamReadings(cmd, page, limit, sortBy, direction, searchString, total)

                rd.IIISWebDb.Close()
                'connection.Close()
                cmd.Dispose()


                Return readings


            Catch ex As Exception
                Utilities.WriteError(ex, "GetAllReadings")
            End Try

        End Function

        Public Function GetDivisions() As DivisionList



        End Function


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="strMP"></param>
        ''' <returns></returns>
        Public Function ReadingsByMP(ByVal strMP As String) As IEnumerable(Of ReadingModel) 'Implements IReadingRepository.ReadingsByMP

            Dim rd As ReadingsData = New ReadingsData
            Try

                'connection.Open()
                'Dim cmd As IDbCommand = connection.CreateCommand()
                'cmd.CommandText = rd.GetBrdgUCSql(strMP)
                Dim readings As List(Of ReadingModel) = New List(Of ReadingModel)

                'readings = GetParamReadings(cmd)
                'readings = GetAllReadings.Where(Function(r As ReadingModel) r.Milepost = Math.Round(Convert.ToDouble(strMP), 2))

                'connection.Close()
                'cmd.Dispose()

                Return readings

            Catch ex As Exception
                Utilities.WriteError(ex, "ReadingsByMP")
            End Try

        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="strBT"></param>
        ''' <param name="strDiv"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ReadingsByDivision(ByVal strBT As String, ByVal strDiv As String) As IEnumerable(Of ReadingModel) 'Implements IReadingRepository.ReadingsByDivision

            'Dim rd As ReadingsData = New ReadingsData
            'Try
            '    'connection.Open()
            '    'Dim cmd As IDbCommand = connection.CreateCommand()
            '    'cmd.CommandText = rd.GetFirstRecSql(strBT, strDiv)
            '    Dim readings As List(Of ReadingModel) = New List(Of ReadingModel)

            '    readings = GetAllReadings.Where(Function(r As ReadingModel) r.OVERHEAD_OR_MAINLINE_BRIDGE = strBT And r.Division = strDiv)
            '    'readings = GetParamReadings(cmd)

            '    'connection.Close()
            '    'cmd.Dispose()

            '    Return readings

            'Catch ex As Exception
            '    Utilities.WriteError(ex, "ReadingsByDivision")
            'End Try

        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="intID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ReadingByID(ByVal intID As Integer) As ReadingModel 'Implements IReadingRepository.ReadingByID

            Dim reading As ReadingModel

            'reading = GetAllReadings.First(Function(r As ReadingModel) r.INSP_ID = intID)

            Return reading

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="strSQL"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetParamReadings(ByRef cmd As IDbCommand, page As System.Nullable(Of Integer),
                                                limit As System.Nullable(Of Integer), sortBy As String,
                                                direction As String, Optional searchString As String = Nothing, Optional ByRef total As Integer = 0) _
                                          As IEnumerable(Of ReadingModel)

            Dim rd As ReadingsData = New ReadingsData

            'page = 10
            'limit = 5

            Try
                cmd.Connection.Open()

                Dim dr As IDataReader = cmd.ExecuteReader
                Dim lstReadings As List(Of ReadingModel) = New List(Of ReadingModel)
                Dim readings As IOrderedQueryable(Of ReadingModel)
                While dr.Read
                    Dim reading As ReadingModel = New ReadingModel
                    rd.Map(dr, reading)
                    lstReadings.Add(reading)
                End While

                readings = (From r In lstReadings
                            Select New ReadingModel() With {
                               .INSP_ID = Integer.Parse(r.INSP_ID),
                               .Milepost = r.Milepost,
                                .UC_Read_Date = r.UC_Read_Date,
                                .Inspect_Date = r.Inspect_Date
                                            }).AsQueryable()
                total = readings.Count
                If Not (String.IsNullOrWhiteSpace(searchString)) Then

                    readings = readings.Where(Function(r As ReadingModel) r.Milepost.Contains(searchString))
                End If

                If Not IsNothing(sortBy) And Not IsNothing(direction) Then

                    If (direction.Trim().ToLower() = "asc") Then

                        readings = SortHelper.OrderBy(Of ReadingModel)(readings, sortBy)

                    Else

                        readings = SortHelper.OrderByDescending(Of ReadingModel)(readings, sortBy)

                    End If
                End If

                If (page.HasValue And limit.HasValue) Then
                    Dim start As Integer = (page.Value - 1) * limit.Value
                    readings = readings.Skip(start).Take(limit.Value)
                End If

                Return readings

            Catch ex As Exception
                Utilities.WriteError(ex, "GetParamReadings")

            Finally
                cmd.Connection.Close()
            End Try

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="reading"></param>
        Public Sub Save(reading As ReadingModel)

            'insertUnderclearance = False
            Dim sErrMsg As String
            'Dim rd As ReadingsData = New ReadingsData
            ''FIRST GET THE nexid THAT HAS BEEN INSERTED
            'Dim nextIDCmd = New OracleCommand
            ''Dim strInsert As String = UCDB.GetInsertSQL("IIIS.BIMS_BRIDGE_UNDERCLEARANCE", lstKV)

            '' this now returns IIISWEB connection with update  TVO 5/24/2016
            ''Dim UCDB As MyDB = New MyDB

            ''Dim entUC As ClsBrdgUCEntity = New ClsBrdgUCEntity(lkv)
            ''Dim insertUCCmd As New OracleCommand
            'Dim insertUCCmd As New OracleCommand

            'Dim iNextID As Long = 0
            'Dim iIsInserted As Integer = 0
            'Dim sSQL As String = ""
            'Dim strDate As String = rd.GetDateSQL(reading.Inspect_Date)

            ''sSQL = "insert into  iiis.BIMS_BRIDGE_UNDERCLEARANCE (MILEPOST, INSPECT_DATE, ID, A_MEASURE, B_MEASURE,"
            'sSQL = "INSERT INTO IIIS_WEB_UNDRCLR(MILEPOST, INSPECT_DATE, UC_READ_DATE, "
            ''A_MEASURE, B_MEASURE, "

            ''sSQL = "insert into  iiis.BIMS_BRIDGE_UNDERCLEARANCE (MILEPOST, INSPECT_DATE, UC_READ_DATE, A_MEASURE, B_MEASURE,"
            ''sSQL += "C_MEASURE, D_MEASURE, E_MEASURE, F_MEASURE, G_MEASURE, H_MEASURE, AA_MEASURE, BB_MEASURE, CC_MEASURE,"
            ''sSQL += "DD_MEASURE, EE_MEASURE, FF_MEASURE, GG_MEASURE, HH_MEASURE, MEASUREMENT_DESCRIPTION, REMARKS, DIR_OF_ORIENT, "
            ''sSQL += "SOUTH_OR_EAST_BOUND, NORTH_OR_WEST_BOUND,TWY_TRAFFIC_DIRECTION, OVERHEAD_OR_MAINLINE_BRIDGE, "
            'sSQL += "ADD_DATE, ADD_ID, UPDATE_DATE, UPDATE_ID) "
            'sSQL += "VALUES (:MILEPOST,:INSPECT_DATE, :UC_READ_DATE, "
            ''sSQL += ":A_MEASURE,:B_MEASURE,:C_MEASURE, :D_MEASURE, :E_MEASURE,"
            ''sSQL += ":F_MEASURE, :G_MEASURE, :H_MEASURE, :AA_MEASURE, :BB_MEASURE, "
            ''sSQL += ":CC_MEASURE,:DD_MEASURE, :EE_MEASURE, :FF_MEASURE, :GG_MEASURE, :HH_MEASURE, "
            ''sSQL += ":MEASUREMENT_DESCRIPTION, :REMARKS, :DIR_OF_ORIENT, :SOUTH_OR_EAST_BOUND, :NORTH_OR_WEST_BOUND,"
            ''sSQL += ":TWY_TRAFFIC_DIRECTION, :OVERHEAD_OR_MAINLINE_BRIDGE, :ADD_DATE, :ADD_ID, :UPDATE_DATE, :UPDATE_ID)"
            'sSQL += " :ADD_DATE, :ADD_ID, :UPDATE_DATE, :UPDATE_ID)"


            'rd.dbOpenConnection()
            'insertUCCmd.Connection = rd.IIISWebDb 'IIISDevDb
            'insertUCCmd.CommandText = sSQL
            'insertUCCmd.CommandType = CommandType.Text

            'insertUCCmd.Parameters.Add("MILEPOST", reading.Milepost)

            'insertUCCmd.Parameters.Add("INSPECT_DATE", rd.NVL(Of Date)(reading.Inspect_Date))
            'insertUCCmd.Parameters.Add("UC_READ_DATE", reading.UC_Read_Date)
            ''insertUCCmd.Parameters.Add("ID", entUC.INSP_ID)
            ''insertUCCmd.Parameters.Add("A_MEASURE", entUC.A_measure)
            ''insertUCCmd.Parameters.Add("B_MEASURE", entUC.B_measure)
            ''insertUCCmd.Parameters.Add("C_MEASURE", entUC.C_measure)
            ''insertUCCmd.Parameters.Add("D_MEASURE", entUC.D_measure)
            ''insertUCCmd.Parameters.Add("E_MEASURE", entUC.E_measure)
            ''insertUCCmd.Parameters.Add("F_MEASURE", entUC.F_measure)
            ''insertUCCmd.Parameters.Add("G_MEASURE", entUC.G_measure)
            ''insertUCCmd.Parameters.Add("H_MEASURE", entUC.H_measure)
            ''insertUCCmd.Parameters.Add("AA_MEASURE", entUC.AA_measure)
            ''insertUCCmd.Parameters.Add("BB_MEASURE", entUC.BB_measure)
            ''insertUCCmd.Parameters.Add("CC_MEASURE", entUC.CC_measure)
            ''insertUCCmd.Parameters.Add("DD_MEASURE", entUC.DD_measure)
            ''insertUCCmd.Parameters.Add("EE_MEASURE", entUC.EE_measure)
            ''insertUCCmd.Parameters.Add("FF_MEASURE", entUC.FF_measure)
            ''insertUCCmd.Parameters.Add("GG_MEASURE", entUC.GG_measure)
            ''insertUCCmd.Parameters.Add("HH_MEASURE", entUC.HH_measure)
            ''insertUCCmd.Parameters.Add("MEASUREMENT_DESCRIPTION", entUC.Measurement_description)
            ''insertUCCmd.Parameters.Add("REMARKS", entUC.Remarks)
            ''insertUCCmd.Parameters.Add("DIR_OF_ORIENT", entUC.DIR_OF_ORIENT)
            ''insertUCCmd.Parameters.Add("SOUTH_OR_EAST_BOUND", entUC.SOUTH_OR_EAST_BOUND)
            ''insertUCCmd.Parameters.Add("NORTH_OR_WEST_BOUND", entUC.NORTH_OR_WEST_BOUND)
            ''insertUCCmd.Parameters.Add("TWY_TRAFFIC_DIRECTION", entUC.TWY_TRAFFIC_DIRECTION)
            ''insertUCCmd.Parameters.Add("OVERHEAD_OR_MAINLINE_BRIDGE", entUC.OVERHEAD_OR_MAINLINE_BRIDGE)
            'insertUCCmd.Parameters.Add("ADD_DATE", entUC.ADD_Date)
            'insertUCCmd.Parameters.Add("ADD_ID", entUC.ADD_ID)
            'insertUCCmd.Parameters.Add("UPDATE_DATE", entUC.Update_Date)
            'insertUCCmd.Parameters.Add("UPDATE_ID", entUC.UPDATE_ID)

            Try
                'iIsInserted = insertUCCmd.ExecuteNonQuery()
                If InsertUC(reading) = 1 Then
                    'insertUnderclearance = True
                    sErrMsg = "ADDED TO BRIDGE UNDERCLEANCE INSPECTIONS"
                Else
                    sErrMsg = "UNABLE TO ADD MILEPOST TO BRIDGE UNDERCLEANCE INSPECTIONS"
                End If
            Catch ex As Exception
                sErrMsg = "ERRORS IN INSERTING MILEPOST TO RETAINING INVENTORY:" & ex.Message
                Utilities.WriteError(ex, sErrMsg)
            Finally
                'rd.dbCloseConnection()
                'UCDB = Nothing
            End Try
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="reading"></param>
        ''' <returns></returns>
        Public Function InsertUC(reading As ReadingModel) As Boolean

            InsertUC = False
            Dim sErrMsg As String
            Dim rd As ReadingsData = New ReadingsData

            'FIRST GET THE nexid THAT HAS BEEN INSERTED
            Dim nextIDCmd = New OracleCommand
            'Dim strInsert As String = UCDB.GetInsertSQL("IIIS.BIMS_BRIDGE_UNDERCLEARANCE", lstKV)

            ' this now returns IIISWEB connection with update  TVO 5/24/2016
            'Dim UCDB As MyDB = New MyDB

            'Dim entUC As ClsBrdgUCEntity = New ClsBrdgUCEntity(lkv)
            'Dim insertUCCmd As New OracleCommand
            Dim insertUCCmd As New OracleCommand

            Dim iNextID As Long = 0
            Dim iIsInserted As Integer = 0
            Dim sSQL As String = ""
            Dim strDate As String = rd.GetDateSQL(reading.Inspect_Date)

            'sSQL = "insert into  iiis.BIMS_BRIDGE_UNDERCLEARANCE (MILEPOST, INSPECT_DATE, ID, A_MEASURE, B_MEASURE,"
            sSQL = "INSERT INTO IIIS_WEB_UNDRCLR(MILEPOST, INSPECT_DATE, UC_READ_DATE, "
            'A_MEASURE, B_MEASURE, "

            'sSQL = "insert into  iiis.BIMS_BRIDGE_UNDERCLEARANCE (MILEPOST, INSPECT_DATE, UC_READ_DATE, A_MEASURE, B_MEASURE,"
            'sSQL += "C_MEASURE, D_MEASURE, E_MEASURE, F_MEASURE, G_MEASURE, H_MEASURE, AA_MEASURE, BB_MEASURE, CC_MEASURE,"
            'sSQL += "DD_MEASURE, EE_MEASURE, FF_MEASURE, GG_MEASURE, HH_MEASURE, MEASUREMENT_DESCRIPTION, REMARKS, DIR_OF_ORIENT, "
            'sSQL += "SOUTH_OR_EAST_BOUND, NORTH_OR_WEST_BOUND,TWY_TRAFFIC_DIRECTION, OVERHEAD_OR_MAINLINE_BRIDGE, "
            sSQL += "ADD_DATE, ADD_ID, UPDATE_DATE, UPDATE_ID) "
            sSQL += "VALUES (:MILEPOST,:INSPECT_DATE, :UC_READ_DATE, "
            'sSQL += ":A_MEASURE,:B_MEASURE,:C_MEASURE, :D_MEASURE, :E_MEASURE,"
            'sSQL += ":F_MEASURE, :G_MEASURE, :H_MEASURE, :AA_MEASURE, :BB_MEASURE, "
            'sSQL += ":CC_MEASURE,:DD_MEASURE, :EE_MEASURE, :FF_MEASURE, :GG_MEASURE, :HH_MEASURE, "
            'sSQL += ":MEASUREMENT_DESCRIPTION, :REMARKS, :DIR_OF_ORIENT, :SOUTH_OR_EAST_BOUND, :NORTH_OR_WEST_BOUND,"
            'sSQL += ":TWY_TRAFFIC_DIRECTION, :OVERHEAD_OR_MAINLINE_BRIDGE, :ADD_DATE, :ADD_ID, :UPDATE_DATE, :UPDATE_ID)"
            sSQL += " :ADD_DATE, :ADD_ID, :UPDATE_DATE, :UPDATE_ID)"


            rd.dbOpenConnection()
            insertUCCmd.Connection = rd.IIISWebDb 'IIISDevDb
            insertUCCmd.CommandText = sSQL
            insertUCCmd.CommandType = CommandType.Text

            insertUCCmd.Parameters.Add("MILEPOST", reading.Milepost)

            insertUCCmd.Parameters.Add("INSPECT_DATE", rd.NVL(Of Date)(reading.Inspect_Date))
            insertUCCmd.Parameters.Add("UC_READ_DATE", reading.UC_Read_Date)
            'insertUCCmd.Parameters.Add("ID", entUC.INSP_ID)
            'insertUCCmd.Parameters.Add("A_MEASURE", entUC.A_measure)
            'insertUCCmd.Parameters.Add("B_MEASURE", entUC.B_measure)
            'insertUCCmd.Parameters.Add("C_MEASURE", entUC.C_measure)
            'insertUCCmd.Parameters.Add("D_MEASURE", entUC.D_measure)
            'insertUCCmd.Parameters.Add("E_MEASURE", entUC.E_measure)
            'insertUCCmd.Parameters.Add("F_MEASURE", entUC.F_measure)
            'insertUCCmd.Parameters.Add("G_MEASURE", entUC.G_measure)
            'insertUCCmd.Parameters.Add("H_MEASURE", entUC.H_measure)
            'insertUCCmd.Parameters.Add("AA_MEASURE", entUC.AA_measure)
            'insertUCCmd.Parameters.Add("BB_MEASURE", entUC.BB_measure)
            'insertUCCmd.Parameters.Add("CC_MEASURE", entUC.CC_measure)
            'insertUCCmd.Parameters.Add("DD_MEASURE", entUC.DD_measure)
            'insertUCCmd.Parameters.Add("EE_MEASURE", entUC.EE_measure)
            'insertUCCmd.Parameters.Add("FF_MEASURE", entUC.FF_measure)
            'insertUCCmd.Parameters.Add("GG_MEASURE", entUC.GG_measure)
            'insertUCCmd.Parameters.Add("HH_MEASURE", entUC.HH_measure)
            'insertUCCmd.Parameters.Add("MEASUREMENT_DESCRIPTION", entUC.Measurement_description)
            'insertUCCmd.Parameters.Add("REMARKS", entUC.Remarks)
            'insertUCCmd.Parameters.Add("DIR_OF_ORIENT", entUC.DIR_OF_ORIENT)
            'insertUCCmd.Parameters.Add("SOUTH_OR_EAST_BOUND", entUC.SOUTH_OR_EAST_BOUND)
            'insertUCCmd.Parameters.Add("NORTH_OR_WEST_BOUND", entUC.NORTH_OR_WEST_BOUND)
            'insertUCCmd.Parameters.Add("TWY_TRAFFIC_DIRECTION", entUC.TWY_TRAFFIC_DIRECTION)
            'insertUCCmd.Parameters.Add("OVERHEAD_OR_MAINLINE_BRIDGE", entUC.OVERHEAD_OR_MAINLINE_BRIDGE)

            insertUCCmd.Parameters.Add("ADD_DATE", reading.ADD_Date)
            insertUCCmd.Parameters.Add("ADD_ID", reading.ADD_ID)
            insertUCCmd.Parameters.Add("UPDATE_DATE", reading.Update_Date)
            insertUCCmd.Parameters.Add("UPDATE_ID", reading.UPDATE_ID)

            Try
                iIsInserted = insertUCCmd.ExecuteNonQuery()
                If iIsInserted = 1 Then
                    InsertUC = True
                    sErrMsg = "ADDED TO BRIDGE UNDERCLEANCE INSPECTIONS"
                Else
                    sErrMsg = "UNABLE TO ADD MILEPOST TO BRIDGE UNDERCLEANCE INSPECTIONS"
                End If
            Catch ex As Exception
                sErrMsg = "ERRORS IN INSERTING MILEPOST TO RETAINING INVENTORY:" & ex.Message
                Utilities.WriteError(ex, sErrMsg)
            Finally
                rd.dbCloseConnection()
                'UCDB = Nothing
            End Try
        End Function




    End Class

    Public Class DivisionList
        ''' <summary> 
        ''' Dictionary holding the sample Makes values 
        ''' </summary> 
        Public Property Divisions() As IDictionary(Of String, String)
            Get
                Return d_Divisions
            End Get
            Set(value As IDictionary(Of String, String))
                d_Divisions = value
            End Set
        End Property

        Private d_Divisions As IDictionary(Of String, String)
    End Class



    '<PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage:="The password and confirmation password do not match.")> _
    'Public Class RegisterModel
    '    Private userNameValue As String
    '    Private passwordValue As String
    '    Private confirmPasswordValue As String
    '    Private emailValue As String

    '    <Required()> _
    '    <DisplayName("User name")> _
    '    Public Property UserName() As String
    '        Get
    '            Return userNameValue
    '        End Get
    '        Set(ByVal value As String)
    '            userNameValue = value
    '        End Set
    '    End Property

    '    <Required()> _
    '    <DataType(DataType.EmailAddress)> _
    '    <DisplayName("Email address")> _
    '    Public Property Email() As String
    '        Get
    '            Return emailValue
    '        End Get
    '        Set(ByVal value As String)
    '            emailValue = value
    '        End Set
    '    End Property

    '    <Required()> _
    '    <ValidatePasswordLength()> _
    '    <DataType(DataType.Password)> _
    '    <DisplayName("Password")> _
    '    Public Property Password() As String
    '        Get
    '            Return passwordValue
    '        End Get
    '        Set(ByVal value As String)
    '            passwordValue = value
    '        End Set
    '    End Property

    '    <Required()> _
    '    <DataType(DataType.Password)> _
    '    <DisplayName("Confirm password")> _
    '    Public Property ConfirmPassword() As String
    '        Get
    '            Return confirmPasswordValue
    '        End Get
    '        Set(ByVal value As String)
    '            confirmPasswordValue = value
    '        End Set
    '    End Property
    'End Class
#End Region

#Region "Services"
    ' The FormsAuthentication type is sealed and contains static members, so it is difficult to
    ' unit test code that calls its members. The interface and helper class below demonstrate
    ' how to create an abstract wrapper around such a type in order to make the AccountController
    ' code unit testable.

    Public Interface IReadingService
        'ReadOnly Property MinPasswordLength() As Integer

        'Function ValidateUser(ByVal userName As String, ByVal password As String) As Boolean
        'Function CreateUser(ByVal userName As String, ByVal password As String, ByVal email As String) As MembershipCreateStatus
        'Function ChangePassword(ByVal userName As String, ByVal oldPassword As String, ByVal newPassword As String) As Boolean
    End Interface

    Public Class ReadingService
        Implements IReadingService

        'Private ReadOnly _provider As MembershipProvider

        Public Sub New()
            'Me.New(Nothing)
        End Sub

        'Public Sub New(ByVal provider As MembershipProvider)
        '    _provider = If(provider, Membership.Provider)
        'End Sub

        'Public ReadOnly Property MinPasswordLength() As Integer Implements IMembershipService.MinPasswordLength
        '    Get
        '        Return _provider.MinRequiredPasswordLength
        '    End Get
        'End Property

        'Public Function ValidateUser(ByVal userName As String, ByVal password As String) As Boolean Implements IMembershipService.ValidateUser
        '    If String.IsNullOrEmpty(userName) Then Throw New ArgumentException("Value cannot be null or empty.", "userName")
        '    If String.IsNullOrEmpty(password) Then Throw New ArgumentException("Value cannot be null or empty.", "password")

        '    Return _provider.ValidateUser(userName, password)
        'End Function

        'Public Function CreateUser(ByVal userName As String, ByVal password As String, ByVal email As String) As MembershipCreateStatus Implements IMembershipService.CreateUser
        '    If String.IsNullOrEmpty(userName) Then Throw New ArgumentException("Value cannot be null or empty.", "userName")
        '    If String.IsNullOrEmpty(password) Then Throw New ArgumentException("Value cannot be null or empty.", "password")
        '    If String.IsNullOrEmpty(email) Then Throw New ArgumentException("Value cannot be null or empty.", "email")

        '    Dim status As MembershipCreateStatus
        '    _provider.CreateUser(userName, password, email, Nothing, Nothing, True, Nothing, status)
        '    Return status
        'End Function

        'Public Function ChangePassword(ByVal userName As String, ByVal oldPassword As String, ByVal newPassword As String) As Boolean Implements IMembershipService.ChangePassword
        '    If String.IsNullOrEmpty(userName) Then Throw New ArgumentException("Value cannot be null or empty.", "username")
        '    If String.IsNullOrEmpty(oldPassword) Then Throw New ArgumentException("Value cannot be null or empty.", "oldPassword")
        '    If String.IsNullOrEmpty(newPassword) Then Throw New ArgumentException("Value cannot be null or empty.", "newPassword")

        '    ' The underlying ChangePassword() will throw an exception rather
        '    ' than return false in certain failure scenarios.
        '    Try
        '        Dim currentUser As MembershipUser = _provider.GetUser(userName, True)
        '        Return currentUser.ChangePassword(oldPassword, newPassword)
        '    Catch ex As ArgumentException
        '        Return False
        '    Catch ex As MembershipPasswordException
        '        Return False
        '    End Try
        'End Function
    End Class

    'Public Interface IFormsAuthenticationService
    '    Sub SignIn(ByVal userName As String, ByVal createPersistentCookie As Boolean)
    '    Sub SignOut()
    'End Interface

    'Public Class FormsAuthenticationService
    '    Implements IFormsAuthenticationService

    '    Public Sub SignIn(ByVal userName As String, ByVal createPersistentCookie As Boolean) Implements IFormsAuthenticationService.SignIn
    '        If String.IsNullOrEmpty(userName) Then Throw New ArgumentException("Value cannot be null or empty.", "userName")

    '        FormsAuthentication.SetAuthCookie(userName, createPersistentCookie)
    '    End Sub

    '    Public Sub SignOut() Implements IFormsAuthenticationService.SignOut
    '        FormsAuthentication.SignOut()
    '    End Sub
    'End Class
#End Region

#Region "Validation"
    Public NotInheritable Class ReadingValidation
        Public Shared Function ErrorCodeToString(ByVal createStatus As MembershipCreateStatus) As String
            ' See http://go.microsoft.com/fwlink/?LinkID=177550 for
            ' a full list of status codes.
            'Select Case createStatus
            '    Case MembershipCreateStatus.DuplicateUserName
            '        Return "Username already exists. Please enter a different user name."

            '    Case MembershipCreateStatus.DuplicateEmail
            '        Return "A username for that e-mail address already exists. Please enter a different e-mail address."

            '    Case MembershipCreateStatus.InvalidPassword
            '        Return "The password provided is invalid. Please enter a valid password value."

            '    Case MembershipCreateStatus.InvalidEmail
            '        Return "The e-mail address provided is invalid. Please check the value and try again."

            '    Case MembershipCreateStatus.InvalidAnswer
            '        Return "The password retrieval answer provided is invalid. Please check the value and try again."

            '    Case MembershipCreateStatus.InvalidQuestion
            '        Return "The password retrieval question provided is invalid. Please check the value and try again."

            '    Case MembershipCreateStatus.InvalidUserName
            '        Return "The user name provided is invalid. Please check the value and try again."

            '    Case MembershipCreateStatus.ProviderError
            '        Return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator."

            '    Case MembershipCreateStatus.UserRejected
            '        Return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator."

            '    Case Else
            '        Return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator."
            'End Select
        End Function
    End Class

    '<AttributeUsage(AttributeTargets.Class, AllowMultiple:=True, Inherited:=False)> _
    'Public NotInheritable Class PropertiesMustMatchAttribute
    'Inherits ValidationAttribute

    'Private Const _defaultErrorMessage As String = "'{0}' and '{1}' do not match."
    'Private ReadOnly _confirmProperty As String
    'Private ReadOnly _originalProperty As String
    'Private ReadOnly _typeId As New Object()

    'Public Sub New(ByVal originalProperty As String, ByVal confirmProperty As String)
    '    MyBase.New(_defaultErrorMessage)

    '    _originalProperty = originalProperty
    '    _confirmProperty = confirmProperty
    'End Sub

    'Public ReadOnly Property ConfirmProperty() As String
    '    Get
    '        Return _confirmProperty
    '    End Get
    'End Property

    'Public ReadOnly Property OriginalProperty() As String
    '    Get
    '        Return _originalProperty
    '    End Get
    'End Property

    'Public Overrides ReadOnly Property TypeId() As Object
    '    Get
    '        Return _typeId
    '    End Get
    'End Property

    'Public Overrides Function FormatErrorMessage(ByVal name As String) As String
    '    Return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString, OriginalProperty, ConfirmProperty)
    'End Function

    'Public Overrides Function IsValid(ByVal value As Object) As Boolean
    '    Dim properties As PropertyDescriptorCollection = TypeDescriptor.GetProperties(value)
    '    Dim originalValue = properties.Find(OriginalProperty, True).GetValue(value)
    '    Dim confirmValue = properties.Find(ConfirmProperty, True).GetValue(value)
    '    Return Object.Equals(originalValue, confirmValue)
    'End Function
    'End Class

    '<AttributeUsage(AttributeTargets.Field Or AttributeTargets.Property, AllowMultiple:=False, Inherited:=True)> _
    'Public NotInheritable Class ValidatePasswordLengthAttribute
    'Inherits ValidationAttribute

    'Private Const _defaultErrorMessage As String = "'{0}' must be at least {1} characters long."
    'Private ReadOnly _minCharacters As Integer = Membership.Provider.MinRequiredPasswordLength

    'Public Sub New()
    '    MyBase.New(_defaultErrorMessage)
    'End Sub

    'Public Overrides Function FormatErrorMessage(ByVal name As String) As String
    '    Return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString, name, _minCharacters)
    'End Function

    'Public Overrides Function IsValid(ByVal value As Object) As Boolean
    '    Dim valueAsString As String = TryCast(value, String)
    '    Return (valueAsString IsNot Nothing) AndAlso (valueAsString.Length >= _minCharacters)
    'End Function
    'End Class
#End Region
End Namespace
