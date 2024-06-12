<%@ Page Language="VB" AutoEventWireup="false" CodeFile="tactica.aspx.vb" Inherits="tactica" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

  <!-- Required meta tags -->
  <meta charset="utf-8"/>
  <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no"/>
  <title>Skydash Admin</title>
  <!-- plugins:css -->
  <link rel="stylesheet" href="vendors/feather/feather.css"/>
  <link rel="stylesheet" href="vendors/ti-icons/css/themify-icons.css"/>
  <link rel="stylesheet" href="vendors/css/vendor.bundle.base.css"/>
  <!-- endinject -->
  <!-- Plugin css for this page -->
  <link rel="stylesheet" href="vendors/datatables.net-bs4/dataTables.bootstrap4.css"/>
  <link rel="stylesheet" href="vendors/ti-icons/css/themify-icons.css"/>
  <link rel="stylesheet" type="text/css" href="js/select.dataTables.min.css"/>
  <!-- End plugin css for this page -->
  <!-- inject:css -->
  <link rel="stylesheet" href="css/vertical-layout-light/style.css"/>
  <!-- endinject -->
  <link rel="shortcut icon" href="images/favicon.png" />

    <style type="text/css">
 .a_la_derecha
 {
    text-align :right ;
 }
 </style>

</head>
<body>
    <form id="form1" runat="server">
       <div class="container-scroller">
           <nav class="navbar col-lg-12 col-12 p-0 fixed-top d-flex flex-row">
      <div class="text-center navbar-brand-wrapper d-flex align-items-center justify-content-center">
        <a class="navbar-brand brand-logo mr-5" href="index.html"><img src="images/logo_pegaduro.jpeg" class="mr-2" alt="logo"/></a>
        <a class="navbar-brand brand-logo-mini" href="index.html"><img src="images/logo_pegaduro.jpeg" alt="logo"/></a>
      </div>
      <div class="navbar-menu-wrapper d-flex align-items-center justify-content-end">
        <button class="navbar-toggler navbar-toggler align-self-center" type="button" data-toggle="minimize">
          <span class="icon-menu"></span>
        </button>
        <%--<ul class="navbar-nav mr-lg-2" >
          <li class="nav-item nav-search d-none d-lg-block">
            <div class="input-group">
              <div class="input-group-prepend hover-cursor" id="navbar-search-icon">
                <span class="input-group-text" id="search">
                  <i class="icon-search"></i>
                </span>
              </div>
              <input type="text" class="form-control" id="navbar-search-input" placeholder="Search now" aria-label="search" aria-describedby="search">
            </div>
          </li>
        </ul>--%>
    
      </div>
    </nav>
           <div class="container-fluid page-body-wrapper">
               <!-- partial:../../partials/_settings-panel.html -->
               <div class="theme-setting-wrapper"> </div>

                   <nav class="sidebar sidebar-offcanvas" id="sidebar">
                     <ul class="nav">

                       <li class="nav-item">
            <a class="nav-link" data-toggle="collapse" href="#auth" aria-expanded="true" aria-controls="auth">
              <i class="icon-head menu-icon"></i>
              <span class="menu-title">Entidades</span>
              <i class="menu-arrow"></i>
            </a>
            <div class="collapse show" id="auth" style="">
              <ul class="nav flex-column sub-menu">
                <li class="nav-item"> <a class="nav-link" href="default.aspx"> Rutas </a></li>
                <li class="nav-item"> <a class="nav-link" href="Vendedores.aspx"> Vendedores </a></li>
                <li class="nav-item"> <a class="nav-link" href="Ciudades.aspx"> Ciudades </a></li>
              </ul>
            </div>
          </li>
