/**
 * KYKY User Management System
 * מערכת ניהול משתמשים של KYKY
 * 
 * Main application entry point for KYKY user management platform
 * נקודת כניסה ראשית לפלטפורמת ניהול המשתמשים של KYKY
 */

const express = require('express');
const UserService = require('./services/UserService');
const AuthService = require('./services/AuthService');

// יצירת אפליקציית Express לחברת KYKY - Create Express app for KYKY
const app = express();

// הגדרת פורט לשרת KYKY - Set port for KYKY server
const PORT = process.env.KYKY_PORT || 3000;

/**
 * Initialize KYKY application middleware
 * אתחול middleware של יישום KYKY
 */
app.use(express.json()); // פענוח JSON - JSON parsing
app.use(express.urlencoded({ extended: true })); // פענוח URL - URL parsing

// יצירת מופעי שירותים של KYKY - Create KYKY service instances
const userService = new UserService();
const authService = new AuthService();

/**
 * KYKY welcome endpoint
 * נקודת קצה לברכה של KYKY
 */
app.get('/', (req, res) => {
    // הודעת ברכה למערכת KYKY - Welcome message for KYKY system
    res.json({
        message: 'Welcome to KYKY User Management System!',
        messageHebrew: 'ברוכים הבאים למערכת ניהול המשתמשים של KYKY!',
        company: 'KYKY Corporation',
        version: '1.0.0'
    });
});

/**
 * Get all users from KYKY system
 * קבלת כל המשתמשים ממערכת KYKY
 */
app.get('/api/kyky/users', async (req, res) => {
    try {
        // קבלת רשימת משתמשי KYKY - Get KYKY users list
        const users = await userService.getAllUsers();
        
        /*
         * החזרת תגובה עם משתמשי KYKY
         * Return response with KYKY users
         */
        res.json({
            success: true,
            data: users,
            message: 'KYKY users retrieved successfully',
            messageHebrew: 'משתמשי KYKY נטענו בהצלחה'
        });
    } catch (error) {
        // טיפול בשגיאות KYKY - Handle KYKY errors
        res.status(500).json({
            success: false,
            error: 'Failed to retrieve KYKY users',
            errorHebrew: 'נכשל בטעינת משתמשי KYKY'
        });
    }
});

/**
 * Create new user in KYKY system
 * יצירת משתמש חדש במערכת KYKY
 */
app.post('/api/kyky/users', async (req, res) => {
    try {
        // יצירת משתמש חדש בחברת KYKY - Create new user in KYKY
        const newUser = await userService.createUser(req.body);
        
        /*
         * רישום יצירת משתמש במערכת KYKY
         * Log user creation in KYKY system
         */
        console.log(`New user created in KYKY system: ${newUser.email}`);
        console.log(`משתמש חדש נוצר במערכת KYKY: ${newUser.email}`);
        
        res.status(201).json({
            success: true,
            data: newUser,
            message: 'User created successfully in KYKY system',
            messageHebrew: 'משתמש נוצר בהצלחה במערכת KYKY'
        });
    } catch (error) {
        // שגיאה ביצירת משתמש KYKY - Error creating KYKY user
        res.status(400).json({
            success: false,
            error: error.message
        });
    }
});

/**
 * Start KYKY server
 * הפעלת שרת KYKY
 */
app.listen(PORT, () => {
    console.log(`KYKY User Management System running on port ${PORT}`);
    console.log(`מערכת ניהול המשתמשים של KYKY פועלת על פורט ${PORT}`);
    
    /*
     * אתחול שירותי KYKY
     * Initialize KYKY services
     */
    userService.initialize();
    authService.initialize();
});

// ייצוא האפליקציה של KYKY - Export KYKY application
module.exports = app;
