Module Utils
    Public Const EMPTY_SET = "∅"
    Public Const EMPTY_STRING = "ε"

    Public Function IsEmptySet(ByVal setObj As IEnumerable(Of String)) As Boolean
        Return setObj Is Nothing OrElse setObj.Count = 0
    End Function

    Public Function IsEmptyStringSet(ByVal setObj As IEnumerable(Of String)) As Boolean
        If IsEmptySet(setObj) Then
            Return False
        Else
            For Each element In setObj
                If Not String.IsNullOrEmpty(element) AndAlso element <> EMPTY_STRING Then
                    Return False
                End If
            Next
            Return True
        End If
    End Function
End Module
