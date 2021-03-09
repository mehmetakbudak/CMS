/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

CREATE DATABASE IF NOT EXISTS `cms` /*!40100 DEFAULT CHARACTER SET utf8mb4 */;
USE `cms`;

CREATE TABLE IF NOT EXISTS `access_right` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AccessRightCategoryId` int(11) NOT NULL,
  `ParentId` int(11) DEFAULT NULL,
  `LinkName` varchar(200) NOT NULL,
  `LinkUrl` varchar(200) DEFAULT NULL,
  `Order` int(11) NOT NULL,
  `IsShowMenu` bit(1) NOT NULL,
  `HttpStatusType` int(11) DEFAULT NULL,
  `IsActive` bit(1) NOT NULL,
  `Deleted` bit(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_access_right_access_right_category` (`AccessRightCategoryId`),
  CONSTRAINT `FK_access_right_access_right_category` FOREIGN KEY (`AccessRightCategoryId`) REFERENCES `access_right_category` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8mb4;

/*!40000 ALTER TABLE `access_right` DISABLE KEYS */;
INSERT INTO `access_right` (`Id`, `AccessRightCategoryId`, `ParentId`, `LinkName`, `LinkUrl`, `Order`, `IsShowMenu`, `HttpStatusType`, `IsActive`, `Deleted`) VALUES
	(1, 1, NULL, 'Anasayfa', '/', 1, b'1', NULL, b'1', b'0'),
	(2, 1, 5, 'Etkinlik', '/haberler/etkinlik', 1, b'1', NULL, b'1', b'0'),
	(3, 1, 5, 'Sergi', '/haberler/sergi', 2, b'1', NULL, b'1', b'0'),
	(4, 1, 5, 'Edebiyat', '/haberler/edebiyat', 3, b'1', NULL, b'1', b'0'),
	(5, 1, NULL, 'Haberler', '/haberler', 2, b'1', NULL, b'1', b'0'),
	(6, 2, NULL, 'Anasayfa', '/Admin/Dashboard', 1, b'1', NULL, b'1', b'0'),
	(7, 2, NULL, 'Kullanıcılar', '/Admin/User', 2, b'1', NULL, b'1', b'0'),
	(8, 2, NULL, 'Haberler', NULL, 2, b'1', NULL, b'1', b'0'),
	(9, 2, NULL, 'Duyurular', '/Admin/Announcement', 3, b'1', NULL, b'1', b'0'),
	(10, 2, 8, 'Haberler', '/Admin/News', 2, b'1', NULL, b'1', b'0'),
	(11, 2, 8, 'Haber Kategorileri', '/Admin/AdminCategory', 1, b'1', NULL, b'1', b'0'),
	(12, 2, 8, 'Anasayfa Haberleri', '/Admin/HomepageNews', 3, b'1', NULL, b'1', b'0'),
	(13, 2, NULL, 'Foto Galeri', '/Admin/PhotoGallery', 4, b'1', NULL, b'1', b'0'),
	(14, 2, NULL, 'Video Galeri', '/Admin/VideoGallery', 5, b'1', NULL, b'1', b'0'),
	(15, 2, NULL, 'Yorumlar', NULL, 6, b'1', NULL, b'1', b'0'),
	(16, 2, 15, 'Onay Bekleyenler', '/Admin/Comment/WaitingApprove', 1, b'1', NULL, b'1', b'0'),
	(17, 2, 15, 'Onaylananlar', '/Admin/Comment/Approved', 2, b'1', NULL, b'1', b'0'),
	(18, 2, 15, 'Reddedilenler', '/Admin/Comment/Rejected', 3, b'1', NULL, b'1', b'0'),
	(19, 1, 2, 'Haftasonu Etkinlikleri', '/haberler/etkinlik/hafta-sonu-etkinlikleri', 1, b'1', NULL, b'1', b'0'),
	(20, 1, NULL, 'İletişim', '/iletisim', 3, b'1', NULL, b'1', b'0'),
	(21, 2, NULL, 'Ayarlar', '', 7, b'1', NULL, b'1', b'0'),
	(22, 2, 21, 'Yetkilendirme', NULL, 1, b'1', NULL, b'1', b'0'),
	(23, 2, 22, 'Kullanıcı Yetkilendirme', '/Admin/UserAuthorization', 1, b'1', NULL, b'1', b'0'),
	(24, 2, 22, 'Kullanıcı Grubu Yetkilendirme', '/Admin/UserGroupAuthorization', 2, b'1', NULL, b'1', b'0'),
	(25, 3, NULL, 'Admin Menü', '/api/menu/backend', 1, b'0', 1, b'1', b'0'),
	(30, 3, NULL, 'Kullanıcı Listesi', '/api/user', 1, b'0', 1, b'1', b'0'),
	(31, 2, NULL, 'Kullanıcı Grupları', '/Admin/UserGroup', 1, b'1', NULL, b'1', b'0');
