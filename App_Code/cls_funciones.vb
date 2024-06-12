Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.VisualBasic

Public Class cls_funciones
    Public sqlCon As New SqlConnection
    Public Function fnConexion()
        sqlCon = New SqlConnection
        sqlCon.ConnectionString = "Server=PDGDL-SQL; Initial Catalog=IP; uid= sa; pwd=P3g4dur0;"
        Try
            sqlCon.Open()
        Catch ex As Exception

        End Try
        Return sqlCon
    End Function
    Public Function fnEjecutarConsulta(ByVal ssql As String)
        Dim dtDatos As New DataTable
        Dim cmd As SqlCommand
        Dim da As SqlDataAdapter
        Try
            fnConexion()
            If sqlCon.State = ConnectionState.Closed Then
                sqlCon.Open()
            End If
            cmd = New SqlCommand(ssql, sqlCon)
            da = New SqlDataAdapter(cmd)
            da.Fill(dtDatos)
            cmd.Connection.Close()
            cmd.Dispose()

        Catch ex As Exception

        End Try

        Return dtDatos
    End Function
    Public Sub fnEjecutarInsert(ByVal ssql As String)
        Dim dtDatos As New DataTable
        Dim cmd As SqlCommand
        Dim da As SqlDataAdapter
        Try
            fnConexion()
            If sqlCon.State = ConnectionState.Closed Then
                sqlCon.Open()
            End If
            cmd = New SqlCommand(ssql, sqlCon)
            cmd.ExecuteNonQuery()
            cmd.Connection.Close()
            cmd.Dispose()

        Catch ex As Exception

        End Try


    End Sub
End Class
