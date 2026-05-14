Imports System.Data.SqlClient
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class MainForm

    Private Const ConnString As String =
        "Server=10.199.200.101\SQLEXPRESS,1433;Database=Sandbox;User Id=user;Password=user;"

    Private dbConnection As SqlConnection


    Private Const AppPassword As String = "ADMIN_123!"

    ' ── Form Load: check password, then start async connection ─────
    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim input = InputBox("Enter password:", "DMC Verifier — Login")

        If input <> AppPassword Then
            MessageBox.Show("Incorrect password.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Hand)
            Close()
            Return
        End If

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
            lblData.Text = "Connection error: " & ex.Message
        End Try
    End Sub

    ' ── Update lblStatusDB from the real connection state ───────────
    Private Sub UpdateConnectionLabel()
        If dbConnection IsNot Nothing AndAlso dbConnection.State = ConnectionState.Open Then
            lblStatusDB.Text = "Connected"
            lblStatusDB.ForeColor = Color.Black
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
            lblData.Text = "Please enter a DMC value."
            Return
        End If

        If dbConnection Is Nothing OrElse dbConnection.State <> ConnectionState.Open Then
            UpdateConnectionLabel()
            lblData.Text = "No database connection."
            Return
        End If

        lblData.Text = "Processing..."
        txtBoxDMC.Enabled = False

        BuildAndSendPayloadAsync(dmcValue)
    End Sub

    ' ── Build JSON payload and call stored procedure async ──────────

    Private Async Sub BuildAndSendPayloadAsync(dmcValue As String)
        Try
            ' Build payload using Dictionary, then serialize to JSON
            Dim payloadObj = New Dictionary(Of String, Object) From {
                {"DMC", dmcValue}
            }
            Dim payload As String = JsonConvert.SerializeObject(payloadObj)

            ' Call stored procedure
            Dim result As String = Await ExecuteSprocAsync(payload)

            ' Deserialize into strong model
            Dim response As SQLResponse = JsonConvert.DeserializeObject(Of SQLResponse)(result)

            Dim success As Integer = If(response?.success, 0)
            Dim message As String = response?.message
            Dim totalCount As Integer

            Dim rawParsedJson As String = JsonConvert.SerializeObject(result, Formatting.Indented)


            ' Extract all TotalCount values
            If response?.data IsNot Nothing Then
                For Each item In response.data
                    For Each kvp In item.Extra
                        Dim key As String = kvp.Key
                        Dim value As String = kvp.Value.ToString()
                        If key = "TotalCount" AndAlso Integer.TryParse(value, totalCount) Then
                            ' Found TotalCount, can break if only one expected
                            Exit For
                        End If
                    Next
                Next
            End If

            If success = 1 Then
                If totalCount = 0 Then
                    lblData.Text = "No records found."
                    Me.BackColor = Color.Red
                    lblMessage.Text = message
                    lblData.ForeColor = Color.Orange
                ElseIf totalCount = 1 Then
                    lblData.Text = "Found: " & String.Join(", ", totalCount)
                    Me.BackColor = Color.Green
                    lblMessage.Text = message
                    lblData.ForeColor = Color.LightGreen
                Else
                    lblData.Text = "Found: " & String.Join(", ", totalCount)
                    Me.BackColor = Color.Yellow
                    lblMessage.Text = message
                    lblData.ForeColor = Color.Green
                End If
            Else
                lblData.Text = $"ERROR: {message}"
                lblData.ForeColor = Color.Red
            End If

            UpdateConnectionLabel()
        Catch ex As Exception
            UpdateConnectionLabel()
            lblData.Text = "Error: " & ex.Message
            lblData.ForeColor = Color.Red
        Finally
            txtBoxDMC.Clear()
            txtBoxDMC.Enabled = True
            txtBoxDMC.SelectAll()
            txtBoxDMC.Focus()
        End Try
    End Sub

    ' ── Execute stored procedure, return output parameter value ─────
    Private Async Function ExecuteSprocAsync(payload As String) As Task(Of String)
        Using cmd As New SqlCommand("DC_check_ZSB_data", dbConnection)
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

Public Class SQLResponse
    Public Property success As Integer
    Public Property data As List(Of DataItem)
    Public Property message As String
End Class

Public Class DataItem
    <JsonExtensionData>
    Public Property Extra As IDictionary(Of String, JToken)
End Class
