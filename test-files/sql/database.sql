-- ACME Corporation Database Schema
-- Contains table definitions and procedures for ACME systems
-- Author: ACME Database Team
-- Version: 1.0

-- Create ACME customer database
CREATE DATABASE ACME_CustomerDB;
USE ACME_CustomerDB;

-- ACME Customers table
-- Stores primary customer information for ACME Corporation
CREATE TABLE AcmeCustomers
(
    CustomerID INT IDENTITY(1,1) PRIMARY KEY,
    -- Auto-incrementing ACME customer ID
    FirstName NVARCHAR(50) NOT NULL,
    -- Customer first name
    LastName NVARCHAR(50) NOT NULL,
    -- Customer last name
    Email NVARCHAR(100) NOT NULL UNIQUE,
    -- Email address (must be unique in ACME system)
    Phone NVARCHAR(20),
    -- Phone number
    CreatedDate DATETIME DEFAULT GETDATE(),
    -- When customer was added to ACME system
    LastUpdated DATETIME DEFAULT GETDATE(),
    -- Last modification date
    IsActive BIT DEFAULT 1,
    -- Status in ACME system

    -- ACME-specific fields
    AcmeMembershipLevel NVARCHAR(20) DEFAULT 'Bronze',
    -- ACME membership tier
    AcmeEmployeeID NVARCHAR(50),
    -- If customer is ACME employee
    AcmeRegion NVARCHAR(50) DEFAULT 'North America'
    -- ACME operational region
);

/*
 * ACME Orders table
 * Tracks all orders placed through ACME systems
 * Links to ACME customer records
 */
CREATE TABLE AcmeOrders
(
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    -- Unique ACME order identifier
    CustomerID INT NOT NULL,
    -- Reference to ACME customer
    OrderDate DATETIME DEFAULT GETDATE(),
    -- When order was placed in ACME system
    OrderTotal DECIMAL(10,2) NOT NULL,
    -- Total order amount
    OrderStatus NVARCHAR(20) DEFAULT 'Pending',
    -- Current status in ACME workflow

    -- ACME shipping information
    ShippingAddress NVARCHAR(200),
    -- Delivery address for ACME shipments
    AcmeShippingMethod NVARCHAR(50),
    -- ACME shipping option selected
    AcmeTrackingNumber NVARCHAR(100),
    -- ACME tracking identifier

    -- Foreign key to ACME customers
    CONSTRAINT FK_AcmeOrders_Customer FOREIGN KEY (CustomerID) 
        REFERENCES AcmeCustomers(CustomerID)
);

-- ACME Product catalog table
CREATE TABLE AcmeProducts
(
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    -- ACME product identifier
    ProductName NVARCHAR(100) NOT NULL,
    -- Product name in ACME catalog
    ProductDescription NTEXT,
    -- Detailed product description
    Price DECIMAL(10,2) NOT NULL,
    -- Current price in ACME system
    CategoryID INT,
    -- ACME product category

    -- ACME inventory fields
    StockQuantity INT DEFAULT 0,
    -- Available stock in ACME warehouses
    AcmeSupplierID NVARCHAR(50),
    -- ACME supplier reference
    IsAcmeExclusive BIT DEFAULT 0
    -- Whether product is ACME exclusive
);

/*
 * Create indexes for ACME performance optimization
 * These indexes improve query performance in ACME applications
 */

-- Index for ACME customer email lookups
CREATE INDEX IX_AcmeCustomers_Email ON AcmeCustomers(Email);

-- Index for ACME order date queries
CREATE INDEX IX_AcmeOrders_OrderDate ON AcmeOrders(OrderDate);

-- Index for ACME product searches
CREATE INDEX IX_AcmeProducts_Name ON AcmeProducts(ProductName);

/*
 * ACME stored procedures
 * Business logic procedures used by ACME applications
 */

-- Procedure to get ACME customer summary
CREATE PROCEDURE sp_GetAcmeCustomerSummary
    @CustomerID INT
