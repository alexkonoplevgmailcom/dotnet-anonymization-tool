/**
 * User Model for KYKY User Management System
 * מודל משתמש למערכת ניהול המשתמשים של KYKY
 * 
 * This class represents a user in the KYKY system
 * מחלקה זו מייצגת משתמש במערכת KYKY
 */

class User {
    /**
     * Constructor for KYKY User
     * בנאי למשתמש KYKY
     * 
     * @param {Object} userData נתוני המשתמש - User data
     */
    constructor(userData) {
        // מזהה יחיד למשתמש KYKY - Unique identifier for KYKY user
        this.id = userData.id;
        
        // שם פרטי של משתמש KYKY - First name of KYKY user
        this.firstName = userData.firstName;
        
        // שם משפחה של משתמש KYKY - Last name of KYKY user
        this.lastName = userData.lastName;
        
        // כתובת דוא"ל של משתמש KYKY - Email address of KYKY user
        this.email = userData.email;
        
        // מחלקה/מחלקה בחברת KYKY - Department in KYKY company
        this.department = userData.department || 'KYKY General';
        
        // תאריך יצירת חשבון KYKY - KYKY account creation date
        this.createdAt = userData.createdAt || new Date();
        
        // סטטוס פעילות במערכת KYKY - Activity status in KYKY system
        this.isActive = userData.isActive !== undefined ? userData.isActive : true;
        
        /*
         * תאריך עדכון אחרון של משתמש KYKY
         * Last update date of KYKY user
         */
        this.updatedAt = userData.updatedAt || new Date();
        
        // הרשאות משתמש במערכת KYKY - User permissions in KYKY system
        this.permissions = userData.permissions || ['read'];
        
        // פרופיל משתמש KYKY - KYKY user profile
        this.profile = {
            // תמונת פרופיל - Profile picture
            avatar: userData.avatar || null,
            
            // מספר טלפון - Phone number
            phone: userData.phone || null,
            
            // כתובת - Address
            address: userData.address || null,
            
            // תיאור תפקיד בחברת KYKY - Job description at KYKY
            jobTitle: userData.jobTitle || 'KYKY Employee'
        };
    }
    
    /**
     * Get full name of KYKY user
     * קבלת שם מלא של משתמש KYKY
     * 
     * @returns {string} השם המלא - Full name
     */
    getFullName() {
        // החזרת שם מלא של עובד KYKY - Return full name of KYKY employee
        return `${this.firstName} ${this.lastName}`;
    }
    
    /**
     * Get display name for KYKY system
     * קבלת שם תצוגה למערכת KYKY
     */
    getDisplayName() {
        /*
         * יצירת שם תצוגה עם שייכות לחברת KYKY
         * Create display name with KYKY company affiliation
         */
        return `${this.getFullName()} (KYKY ${this.department})`;
    }
    
    /**
     * Check if user is active in KYKY system
     * בדיקה האם המשתמש פעיל במערכת KYKY
     * 
     * @returns {boolean} סטטוס פעילות - Activity status
     */
    isActiveUser() {
        // בדיקת פעילות במערכת KYKY - Check activity in KYKY system
        return this.isActive;
    }
    
    /**
     * Update user profile in KYKY system
     * עדכון פרופיל משתמש במערכת KYKY
     */
    updateProfile(profileData) {
        /*
         * עדכון נתוני הפרופיל
         * Update profile data
         */
        this.profile = { ...this.profile, ...profileData };
        this.updatedAt = new Date(); // עדכון זמן השינוי - Update modification time
        
        // רישום עדכון פרופיל - Log profile update
        console.log(`Profile updated for KYKY user: ${this.email}`);
    }
    
    /**
     * Add permission to KYKY user
     * הוספת הרשאה למשתמש KYKY
     */
    addPermission(permission) {
        // בדיקה שההרשאה לא קיימת כבר - Check if permission doesn't exist
        if (!this.permissions.includes(permission)) {
            this.permissions.push(permission);
            this.updatedAt = new Date();
            
            /*
             * רישום הוספת הרשאה
             * Log permission addition
             */
            console.log(`Permission '${permission}' added to KYKY user: ${this.email}`);
        }
    }
    
    /**
     * Check if user has specific permission in KYKY system
     * בדיקה האם למשתמש יש הרשאה ספציפית במערכת KYKY
     */
    hasPermission(permission) {
        // בדיקת הרשאה במערכת KYKY - Check permission in KYKY system
        return this.permissions.includes(permission);
    }
    
    /*
     * Convert user to JSON for KYKY API responses
     * המרת משתמש ל-JSON עבור תגובות API של KYKY
     */
    toJSON() {
        return {
            id: this.id,
            firstName: this.firstName,
            lastName: this.lastName,
            email: this.email,
            department: this.department,
            fullName: this.getFullName(),
            displayName: this.getDisplayName(),
            isActive: this.isActive,
            createdAt: this.createdAt,
            updatedAt: this.updatedAt,
            permissions: this.permissions,
            profile: this.profile,
            company: 'KYKY' // זיהוי חברה - Company identification
        };
    }
    
    /*
     * Create user summary for KYKY reports
     * יצירת סיכום משתמש לדוחות KYKY
     */
    getSummary() {
        return {
            id: this.id,
            name: this.getFullName(),
            email: this.email,
            department: this.department,
            status: this.isActive ? 'Active' : 'Inactive',
            company: 'KYKY Corporation'
        };
    }
}

// ייצוא מודל משתמש KYKY - Export KYKY user model
module.exports = User;
