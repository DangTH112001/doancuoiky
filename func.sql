USE `doan`;
DROP function IF EXISTS `get_rank`;

USE `doan`;
DROP function IF EXISTS `doan`.`get_rank`;
;

DELIMITER $$
USE `doan`$$
CREATE DEFINER=`root`@`localhost` FUNCTION `get_rank`(id int) RETURNS int(11)
BEGIN
	DECLARE result int;
	SELECT @RN:=@RN+1 as r into result
	FROM (
		SELECT  DISTINCT uid, sum(score) tong
		FROM    interaction
		group by uid
		ORDER   BY sum(score) DESC
	 ) D
	CROSS JOIN  (Select @RN:=0) Z
    WHERE D.uid = id
	ORDER BY tong desc;
	RETURN result;
END$$

DELIMITER ;
;