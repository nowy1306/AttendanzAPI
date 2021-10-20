CREATE TABLE IF NOT EXISTS class_groups(
	id BIGINT AUTO_INCREMENT,
	name VARCHAR(256) NOT NULL,
	total_number_of_classes INT NOT NULL,
	PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS students(
	id BIGINT AUTO_INCREMENT,
	firstname VARCHAR(256) NOT NULL,
	lastname VARCHAR(256) NOT NULL,
	index_number VARCHAR(8) NOT NULL,
	student_card_code VARCHAR(16) NOT NULL,
	PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS subjects(
	id BIGINT AUTO_INCREMENT,
	name VARCHAR(256) NOT NULL,
	code VARCHAR(16) NOT NULL,
	PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS groups_subjects(
	group_id BIGINT NOT NULL,
	subject_id BIGINT NOT NULL,
	FOREIGN KEY (group_id) REFERENCES class_groups(id),
	FOREIGN KEY (subject_id) REFERENCES subjects(id)
);

CREATE TABLE IF NOT EXISTS groups_students(
	group_id BIGINT NOT NULL,
	student_id BIGINT NOT NULL,
	FOREIGN KEY (group_id) REFERENCES class_groups(id),
	FOREIGN KEY (student_id) REFERENCES students(id)
);

CREATE TABLE IF NOT EXISTS attendance_lists(
	id BIGINT AUTO_INCREMENT,
	group_id BIGINT NOT NULL,
	PRIMARY KEY (id),
	FOREIGN KEY (group_id) REFERENCES class_groups(id)
);

CREATE TABLE IF NOT EXISTS presences(
	id BIGINT AUTO_INCREMENT,
	attendance_list_id BIGINT NOT NULL,
	class_date DATE NOT NULL,
	class_number INT NOT NULL,
	status VARCHAR(16) NOT NULL,
	PRIMARY KEY (id),
	FOREIGN KEY (attendance_list_id) REFERENCES attendance_lists(id)
);