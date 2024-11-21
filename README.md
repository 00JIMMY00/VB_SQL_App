VB_SQL_App
A simple Visual Basic application that connects to an SQL Server database. The application allows users to be inserted into the database and enables users to purchase products. It demonstrates basic CRUD operations and handles a many-to-many relationship between users and products.

Table of Contents
Features
Prerequisites
Installation
Database Setup
Running the Application
Usage
Project Structure
Contributing
License
Features
Insert new users into the database.
Display a list of users.
Purchase products for a randomly selected user.
Display purchase history for users.
Many-to-many relationship between users and products using a linking table.
Basic input validation and error handling.
Prerequisites
Visual Studio (with Visual Basic support).
SQL Server or SQL Server Express.
.NET Framework installed on your system.
Basic knowledge of Visual Basic and SQL.
Installation
Clone the Repository

bash
Copy code
git clone https://github.com/yourusername/VB_SQL_App.git
Open the Solution in Visual Studio

Navigate to the cloned directory.
Open the VB_SQL_App.sln file with Visual Studio.
Restore NuGet Packages

Visual Studio should automatically restore any required packages.
If not, right-click on the solution and select Restore NuGet Packages.
Database Setup
1. Create the Database
Open SQL Server Management Studio.

Connect to your SQL Server instance.

Execute the following SQL script to create the TestDB database:

sql
Copy code
CREATE DATABASE TestDB;
GO
USE TestDB;
GO
2. Create Tables
Execute the following SQL scripts to create the necessary tables:

Users Table
sql
Copy code
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL
);
Products Table
sql
Copy code
CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    ProductName VARCHAR(100) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL
);
UserProducts Table
sql
Copy code
CREATE TABLE UserProducts (
    UserProductID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    ProductID INT NOT NULL,
    PurchaseDate DATETIME DEFAULT GETDATE(),
    Quantity INT DEFAULT 1,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);
3. Insert Sample Data
Insert Products
sql
Copy code
INSERT INTO Products (ProductName, Price) VALUES
('Product A', 10.99),
('Product B', 15.50),
('Product C', 7.25);
Running the Application
Update the Connection String

In the Form1.vb file, update the connectionString variable with your SQL Server instance name:

vb
Copy code
Private connectionString As String = "Data Source=YOUR_SERVER_NAME;Initial Catalog=TestDB;Integrated Security=True"
Replace YOUR_SERVER_NAME with your actual server name.

Build the Solution

In Visual Studio, go to Build > Build Solution.
Run the Application

Press F5 or click the Start button to run the application.
