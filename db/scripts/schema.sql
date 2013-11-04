USE master
IF EXISTS(select * from sys.databases where name='pc_test')
DROP DATABASE pc_test
CREATE DATABASE pc_test

USE pc_test


-- -----------------------------------------------------
-- Table pc_test.thing
-- -----------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.tables t join sys.schemas s on (t.schema_id = s.schema_id)
 where s.name = 'pc_test' and t.name = 'thing') 
 create table thing 
 (
  id INT IDENTITY ,
  name NVARCHAR(45) NOT NULL ,
  corners INT NULL ,
  edges INT NOT NULL ,
  test int null ,
  PRIMARY KEY (id) )

-- -----------------------------------------------------
-- Table pc_test.widget
-- -----------------------------------------------------

IF NOT EXISTS (SELECT * FROM sys.tables t join sys.schemas s on (t.schema_id = s.schema_id)
 where s.name = 'pc_test' and t.name = 'widget') 
 create table widget(
  id INT IDENTITY ,
  thing_id INT NOT NULL ,
  DESCRIPTION NVARCHAR(45) NULL ,
  PRIMARY KEY (id) ,
  CONSTRAINT fk_widget_thing
    FOREIGN KEY (thing_id)
    REFERENCES thing (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)

CREATE INDEX fk_widget_thing ON widget (thing_id)



-- -----------------------------------------------------
-- Table pc_test.field_test
-- -----------------------------------------------------

IF NOT EXISTS (SELECT * FROM sys.tables t join sys.schemas s on (t.schema_id = s.schema_id)
 where s.name = 'pc_test' and t.name = 'field_test') 
 create table field_test (
  id INT IDENTITY ,
  int_field INT NOT NULL ,
  int_field_nullable INT NULL ,
  decimal_field DECIMAL(10,2) NOT NULL ,
  decimal_field_nullable DECIMAL(10,2) NULL ,
  string_field NVARCHAR(45) NOT NULL ,
  string_field_nullable NVARCHAR(45) NULL ,
  text_field NTEXT NOT NULL ,
  text_field_nullable NTEXT NULL ,
  datetime_field DATETIME NOT NULL ,
  datetime_field_nullable DATETIME NULL ,
  tinyint_field TINYINT NOT NULL ,
  tinyint_field_nullable TINYINT NULL ,
  timestamp_field TIMESTAMP NOT NULL ,
  timestamp_field_nullable DATETIME NULL ,
  enum_field INT NOT NULL ,
  enum_field_nullable INT NULL ,
  object_field BINARY NOT NULL ,
  object_field_nullable BINARY NULL ,
  foreign_key_field INT NOT NULL ,
  foreign_key_field_nullable INT NULL ,
  int_date_field INT NOT NULL ,
  int_date_field_nullable INT NULL ,
  indexed_field INT NOT NULL ,
  indexed_field_nullable INT NULL ,
  node_id_field NVARCHAR(500) NOT NULL ,
  node_id_field_nullable NVARCHAR(500) NULL ,
  default_value_is_two INT NOT NULL DEFAULT 2 ,
  PRIMARY KEY (id) ,
  CONSTRAINT fk_typeTests_widget1
    FOREIGN KEY (foreign_key_field )
    REFERENCES widget (id )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fk_typeTests_widget2
    FOREIGN KEY (foreign_key_field_nullable )
    REFERENCES widget (id )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)

  CREATE INDEX fk_typeTests_widget1 ON field_test (foreign_key_field)
  CREATE INDEX fk_typeTests_widget2 ON field_test (foreign_key_field_nullable)
  CREATE INDEX idx_indexed_field ON field_test (indexed_field)
  CREATE INDEX idx_indexed_field_nullable ON field_test (indexed_field_nullable)

-- -----------------------------------------------------
-- Table pc_test.versioned_thing
-- -----------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.tables t join sys.schemas s on (t.schema_id = s.schema_id)
 where s.name = 'pc_test' and t.name = 'versioned_thing') 
 create table versioned_thing (
  id INT IDENTITY ,
  name NVARCHAR(45) NULL ,
  version_no INT NOT NULL ,
  version_date TIMESTAMP NOT NULL ,
  PRIMARY KEY (id) )

-- -----------------------------------------------------
-- Table pc_test.controlled_update_thing
-- -----------------------------------------------------

IF NOT EXISTS (SELECT * FROM sys.tables t join sys.schemas s on (t.schema_id = s.schema_id)
 where s.name = 'pc_test' and t.name = 'controlled_update_thing') 
 create table controlled_update_thing (
  id INT IDENTITY ,
  name NVARCHAR(45) NULL ,
  property_authorization NTEXT,
  version_no INT NOT NULL ,
  version_date TIMESTAMP NOT NULL ,
  PRIMARY KEY (id) )

-- -----------------------------------------------------
-- Table pc_test.node_builder_test
-- -----------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.tables t join sys.schemas s on (t.schema_id = s.schema_id)
 where s.name = 'pc_test' and t.name = 'node_builder_test') 
 create table node_builder_test (
  id INT IDENTITY ,
  field1 INT NOT NULL ,
  field2 NVARCHAR(45) NOT NULL ,
  node_id_field NVARCHAR(50) NOT NULL ,
  PRIMARY KEY (id) )