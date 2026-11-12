Public Interface IOrderView

    Sub SetData(ByVal dt As DataTable)

End Interface


Public Class UcOrderList
    Implements IOrderView

    Public Sub SetData( _
        ByVal dt As DataTable) _
        Implements IOrderView.SetData

        Me.dgvOrder.DataSource = dt

    End Sub

End Class


Public Class OrderService

    Public Function GetOrders( _
        ByVal customerCd As String) _
        As DataTable

        Dim dt As New DataTable

        dt.Columns.Add("OrderNo")
        dt.Columns.Add("CustomerCd")
        dt.Columns.Add("Amount")

        Select Case customerCd

            Case "001"

                dt.Rows.Add("A001", "001", "1000")
                dt.Rows.Add("A002", "001", "2000")

            Case "002"

                dt.Rows.Add("B001", "002", "5000")
                dt.Rows.Add("B002", "002", "8000")

        End Select

        Return dt

    End Function

End Class


Public Class OrderPresenter

    Private ReadOnly _view As IOrderView
    Private ReadOnly _service As OrderService

    Public Sub New( _
        ByVal view As IOrderView)

        _view = view
        _service = New OrderService()

    End Sub

    ''' <summary>
    ''' 顧客コードから受注一覧を取得
    ''' </summary>
    Public Sub LoadOrders( _
        ByVal customerCd As String)

        Dim dt As DataTable

        dt = _service.GetOrders(customerCd)

        _view.SetData(dt)

    End Sub

End Class


Public Class MainForm

    Private _customerPresenter As CustomerPresenter
    Private _orderPresenter As OrderPresenter

    Private Sub MainForm_Load( _
        ByVal sender As Object, _
        ByVal e As EventArgs) _
        Handles MyBase.Load

        _customerPresenter = _
            New CustomerPresenter(Me.ucCustomerList1)

        _orderPresenter = _
            New OrderPresenter(Me.ucOrderList1)

        AddHandler _
            _customerPresenter.CustomerSelected, _
            AddressOf CustomerSelected

    End Sub

    Private Sub CustomerSelected( _
        ByVal customerCd As String)

        _orderPresenter.LoadOrders(customerCd)

    End Sub

End Class


Public Class CustomerInfo

    Public CustomerCd As String
    Public CustomerName As String

End Class


Public Class CustomerSelectedEventArgs

    Public CustomerCd As String
    Public CustomerName As String
    Public BranchCd As String

End Class


Public Event CustomerSelected( _
    ByVal data As CustomerSelectedEventArgs)
    


Private Sub CustomerSelected( _
    ByVal data As CustomerSelectedEventArgs)

    _orderPresenter.LoadOrders(data.CustomerCd)

End Sub


Imports System.Data
Imports System.Data.SqlClient

Public Class OrderRepository

    Private ReadOnly _connectionString As String

    Public Sub New(ByVal connectionString As String)

        _connectionString = connectionString

    End Sub

    ''' <summary>
    ''' 顧客別受注一覧取得
    ''' </summary>
    Public Function GetOrdersByCustomer( _
        ByVal customerCd As String) _
        As DataTable

        Dim dt As New DataTable()

        Dim sql As String = _
            "SELECT " & _
            "    OrderNo, " & _
            "    CustomerCd, " & _
            "    OrderDate, " & _
            "    Amount " & _
            "FROM T_Order " & _
            "WHERE CustomerCd = @CustomerCd " & _
            "ORDER BY OrderDate DESC"

        Using con As New SqlConnection(_connectionString)

            Using cmd As New SqlCommand(sql, con)

                cmd.Parameters.AddWithValue( _
                    "@CustomerCd", _
                    customerCd)

                Using da As New SqlDataAdapter(cmd)

                    da.Fill(dt)

                End Using

            End Using

        End Using

        Return dt

    End Function

End Class



Imports System.Data

