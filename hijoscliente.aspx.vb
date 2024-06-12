Imports System.Data
Partial Class hijoscliente
    Inherits System.Web.UI.Page
    Public objDatos As New cls_funciones
    Public ssql As String = ""
    Public sCliente As String
    Public ListaPrecios As String = ""
    Public iTipoLista As Int16 = 15

    Private Sub hijoscliente_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not IsPostBack Then


            If Request.QueryString.Count > 0 Then
                sCliente = Request.QueryString(0)
            End If
            fnGeneralesCliente()
            fnIndicadorClientePivot()
        End If
    End Sub
    Public Sub fnTipoLista(listaPrecios As String)
        If lblListaPrecios.Text.Contains("12") Then
            iTipoLista = 12
        End If

        If lblListaPrecios.Text.Contains("15") Then
            iTipoLista = 15
        End If

        If lblListaPrecios.Text.Contains("6") Then
            iTipoLista = 6
        End If


    End Sub

    Public Sub fnGeneralesCliente()
        Dim sCardCode As String = "PRPISA94"

        If Request.QueryString.Count > 0 Then
            sCardCode = Request.QueryString("cliente")
            ssql = "SELECT CardCode  FROM OCRD where cardname=" & "'" & sCliente & "' and cardType='C' and validFor='Y' and GroupCode in (select GroupCode   from OCRG where GroupType ='C' and GroupName='" & Request.QueryString("ruta") & "')"
            Dim dtCliente As New DataTable
            dtCliente = objDatos.fnEjecutarConsulta(ssql)
            If dtCliente.Rows.Count > 0 Then
                sCardCode = dtCliente.Rows(0)(0)
            End If
        End If
            ssql = "EXEC spGeneralesCliente " & "'" & sCardCode & "'"
        Dim dtGenerales As New DataTable
        dtGenerales = objDatos.fnEjecutarConsulta(ssql)
        If dtGenerales.Rows.Count > 0 Then
            ListaPrecios = dtGenerales.Rows(0)("ListaPrecios")
            lblRazonSocial.Text = dtGenerales.Rows(0)("CardNAme")
            lblListaPrecios.Text = dtGenerales.Rows(0)("ListaPrecios")
            lblCondiciones.Text = dtGenerales.Rows(0)("CondicionesPago")
            lblLimiteCredito.Text = Convert.ToDouble(dtGenerales.Rows(0)("LimiteCredito")).ToString("C2")

            fnTipoLista(lblListaPrecios.Text)
        End If

    End Sub
    Public Function fnCreaDatatable()
        Dim dtDatos As New DataTable

        Dim AñoActual As Int16 = Now.Year - 2000
        Dim AñoAnterior As Int16 = AñoActual - 1

        Dim sOrderby As String
        Dim iMesVencido As Int16

        iMesVencido = Now.Date.Month - 1
        If iMesVencido = 0 Then
            sOrderby = "cv" & fnMes(12) & "Ant desc"
        Else
            sOrderby = "cv" & fnMes(Now.Date.Month - 1) & "act desc"
        End If

        ssql = "Select cvFamilia as Familia, cvHijo As Hijo,cvEneant As EneAnt,cvFebAnt As FebAnt,cvMarAnt As MarAnt,cvAbrAnt As AbrAnt,cvMayAnt As MayAnt, " _
            & " cvJunAnt As JunAnt,cvJulant As JulAnt,cvAgoAnt As AgoAnt,cvSepAnt As SepAnt,cvOctAnt As OctAnt,cvNovant As NovAnt,cvDicant As DicAnt, " _
            & " cvEneAct As EneAct,cvFebAct As FebAct,cvMarAct As MarAct,cvAbrAct As AbrAct,cvMayAct As MayAct, " _
            & " cvJunAct As JunAct,cvJulAct As JulAct,cvAgoAct As AgoAct,cvSepAct As SepAct,cvOctAct As OctAct,cvNovAct As NovAct,cvDicAct As DicAct  " _
            & " from satelite_SAP.indicadores.VentasMesRutaClientesHijos where cvCliente=  " & "'" & sCliente & "' order by " & sOrderby

        dtDatos = objDatos.fnEjecutarConsulta(ssql)

        ''dtDatos.Columns.Add("Hijo")

        ''For i = 1 To 12
        ''    dtDatos.Columns.Add(fnMes(i) & AñoAnterior)
        ''Next

        ''For x = 1 To Now.Date.Month
        ''    dtDatos.Columns.Add(fnMes(x) & AñoActual)
        ''Next

        ''''Ahora los hijos
        ''ssql = "SELECT Hijo FROM (select Hijo  ,sum(toneladas) tons from vw_IP_Ventas where year(fecha)='" & Now.Year & "' and MONTH (fecha)=" & Now.Month - 1 & " and Nombre='" & sCliente & "' group by Hijo) X order by Tons desc "
        ''Dim dtHijos As New DataTable
        ''dtHijos = objDatos.fnEjecutarConsulta(ssql)

        ''For i = 0 To dtHijos.Rows.Count - 1 Step 1
        ''    Dim fila As DataRow

        ''    fila = dtDatos.NewRow
        ''    fila("hijo") = dtHijos.Rows(i)(0)
        ''    For mes = 1 To 12
        ''        fila(fnMes(mes) & AñoAnterior) = fnTonsMesAñoHijoCliente(dtHijos.Rows(i)(0), sCliente, mes, Now.Year - 1).ToString("N2")
        ''    Next

        ''    For mesActual = 1 To Now.Date.Month - 1
        ''        fila(fnMes(mesActual) & AñoActual) = fnTonsMesAñoHijoCliente(dtHijos.Rows(i)(0), sCliente, mesActual, Now.Year).ToString("N2")
        ''    Next

        ''    dtDatos.Rows.Add(fila)

        ''Next


        Return dtDatos
    End Function
    Public Sub fnIndicadorClientePivot()
        Dim dtIndicador As New DataTable
        dtIndicador = fnCreaDatatable()

        Dim AñoActual As Int16 = Now.Year - 2000
        Dim AñoAnterior As Int16 = AñoActual - 1

        Dim sHtml As String = ""

        For i = 0 To dtIndicador.Columns.Count - 1 Step 1
            sHtml = sHtml & "<th>" & dtIndicador.Columns(i).ColumnName.Replace("Ant", AñoAnterior).Replace("Act", AñoActual) & "</th>"
        Next

        Dim literal As New LiteralControl(sHtml)
        pnlEncabezadoHijos.Controls.Clear()
        pnlEncabezadoHijos.Controls.Add(literal)

        sHtml = ""
        For i = 0 To dtIndicador.Rows.Count - 1 Step 1
            sHtml = sHtml & "<tr>"
            For x = 0 To dtIndicador.Columns.Count - 1 Step 1

                If x > 1 Then
                    sHtml = sHtml & "<td>" & CDbl(dtIndicador.Rows(i)(x)).ToString("N2") & "</td>"
                Else

                    sHtml = sHtml & "<td>" & dtIndicador.Rows(i)(x) & "</td>"
                End If


            Next
            sHtml = sHtml & "</tr>"

        Next
        literal = New LiteralControl(sHtml)
        pnlLineasHijos.Controls.Clear()
        pnlLineasHijos.Controls.Add(literal)
    End Sub
    Public Function fnTonsMesAñoHijoCliente(Hijo As String, cliente As String, Mes As Int16, Año As Int16) As Double

        Dim Tons As Double = 0

        ssql = "SELECT ISNULL(sum(toneladas),0) tons from vw_IP_Ventas where year(fecha)='" & Año & "' and MONTH (fecha)=" & Mes & " and Nombre = " & "'" & cliente & "' and hijo=" & "'" & Hijo & "'"
        Dim dttons As New DataTable
        dttons = objDatos.fnEjecutarConsulta(ssql)

        If dttons.Rows.Count > 0 Then
            Tons = dttons.Rows(0)(0)
        End If


        Return Tons

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

    Private Sub btnEstrategia_Click(sender As Object, e As EventArgs) Handles btnEstrategia.Click
        If Request.QueryString.Count > 0 Then
            sCliente = Request.QueryString(0)
        End If
        ssql = "SELECT CardCode  FROM OCRD where cardname=" & "'" & sCliente & "' and cardType='C' and validFor='Y' and GroupCode in (select GroupCode   from OCRG where GroupType ='C' and GroupName='" & Request.QueryString("ruta") & "')"
        Dim dtCliente As New DataTable
        dtCliente = objDatos.fnEjecutarConsulta(ssql)
        If dtCliente.Rows.Count > 0 Then
            Response.Redirect("tactica.aspx?cliente=" & dtCliente.Rows(0)(0))
        End If

    End Sub
End Class
