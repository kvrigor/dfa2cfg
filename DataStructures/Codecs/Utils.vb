Imports System.IO
Imports System.Xml.Serialization

Namespace Utils
    Public Module LanguageHelpers
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

    Public Module SerDes
        ''' <summary>
        ''' Serializes an object to a XML file.
        ''' </summary>
        ''' <typeparam name="T">Object type that supports XML serialization.</typeparam>
        ''' <param name="obj">Instance of the object to serialize.</param>
        ''' <param name="xmlFile">XML file where the serialized output will be written into.</param>
        Public Sub SerializeToXML(Of T)(ByVal obj As T, ByVal xmlFile As String)
            'hack to remove namespace definition at root element
            Dim ns As New XmlSerializerNamespaces()
            ns.Add("", "")

            Dim xs As XmlSerializer = New XmlSerializer(GetType(T))
            Using writer As TextWriter = New StreamWriter(xmlFile)
                xs.Serialize(writer, obj, ns)
                writer.Close()
            End Using
        End Sub

        ''' <summary>
        ''' Deserializes an XML file to the desired object type.
        ''' </summary>
        ''' <typeparam name="T">Object type that supports XML serialization.</typeparam>
        ''' <param name="xmlFile">XML file to deserialize.</param>
        ''' <returns>Instance of the deserialized object.</returns>
        Public Function DeserializeFromXML(Of T)(ByVal xmlFile As String) As T
            Dim fs As New FileStream(xmlFile, FileMode.Open)
            Dim xs As XmlSerializer = New XmlSerializer(GetType(T))
            Return CType(xs.Deserialize(fs), T)
        End Function
    End Module

End Namespace