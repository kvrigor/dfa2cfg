Imports Codecs.Utils.LanguageHelpers
Imports System.Xml.Serialization

Namespace Languages.Regular
    ''' <summary>
    ''' Data structure for creating deterministic finite automaton (DFA) objects.
    ''' </summary>
    ''' <remarks>
    ''' A deterministic finite automaton M is a 5-tuple, (Q, Σ, δ, q0, F), consisting of:
    '''  - Finite set of states [ Q ]
    '''  - Finite set of input symbols called the alphabet [ Σ ] 
    '''  - Transition function [ 𝛿: Q x Σ → Q ]
    '''  - Start state [ q0 ∈ Q ]
    '''  - Set of accept states [ F ⊆ Q ]
    ''' </remarks>
    <Serializable()>
    Public Class DFA
        Implements ICloneable

        Protected _states As List(Of String)
        Protected _inputSymbols As List(Of String)
        Protected _transitions As List(Of TransFunc)
        Protected _acceptStates As List(Of String)

#Region "Properties"
        ''' <summary>
        ''' Gets/sets the name of the DFA.
        ''' </summary>
        ''' <returns></returns>
        <XmlAttribute()>
        Public Property Name As String

        ''' <summary>
        ''' Returns a string array containing all states of the DFA.
        ''' </summary>
        ''' <returns></returns>
        <XmlArrayItem(ElementName:="state", IsNullable:=True)>
        Public Property States As String()
            Get
                _states.Sort()
                Return _states.ToArray()
            End Get
            Set(value As String())
                _states = value.ToList()
            End Set
        End Property

        ''' <summary>
        ''' Returns a string array of input symbols.
        ''' </summary>
        ''' <returns></returns>
        <XmlArrayItem(ElementName:="symbol", IsNullable:=True)>
        Public Property InputSymbols As String()
            Get
                _inputSymbols.Sort()
                Return _inputSymbols.ToArray()
            End Get
            Set(value As String())
                _inputSymbols = value.ToList()
            End Set
        End Property

        ''' <summary>
        ''' Gets/sets the start state.
        ''' </summary>
        <XmlElement(IsNullable:=True)>
        Public Property StartState As String

        ''' <summary>
        ''' Returns a string array of accept states.
        ''' </summary>
        ''' <returns></returns>
        <XmlArrayItem(ElementName:="state", IsNullable:=True)>
        Public Property AcceptStates As String()
            Get
                _acceptStates.Sort()
                Return _acceptStates.ToArray()
            End Get
            Set(value As String())
                _acceptStates = value.ToList()
            End Set
        End Property

        ''' <summary>
        ''' Returns an array containing all transition functions.
        ''' </summary>
        ''' <returns></returns>
        <XmlArray(IsNullable:=True)>
        Public Property Transitions As TransFunc()
            Get
                _transitions.Sort()
                Return _transitions.ToArray()
            End Get
            Set(value As TransFunc())
                _transitions = value.ToList()
            End Set
        End Property
#End Region

#Region "Constructors"
        ''' <summary>
        ''' Initializes an empty DFA.
        ''' </summary>
        Public Sub New()
            _states = New List(Of String)
            _inputSymbols = New List(Of String)
            _transitions = New List(Of TransFunc)
            _StartState = String.Empty
            _acceptStates = New List(Of String)
            _Name = String.Empty
        End Sub

        ''' <summary>
        ''' Initializes a DFA with the specified parameters.
        ''' </summary>
        ''' <param name="Q">String array of all DFA states.</param>
        ''' <param name="sigma">String array of input symbols.</param>
        ''' <param name="delta">Array of transition functions.</param>
        ''' <param name="q0">Start state.</param>
        ''' <param name="F">String array of accept states.</param>
        ''' <param name="name">Name identifying the DFA.</param>
        Public Sub New(ByVal Q As String(), ByVal sigma As String(), ByVal delta As TransFunc(), ByVal q0 As String, ByVal F As String(), Optional ByVal name As String = "")
            _states = Q.ToList()
            _inputSymbols = sigma.ToList()
            _transitions = delta.ToList()
            _StartState = q0
            _acceptStates = F.ToList()
            _Name = name
        End Sub

        ''' <summary>
        ''' Initializes a DFA from an existing DFA object.
        ''' </summary>
        ''' <param name="dfa">Existing DFA object.</param>
        Public Sub New(ByVal dfa As DFA)
            _states = dfa.States.ToList()
            _inputSymbols = dfa.InputSymbols.ToList()
            _transitions = dfa.Transitions.ToList()
            _StartState = dfa.StartState
            _acceptStates = dfa.AcceptStates.ToList()
            _Name = dfa.Name
        End Sub
#End Region

