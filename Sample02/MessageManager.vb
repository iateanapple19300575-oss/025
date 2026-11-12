Imports System.Text

Public Class MessageManager

    Public Shared Sub ShowError( _
        ByVal info As AppMessage)

        Dim frm As New FrmMessage()

        frm.Text = "エラー"

        frm.MessageText = BuildMessage(info)

        frm.ShowDialog()

    End Sub

    Public Shared Function BuildMessage( _
        ByVal info As AppMessage) _
        As String

        Dim format As String

        format = _
            My.Resources.ResourceManager.GetString( _
                info.MessageId)

        If info.Parameters IsNot Nothing _
        AndAlso info.Parameters.Length > 0 Then

            format = _
                String.Format( _
                    format, _
                    info.Parameters)

        End If

        Dim sb As New StringBuilder()

        sb.AppendLine(format)

        If Not String.IsNullOrEmpty(info.Target) Then

            sb.AppendLine()
            sb.AppendLine("対象")
            sb.AppendLine(info.Target)

        End If

        If Not String.IsNullOrEmpty(info.ActionMessage) Then

            sb.AppendLine()
            sb.AppendLine("対処方法")
            sb.AppendLine(info.ActionMessage)

        End If

        sb.AppendLine()

        sb.AppendLine("[" & info.MessageId & "]")

        Return sb.ToString()

    End Function

End Class