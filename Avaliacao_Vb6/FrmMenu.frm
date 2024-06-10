VERSION 5.00
Object = "{5E9E78A0-531B-11CF-91F6-C2863C385E30}#1.0#0"; "MSFLXGRD.OCX"
Object = "{EAB22AC0-30C1-11CF-A7EB-0000C05BAE0B}#1.1#0"; "ieframe.dll"
Begin VB.Form FrmMenu 
   BackColor       =   &H80000002&
   Caption         =   "Menu"
   ClientHeight    =   9735
   ClientLeft      =   225
   ClientTop       =   870
   ClientWidth     =   19425
   LinkTopic       =   "Form1"
   ScaleHeight     =   9735
   ScaleWidth      =   19425
   StartUpPosition =   3  'Windows Default
   Begin SHDocVwCtl.WebBrowser WebBrowser1 
      Height          =   8295
      Left            =   11160
      TabIndex        =   1
      Top             =   840
      Width           =   8175
      ExtentX         =   14420
      ExtentY         =   14631
      ViewMode        =   0
      Offline         =   0
      Silent          =   0
      RegisterAsBrowser=   0
      RegisterAsDropTarget=   1
      AutoArrange     =   0   'False
      NoClientEdge    =   0   'False
      AlignLeft       =   0   'False
      NoWebView       =   0   'False
      HideFileNames   =   0   'False
      SingleClick     =   0   'False
      SingleSelection =   0   'False
      NoFolders       =   0   'False
      Transparent     =   0   'False
      ViewID          =   "{0057D0E0-3573-11CF-AE69-08002B2E1262}"
      Location        =   "http:///"
   End
   Begin MSFlexGridLib.MSFlexGrid MSFlexGrid1 
      Height          =   8295
      Left            =   120
      TabIndex        =   0
      Top             =   840
      Width           =   10935
      _ExtentX        =   19288
      _ExtentY        =   14631
      _Version        =   393216
      Cols            =   7
   End
   Begin VB.Menu Rel 
      Caption         =   "Relatório"
   End
   Begin VB.Menu api 
      Caption         =   "API"
   End
   Begin VB.Menu sair 
      Caption         =   "Sair"
   End
End
Attribute VB_Name = "FrmMenu"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub api_Click()
LoadJSON GetRandomUsers(10)

End Sub

Private Function GenerateHTML(json As String) As String
    Dim html As String
    html = "<html><body><pre>" & json & "</pre></body></html>"
    GenerateHTML = html
End Function
Private Sub Sleep(milliseconds As Long)
    Dim endTime As Long
    endTime = Timer + (milliseconds / 1000)
    Do While Timer < endTime
        DoEvents
    Loop
End Sub

Private Sub LoadJSON(json As String)
    Dim html As String
    html = GenerateHTML(json)
    WebBrowser1.navigate "about:blank"
    WebBrowser1.document.Open
    Sleep 2000
    WebBrowser1.document.Write html
    Sleep 2000
    WebBrowser1.document.Close
End Sub

Private Sub ShowJSONInWebBrowser()
    Dim json As String
    ' Suponha que 'json' contenha o JSON retornado da API
    json = "{""example"": ""json""}"
    LoadJSON json
End Sub


Function GetRandomUsers(count As Integer) As String
    Dim xmlHttp As Object
    Dim responseText As String
    
    Set xmlHttp = CreateObject("MSXML2.ServerXMLHTTP.6.0")
    
    xmlHttp.Open "GET", "https://randomuser.me/api/?results=" & count, False
    xmlHttp.send
    
    If xmlHttp.Status = 200 Then ' Verifica se a solicitação foi bem-sucedida
        responseText = xmlHttp.responseText
    Else
        MsgBox "Erro ao recuperar os dados: " & xmlHttp.Status & " - " & xmlHttp.statusText
    End If
    
    GetRandomUsers = FormatJson(responseText)
