

DELIMITER $$

--
-- Definition of trigger `tr_thing_after_ins`
--

DROP TRIGGER IF EXISTS `tr_thing_after_ins`$$

CREATE TRIGGER `tr_thing_after_ins` AFTER INSERT ON `thing` FOR EACH ROW
BEGIN
  
  /* Log this action */
  INSERT INTO database_updates (updated_entity_type, updated_entity_id, update_type) VALUES ('Thing', NEW.ID, 'I');

END$$


DELIMITER $$

--
-- Definition of trigger `tr_thing_after_upd`
--

DROP TRIGGER IF EXISTS `tr_thing_after_upd`$$

CREATE TRIGGER `tr_thing_after_upd` AFTER UPDATE ON `thing` FOR EACH ROW
BEGIN
  
  /* Log this action */
  INSERT INTO database_updates (updated_entity_type, updated_entity_id, update_type) VALUES ('Thing', NEW.ID, 'U');

END$$


DELIMITER $$

--
-- Definition of trigger `tr_thing_after_del`
--

DROP TRIGGER IF EXISTS `tr_thing_after_del`$$

CREATE TRIGGER `tr_thing_after_del` AFTER DELETE ON `thing` FOR EACH ROW
BEGIN
  
  /* Log this action */
  INSERT INTO database_updates (updated_entity_type, updated_entity_id, update_type) VALUES ('Thing', OLD.ID, 'D');

END$$