#Region "Core functions"
        ''' <summary>
        ''' Checks whether the DFA is valid or not. A DFA is considered valid if it meets the following conditions: 
        '''   1) Q is not null
        '''   2) Start state is a member of Q (q0 ∈ Q)
        '''   3) Set of accept states is a subset of Q (F ⊆ Q) 
        '''   4) Has exactly one state for each possible combination of a state and an input symbol (Q x Σ → Q)
        ''' </summary>
        ''' <returns></returns>
        Public Overridable ReadOnly Property IsValid() As Boolean
            Get
                If IsEmptySet(_states) OrElse IsEmptyStringSet(_states) Then Return False
                If Not _states.Contains(_StartState) Then Return False
                If Not IsEmptySet(_acceptStates) Then
                    For Each acceptState In _acceptStates
                        If Not _states.Contains(acceptState) Then
                            Return False
                        End If
                    Next
                End If
                If _inputSymbols.Count = 0 Then Return False
                For Each state In _states
                    Dim maxcount As Integer = _inputSymbols.Count
                    For Each _transFunc As TransFunc In _transitions
                        If _transFunc.PrevState = state Then
                            maxcount = maxcount - 1
                        End If
                    Next
                    If _states.Count > 1 AndAlso maxcount <> 0 Then Return False
                Next
                Return True
            End Get
        End Property

        ''' <summary>
        ''' Adds a new transition function to the transition table.
        ''' </summary>
        ''' <param name="tf">The new TransFunc object to be added.</param>
        Public Overridable Sub AddTransitions(ByVal tf As TransFunc)
            If _transitions.Contains(tf) Then
                Throw New ArgumentException($"The transition table already contains {tf.ToString()}.")
            Else
                _transitions.Add(tf)
            End If
        End Sub

        ''' <summary>
        ''' Add new transition functions to the transition table.
        ''' </summary>
        ''' <param name="tfArray">Array of new TransFunc objects to be added.</param>
        Public Overridable Sub AddTransitions(ByVal tfArray() As TransFunc)
            For Each tf In tfArray
                AddTransitions(tf)
            Next
        End Sub

        ''' <summary>
        ''' Adds a new state to the set of states.
        ''' </summary>
        ''' <param name="s">The new state to be added.</param>
        Public Overridable Sub AddStates(ByVal s As String)
            If _states.Contains(s) Then
                Throw New ArgumentException($"The set of states already contains '{s}'.")
            Else
                _states.Add(s)
            End If
        End Sub

        ''' <summary>
        ''' Adds new states to the set of states.
        ''' </summary>
        ''' <param name="sArray">String array of the new states to be added.</param>
        Public Overridable Sub AddStates(ByVal sArray() As String)
            For Each s In sArray
                AddStates(s)
            Next
        End Sub

        ''' <summary>
        ''' Adds a new input symbol to the set of input symbols.
        ''' </summary>
        ''' <param name="s">The new input symbol to be added.</param>
        Public Overridable Sub AddInputSymbols(ByVal s As String)
            If _inputSymbols.Contains(s) Then
                Throw New ArgumentException($"The set of input symbols already contains '{s}'.")
            Else
                _inputSymbols.Add(s)
            End If
        End Sub

        ''' <summary>
        ''' Adds new input symbols to the set of input symbols.
        ''' </summary>
        ''' <param name="sArray">String array of the new input symbols to be added.</param>
        Public Overridable Sub AddInputSymbols(ByVal sArray() As String)
            For Each s In sArray
                AddInputSymbols(s)
            Next
        End Sub

        ''' <summary>
        ''' Adds a new final state to the set of final states.
        ''' </summary>
        ''' <param name="s">The new final state to be added.</param>
        Public Overridable Sub AddFinalStates(ByVal s As String)
            If _acceptStates.Contains(s) Then
                Throw New ArgumentException($"The set of final states already contains '{s}'.")
            Else
                _acceptStates.Add(s)
            End If
        End Sub

        ''' <summary>
        ''' Adds new final states to the set of final states.
        ''' </summary>
        ''' <param name="sArray">String array of the new input symbols to be added.</param>
        Public Overridable Sub AddFinalStates(ByVal sArray() As String)
            For Each s In sArray
                AddFinalStates(s)
            Next
        End Sub
#End Region

#Region "Object helper functions"
        Public Overrides Function ToString() As String
            Return $"DFA Name: {_Name}" & vbCrLf &
               $"    Q = {{{String.Join(", ", States)}}}" & vbCrLf &
               $"    Σ = {{{String.Join(", ", InputSymbols)}}}" & vbCrLf &
               $"   q0 = {_StartState}" & vbCrLf &
               $"    F = {{{String.Join(", ", AcceptStates)}}}" & vbCrLf &
               "    𝛿:" & vbCrLf & PrintTransitionTable()
        End Function

        Public Function Clone() As Object Implements ICloneable.Clone
            Return New DFA(States, InputSymbols, Transitions, StartState, AcceptStates)
        End Function
#End Region

#Region "Private helper functions"
        Protected Function PrintTransitionTable() As String
            'TODO: Display transition functions in tabular format 
            Return ""
        End Function
#End Region
    End Class

    ''' <summary>
    '''  Data structure for creating transition functions.
    ''' </summary>
    <Serializable()>
    Public Class TransFunc
        Implements ICloneable
        Implements IComparable(Of TransFunc)

        <XmlAttribute()>
        Public Property PrevState As String

        <XmlAttribute()>
        Public Property Input As String

        <XmlAttribute()>
        Public Property NextState As String

        Public Sub New()
            PrevState = Nothing
            Input = Nothing
            NextState = Nothing
        End Sub

        Public Sub New(ByVal previousState As String, input As String, nextState As String)
            _PrevState = previousState
            _Input = input
            _NextState = nextState
        End Sub

        Public Overrides Function Equals(obj As Object) As Boolean
            If TypeOf obj Is TransFunc Then
                Dim tfObj As TransFunc = CType(obj, TransFunc)
                Return Me.PrevState = tfObj.PrevState AndAlso Me.Input = tfObj.Input AndAlso Me.NextState = tfObj.NextState
            Else
                Return False
            End If
        End Function

        Public Overrides Function ToString() As String
            Return $"𝛿({PrevState}, {Input}) = {NextState}"
        End Function

        Public Function Clone() As Object Implements ICloneable.Clone
            Return New TransFunc(_PrevState, _Input, _NextState)
        End Function

        Public Function CompareTo(other As TransFunc) As Integer Implements IComparable(Of TransFunc).CompareTo
            If other Is Nothing Then
                Return 1
            Else
                Return Me.PrevState.CompareTo(other.PrevState)
            End If
        End Function
    End Class
End Namespace
