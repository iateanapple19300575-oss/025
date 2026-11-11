Form.vb
----------------------------------------------------------------------------------------------------
Private Sub btnBulkSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBulkSave.Click
    Dim selectedStaffs As List(Of StaffWageSummary) = GetCheckedStaffs() ' チェックされた一覧を取得
    Dim applyDate As DateTime = dtpBulkApplyDate.Value.Date
    Dim raiseAmount As Decimal = numRaiseAmount.Value
    Dim note As String = txtBulkNote.Text

    If selectedStaffs.Count = 0 Then
        MessageBox.Show("対象者が選択されていません。")
        Return
    End If

    ' 1. 事前重複チェックを実行
    Dim duplicates As List(Of StaffWageSummary) = _wageService.CheckDuplicates(selectedStaffs, applyDate)
    Dim targetStaffs As List(Of StaffWageSummary) = selectedStaffs

    ' 2. 重複が見つかった場合の事前警告ダイアログ
    If duplicates.Count > 0 Then
        ' 最大10名分まで氏名・IDを表示（多すぎる場合の画面崩れ防止）
        Dim previewNames As String = String.Join(Environment.NewLine, _
            duplicates.Take(10).Select(Function(s) "・" & s.StaffId & " : " & s.StaffName).ToArray())

        If duplicates.Count > 10 Then
            previewNames &= Environment.NewLine & "…他 " & (duplicates.Count - 10) & " 名"
        End If

        Dim msg As String = String.Format( _
            "以下の {0} 名は、すでに {1:yyyy/MM/dd} の時給データが登録されています。" & Environment.NewLine & Environment.NewLine & _
            "{2}" & Environment.NewLine & Environment.NewLine & _
            "重複している人を【除外】して、残り {3} 名のみ登録しますか？" & Environment.NewLine & _
            "（「いいえ」を選ぶと処理を中止します）", _
            duplicates.Count, applyDate, previewNames, selectedStaffs.Count - duplicates.Count)

        Dim res As DialogResult = MessageBox.Show(msg, "重複データの確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

        If res = DialogResult.No Then
            Return ' 処理中止
        End If

        ' 重複した人を除外したリストを作成
        Dim dupIds As New HashSet(Of String)(duplicates.Select(Function(s) s.StaffId))
        targetStaffs = selectedStaffs.Where(Function(s) Not dupIds.Contains(s.StaffId)).ToList()

        If targetStaffs.Count = 0 Then
            MessageBox.Show("登録対象者が0名になったため、処理を終了します。")
            Return
        End If
    End If

    ' 3. 一括登録の実行
    Try
        _wageService.ApplyBulkRaiseByAmount(targetStaffs, applyDate, raiseAmount, note)
        MessageBox.Show(targetStaffs.Count & " 名の時給を一括登録しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
        RefreshStaffList() ' 一覧を再読み込み
    Catch ex As Exception
        MessageBox.Show("登録中にエラーが発生したため、すべての変更を取り消しました。" & Environment.NewLine & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Try
End Sub
----------------------------------------------------------------------------------------------------
