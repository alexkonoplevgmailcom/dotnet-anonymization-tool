package com.MyCompany.ecommerce.model;
public class Product {
    private Long id;
    private String name;
    private Double price;
    private String description;
    private String category;
    public Product() {
    }
    public Product(Long id, String name, Double price) {
        this.id = id;
        this.name = name;
        this.price = price;
    }
    public Long getId() {
        return id; 
    }
    public String getName() {
        return name; 
    }
    public void setId(Long id) {
        this.id = id; 
    }
    public void setName(String name) {
        this.name = name; 
    }
    @Override
    public String toString() {
        return String.format("MyCompany Product[id=%d, name=%s, price=%.2f]", 
                           id, name, price);
    }
}