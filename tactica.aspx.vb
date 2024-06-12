
Imports System.Data
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports Microsoft.SqlServer.Server

Partial Class tactica
    Inherits System.Web.UI.Page
    Public objDatos As New cls_funciones
    Public ssql As String = ""
    Public iTipoLista As Int16 = 1


    Public PromTons As Double = 0
    Public PromPrecio As Double = 0
    Public ListaPrecios As String = ""

    ''Variables dinamicas para Tacticas
    Public porcSetPoint As Double = 0
    Public porcEmpuje As Double = 0
    Public porcNC As Double = 0
    Public TonsEmpuje As Double = 0

    ''Variables para el reporte
    Public _sinTonsVend4m As Double = 0
    Public _sinUtilidadEsperada As Double = 0
    Public _sinUtilidadAdicional As Double = 0
    Public _sinUtilidadTot As Double = 0


    Public _conTonsVend4m As Double = 0
    Public _conUtilidadEsperada As Double = 0
    Public _conUtilidadAdicional As Double = 0
    Public _conUtilidadObtenida As Double = 0
    Public _conPorcAumentoNC As Double = 0
    Public _conUtilidadTot As Double = 0


    Public GananciaMarginal As Double = 0
    Public PorcGanancia As Double = 0

    Public sVendedor As String = ""


    Private Sub tactica_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            fnGeneralesCliente()

            fnTiposTactica()

            fnToneladasPrecioUltimosMeses()
            fnClienteSinEstrategia()
            fnProductoEmpuje()
            fnClienteConEstrategia()
        End If
        fnCargaParametrosTactica()
    End Sub

    Public Sub fnTiposTactica()
        Dim sCardCode As String = "PRPISA94"
        If Request.QueryString.Count > 0 Then
            sCardCode = Request.QueryString("cliente")

        End If

        Dim sTipoRuta As String = ""
        Dim sRutaLoF As String = ""
        Dim iDias As Int16 = 0
        Dim PathTactica As String = ""
        Dim TipoOperacion As String = ""

        ''Obtenemos la ruta del cliente
        ssql = "select groupCode,datediff(day,ISNULL(CreateDate,'20000101'),getdate()) as dias from IP..OCRD where cardcode=" & "'" & sCardCode & "'"
        Dim dtruta As New DataTable
        dtruta = objDatos.fnEjecutarConsulta(ssql)
        If dtruta.Rows.Count > 0 Then
            iDias = dtruta.Rows(0)(1)
            ssql = "select  ISNULL(tipomercado,'ANTIGUO')tipomercado,ISNULL(tiporuta,'FORANEA')tiporuta from Satelite_SAP .ventas.TacticasRuta where IdRuta = " & "'" & dtruta.Rows(0)(0) & "'"
            Dim dtConfigRuta As New DataTable
            dtConfigRuta = objDatos.fnEjecutarConsulta(ssql)
            If dtConfigRuta.Rows.Count > 0 Then
                sTipoRuta = dtConfigRuta.Rows(0)("tipomercado")
                sRutaLoF = dtConfigRuta.Rows(0)("tiporuta")
            End If

        End If
        Dim iDiasUltimaCompra As Int32 = fnUltimaCompraDias(sCardCode)

        If iDias > 15 Then
            If iDiasUltimaCompra > 180 Then
                TipoOperacion = "PRIMERA OPERACION" ''PROSPECTO, se toma como si fuera nuevo
            Else
                TipoOperacion = "MANTENER"
            End If

        Else
            TipoOperacion = "PRIMERA OPERACION"
        End If
        ssql = "select tipoOperacion from Satelite_SAP .ventas.tactica where cvCardCode= " & "'" & sCardCode & "' and cvEstatus ='FIRMADA' "
        Dim dtYatuvoTactica As New DataTable
        dtYatuvoTactica = objDatos.fnEjecutarConsulta(ssql)
        If dtYatuvoTactica.Rows.Count = 0 Then
            TipoOperacion = "PRIMERA OPERACION"
        Else
            ''Ya tuvo tactica
            ''Veamos que tipo de tactica
            If dtYatuvoTactica.Rows(0)(0) = "PRIMERA OPERACION" Then
                TipoOperacion = "RECOMPRA"
            Else
                If dtYatuvoTactica.Rows(0)(0) = "SUBIR VOLUMEN" Then
                    TipoOperacion = "MANTENER"
                End If
            End If

        End If


        TipoOperacion = "PRIMERA OPERACION"
        PathTactica = sTipoRuta & " \ " & sRutaLoF & " \ " & TipoOperacion

        lblRutaTactica.Text = "MERCADO " & PathTactica
        ssql = "select ciIdRel,cvNombre from Satelite_SAP .ventas.tacticas_Config where tipoMercado= " & "'" & sTipoRuta & "' and tipoRuta=" & "'" & sRutaLoF & "' and tipoOperacion=" & "'" & TipoOperacion & "'"

        If dtYatuvoTactica.Rows.Count = 0 Then
            ssql = ssql & " UNION ALL select ciIdRel,cvNombre from Satelite_SAP .ventas.tacticas_Config where ciIdRel=1 "
        End If

        If iTipoLista >= 6 And (TipoOperacion <> "MANTENER" Or TipoOperacion <> "RECOMPRA") Then
            ''Tiene lista mayor a 6, de va por default a abanico
            ssql = " select ciIdRel,cvNombre from Satelite_SAP .ventas.tacticas_Config where ciIdRel=1 "
            lblRutaTactica.Text = lblRutaTactica.Text & " Lista: " & iTipoLista

        Else
            If TipoOperacion = "MANTENER" Then
                ''Tactica de mantenimiento
            End If


        End If
        Dim dtTiposTactica As New DataTable
        dtTiposTactica = objDatos.fnEjecutarConsulta(ssql)

        ddlTipoTactica.DataSource = dtTiposTactica
        ddlTipoTactica.DataTextField = "cvNombre"
        ddlTipoTactica.DataValueField = "ciIdRel"
        ddlTipoTactica.DataBind()

    End Sub

    Public Function fnUltimaCompraDias(CardCode As String) As Int32
        Dim dias As Int32

        ssql = "SELECT TOP 1 datediff(day,ISNULL(DocDate,'20000101'),getdate()) as dias FROM IP..OINV where cardCode=" & "'" & CardCode & "' order by docdate desc "
        Dim dtUltima As New DataTable
        dtUltima = objDatos.fnEjecutarConsulta(ssql)
        If dtUltima.Rows.Count = 0 Then
            dias = 0
        Else
            dias = dtUltima.Rows(0)(0)
        End If


        Return dias
    End Function


    Public Sub fnProductoEmpuje()
        Dim sCardCode As String = "PRPISA94"

        If Request.QueryString.Count > 0 Then
            sCardCode = Request.QueryString("cliente")

        End If
        Dim sFabricadoEn As String = fnTipoProducto(sCardCode)

        ssql = "SELECT T0.ItemCode ,T2.ItemName, T0.ItemCode + ' - ' + T2.ItemName as Descripcion, T0.Price  FROM ITM1 T0 INNER JOIN OPLN T1 on T1.ListNum = T0.PriceList INNER JOIN OITM T2 on T2.ItemCode =T0.ItemCode WHERE T2.U_GrupoMaestro='Productos Pegaduro' AND T2.U_VendidoEn in(" & sFabricadoEn & ") AND T1.ListName ='" & ListaPrecios & "' and Price >0 " _
