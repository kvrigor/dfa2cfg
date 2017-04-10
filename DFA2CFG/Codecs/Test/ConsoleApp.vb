Imports Codecs.Languages.Regular
Imports Codecs.Languages.ContextFree
Imports Codecs.Utils

Module ConsoleApp
    Sub Main()
        Console.OutputEncoding = System.Text.Encoding.UTF8
        'SerDesExamples()
        DFAToCFGExample()
    End Sub

    Private Sub SerDesExamples()
        SerializeDFAExample()
        DeserializeDFAExample()
        SerializeCFGExample()
        DeserializeCFGExample()
    End Sub

    Private Sub DFAToCFGExample()
        Dim dfa1 As DFA = CreateSampleDFA()
        Console.WriteLine(dfa1.ToString())
        Dim cfg1 As CFG = dfa1.ToCFG("DFA_TO_CFG")
        Console.WriteLine(cfg1.ToString())
        Console.ReadKey()
    End Sub

    Private Sub SerializeDFAExample()
        Console.WriteLine("Running SerializeDFAExample()")
        Dim dfa1 As DFA = CreateSampleDFA()
        Console.WriteLine(dfa1.ToString())
        Console.WriteLine(dfa1.IsValid)
        SerializeToXML(dfa1, "dfaTest.xml")
        Process.Start("dfaTest.xml")
        Console.ReadKey()
    End Sub

    Private Sub DeserializeDFAExample()
        Console.WriteLine("Running DeserializeDFAExample()")
        Dim dfa2 As DFA = DeserializeFromXML(Of DFA)("dfaTest.xml")
        Console.WriteLine(dfa2.ToString())
        Console.WriteLine(dfa2.IsValid)
        Console.ReadKey()
    End Sub

    Private Sub SerializeCFGExample()
        Console.WriteLine("Running SerializeCFGExample()")
        Dim cfg1 As CFG = CreateSampleCFG()
        Console.WriteLine(cfg1.ToString())
        Console.WriteLine(cfg1.IsValid)
        SerializeToXML(cfg1, "cfgTest.xml")
        Process.Start("cfgTest.xml")
        Console.ReadKey()
    End Sub

    Private Sub DeserializeCFGExample()
        Console.WriteLine("Running DeserializeCFGExample()")
        Dim cfg2 As CFG = DeserializeFromXML(Of CFG)("cfgTest.xml")
        Console.WriteLine(cfg2.ToString())
        Console.WriteLine(cfg2.IsValid)
        Console.ReadKey()
    End Sub

    Private Function CreateSampleDFA() As DFA
        Dim states() As String = {"q3", "q1", "q2"}
        Dim symbols() As String = {"1", "0"}
        Dim transTable As New List(Of TransFunc)
        transTable.Add(New TransFunc("q3", "0", "q2"))
        transTable.Add(New TransFunc("q1", "0", "q1"))
        transTable.Add(New TransFunc("q2", "0", "q3"))
        transTable.Add(New TransFunc("q1", "1", "q2"))
        transTable.Add(New TransFunc("q3", "1", "q2"))
        transTable.Add(New TransFunc("q2", "1", "q2"))
        Return New DFA(states, symbols, transTable.ToArray(), "q1", {"q2"}, "DFA_Test")
    End Function

    Private Function CreateSampleCFG() As CFG
        Dim variables() As String = {"E", "T", "F"}
        Dim terminals() As String = {"a", "+", "x", "(", ")"}
        Dim rules As New List(Of GrammarRule)
        rules.Add(New GrammarRule("E", {"E + T", "T"}))
        rules.Add(New GrammarRule("T", {"T x F", "F"}))
        rules.Add(New GrammarRule("F", {"E", "a"}))
        Return New CFG(variables, terminals, rules.ToArray(), "E", "CFG_Test")
    End Function
End Module
