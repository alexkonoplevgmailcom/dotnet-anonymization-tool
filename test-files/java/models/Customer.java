// Customer model for ACME Corporation
package com.acme.models;

/**
 * Represents a customer in the ACME system
 * Contains customer information and ACME-specific methods
 * 
 * @author ACME Development Team
 * @version 1.0
 */
public class Customer {
    
    // Customer's full name in ACME format
    private String name;
    
    /*
     * Customer email address
     * Must follow ACME email validation rules
     * Should end with @acme.com for internal customers
     */
    private String email;
    
    // Unique ACME customer identifier
    private String customerId;
    
    // Customer status in ACME system
    private boolean isActive;

    /**
     * Constructor for ACME Customer
     * 
     * @param name Customer's full name
     * @param email Customer's email address
     */
    public Customer(String name, String email) {
        this.name = name;
        this.email = email;
        this.customerId = generateAcmeCustomerId(); // Generate ACME ID
        this.isActive = true; // Default to active in ACME system
    }

    // Getters and setters for ACME customer properties
    
    /**
     * Gets customer name
     * @return Customer's full name
     */
    public String getName() {
        return name;
    }

    /*
     * Sets customer name
     * Validates against ACME naming conventions
     */
    public void setName(String name) {
        // TODO: Add ACME name validation
        this.name = name;
    }

    /**
     * Gets customer email
     * @return Customer's email address
     */
    public String getEmail() {
        return email;
    }

    /*
     * Sets customer email
     * Must pass ACME email validation
     */
    public void setEmail(String email) {
        // Validate ACME email format
        if (isValidAcmeEmail(email)) {
            this.email = email;
        }
    }

    /**
     * Generates unique ACME customer ID
     * Format: ACME_CUST_{timestamp}
     * 
     * @return Generated customer ID
     */
    private String generateAcmeCustomerId() {
        // Generate ACME-specific customer ID
        return "ACME_CUST_" + System.currentTimeMillis();
    }

    /*
     * Validates email against ACME standards
     * Checks format and domain restrictions
     */
    private boolean isValidAcmeEmail(String email) {
        // Basic ACME email validation
        return email != null && email.contains("@") && email.length() > 0;
    }

    /**
     * Checks if customer is internal ACME employee
     * @return true if email domain is @acme.com
     */
    public boolean isAcmeEmployee() {
        return email != null && email.endsWith("@acme.com");
    }
}
