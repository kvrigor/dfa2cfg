Imports Codecs.Utils.LanguageHelpers
Imports System.Xml.Serialization

Namespace Languages.ContextFree
    ''' <summary>
    ''' Data structure for creating context free grammar (CFG) objects.
    ''' </summary>
    ''' <remarks>
    ''' A context free grammar is a 4-tuple, (V, Σ, R, S), consisting of:
    '''  - Finite set of variables [ V ]
    '''  - Finite set of terminals disjoint from V [ Σ ] 
    '''  - Rules of the grammar (a.k.a productions) [ R: V → (V ∪ Σ)* ]
    '''  - Start variable [ S ∈ V ]
    ''' </remarks>
    <Serializable()>
    Public Class CFG
        Implements ICloneable

        Private _variables As List(Of String)
        Private _terminals As List(Of String)
        Private _grammarRules As List(Of GrammarRule)

#Region "Properties"
        ''' <summary>
        ''' Gets/sets the name of the CFG.
        ''' </summary>
        ''' <returns></returns>
        <XmlAttribute()>
        Public Property Name As String

        ''' <summary>
        ''' Returns a string array of variables.
        ''' </summary>
        ''' <returns></returns>
        <XmlArrayItem(ElementName:="var", IsNullable:=True)>
        Public Property Variables As String()
            Get
                _variables.Sort()
                Return _variables.ToArray()
            End Get
            Set(value As String())
                _variables = value.ToList()
            End Set
        End Property

        ''' <summary>
        ''' Gets/sets the start variable.
        ''' </summary>
        <XmlElement(IsNullable:=True)>
        Public Property StartVariable As String

        ''' <summary>
        ''' Returns a string array of terminals.
        ''' </summary>
        ''' <returns></returns>
        <XmlArrayItem(ElementName:="symbol", IsNullable:=True)>
        Public Property Terminals As String()
            Get
                _terminals.Sort()
                Return _terminals.ToArray()
            End Get
            Set(value As String())
                _terminals = value.ToList()
            End Set
        End Property

        ''' <summary>
        ''' Returns an array of grammar rules.
        ''' </summary>
        ''' <returns></returns>
        <XmlArrayItem(ElementName:="rule", IsNullable:=True)>
        Public Property GrammarRules As GrammarRule()
            Get
                SortGrammarRules()
                Return _grammarRules.ToArray()
            End Get
            Set(value As GrammarRule())
                _grammarRules = value.ToList()
            End Set
        End Property

#End Region

#Region "Constructors"
        ''' <summary>
        ''' Initializes an empty CFG.
        ''' </summary>
        Public Sub New()
            _variables = New List(Of String)
            _terminals = New List(Of String)
            _grammarRules = New List(Of GrammarRule)
            _StartVariable = String.Empty
            _Name = String.Empty
        End Sub

        ''' <summary>
        ''' Initializes a CFG with the specified parameters.
        ''' </summary>
        ''' <param name="V">String array of variables.</param>
        ''' <param name="sigma">String array of terminals.</param>
        ''' <param name="R">Array of grammar rules.</param>
        ''' <param name="S">Start variable.</param>
        ''' <param name="name">Name identifying the CFG.</param>
        Public Sub New(ByVal V As String(), ByVal sigma As String(), ByVal R As GrammarRule(), ByVal S As String, Optional ByVal name As String = "")
            _variables = V.ToList()
            _terminals = sigma.ToList()
            _grammarRules = R.ToList()
            _StartVariable = S
            _Name = name
        End Sub

        ''' <summary>
        ''' Initializes a CFG from an existing CFG object.
        ''' </summary>
        ''' <param name="cfg"></param>
        Public Sub New(ByVal cfg As CFG)
            _variables = cfg.Variables.ToList()
            _terminals = cfg.Terminals.ToList()
            _grammarRules = cfg.GrammarRules.ToList()
            _StartVariable = cfg.StartVariable
            _Name = cfg.Name
        End Sub
#End Region

