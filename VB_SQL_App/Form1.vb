Imports System.Data.SqlClient

Public Class Form1

    ' Define the connection string (replace YOUR_SERVER_NAME with your server name)
    Private connectionString As String = "Data Source=DESKTOP-DBR7VNK\SQLEXPRESS;Initial Catalog=TestDB;Integrated Security=True"

    ' Variable to store the current selected user ID
    Private currentUserID As Integer = -1
    Private txtSelectedUser As Object

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Load products into the ComboBox when the form loads
        LoadProducts()
        ' Retrieve users to display in dgvUsers
        RetrieveUsers()
    End Sub

    ' Function to insert a user into the database
    Public Sub InsertUser(username As String, email As String)
        Using con As New SqlConnection(connectionString)
            Dim query As String = "INSERT INTO Users (Username, Email) VALUES (@Username, @Email)"
            Using cmd As New SqlCommand(query, con)
                ' Add parameters to prevent SQL injection
                cmd.Parameters.AddWithValue("@Username", username)
                cmd.Parameters.AddWithValue("@Email", email)
                con.Open()
                cmd.ExecuteNonQuery()
                MessageBox.Show("User inserted successfully.")
                ' Refresh the users list
                RetrieveUsers()
            End Using
        End Using
    End Sub

    ' Function to retrieve users from the database
    Public Sub RetrieveUsers()
        Using con As New SqlConnection(connectionString)
            Dim query As String = "SELECT * FROM Users"
            Using cmd As New SqlCommand(query, con)
                con.Open()
                Dim dt As New DataTable()
                dt.Load(cmd.ExecuteReader())
                dgvUsers.DataSource = dt
            End Using
        End Using
    End Sub

    ' Function to load products into the ComboBox
    Private Sub LoadProducts()
        Using con As New SqlConnection(connectionString)
            Dim query As String = "SELECT ProductID, ProductName FROM Products"
            Using cmd As New SqlCommand(query, con)
                con.Open()
                Dim dt As New DataTable()
                dt.Load(cmd.ExecuteReader())
                cmbProducts.DataSource = dt
                cmbProducts.DisplayMember = "ProductName"
                cmbProducts.ValueMember = "ProductID"
            End Using
        End Using
    End Sub

    ' Function to purchase a product
    Public Sub PurchaseProduct(userID As Integer, productID As Integer, quantity As Integer)
        Using con As New SqlConnection(connectionString)
            Dim query As String = "
                INSERT INTO UserProducts (UserID, ProductID, Quantity)
                VALUES (@UserID, @ProductID, @Quantity)"
            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@UserID", userID)
                cmd.Parameters.AddWithValue("@ProductID", productID)
                cmd.Parameters.AddWithValue("@Quantity", quantity)
                con.Open()
                cmd.ExecuteNonQuery()
                MessageBox.Show("Product purchased successfully.")
            End Using
        End Using
    End Sub

    ' Function to retrieve purchased products for a user
    Private Sub RetrievePurchases(userID As Integer)
        Using con As New SqlConnection(connectionString)
            Dim query As String = "
                SELECT UP.UserProductID, P.ProductName, UP.Quantity, UP.PurchaseDate
                FROM UserProducts UP
                INNER JOIN Products P ON UP.ProductID = P.ProductID
                WHERE UP.UserID = @UserID
                ORDER BY UP.PurchaseDate DESC"
            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@UserID", userID)
                con.Open()
                Dim dt As New DataTable()
                dt.Load(cmd.ExecuteReader())
                dgvPurchases.DataSource = dt
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
        ' Clear input fields after insertion
        txtUsername.Clear()
        txtEmail.Clear()
    End Sub

    ' Event handler for the Retrieve button click
    Private Sub btnRetrieve_Click(sender As Object, e As EventArgs) Handles btnRetrieve.Click
        RetrieveUsers()
    End Sub

    ' Event handler for the Purchase button click
    Private Sub btnPurchase_Click(sender As Object, e As EventArgs) Handles btnPurchase.Click
        If currentUserID = -1 Then
            MessageBox.Show("Please select a user first.")
            Return
        End If

        Dim productID As Integer = CInt(cmbProducts.SelectedValue)
        Dim quantity As Integer

        If Not Integer.TryParse(txtQuantity.Text.Trim(), quantity) OrElse quantity <= 0 Then
            MessageBox.Show("Please enter a valid quantity.")
            Return
        End If

        PurchaseProduct(currentUserID, productID, quantity)
        RetrievePurchases(currentUserID) ' Refresh the purchases DataGridView
    End Sub

    ' Event handler for selecting a user from dgvUsers
    Private Sub dgvUsers_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvUsers.CellClick
        ' Get the selected user's ID
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = dgvUsers.Rows(e.RowIndex)
            currentUserID = CInt(row.Cells("UserID").Value)
            txtSelectedUser.Text = row.Cells("Username").Value.ToString()
            ' Retrieve purchases for the selected user
            RetrievePurchases(currentUserID)
        End If
    End Sub

End Class
