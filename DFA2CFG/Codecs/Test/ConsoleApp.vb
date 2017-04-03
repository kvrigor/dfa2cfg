Imports Codecs.Languages.Regular

Module ConsoleApp
    Sub Main()
        Dim dfa1 As DFA = CreateDFASample()

        Console.WriteLine(dfa1.ToString())
        Console.ReadKey()
    End Sub

    Private Function CreateDFASample() As DFA
        Dim states() As String = {"q1", "q2", "q3"}
        Dim symbols() As String = {"0", "1"}
        Dim transTable As New List(Of TransFunc)
        transTable.Add(New TransFunc("q1", "0", "q1"))
        transTable.Add(New TransFunc("q1", "1", "q2"))
        transTable.Add(New TransFunc("q2", "0", "q3"))
        transTable.Add(New TransFunc("q2", "1", "q2"))
        transTable.Add(New TransFunc("q3", "0", "q2"))
        transTable.Add(New TransFunc("q3", "1", "q2"))
        Return New DFA(states, symbols, transTable.ToArray(), "q1", {"q2"}, "Sample")
    End Function

End Module
