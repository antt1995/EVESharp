/*
SQLyog Community Edition- MySQL GUI v7.11 
MySQL - 5.0.67-community-nt : Database - eve_evemu_dynamic
*********************************************************************
*/


/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

/*Table structure for table `cacheLocations` */

DROP TABLE IF EXISTS `cacheLocations`;

CREATE TABLE `cacheLocations` (
  `locationID` int(10) unsigned NOT NULL default '0',
  `locationName` varchar(100) NOT NULL default '',
  `x` double NOT NULL default '0',
  `y` double NOT NULL default '0',
  `z` double NOT NULL default '0',
  PRIMARY KEY  (`locationID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `cacheLocations` */

/*Table structure for table `cacheOwners` */

DROP TABLE IF EXISTS `cacheOwners`;

CREATE TABLE `cacheOwners` (
  `ownerID` int(10) unsigned NOT NULL default '0',
  `ownerName` varchar(100) NOT NULL default '',
  `typeID` int(10) unsigned NOT NULL default '0',
  PRIMARY KEY  (`ownerID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `cacheOwners` */

/*Table structure for table `chrCorporationRoles` */

DROP TABLE IF EXISTS `chrCorporationRoles`;

CREATE TABLE `chrCorporationRoles` (
  `characterID` int(10) unsigned NOT NULL default '0',
  `corprole` bigint(20) unsigned NOT NULL default '0',
  `rolesAtAll` bigint(20) unsigned NOT NULL default '0',
  `rolesAtBase` bigint(20) unsigned NOT NULL default '0',
  `rolesAtHQ` bigint(20) unsigned NOT NULL default '0',
  `rolesAtOther` bigint(20) unsigned NOT NULL default '0',
  PRIMARY KEY  (`characterID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `chrCorporationRoles` */

/*Table structure for table `chrEmployment` */

DROP TABLE IF EXISTS `chrEmployment`;

CREATE TABLE `chrEmployment` (
  `characterID` int(10) unsigned NOT NULL default '0',
  `corporationID` int(10) unsigned NOT NULL default '0',
  `startDate` bigint(20) unsigned NOT NULL default '0',
  `deleted` tinyint(4) NOT NULL default '0',
  PRIMARY KEY  (`characterID`,`corporationID`,`startDate`),
  KEY `corporationID` (`corporationID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `chrEmployment` */

/*Table structure for table `chrMissionState` */

DROP TABLE IF EXISTS `chrMissionState`;

CREATE TABLE `chrMissionState` (
  `characterID` int(10) unsigned NOT NULL default '0',
  `missionID` int(10) unsigned NOT NULL default '0',
  `missionState` tinyint(3) unsigned NOT NULL default '0',
  `expirationTime` bigint(20) unsigned NOT NULL default '0',
  PRIMARY KEY  (`characterID`,`missionID`),
  KEY `missionID` (`missionID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `chrMissionState` */

/*Table structure for table `chrNotes` */

DROP TABLE IF EXISTS `chrNotes`;

CREATE TABLE `chrNotes` (
  `itemID` int(10) unsigned NOT NULL default '0',
  `ownerID` int(10) unsigned default NULL,
  `note` text
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `chrNotes` */

/*Table structure for table `chrNPCStandings` */

DROP TABLE IF EXISTS `chrNPCStandings`;

CREATE TABLE `chrNPCStandings` (
  `characterID` int(10) unsigned NOT NULL default '0',
  `fromID` int(10) unsigned NOT NULL default '0',
  `standing` double NOT NULL default '0',
  PRIMARY KEY  (`characterID`,`fromID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `chrNPCStandings` */

/*Table structure for table `chrOffers` */

DROP TABLE IF EXISTS `chrOffers`;

CREATE TABLE `chrOffers` (
  `characterID` int(10) unsigned NOT NULL default '0',
  `offerID` int(10) unsigned NOT NULL default '0',
  `expirationTime` bigint(20) unsigned NOT NULL default '0',
  PRIMARY KEY  (`characterID`,`offerID`),
  KEY `offerID` (`offerID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `chrOffers` */

/*Table structure for table `chrOwnerNote` */

DROP TABLE IF EXISTS `chrOwnerNote`;

CREATE TABLE `chrOwnerNote` (
  `noteID` int(10) unsigned NOT NULL auto_increment,
  `ownerID` int(10) unsigned NOT NULL,
  `label` text,
  `note` text,
  UNIQUE KEY `noteID` (`noteID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `chrOwnerNote` */

/*Table structure for table `chrStandings` */

DROP TABLE IF EXISTS `chrStandings`;

CREATE TABLE `chrStandings` (
  `characterID` int(10) unsigned NOT NULL default '0',
  `toID` int(10) unsigned NOT NULL default '0',
  `standing` double NOT NULL default '0',
  PRIMARY KEY  (`characterID`,`toID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `chrStandings` */


/*Table structure for table `courierMissions` */

DROP TABLE IF EXISTS `courierMissions`;

CREATE TABLE `courierMissions` (
  `missionID` int(10) unsigned NOT NULL auto_increment,
  `kind` tinyint(3) unsigned NOT NULL default '0',
  `state` tinyint(3) unsigned NOT NULL default '0',
  `availabilityID` int(10) unsigned default NULL,
  `inOrder` tinyint(3) unsigned NOT NULL default '0',
  `description` text NOT NULL,
  `issuerID` int(10) unsigned default NULL,
  `assigneeID` int(10) unsigned default NULL,
  `acceptFee` double NOT NULL default '0',
  `acceptorID` int(10) unsigned default NULL,
  `dateIssued` int(10) unsigned NOT NULL default '0',
  `dateExpires` int(10) unsigned NOT NULL default '0',
  `dateAccepted` int(10) unsigned NOT NULL default '0',
  `dateCompleted` int(10) unsigned NOT NULL default '0',
  `totalReward` double NOT NULL default '0',
  `tracking` tinyint(3) unsigned NOT NULL default '0',
  `pickupStationID` int(10) unsigned default NULL,
  `craterID` int(10) unsigned default NULL,
  `dropStationID` int(10) unsigned default NULL,
  `volume` double NOT NULL default '0',
  `pickupSolarSystemID` int(10) unsigned default NULL,
  `pickupRegionID` int(10) unsigned default NULL,
  `dropSolarSystemID` int(10) unsigned default NULL,
  `dropRegionID` int(10) unsigned default NULL,
  PRIMARY KEY  (`missionID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `courierMissions` */

/*Table structure for table `droneState` */

DROP TABLE IF EXISTS `droneState`;

CREATE TABLE `droneState` (
  `droneID` int(10) unsigned NOT NULL default '0',
  `solarSystemID` int(10) unsigned NOT NULL default '0',
  `ownerID` int(10) unsigned NOT NULL default '0',
  `controllerID` int(10) unsigned NOT NULL default '0',
  `activityState` int(10) unsigned NOT NULL default '0',
  `typeID` int(10) unsigned NOT NULL default '0',
  `controllerOwnerID` int(10) unsigned NOT NULL default '0',
  PRIMARY KEY  (`droneID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `droneState` */

/*Table structure for table `npcStandings` */

DROP TABLE IF EXISTS `npcStandings`;

CREATE TABLE `npcStandings` (
  `fromID` int(10) unsigned NOT NULL default '0',
  `toID` int(10) unsigned NOT NULL default '0',
  `standing` double NOT NULL default '0',
  PRIMARY KEY  (`fromID`,`toID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `npcStandings` */

/*Table structure for table `ramAssemblyLineStationCostLogs` */

DROP TABLE IF EXISTS `ramAssemblyLineStationCostLogs`;

CREATE TABLE `ramAssemblyLineStationCostLogs` (
  `assemblyLineTypeID` int(11) NOT NULL default '0',
  `stationID` int(11) NOT NULL default '0',
  `logDateTime` char(20) NOT NULL default '',
  `_usage` double NOT NULL default '0',
  `costPerHour` float NOT NULL default '0',
  PRIMARY KEY  (`assemblyLineTypeID`),
  KEY `stationID` (`stationID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `ramAssemblyLineStationCostLogs` */

/*Table structure for table `ramJobs` */

DROP TABLE IF EXISTS `ramJobs`;

CREATE TABLE `ramJobs` (
  `jobID` int(10) unsigned NOT NULL auto_increment,
  `ownerID` int(10) unsigned NOT NULL,
  `installerID` int(10) unsigned NOT NULL,
  `assemblyLineID` int(10) unsigned NOT NULL,
  `installedItemID` int(10) unsigned NOT NULL,
  `installTime` bigint(20) unsigned NOT NULL,
  `beginProductionTime` bigint(20) unsigned NOT NULL,
  `pauseProductionTime` bigint(20) unsigned default NULL,
  `endProductionTime` bigint(20) unsigned NOT NULL,
  `description` varchar(250) NOT NULL default 'blah',
  `runs` int(10) unsigned NOT NULL,
  `outputFlag` int(10) unsigned NOT NULL,
  `completedStatusID` int(10) unsigned NOT NULL,
  `installedInSolarSystemID` int(10) unsigned NOT NULL,
  `licensedProductionRuns` int(10) default NULL,
  PRIMARY KEY  (`jobID`),
  KEY `RAMJOBS_ASSEMBLYLINES` (`assemblyLineID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `ramJobs` */

/*Table structure for table `rentalInfo` */

DROP TABLE IF EXISTS `rentalInfo`;

CREATE TABLE `rentalInfo` (
  `stationID` int(10) unsigned NOT NULL default '0',
  `slotNumber` int(10) unsigned NOT NULL default '0',
  `renterID` int(10) unsigned NOT NULL default '0',
  `typeID` int(10) unsigned NOT NULL default '0',
  `rentPeriodInDays` int(10) unsigned NOT NULL default '0',
  `periodCost` double NOT NULL default '0',
  `billID` int(10) unsigned NOT NULL default '0',
  `balanceDueDate` int(10) unsigned NOT NULL default '0',
  `discontinue` tinyint(3) unsigned NOT NULL default '0',
  `publiclyAvailable` tinyint(3) unsigned NOT NULL default '0',
  PRIMARY KEY  (`stationID`,`slotNumber`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `rentalInfo` */

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;