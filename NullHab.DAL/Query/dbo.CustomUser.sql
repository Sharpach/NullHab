CREATE TABLE dbo.CustomUser (
    id bigserial CONSTRAINT id PRIMARY KEY,
    Email    varchar (100),
    PasswordHash     varchar (4000),
    UserName       varchar (100)
);