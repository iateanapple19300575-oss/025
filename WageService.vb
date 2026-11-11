----------------------------------------------------------------------------------------------------
Public Class WageService
    Private ReadOnly _repository As IWageRepository

    Public Sub New(ByVal repository As IWageRepository)
        _repository = repository
    End Sub

    ' 重複しているスタッフの情報を抽出して返す
    Public Function CheckDuplicates(ByVal selectedStaffs As List(Of StaffWageSummary), _
                                    ByVal applyDate As DateTime) As List(Of StaffWageSummary)
        ' IDだけのリストを作成（FW3.5のLINQを使用）
        Dim staffIds As List(Of String) = selectedStaffs.Select(Function(s) s.StaffId).ToList()

        ' DBに一括問い合わせ
        Dim dupIds As List(Of String) = _repository.FindDuplicateStaffIds(staffIds, applyDate)

        ' 重複していたスタッフのオブジェクトだけを抽出して返す
        Dim dupSet As New HashSet(Of String)(dupIds)
        Return selectedStaffs.Where(Function(s) dupSet.Contains(s.StaffId)).ToList()
    End Function

    ' 一括登録実行（既存メソッド）
    Public Sub ApplyBulkRaiseByAmount(ByVal targetStaffs As List(Of StaffWageSummary), _
                                      ByVal applyDate As DateTime, _
                                      ByVal raiseAmount As Decimal, _
                                      ByVal note As String)
        Dim newItems As New List(Of WageHistoryItem)()
        For Each staff As StaffWageSummary In targetStaffs
            Dim item As New WageHistoryItem()
            item.StaffId = staff.StaffId
            item.ApplyDate = applyDate.Date
            item.HourlyWage = staff.CurrentWage + raiseAmount
            item.Note = note
            newItems.Add(item)
        Next

        _repository.BulkInsertWageHistories(newItems)
    End Sub
End Class
----------------------------------------------------------------------------------------------------
