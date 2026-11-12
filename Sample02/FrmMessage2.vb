Imports System.Windows.Forms
Imports System.Drawing

Public Class FrmMessage
    Inherits Form

    Private lblTitle As Label
    Private txtMessage As TextBox
    Private btnOK As Button

    Public Sub New()

        InitializeComponent()

    End Sub

    Private Sub InitializeComponent()

        Me.lblTitle = New Label()
        Me.txtMessage = New TextBox()
        Me.btnOK = New Button()

        '
        ' Form
        '
        Me.Text = "エラー"
        Me.StartPosition = FormStartPosition.CenterParent
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False

        Me.Width = 700
        Me.Height = 450

        '
        ' lblTitle
        '
        Me.lblTitle.Left = 15
        Me.lblTitle.Top = 15
        Me.lblTitle.Width = 650
        Me.lblTitle.Height = 30

        Me.lblTitle.Font = _
            New Font("Meiryo UI", 12.0F, FontStyle.Bold)

        Me.lblTitle.Text = "エラー"

        '
        ' txtMessage
        '
        Me.txtMessage.Left = 15
        Me.txtMessage.Top = 50
        Me.txtMessage.Width = 650
        Me.txtMessage.Height = 300

        Me.txtMessage.Multiline = True

        Me.txtMessage.ReadOnly = True

        Me.txtMessage.ScrollBars = ScrollBars.Vertical

        Me.txtMessage.BorderStyle = BorderStyle.FixedSingle

        Me.txtMessage.Font = _
            New Font("Meiryo UI", 12.0F)

        Me.txtMessage.BackColor = Color.White

        Me.txtMessage.TabStop = False

        '
        ' btnOK
        '
        Me.btnOK.Text = "OK"

        Me.btnOK.Width = 100
        Me.btnOK.Height = 35

        Me.btnOK.Left = 565
        Me.btnOK.Top = 365

        AddHandler Me.btnOK.Click, _
            AddressOf btnOK_Click

        '
        ' Controls
        '
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.txtMessage)
        Me.Controls.Add(Me.btnOK)

    End Sub

    Private Sub btnOK_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs)

        Me.DialogResult = DialogResult.OK

        Me.Close()

    End Sub

    Public Property TitleText() As String

        Get
            Return Me.lblTitle.Text
        End Get

        Set(ByVal value As String)

            Me.lblTitle.Text = value

        End Set

    End Property

    Public Property MessageText() As String

        Get
            Return Me.txtMessage.Text
        End Get

        Set(ByVal value As String)

            Me.txtMessage.Text = value

        End Set

    End Property

End Class


Public Class MessageManager

    Public Shared Sub ShowError( _
        ByVal message As String)

        Dim frm As New FrmMessage()

        frm.TitleText = "エラー"

        frm.MessageText = message

        frm.ShowDialog()

        frm.Dispose()

    End Sub

End Class
