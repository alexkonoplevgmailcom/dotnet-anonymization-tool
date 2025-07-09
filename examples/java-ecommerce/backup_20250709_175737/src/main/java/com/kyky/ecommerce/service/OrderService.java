package com.MyCompany.ecommerce.service;
public class OrderService {
    private Long orderCounter;
    public OrderService() {
        this.orderCounter = 0L; 
    }
    public void initialize() {
        System.out.println("MyCompany Order Service initialized successfully");
        System.out.println("שירות הזמנות MyCompany אותחל בהצלחה");
    }
    public Long createOrder(Long customerId, Long productId) {
        orderCounter++;
        System.out.println("MyCompany Order #" + orderCounter + " created for customer " + customerId);
        return orderCounter;
    }
    public boolean processPayment(Long orderId, Double amount) {
        System.out.println("Processing payment for MyCompany order #" + orderId + " - Amount: $" + amount);
        return true; 
    }
}