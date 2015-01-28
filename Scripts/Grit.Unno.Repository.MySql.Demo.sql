CREATE DATABASE  IF NOT EXISTS `unno` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `unno`;
-- MySQL dump 10.13  Distrib 5.6.13, for Win32 (x86)
--
-- Host: 127.0.0.1    Database: unno
-- ------------------------------------------------------
-- Server version	5.6.19

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `unno_companyinfo`
--

DROP TABLE IF EXISTS `unno_companyinfo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `unno_companyinfo` (
  `NodeId` varchar(36) NOT NULL,
  `ParentIndex` int(11) NOT NULL,
  `NodeIndex` int(11) NOT NULL,
  `CompanyName` varchar(200) DEFAULT NULL,
  `RegOfYear` int(11) DEFAULT NULL,
  PRIMARY KEY (`NodeId`,`ParentIndex`,`NodeIndex`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `unno_companyinfo`
--

LOCK TABLES `unno_companyinfo` WRITE;
/*!40000 ALTER TABLE `unno_companyinfo` DISABLE KEYS */;
/*!40000 ALTER TABLE `unno_companyinfo` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `unno_companyinfo_car`
--

DROP TABLE IF EXISTS `unno_companyinfo_car`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `unno_companyinfo_car` (
  `NodeId` varchar(36) NOT NULL,
  `ParentIndex` int(11) NOT NULL,
  `NodeIndex` int(11) NOT NULL,
  `BuyDate` datetime DEFAULT NULL,
  `Model` varchar(1000) DEFAULT NULL,
  `Price` decimal(20,6) DEFAULT NULL,
  PRIMARY KEY (`NodeId`,`ParentIndex`,`NodeIndex`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `unno_companyinfo_car`
--

LOCK TABLES `unno_companyinfo_car` WRITE;
/*!40000 ALTER TABLE `unno_companyinfo_car` DISABLE KEYS */;
/*!40000 ALTER TABLE `unno_companyinfo_car` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `unno_companyinfo_financialreport`
--

DROP TABLE IF EXISTS `unno_companyinfo_financialreport`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `unno_companyinfo_financialreport` (
  `NodeId` varchar(36) NOT NULL,
  `ParentIndex` int(11) NOT NULL,
  `NodeIndex` int(11) NOT NULL,
  `Year` int(11) DEFAULT NULL,
  `GrossProfit` decimal(20,6) DEFAULT NULL,
  `NetProfit` decimal(20,6) DEFAULT NULL,
  `Full` int(11) DEFAULT NULL,
  `UpdateAt` datetime DEFAULT NULL,
  `Category` varchar(200) DEFAULT NULL,
  `Remark` varchar(1000) DEFAULT NULL,
  PRIMARY KEY (`NodeId`,`ParentIndex`,`NodeIndex`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `unno_companyinfo_financialreport`
--

LOCK TABLES `unno_companyinfo_financialreport` WRITE;
/*!40000 ALTER TABLE `unno_companyinfo_financialreport` DISABLE KEYS */;
/*!40000 ALTER TABLE `unno_companyinfo_financialreport` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `unno_companyinfo_house`
--

DROP TABLE IF EXISTS `unno_companyinfo_house`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `unno_companyinfo_house` (
  `NodeId` varchar(36) NOT NULL,
  `ParentIndex` int(11) NOT NULL,
  `NodeIndex` int(11) NOT NULL,
  `Size` decimal(20,6) DEFAULT NULL,
  `Address` varchar(1000) DEFAULT NULL,
  PRIMARY KEY (`NodeId`,`ParentIndex`,`NodeIndex`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `unno_companyinfo_house`
--

LOCK TABLES `unno_companyinfo_house` WRITE;
/*!40000 ALTER TABLE `unno_companyinfo_house` DISABLE KEYS */;
/*!40000 ALTER TABLE `unno_companyinfo_house` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `unno_companyinfo_house_reference`
--

DROP TABLE IF EXISTS `unno_companyinfo_house_reference`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `unno_companyinfo_house_reference` (
  `NodeId` varchar(36) NOT NULL,
  `ParentIndex` int(11) NOT NULL,
  `NodeIndex` int(11) NOT NULL,
  `Site` varchar(1000) DEFAULT NULL,
  `Price` decimal(20,6) DEFAULT NULL,
  PRIMARY KEY (`NodeId`,`ParentIndex`,`NodeIndex`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `unno_companyinfo_house_reference`
--

LOCK TABLES `unno_companyinfo_house_reference` WRITE;
/*!40000 ALTER TABLE `unno_companyinfo_house_reference` DISABLE KEYS */;
/*!40000 ALTER TABLE `unno_companyinfo_house_reference` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `unno_node_wrapper`
--

DROP TABLE IF EXISTS `unno_node_wrapper`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `unno_node_wrapper` (
  `NodeId` varchar(36) NOT NULL,
  `UnitId` varchar(36) NOT NULL,
  `Version` int(11) DEFAULT NULL,
  `UpdateAt` datetime DEFAULT NULL,
  PRIMARY KEY (`NodeId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `unno_node_wrapper`
