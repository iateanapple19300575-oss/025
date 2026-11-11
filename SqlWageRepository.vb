SqlWageRepository.vb
----------------------------------------------------------------------------------------------------
Public Function FindDuplicateStaffIds(ByVal staffIds As List(Of String), ByVal applyDate As DateTime) As List(Of String) Implements IWageRepository.FindDuplicateStaffIds
    Dim duplicates As New List(Of String)()
    If staffIds Is Nothing OrElse staffIds.Count = 0 Then
        Return duplicates
    End If

    Using conn As New SqlConnection(_connectionString)
        conn.Open()

        ' パラメータ名のリストを作成（例: "@Id0,@Id1,@Id2"）
        Dim paramNames As New List(Of String)()
        For i As Integer = 0 To staffIds.Count - 1
            paramNames.Add("@Id" & i.ToString())
        Next

        Dim sql As String = String.Format( _
            "SELECT DISTINCT StaffId FROM T_HourlyWageHistory " & _
            "WHERE ApplyDate = @ApplyDate AND StaffId IN ({0})", _
            String.Join(",", paramNames.ToArray()))

        Using cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@ApplyDate", SqlDbType.DateTime).Value = applyDate.Date

            For i As Integer = 0 To staffIds.Count - 1
                cmd.Parameters.Add(paramNames(i), SqlDbType.VarChar, 20).Value = staffIds(i)
            Next

            Using reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    duplicates.Add(reader("StaffId").ToString())
                End While
            End Using
        End Using
    End Using

    Return duplicates
End Function
----------------------------------------------------------------------------------------------------
