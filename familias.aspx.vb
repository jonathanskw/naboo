
Imports System.Data

Partial Class familias
    Inherits System.Web.UI.Page
    Public objDatos As New cls_funciones
    Public ssql As String = ""

    Private Sub familias_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim dtIndicador As New DataTable
        dtIndicador = fnCreaDatatable()
        Dim sHtml As String = ""

        For i = 0 To dtIndicador.Columns.Count - 1 Step 1
            sHtml = sHtml & "<th>" & dtIndicador.Columns(i).ColumnName & "</th>"
        Next

        Dim literal As New LiteralControl(sHtml)
        pnlEncabezado.Controls.Clear()
        pnlEncabezado.Controls.Add(literal)

        sHtml = ""
        For i = 0 To dtIndicador.Rows.Count - 1 Step 1
            sHtml = sHtml & "<tr>"
            For x = 0 To dtIndicador.Columns.Count - 1 Step 1
                sHtml = sHtml & "<td>" & dtIndicador.Rows(i)(x) & "</td>"
            Next
            sHtml = sHtml & "</tr>"

        Next
        literal = New LiteralControl(sHtml)
        pnlLineas.Controls.Clear()
        pnlLineas.Controls.Add(literal)
    End Sub
    Public Function fnCreaDatatable()
        Dim dtDatos As New DataTable

        Dim AñoActual As Int16 = Now.Year - 2000
        Dim AñoAnterior As Int16 = AñoActual - 1

        dtDatos.Columns.Add("Familia")

        For i = 1 To 12
            dtDatos.Columns.Add(fnMes(i) & AñoAnterior)
        Next

        For x = 1 To Now.Date.Month
            dtDatos.Columns.Add(fnMes(x) & AñoActual)
        Next

        Return dtDatos
    End Function

    Public Function fnMes(mes As Int16)
        Dim sMes As String = ""

        If mes = 1 Then
            sMes = "ene"
        End If
        If mes = 2 Then
            sMes = "feb"
        End If
        If mes = 3 Then
            sMes = "mar"
        End If
        If mes = 4 Then
            sMes = "abr"
        End If
        If mes = 5 Then
            sMes = "may"
        End If
        If mes = 6 Then
            sMes = "jun"
        End If
        If mes = 7 Then
            sMes = "jul"
        End If
        If mes = 8 Then
            sMes = "ago"
        End If
        If mes = 9 Then
            sMes = "sep"
        End If
        If mes = 10 Then
            sMes = "oct"
        End If
        If mes = 11 Then
            sMes = "nov"
        End If
        If mes = 12 Then
            sMes = "dic"
        End If

        Return sMes
    End Function

    Public Function fnCargaInfo()
        ssql = "select * from [@familia] where u_maestro ='Productos Pegaduro' and code not like 'Factores%' order by Code "
    End Function
End Class