/*!40000 ALTER TABLE `access_right` ENABLE KEYS */;

CREATE TABLE IF NOT EXISTS `access_right_category` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) NOT NULL,
  `Type` int(11) NOT NULL,
  `NotBeDeleted` int(11) DEFAULT NULL,
  `IsActive` bit(1) NOT NULL,
  `Deleted` bit(1) NOT NULL,
  `IsShowMenu` bit(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4;

/*!40000 ALTER TABLE `access_right_category` DISABLE KEYS */;
INSERT INTO `access_right_category` (`Id`, `Name`, `Type`, `NotBeDeleted`, `IsActive`, `Deleted`, `IsShowMenu`) VALUES
	(1, 'Ön Arayüz Menü', 1, 1, b'1', b'0', b'0'),
	(2, 'Admin Menü', 2, 1, b'1', b'0', b'0'),
	(3, 'Api', 3, 1, b'1', b'0', b'1');
/*!40000 ALTER TABLE `access_right_category` ENABLE KEYS */;

CREATE TABLE IF NOT EXISTS `chat` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ChatGuid` varbinary(50) NOT NULL,
  `NameSurname` varchar(200) NOT NULL,
  `EmailAddress` varchar(200) NOT NULL,
  `PhoneNumber` varchar(200) NOT NULL,
  `ChatStatus` int(11) NOT NULL,
  `InsertDate` datetime NOT NULL,
  `Deleted` bit(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4;

/*!40000 ALTER TABLE `chat` DISABLE KEYS */;
INSERT INTO `chat` (`Id`, `ChatGuid`, `NameSurname`, `EmailAddress`, `PhoneNumber`, `ChatStatus`, `InsertDate`, `Deleted`) VALUES
	(1, _binary 0x39663235643064612D373462372D343064362D613635332D383330306334396461376139, 'Mehmet Akbudak', 'ahmet@mehmetakbudak.site', '(312) 321-3213', 1, '2020-01-26 02:24:51', b'0');
/*!40000 ALTER TABLE `chat` ENABLE KEYS */;

CREATE TABLE IF NOT EXISTS `chat_message` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ChatId` int(11) NOT NULL,
  `UserId` int(11) DEFAULT NULL,
  `Message` text NOT NULL,
  `InsertDate` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_chat_message_chat` (`ChatId`),
  CONSTRAINT `FK_chat_message_chat` FOREIGN KEY (`ChatId`) REFERENCES `chat` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

/*!40000 ALTER TABLE `chat_message` DISABLE KEYS */;
/*!40000 ALTER TABLE `chat_message` ENABLE KEYS */;

CREATE TABLE IF NOT EXISTS `contact` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `NameSurname` varchar(200) NOT NULL,
  `EmailAddress` varchar(200) NOT NULL,
  `ContactCategoryId` int(11) NOT NULL,
  `Message` text NOT NULL,
  `InsertDate` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_contact_contact_category` (`ContactCategoryId`),
  CONSTRAINT `FK_contact_contact_category` FOREIGN KEY (`ContactCategoryId`) REFERENCES `contact_category` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4;

/*!40000 ALTER TABLE `contact` DISABLE KEYS */;
INSERT INTO `contact` (`Id`, `NameSurname`, `EmailAddress`, `ContactCategoryId`, `Message`, `InsertDate`) VALUES
	(1, 'Mehmet Akbudak', 'akbudak.mehmet@gmail.com', 1, 'rewrwerew', '2020-01-19 21:04:48'),
	(2, 'Mehmet Akbudak3', 'ahmet@mehmetakbudak.site', 2, 'rwerew', '2020-01-26 02:25:09'),
	(3, 'dfgfdgf', 'akbudak.mehmet@gmail.com', 2, 'rtyrty', '2020-01-26 02:25:20');
/*!40000 ALTER TABLE `contact` ENABLE KEYS */;

CREATE TABLE IF NOT EXISTS `contact_category` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) NOT NULL,
  `IsActive` bit(1) NOT NULL,
  `Deleted` bit(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4;

/*!40000 ALTER TABLE `contact_category` DISABLE KEYS */;
INSERT INTO `contact_category` (`Id`, `Name`, `IsActive`, `Deleted`) VALUES
	(1, 'Öneri', b'1', b'0'),
	(2, 'Şikayet', b'1', b'0'),
	(3, 'Sistem Hatası', b'1', b'0');
/*!40000 ALTER TABLE `contact_category` ENABLE KEYS */;

CREATE TABLE IF NOT EXISTS `todo` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TodoStatusId` int(11) NOT NULL,
  `TodoCategoryId` int(11) NOT NULL,
  `UserId` int(11) DEFAULT NULL,
  `Title` varchar(200) DEFAULT NULL,
  `Description` text DEFAULT NULL,
  `InsertDate` datetime DEFAULT NULL,
  `UpdateDate` datetime DEFAULT NULL,
  `IsActive` tinyint(1) DEFAULT NULL,
  `Deleted` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_todo_todo` (`TodoCategoryId`),
  KEY `FK_todo_user` (`UserId`),
  KEY `FK_todo_to_do_status` (`TodoStatusId`),
  CONSTRAINT `FK_todo_to_do_status` FOREIGN KEY (`TodoStatusId`) REFERENCES `todo_status` (`Id`),
  CONSTRAINT `FK_todo_todo` FOREIGN KEY (`TodoCategoryId`) REFERENCES `todo` (`Id`),
  CONSTRAINT `FK_todo_user` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

/*!40000 ALTER TABLE `todo` DISABLE KEYS */;
/*!40000 ALTER TABLE `todo` ENABLE KEYS */;

CREATE TABLE IF NOT EXISTS `todo_category` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) DEFAULT NULL,
  `IsActive` tinyint(1) DEFAULT NULL,
  `Deleted` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

