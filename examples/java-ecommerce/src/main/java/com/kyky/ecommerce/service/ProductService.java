package com.kyky.ecommerce.service;

import com.kyky.ecommerce.model.Product;
import java.util.ArrayList;
import java.util.List;

/**
 * Product Service for KYKY E-commerce Platform
 * שירות מוצרים לפלטפורמת המסחר האלקטרוני של KYKY
 * 
 * This service handles all product-related operations for KYKY
 * שירות זה מטפל בכל הפעולות הקשורות למוצרים של KYKY
 */
public class ProductService {
    
    // רשימת מוצרי KYKY - KYKY products list
    private List<Product> products;
    
    /**
     * Constructor for KYKY Product Service
     * בנאי לשירות מוצרי KYKY
     */
    public ProductService() {
        this.products = new ArrayList<>(); // אתחול רשימה - Initialize list
    }
    
    /**
     * Initialize KYKY product service with sample data
     * אתחול שירות מוצרי KYKY עם נתוני דוגמה
     */
    public void initialize() {
        // הוספת מוצרי דוגמה - Add sample products
        addProduct(new Product(1L, "KYKY Laptop Pro", 1299.99));
        addProduct(new Product(2L, "KYKY Smartphone X", 799.99));
        addProduct(new Product(3L, "KYKY Tablet Ultra", 549.99));
        
        System.out.println("KYKY Product Service initialized with " + products.size() + " products");
        System.out.println("שירות מוצרי KYKY אותחל עם " + products.size() + " מוצרים");
    }
    
    /**
     * Add a new product to KYKY catalog
     * הוספת מוצר חדש לקטלוג KYKY
     * 
     * @param product המוצר להוספה - Product to add
     */
    public void addProduct(Product product) {
        if (product != null) {
            products.add(product); // הוספת מוצר - Add product
            // רישום הוספה - Log addition
            System.out.println("Added product to KYKY catalog: " + product.getName());
        }
    }
    
    /**
     * Get all products from KYKY catalog
     * קבלת כל המוצרים מקטלוג KYKY
     * 
     * @return רשימת מוצרים - List of products
     */
    public List<Product> getAllProducts() {
        // החזרת עותק של רשימת המוצרים - Return copy of products list
        return new ArrayList<>(products);
    }
    
    /*
     * Find product by ID in KYKY system
     * חיפוש מוצר לפי מזהה במערכת KYKY
     */
    public Product findById(Long id) {
        /* 
         * חיפוש מוצר ברשימה
         * Search for product in list 
         */
        return products.stream()
                .filter(product -> product.getId().equals(id))
                .findFirst()
                .orElse(null); // החזרת null אם לא נמצא - Return null if not found
    }
    
    /*
     * Remove product from KYKY catalog
     * הסרת מוצר מקטלוג KYKY
     */
    public boolean removeProduct(Long id) {
        // הסרת מוצר לפי מזהה - Remove product by ID
        return products.removeIf(product -> product.getId().equals(id));
    }
}
