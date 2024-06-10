VERSION 5.00
Begin VB.Form FrmLogin 
   BackColor       =   &H80000002&
   Caption         =   "Login"
   ClientHeight    =   4560
   ClientLeft      =   120
   ClientTop       =   465
   ClientWidth     =   7635
   LinkTopic       =   "Form1"
   ScaleHeight     =   4560
   ScaleWidth      =   7635
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton CmdEnter 
      Caption         =   "Entrar"
      Height          =   495
      Left            =   3240
      TabIndex        =   2
      Top             =   3720
      Width           =   1335
   End
   Begin VB.TextBox TxtPwd 
      Height          =   495
      IMEMode         =   3  'DISABLE
      Left            =   2760
      PasswordChar    =   "*"
      TabIndex        =   1
      Top             =   2640
      Width           =   2175
   End
   Begin VB.TextBox TxtUser 
      Height          =   495
      Left            =   2760
      TabIndex        =   0
      Top             =   1800
      Width           =   2175
   End
End
Attribute VB_Name = "FrmLogin"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub CmdEnter_Click()

    If TxtUser.Text = "admin" And TxtPwd.Text = "admin" Then
        FrmMenu.Show (1)
    Else
        MsgBox ("Deseja salvar as alterações?")
    End If
    
    
End Sub
