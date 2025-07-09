/**
 * Authentication Service for KYKY User Management System
 * שירות אימות למערכת ניהול המשתמשים של KYKY
 */

const jwt = require('jsonwebtoken');
const bcrypt = require('bcrypt');

class AuthService {
    /**
     * Constructor for KYKY Authentication Service
     * בנאי לשירות אימות KYKY
     */
    constructor() {
        // מפתח סודי לחתימת JWT של KYKY - KYKY JWT signing secret
        this.jwtSecret = process.env.KYKY_JWT_SECRET || 'kyky-default-secret-key';
        
        // זמן תפוגה לטוקן KYKY - KYKY token expiration
        this.tokenExpiration = '24h';
        
        // רמת הצפנה לסיסמאות KYKY - KYKY password encryption level
        this.saltRounds = 12;
    }
    
    /**
     * Initialize KYKY authentication service
     * אתחול שירות אימות KYKY
     */
    initialize() {
        /*
         * הודעת אתחול שירות אימות KYKY
         * KYKY authentication service initialization message
         */
        console.log('KYKY Authentication Service initialized successfully');
        console.log('שירות אימות KYKY אותחל בהצלחה');
        
        // בדיקת הגדרת מפתח JWT - Check JWT secret configuration
        if (this.jwtSecret === 'kyky-default-secret-key') {
            console.warn('Warning: Using default JWT secret for KYKY system');
            console.warn('אזהרה: משתמש במפתח JWT ברירת מחדל למערכת KYKY');
        }
    }
    
    /**
     * Hash password for KYKY user
     * הצפנת סיסמה למשתמש KYKY
     * 
     * @param {string} password הסיסמה להצפנה - Password to hash
     * @returns {Promise<string>} הסיסמה המוצפנת - Hashed password
     */
    async hashPassword(password) {
        try {
            // הצפנת הסיסמה עבור מערכת KYKY - Hash password for KYKY system
            const hashedPassword = await bcrypt.hash(password, this.saltRounds);
            
            /*
             * רישום הצפנת סיסמה (ללא חשיפת הסיסמה)
             * Log password hashing (without exposing password)
             */
            console.log('Password hashed successfully for KYKY user');
            
            return hashedPassword;
        } catch (error) {
            // שגיאה בהצפנת סיסמה - Password hashing error
            throw new Error('Failed to hash password for KYKY user');
        }
    }
    
    /**
     * Verify password for KYKY user login
     * אימות סיסמה להתחברות משתמש KYKY
     * 
     * @param {string} plainPassword סיסמה גולמית - Plain password
     * @param {string} hashedPassword סיסמה מוצפנת - Hashed password
     * @returns {Promise<boolean>} תוצאת האימות - Verification result
     */
    async verifyPassword(plainPassword, hashedPassword) {
        try {
            /*
             * השוואת סיסמאות במערכת KYKY
             * Compare passwords in KYKY system
             */
            const isValid = await bcrypt.compare(plainPassword, hashedPassword);
            
            // רישום ניסיון אימות - Log authentication attempt
            console.log(`Password verification for KYKY user: ${isValid ? 'SUCCESS' : 'FAILED'}`);
            
            return isValid;
        } catch (error) {
            // שגיאה באימות סיסמה - Password verification error
            console.error('Error verifying password for KYKY user:', error);
            return false;
        }
    }
    
    /**
     * Generate JWT token for KYKY user
     * יצירת טוקן JWT למשתמש KYKY
     * 
     * @param {Object} user נתוני המשתמש - User data
     * @returns {string} טוקן JWT - JWT token
     */
    generateToken(user) {
        // יצירת payload לטוקן KYKY - Create KYKY token payload
        const payload = {
            id: user.id,
            email: user.email,
            department: user.department,
            company: 'KYKY',
            iat: Math.floor(Date.now() / 1000) // זמן יצירה - Creation time
        };
        
        /*
         * חתימה על הטוקן עם מפתח KYKY
         * Sign token with KYKY secret
         */
        const token = jwt.sign(payload, this.jwtSecret, { 
            expiresIn: this.tokenExpiration,
            issuer: 'KYKY-Auth-Service'
        });
        
        // רישום יצירת טוקן - Log token creation
        console.log(`JWT token generated for KYKY user: ${user.email}`);
        
        return token;
    }
    
    /**
     * Verify JWT token for KYKY system
     * אימות טוקן JWT למערכת KYKY
     */
    verifyToken(token) {
        try {
            /*
             * אימות טוקן עם מפתח KYKY
             * Verify token with KYKY secret
             */
            const decoded = jwt.verify(token, this.jwtSecret);
            
            // בדיקת שייכות לחברת KYKY - Check KYKY company affiliation
            if (decoded.company !== 'KYKY') {
                throw new Error('Token not issued for KYKY system');
            }
            
            return decoded;
        } catch (error) {
            // שגיאה באימות טוקן - Token verification error
            console.error('Invalid token for KYKY system:', error.message);
            throw new Error('Invalid token for KYKY system');
        }
    }
    
    /*
     * Middleware for KYKY token authentication
     * Middleware לאימות טוקן KYKY
     */
    authenticateToken() {
        return (req, res, next) => {
            // קבלת טוקן מהכותרת - Get token from header
            const authHeader = req.headers['authorization'];
            const token = authHeader && authHeader.split(' ')[1];
            
            if (!token) {
                return res.status(401).json({
                    error: 'Access token required for KYKY system',
                    errorHebrew: 'נדרש טוקן גישה למערכת KYKY'
                });
            }
            
            try {
                // אימות הטוקן - Verify token
                const user = this.verifyToken(token);
                req.kykyUser = user; // הוספת משתמש KYKY לבקשה - Add KYKY user to request
                next();
            } catch (error) {
                /*
                 * שגיאה באימות - Authentication error
                 */
                return res.status(403).json({
                    error: 'Invalid token for KYKY system',
                    errorHebrew: 'טוקן לא תקין למערכת KYKY'
                });
            }
        };
    }
}

// ייצוא שירות אימות KYKY - Export KYKY authentication service
module.exports = AuthService;