Public Class OrderService

    Private ReadOnly _repository As OrderRepository

    Public Sub New( _
        ByVal repository As OrderRepository)

        _repository = repository

    End Sub

    ''' <summary>
    ''' 受注一覧取得
    ''' </summary>
    Public Function GetOrders( _
        ByVal customerCd As String) _
        As DataTable

        If String.IsNullOrEmpty(customerCd) Then

            Return New DataTable()

        End If

        Return _
            _repository.GetOrdersByCustomer(customerCd)

    End Function

End Class


Public Interface IOrderView

    Sub SetData(ByVal dt As DataTable)

End Interface


Imports System.Data

Public Class OrderPresenter

    Private ReadOnly _view As IOrderView
    Private ReadOnly _service As OrderService

    Public Sub New( _
        ByVal view As IOrderView, _
        ByVal service As OrderService)

        _view = view
        _service = service

    End Sub

    ''' <summary>
    ''' 顧客変更時
    ''' </summary>
    Public Sub LoadOrders( _
        ByVal customerCd As String)

        Dim dt As DataTable

        dt = _service.GetOrders(customerCd)

        _view.SetData(dt)

    End Sub

End Class


Public Class MainForm

    Private _customerPresenter As CustomerPresenter
    Private _orderPresenter As OrderPresenter

    Private Sub MainForm_Load( _
        ByVal sender As Object, _
        ByVal e As EventArgs) _
        Handles MyBase.Load

        Dim conStr As String

        conStr = AppConfig.ConnectionString

        Dim repository As New OrderRepository(conStr)

        Dim service As New OrderService(repository)

        _orderPresenter = _
            New OrderPresenter( _
                Me.ucOrderList1, _
                service)

        _customerPresenter = _
            New CustomerPresenter( _
                Me.ucCustomerList1)

        AddHandler _
            _customerPresenter.CustomerSelected, _
            AddressOf CustomerSelected

    End Sub

    Private Sub CustomerSelected( _
        ByVal customerCd As String)

        _orderPresenter.LoadOrders(customerCd)

    End Sub

End Class


Imports System.Data
Imports System.Data.SqlClient

Public Class OrderRepository

    Private ReadOnly _connectionString As String

    Public Sub New(ByVal connectionString As String)

        _connectionString = connectionString

    End Sub

    Public Function GetOrdersByCustomer( _
        ByVal customerCd As String) _
        As DataTable

        Dim dt As New DataTable()

        Dim sql As String = _
            "SELECT * " & _
            "FROM T_Order " & _
            "WHERE CustomerCd = @CustomerCd"

        Using con As New SqlConnection(_connectionString)

            con.Open()

            Using cmd As New SqlCommand(sql, con)

                cmd.Parameters.AddWithValue( _
                    "@CustomerCd", _
                    customerCd)

                Using da As New SqlDataAdapter(cmd)

                    da.Fill(dt)

                End Using

            End Using

        End Using

        Return dt

    End Function

End Class


Public Class OrderService

    Private ReadOnly _repository As OrderRepository

    Public Sub New( _
        ByVal repository As OrderRepository)

        _repository = repository

    End Sub

    Public Function GetOrders( _
        ByVal customerCd As String) _
        As DataTable

        Return _
            _repository.GetOrdersByCustomer(customerCd)

    End Function

End Class


Public Interface IOrderView

    Sub SetData(ByVal dt As DataTable)

    Sub ShowError(ByVal message As String)

End Interface


Public Class UcOrderList
    Implements IOrderView

    Public Sub SetData( _
        ByVal dt As DataTable) _
        Implements IOrderView.SetData

        dgvOrder.DataSource = dt

    End Sub

    Public Sub ShowError( _
        ByVal message As String) _
        Implements IOrderView.ShowError

        MessageBox.Show( _
            message, _
            "エラー", _
            MessageBoxButtons.OK, _
            MessageBoxIcon.Error)

    End Sub

End Class


Imports System.Data.SqlClient

