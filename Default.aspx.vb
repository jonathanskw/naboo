
Imports System.Data
Imports System.Web.Services

Partial Class _Default
    Inherits System.Web.UI.Page

    Public objDatos As New cls_funciones
    Public ssql As String = ""
    Private Sub _Default_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            fnCargaEntidades()
            'fnIndicadorRuta("GDL Tlaquepaque - LV")

        End If
    End Sub

    Public Sub fnIndicadorRuta(Ruta As String)
        Dim AñoActual As Int16 = Now.Year
        Dim AñoAnterior As Int16 = AñoActual - 1
        Dim AñoPasado As Int16 = AñoAnterior - 1

        Dim dtIndicador As New DataTable
        dtIndicador = fnEstructuraDT(Ruta)




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

    Public Sub fnIndicadorRutaAcum(Ruta As String)
        Dim AñoActual As Int16 = Now.Year
        Dim AñoAnterior As Int16 = AñoActual - 1
        Dim AñoPasado As Int16 = AñoAnterior - 1

        Dim dtIndicador As New DataTable
        dtIndicador = fnEstructuraRutaAcumDT(Ruta)




        Dim sHtml As String = ""

        For i = 0 To dtIndicador.Columns.Count - 1 Step 1
            sHtml = sHtml & "<th>" & dtIndicador.Columns(i).ColumnName & "</th>"
        Next

        Dim literal As New LiteralControl(sHtml)
        pnlcolumnasruta2.Controls.Clear()
        pnlcolumnasruta2.Controls.Add(literal)

        sHtml = ""
        For i = 0 To dtIndicador.Rows.Count - 1 Step 1
            sHtml = sHtml & "<tr>"
            For x = 0 To dtIndicador.Columns.Count - 1 Step 1
                sHtml = sHtml & "<td>" & dtIndicador.Rows(i)(x) & "</td>"
            Next
            sHtml = sHtml & "</tr>"

        Next
        literal = New LiteralControl(sHtml)
        pnlFilasruta2.Controls.Clear()
        pnlFilasruta2.Controls.Add(literal)


    End Sub

    Public Function fnToneladasRutaAño(Año As Int16, Mes As Int16, ruta As String) As Double
        Dim Tons As Double
        ssql = "select ISNULL(SUM(Toneladas),0) as Tons from vw_IP_Ventas where grupo = '" & ruta & "' and year(fecha)=" & Año & " and Month(Fecha)=" & Mes
        ssql = "select ISNULL(SUM(Toneladas),0) as Tons from Satelite_SAP .sistemas.Eficiencia where ruta = '" & ruta & "' and year(fecha)=" & Año & " and Month(Fecha)=" & Mes
        Dim dtTons As New DataTable
        dtTons = objDatos.fnEjecutarConsulta(ssql)
        If dtTons.Rows.Count > 0 Then
            Tons = dtTons.Rows(0)(0)
        End If

        Return Tons
    End Function

    Public Function fnPrecioTonsRutaAño(Año As Int16, Mes As Int16, ruta As String) As Double
        Dim Precio As Double
        ssql = "select ISNULL(SUM(PRecioSinIVA),0) as Tons from vw_IP_Ventas where grupo = '" & ruta & "' and year(fecha)=" & Año & " and Month(Fecha)=" & Mes
        ssql = "select ISNULL(SUM(TotalSinIVA),0) as Tons from Satelite_SAP .sistemas.Eficiencia where ruta = '" & ruta & "' and year(fecha)=" & Año & " and Month(Fecha)=" & Mes
        Dim dtTons As New DataTable
        dtTons = objDatos.fnEjecutarConsulta(ssql)
        If dtTons.Rows.Count > 0 Then
            Precio = dtTons.Rows(0)(0)
        End If

        Return Precio
    End Function

    Public Sub fnCargaEntidades()
        ssql = "select distinct grupo from vw_IP_Ventas  "
        ssql = "select distinct ruta as grupo from Satelite_SAP .sistemas.Eficiencia  "
        Dim dtrutas As New DataTable
        dtrutas = objDatos.fnEjecutarConsulta(ssql)
        Dim fila As DataRow
        fila = dtrutas.NewRow
        fila("Grupo") = "-Seleccione-"

        dtrutas.Rows.Add(fila)

        ddlRutas.DataSource = dtrutas
        ddlRutas.DataTextField = "Grupo"
        ddlRutas.DataValueField = "Grupo"
        ddlRutas.DataBind()
        ddlRutas.SelectedValue = "-Seleccione-"

        ddlRutasAcum.DataSource = dtrutas
        ddlRutasAcum.DataTextField = "Grupo"
        ddlRutasAcum.DataValueField = "Grupo"
        ddlRutasAcum.DataBind()
        ddlRutasAcum.SelectedValue = "-Seleccione-"
    End Sub

    Public Function fnEstructuraDT(Ruta As String) As DataTable

        Dim AñoActual As Int16 = Now.Year
        Dim AñoAnterior As Int16 = AñoActual - 1
        Dim AñoPasado As Int16 = AñoAnterior - 1

        Dim dtIndicador As New DataTable
        'dtIndicador.Columns.Add("Mes")
        'dtIndicador.Columns.Add(AñoAnterior)
        'dtIndicador.Columns.Add(AñoActual)
        'dtIndicador.Columns.Add("%")
        'dtIndicador.Columns.Add("Precio Prom")

        ssql = "select Mes,Format(Tons_Anterior, 'N', 'en-us') as '" & AñoAnterior & "',format(Tons_Actual, 'N', 'en-us') as '" & AñoActual & "' ,Format(Porcentaje, 'N', 'en-us') as '%' ,FORMAT(Precioprom, 'N', 'en-us') as 'Precio Prom'  from Satelite_SAP.indicadores.VentasMesRuta where ruta='" & Ruta & "' and tipo ='Normal'"
        dtIndicador = objDatos.fnejecutarConsulta(ssql)

        Return dtIndicador
    End Function

    Public Function fnEstructuraRutaAcumDT(Ruta As String) As DataTable

        Dim AñoActual As Int16 = Now.Year
        Dim AñoAnterior As Int16 = AñoActual - 1
        Dim AñoPasado As Int16 = AñoAnterior - 1

        Dim dtIndicador As New DataTable
        'dtIndicador.Columns.Add("Mes")
        'dtIndicador.Columns.Add(AñoAnterior)
        'dtIndicador.Columns.Add(AñoActual)
        'dtIndicador.Columns.Add("%")
        'dtIndicador.Columns.Add("Precio Prom")

        ssql = "select Mes,Format(Tons_Anterior, 'N', 'en-us') as '" & AñoAnterior & "',format(Tons_Actual, 'N', 'en-us') as '" & AñoActual & "' ,Format(Porcentaje, 'N', 'en-us') as '%' ,FORMAT(Precioprom, 'N', 'en-us') as 'Precio Prom'  from Satelite_SAP.indicadores.VentasMesRuta where ruta='" & Ruta & "' and tipo ='Acum'"
        dtIndicador = objDatos.fnejecutarConsulta(ssql)

        Return dtIndicador
    End Function


    Public Function fnEstructuraDT_Ant(Ruta As String) As DataTable

        Dim AñoActual As Int16 = Now.Year
        Dim AñoAnterior As Int16 = AñoActual - 1
        Dim AñoPasado As Int16 = AñoAnterior - 1

        Dim dtIndicador As New DataTable
        dtIndicador.Columns.Add("Mes")
        dtIndicador.Columns.Add(AñoAnterior)
        dtIndicador.Columns.Add(AñoActual)
        dtIndicador.Columns.Add("%")
        dtIndicador.Columns.Add("Precio Prom")

        ''Los meses
        Dim Tons1 As Double = 0
        Dim Tons2 As Double = 0
        Dim fila As DataRow
        Dim acum1 As Double = 0
        Dim acum2 As Double = 0


        For i = 1 To 12

            fila = dtIndicador.NewRow
            fila("Mes") = fnMes(i)

            Tons1 = fnToneladasRutaAño(AñoAnterior, i, Ruta)
            Tons2 = fnToneladasRutaAño(AñoActual, i, Ruta)


            acum1 = acum1 + Tons1
            acum2 = acum2 + Tons2

            fila("" & AñoAnterior & "") = Tons1.ToString("N2")
            fila("" & AñoActual & "") = Tons2.ToString("N2")

            Dim Porc As Double = 0
            Porc = (Tons2 * 100 / Tons1).ToString("N2")


            If Porc < 100 Then
                fila("%") = "<code>" & Porc & "</code>"
            Else
                fila("%") = Porc
            End If



            If Tons2 = 0 Then
                fila("Precio Prom") = "0.00"
            Else
                fila("Precio Prom") = (fnPrecioTonsRutaAño(AñoActual, i, Ruta) / Tons2).ToString("N2")
            End If


            dtIndicador.Rows.Add(fila)

        Next
        fila = dtIndicador.NewRow
        fila("Mes") = "Total Anualizado"
        fila("" & AñoAnterior & "") = acum1.ToString("N2")
        fila("" & AñoActual & "") = acum2.ToString("N2")
        fila("%") = (acum2 * 100 / acum1).ToString("N2")
        dtIndicador.Rows.Add(fila)

        Return dtIndicador
    End Function

    Public Function fnEstructuraRutaAcumDT_ant(Ruta As String) As DataTable

        Dim AñoActual As Int16 = Now.Year
        Dim AñoAnterior As Int16 = AñoActual - 1
        Dim AñoPasado As Int16 = AñoAnterior - 1

        Dim dtIndicador As New DataTable
        dtIndicador.Columns.Add("Mes")
        dtIndicador.Columns.Add(AñoAnterior)
        dtIndicador.Columns.Add(AñoActual)
        dtIndicador.Columns.Add("%")
        dtIndicador.Columns.Add("Precio Prom")

        ''Los meses
        Dim Tons1 As Double = 0
        Dim Tons2 As Double = 0

        Dim TonsAcum1 As Double = 0
        Dim TonsAcum2 As Double = 0
        Dim PrecioTonAcum As Double = 0

        For i = 1 To 12
            Dim fila As DataRow
            fila = dtIndicador.NewRow
            fila("Mes") = fnMes(i)

            Tons1 = fnToneladasRutaAño(AñoAnterior, i, Ruta)
            Tons2 = fnToneladasRutaAño(AñoActual, i, Ruta)
            If Tons2 = 0 Then
                Tons2 = TonsAcum2 / i
            End If

            TonsAcum1 = TonsAcum1 + Tons1
            TonsAcum2 = TonsAcum2 + Tons2

            fila("" & AñoAnterior & "") = TonsAcum1.ToString("N2")
            fila("" & AñoActual & "") = TonsAcum2.ToString("N2")

            fila("%") = (TonsAcum2 * 100 / TonsAcum1).ToString("N2")
            If Tons2 = 0 Then
                fila("Precio Prom") = "0.00"
            Else
                PrecioTonAcum = PrecioTonAcum + fnPrecioTonsRutaAño(AñoActual, i, Ruta)
                fila("Precio Prom") = (PrecioTonAcum / TonsAcum2).ToString("N2")
            End If


            dtIndicador.Rows.Add(fila)

        Next


        Return dtIndicador
    End Function
    Public Function fnMesCorto(mes As Int16)
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
    Public Function fnMes(mes As Int16)
        Dim sMes As String = ""

        If mes = 1 Then
            sMes = "Enero"
        End If
        If mes = 2 Then
            sMes = "Febrero"
        End If
        If mes = 3 Then
            sMes = "Marzo"
        End If
        If mes = 4 Then
            sMes = "Abril"
        End If
        If mes = 5 Then
            sMes = "Mayo"
        End If
        If mes = 6 Then
            sMes = "Junio"
        End If
        If mes = 7 Then
            sMes = "Julio"
        End If
        If mes = 8 Then
            sMes = "Agosto"
        End If
        If mes = 9 Then
            sMes = "Septiembre"
        End If
        If mes = 10 Then
            sMes = "Octubre"
        End If
        If mes = 11 Then
            sMes = "Noviembre"
        End If
        If mes = 12 Then
            sMes = "Diciembre"
        End If

        Return sMes
    End Function

    Private Sub ddlRutas_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRutas.SelectedIndexChanged
        fnIndicadorRuta(ddlRutas.SelectedItem.Text)
        fnIndicadorRutaAcum(ddlRutas.SelectedItem.Text)
        fnIndicadorClientePivot()
    End Sub

    Private Sub ddlRutasAcum_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRutasAcum.SelectedIndexChanged
        fnIndicadorRutaAcum(ddlRutasAcum.SelectedItem.Text)
    End Sub

    Public Function fnCreaDatatablePivot()
        Dim dtDatos As New DataTable

        Dim AñoActual As Int16 = Now.Year - 2000
        Dim AñoAnterior As Int16 = AñoActual - 1


        Dim sOrderby As String
        Dim iMesVencido As Int16

        iMesVencido = Now.Date.Month - 1
        If iMesVencido = 0 Then
            sOrderby = "cv" & fnMesCorto(12) & "Ant desc"
        Else
            sOrderby = "cv" & fnMesCorto(Now.Date.Month - 1) & "act desc"
        End If
        ''dtDatos.Columns.Add("Cliente")

        ''For i = 1 To 12
        ''    dtDatos.Columns.Add(fnMesPivot(i) & AñoAnterior)
        ''Next

        ''For x = 1 To Now.Date.Month
        ''    dtDatos.Columns.Add(fnMesPivot(x) & AñoActual)
        ''Next
        ssql = "select cvCliente as Cliente,cvEneant as EneAnt,cvFebAnt as FebAnt,cvMarAnt as MarAnt,cvAbrAnt as AbrAnt,cvMayAnt as MayAnt, " _
            & " cvJunAnt as JunAnt,cvJulant as JulAnt,cvAgoAnt as AgoAnt,cvSepAnt as SepAnt,cvOctAnt as OctAnt,cvNovant as NovAnt,cvDicant as DicAnt, " _
            & " cvEneAct as EneAct,cvFebAct as FebAct,cvMarAct as MarAct,cvAbrAct as AbrAct,cvMayAct as MayAct, " _
            & " cvJunAct as JunAct,cvJulAct as JulAct,cvAgoAct as AgoAct,cvSepAct as SepAct,cvOctAct as OctAct,cvNovAct as NovAct,cvDicAct as DicAct  " _
            & " from satelite_SAP.indicadores.VentasMesRutaClientes where cvRuta=  " & "'" & ddlRutas.SelectedItem.Text & "' order by " & sOrderby

        dtDatos = objDatos.fnEjecutarConsulta(ssql)


        '''Ahora los clientes
        'ssql = "SELECT Nombre FROM (select Nombre  ,sum(toneladas) tons from vw_IP_Ventas where year(fecha)='" & Now.Year & "' and MONTH (fecha)=" & Now.Month - 1 & " and Grupo='" & ddlRutas.SelectedItem.Text & "' group by Nombre) X order by Tons desc "
        'ssql = "SELECT Nombre FROM (select Nombre  ,sum(toneladas) tons from Satelite_SAP .sistemas.Eficiencia where year(fecha)='" & Now.Year & "' and MONTH (fecha)=" & Now.Month - 1 & " and ruta='" & ddlRutas.SelectedItem.Text & "' group by Nombre) X order by Tons desc "
        'Dim dtClientes As New DataTable
        'dtClientes = objDatos.fnEjecutarConsulta(ssql)

        'For i = 0 To dtClientes.Rows.Count - 1 Step 1
        '    Dim fila As DataRow

        '    fila = dtDatos.NewRow
        '    fila("cliente") = "<a href='hijoscliente.aspx?cliente=" & dtClientes.Rows(i)(0) & "'>" & dtClientes.Rows(i)(0) & "</a>"
        '    For mes = 1 To 12
        '        fila(fnMesPivot(mes) & AñoAnterior) = fnTonsMesAñoCliente(dtClientes.Rows(i)(0), mes, Now.Year - 1).ToString("N2")
        '    Next

        '    For mesActual = 1 To Now.Date.Month - 1
        '        fila(fnMesPivot(mesActual) & AñoActual) = fnTonsMesAñoCliente(dtClientes.Rows(i)(0), mesActual, Now.Year).ToString("N2")
        '    Next

        '    dtDatos.Rows.Add(fila)

        'Next


        Return dtDatos
    End Function


    Public Function fnTonsMesAñoCliente(cliente As String, Mes As Int16, Año As Int16) As Double

        Dim Tons As Double = 0

        ssql = "SELECT ISNULL(sum(toneladas),0) tons from Satelite_SAP .sistemas.Eficiencia where year(fecha)='" & Año & "' and MONTH (fecha)=" & Mes & " and Nombre = " & "'" & cliente & "'"
        ssql = "SELECT ISNULL(sum(toneladas),0) tons from  Satelite_SAP .indicadores.ventasrutaCliente where Año='" & Año & "' and Mes=" & Mes & " and Nombre = " & "'" & cliente & "'"
        Dim dttons As New DataTable
        dttons = objDatos.fnEjecutarConsulta(ssql)

        If dttons.Rows.Count > 0 Then
            Tons = dttons.Rows(0)(0)
        End If


        Return Tons

    End Function


    Public Function fnMesPivot(mes As Int16)
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


    Public Sub fnIndicadorClientePivot()

        Dim AñoActual As Int16 = Now.Year - 2000
        Dim AñoAnterior As Int16 = AñoActual - 1

        Dim dtIndicador As New DataTable
        dtIndicador = fnCreaDatatablePivot()
        Dim sHtml As String = ""

        For i = 0 To dtIndicador.Columns.Count - 1 Step 1
            sHtml = sHtml & "<th>" & dtIndicador.Columns(i).ColumnName.Replace("Ant", AñoAnterior).Replace("Act", AñoActual) & "</th>"
        Next

        Dim literal As New LiteralControl(sHtml)
        pnlEncabezadoClientes.Controls.Clear()
        pnlEncabezadoClientes.Controls.Add(literal)

        sHtml = ""
        For i = 0 To dtIndicador.Rows.Count - 1 Step 1
            sHtml = sHtml & "<tr>"
            For x = 0 To dtIndicador.Columns.Count - 1 Step 1
                If x > 0 Then
                    sHtml = sHtml & "<td>" & CDbl(dtIndicador.Rows(i)(x)).ToString("N2") & "</td>"
                Else

                    sHtml = sHtml & "<td>" & "<a href='hijoscliente.aspx?cliente=" & dtIndicador.Rows(i)(x) & "&ruta=" & ddlRutas.SelectedValue & "'>" & dtIndicador.Rows(i)(x) & "</a>" & "</td>"
                End If

            Next
            sHtml = sHtml & "</tr>"

        Next
        literal = New LiteralControl(sHtml)
        pnlLineasCliente.Controls.Clear()
        pnlLineasCliente.Controls.Add(literal)
    End Sub

    <WebMethod>
    Public Shared Function fnGeneraIndicadorHijos(Cliente As String) As String


    End Function

    Public Sub fnGeneraHTML()

    End Sub
End Class
