Public Class AppMessage

    Private _messageId As String
    Private _target As String
    Private _actionMessage As String
    Private _detailMessage As String
    Private _parameters() As Object

    Public Property MessageId() As String
        Get
            Return _messageId
        End Get
        Set(ByVal value As String)
            _messageId = value
        End Set
    End Property

    Public Property Target() As String
        Get
            Return _target
        End Get
        Set(ByVal value As String)
            _target = value
        End Set
    End Property

    Public Property ActionMessage() As String
        Get
            Return _actionMessage
        End Get
        Set(ByVal value As String)
            _actionMessage = value
        End Set
    End Property

    Public Property DetailMessage() As String
        Get
            Return _detailMessage
        End Get
        Set(ByVal value As String)
            _detailMessage = value
        End Set
    End Property

    Public Property Parameters() As Object()
        Get
            Return _parameters
        End Get
        Set(ByVal value As Object())
            _parameters = value
        End Set
    End Property

End Class