Public Class OrderPresenter

    Private ReadOnly _view As IOrderView
    Private ReadOnly _service As OrderService

    Public Sub New( _
        ByVal view As IOrderView, _
        ByVal service As OrderService)

        _view = view
        _service = service

    End Sub

    Public Sub LoadOrders( _
        ByVal customerCd As String)

        Try

            Dim dt As DataTable

            dt = _service.GetOrders(customerCd)

            _view.SetData(dt)

        Catch ex As SqlException

            _view.ShowError( _
                "データベースに接続できません。" & _
                vbCrLf & _
                ex.Message)

        Catch ex As Exception

            _view.ShowError( _
                "予期しないエラーが発生しました。" & _
                vbCrLf & _
                ex.Message)

        End Try

    End Sub

End Class


Public Class Result

    Public Success As Boolean

    Public Message As String

End Class


Public Class DataResult

    Public Success As Boolean

    Public Message As String

    Public Data As DataTable

End Class


Public Function GetOrders( _
    ByVal customerCd As String) _
    As DataResult

    Dim result As New DataResult

    Try

        result.Data = _
            _repository.GetOrdersByCustomer(customerCd)

        result.Success = True

    Catch ex As Exception

        result.Success = False

        result.Message = ex.Message

    End Try

    Return result

End Function


Public Sub LoadOrders( _
    ByVal customerCd As String)

    Dim result As DataResult

    result = _service.GetOrders(customerCd)

    If result.Success Then

        _view.SetData(result.Data)

    Else

        _view.ShowError(result.Message)

    End If

End Sub


Public Class DataResult

    Public Success As Boolean

    Public ErrorCode As String

    Public Message As String

    Public Data As DataTable

End Class


Imports System.Data.SqlClient

Public Class OrderService

    Private ReadOnly _repository As OrderRepository

    Public Sub New( _
        ByVal repository As OrderRepository)

        _repository = repository

    End Sub

    Public Function GetOrders( _
        ByVal customerCd As String) _
        As DataResult

        Dim result As New DataResult()

        Try

            result.Data = _
                _repository.GetOrdersByCustomer(customerCd)

            If result.Data.Rows.Count = 0 Then

                result.Success = False
                result.ErrorCode = "E004"
                result.Message = "該当データがありません。"

                Return result

            End If

            result.Success = True

        Catch ex As SqlException

            Select Case ex.Number

                Case 53
                    result.ErrorCode = "E001"
                    result.Message = "DBサーバーに接続できません。"

                Case -2
                    result.ErrorCode = "E003"
                    result.Message = "DB処理がタイムアウトしました。"

                Case Else
                    result.ErrorCode = "E002"
                    result.Message = ex.Message

            End Select

            result.Success = False

        Catch ex As Exception

            result.Success = False
            result.ErrorCode = "E999"
            result.Message = ex.Message

        End Try

        Return result

    End Function

End Class


Public Sub LoadOrders( _
    ByVal customerCd As String)

    Dim result As DataResult

    result = _service.GetOrders(customerCd)

    If result.Success Then

        _view.SetData(result.Data)

        Exit Sub

    End If

    Select Case result.ErrorCode

        Case "E001"

            _view.ShowWarning( _
                "サーバーに接続できません。" & _
                vbCrLf & _
                "ネットワークを確認してください。")

        Case "E003"

            _view.ShowWarning( _
                "処理が混雑しています。" & _
                vbCrLf & _
                "しばらくしてから再実行してください。")

        Case "E004"

            _view.ShowInfo( _
                "該当する受注データがありません。")

        Case Else

            _view.ShowError( _
                "システムエラーが発生しました。" & _
                vbCrLf & _
                "(" & result.ErrorCode & ")")

    End Select

End Sub


Public Interface IOrderView

    Sub SetData(ByVal dt As DataTable)

    Sub ShowInfo(ByVal message As String)

    Sub ShowWarning(ByVal message As String)

    Sub ShowError(ByVal message As String)

