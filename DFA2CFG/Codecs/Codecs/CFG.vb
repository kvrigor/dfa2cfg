''' <summary>
''' Data structure for context free grammar (CFG) objects.
''' </summary>
''' <remarks>
''' A context free grammar is a 4-tuple, (V, Σ, R, S), consisting of:
'''  - Finite set of variables [ V ]
'''  - Finite set of terminals disjoint from V [ Σ ] 
'''  - Rules of the grammar (a.k.a productions) [ R: V → (V ∪ Σ)*]
'''  - Start variable [ S ∈ V]
''' </remarks>
Public Class CFG
    Public Sub New()

    End Sub
End Class
