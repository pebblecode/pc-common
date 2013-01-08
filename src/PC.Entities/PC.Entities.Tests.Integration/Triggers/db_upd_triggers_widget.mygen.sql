

DELIMITER $$

--
-- Definition of trigger `tr_widget_after_ins`
--

DROP TRIGGER IF EXISTS `tr_widget_after_ins`$$

CREATE TRIGGER `tr_widget_after_ins` AFTER INSERT ON `widget` FOR EACH ROW
BEGIN
  
  /* Log this action */
  INSERT INTO database_updates (updated_entity_type, updated_entity_id, update_type) VALUES ('Widget', NEW.ID, 'I');

END$$


DELIMITER $$

--
-- Definition of trigger `tr_widget_after_upd`
--

DROP TRIGGER IF EXISTS `tr_widget_after_upd`$$

CREATE TRIGGER `tr_widget_after_upd` AFTER UPDATE ON `widget` FOR EACH ROW
BEGIN
  
  /* Log this action */
  INSERT INTO database_updates (updated_entity_type, updated_entity_id, update_type) VALUES ('Widget', NEW.ID, 'U');

END$$


DELIMITER $$

--
-- Definition of trigger `tr_widget_after_del`
--

DROP TRIGGER IF EXISTS `tr_widget_after_del`$$

CREATE TRIGGER `tr_widget_after_del` AFTER DELETE ON `widget` FOR EACH ROW
BEGIN
  
  /* Log this action */
  INSERT INTO database_updates (updated_entity_type, updated_entity_id, update_type) VALUES ('Widget', OLD.ID, 'D');

END$$