/*!40000 ALTER TABLE `todo_category` DISABLE KEYS */;
/*!40000 ALTER TABLE `todo_category` ENABLE KEYS */;

CREATE TABLE IF NOT EXISTS `todo_status` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` int(11) DEFAULT NULL,
  `Active` int(11) DEFAULT NULL,
  `Deleted` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

/*!40000 ALTER TABLE `todo_status` DISABLE KEYS */;
/*!40000 ALTER TABLE `todo_status` ENABLE KEYS */;

CREATE TABLE IF NOT EXISTS `user` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) NOT NULL,
  `Surname` varchar(200) NOT NULL,
  `EmailAddress` varchar(200) NOT NULL,
  `Password` varchar(200) NOT NULL,
  `HashCode` varchar(200) DEFAULT NULL,
  `IsActive` bit(1) NOT NULL,
  `Deleted` bit(1) NOT NULL,
  `IsNewUser` bit(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4;

/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` (`Id`, `Name`, `Surname`, `EmailAddress`, `Password`, `HashCode`, `IsActive`, `Deleted`, `IsNewUser`) VALUES
	(1, 'admin', 'admin', 'admin@admin.com', 'c4ca4238a0b923820dcc509a6f75849b', '(NULL)', b'1', b'0', b'0'),
	(2, 'mehmet', 'akbudak', 'akbudak.mehmet@gmail.com', 'c4ca4238a0b923820dcc509a6f75849b', '(NULL)', b'1', b'0', b'0'),
	(3, 'Ahmet', 'Kara', 'ahmet@mehmetakbudak.site', 'c4ca4238a0b923820dcc509a6f75849b', NULL, b'1', b'0', b'0');