--

LOCK TABLES `unno_node_wrapper` WRITE;
/*!40000 ALTER TABLE `unno_node_wrapper` DISABLE KEYS */;
/*!40000 ALTER TABLE `unno_node_wrapper` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `unno_unit`
--

DROP TABLE IF EXISTS `unno_unit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `unno_unit` (
  `UnitId` varchar(36) NOT NULL,
  `Id` int(11) NOT NULL,
  `ParentId` int(11) DEFAULT NULL,
  `Key` varchar(200) NOT NULL,
  `Name` varchar(200) NOT NULL,
  PRIMARY KEY (`UnitId`,`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `unno_unit`
--

LOCK TABLES `unno_unit` WRITE;
/*!40000 ALTER TABLE `unno_unit` DISABLE KEYS */;
INSERT INTO `unno_unit` VALUES ('47d687dd-c642-4e21-ac52-1d7afbec23b8',0,NULL,'CompanyInfo','企业信息'),('47d687dd-c642-4e21-ac52-1d7afbec23b8',1,0,'CompanyName','企业名称'),('47d687dd-c642-4e21-ac52-1d7afbec23b8',2,0,'RegOfYear','注册年限'),('47d687dd-c642-4e21-ac52-1d7afbec23b8',3,0,'FinancialReport','财务状况'),('47d687dd-c642-4e21-ac52-1d7afbec23b8',4,3,'Year','年份'),('47d687dd-c642-4e21-ac52-1d7afbec23b8',5,3,'GrossProfit','毛利润(万)'),('47d687dd-c642-4e21-ac52-1d7afbec23b8',6,3,'NetProfit','净利润(万)'),('47d687dd-c642-4e21-ac52-1d7afbec23b8',7,3,'Full','全年'),('47d687dd-c642-4e21-ac52-1d7afbec23b8',8,3,'UpdateAt','更新时间'),('47d687dd-c642-4e21-ac52-1d7afbec23b8',9,3,'Category','分类'),('47d687dd-c642-4e21-ac52-1d7afbec23b8',10,3,'Remark','备注'),('47d687dd-c642-4e21-ac52-1d7afbec23b8',11,0,'House','房产信息'),('47d687dd-c642-4e21-ac52-1d7afbec23b8',12,11,'Size','建筑面积'),('47d687dd-c642-4e21-ac52-1d7afbec23b8',13,11,'Address','地址'),('47d687dd-c642-4e21-ac52-1d7afbec23b8',14,11,'Reference','参考'),('47d687dd-c642-4e21-ac52-1d7afbec23b8',15,14,'Site','参考网站'),('47d687dd-c642-4e21-ac52-1d7afbec23b8',16,14,'Price','参考价格'),('47d687dd-c642-4e21-ac52-1d7afbec23b8',17,0,'Car','车辆信息'),('47d687dd-c642-4e21-ac52-1d7afbec23b8',18,17,'BuyDate','购买日期'),('47d687dd-c642-4e21-ac52-1d7afbec23b8',19,17,'Model','型号'),('47d687dd-c642-4e21-ac52-1d7afbec23b8',20,17,'Price','价格');
/*!40000 ALTER TABLE `unno_unit` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `unno_unit_specs`
--

DROP TABLE IF EXISTS `unno_unit_specs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `unno_unit_specs` (
  `UnitId` varchar(36) NOT NULL,
  `Id` int(11) NOT NULL,
  `SpecId` int(11) NOT NULL,
  `Type` int(11) NOT NULL,
  `Min` varchar(100) DEFAULT NULL,
  `Max` varchar(100) DEFAULT NULL,
  `Options` varchar(2000) DEFAULT NULL,
  PRIMARY KEY (`UnitId`,`SpecId`,`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `unno_unit_specs`
--

LOCK TABLES `unno_unit_specs` WRITE;
/*!40000 ALTER TABLE `unno_unit_specs` DISABLE KEYS */;
INSERT INTO `unno_unit_specs` VALUES ('47d687dd-c642-4e21-ac52-1d7afbec23b8',0,0,20,'1','1',NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',1,0,6,'0','200',NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',2,0,5,'0','200',NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',3,0,20,NULL,'5',NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',4,0,5,'2000','2100',NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',5,0,4,NULL,NULL,NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',6,0,4,NULL,NULL,NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',7,0,2,NULL,NULL,NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',8,0,3,NULL,NULL,NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',9,0,10,NULL,NULL,'分类1|分类2|分类3|分类4'),('47d687dd-c642-4e21-ac52-1d7afbec23b8',10,0,6,'0','1000',NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',11,0,20,NULL,'5',NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',12,0,4,NULL,NULL,NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',13,0,6,'0','1000',NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',14,0,20,NULL,'5',NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',15,0,6,'0','1000',NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',16,0,4,NULL,NULL,NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',17,0,20,NULL,'5',NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',18,0,3,'1990-01-01 00:00:00 ','2100-01-01 00:00:00 ',NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',19,0,6,'0','1000',NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',20,0,4,NULL,NULL,NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',0,1,1,NULL,NULL,NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',1,1,1,NULL,NULL,NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',2,1,1,NULL,NULL,NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',4,1,1,NULL,NULL,NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',5,1,1,NULL,NULL,NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',6,1,1,NULL,NULL,NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',7,1,1,NULL,NULL,NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',8,1,1,NULL,NULL,NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',9,1,1,NULL,NULL,NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',12,1,1,NULL,NULL,NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',13,1,1,NULL,NULL,NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',15,1,1,NULL,NULL,NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',16,1,1,NULL,NULL,NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',18,1,1,NULL,NULL,NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',19,1,1,NULL,NULL,NULL),('47d687dd-c642-4e21-ac52-1d7afbec23b8',20,1,1,NULL,NULL,NULL);
/*!40000 ALTER TABLE `unno_unit_specs` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `unno_unit_wrapper`
--

DROP TABLE IF EXISTS `unno_unit_wrapper`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `unno_unit_wrapper` (
  `UnitId` varchar(36) NOT NULL,
  `Name` varchar(200) NOT NULL,
  `Version` int(11) DEFAULT NULL,
  `UpdateAt` datetime DEFAULT NULL,
  PRIMARY KEY (`UnitId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `unno_unit_wrapper`
--

LOCK TABLES `unno_unit_wrapper` WRITE;
/*!40000 ALTER TABLE `unno_unit_wrapper` DISABLE KEYS */;
INSERT INTO `unno_unit_wrapper` VALUES ('47d687dd-c642-4e21-ac52-1d7afbec23b8','项目信息',0,'2014-08-08 16:00:47');
/*!40000 ALTER TABLE `unno_unit_wrapper` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2015-01-25 15:33:01
