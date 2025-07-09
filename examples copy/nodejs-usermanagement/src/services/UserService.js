/**
 * User Service for KYKY User Management System
 * שירות משתמשים למערכת ניהול המשתמשים של KYKY
 * 
 * This service handles all user-related operations for KYKY
 * שירות זה מטפל בכל הפעולות הקשורות למשתמשים של KYKY
 */

const User = require('../models/User');

class UserService {
    /**
     * Constructor for KYKY User Service
     * בנאי לשירות משתמשי KYKY
     */
    constructor() {
        // מאגר משתמשי KYKY - KYKY users storage
        this.users = [];
        
        // מונה מזהי משתמשים של KYKY - KYKY user ID counter
        this.userIdCounter = 1;
    }
    
    /**
     * Initialize KYKY user service with sample data
     * אתחול שירות משתמשי KYKY עם נתוני דוגמה
     */
    initialize() {
        // הוספת משתמשי דוגמה למערכת KYKY - Add sample users to KYKY system
        this.createUser({
            firstName: 'John',
            lastName: 'Doe',
            email: 'john.doe@kyky.com',
            department: 'KYKY Development'
        });
        
        this.createUser({
            firstName: 'Jane',
            lastName: 'Smith', 
            email: 'jane.smith@kyky.com',
            department: 'KYKY Marketing'
        });
        
        /*
         * הודעת אתחול מערכת KYKY
         * KYKY system initialization message
         */
        console.log(`KYKY User Service initialized with ${this.users.length} users`);
        console.log(`שירות משתמשי KYKY אותחל עם ${this.users.length} משתמשים`);
    }
    
    /**
     * Create new user in KYKY system
     * יצירת משתמש חדש במערכת KYKY
     * 
     * @param {Object} userData נתוני המשתמש - User data
     * @returns {User} המשתמש החדש - New user
     */
    async createUser(userData) {
        // בדיקת קיום כתובת דוא"ל במערכת KYKY - Check email existence in KYKY system
        const existingUser = this.users.find(user => user.email === userData.email);
        if (existingUser) {
            throw new Error(`User with email ${userData.email} already exists in KYKY system`);
        }
        
        /*
         * יצירת משתמש חדש עם מזהה KYKY
         * Create new user with KYKY identifier
         */
        const newUser = new User({
            id: this.userIdCounter++,
            firstName: userData.firstName,
            lastName: userData.lastName,
            email: userData.email,
            department: userData.department || 'KYKY General',
            createdAt: new Date(),
            isActive: true
        });
        
        // הוספה למאגר משתמשי KYKY - Add to KYKY users storage
        this.users.push(newUser);
        
        // רישום יצירת משתמש - Log user creation
        console.log(`New KYKY user created: ${newUser.email}`);
        
        return newUser;
    }
    
    /**
     * Get all users from KYKY system
     * קבלת כל המשתמשים ממערכת KYKY
     * 
     * @returns {Array} רשימת משתמשי KYKY - KYKY users list
     */
    async getAllUsers() {
        /*
         * החזרת עותק של רשימת משתמשי KYKY
         * Return copy of KYKY users list
         */
        return [...this.users];
    }
    
    /**
     * Find user by ID in KYKY system
     * חיפוש משתמש לפי מזהה במערכת KYKY
     * 
     * @param {number} id מזהה המשתמש - User ID
     * @returns {User|null} המשתמש או null - User or null
     */
    async findUserById(id) {
        // חיפוש במאגר משתמשי KYKY - Search in KYKY users storage
        return this.users.find(user => user.id === id) || null;
    }
    
    /**
     * Update user in KYKY system
     * עדכון משתמש במערכת KYKY
     */
    async updateUser(id, updateData) {
        const userIndex = this.users.findIndex(user => user.id === id);
        if (userIndex === -1) {
            throw new Error(`User with ID ${id} not found in KYKY system`);
        }
        
        /*
         * עדכון נתוני המשתמש במערכת KYKY
         * Update user data in KYKY system
         */
        this.users[userIndex] = { ...this.users[userIndex], ...updateData };
        
        // רישום עדכון - Log update
        console.log(`KYKY user updated: ${this.users[userIndex].email}`);
        
        return this.users[userIndex];
    }
    
    /*
     * Delete user from KYKY system
     * מחיקת משתמש ממערכת KYKY
     */
    async deleteUser(id) {
        const userIndex = this.users.findIndex(user => user.id === id);
        if (userIndex === -1) {
            return false; // משתמש לא נמצא - User not found
        }
        
        // הסרת המשתמש ממערכת KYKY - Remove user from KYKY system
        const deletedUser = this.users.splice(userIndex, 1)[0];
        
        /*
         * רישום מחיקת משתמש
         * Log user deletion
         */
        console.log(`KYKY user deleted: ${deletedUser.email}`);
        
        return true;
    }
}

// ייצוא שירות משתמשי KYKY - Export KYKY user service
module.exports = UserService;
