ALTER TABLE user
	ADD CONSTRAINT CK_gender
		CHECK (gender = 'Nam' OR gender = 'Nữ' OR gender = 'Khác');
        
ALTER TABLE user
	ADD CONSTRAINT CK_birthday
		CHECK (birthday < createDate);

ALTER TABLE question
	ADD CONSTRAINT CK_answer
		CHECK (answer = 'A' OR answer = 'B' OR answer = 'C' OR answer = 'D');
        
ALTER TABLE multiplechoice
	ADD CONSTRAINT CK_status
		CHECK (status = 'public' OR status = 'private');
        
ALTER TABLE multiplechoice
	ADD CONSTRAINT CK_rating
		CHECK (rating >= 0 AND rating <= 5);
        
ALTER TABLE interaction
	ADD CONSTRAINT CK_rate
		CHECK (rate >= 0 AND rate <= 5);
        
alter table user
	add constraint CK_account_length
		check (LENGTH(account) >= 8);
        
alter table user
	add constraint CK_password_length
		check (LENGTH(password) >= 8);
        
        