& "AND T0.ItemCode not in (select top 3 articulo collate SQL_Latin1_General_CP1_CI_AS FROM(select Articulo,sum(toneladas)as toneladas from Satelite_SAP .sistemas.Eficiencia where codigo='" & sCardCode & "' and fecha between getdate()-120 and getdate()   group by articulo)X order by toneladas desc) Order By Descripcion "
        Dim dtProductos As New DataTable
        dtProductos = objDatos.fnEjecutarConsulta(ssql)

        Dim fila As DataRow
        fila = dtProductos.NewRow
        fila("Itemcode") = "0"
        fila("Descripcion") = "-Seleccione-"
        dtProductos.Rows.Add(fila)
        ddlProductos.DataSource = dtProductos
        ddlProductos.DataTextField = "Descripcion"
        ddlProductos.DataValueField = "ItemCode"
        ddlProductos.DataBind()
        ddlProductos.SelectedValue = "0"

        ssql = "SELECT T0.ItemCode ,T2.ItemName, T0.ItemCode + ' - ' + T2.ItemName as Descripcion, T0.Price  FROM ITM1 T0 INNER JOIN OPLN T1 on T1.ListNum = T0.PriceList INNER JOIN OITM T2 on T2.ItemCode =T0.ItemCode WHERE T2.U_GrupoMaestro='Productos Pegaduro' AND T2.U_VendidoEn in(" & sFabricadoEn & ") AND  T1.ListName ='" & ListaPrecios & "' and Price >0 " _
