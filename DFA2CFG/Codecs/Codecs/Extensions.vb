Imports System.Runtime.CompilerServices
Imports Codecs.Languages.Regular
Imports Codecs.Languages.ContextFree

Namespace Utils
    Public Module DFAExtensions

        ''' <summary>
        ''' Converts a DFA object to its equivalent CFG representation.
        ''' </summary>
        ''' <param name="dfaObj">Valid DFA object to convert.</param>
        ''' <param name="Name">CFG name.</param>
        ''' <returns></returns>
        ''' <remarks>
        ''' rpkv - This whole method is a hack. Arrays are not the best collection structure to use with TransFunc 
        ''' and GrammarRule types as we can't easily perform reordering of the elements when necessary. We'll do
        ''' the optimizations later.
        ''' </remarks>
        <Extension()>
        Public Function ToCFG(ByVal dfaObj As DFA, Optional ByVal Name As String = "") As CFG
            If dfaObj.IsValid() Then
                Dim otherRules As New Dictionary(Of String, GrammarRule)
                Dim startRule As GrammarRule = Nothing

                For Each trans In dfaObj.Transitions()
                    If trans.PrevState = dfaObj.StartState Then
                        If startRule Is Nothing Then
                            startRule = New GrammarRule(dfaObj.StartState, {trans.Input & trans.NextState})
                        Else
                            startRule.AddSubstitutions(trans.Input & trans.NextState)
                        End If
                    Else
                        If Not otherRules.ContainsKey(trans.PrevState) Then
                            otherRules(trans.PrevState) = New GrammarRule(trans.PrevState, {trans.Input & trans.NextState})
                        Else
                            otherRules(trans.PrevState).AddSubstitutions(trans.Input & trans.NextState)
                        End If
                    End If
                Next
                Dim rules As New List(Of GrammarRule)
                rules.Add(startRule) 'a separate GrammarRule object for the start rule was created so that it can be added first!
                rules.AddRange(otherRules.Select(Function(ByVal kvp As KeyValuePair(Of String, GrammarRule)) kvp.Value))
                Return New CFG(dfaObj.States, dfaObj.InputSymbols, rules.ToArray(), dfaObj.StartState, Name)
            Else
                Throw New ArgumentException("DFA object is not a valid DFA.")
            End If
        End Function
    End Module
End Namespace
