
Imports System.Data
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class estrategia
    Inherits System.Web.UI.Page
    Public objDatos As New cls_funciones
    Public ssql As String = ""
    Public iTipoLista As Int16 = 15


    Public PromTons As Double = 0
    Public PromPrecio As Double = 0

    Public ListaPrecios As String = ""
    Private Sub estrategia_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            fnGeneralesCliente()
            fnToneladasPrecioUltimosMeses()
            fnClienteSinEstrategia()
            fnProductoEmpuje()
            fnClienteConEstrategia()
        End If
    End Sub

    Public Sub fnProductoEmpuje()
        Dim sCardCode As String = "PRPISA94"

        If Request.QueryString.Count > 0 Then
            sCardCode = Request.QueryString("cliente")

        End If

        ssql = "SELECT T0.ItemCode ,T2.ItemName, T0.ItemCode + ' - ' + T2.ItemName as Descripcion, T0.Price  FROM ITM1 T0 INNER JOIN OPLN T1 on T1.ListNum = T0.PriceList INNER JOIN OITM T2 on T2.ItemCode =T0.ItemCode WHERE T1.ListName ='" & ListaPrecios & "' and Price >0 " _
& "AND T0.ItemCode not in (select top 3 articulo collate SQL_Latin1_General_CP1_CI_AS FROM(select Articulo,sum(toneladas)as toneladas from Satelite_SAP .sistemas.Eficiencia where codigo='" & sCardCode & "' group by articulo)X order by toneladas desc) Order By Descripcion "
        Dim dtProductos As New DataTable
        dtProductos = objDatos.fnEjecutarConsulta(ssql)
        ddlProductos.DataSource = dtProductos
        ddlProductos.DataTextField = "Descripcion"
        ddlProductos.DataValueField = "ItemCode"
        ddlProductos.DataBind()

        ssql = "SELECT T0.ItemCode ,T2.ItemName, T0.ItemCode + ' - ' + T2.ItemName as Descripcion, T0.Price  FROM ITM1 T0 INNER JOIN OPLN T1 on T1.ListNum = T0.PriceList INNER JOIN OITM T2 on T2.ItemCode =T0.ItemCode WHERE T1.ListName ='" & ListaPrecios & "' and Price >0 " _