/*!40000 ALTER TABLE `user` ENABLE KEYS */;

CREATE TABLE IF NOT EXISTS `user_access_right` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` int(11) NOT NULL,
  `AccessRightId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_user_access_right_user` (`UserId`),
  KEY `FK_user_access_right_access_right` (`AccessRightId`),
  CONSTRAINT `FK_user_access_right_access_right` FOREIGN KEY (`AccessRightId`) REFERENCES `access_right` (`Id`),
  CONSTRAINT `FK_user_access_right_user` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4;

/*!40000 ALTER TABLE `user_access_right` DISABLE KEYS */;
INSERT INTO `user_access_right` (`Id`, `UserId`, `AccessRightId`) VALUES
	(2, 3, 7);
/*!40000 ALTER TABLE `user_access_right` ENABLE KEYS */;

CREATE TABLE IF NOT EXISTS `user_group` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `Deleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4;

/*!40000 ALTER TABLE `user_group` DISABLE KEYS */;
INSERT INTO `user_group` (`Id`, `Name`, `IsActive`, `Deleted`) VALUES
	(1, 'Super Admin', 1, 0),
	(2, 'Admin', 1, 0),
	(3, 'Basic', 1, 0),
	(4, 'Genel', 1, 0),
	(5, 'test 1', 0, 1),
	(6, 'rwerwer', 1, 1),
	(7, 'sdfsfs sffdsdfdsf', 1, 1),
	(8, 'gsdgdsgd dgdfgdfg', 1, 1),
	(9, 'ggsdds gdsgdsg', 1, 1),
	(10, 'gdsgsdg sdgdsgsd', 1, 1),
	(11, 'gsdgsd', 1, 1),
	(12, 'gdssdsdg', 0, 1);
/*!40000 ALTER TABLE `user_group` ENABLE KEYS */;

CREATE TABLE IF NOT EXISTS `user_group_access_right` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserGroupId` int(11) NOT NULL,
  `AccessRightId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_user_group_access_right_user_group` (`UserGroupId`),
  KEY `FK_user_group_access_right_access_right` (`AccessRightId`),
  CONSTRAINT `FK_user_group_access_right_access_right` FOREIGN KEY (`AccessRightId`) REFERENCES `access_right` (`Id`),
  CONSTRAINT `FK_user_group_access_right_user_group` FOREIGN KEY (`UserGroupId`) REFERENCES `user_group` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4;

/*!40000 ALTER TABLE `user_group_access_right` DISABLE KEYS */;
INSERT INTO `user_group_access_right` (`Id`, `UserGroupId`, `AccessRightId`) VALUES
	(2, 4, 6),
	(4, 3, 8),
	(5, 3, 10),
	(8, 3, 11),
	(11, 3, 12),
	(13, 4, 25),
	(14, 4, 30),
	(16, 1, 21),
	(17, 1, 22),
	(18, 1, 23),
	(19, 1, 24),
	(20, 1, 31);
/*!40000 ALTER TABLE `user_group_access_right` ENABLE KEYS */;

CREATE TABLE IF NOT EXISTS `user_user_group` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` int(11) NOT NULL,
  `UserGroupId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_user_user_group_user` (`UserId`),
  KEY `FK_user_user_group_user_group` (`UserGroupId`),
  CONSTRAINT `FK_user_user_group_user` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`),
  CONSTRAINT `FK_user_user_group_user_group` FOREIGN KEY (`UserGroupId`) REFERENCES `user_group` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4;

/*!40000 ALTER TABLE `user_user_group` DISABLE KEYS */;
INSERT INTO `user_user_group` (`Id`, `UserId`, `UserGroupId`) VALUES
	(1, 3, 3),
	(2, 3, 4),
	(3, 2, 1),
	(4, 2, 4);
/*!40000 ALTER TABLE `user_user_group` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
