Public Interface IWageRepository
    ' （既存のメソッド群は省略）

    ' 指定した適用日にすでに履歴が存在するスタッフIDの一覧を返す
    Function FindDuplicateStaffIds(ByVal staffIds As List(Of String), ByVal applyDate As DateTime) As List(Of String)

    ' 一括登録
    Sub BulkInsertWageHistories(ByVal items As List(Of WageHistoryItem))
End Interface


