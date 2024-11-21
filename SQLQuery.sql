CREATE DATABASE TestDB;
GO

USE TestDB;
GO

CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL
);


CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    ProductName VARCHAR(100) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL
);


CREATE TABLE UserProducts (
    UserProductID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    ProductID INT NOT NULL,
    PurchaseDate DATETIME DEFAULT GETDATE(),
    Quantity INT DEFAULT 1,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);


INSERT INTO Products (ProductName, Price) VALUES
('Product A', 10.99),
('Product B', 15.50),
('Product C', 7.25);
