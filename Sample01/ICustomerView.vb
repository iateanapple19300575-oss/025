Public Interface ICustomerView

    Event CustomerSelected(ByVal customerCd As String)

    Sub SetData(ByVal dt As DataTable)

End Interface

