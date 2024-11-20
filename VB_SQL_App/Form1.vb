
Imports System.Data.SqlClient

Public Class Form1

    ' Define the connection string (replace YOUR_SERVER_NAME with your server name)
    Private connectionString As String = "Data Source=DESKTOP-DBR7VNK\SQLEXPRESS;Initial Catalog=TestDB;Integrated Security=True"

    ' Function to insert a user into the database
    Private Sub InsertUser(username As String, email As String)
        Using con As New SqlConnection(connectionString)
            Dim query As String = "INSERT INTO Users (Username, Email) VALUES (@Username, @Email)"
            Using cmd As New SqlCommand(query, con)
                ' Add parameters to prevent SQL injection
                cmd.Parameters.AddWithValue("@Username", username)
                cmd.Parameters.AddWithValue("@Email", email)
                con.Open()
                cmd.ExecuteNonQuery()
                MessageBox.Show("User inserted successfully.")
            End Using
        End Using
    End Sub

    ' Function to retrieve users from the database
    Private Sub RetrieveUsers()
        Using con As New SqlConnection(connectionString)
            Dim query As String = "SELECT * FROM Users"
            Using cmd As New SqlCommand(query, con)
                con.Open()
                Dim dt As New DataTable()
                dt.Load(cmd.ExecuteReader())
                ' Bind the DataTable to the DataGridView
                dgvUsers.DataSource = dt
            End Using
        End Using
    End Sub

    ' Event handler for the Insert button click
    Private Sub btnInsert_Click(sender As Object, e As EventArgs) Handles btnInsert.Click
        Dim username As String = txtUsername.Text.Trim()
        Dim email As String = txtEmail.Text.Trim()

        ' Simple validation to ensure fields are not empty
        If String.IsNullOrEmpty(username) OrElse String.IsNullOrEmpty(email) Then
            MessageBox.Show("Please enter both username and email.")
            Return
        End If

        InsertUser(username, email)
    End Sub

    ' Event handler for the Retrieve button click
    Private Sub btnRetrieve_Click(sender As Object, e As EventArgs) Handles btnRetrieve.Click
        RetrieveUsers()
    End Sub

End Class