End Interface


Public Class ErrorCodes

    Public Const DbConnect As String = "E001"

    Public Const SqlError As String = "E002"

    Public Const Timeout As String = "E003"

    Public Const NoData As String = "E004"

    Public Const SystemError As String = "E999"

End Class


Public Class DataResult

    Public Success As Boolean
    Public ErrorCode As String
    Public Message As String
    Public DetailMessage As String
    Public Data As DataTable

End Class


Public Class ErrorCodes

    Public Const DbConnect As String = "E001"

    Public Const LoginFailed As String = "E002"

    Public Const Timeout As String = "E003"

    Public Const DatabaseNotFound As String = "E004"

    Public Const ForeignKey As String = "E005"

    Public Const DuplicateKey As String = "E006"

    Public Const UniqueKey As String = "E007"

    Public Const DeadLock As String = "E008"

    Public Const LockTimeout As String = "E009"

    Public Const DataTooLong As String = "E010"

    Public Const InvalidCast As String = "E011"

    Public Const InvalidNumeric As String = "E012"

    Public Const Permission As String = "E013"

    Public Const ProcNotFound As String = "E014"

    Public Const TableNotFound As String = "E015"

    Public Const ColumnNotFound As String = "E016"

    Public Const SystemError As String = "E999"

End Class


Imports System.Data.SqlClient

Public Class SqlErrorConverter

    Public Shared Function GetErrorCode( _
        ByVal ex As SqlException) _
        As String

        Select Case ex.Number

            Case 53
                Return ErrorCodes.DbConnect

            Case 18456
                Return ErrorCodes.LoginFailed

            Case -2
                Return ErrorCodes.Timeout

            Case 4060
                Return ErrorCodes.DatabaseNotFound

            Case 547
                Return ErrorCodes.ForeignKey

            Case 2627
                Return ErrorCodes.DuplicateKey

            Case 2601
                Return ErrorCodes.UniqueKey

            Case 1205
                Return ErrorCodes.DeadLock

            Case 1222
                Return ErrorCodes.LockTimeout

            Case 8152
                Return ErrorCodes.DataTooLong

            Case 245
                Return ErrorCodes.InvalidCast

            Case 8114
                Return ErrorCodes.InvalidNumeric

            Case 229
                Return ErrorCodes.Permission

            Case 2812
                Return ErrorCodes.ProcNotFound

            Case 208
                Return ErrorCodes.TableNotFound

            Case 207
                Return ErrorCodes.ColumnNotFound

            Case Else
                Return ErrorCodes.SystemError

        End Select

    End Function

End Class



Catch ex As SqlException

    result.Success = False

    result.ErrorCode = _
        SqlErrorConverter.GetErrorCode(ex)

    result.Message = ex.Message

Catch ex As Exception

    result.Success = False

    result.ErrorCode = ErrorCodes.SystemError

    result.Message = ex.Message

End Try


Select Case result.ErrorCode

    Case ErrorCodes.DbConnect

        _view.ShowError( _
            "データベースサーバーに接続できません。")

    Case ErrorCodes.Timeout

        _view.ShowWarning( _
            "処理がタイムアウトしました。" & vbCrLf & _
            "時間をおいて再実行してください。")

    Case ErrorCodes.DeadLock

        _view.ShowWarning( _
            "他ユーザーと競合しました。" & vbCrLf & _
            "再実行してください。")

    Case ErrorCodes.DuplicateKey

        _view.ShowInfo( _
            "既に登録済みです。")

    Case Else

        _view.ShowError( _
            "システムエラーが発生しました。" & vbCrLf & _
            "コード：" & result.ErrorCode)

End Select


Public Class MessageManager

    Public Shared Function GetMessage( _
        ByVal errorCode As String) _
        As String

        Return My.Resources.ResourceManager.GetString(errorCode)

    End Function

End Class


