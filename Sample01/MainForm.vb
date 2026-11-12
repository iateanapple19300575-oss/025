Public Class MainForm

    Private _customerPresenter As CustomerPresenter
    Private _orderPresenter As OrderPresenter

    Private Sub MainForm_Load( _
        ByVal sender As Object,
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