-- ========================================================
-- CoreConcepts (CC) Complete Database Schema & Seed Script
-- Target: MySQL 8.0+ / MariaDB (XAMPP phpMyAdmin)
-- ========================================================

CREATE DATABASE IF NOT EXISTS `CoreConceptsDb` 
CHARACTER SET utf8mb4 
COLLATE utf8mb4_unicode_ci;

USE `CoreConceptsDb`;

-- 1. Identity Roles Table
CREATE TABLE IF NOT EXISTS `AspNetRoles` (
    `Id` varchar(255) NOT NULL,
    `Name` varchar(256) DEFAULT NULL,
    `NormalizedName` varchar(256) DEFAULT NULL,
    `ConcurrencyStamp` longtext DEFAULT NULL,
    PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 2. Identity Users Table (Extended with FullName & ContactInfo)
CREATE TABLE IF NOT EXISTS `AspNetUsers` (
    `Id` varchar(255) NOT NULL,
    `FullName` varchar(100) NOT NULL DEFAULT '',
    `ContactInfo` varchar(200) DEFAULT NULL,
    `UserName` varchar(256) DEFAULT NULL,
    `NormalizedUserName` varchar(256) DEFAULT NULL,
    `Email` varchar(256) DEFAULT NULL,
    `NormalizedEmail` varchar(256) DEFAULT NULL,
    `EmailConfirmed` tinyint(1) NOT NULL,
    `PasswordHash` longtext DEFAULT NULL,
    `SecurityStamp` longtext DEFAULT NULL,
    `ConcurrencyStamp` longtext DEFAULT NULL,
    `PhoneNumber` longtext DEFAULT NULL,
    `PhoneNumberConfirmed` tinyint(1) NOT NULL,
    `TwoFactorEnabled` tinyint(1) NOT NULL,
    `LockoutEnd` datetime(6) DEFAULT NULL,
    `LockoutEnabled` tinyint(1) NOT NULL,
    `AccessFailedCount` int NOT NULL,
    PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 3. Identity User Roles Mapping
CREATE TABLE IF NOT EXISTS `AspNetUserRoles` (
    `UserId` varchar(255) NOT NULL,
    `RoleId` varchar(255) NOT NULL,
    PRIMARY KEY (`UserId`,`RoleId`),
    CONSTRAINT `FK_AspNetUserRoles_AspNetRoles` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AspNetUserRoles_AspNetUsers` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 4. Identity User Claims
CREATE TABLE IF NOT EXISTS `AspNetUserClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` varchar(255) NOT NULL,
    `ClaimType` longtext DEFAULT NULL,
    `ClaimValue` longtext DEFAULT NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetUserClaims_AspNetUsers` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 5. Identity User Logins
CREATE TABLE IF NOT EXISTS `AspNetUserLogins` (
    `LoginProvider` varchar(255) NOT NULL,
    `ProviderKey` varchar(255) NOT NULL,
    `ProviderDisplayName` longtext DEFAULT NULL,
    `UserId` varchar(255) NOT NULL,
    PRIMARY KEY (`LoginProvider`,`ProviderKey`),
    CONSTRAINT `FK_AspNetUserLogins_AspNetUsers` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 6. Identity Role Claims
CREATE TABLE IF NOT EXISTS `AspNetRoleClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `RoleId` varchar(255) NOT NULL,
    `ClaimType` longtext DEFAULT NULL,
    `ClaimValue` longtext DEFAULT NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 7. Identity User Tokens
CREATE TABLE IF NOT EXISTS `AspNetUserTokens` (
    `UserId` varchar(255) NOT NULL,
    `LoginProvider` varchar(255) NOT NULL,
    `Name` varchar(255) NOT NULL,
    `Value` longtext DEFAULT NULL,
    PRIMARY KEY (`UserId`,`LoginProvider`,`Name`),
    CONSTRAINT `FK_AspNetUserTokens_AspNetUsers` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ========================================================
-- CORECONCEPTS APPLICATION TABLES
-- ========================================================

-- 8. Hero Banners Table (Landing page slider CMS)
CREATE TABLE IF NOT EXISTS `HeroBanners` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Title` varchar(150) NOT NULL,
    `Subtitle` varchar(300) DEFAULT NULL,
    `ImagePath` varchar(300) DEFAULT NULL,
    `ButtonText` varchar(100) DEFAULT NULL,
    `ButtonUrl` varchar(200) DEFAULT NULL,
    `DisplayOrder` int NOT NULL DEFAULT 0,
    `IsActive` tinyint(1) NOT NULL DEFAULT 1,
    `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 9. Home Feature Cards Table (What We Do / Service cards)
CREATE TABLE IF NOT EXISTS `HomeFeatureCards` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Title` varchar(100) NOT NULL,
    `Description` varchar(300) NOT NULL,
    `IconClass` varchar(50) DEFAULT NULL,
    `ImagePath` varchar(300) DEFAULT NULL,
    `LinkUrl` varchar(200) DEFAULT NULL,
    `DisplayOrder` int NOT NULL DEFAULT 0,
    `IsActive` tinyint(1) NOT NULL DEFAULT 1,
    PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 10. Company Projects Table (Running & Completed Showcase)
CREATE TABLE IF NOT EXISTS `CompanyProjects` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Title` varchar(150) NOT NULL,
    `Description` varchar(1000) NOT NULL,
    `Category` varchar(100) NOT NULL DEFAULT 'Software',
    `ImagePath` varchar(300) DEFAULT NULL,
    `ProjectUrl` varchar(200) DEFAULT NULL,
    `Status` varchar(100) NOT NULL DEFAULT 'Running',
    `Technologies` varchar(300) DEFAULT NULL,
    `IsFeatured` tinyint(1) NOT NULL DEFAULT 1,
    `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 11. Job Categories Table (Software, Graphics, UI/UX, QA, etc.)
CREATE TABLE IF NOT EXISTS `JobCategories` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(100) NOT NULL,
    `Description` varchar(300) DEFAULT NULL,
    PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 12. Job Openings & Internships Table
CREATE TABLE IF NOT EXISTS `JobOpenings` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Title` varchar(150) NOT NULL,
    `JobCategoryId` int NOT NULL,
    `Type` varchar(50) NOT NULL DEFAULT 'Internship',
    `Location` varchar(100) NOT NULL DEFAULT 'Dhaka, Bangladesh (Hybrid)',
    `Description` longtext NOT NULL,
    `Requirements` longtext DEFAULT NULL,
    `SalaryOrStipend` varchar(100) DEFAULT NULL,
    `Deadline` datetime(6) DEFAULT NULL,
    `IsActive` tinyint(1) NOT NULL DEFAULT 1,
    `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_JobOpenings_JobCategories` FOREIGN KEY (`JobCategoryId`) REFERENCES `JobCategories` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 13. Job Applications & Resume Table (Candidate Submissions)
CREATE TABLE IF NOT EXISTS `JobApplications` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `JobOpeningId` int NOT NULL,
    `ApplicantUserId` varchar(255) NOT NULL,
    `FullName` varchar(100) NOT NULL,
    `Email` varchar(150) NOT NULL,
    `Phone` varchar(20) NOT NULL,
    `CoverNote` varchar(500) DEFAULT NULL,
    `CvFilePath` varchar(300) NOT NULL,
    `Status` varchar(50) NOT NULL DEFAULT 'Pending',
    `AppliedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_JobApplications_JobOpenings` FOREIGN KEY (`JobOpeningId`) REFERENCES `JobOpenings` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_JobApplications_AspNetUsers` FOREIGN KEY (`ApplicantUserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ========================================================
-- INITIAL SEED DATA (Default Banners, Categories, Jobs)
-- ========================================================

-- Insert Initial Categories
INSERT INTO `JobCategories` (`Id`, `Name`, `Description`) VALUES
(1, 'Software Engineering', 'Web, Backend, API & Full-Stack Development roles'),
(2, 'Graphics & Creative Design', 'UI/UX, Visual Branding, Motion & Creative Graphics'),
(3, 'Quality Assurance & Testing', 'Manual & Automated Software Quality Assurance'),
(4, 'Digital Strategy & IT Support', 'Growth, Marketing & Operations')
ON DUPLICATE KEY UPDATE `Name`=`Name`;

-- Insert Initial Openings
INSERT INTO `JobOpenings` (`Id`, `Title`, `JobCategoryId`, `Type`, `Location`, `Description`, `Requirements`, `SalaryOrStipend`, `Deadline`, `IsActive`, `CreatedAt`) VALUES
(1, 'Software Developer Intern (.NET / React)', 1, 'Internship', 'Dhaka (Hybrid)', 'Opportunity for passionate tech enthusiasts to work on real-world enterprise applications with .NET 10, MySQL, and modern front-end frameworks.', 'Basic understanding of C#, OOP, SQL, and Git. Strong willingness to learn.', '12,000 - 15,000 BDT / month', DATE_ADD(NOW(), INTERVAL 30 DAY), 1, NOW()),
(2, 'Junior Graphics & UI/UX Designer', 2, 'Full-Time', 'Dhaka (On-site)', 'Create modern web/app UI prototypes in Figma, and design digital brand assets, marketing visuals, and presentations.', 'Proficiency in Figma, Adobe Illustrator, and Photoshop. A portfolio link is required.', '20,000 - 30,000 BDT / month', DATE_ADD(NOW(), INTERVAL 25 DAY), 1, NOW()),
(3, 'Graphics Design Intern', 2, 'Internship', 'Remote', 'Hands-on internship designing creative banners, social assets, and iconography under senior designer guidance.', 'Familiarity with Figma/Photoshop/Illustrator. Creativity and keen eye for aesthetics.', '8,000 - 10,000 BDT / month', DATE_ADD(NOW(), INTERVAL 40 DAY), 1, NOW())
ON DUPLICATE KEY UPDATE `Title`=`Title`;

-- Insert Initial Hero Banners
INSERT INTO `HeroBanners` (`Id`, `Title`, `Subtitle`, `ImagePath`, `ButtonText`, `ButtonUrl`, `DisplayOrder`, `IsActive`, `CreatedAt`) VALUES
(1, 'Empowering Digital Evolution with CoreConcepts (CC)', 'We build cutting-edge software solutions, nurture emerging tech talent, and power high-impact digital experiences.', NULL, 'Explore Career & Internships', '/Career', 1, 1, NOW()),
(2, 'Custom Software & Enterprise Engineering', 'From cloud architectures to scalable web platforms, explore how CoreConcepts delivers next-gen software.', NULL, 'View Our Projects', '/Projects', 2, 1, NOW())
ON DUPLICATE KEY UPDATE `Title`=`Title`;

-- Insert Initial Feature Cards
INSERT INTO `HomeFeatureCards` (`Id`, `Title`, `Description`, `IconClass`, `ImagePath`, `LinkUrl`, `DisplayOrder`, `IsActive`) VALUES
(1, 'Software Development', 'Scalable enterprise web applications, high-performance APIs, and robust database architectures.', 'bi-code-slash', NULL, '/Projects', 1, 1),
(2, 'Graphics & UI/UX Design', 'User-centric interface designs, engaging brand identities, and visually stunning digital assets.', 'bi-palette', NULL, '/Projects', 2, 1),
(3, 'Internship Programs', 'Structured industrial training and real-client mentorship for ambitious students and fresh grads.', 'bi-mortarboard', NULL, '/Career', 3, 1),
(4, 'Quality Assurance & Cloud', 'Ensuring high stability, continuous deployment, and security for mission-critical software systems.', 'bi-shield-check', NULL, '/Projects', 4, 1)
ON DUPLICATE KEY UPDATE `Title`=`Title`;

-- Insert Initial Projects
INSERT INTO `CompanyProjects` (`Id`, `Title`, `Description`, `Category`, `ImagePath`, `ProjectUrl`, `Status`, `Technologies`, `IsFeatured`, `CreatedAt`) VALUES
(1, 'Auction Management System', 'A complete multi-vendor online bidding and live auction platform with buyer dashboards, escrow payment tracking, and automated bid syncing.', 'Software', NULL, NULL, 'Completed', 'ASP.NET Core, MySQL, Bootstrap 5, SignalR', 1, NOW()),
(2, 'Tradelink CRM & ERP Suite', 'Customer relationship management and enterprise order tracking system designed for commercial distribution networks.', 'Software', NULL, NULL, 'Running', 'C#, Entity Framework Core, RESTful APIs', 1, NOW()),
(3, 'Modern Brand Identity & UI Kit', 'Comprehensive design system, UI components, typography, and illustration assets for modern SaaS products.', 'Graphics Design', NULL, NULL, 'Completed', 'Figma, Adobe Illustrator, Design System', 1, NOW())
ON DUPLICATE KEY UPDATE `Title`=`Title`;