& "AND T0.ItemCode not in (select top 3 articulo collate SQL_Latin1_General_CP1_CI_AS FROM(select Articulo,sum(toneladas)as toneladas from Satelite_SAP .sistemas.Eficiencia where codigo='" & sCardCode & "' and fecha between getdate()-120 and getdate() group by articulo)X order by toneladas desc) Order By Descripcion "
        Dim dtProductoB As New DataTable
        dtProductoB = objDatos.fnEjecutarConsulta(ssql)
        fila = dtProductoB.NewRow
        fila("Itemcode") = "0"
        fila("Descripcion") = "-Seleccione-"
        dtProductoB.Rows.Add(fila)

        ddlProductoB.DataSource = dtProductoB
        ddlProductoB.DataTextField = "Descripcion"
        ddlProductoB.DataValueField = "ItemCode"
        ddlProductoB.DataBind()
        ddlProductoB.SelectedValue = "0"

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

        If lblListaPrecios.Text.Contains("9") Then
            iTipoLista = 9
        End If

        If lblListaPrecios.Text.Contains("3") Then
            iTipoLista = 3
        End If


    End Sub

    Public Function fnTipoProducto(SN As String) As String
        Dim sProps As String = ""

        ssql = "SELECT QryGroup1,QryGroup2,QryGroup3,QryGroup4,QryGroup9,QryGroup11,QryGroup12,QryGroup14,QryGroup15,QryGroup16,QryGroup40 FROM IP..OCRD where CardCode =" & "'" & SN & "'"
        Dim dtCliente As New DataTable
        dtCliente = objDatos.fnEjecutarConsulta(ssql)

        If dtCliente.Rows(0)("QryGroup1") = "Y" Then
            sProps = sProps & "'Cot',"
        End If

        If dtCliente.Rows(0)("QryGroup2") = "Y" Then
            sProps = sProps & "'Gdl',"
        End If

        If dtCliente.Rows(0)("QryGroup3") = "Y" Then
            sProps = sProps & "'Cot',"
        End If

        If dtCliente.Rows(0)("QryGroup4") = "Y" Then
            sProps = sProps & "'Cul',"
        End If

        If dtCliente.Rows(0)("QryGroup9") = "Y" Then
            sProps = sProps & "'Cot',"
        End If
        If dtCliente.Rows(0)("QryGroup11") = "Y" Then
            sProps = sProps & "'Paz',"
        End If

        If dtCliente.Rows(0)("QryGroup12") = "Y" Then
            sProps = sProps & "'Sah',"
        End If

        If dtCliente.Rows(0)("QryGroup14") = "Y" Then
            sProps = sProps & "'Tij',"
        End If

        If dtCliente.Rows(0)("QryGroup15") = "Y" Then
            sProps = sProps & "'Qro',"
        End If

        If dtCliente.Rows(0)("QryGroup16") = "Y" Then
            sProps = sProps & "'Cun',"
        End If

        If dtCliente.Rows(0)("QryGroup1") = "Y" And dtCliente.Rows(0)("QryGroup40") = "Y" Then

            sProps = sProps & "'All','QRO',"

        Else
            If dtCliente.Rows(0)("QryGroup40") = "Y" Then
                sProps = sProps & "'Qro',"
            End If
        End If
        sProps = sProps & "'All',"
        sProps = sProps.Substring(0, sProps.Length - 1)
        Return sProps
    End Function



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

        If iMesCorte = 1 Then
            iMesCorte = 12
            iAño = iAño - 1
        End If
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
        PromPrecio = (PrecioProm / 4) * 0.985

    End Sub

    Public Sub fnToneladasPrecioUltimosMeses(Meses As Int16)
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
        If iMesCorte = 1 Then
            iMesCorte = 12
            iAño = iAño - 1
        End If


        For i = 1 To Meses

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



        PromTons = SumaTons / Meses
        PromPrecio = (PrecioProm / Meses) * 0.985

    End Sub
    Public Sub fnClienteSinEstrategia()

        Dim UtilidadMensual As Double = 0
        Dim utilidadAl30 As Double = 0

        UtilidadMensual = PromTons * PromPrecio
        utilidadAl30 = UtilidadMensual * (1 + (iTipoLista / 100)) * 0.3

        lblTonsProm.Text = PromTons.ToString("N2")
        _sinTonsVend4m = PromTons
        _sinUtilidadEsperada = utilidadAl30
        _sinUtilidadAdicional = (UtilidadMensual * (iTipoLista / 100))
        _sinUtilidadTot = utilidadAl30 + _sinUtilidadAdicional
        '  lblPrecioProm.Text = PromPrecio.ToString("C2")
        ' lblTotCostoMensual.Text = UtilidadMensual.ToString("C2")
        'lblUtilidad.Text = utilidadAl30.ToString("C2")
        'lblUtilidadPorDif.Text = (UtilidadMensual * (iTipoLista / 100)).ToString("C2")
        'lblUtilidadIntegrada.Text = (utilidadAl30 + (UtilidadMensual * (iTipoLista / 100))).ToString("C2")


    End Sub

    Public Sub fnClienteConEstrategia()
        Dim UtilidadMensual As Double = 0
        Dim utilidadAl30 As Double = 0
        Dim UtilidadIntegrada = (_sinUtilidadAdicional + (_sinUtilidadEsperada * (iTipoLista / 100))).ToString("C2")
        If TonsEmpuje = 0 Then
            PromTons = PromTons * (1 + porcSetPoint)
        Else
            PromTons = PromTons + TonsEmpuje
            ' PromTons = TonsEmpuje
        End If


        UtilidadMensual = PromTons * PromPrecio
        utilidadAl30 = UtilidadMensual * (1 + (iTipoLista / 100)) * 0.3

        lblTonsProme.Text = PromTons.ToString("N2")
        ' lblprecioProme.Text = PromPrecio.ToString("C2")
        Dim TotalCostoe = UtilidadMensual
        Dim utilidade = utilidadAl30
        Dim UtilidadPorDife = (UtilidadMensual * (iTipoLista / 100))
        '  lblUtilidadIntegradae.Text = (utilidadAl30 + (UtilidadMensual * (iTipoLista / 100))).ToString("C2")

        If porcEmpuje = 0 Then
            txtToneladas.Text = TonsEmpuje
        Else
            txtToneladas.Text = (PromTons * porcEmpuje).ToString("N2")
        End If



        Dim CostoRealCliente As Double
        CostoRealCliente = fnCostoCliente()

        Try
            Dim UtilidadProducto = ((CDbl(txtPrecioVtaPublico.Text.Replace("$", "").Replace(",", "")) - CostoRealCliente) * CDbl(txtToneladas.Text)).ToString("C2")
            Dim UtilidadDistTotal = UtilidadPorDife

            Dim TotalCostoProducto = (CDbl(txtToneladas.Text) * CostoRealCliente)

            Dim Nc = (TotalCostoProducto + TotalCostoe) * porcNC
            Dim UtilidadTotal = (UtilidadProducto + utilidade + Nc)
            Dim UtilidadIntegradaTotal = (UtilidadDistTotal + UtilidadTotal)

            Dim PorcentajeAumento = ((UtilidadIntegradaTotal / UtilidadIntegrada)) - 1

            _conTonsVend4m = PromTons
            _conUtilidadEsperada = utilidadAl30
            _conUtilidadAdicional = (UtilidadMensual * (iTipoLista / 100))
            _conUtilidadObtenida = UtilidadProducto
            _conPorcAumentoNC = Nc
            _conUtilidadTot = _conUtilidadAdicional + _conUtilidadEsperada + _conUtilidadObtenida + Nc

            GananciaMarginal = _conUtilidadTot - _sinUtilidadTot
            PorcGanancia = ((_conUtilidadTot * 100) / _sinUtilidadTot) - 100
        Catch ex As Exception

        End Try


    End Sub

    Public Function fnCostoCliente()
        Dim costo As Double = 0
        ssql = "select Price ,SWeight1 as Peso from IP..ITM1 T0 INNER JOIN IP..OITM T1 ON T1.ItemCode = T0.ItemCode INNER JOIN IP..OPLN T2 on T0.PriceList =T2.ListNum  where T0.ItemCode ='" & ddlProductos.SelectedValue & "' and T2.ListName ='" & lblListaPrecios.Text & "'"
        Dim dtPrecio As New DataTable
        dtPrecio = objDatos.fnEjecutarConsulta(ssql)

        If dtPrecio.Rows.Count > 0 Then
            Dim peso As Double = 0
            peso = dtPrecio.Rows(0)("Peso")
            costo = dtPrecio.Rows(0)("Price")

            costo = (1000 * costo / peso)
        End If
        'lblCosto.Text = costo
        Return costo
    End Function
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        'Response.Redirect("vistaprevia.aspx")

        Dim dts As New DataSet
        dts = New dtsDatos

        Dim dtReporte As New DataTable
        dtReporte = New dtsDatos.datosDataTable

        Dim dtDetalles As New DataTable
        dtDetalles = fnDetalleTactica(ddlTipoTactica.SelectedValue)
        If dtDetalles.Rows.Count > 0 Then
            fnToneladasPrecioUltimosMeses(CInt(dtDetalles.Rows(0)("ciMesesPromedioVenta")))

        Else
            fnToneladasPrecioUltimosMeses()
        End If


        fnTipoLista(lblListaPrecios.Text)
        fnClienteSinEstrategia()
        fnClienteConEstrategia()


        Dim fila As DataRow
        fila = dtReporte.NewRow
        fila("Distribuidor") = lblRazonSocial.Text
        fila("Fecha") = Now.Date.ToShortDateString
        fila("TonsVend4mSin") = _sinTonsVend4m
        fila("UtilidadEsperadaSin") = _sinUtilidadEsperada
        fila("UtilidadAdicionalSin") = _sinUtilidadAdicional
        fila("UtilidadTotSin") = _sinUtilidadTot

        fila("TonsVend4mCon") = _conTonsVend4m
        fila("Utilidadobtenida") = _conUtilidadObtenida
        fila("UtilidadEsperadaCon") = _conUtilidadEsperada
        fila("UtilidadAdicionalCon") = _conUtilidadAdicional
        fila("UtilidadTotCon") = _conUtilidadTot
        fila("PorcAumento") = _conPorcAumentoNC

        fila("GananciaMarginal") = GananciaMarginal
        fila("PorcGanancia") = PorcGanancia


        If ddlProductos.SelectedValue = "0" Then
            fila("Producto1") = ""
        Else
            fila("Producto1") = ddlProductos.SelectedItem.Text
        End If
        If ddlProductoB.SelectedValue = "0" Then
            fila("Producto2") = ""
        Else
            fila("Producto2") = ddlProductoB.SelectedItem.Text
        End If

        fila("del") = txtDel.Text
        fila("al") = txtAl.Text

        dtReporte.Rows.Add(fila)

        ssql = "select cvCardCode,cvCardName,CvVendedor,cfPromTons ,cfPromTonsMAs5 ,cfPrecioTonDistribuidor,cvProductoA,cvProductoB,cfTonsObjetivo ,cfNC,cfPorcUtilidadEsperada ,cdFechaTactica ,cdFechainicio ,cdFechaFin,cvArchivo,cvEstatus   from Satelite_SAP.[VENTAS].[Tactica]"


        Dim sNombreArchivo As String = "Tactica_" & Request.QueryString("cliente") & "_" & ddlProductos.SelectedValue & ".pdf"
        ssql = "INSERT INTO Satelite_SAP.[VENTAS].[Tactica] ( cvCardCode,cvCardName,CvVendedor,cfPromTons ,cfPromTonsMAs5 ,cfPrecioTonDistribuidor,cvProductoA,cvProductoB,cfTonsObjetivo ,cfNC,cfPorcUtilidadEsperada ,cdFechaTactica ,cdFechainicio ,cdFechaFin,cvArchivo,cvEstatus) VALUES (" _
            & "'" & Request.QueryString("cliente") & "'," _
            & "'" & lblRazonSocial.Text & "'," _
            & "'" & fnVendedor(Request.QueryString("cliente")) & "'," _
            & "'" & _sinTonsVend4m & "'," _
            & "'" & _conTonsVend4m & "'," _
            & "'" & txtPrecioVtaPublico.Text.Replace("$", "").Replace(",", "") & "'," _
            & "'" & ddlProductos.SelectedValue & "'," _
            & "'" & ddlProductoB.SelectedValue & "'," _
            & "'" & txtToneladas.Text & "'," _
            & "'" & _conPorcAumentoNC & "'," _
            & "'" & PorcGanancia & "', GETDATE()," _
            & "'" & txtDel.Text.Replace("-", "") & "'," _
            & "'" & txtAl.Text.Replace("-", "") & "','" & sNombreArchivo & "','CREADA')"
        objDatos.fnEjecutarInsert(ssql)
        Dim reporte As New ReportDocument
        reporte.Load(Server.MapPath("~") & "\formato\Reporte_layout.rpt")
        reporte.SetDataSource(dtReporte)
        reporte.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~") & "\archivos\" & sNombreArchivo)
        reporte.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, sNombreArchivo)
        reporte.Dispose()

    End Sub

    Public Function fnVendedor(cardCode As String) As String

        ssql = "SELECT slpName FROM OSLP where slpCode = (Select slpcode FROM OCRD where cardcode ='" & cardCode & "')"
        Dim dtVendedor As New DataTable
        dtVendedor = objDatos.fnEjecutarConsulta(ssql)
        If dtVendedor.Rows.Count > 0 Then
            sVendedor = dtVendedor.Rows(0)(0)
        End If

        Return sVendedor
    End Function

    Private Sub ddlTipoTactica_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipoTactica.SelectedIndexChanged
        fnCargaParametrosTactica()
    End Sub

    Private Function fnDetalleTactica(IdTactica As Int16)
        ssql = "select cfSetPoint,cfEmpuje,cfNC,cfPeriodo,cfToneladasSetPoint,cfToneladasEmpuje,cvLeyendaTons,ciMesesPromedioVenta,ISNULL(cvLeyendaEtiquetaTonsProm,'') as cvLeyendaEtiquetaTonsProm,tipoOperacion from Satelite_SAP .ventas.tacticas_Config where ciIdRel=" & "'" & IdTactica & "'"
        Dim dtDetalles As New DataTable
        dtDetalles = objDatos.fnEjecutarConsulta(ssql)
        Return dtDetalles
    End Function

    Private Sub fnCargaParametrosTactica()
        Try
            Dim dtDetalles As New DataTable
            dtDetalles = fnDetalleTactica(ddlTipoTactica.SelectedValue)
            If dtDetalles.Rows.Count > 0 Then
                fnToneladasPrecioUltimosMeses(CInt(dtDetalles.Rows(0)("ciMesesPromedioVenta")))

                If CDbl(dtDetalles.Rows(0)("cfSetPoint")) = 0 Then
                    pnlTactica.Visible = False
                    If CDbl(dtDetalles.Rows(0)("cfToneladasEmpuje")) = 0 Then
                        lblEtiquetaTonsProm.Visible = False
                        lblTonsProm.Visible = False
                        pnlTacticaSinSetPoint.Visible = True
                        txtToneladas.Text = "0"
                    Else
                        pnlTacticaSinSetPoint.Visible = True
                    End If

                Else
                    porcSetPoint = CDbl(dtDetalles.Rows(0)("cfSetPoint")) / 100
                    pnlTactica.Visible = True
                    pnlTacticaSinSetPoint.Visible = False
                End If

                porcEmpuje = CDbl(dtDetalles.Rows(0)("cfEmpuje")) / 100
                porcNC = CDbl(dtDetalles.Rows(0)("cfNC")) / 100
                TonsEmpuje = CDbl(dtDetalles.Rows(0)("cfToneladasEmpuje"))

                fnClienteSinEstrategia()
                fnClienteConEstrategia()

                If CDbl(dtDetalles.Rows(0)("cfToneladasEmpuje")) = 0 Then
                Else
                    txtToneladas.Text = CDbl(dtDetalles.Rows(0)("cfToneladasEmpuje"))

                End If

                lblLeyendaTons.Text = dtDetalles.Rows(0)("cvLeyendaTons")

                If CStr(dtDetalles.Rows(0)("tipoOperacion")).Contains("PRIMERA") Then
                    lblEtiquetaTonsProm.Visible = False
                    lblTonsProm.Visible = False

                End If

                lblEtiquetaTonsProm.Text = dtDetalles.Rows(0)("cvLeyendaEtiquetaTonsProm")

            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtTonsObjetivo_TextChanged(sender As Object, e As EventArgs) Handles txtTonsObjetivo.TextChanged
        Try
            If porcEmpuje = 0 Then
                txtToneladas.Text = TonsEmpuje
            Else
                txtToneladas.Text = CDbl(txtTonsObjetivo.Text) * porcEmpuje
            End If

            PromTons = CDbl(txtToneladas.Text)
        Catch ex As Exception

        End Try
    End Sub


End Class
