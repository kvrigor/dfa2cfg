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
        ''' </remarks>
        <Extension()>
        Public Function ToCFG(ByVal dfaObj As DFA, Optional ByVal Name As String = "") As CFG
            If dfaObj.IsValid() Then
                Dim rules As New Dictionary(Of String, GrammarRule)
                For Each trans In dfaObj.Transitions()
                    If Not rules.ContainsKey(trans.PrevState) Then
                        rules(trans.PrevState) = New GrammarRule(trans.PrevState, {trans.Input & trans.NextState})
                    Else
                        rules(trans.PrevState).AddSubstitutions(trans.Input & trans.NextState)
                    End If
                    If dfaObj.AcceptStates.ToList.Contains(trans.PrevState) Then
                        If Not rules(trans.PrevState).Substitutions.ToList.Contains("ε") Then
                            rules(trans.PrevState).AddSubstitutions("ε")
                        End If
                    End If
                Next
                Return New CFG(dfaObj.States, dfaObj.InputSymbols, rules.Values.ToArray(), dfaObj.StartState, Name)
            Else
                Throw New ArgumentException("DFA object is not a valid DFA.")
            End If
        End Function
    End Module
End Namespace
