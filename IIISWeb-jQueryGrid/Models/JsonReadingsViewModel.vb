Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations

Namespace Models
    Public Class JsonReadingsViewModel

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


        'Public Property ADD_ID As String
        '    Get
        '        Return _addID
        '    End Get
        '    Set(value As String)
        '        _addID = value
        '    End Set
        'End Property
        'Private Property _addID As String

        'Public Property ADD_Date As Date
        '    Get
        '        Return _add_date
        '    End Get
        '    Set(value As Date)
        '        _add_date = value
        '    End Set
        'End Property
        'Private Property _add_date As Date

        'Public Property UPDATE_ID As String
        '    Get
        '        Return _updateID
        '    End Get
        '    Set(value As String)
        '        _updateID = value
        '    End Set
        'End Property
        'Private Property _updateID As String

        'Public Property Update_Date As Date
        '    Get
        '        Return _update_date
        '    End Get
        '    Set(value As Date)
        '        _update_date = value
        '    End Set
        'End Property
        'Private Property _update_date As Date

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



    End Class
End Namespace
