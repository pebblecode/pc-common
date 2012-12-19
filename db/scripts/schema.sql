SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL';

DROP SCHEMA IF EXISTS pc_test ;
CREATE SCHEMA IF NOT EXISTS pc_test DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci ;
USE pc_test ;

-- -----------------------------------------------------
-- Table pc_test.thing
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS pc_test.thing (
  id INT NOT NULL AUTO_INCREMENT ,
  name VARCHAR(45) NOT NULL ,
  corners INT NULL ,
  edges INT NOT NULL ,
  test int null ,
  PRIMARY KEY (id) )
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table pc_test.widget
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS pc_test.widget (
  id INT NOT NULL AUTO_INCREMENT ,
  thing_id INT NOT NULL ,
  description VARCHAR(45) NULL ,
  PRIMARY KEY (id) ,
  INDEX fk_widget_thing (thing_id ASC) ,
  CONSTRAINT fk_widget_thing
    FOREIGN KEY (thing_id )
    REFERENCES pc_test.thing (id )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table pc_test.field_test
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS pc_test.field_test (
  id INT NOT NULL AUTO_INCREMENT ,
  int_field INT NOT NULL ,
  int_field_nullable INT NULL ,
  decimal_field DECIMAL(10,2) NOT NULL ,
  decimal_field_nullable DECIMAL(10,2) NULL ,
  string_field VARCHAR(45) NOT NULL ,
  string_field_nullable VARCHAR(45) NULL ,
  text_field TEXT NOT NULL ,
  text_field_nullable TEXT NULL ,
  datetime_field DATETIME NOT NULL ,
  datetime_field_nullable DATETIME NULL ,
  tinyint_field TINYINT NOT NULL ,
  tinyint_field_nullable TINYINT NULL ,
  timestamp_field TIMESTAMP NOT NULL ,
  timestamp_field_nullable TIMESTAMP NULL ,
  enum_field INT NOT NULL ,
  enum_field_nullable INT NULL ,
  object_field LONGBLOB NOT NULL ,
  object_field_nullable LONGBLOB NULL ,
  foreign_key_field INT NOT NULL ,
  foreign_key_field_nullable INT NULL ,
  int_date_field INT NOT NULL ,
  int_date_field_nullable INT NULL ,
  indexed_field INT NOT NULL ,
  indexed_field_nullable INT NULL ,
  node_id_field VARCHAR(500) NOT NULL ,
  node_id_field_nullable VARCHAR(500) NULL ,
  default_value_is_two INT NOT NULL DEFAULT 2 ,
  PRIMARY KEY (id) ,
  INDEX fk_typeTests_widget1 (foreign_key_field ASC) ,
  INDEX fk_typeTests_widget2 (foreign_key_field_nullable ASC) ,
  INDEX idx_indexed_field (indexed_field ASC) ,
  INDEX idx_indexed_field_nullable (indexed_field_nullable ASC) ,
  CONSTRAINT fk_typeTests_widget1
    FOREIGN KEY (foreign_key_field )
    REFERENCES pc_test.widget (id )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fk_typeTests_widget2
    FOREIGN KEY (foreign_key_field_nullable )
    REFERENCES pc_test.widget (id )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table pc_test.versioned_thing
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS pc_test.versioned_thing (
  id INT NOT NULL AUTO_INCREMENT ,
  name VARCHAR(45) NULL ,
  version_no INT NOT NULL ,
  version_date TIMESTAMP NOT NULL ,
  PRIMARY KEY (id) )
ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table pc_test.controlled_update_thing
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS pc_test.controlled_update_thing (
  id INT NOT NULL AUTO_INCREMENT ,
  name VARCHAR(45) NULL ,
  property_authorization TEXT,
  version_no INT NOT NULL ,
  version_date TIMESTAMP NOT NULL ,
  PRIMARY KEY (id) )
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table pc_test.node_builder_test
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS pc_test.node_builder_test (
  id INT NOT NULL AUTO_INCREMENT ,
  field1 INT NOT NULL ,
  field2 VARCHAR(45) NOT NULL ,
  node_id_field VARCHAR(50) NOT NULL ,
  PRIMARY KEY (id) )
ENGINE = InnoDB;



SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;