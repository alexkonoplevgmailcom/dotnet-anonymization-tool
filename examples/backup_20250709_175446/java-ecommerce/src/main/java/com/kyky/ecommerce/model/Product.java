package com.kyky.ecommerce.model;

/**
 * Product model for KYKY E-commerce
 * מודל מוצר עבור מסחר אלקטרוני KYKY
 */
public class Product {
    
    // מזהה מוצר יחיד - Unique product identifier
    private Long id;
    
    // שם המוצר - Product name
    private String name;
    
    // מחיר המוצר - Product price
    private Double price;
    
    // תיאור המוצר - Product description
    private String description;
    
    // קטגוריית המוצר - Product category
    private String category;
    
    /**
     * Default constructor for KYKY Product
     * בנאי ברירת מחדל למוצר KYKY
     */
    public Product() {
        // אתחול ברירת מחדל - Default initialization
    }
    
    /**
     * Constructor with parameters for KYKY Product
     * בנאי עם פרמטרים למוצר KYKY
     * 
     * @param id מזהה המוצר - Product ID
     * @param name שם המוצר - Product name
     * @param price מחיר המוצר - Product price
     */
    public Product(Long id, String name, Double price) {
        this.id = id;
        this.name = name;
        this.price = price;
    }
    
    // Getter methods - פונקציות קבלת ערכים
    
    /**
     * Get product ID
     * קבלת מזהה המוצר
     */
    public Long getId() {
        return id; // החזרת מזהה - Return ID
    }
    
    /**
     * Get product name
     * קבלת שם המוצר
     */
    public String getName() {
        return name; // החזרת שם - Return name
    }
    
    // Setter methods - פונקציות הגדרת ערכים
    
    /**
     * Set product ID for KYKY system
     * הגדרת מזהה מוצר למערכת KYKY
     */
    public void setId(Long id) {
        this.id = id; // הגדרת מזהה - Set ID
    }
    
    /**
     * Set product name for KYKY catalog
     * הגדרת שם מוצר לקטלוג KYKY
     */
    public void setName(String name) {
        this.name = name; // הגדרת שם - Set name
    }
    
    /*
     * toString method for KYKY Product display
     * פונקציה להצגת מוצר KYKY
     */
    @Override
    public String toString() {
        return String.format("KYKY Product[id=%d, name=%s, price=%.2f]", 
                           id, name, price);
    }
}
