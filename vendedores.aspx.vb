
Imports System.Data

Partial Class vendedores
    Inherits System.Web.UI.Page


    Public objDatos As New cls_funciones
    Public ssql As String = ""
    Private Sub vendedores_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            fnCargaEntidades()
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

        ssql = "Select TOP 20 cvFamilia as Familia, cvHijo As Hijo,cvEneant As EneAnt,cvFebAnt As FebAnt,cvMarAnt As MarAnt,cvAbrAnt As AbrAnt,cvMayAnt As MayAnt, " _
            & " cvJunAnt As JunAnt,cvJulant As JulAnt,cvAgoAnt As AgoAnt,cvSepAnt As SepAnt,cvOctAnt As OctAnt,cvNovant As NovAnt,cvDicant As DicAnt, " _
            & " cvEneAct As EneAct,cvFebAct As FebAct,cvMarAct As MarAct,cvAbrAct As AbrAct,cvMayAct As MayAct, " _
            & " cvJunAct As JunAct,cvJulAct As JulAct,cvAgoAct As AgoAct,cvSepAct As SepAct,cvOctAct As OctAct,cvNovAct As NovAct,cvDicAct As DicAct  " _
            & " from satelite_SAP.indicadores.VentasMesRutaClientesHijos -- order by " & sOrderby

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

    Public Sub fnIndicadorVendedor(Vendedor As String)
        Dim AñoActual As Int16 = Now.Year
        Dim AñoAnterior As Int16 = AñoActual - 1
        Dim AñoPasado As Int16 = AñoAnterior - 1

        Dim dtIndicador As New DataTable
        dtIndicador = fnEstructuraDT(Vendedor)




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

    Public Sub fnIndicadorVendedorAcum(Vendedor As String)
        Dim AñoActual As Int16 = Now.Year
        Dim AñoAnterior As Int16 = AñoActual - 1
        Dim AñoPasado As Int16 = AñoAnterior - 1

        Dim dtIndicador As New DataTable
        dtIndicador = fnEstructuraVendedorAcumDT(Vendedor)




        Dim sHtml As String = ""

        For i = 0 To dtIndicador.Columns.Count - 1 Step 1
            sHtml = sHtml & "<th>" & dtIndicador.Columns(i).ColumnName & "</th>"
        Next

        Dim literal As New LiteralControl(sHtml)
        pnlcolumnasvendedor2.Controls.Clear()
        pnlcolumnasvendedor2.Controls.Add(literal)

        sHtml = ""
        For i = 0 To dtIndicador.Rows.Count - 1 Step 1
            sHtml = sHtml & "<tr>"
            For x = 0 To dtIndicador.Columns.Count - 1 Step 1
                sHtml = sHtml & "<td>" & dtIndicador.Rows(i)(x) & "</td>"
            Next
            sHtml = sHtml & "</tr>"

        Next
        literal = New LiteralControl(sHtml)
        pnlFilasVendedor2.Controls.Clear()
        pnlFilasVendedor2.Controls.Add(literal)


    End Sub

    Public Function fnToneladasVendedorAño(Año As Int16, Mes As Int16, Vendedor As String) As Double
        Dim Tons As Double
        ssql = "select ISNULL(SUM(Toneladas),0) as Tons from vw_IP_Ventas where VendedorFact = '" & Vendedor & "' and year(fecha)=" & Año & " and Month(Fecha)=" & Mes
        Dim dtTons As New DataTable
        dtTons = objDatos.fnEjecutarConsulta(ssql)
        If dtTons.Rows.Count > 0 Then
            Tons = dtTons.Rows(0)(0)
        End If

        Return Tons
    End Function

    Public Function fnPrecioTonsVendedorAño(Año As Int16, Mes As Int16, Vendedor As String) As Double
        Dim Precio As Double
        ssql = "select ISNULL(SUM(PRecioSinIVA),0) as Tons from vw_IP_Ventas where VendedorFact = '" & Vendedor & "' and year(fecha)=" & Año & " and Month(Fecha)=" & Mes
        Dim dtTons As New DataTable
        dtTons = objDatos.fnEjecutarConsulta(ssql)
        If dtTons.Rows.Count > 0 Then
            Precio = dtTons.Rows(0)(0)
        End If

        Return Precio
    End Function

    Public Sub fnCargaEntidades()
        ssql = "select distinct VendedorFact from vw_IP_Ventas WHERE vendedorFact not like 'Z%' "
        Dim dtrutas As New DataTable
        dtrutas = objDatos.fnEjecutarConsulta(ssql)
        Dim fila As DataRow
        fila = dtrutas.NewRow
        fila("VendedorFact") = "-Seleccione-"

        dtrutas.Rows.Add(fila)

        ddlVendedor.DataSource = dtrutas
        ddlVendedor.DataTextField = "VendedorFact"
        ddlVendedor.DataValueField = "VendedorFact"
        ddlVendedor.DataBind()
        ddlVendedor.SelectedValue = "-Seleccione-"

        ddlVendedorAcum.DataSource = dtrutas
        ddlVendedorAcum.DataTextField = "VendedorFact"
        ddlVendedorAcum.DataValueField = "VendedorFact"
        ddlVendedorAcum.DataBind()
        ddlVendedorAcum.SelectedValue = "-Seleccione-"
    End Sub
    Public Function fnEstructuraDT(Vendedor As String) As DataTable

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

            Tons1 = fnToneladasVendedorAño(AñoAnterior, i, Vendedor)
            Tons2 = fnToneladasVendedorAño(AñoActual, i, Vendedor)


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

            'fila("%") = (Tons2 * 100 / Tons1).ToString("N2")

            If Tons2 = 0 Then
                fila("Precio Prom") = "0.00"
            Else
                fila("Precio Prom") = (fnPrecioTonsVendedorAño(AñoActual, i, Vendedor) / Tons2).ToString("N2")
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

    Public Function fnEstructuraVendedorAcumDT(Vendedor As String) As DataTable

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

            Tons1 = fnToneladasVendedorAño(AñoAnterior, i, Vendedor)
            Tons2 = fnToneladasVendedorAño(AñoActual, i, Vendedor)
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
                PrecioTonAcum = PrecioTonAcum + fnPrecioTonsVendedorAño(AñoActual, i, Vendedor)
                fila("Precio Prom") = (PrecioTonAcum / TonsAcum2).ToString("N2")
            End If


            dtIndicador.Rows.Add(fila)

        Next


        Return dtIndicador
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

    Private Sub ddlVendedor_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlVendedor.SelectedIndexChanged
        fnIndicadorVendedor(ddlVendedor.SelectedItem.Text)
        fnIndicadorVendedorAcum(ddlVendedor.SelectedItem.Text)
        fnIndicadorClientePivot()

    End Sub

    Private Sub ddlVendedorAcum_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlVendedorAcum.SelectedIndexChanged
        fnIndicadorVendedorAcum(ddlVendedorAcum.SelectedItem.Text)
    End Sub
End Class
