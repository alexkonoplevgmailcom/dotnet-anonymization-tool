package com.MyCompany.ecommerce;
import com.MyCompany.ecommerce.service.ProductService;
import com.MyCompany.ecommerce.service.OrderService;
public class KYKYEcommerceApplication {
    private ProductService productService;
    private OrderService orderService;
    public KYKYEcommerceApplication() {
        this.productService = new ProductService();
        this.orderService = new OrderService();
    }
    public static void main(String[] args) {
        System.out.println("Welcome to MyCompany E-commerce Platform!");
        System.out.println("ברוכים הבאים לפלטפורמת המסחר האלקטרוני של MyCompany!");
        KYKYEcommerceApplication app = new KYKYEcommerceApplication();
        app.startApplication();
    }
    private void startApplication() {
        productService.initialize();
        orderService.initialize();
        System.out.println("MyCompany E-commerce system is now running...");
    }
}