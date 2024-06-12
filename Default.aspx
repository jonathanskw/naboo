<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

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
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/1.3.2/jspdf.min.js"></script>

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
                   <div class="main-panel" >
                       <div class="content-wrapper" id="imprimir">
                           
                           <div class="row">
                               <div class="col-lg-6 grid-margin stretch-card">
                                   <div class="card">
                                       <div class="card-body">
                                           <h4 class="card-title">Indicador por ruta</h4>
                                           <p class="card-description">
                                               <asp:DropDownList ID="ddlRutas" runat="server" AutoPostBack ="true" ></asp:DropDownList>
                                               </p>
                                           <p class="card-description">
                                              Toneladas por mes
                                           </p>
                                           <div class="table-responsive">
                                               <table class="table">
                                                   <thead>
                                                       <tr>
                                                           <asp:Panel ID="pnlEncabezado" runat="server"></asp:Panel>
                                                   
                                                       </tr>
                                                   </thead>
                                                   <tbody>
                                                        <asp:Panel ID="pnlLineas" runat="server"></asp:Panel>
                                                   
                                                   </tbody>
                                               </table>
                                           </div>
                                       </div>
                                   </div>
                               </div>
                                <div class="col-lg-6 grid-margin stretch-card">
                                   <div class="card">
                                       <div class="card-body">
                                           <h4 class="card-title">Indicador por ruta</h4>
                                           <p class="card-description">
                                               <asp:DropDownList ID="ddlRutasAcum" runat="server" AutoPostBack ="true" visible="false" ></asp:DropDownList>
                                               <br/>
                                               </p>
                                           <p class="card-description">
                                              Toneladas   <code> acumuladas </code> por mes
                                           </p>
                                           <div class="table-responsive">
                                               <table class="table">
                                                   <thead>
                                                       <tr>
                                                           <asp:Panel ID="pnlcolumnasruta2" runat="server"></asp:Panel>
                                                   
                                                       </tr>
                                                   </thead>
                                                   <tbody>
                                                        <asp:Panel ID="pnlFilasruta2" runat="server"></asp:Panel>
                                                   
                                                   </tbody>
                                               </table>
                                           </div>
                                       </div>
                                   </div>
                               </div>

                           </div>

                              <div class="row" id="porCliente">
                               <div class="col-lg-12 grid-margin stretch-card">
                                   <div class="card">
                                       <div class="card-body">
                                           <h4 class="card-title">Indicador por cliente</h4>
                                           
                                           <p class="card-description">
                                              Toneladas por mes
                                           </p>
                                           <div class="table-responsive">
                                               <table class="table">
                                                   <thead>
                                                       <tr>
                                                           <asp:Panel ID="pnlEncabezadoClientes" runat="server"></asp:Panel>
                                                   
                                                       </tr>
                                                   </thead>
                                                   <tbody>
                                                        <asp:Panel ID="pnlLineasCliente" runat="server"></asp:Panel>
                                                   
                                                   </tbody>
                                               </table>
                                           </div>
                                       </div>
                                   </div>
                               </div>
                           
                           </div>

                           
                       </div>
                        <div class="row" >
                             <div class="col-lg-6 grid-margin stretch-card">
                                  <a href="javascript:pruebaDivAPdf()" class="btn btn-primary mr-2" >Exportar a PDF</a>

                                  </div>
 </div>
                      
                   </div>
              
           </div>
      </div>

    </form>

    <script>
        function pruebaDivAPdf() {
            var pdf = new jsPDF('pl', 'mm', [612, 792]);

            source = $('#imprimir')[0];

            specialElementHandlers = {
                '#bypassme': function (element, renderer) {
                    return true
                }
            };
            margins = {
                top: 8,
                bottom: 6,
                left: 4,
                width: 52
            };

            pdf.fromHTML(
                source,
                margins.left, // x coord
                margins.top, { // y coord
                'width': margins.width,
                'elementHandlers': specialElementHandlers
            },

                function (dispose) {
                    pdf.save('Indicadores.pdf');
                }, margins
            );
        }
    </script>
    <script type="text/javascript">
 function fnClick(Cliente) {

                //alert('entra');

     var rate_value =1;

                try {
          
                            
                    PageMethods.fnGeneraIndicadorHijos(Cliente, onSucess, onError);

                function onSucess(result) {
                      alert(result);
                   
                    
                }

                function onError(result) {
                   
                    }
                } catch (error) {

                }

                       
            }
    </script>

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
