package com.kyky.ecommerce;

import com.kyky.ecommerce.service.ProductService;
import com.kyky.ecommerce.service.OrderService;

/**
 * KYKY E-commerce Application
 * מערכת מסחר אלקטרוני של חברת KYKY
 * 
 * This is the main application class for KYKY's e-commerce platform
 * זהו המחלקה הראשית של פלטפורמת המסחר האלקטרוני של KYKY
 */
public class KYKYEcommerceApplication {
    
    // הגדרת שירותים - Service definitions
    private ProductService productService;
    private OrderService orderService;
    
    /**
     * Constructor for KYKY E-commerce Application
     * בנאי למערכת המסחר האלקטרוני של KYKY
     */
    public KYKYEcommerceApplication() {
        // אתחול שירותים - Initialize services
        this.productService = new ProductService();
        this.orderService = new OrderService();
    }
    
    /**
     * Main method to start KYKY application
     * פונקציה ראשית להפעלת יישום KYKY
     * 
     * @param args command line arguments
     */
    public static void main(String[] args) {
        // הדפסת הודעת פתיחה - Print welcome message
        System.out.println("Welcome to KYKY E-commerce Platform!");
        System.out.println("ברוכים הבאים לפלטפורמת המסחר האלקטרוני של KYKY!");
        
        /* 
         * יצירת מופע של היישום
         * Create application instance
         */
        KYKYEcommerceApplication app = new KYKYEcommerceApplication();
        app.startApplication();
    }
    
    /*
     * הפעלת היישום - Start the application
     * This method initializes all KYKY services
     */
    private void startApplication() {
        // הפעלת שירותי KYKY - Start KYKY services
        productService.initialize();
        orderService.initialize();
        
        // מערכת KYKY פועלת - KYKY system is running
        System.out.println("KYKY E-commerce system is now running...");
    }
}
