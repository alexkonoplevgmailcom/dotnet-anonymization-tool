package com.MyCompany.ecommerce.service;
import com.MyCompany.ecommerce.model.Product;
import java.util.ArrayList;
import java.util.List;
public class ProductService {
    private List<Product> products;
    public ProductService() {
        this.products = new ArrayList<>(); 
    }
    public void initialize() {
        addProduct(new Product(1L, "MyCompany Laptop Pro", 1299.99));
        addProduct(new Product(2L, "MyCompany Smartphone X", 799.99));
        addProduct(new Product(3L, "MyCompany Tablet Ultra", 549.99));
        System.out.println("MyCompany Product Service initialized with " + products.size() + " products");
        System.out.println("שירות מוצרי MyCompany אותחל עם " + products.size() + " מוצרים");
    }
    public void addProduct(Product product) {
        if (product != null) {
            products.add(product); 
            System.out.println("Added product to MyCompany catalog: " + product.getName());
        }
    }
    public List<Product> getAllProducts() {
        return new ArrayList<>(products);
    }
    public Product findById(Long id) {
        return products.stream()
                .filter(product -> product.getId().equals(id))
                .findFirst()
                .orElse(null); 
    }
    public boolean removeProduct(Long id) {
        return products.removeIf(product -> product.getId().equals(id));
    }
}