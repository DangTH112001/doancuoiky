DELIMITER $$
CREATE TRIGGER TRG_total_INSERT
AFTER INSERT ON `belong`
FOR EACH ROW
BEGIN
	UPDATE `multiplechoice`
    SET total = total + 1
    WHERE id = NEW.mcid;
END$$
DELIMITER ;

DELIMITER $$
CREATE TRIGGER TRG_total_DELETE
BEFORE DELETE ON `belong`
FOR EACH ROW
BEGIN
	UPDATE `multiplechoice`
    SET total = total - 1
    WHERE id = OLD.mcid;
END$$
DELIMITER ;

DELIMITER $$
CREATE TRIGGER TRG_participant_UPDATE
AFTER UPDATE ON `interaction`
FOR EACH ROW
BEGIN
	IF !(NEW.done <=> OLD.done) THEN
		UPDATE `multiplechoice`
		SET participant = participant + 1
		WHERE id = NEW.mcid AND NEW.done = 1;
	END IF;
END$$
DELIMITER ;


DELIMITER $$
CREATE TRIGGER TRG_participant_INSERT
AFTER INSERT ON `interaction`
FOR EACH ROW
BEGIN
	UPDATE `multiplechoice`
    SET participant = participant + 1
    WHERE id = NEW.mcid AND NEW.done = 1;
END$$
DELIMITER ;

DELIMITER $$
CREATE TRIGGER TRG_participant_DELETE
BEFORE DELETE ON `interaction`
FOR EACH ROW
BEGIN
	UPDATE `multiplechoice`
    SET participant = participant - 1
    WHERE id = OLD.mcid AND OLD.done = 1;
END$$
DELIMITER ;



