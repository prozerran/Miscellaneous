
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Imports System.Data.SqlClient
Imports System.Data

Namespace SQL_vblib
    Public Class SQL_vb_connector

        Private conn As SqlConnection
        Private dt As DataTable

        Public Sub New(ip As String, db As String, user As String, pass As String)
            Connect(ip, db, user, pass)
        End Sub

        Public Sub Connect(ip As String, db As String, user As String, pass As String)
            Dim connStr As String = String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}",
                ip, db, user, pass)

            conn = New SqlConnection(connStr)
            conn.Open()
        End Sub

        Public Sub Disconnect()
            conn.Close()
        End Sub

        Public Function SelectTable(tbname As String) As DataTable
            Dim sql As String = String.Format("SELECT * FROM {0}", tbname)
            Dim command As SqlCommand = New SqlCommand(sql, conn)

            Dim dr As SqlDataReader = command.ExecuteReader()
            dt = New DataTable()
            dt.Load(dr)

            dr.Close()
            command.Dispose()
            Return dt
        End Function

        Public Sub UpdateTable(tbname As String, cust As String, id As Integer)
            Dim sql As String = String.Format("INSERT INTO {0} (Customer, id) Values ('{1}', {2})", tbname, cust, id)
            Dim command As SqlCommand = New SqlCommand(sql, conn)
            command.ExecuteNonQuery()
        End Sub

        Public Sub EmptyTable(tbname As String)
            Dim sql As String = string.Format("TRUNCATE TABLE {0}", tbname)
            Dim command As SqlCommand = New SqlCommand(sql, conn)
            command.ExecuteNonQuery()
        End Sub

    End Class
End Namespace