End Function

Private Function AFormatJson(jsonString As String) As String
    Dim sc As New ScriptControl
    sc.Language = "JScript"

    ' Carrega o JSON como um objeto
    Dim jsonObj As Object
    Set jsonObj = sc.Eval("(" & jsonString & ")")

    ' Serializa o objeto de volta para JSON formatado
    Dim formattedJson As String
    formattedJson = sc.Run("JSON.stringify", jsonObj, Nothing, 4)

    FormatJson = formattedJson
End Function

Function FormatJson(json As String, Optional indent As String = "    ") As String
    Dim level As Integer
    Dim position As Integer
    Dim length As Integer
    Dim currentChar As String
    Dim result As String
    
    length = Len(json)
    currentChar = ""
    level = 0
    position = 1
    
    Do While position <= length
        currentChar = Mid(json, position, 1)
        If currentChar = "{" Or currentChar = "[" Then
            result = result & currentChar & vbCrLf & String(level + 1, indent)
            level = level + 1
        ElseIf currentChar = "}" Or currentChar = "]" Then
            result = result & vbCrLf & String(level, indent) & currentChar
            level = level - 1
        ElseIf currentChar = "," Then
            result = result & currentChar & vbCrLf & String(level, indent)
        Else
            result = result & currentChar
        End If
        position = position + 1
    Loop
    
    FormatJson = result
End Function

Private Sub Rel_Click()
    Dim cn As ADODB.Connection
    Set cn = New ADODB.Connection
    Dim strSQL As String
    Dim row As Integer
    
    cn.ConnectionString = "DSN=PostgreSQL35W;SERVER=localhost;port=5432;DATABASE=postgres;UID=postgres;PWD=76577614"
    cn.ConnectionTimeout = 10
    cn.Open

    Set rs = New ADODB.Recordset
    strSQL = "SELECT * FROM USERS ORDER BY ID;"
    
    rs.Open strSQL, cn
    
    MSFlexGrid1.row = 0
    MSFlexGrid1.ColWidth(0) = 100
    MSFlexGrid1.Col = 1
    MSFlexGrid1.Text = "Id"
    MSFlexGrid1.ColWidth(1) = 1000
    MSFlexGrid1.Col = 2
    MSFlexGrid1.Text = "Name"
    MSFlexGrid1.ColWidth(2) = 1500
    MSFlexGrid1.Col = 3
    MSFlexGrid1.Text = "E-mail"
    MSFlexGrid1.ColWidth(3) = 3000
    MSFlexGrid1.Col = 4
    MSFlexGrid1.Text = "Gender"
    MSFlexGrid1.ColWidth(4) = 1000
    MSFlexGrid1.Col = 5
    MSFlexGrid1.Text = "Phone"
    MSFlexGrid1.ColWidth(5) = 2000
    MSFlexGrid1.Col = 6
    MSFlexGrid1.Text = "Cell"
    MSFlexGrid1.ColWidth(6) = 2000
    
    row = 1
    MSFlexGrid1.Rows = 2
    Do While Not rs.EOF
        MSFlexGrid1.Rows = row + 1
        MSFlexGrid1.row = row
        MSFlexGrid1.Col = 1
        MSFlexGrid1.Text = rs("Id")
        MSFlexGrid1.Col = 2
        MSFlexGrid1.Text = rs("name_firstname")
        MSFlexGrid1.Col = 3
        MSFlexGrid1.Text = rs("Email")
        MSFlexGrid1.Col = 4
        MSFlexGrid1.Text = rs("Gender")
        MSFlexGrid1.Col = 5
        MSFlexGrid1.Text = rs("Phone")
        MSFlexGrid1.Col = 6
        MSFlexGrid1.Text = rs("Cell")
        
        rs.MoveNext
        
        row = row + 1
    Loop
    

    rs.Close
    cn.Close
End Sub

Private Sub sair_Click()
End
End Sub