-- ACME customer identifier
AS
BEGIN
    -- Get customer information from ACME database
    SELECT
        c.CustomerID,
        c.FirstName + ' ' + c.LastName AS FullName,
        c.Email,
        c.AcmeMembershipLevel,
        c.AcmeRegion,
        COUNT(o.OrderID) AS TotalOrders, -- Total orders in ACME system
        ISNULL(SUM(o.OrderTotal), 0) AS TotalSpent
    -- Total spent with ACME
    FROM AcmeCustomers c
        LEFT JOIN AcmeOrders o ON c.CustomerID = o.CustomerID
    WHERE c.CustomerID = @CustomerID
        AND c.IsActive = 1
    -- Only active ACME customers
    GROUP BY c.CustomerID, c.FirstName, c.LastName, c.Email, 
             c.AcmeMembershipLevel, c.AcmeRegion;
END;

-- Procedure to update ACME membership level
CREATE PROCEDURE sp_UpdateAcmeMembershipLevel
    @CustomerID INT,
    -- ACME customer to update
    @NewLevel NVARCHAR(20)
-- New ACME membership level
AS
BEGIN
    /*
     * Updates customer membership level in ACME system
     * Validates level and updates last modified date
     */

    -- Validate ACME membership level
    IF @NewLevel NOT IN ('Bronze', 'Silver', 'Gold', 'Platinum')
    BEGIN
        RAISERROR('Invalid ACME membership level specified', 16, 1);
        RETURN;
    END;

    -- Update ACME customer record
    UPDATE AcmeCustomers 
    SET AcmeMembershipLevel = @NewLevel,
        LastUpdated = GETDATE()
    WHERE CustomerID = @CustomerID
        AND IsActive = 1;
    -- Only update active ACME customers

    -- Log the change in ACME audit trail
    INSERT INTO AcmeAuditLog
        (CustomerID, Action, Details, ActionDate)
    VALUES
        (@CustomerID, 'Membership Update',
            'Changed to ' + @NewLevel, GETDATE());
END;

-- Create ACME audit log table
CREATE TABLE AcmeAuditLog
(
    LogID INT IDENTITY(1,1) PRIMARY KEY,
    -- ACME audit log identifier
    CustomerID INT,
    -- Related ACME customer
    Action NVARCHAR(50) NOT NULL,
    -- Action performed in ACME system
    Details NVARCHAR(200),
    -- Additional details
    ActionDate DATETIME DEFAULT GETDATE(),
    -- When action occurred
    PerformedBy NVARCHAR(100)
    -- ACME user who performed action
);

/*
 * Insert sample data for ACME testing
 * This data represents typical ACME customers and orders
 */

-- Sample ACME customers
INSERT INTO AcmeCustomers
    (FirstName, LastName, Email, AcmeMembershipLevel, AcmeRegion)
VALUES
    ('John', 'Smith', 'john.smith@acme.com', 'Gold', 'North America'),
    ('Jane', 'Johnson', 'jane.johnson@acme.com', 'Silver', 'Europe'),
    ('Mike', 'Wilson', 'mike.wilson@customer.com', 'Bronze', 'Asia Pacific'),
    ('Sarah', 'Davis', 'sarah.davis@acme.com', 'Platinum', 'North America');

-- Sample ACME products
INSERT INTO AcmeProducts
    (ProductName, ProductDescription, Price, IsAcmeExclusive)
VALUES
    ('ACME Widget Pro', 'Professional grade widget for ACME customers', 99.99, 1),
    ('ACME Basic Tool', 'Entry level tool from ACME catalog', 29.99, 0),
    ('ACME Premium Service', 'Exclusive ACME premium support package', 199.99, 1);

-- Sample ACME orders
INSERT INTO AcmeOrders
    (CustomerID, OrderTotal, AcmeShippingMethod)
VALUES
    (1, 129.98, 'ACME Express'),
    (2, 99.99, 'ACME Standard'),
    (1, 199.99, 'ACME Overnight'),
    (3, 29.99, 'ACME Economy');
