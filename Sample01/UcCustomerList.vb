Public Class UcCustomerList
    Implements ICustomerView

    Public Event CustomerSelected( _
        ByVal customerCd As String) _
        Implements ICustomerView.CustomerSelected

    Public Sub SetData( _
        ByVal dt As DataTable) _
        Implements ICustomerView.SetData

        Me.dgvCustomer.DataSource = dt

    End Sub

    Private Sub dgvCustomer_CellClick( _
        ByVal sender As Object, _
        ByVal e As DataGridViewCellEventArgs) _
        Handles dgvCustomer.CellClick

        If e.RowIndex < 0 Then
            Exit Sub
        End If

        Dim customerCd As String

        customerCd = _
            Convert.ToString( _
                dgvCustomer.Rows(e.RowIndex).Cells("CustomerCd").Value)

        RaiseEvent CustomerSelected(customerCd)

    End Sub

End Class