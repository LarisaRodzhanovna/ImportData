CREATE TABLE IF NOT EXISTS job_titles (
    id SERIAL PRIMARY KEY,
    "name" VARCHAR(255) UNIQUE NOT NULL
);

COMMENT ON TABLE "public".job_titles IS 'Названия должностей';
COMMENT ON COLUMN "public".job_titles.id IS 'Идентификатор должности';
COMMENT ON COLUMN "public".job_titles."name" IS 'Название должности';


CREATE TABLE IF NOT EXISTS departments (
    id SERIAL PRIMARY KEY,
    "name" VARCHAR(255) NOT NULL,
    parent_id INT,
    manager_id INT,
    phone VARCHAR(255) NOT NULL
);

COMMENT ON TABLE "public".departments IS 'Подразделение';
COMMENT ON COLUMN "public".departments.id IS 'Идентификатор подразделения';
COMMENT ON COLUMN "public".departments."name" IS 'Название подразделения';
COMMENT ON COLUMN "public".departments.parent_id IS 'Идентификатор вышестоящего подразделения';
COMMENT ON COLUMN "public".departments.manager_id IS 'Название руководителя подразделения';
COMMENT ON COLUMN "public".departments.phone IS 'Телефон подразделения';

CREATE TABLE IF NOT EXISTS Employees (
    id SERIAL PRIMARY KEY,
    department_id INT,
    full_name VARCHAR(255) NOT NULL,
    login VARCHAR(255) NOT NULL,
    "password" VARCHAR(255),
    job_title_id INT    
);

COMMENT ON TABLE "public".employees IS 'Сотрудники';
COMMENT ON COLUMN "public".employees.id IS 'Идентификатор сотрудника';
COMMENT ON COLUMN "public".employees.department_id IS 'Идентификатор подразделения сотрудника';
COMMENT ON COLUMN "public".employees.full_name IS 'Фамилия Имя Отчество';
COMMENT ON COLUMN "public".employees.login IS 'Логин';
COMMENT ON COLUMN "public".employees."password" IS 'Пароль (хэш)';
COMMENT ON COLUMN "public".employees.job_title_id IS 'Идентификатор должности сотрудника';


ALTER TABLE departments
ADD CONSTRAINT fk_parent_id FOREIGN KEY (parent_id) REFERENCES departments(id),
ADD CONSTRAINT fk_manager_id FOREIGN KEY (manager_id) REFERENCES employees(id);

ALTER TABLE employees 
ADD CONSTRAINT fk_job_title_id FOREIGN KEY (job_title_id) REFERENCES job_titles(id),
ADD CONSTRAINT fk_department_id FOREIGN KEY (department_id) REFERENCES departments(id);