</ul>
                   </nav>
                   <div class="main-panel">
                       <div class="content-wrapper">
                           <div class="row">
                               <div class="col-lg-12 grid-margin stretch-card">
                                   <div class="card">
                                       <div class="card-body">
                                           <h4 class="card-title">Generales del cliente</h4>

                                           <p class="card-description">
                                               Resumen
                                           </p>
                                           <div class="forms-sample">
                                               <div class="form-group row">
                                                   <label class="col-sm-3 col-form-label" for="exampleInputUsername2">Razón Social</label>
                                                   <div class="col-sm-9">
                                                       <asp:Label ID="lblRazonSocial" runat="server" Text="" class="form-control"></asp:Label>
                                                       
                                                   </div>
                                               </div>
                                                <div class="form-group row">
                                                   <label class="col-sm-3 col-form-label" for="exampleInputUsername2">Lista de Precios</label>
                                                   <div class="col-sm-3">
                                                       <asp:Label ID="lblListaPrecios" runat="server" Text="" class="form-control"></asp:Label>
                                                       
                                                   </div>  
                                                    <label class="col-sm-3 col-form-label" for="exampleInputUsername2">Condiciones Pago</label>
                                                   <div class="col-sm-3">
                                                       <asp:Label ID="lblCondiciones" runat="server" Text="" class="form-control"></asp:Label>
                                                       
                                                   </div>

                                               </div>
                                              
                                                <div class="form-group row">
                                                   <label class="col-sm-3 col-form-label" for="exampleInputUsername2">Límite de crédito</label>
                                                   <div class="col-sm-9">
                                                       <asp:Label ID="lblLimiteCredito" runat="server" Text="" class="form-control"></asp:Label>
                                                       
                                                   </div>
                                               </div>

                                                <div class="form-group row">
                                                
                                                     <asp:Label class="col-sm-9 col-form-label"  ID="lblRutaTactica" runat="server" Text="" ></asp:Label>
                                               </div>

                                                  <div class="form-group row">
                                                   <label class="col-sm-3 col-form-label" for="exampleInputUsername2">Tipo de táctica</label>
                                                   <div class="col-sm-9">
                                                       <asp:DropDownList ID="ddlTipoTactica" runat="server" AutoPostBack ="true" ></asp:DropDownList>
                                                       
                                                   </div>
                                               </div>
                                               

                                           </div>
                                       </div>
                                       </div>
                                   </div>
                                  </div>
                            <div class="row">

                                  <div class="col-lg-6 grid-margin stretch-card">
                                   <div class="card">
                                       <div class="card-body">
                                           <h4 class="card-title">Táctica</h4>

                                           <p class="card-description">
                                               
                                           </p>
                                               <div class="forms-sample">
                                                 <div class="form-group row">
                                                  <%-- <label class="col-sm-6 col-form-label" for="exampleInputUsername2">PROMEDIO TONS ULTIMOS 4 MESES</label>--%>
                                                     <asp:Label class="col-sm-6 col-form-label" for="exampleInputUsername2" ID="lblEtiquetaTonsProm" runat="server" Text="PROMEDIO TONS ULTIMOS 4 MESES"></asp:Label>
                                                   <div class="col-sm-6">
                                                       <asp:Label ID="lblTonsProm" runat="server" Text="" class="form-control a_la_derecha"></asp:Label>
                                                       
                                                   </div>
                                               </div>
                                                   <asp:Panel ID="pnlTactica" runat="server" Visible="true">
                                                       <div class="form-group row">
                                                           <label class="col-sm-6 col-form-label" for="exampleInputUsername2">TONS OBJETIVO PROMEDIO MAS 5 %</label>
                                                           
                                                           <div class="col-sm-6">
                                                               <asp:Label ID="lblTonsProme" runat="server" Text="" class="form-control a_la_derecha "></asp:Label>

                                                           </div>
                                                       </div>

                                                   </asp:Panel>

                                                   <asp:Panel ID="pnlTacticaSinSetPoint" runat="server" Visible="false" >
                                                         <div class="form-group row">
                                                       <%--    <label class="col-sm-6 col-form-label" for="exampleInputUsername2">TONS OBJETIVO</label>--%>
                                                             <asp:Label class="col-sm-6 col-form-label" for="exampleInputUsername2" ID="lblLeyendaTons" runat="server" Text="TONS OBJETIVO"></asp:Label>
                                                           <div class="col-sm-6">
                                                               <asp:TextBox ID="txtTonsObjetivo" runat="server" class="form-control a_la_derecha " AutoPostBack ="true" ></asp:TextBox>
                                                               

                                                           </div>
                                                       </div>

                                                   </asp:Panel>
                                     
                                                   <div class="form-group row">
                                                   <label class="col-sm-6 col-form-label" for="exampleInputUsername2">PRODUCTO DE EMPUJE: TECLEAR PRECIO DE VENTA (tonelada al público antes de IVA igual a Distribuidor +6)</label>
                                                   <div class="col-sm-6">
                                                       <asp:TextBox ID="txtPrecioVtaPublico" runat="server" class="form-control a_la_derecha "  ></asp:TextBox>

                                                   </div>
                                               </div>

                                                   

                                               
                                           </div>
                                           </div>
                                       </div>
                                   </div>
                               

                                 <div class="col-lg-6 grid-margin stretch-card">
                                   <div class="card">
                                       <div class="card-body">
 <p class="card-description">Productos de empuje </p>
                                               <div class="form-group row">

                                                   <div class="col-sm-6">
                                                       <asp:DropDownList ID="ddlProductos" runat="server" class="form-control"></asp:DropDownList>
                                                   </div>

                                                   <div class="col-sm-3">
                                                       <label class="col-form-label" for="exampleInputUsername2">Tons</label>

                                                   </div>
                                                   <div class="col-sm-3">
                                                       <asp:TextBox ID="txtToneladas" runat="server" class="form-control a_la_derecha "></asp:TextBox>
                                                   </div>
                                                   <div class="col-sm-6">
                                                       <asp:DropDownList ID="ddlProductoB" runat="server" class="form-control"></asp:DropDownList>
                                                   </div>
                                               </div>
                                                    <hr>
                                                <div class="form-group row">
                                                    <asp:Label ID="lblCosto" runat="server" Text="Label" Visible="false"></asp:Label>
                                                     <label class="col-sm-6 col-form-label" for="exampleInputUsername2">TEMPORALIDAD (DEBE TERMINAR EN DIA 15)</label>
