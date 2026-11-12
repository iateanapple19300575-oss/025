Public Class CustomerPresenter

    Private ReadOnly _view As ICustomerView

    Public Event CustomerSelected(ByVal customerCd As String)

    Public Sub New(ByVal view As ICustomerView)

        _view = view

        AddHandler _
            _view.CustomerSelected, _
            AddressOf View_CustomerSelected

    End Sub

    Private Sub View_CustomerSelected( _
        ByVal customerCd As String)

        RaiseEvent CustomerSelected(customerCd)

    End Sub

End Class