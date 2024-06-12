<%@ Page Language="VB" AutoEventWireup="false" CodeFile="familias.aspx.vb" Inherits="familias" %>

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
                                           <h4 class="card-title">Indicador por familias</h4>
                                           
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

