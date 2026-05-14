Imports System.Data.SqlClient
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class MainForm

    Private Const ConnString As String =
        "Server=172.23.5.95\SQLEXPRESS,1433;Database=SWO_result;User Id=user;Password=user;"

    Private dbConnection As SqlConnection

    ' ── Form Load: start async connection ───────────────────────────
    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblStatusDB.Text = "Connecting..."
        lblStatusDB.ForeColor = Color.Orange
        ConnectAsync()
    End Sub

    ' ── Form Closing: close the database connection ─────────────────
    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If dbConnection IsNot Nothing Then
            dbConnection.Close()
        End If
    End Sub

    ' ── Async connect and update status label ───────────────────────
    Private Async Sub ConnectAsync()
        Try
            dbConnection = New SqlConnection(ConnString)
            Await dbConnection.OpenAsync()
            UpdateConnectionLabel()
        Catch ex As Exception
            UpdateConnectionLabel()
            lblDBResponse.Text = "Connection error: " & ex.Message
        End Try
    End Sub

    ' ── Update lblStatusDB from the real connection state ───────────
    Private Sub UpdateConnectionLabel()
        If dbConnection IsNot Nothing AndAlso dbConnection.State = ConnectionState.Open Then
            lblStatusDB.Text = "Connected"
            lblStatusDB.ForeColor = Color.Green
        Else
            lblStatusDB.Text = "Disconnected"
            lblStatusDB.ForeColor = Color.Red
        End If
    End Sub

    ' ── Enter key in DMC textbox: send payload to SQL ───────────────
    Private Sub txtBoxDMC_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBoxDMC.KeyDown
        If e.KeyCode <> Keys.Enter Then Return
        e.SuppressKeyPress = True

        Dim dmcValue As String = txtBoxDMC.Text.Trim()
        If dmcValue = "" Then
            lblDBResponse.Text = "Please enter a DMC value."
            Return
        End If

        If dbConnection Is Nothing OrElse dbConnection.State <> ConnectionState.Open Then
            UpdateConnectionLabel()
            lblDBResponse.Text = "No database connection."
            Return
        End If

        lblDBResponse.Text = "Processing..."
        txtBoxDMC.Enabled = False

        BuildAndSendPayloadAsync(dmcValue)
    End Sub

    ' ── Build JSON payload and call stored procedure async ──────────
    Private Async Sub BuildAndSendPayloadAsync(dmcValue As String)
        Try
            ' Build payload using Dictionary, then serialize to JSON
            Dim payloadObj = New Dictionary(Of String, Object) From {
                {"dmc", dmcValue}
            }
            Dim payload As String = JsonConvert.SerializeObject(payloadObj)

            ' Call stored procedure
            Dim result As String = Await ExecuteSprocAsync(payload)

            ' Deserialize response and extract success + message
            Dim resp = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(result)
            Dim success = If(resp IsNot Nothing AndAlso resp.ContainsKey("success"),
                             Convert.ToInt32(resp("success")), 0)
            Dim message = If(resp IsNot Nothing AndAlso resp.ContainsKey("message"),
                             resp("message")?.ToString(), result)

            If success = 1 Then
                lblDBResponse.Text = $"OK: {message}"
                lblDBResponse.ForeColor = Color.Green
            Else
                lblDBResponse.Text = $"ERROR: {message}"
                lblDBResponse.ForeColor = Color.Red
            End If

            UpdateConnectionLabel()
        Catch ex As Exception
            UpdateConnectionLabel()
            lblDBResponse.Text = "Error: " & ex.Message
            lblDBResponse.ForeColor = Color.Red
        Finally
            txtBoxDMC.Clear()
            txtBoxDMC.Enabled = True
            txtBoxDMC.SelectAll()
            txtBoxDMC.Focus()
        End Try
    End Sub

    ' ── Execute stored procedure, return output parameter value ─────
    Private Async Function ExecuteSprocAsync(payload As String) As Task(Of String)
        Using cmd As New SqlCommand("sp_check_dmc", dbConnection)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add("@payload", SqlDbType.NVarChar, -1).Value = payload

            Dim outParam As New SqlParameter("@result", SqlDbType.NVarChar, -1)
            outParam.Direction = ParameterDirection.Output
            cmd.Parameters.Add(outParam)

            Await cmd.ExecuteNonQueryAsync()

            Return outParam.Value.ToString()
        End Using
    End Function

End Class
