

DELIMITER $$

--
-- Definition of trigger `tr_field_test_after_ins`
--

DROP TRIGGER IF EXISTS `tr_field_test_after_ins`$$

CREATE TRIGGER `tr_field_test_after_ins` AFTER INSERT ON `field_test` FOR EACH ROW
BEGIN
  
  /* Log this action */
  INSERT INTO database_updates (updated_entity_type, updated_entity_id, update_type) VALUES ('FieldTest', NEW.ID, 'I');

END$$


DELIMITER $$

--
-- Definition of trigger `tr_field_test_after_upd`
--

DROP TRIGGER IF EXISTS `tr_field_test_after_upd`$$

CREATE TRIGGER `tr_field_test_after_upd` AFTER UPDATE ON `field_test` FOR EACH ROW
BEGIN
  
  /* Log this action */
  INSERT INTO database_updates (updated_entity_type, updated_entity_id, update_type) VALUES ('FieldTest', NEW.ID, 'U');

END$$


DELIMITER $$

--
-- Definition of trigger `tr_field_test_after_del`
--

DROP TRIGGER IF EXISTS `tr_field_test_after_del`$$

CREATE TRIGGER `tr_field_test_after_del` AFTER DELETE ON `field_test` FOR EACH ROW
BEGIN
  
  /* Log this action */
  INSERT INTO database_updates (updated_entity_type, updated_entity_id, update_type) VALUES ('FieldTest', OLD.ID, 'D');

END$$