& "AND T0.ItemCode not in (select top 3 articulo collate SQL_Latin1_General_CP1_CI_AS FROM(select Articulo,sum(toneladas)as toneladas from Satelite_SAP .sistemas.Eficiencia where codigo='" & sCardCode & "' group by articulo)X order by toneladas desc) Order By Descripcion "
        Dim dtProductoB As New DataTable
        dtProductoB = objDatos.fnEjecutarConsulta(ssql)
        ddlProductoB.DataSource = dtProductos
        ddlProductoB.DataTextField = "Descripcion"
        ddlProductoB.DataValueField = "ItemCode"
        ddlProductoB.DataBind()


    End Sub

    Public Sub fnGeneralesCliente()
        Dim sCardCode As String = "PRPISA94"

        If Request.QueryString.Count > 0 Then
            sCardCode = Request.QueryString("cliente")

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


    Public Sub fnToneladasPrecioUltimosMeses()
        Dim sCardCode As String = "PRPISA94"
        Dim SumaTons As Double = 0
        Dim PrecioProm As Double = 0

        If Request.QueryString.Count > 0 Then
            sCardCode = Request.QueryString("cliente")

        End If

        ''Tons promedio y precio promedio ultimos 4 meses

        Dim iMesCorte As Int16
        iMesCorte = Now.Date.Month - 1
        Dim iAño As Int16
        iAño = Now.Date.Year


        For i = 1 To 4

            Dim Tons As Double = 0
            Dim Precio As Double = 0

            ssql = "select ISNULL(SUM(Toneladas),0) as Tons from vw_IP_Ventas where Codigo = '" & sCardCode & "' and year(fecha)=" & iAño & " and Month(Fecha)=" & iMesCorte
            Dim dtTons As New DataTable
            dtTons = objDatos.fnEjecutarConsulta(ssql)
            If dtTons.Rows.Count > 0 Then
                SumaTons = SumaTons + dtTons.Rows(0)(0)
                Tons = dtTons.Rows(0)(0)
            End If

            If Tons = 0 Then

            Else
                ssql = "select ISNULL(SUM(PRecioSinIVA),0) as Tons from vw_IP_Ventas where Codigo = '" & sCardCode & "' and year(fecha)=" & iAño & " and Month(Fecha)=" & iMesCorte
                Dim dtPrecioProm As New DataTable
                dtPrecioProm = objDatos.fnEjecutarConsulta(ssql)
                If dtPrecioProm.Rows.Count > 0 Then
                    Precio = dtPrecioProm.Rows(0)(0)
                    PrecioProm = PrecioProm + (Precio / Tons)
                End If
            End If


            iMesCorte = iMesCorte - 1
            If iMesCorte = 0 Then
                iMesCorte = 12
                iAño = iAño - 1
            End If
        Next



        PromTons = SumaTons / 4
        PromPrecio = PrecioProm / 4

    End Sub
    Public Sub fnClienteSinEstrategia()

        Dim UtilidadMensual As Double = 0
        Dim utilidadAl30 As Double = 0

        UtilidadMensual = PromTons * PromPrecio
        utilidadAl30 = UtilidadMensual * (1 + (iTipoLista / 100)) * 0.3

        lblTonsProm.Text = PromTons.ToString("N2")
        lblPrecioProm.Text = PromPrecio.ToString("C2")
        lblTotCostoMensual.Text = UtilidadMensual.ToString("C2")
        lblUtilidad.Text = utilidadAl30.ToString("C2")
        lblUtilidadPorDif.Text = (UtilidadMensual * (iTipoLista / 100)).ToString("C2")
        lblUtilidadIntegrada.Text = (utilidadAl30 + (UtilidadMensual * (iTipoLista / 100))).ToString("C2")


    End Sub
    Public Sub fnClienteConEstrategia()
        Dim UtilidadMensual As Double = 0
        Dim utilidadAl30 As Double = 0

        PromTons = PromTons * 1.05

        UtilidadMensual = PromTons * PromPrecio
        utilidadAl30 = UtilidadMensual * (1 + (iTipoLista / 100)) * 0.3

        lblTonsProme.Text = PromTons.ToString("N2")
        lblprecioProme.Text = PromPrecio.ToString("C2")
        lblTotalCostoe.Text = UtilidadMensual.ToString("C2")
        lblutilidade.Text = utilidadAl30.ToString("C2")
        lblUtilidadPorDife.Text = (UtilidadMensual * (iTipoLista / 100)).ToString("C2")
        lblUtilidadIntegradae.Text = (utilidadAl30 + (UtilidadMensual * (iTipoLista / 100))).ToString("C2")

        txtToneladas.Text = (PromTons * 0.1).ToString("N2")
    End Sub

    Private Sub txtCostoRealCliente_TextChanged(sender As Object, e As EventArgs) Handles txtCostoRealCliente.TextChanged
        lblTotalCostoProducto.Text = (CDbl(txtToneladas.Text) * CDbl(txtCostoRealCliente.Text)).ToString("C2")
    End Sub

    Private Sub txtPrecioVtaPublico_TextChanged(sender As Object, e As EventArgs) Handles txtPrecioVtaPublico.TextChanged
        lblUtilidadProducto.Text = ((CDbl(txtPrecioVtaPublico.Text.Replace("$", "").Replace(",", "")) - CDbl(txtCostoRealCliente.Text)) * CDbl(txtToneladas.Text)).ToString("C2")
    End Sub

    Private Sub btnCalcular_Click(sender As Object, e As EventArgs) Handles btnCalcular.Click

        lblUtilidadDistTotal.Text = lblUtilidadPorDife.Text


        lblNc.Text = (((CDbl(lblTotalCostoProducto.Text.Replace("$", "").Replace(",", "")) + CDbl(lblTotalCostoe.Text.Replace("$", "").Replace(",", "")))) * 0.025).ToString("C2")
        lblUtilidadTotal.Text = (CDbl(lblUtilidadProducto.Text.Replace("$", "").Replace(",", "")) + CDbl(lblutilidade.Text.Replace("$", "").Replace(",", "") + CDbl(lblNc.Text.Replace("$", "").Replace(",", "")))).ToString("C2")
        lblUtilidadIntegradaTotal.Text = (CDbl(lblUtilidadDistTotal.Text.Replace("$", "").Replace(",", "")) + CDbl(lblUtilidadTotal.Text.Replace("$", "").Replace(",", ""))).ToString("C2")

        lblPorcentajeAumento.Text = ((CDbl(lblUtilidadIntegradaTotal.Text.Replace("$", "").Replace(",", "")) / CDbl(lblUtilidadIntegrada.Text.Replace("$", "").Replace(",", ""))) - 1).ToString("P2")

    End Sub

    Private Sub btnFormato_Click(sender As Object, e As EventArgs) Handles btnFormato.Click
        Dim reporte As New ReportDocument
        reporte.Load(Server.MapPath("~") & "\formato\Reporte_layout.rpt")
        reporte.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "estrategia")
        reporte.Dispose()
    End Sub
End Class
