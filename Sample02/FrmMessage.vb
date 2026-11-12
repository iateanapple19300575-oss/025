Imports System.Windows.Forms
Imports System.Drawing

Public Class FrmMessage
    Inherits Form

    Private lblMessage As Label
    Private btnOK As Button

    Public Sub New()

        Me.InitializeComponent()

    End Sub

    Private Sub InitializeComponent()

        Me.lblMessage = New Label()
        Me.btnOK = New Button()

        '
        ' Form
        '
        Me.Text = "メッセージ"
        Me.StartPosition = FormStartPosition.CenterParent
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False

        Me.Width = 700
        Me.Height = 400

        '
        ' lblMessage
        '
        Me.lblMessage.Left = 20
        Me.lblMessage.Top = 20
        Me.lblMessage.Width = 640
        Me.lblMessage.Height = 280

        Me.lblMessage.Font = _
            New Font( _
                "Meiryo UI", _
                12.0F, _
                FontStyle.Regular)

        '
        ' btnOK
        '
        Me.btnOK.Text = "OK"

        Me.btnOK.Width = 120
        Me.btnOK.Height = 40

        Me.btnOK.Left = 540
        Me.btnOK.Top = 310

        AddHandler _
            Me.btnOK.Click, _
            AddressOf btnOK_Click

        '
        ' Control追加
        '
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.btnOK)

    End Sub

    Private Sub btnOK_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs)

        Me.DialogResult = DialogResult.OK

        Me.Close()

    End Sub

    Public Property MessageText() As String

        Get
            Return lblMessage.Text
        End Get

        Set(ByVal value As String)

            lblMessage.Text = value

        End Set

    End Property

End Class