#Region "Core functions"
        ''' <summary>
        ''' Checks whether:
        '''    1) V, Σ, and R are not null.
        '''    2) Start variable is a member of V
        ''' </summary>
        ''' <returns></returns>
        Public Overridable ReadOnly Property IsValid() As Boolean
            Get
                If IsEmptySet(_variables) OrElse IsEmptyStringSet(_variables) Then Return False
                If IsEmptySet(_terminals) OrElse IsEmptyStringSet(_terminals) Then Return False
                If IsNothing(_grammarRules) OrElse IsNothing(_grammarRules) Then Return False
                If Not _variables.Contains(_StartVariable) Then Return False
                Return True
            End Get
        End Property

        ''' <summary>
        ''' Adds a new variable to the set of variables.
        ''' </summary>
        ''' <param name="s">The new variable to be added.</param>
        Public Sub AddVariables(ByVal s As String)
            If _variables.Contains(s) Then
                Throw New ArgumentException($"The set of variables already contains '{s}'.")
            Else
                _variables.Add(s)
            End If
        End Sub

        ''' <summary>
        ''' Adds new variables to the set of variables.
        ''' </summary>
        ''' <param name="sArray">String array of the new variables to be added.</param>
        Public Sub AddVariables(ByVal sArray() As String)
            For Each s In sArray
                AddVariables(s)
            Next
        End Sub

        ''' <summary>
        ''' Adds a new terminal to the set of terminals.
        ''' </summary>
        ''' <param name="s">The new terminal to be added.</param>
        Public Sub AddTerminals(ByVal s As String)
            If _terminals.Contains(s) Then
                Throw New ArgumentException($"The set of terminals already contains '{s}'.")
            Else
                _terminals.Add(s)
            End If
        End Sub

        ''' <summary>
        ''' Adds new terminals to the set of terminals.
        ''' </summary>
        ''' <param name="sArray">String array of the new terminals to be added.</param>
        Public Sub AddTerminals(ByVal sArray() As String)
            For Each s In sArray
                AddTerminals(s)
            Next
        End Sub

        ''' <summary>
        ''' Adds a new grammar rule to the set of grammar rules.
        ''' </summary>
        ''' <param name="gr">The new GrammarRule object to be added.</param>
        Public Sub AddGrammarRules(ByVal gr As GrammarRule)
            For Each g In _grammarRules
                If g.Variable = gr.Variable Then
                    Throw New ArgumentException($"The grammar rules already contain the rule for variable {gr.Variable}.")
                End If
            Next
            _grammarRules.Add(gr)
        End Sub

        ''' <summary>
        ''' Adds new grammar rules to the set of grammar rules.
        ''' </summary>
        ''' <param name="grArray">Array of new GrammarRule objects to be added.</param>
        Public Sub AddGrammarRules(ByVal grArray() As GrammarRule)
            For Each gr In grArray
                AddGrammarRules(gr)
            Next
        End Sub
#End Region

#Region "Object helper functions"
        Public Overrides Function ToString() As String
            Return $"Equivalent CFG" & vbCrLf &
               $"    V = {{{String.Join(", ", Variables)}}}" & vbCrLf &
               $"    Σ = {{{String.Join(", ", Terminals)}}}" & vbCrLf &
               $"    S = {_StartVariable}" & vbCrLf &
               "    R:" & vbCrLf & vbTab & String.Join(Of GrammarRule)(vbCrLf & vbTab, GrammarRules)
        End Function

        Public Function Clone() As Object Implements ICloneable.Clone
            Return New CFG(Variables, Terminals, GrammarRules, _StartVariable, _Name)
        End Function
#End Region

#Region "Private helper functions"
        ''' <summary>
        ''' Ensures start variable is located at the first element, 
        ''' then the rest are sorted alphabetically.
        ''' </summary>
        Private Sub SortGrammarRules()
            _grammarRules.Sort()
            Dim startRule As GrammarRule = _grammarRules.SingleOrDefault(Function(ByVal gr As GrammarRule) gr.Variable = _StartVariable)
            If startRule IsNot Nothing Then
                _grammarRules.Remove(startRule)
                _grammarRules.Insert(0, startRule)
            End If
        End Sub
#End Region
    End Class

    ''' <summary>
    ''' Data structure for creating grammar rules.
    ''' </summary>
    <Serializable()>
    Public Class GrammarRule
        Implements ICloneable
        Implements IComparable(Of GrammarRule)

        Private _substitutions As List(Of String)

        <XmlAttribute()>
        Public Property Variable As String
        <XmlElement(ElementName:="sub")>
        Public Property Substitutions As String()
            Get
                Return _substitutions.ToArray()
            End Get
            Set(value As String())
                _substitutions = value.ToList()
            End Set
        End Property

        ''' <summary>
        ''' Creates an empty GrammarRule object.
        ''' </summary>
        Public Sub New()
            _Variable = Nothing
            _substitutions = Nothing
        End Sub

        ''' <summary>
        ''' Creates a GrammarRule object with the specified parameters.
        ''' </summary>
        ''' <param name="V">Variable.</param>
        ''' <param name="sList">Array of substitution rules.</param>
        Public Sub New(ByVal V As String, ByVal sList As String())
            _Variable = V
            _substitutions = sList.ToList()
        End Sub

        ''' <summary>
        ''' Adds a substitution rule.
        ''' </summary>
        ''' <param name="s"></param>
        Public Sub AddSubstitutions(ByVal s As String)
            If _substitutions.Contains(s) Then
                Throw New ArgumentException($"Variable '{_Variable}' already contains the substitution '{s}'.")
            Else
                _substitutions.Add(s)
            End If
        End Sub

        ''' <summary>
        ''' Adds an array of substitution rules.
        ''' </summary>
        ''' <param name="sArray"></param>
        Public Sub AddSubstitutions(ByVal sArray() As String)
            For Each s In sArray
                AddSubstitutions(s)
            Next
        End Sub

        Public Overrides Function ToString() As String
            Return $"{_Variable} → {String.Join(" | ", _substitutions)}"
        End Function

        Public Function Clone() As Object Implements ICloneable.Clone
            Return New GrammarRule(_Variable, _substitutions.ToArray())
        End Function

        Public Function CompareTo(other As GrammarRule) As Integer Implements IComparable(Of GrammarRule).CompareTo
            If other Is Nothing Then
                Return 1
            Else
                Return Me.Variable.CompareTo(other.Variable)
            End If
        End Function
    End Class

End Namespace