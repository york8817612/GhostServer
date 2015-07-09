/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50617
Source Host           : localhost:3306
Source Database       : ghost

Target Server Type    : MYSQL
Target Server Version : 50617
File Encoding         : 65001

Date: 2015-07-09 17:11:21
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `accounts`
-- ----------------------------
DROP TABLE IF EXISTS `accounts`;
CREATE TABLE `accounts` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userName` varchar(20) CHARACTER SET big5 NOT NULL,
  `password` varchar(128) CHARACTER SET big5 NOT NULL,
  `salt` varchar(32) NOT NULL,
  `pin` varchar(32) NOT NULL,
  `birthday` datetime NOT NULL,
  `creation` datetime NOT NULL,
  `gender` int(1) NOT NULL,
  `isLoggedIn` int(1) NOT NULL,
  `isBanned` int(1) NOT NULL,
  `isMaster` int(1) NOT NULL,
  `cashPoint` int(8) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of accounts
-- ----------------------------
INSERT INTO `accounts` VALUES ('1', 'admin', '123456', '', '', '2015-04-07 21:11:26', '2015-04-07 21:11:30', '0', '0', '0', '1', '0');

-- ----------------------------
-- Table structure for `characters`
-- ----------------------------
DROP TABLE IF EXISTS `characters`;
CREATE TABLE `characters` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `accountId` int(11) NOT NULL,
  `worldId` int(1) NOT NULL,
  `name` varchar(20) NOT NULL,
  `title` varchar(20) NOT NULL,
  `gender` int(1) NOT NULL DEFAULT '1',
  `hair` int(8) NOT NULL DEFAULT '0',
  `eyes` int(8) NOT NULL DEFAULT '0',
  `level` int(4) NOT NULL DEFAULT '1',
  `classId` int(4) NOT NULL DEFAULT '0',
  `classLv` int(4) NOT NULL DEFAULT '-1',
  `hp` int(8) NOT NULL DEFAULT '1',
  `maxHp` int(8) NOT NULL DEFAULT '1',
  `sp` int(8) NOT NULL DEFAULT '1',
  `maxSp` int(8) NOT NULL DEFAULT '1',
  `exp` int(8) NOT NULL DEFAULT '1',
  `rank` int(8) NOT NULL DEFAULT '0',
  `c_str` int(4) NOT NULL DEFAULT '1',
  `c_dex` int(4) NOT NULL DEFAULT '1',
  `c_vit` int(4) NOT NULL,
  `c_int` int(4) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of characters
-- ----------------------------
INSERT INTO `characters` VALUES ('20', '2', '0', 'fdffd', '', '1', '9010011', '9110011', '1', '0', '255', '31', '31', '15', '15', '0', '0', '3', '3', '3', '3');
INSERT INTO `characters` VALUES ('21', '1', '0', 'ggfg', '', '1', '9010011', '9110011', '1', '0', '255', '31', '31', '15', '15', '0', '0', '3', '3', '3', '3');
INSERT INTO `characters` VALUES ('22', '1', '0', 'fgff', '', '1', '9010041', '9110031', '1', '0', '255', '31', '31', '15', '15', '0', '0', '3', '3', '3', '3');
INSERT INTO `characters` VALUES ('23', '1', '0', '25456', '', '2', '9010032', '9110022', '1', '0', '255', '31', '31', '15', '15', '0', '0', '3', '3', '3', '3');

-- ----------------------------
-- Table structure for `items`
-- ----------------------------
DROP TABLE IF EXISTS `items`;
CREATE TABLE `items` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `cid` int(11) NOT NULL,
  `itemId` int(8) NOT NULL,
  `quantity` smallint(4) NOT NULL DEFAULT '1',
  `slot` int(3) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of items
-- ----------------------------
INSERT INTO `items` VALUES ('1', '21', '8010011', '1', '255');
INSERT INTO `items` VALUES ('2', '21', '8110011', '1', '254');
INSERT INTO `items` VALUES ('3', '22', '7940011', '1', '255');
INSERT INTO `items` VALUES ('4', '22', '8120011', '1', '249');
INSERT INTO `items` VALUES ('5', '23', '8030011', '1', '255');
INSERT INTO `items` VALUES ('6', '23', '8120012', '1', '249');
