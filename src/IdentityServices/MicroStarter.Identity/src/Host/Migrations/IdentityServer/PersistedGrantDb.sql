CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" varchar(150) NOT NULL,
    "ProductVersion" varchar(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

CREATE TABLE "PersistedGrants" (
    "Key" varchar(200) NOT NULL,
    "Type" varchar(50) NOT NULL,
    "SubjectId" varchar(200) NULL,
    "ClientId" varchar(200) NOT NULL,
    "CreationTime" timestamp without time zone NOT NULL,
    "Expiration" timestamp without time zone NULL,
    "Data" varchar(50000) NOT NULL,
    CONSTRAINT "PK_PersistedGrants" PRIMARY KEY ("Key")
);

CREATE INDEX "IX_PersistedGrants_SubjectId_ClientId_Type" ON "PersistedGrants" ("SubjectId", "ClientId", "Type");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20180807210650_Grants', '2.1.1-rtm-30846');

