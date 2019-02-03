CREATE TABLE dbo.UserClaim (
    id			bigserial CONSTRAINT claimId PRIMARY KEY,
    UserId		bigint REFERENCES dbo.CustomUser ON DELETE CASCADE,
    ClaimType		varchar (4000),
    ClaimValue		varchar (4000)
);