If result.Success = False Then

    _view.ShowError( _
        MessageManager.GetMessage( _
            result.ErrorCode))

End If


Public Class SqlErrorConverter

    Public Shared Function GetErrorCode( _
        ByVal ex As SqlException) _
        As String

        Select Case ex.Number

            Case 53
                Return ErrorCodes.DbConnect

            Case 18456
                Return ErrorCodes.LoginFailed

            Case -2
                Return ErrorCodes.Timeout

            Case 2627
                Return ErrorCodes.DuplicateKey

            Case 1205
                Return ErrorCodes.DeadLock

            Case Else
                Return ErrorCodes.SystemError

        End Select

    End Function

End Class


Public Class MessageManager

    Public Shared Function GetMessage( _
        ByVal key As String, _
        ByVal ParamArray args() As Object) _
        As String

        Dim format As String

        format = _
            My.Resources.ResourceManager.GetString(key)

        Return String.Format(format, args)

    End Function

End Class


_view.ShowError(
    String.Format(
        "{0}" & vbCrLf & vbCrLf & _
        "[{1}]",
        MessageManager.GetMessage(result.ErrorCode),
        result.ErrorCode))
        
        

Public Class MessageManager

    Public Shared Function GetMessage( _
        ByVal key As String, _
        ByVal ParamArray args() As Object) _
        As String

        Dim format As String

        format = My.Resources.ResourceManager.GetString(key)

        If String.IsNullOrEmpty(format) Then

            Return key

        End If

        Return String.Format(format, args)

    End Function

End Class


Public Class DataResult

    Public Success As Boolean

    Public ErrorCode As String

    Public Message As String

    Public DetailMessage As String

    Public Data As DataTable

End Class


Catch ex As SqlException

    result.Success = False

    result.ErrorCode = ErrorCodes.Timeout

    result.DetailMessage = ex.Message

End Try


If Not result.Success Then

    LogManager.WriteError( _
        result.ErrorCode, _
        result.DetailMessage)

    _view.ShowError( _
        MessageManager.GetMessage( _
            result.ErrorCode))

    Exit Sub

End If


Public Shared Function GetErrorMessage( _
    ByVal errorCode As String, _
    ByVal ParamArray args() As Object) _
    As String

    Return String.Format( _
        "{0}" & vbCrLf & vbCrLf & _
        "[{1}]", _
        GetMessage(errorCode, args), _
        errorCode)

End Function


Imports System.Text.RegularExpressions

Public Class MessageManager

    Public Shared Function GetMessage( _
        ByVal key As String, _
        ByVal ParamArray args() As Object) _
        As String

        Dim format As String

        format = _
            My.Resources.ResourceManager.GetString(key)

        If String.IsNullOrEmpty(format) Then

            Return String.Format( _
                "[{0}] メッセージが定義されていません。", _
                key)

        End If

        ValidateParameterCount( _
            key, _
            format, _
            args)

        Return String.Format(format, args)

    End Function

    Private Shared Sub ValidateParameterCount( _
        ByVal key As String, _
        ByVal format As String, _
        ByVal args() As Object)

        Dim matches As MatchCollection

        matches = Regex.Matches( _
            format, _
            "\{(\d+)\}")

        Dim maxIndex As Integer

        maxIndex = -1

        Dim m As Match

        For Each m In matches

            Dim index As Integer

            index = _
                Convert.ToInt32( _
                    m.Groups(1).Value)

            If index > maxIndex Then

                maxIndex = index

            End If

        Next

        If maxIndex >= args.Length Then

            Throw New ApplicationException( _
                String.Format( _
                    "メッセージ[{0}]のパラメータ不足。" & _
                    "必要:{1}個、指定:{2}個", _
                    key, _
                    maxIndex + 1, _
                    args.Length))

        End If

    End Sub

End Class


If maxIndex >= args.Length Then

    LogManager.WriteError(
        "MSG_PARAM_ERROR",
        key)

    Return format

End If



