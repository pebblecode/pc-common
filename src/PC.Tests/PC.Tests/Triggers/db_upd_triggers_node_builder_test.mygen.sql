

DELIMITER $$

--
-- Definition of trigger `tr_node_builder_test_after_ins`
--

DROP TRIGGER IF EXISTS `tr_node_builder_test_after_ins`$$

CREATE TRIGGER `tr_node_builder_test_after_ins` AFTER INSERT ON `node_builder_test` FOR EACH ROW
BEGIN
  
  /* Log this action */
  INSERT INTO database_updates (updated_entity_type, updated_entity_id, update_type) VALUES ('NodeBuilderTest', NEW.ID, 'I');

END$$


DELIMITER $$

--
-- Definition of trigger `tr_node_builder_test_after_upd`
--

DROP TRIGGER IF EXISTS `tr_node_builder_test_after_upd`$$

CREATE TRIGGER `tr_node_builder_test_after_upd` AFTER UPDATE ON `node_builder_test` FOR EACH ROW
BEGIN
  
  /* Log this action */
  INSERT INTO database_updates (updated_entity_type, updated_entity_id, update_type) VALUES ('NodeBuilderTest', NEW.ID, 'U');

END$$


DELIMITER $$

--
-- Definition of trigger `tr_node_builder_test_after_del`
--

DROP TRIGGER IF EXISTS `tr_node_builder_test_after_del`$$

CREATE TRIGGER `tr_node_builder_test_after_del` AFTER DELETE ON `node_builder_test` FOR EACH ROW
BEGIN
  
  /* Log this action */
  INSERT INTO database_updates (updated_entity_type, updated_entity_id, update_type) VALUES ('NodeBuilderTest', OLD.ID, 'D');

END$$
