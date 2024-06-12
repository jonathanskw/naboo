
Imports System.Data
Imports System.Web.Services
Partial Class empresa
    Inherits System.Web.UI.Page

    Public objDatos As New cls_funciones
    Public ssql As String = ""

    Private Sub empresa_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            fnIndicadorEmpresa()
            fnIndicadorEmpresaAcum()
            fnIndicadorFamiliasPivot()
        End If
    End Sub

    Public Function fnToneladasEmpresa(Año As Int16, mes As Int16)
        Dim dtIndicadorPaso As New DataTable
        Dim tons As Double = 0
        ssql = "SELECT Toneladas " _
            & " FROM SAtelite_SAP..TablaToneladasEmpresa  T0 Where T0.Anio =" & "'" & Año & "' and T0.mes=" & "'" & mes & "'"
        dtIndicadorPaso = objDatos.fnEjecutarConsulta(ssql)

        If dtIndicadorPaso.Rows.Count > 0 Then
            tons = dtIndicadorPaso.Rows(0)(0)

        End If
        Return tons
    End Function

    Public Function fnPrecioPromEmpresa(Año As Int16, mes As Int16)
        Dim PrecioProm As Double = 0
        Dim dtIndicadorPaso As New DataTable
        ssql = "SELECT  ISNULL(SUM(TotalSinIVA),0) fROM SAtelite_SAP.SISTEMAS.Eficiencia where YEAR(fecha) =" & "'" & Año & "'  AND Month(fecha)=" & "'" & mes & "'"
        dtIndicadorPaso = objDatos.fnEjecutarConsulta(ssql)
        If dtIndicadorPaso.Rows.Count > 0 Then
            PrecioProm = dtIndicadorPaso.Rows(0)(0)

        End If
        Return PrecioProm

    End Function


    Public Function fnEstructuraDT() As DataTable

        Dim AñoActual As Int16 = Now.Year
        Dim AñoAnterior As Int16 = AñoActual - 1
        Dim AñoPasado As Int16 = AñoAnterior - 1

        Dim Tons1 As Double = 0
        Dim Tons2 As Double = 0

        Dim TonsAcum1 As Double = 0
        Dim TonsAcum2 As Double = 0
        Dim PrecioTonAcum As Double = 0

        Dim fila As DataRow
        Dim dtIndicador As New DataTable
        dtIndicador.Columns.Add("Mes")
        dtIndicador.Columns.Add(AñoAnterior)
        dtIndicador.Columns.Add(AñoActual)
        dtIndicador.Columns.Add("%")
        dtIndicador.Columns.Add("Precio Prom", GetType(Double))



        For i = 1 To 12

            fila = dtIndicador.NewRow
            fila("Mes") = fnMes(i)
            Tons1 = fnToneladasEmpresa(AñoAnterior, i)
            If i < Now.Month Then

                Tons2 = fnToneladasEmpresa(AñoActual, i)
            Else

                Tons2 = 0
            End If




            TonsAcum1 = TonsAcum1 + Tons1
            TonsAcum2 = TonsAcum2 + Tons2

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
                fila("Precio Prom") = CDbl((fnPrecioPromEmpresa(AñoActual, i) / Tons2)).ToString("N2")
            End If


            dtIndicador.Rows.Add(fila)




        Next

        fila = dtIndicador.NewRow
        fila("Mes") = "Total Anualizado"
        fila("" & AñoAnterior & "") = TonsAcum1.ToString("N2")
        fila("" & AñoActual & "") = TonsAcum2.ToString("N2")
        fila("%") = (TonsAcum2 * 100 / TonsAcum1).ToString("N2")
        dtIndicador.Rows.Add(fila)

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
    Public Function fnEstructuraEmpresaAcumDT() As DataTable
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
            Tons1 = fnToneladasEmpresa(AñoAnterior, i)
            If i < Now.Month Then
                Tons2 = fnToneladasEmpresa(AñoActual, i)
            Else
                Tons2 = 0
            End If
            If Tons2 = 0 Then
                Tons2 = TonsAcum2 / i
            End If

            If i < Now.Month Then
                TonsAcum2 = TonsAcum2 + Tons2
            Else
                TonsAcum2 = 0
            End If
            TonsAcum1 = TonsAcum1 + Tons1


            fila("" & AñoAnterior & "") = TonsAcum1.ToString("N2")
            fila("" & AñoActual & "") = TonsAcum2.ToString("N2")

            fila("%") = (TonsAcum2 * 100 / TonsAcum1).ToString("N2")
            If Tons2 = 0 Then
                fila("Precio Prom") = "0.00"
            Else
                PrecioTonAcum = PrecioTonAcum + fnPrecioPromEmpresa(AñoActual, i)
                fila("Precio Prom") = CDbl((PrecioTonAcum / TonsAcum2)).ToString("N2")
            End If


            dtIndicador.Rows.Add(fila)



        Next


        Return dtIndicador
    End Function

    Public Sub fnIndicadorEmpresa()
        Dim AñoActual As Int16 = Now.Year
        Dim AñoAnterior As Int16 = AñoActual - 1
        Dim AñoPasado As Int16 = AñoAnterior - 1

        Dim dtIndicador As New DataTable
        dtIndicador = fnEstructuraDT()




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

    Public Sub fnIndicadorEmpresaAcum()
        Dim AñoActual As Int16 = Now.Year
        Dim AñoAnterior As Int16 = AñoActual - 1
        Dim AñoPasado As Int16 = AñoAnterior - 1

        Dim dtIndicador As New DataTable
        dtIndicador = fnEstructuraEmpresaAcumDT()




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
    Public Sub fnIndicadorFamiliasPivot()
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

                    sHtml = sHtml & "<td style =' position: sticky;'>" & dtIndicador.Rows(i)(x) & "</td>"
                End If


            Next
            sHtml = sHtml & "</tr>"

        Next
        literal = New LiteralControl(sHtml)
        pnlLineasHijos.Controls.Clear()
        pnlLineasHijos.Controls.Add(literal)
    End Sub

    Public Function fnCreaDatatable()
        Dim dtDatos As New DataTable

        Dim AñoActual As Int16 = Now.Year - 2000
        Dim AñoAnterior As Int16 = AñoActual - 1

        Dim sOrderby As String
        Dim iMesVencido As Int16

        iMesVencido = Now.Date.Month - 1
        If iMesVencido = 0 Then
            sOrderby = fnMesCorto(12) & "Ant desc"
        Else
            sOrderby = fnMesCorto(Now.Date.Month - 1) & "act desc"
        End If

        ssql = "Select cvFamilia as Familia, cvHijo As Hijo,sum(cvEneant) As EneAnt,sum(cvFebAnt) As FebAnt,sum(cvMarAnt) As MarAnt,sum(cvAbrAnt) As AbrAnt,sum(cvMayAnt) As MayAnt, " _
             & " sum(cvJunAnt) As JunAnt,sum(cvJulant) As JulAnt,sum(cvAgoAnt) As AgoAnt,sum(cvSepAnt) As SepAnt,sum(cvOctAnt) As OctAnt,sum(cvNovant) As NovAnt,sum(cvDicant) As DicAnt, " _
             & " sum(cvEneAct) As EneAct,sum(cvFebAct) As FebAct,sum(cvMarAct) As MarAct,sum(cvAbrAct) As AbrAct,sum(cvMayAct) As MayAct, " _
             & " sum(cvJunAct) As JunAct,sum(cvJulAct) As JulAct,sum(cvAgoAct) As AgoAct,sum(cvSepAct) As SepAct,sum(cvOctAct) As OctAct,sum(cvNovAct) As NovAct,sum(cvDicAct) As DicAct  " _
             & " from satelite_SAP.indicadores.VentasMesRutaClientesHijos group by cvFamilia,cvhijo order by " & sOrderby

        dtDatos = objDatos.fnEjecutarConsulta(ssql)




        Return dtDatos
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
End Class
