
Imports CrystalDecisions.CrystalReports.Engine

Partial Class vistaprevia
    Inherits System.Web.UI.Page

    Private Sub vistaprevia_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim reporte As New ReportDocument
        reporte.Load(Server.MapPath("~") & "\formato\Reporte_layout.rpt")
        crpviewer.ReportSource = reporte
        crpviewer.RefreshReport()
    End Sub
End Class
