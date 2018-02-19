/*
Navicat MySQL Data Transfer

Source Server         : IData
Source Server Version : 50720
Source Host           : localhost:3306
Source Database       : idata

Target Server Type    : MYSQL
Target Server Version : 50720
File Encoding         : 65001

Date: 2018-02-19 15:40:09
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `files`
-- ----------------------------
DROP TABLE IF EXISTS `files`;
CREATE TABLE `files` (
  `FileID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `ParentPath` varchar(255) NOT NULL,
  `Type` int(11) NOT NULL,
  `HashID` int(11) DEFAULT NULL,
  `Size` int(11) NOT NULL DEFAULT '0',
  `Users` int(11) NOT NULL,
  PRIMARY KEY (`FileID`),
  KEY `HashID` (`HashID`) USING BTREE,
  KEY `UsersFQ` (`Users`),
  CONSTRAINT `UsersFQ` FOREIGN KEY (`Users`) REFERENCES `users` (`UserID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `files_ibfk_1` FOREIGN KEY (`HashID`) REFERENCES `hashes` (`HashID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of files
-- ----------------------------

-- ----------------------------
-- Table structure for `hashes`
-- ----------------------------
DROP TABLE IF EXISTS `hashes`;
CREATE TABLE `hashes` (
  `HashID` int(255) NOT NULL AUTO_INCREMENT,
  `MD5` varchar(255) NOT NULL,
  `Counter` int(11) NOT NULL DEFAULT '1',
  `Extension` char(10) NOT NULL,
  PRIMARY KEY (`HashID`),
  UNIQUE KEY `MD5` (`MD5`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of hashes
-- ----------------------------

-- ----------------------------
-- Table structure for `users`
-- ----------------------------
DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
  `Email` varchar(12) NOT NULL,
  `UserPassword` varchar(24) NOT NULL,
  `UserID` int(11) NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(12) NOT NULL,
  `LastName` varchar(12) DEFAULT NULL,
  PRIMARY KEY (`UserID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of users
-- ----------------------------
INSERT INTO `users` VALUES ('admin', 'admin', '1', 'tobi', 'jon');
INSERT INTO `users` VALUES ('test', 'test', '5', 'test', 'test');

-- ----------------------------
-- Procedure structure for `SP_Files_AddFolder`
-- ----------------------------
DROP PROCEDURE IF EXISTS `SP_Files_AddFolder`;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_Files_AddFolder`(paramFolderName VarChar(50), paramParentPath VarChar(21) ,paramUserID int(20))
BEGIN
INSERT INTO Files ( Name, ParentPath, Type , users)
VALUES (paramFolderName , paramParentPath, 1, paramUserID);
END
;;
DELIMITER ;

-- ----------------------------
-- Procedure structure for `SP_Files_DeleteFile`
-- ----------------------------
DROP PROCEDURE IF EXISTS `SP_Files_DeleteFile`;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_Files_DeleteFile`(in paramFileID int ,  in paramMD5  VarChar(255) , paramUserID int(20))
BEGIN
    DECLARE tempCounter INTEGER ; 
    DELETE FROM Files WHERE FileID IN (paramFileID)&& files.Users = paramUserID;
     
    SELECT @tempCounter := Counter 
    FROM Hashes WHERE MD5  = paramMD5;
		
    IF (@tempCounter = 0)
		THEN
         DELETE FROM Hashes WHERE MD5 IN (ParamMD5) ; 
    END IF ;
END
;;
DELIMITER ;

-- ----------------------------
-- Procedure structure for `SP_Files_DeleteFolder`
-- ----------------------------
DROP PROCEDURE IF EXISTS `SP_Files_DeleteFolder`;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_Files_DeleteFolder`(paramParentPath VarChar(60) , paramID VarChar(40) ,paramFolderName VarChar(40),paramUserID int(20))
BEGIN
 
  DELETE FROM files WHERE fileID = paramID;
    IF (paramParentPath ="") THEN 
        DELETE FROM files 
        WHERE ParentPath 
        LIKE CONCAT(paramFolderName,'%')&& files.Users = paramUserID;
    ELSE 
        DELETE FROM files 
        WHERE ParentPath 
        LIKE CONCAT(paramParentPath,"/",paramFolderName,'%') && files.Users = paramUserID;
    END IF ; 
  
  SELECT *
  FROM   hashes
  WHERE  Counter = 0 ; 
  
     DELETE FROM Hashes WHERE Counter=0 ; 
END
;;
DELIMITER ;

-- ----------------------------
-- Procedure structure for `SP_Files_GetDirctoryInfo`
-- ----------------------------
DROP PROCEDURE IF EXISTS `SP_Files_GetDirctoryInfo`;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_Files_GetDirctoryInfo`(paramParentPath VarChar(255), paramUserID VarChar(12))
BEGIN
 SELECT FileID, Name, ParentPath, Type, Files.HashID, Size, Extension

  FROM   Files

  LEFT JOIN Hashes ON Hashes.HashID = Files.HashID 

  WHERE ParentPath = paramParentPath && files.Users = paramUserID ;
END
;;
DELIMITER ;

-- ----------------------------
-- Procedure structure for `SP_Files_GetDirctoryList`
-- ----------------------------
DROP PROCEDURE IF EXISTS `SP_Files_GetDirctoryList`;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_Files_GetDirctoryList`(paramParentPath VarChar(255) ,paramUserID int(25))
BEGIN
  SELECT FileID, Name, ParentPath, Type, Files.HashID, Size, MD5, Extension
  FROM   Files
  LEFT JOIN Hashes ON Hashes.HashID = Files.HashID 
  WHERE ParentPath LIKE CONCAT(paramParentPath,"%")&& files.Users = paramUserID;
END
;;
DELIMITER ;

-- ----------------------------
-- Procedure structure for `SP_Files_InsertFile`
-- ----------------------------
DROP PROCEDURE IF EXISTS `SP_Files_InsertFile`;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_Files_InsertFile`(paramMD5 Varchar (255) , paramName VarChar(50), paramParentPath VarChar(105) , paramType int(1),paramSize int(100) ,paramExtension VarChar(10) , paramUserID int(20))
BEGIN

	DECLARE temHashID INTEGER ;

	SELECT HashID INTO temHashID FROM Hashes WHERE MD5 = paramMD5;

	IF (temHashID IS NULL)
	THEN
			INSERT INTO Hashes (MD5 , Extension) 
			VALUES ( paramMD5 , paramExtension);
     
			 SET temHashID  = LAST_INSERT_ID();

	ELSE
			UPDATE Hashes SET Counter = Counter + 1 WHERE HashID = temHashID;	
	End IF ;

	INSERT INTO Files (HashID, Name, ParentPath, Type ,Size , users)
	VALUES (temHashID, paramName, paramParentPath, paramType, paramSize, paramUserID);

END
;;
DELIMITER ;

-- ----------------------------
-- Procedure structure for `SP_Files_RenameFolder`
-- ----------------------------
DROP PROCEDURE IF EXISTS `SP_Files_RenameFolder`;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_Files_RenameFolder`(paramNewName varchar(20), paramFolderName varchar(20), paramFolderID int(10) , paramUserID int(20))
BEGIN

   UPDATE files
   SET ParentPath = CONCAT
   (REPLACE(LEFT(ParentPath, INSTR(ParentPath, paramFolderName)+LENGTH(paramFolderName)), ParamFolderName, paramNewName),
   SUBSTRING(ParentPath, INSTR(ParentPath, paramFolderName)+LENGTH(paramFolderName)+1))
   WHERE  parentPath LIKE CONCAT('%/',ParamFolderName,'%') && files.Users = paramUserID; 



   UPDATE files 
   SET Name = paramNewName
   WHERE FileID = paramFolderID ; 

END
;;
DELIMITER ;

-- ----------------------------
-- Procedure structure for `SP_Files_UpdateFile`
-- ----------------------------
DROP PROCEDURE IF EXISTS `SP_Files_UpdateFile`;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_Files_UpdateFile`(paramName VARCHAR(50) , paramParentPath VARCHAR(50) , paramFileID INT , paramUserID int (20))
BEGIN
	UPDATE files
	SET Name = paramName  , ParentPath = paramParentPath  
	WHERE FileID = paramFileID && files.Users = paramUserID;
END
;;
DELIMITER ;

-- ----------------------------
-- Procedure structure for `SP_Users_AuthenticateUser`
-- ----------------------------
DROP PROCEDURE IF EXISTS `SP_Users_AuthenticateUser`;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_Users_AuthenticateUser`(paramEmail varchar(20) , paramPassword varchar (20))
BEGIN

SELECT users.Email, users.UserPassword , users.UserID , users.FirstName , users.LastName
FROM users

WHERE users.Email = paramEmail  && users.UserPassword = paramPassword  ; 
END
;;
DELIMITER ;

-- ----------------------------
-- Procedure structure for `SP_Users_DeleteUser`
-- ----------------------------
DROP PROCEDURE IF EXISTS `SP_Users_DeleteUser`;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_Users_DeleteUser`(paramID  Int(20) , paramMD5 varchar(20))
BEGIN

    DECLARE tempCounter INTEGER ; 
    DECLARE tempUserFilesID INTEGER; 
 
    SELECT @tempUserFilesID := files.HashID 
    FROM files WHERE files.Users = paramID;
    
    SELECT @tempCounter := Counter 
    FROM Hashes WHERE hashes.HashID  = @tempUserFilesID;
    
    IF (@tempCounter = 0)
		THEN
         DELETE FROM Hashes WHERE MD5 IN (ParamMD5) ; 
    END IF ;

    DELETE FROM Users  WHERE users.id = paramID ;
    DELETE FROM files WHERE files.Users = paramID;       
 
END
;;
DELIMITER ;

-- ----------------------------
-- Procedure structure for `SP_Users_Register`
-- ----------------------------
DROP PROCEDURE IF EXISTS `SP_Users_Register`;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_Users_Register`(paramEmail varchar(20) , paramFirstName varchar(10) , paramLastName varchar(10) ,paramPassword varchar(40) )
BEGIN
INSERT INTO 
users (users.Email , users.FirstName ,users.LastName , users.UserPassword)

values 
(paramEmail , paramFirstName , paramLastName , paramPassword);


END
;;
DELIMITER ;
DROP TRIGGER IF EXISTS `Trg_FolderDelete`;
DELIMITER ;;
CREATE TRIGGER `Trg_FolderDelete` BEFORE DELETE ON `files` FOR EACH ROW BEGIN
IF (OLD.HashID IS NOT NULL ) THEN
UPDATE Hashes SET Counter = Counter -1 WHERE HashID = OLD.HashID;
END IF ;
END
;;
DELIMITER ;