</div>
                                                <div class="form-group row">
                                                               <div class="col-sm-3">
                                                                   <label class="col-form-label" for="exampleInputUsername2">DEL</label>

                                                               </div>
                                                    <div class="col-sm-3">
                                                        <asp:TextBox ID="txtDel" runat="server" TextMode="Date" ></asp:TextBox>
                                                        
                                                        </div>

                                                       </div>

                                                <div class="form-group row">
                                                               <div class="col-sm-3">
                                                                   <label class="col-form-label" for="exampleInputUsername2">AL</label>

                                                               </div>
                                                    <div class="col-sm-3">
                                                        <asp:TextBox ID="txtAl" runat="server" TextMode="Date" ></asp:TextBox>
                                                        
                                                        </div>

                                                       </div>

                                           
                                               <div class="form-group row">
                                                 
                                               <asp:Button ID="btnGuardar" runat="server" Text="Guardar"  class="btn btn-primary mr-2" />
                                                   <asp:Button ID="btnFormato" runat="server" Text="Generar Formato"  class="btn btn-primary mr-2" Visible="false"  />
                                                       </div>



                                       </div>
                                       </div>
                                     </div>

                                   </div>
                   </div>
              
          
    </form>

    <!-- plugins:js -->
  <script src="vendors/js/vendor.bundle.base.js"></script>
  <!-- endinject -->
  <!-- Plugin js for this page -->
  <script src="vendors/chart.js/Chart.min.js"></script>
  <script src="vendors/datatables.net/jquery.dataTables.js"></script>
  <script src="vendors/datatables.net-bs4/dataTables.bootstrap4.js"></script>
  <script src="js/dataTables.select.min.js"></script>

  <!-- End plugin js for this page -->
  <!-- inject:js -->
  <script src="js/off-canvas.js"></script>
  <script src="js/hoverable-collapse.js"></script>
  <script src="js/template.js"></script>
  <script src="js/settings.js"></script>
  <script src="js/todolist.js"></script>
  <!-- endinject -->
  <!-- Custom js for this page-->
  <script src="js/dashboard.js"></script>
  <script src="js/Chart.roundedBarCharts.js"></script>
  <!-- End custom js for this page-->
</body>
</html>


