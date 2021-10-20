USE db;

CREATE TABLE IF NOT EXISTS accounts(
	id BIGINT AUTO_INCREMENT,
	login VARCHAR(64) NOT NULL,
	password VARCHAR(64) NOT NULL,
	card_code VARCHAR(16) NOT NULL,
	PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS subjects(
	id BIGINT AUTO_INCREMENT,
	name VARCHAR(256) NOT NULL,
	code VARCHAR(16) NOT NULL,
	semester_type VARCHAR(16) NOT NULL,
	semester INT NOT NULL,
	PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS _groups(
	id BIGINT AUTO_INCREMENT,
	account_id BIGINT NOT NULL,
	subject_id BIGINT NOT NULL,
	name VARCHAR(256) NOT NULL,
	total_class_number INT NOT NULL,
	start_date DATETIME NOT NULL,
	end_date DATETIME NOT NULL,
	week_day VARCHAR(16) NOT NULL,
	start_time TIME NOT NULL,
	end_time TIME NOT NULL,
	PRIMARY KEY (id),
	FOREIGN KEY (account_id) REFERENCES accounts(id),
	FOREIGN KEY (subject_id) REFERENCES subjects(id)
);

CREATE TABLE IF NOT EXISTS students(
	id BIGINT AUTO_INCREMENT,
	firstname VARCHAR(256) NOT NULL,
	lastname VARCHAR(256) NOT NULL,
	index_number VARCHAR(8) NOT NULL,
	PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS group_students(
	id BIGINT AUTO_INCREMENT,
	group_id BIGINT NOT NULL,
	student_id BIGINT NOT NULL,
	student_card_code VARCHAR(16) NOT NULL,
	PRIMARY KEY (id),
	FOREIGN KEY (group_id) REFERENCES _groups(id),
	FOREIGN KEY (student_id) REFERENCES students(id)
);

CREATE TABLE IF NOT EXISTS classes(
	id BIGINT AUTO_INCREMENT,
	group_id BIGINT NOT NULL,
	date DATETIME NOT NULL,
	note VARCHAR(256),
	PRIMARY KEY (id),
	FOREIGN KEY (group_id) REFERENCES _groups(id)
);

CREATE TABLE IF NOT EXISTS presences(
	id BIGINT AUTO_INCREMENT,
	group_student_id BIGINT NOT NULL,
	class_id BIGINT NOT NULL,
	status VARCHAR(16) NOT NULL,
	PRIMARY KEY (id),
	FOREIGN KEY (group_student_id) REFERENCES group_students(id),
	FOREIGN KEY (class_id) REFERENCES classes(id)
);

CREATE TABLE IF NOT EXISTS scanners(
	id BIGINT AUTO_INCREMENT,
	_key VARCHAR(32) NOT NULL,
	description VARCHAR(128) NOT NULL,
	is_blocked TINYINT(1) NOT NULL,
	is_confirmed TINYINT(1) NOT NULL,
	PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS control_processes(
	id BIGINT AUTO_INCREMENT,
	scanner_id BIGINT DEFAULT NULL,
	class_id BIGINT NOT NULL,
	control_mode VARCHAR(16) NOT NULL,
	PRIMARY KEY (id),
	FOREIGN KEY (scanner_id) REFERENCES scanners(id),
	FOREIGN KEY (class_id) REFERENCES classes(id)
);

INSERT INTO accounts(login, password, card_code) VALUES("demo", "demo", "demo");