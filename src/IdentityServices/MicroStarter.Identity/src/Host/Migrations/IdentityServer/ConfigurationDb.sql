CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" varchar(150) NOT NULL,
    "ProductVersion" varchar(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

CREATE TABLE "ApiResources" (
    "Id" serial NOT NULL,
    "Enabled" boolean NOT NULL,
    "Name" varchar(200) NOT NULL,
    "DisplayName" varchar(200) NULL,
    "Description" varchar(1000) NULL,
    CONSTRAINT "PK_ApiResources" PRIMARY KEY ("Id")
);

CREATE TABLE "Clients" (
    "Id" serial NOT NULL,
    "Enabled" boolean NOT NULL,
    "ClientId" varchar(200) NOT NULL,
    "ProtocolType" varchar(200) NOT NULL,
    "RequireClientSecret" boolean NOT NULL,
    "ClientName" varchar(200) NULL,
    "Description" varchar(1000) NULL,
    "ClientUri" varchar(2000) NULL,
    "LogoUri" varchar(2000) NULL,
    "RequireConsent" boolean NOT NULL,
    "AllowRememberConsent" boolean NOT NULL,
    "AlwaysIncludeUserClaimsInIdToken" boolean NOT NULL,
    "RequirePkce" boolean NOT NULL,
    "AllowPlainTextPkce" boolean NOT NULL,
    "AllowAccessTokensViaBrowser" boolean NOT NULL,
    "FrontChannelLogoutUri" varchar(2000) NULL,
    "FrontChannelLogoutSessionRequired" boolean NOT NULL,
    "BackChannelLogoutUri" varchar(2000) NULL,
    "BackChannelLogoutSessionRequired" boolean NOT NULL,
    "AllowOfflineAccess" boolean NOT NULL,
    "IdentityTokenLifetime" integer NOT NULL,
    "AccessTokenLifetime" integer NOT NULL,
    "AuthorizationCodeLifetime" integer NOT NULL,
    "ConsentLifetime" integer NULL,
    "AbsoluteRefreshTokenLifetime" integer NOT NULL,
    "SlidingRefreshTokenLifetime" integer NOT NULL,
    "RefreshTokenUsage" integer NOT NULL,
    "UpdateAccessTokenClaimsOnRefresh" boolean NOT NULL,
    "RefreshTokenExpiration" integer NOT NULL,
    "AccessTokenType" integer NOT NULL,
    "EnableLocalLogin" boolean NOT NULL,
    "IncludeJwtId" boolean NOT NULL,
    "AlwaysSendClientClaims" boolean NOT NULL,
    "ClientClaimsPrefix" varchar(200) NULL,
    "PairWiseSubjectSalt" varchar(200) NULL,
    CONSTRAINT "PK_Clients" PRIMARY KEY ("Id")
);

CREATE TABLE "IdentityResources" (
    "Id" serial NOT NULL,
    "Enabled" boolean NOT NULL,
    "Name" varchar(200) NOT NULL,
    "DisplayName" varchar(200) NULL,
    "Description" varchar(1000) NULL,
    "Required" boolean NOT NULL,
    "Emphasize" boolean NOT NULL,
    "ShowInDiscoveryDocument" boolean NOT NULL,
    CONSTRAINT "PK_IdentityResources" PRIMARY KEY ("Id")
);

CREATE TABLE "ApiClaims" (
    "Id" serial NOT NULL,
    "Type" varchar(200) NOT NULL,
    "ApiResourceId" integer NOT NULL,
    CONSTRAINT "PK_ApiClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ApiClaims_ApiResources_ApiResourceId" FOREIGN KEY ("ApiResourceId") REFERENCES "ApiResources" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ApiScopes" (
    "Id" serial NOT NULL,
    "Name" varchar(200) NOT NULL,
    "DisplayName" varchar(200) NULL,
    "Description" varchar(1000) NULL,
    "Required" boolean NOT NULL,
    "Emphasize" boolean NOT NULL,
    "ShowInDiscoveryDocument" boolean NOT NULL,
    "ApiResourceId" integer NOT NULL,
    CONSTRAINT "PK_ApiScopes" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ApiScopes_ApiResources_ApiResourceId" FOREIGN KEY ("ApiResourceId") REFERENCES "ApiResources" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ApiSecrets" (
    "Expiration" timestamp without time zone NULL,
    "Id" serial NOT NULL,
    "Description" varchar(1000) NULL,
    "Value" varchar(2000) NULL,
    "Type" varchar(250) NULL,
    "ApiResourceId" integer NOT NULL,
    CONSTRAINT "PK_ApiSecrets" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ApiSecrets_ApiResources_ApiResourceId" FOREIGN KEY ("ApiResourceId") REFERENCES "ApiResources" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ClientClaims" (
    "Id" serial NOT NULL,
    "Type" varchar(250) NOT NULL,
    "Value" varchar(250) NOT NULL,
    "ClientId" integer NOT NULL,
    CONSTRAINT "PK_ClientClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ClientClaims_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ClientCorsOrigins" (
    "Id" serial NOT NULL,
    "Origin" varchar(150) NOT NULL,
    "ClientId" integer NOT NULL,
    CONSTRAINT "PK_ClientCorsOrigins" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ClientCorsOrigins_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ClientGrantTypes" (
    "Id" serial NOT NULL,
    "GrantType" varchar(250) NOT NULL,
    "ClientId" integer NOT NULL,
    CONSTRAINT "PK_ClientGrantTypes" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ClientGrantTypes_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ClientIdPRestrictions" (
    "Id" serial NOT NULL,
    "Provider" varchar(200) NOT NULL,
    "ClientId" integer NOT NULL,
    CONSTRAINT "PK_ClientIdPRestrictions" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ClientIdPRestrictions_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ClientPostLogoutRedirectUris" (
    "Id" serial NOT NULL,
    "PostLogoutRedirectUri" varchar(2000) NOT NULL,
    "ClientId" integer NOT NULL,
    CONSTRAINT "PK_ClientPostLogoutRedirectUris" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ClientPostLogoutRedirectUris_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ClientProperties" (
    "Id" serial NOT NULL,
    "Key" varchar(250) NOT NULL,
    "Value" varchar(2000) NOT NULL,
    "ClientId" integer NOT NULL,
    CONSTRAINT "PK_ClientProperties" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ClientProperties_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ClientRedirectUris" (
    "Id" serial NOT NULL,
    "RedirectUri" varchar(2000) NOT NULL,
    "ClientId" integer NOT NULL,
    CONSTRAINT "PK_ClientRedirectUris" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ClientRedirectUris_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ClientScopes" (
    "Id" serial NOT NULL,
    "Scope" varchar(200) NOT NULL,
    "ClientId" integer NOT NULL,
    CONSTRAINT "PK_ClientScopes" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ClientScopes_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ClientSecrets" (
    "Expiration" timestamp without time zone NULL,
    "Id" serial NOT NULL,
    "Description" varchar(2000) NULL,
    "Value" varchar(2000) NOT NULL,
    "Type" varchar(250) NULL,
    "ClientId" integer NOT NULL,
    CONSTRAINT "PK_ClientSecrets" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ClientSecrets_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id") ON DELETE CASCADE
);

CREATE TABLE "IdentityClaims" (
    "Id" serial NOT NULL,
    "Type" varchar(200) NOT NULL,
    "IdentityResourceId" integer NOT NULL,
    CONSTRAINT "PK_IdentityClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_IdentityClaims_IdentityResources_IdentityResourceId" FOREIGN KEY ("IdentityResourceId") REFERENCES "IdentityResources" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ApiScopeClaims" (
    "Id" serial NOT NULL,
    "Type" varchar(200) NOT NULL,
    "ApiScopeId" integer NOT NULL,
    CONSTRAINT "PK_ApiScopeClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ApiScopeClaims_ApiScopes_ApiScopeId" FOREIGN KEY ("ApiScopeId") REFERENCES "ApiScopes" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_ApiClaims_ApiResourceId" ON "ApiClaims" ("ApiResourceId");

CREATE UNIQUE INDEX "IX_ApiResources_Name" ON "ApiResources" ("Name");

CREATE INDEX "IX_ApiScopeClaims_ApiScopeId" ON "ApiScopeClaims" ("ApiScopeId");

CREATE INDEX "IX_ApiScopes_ApiResourceId" ON "ApiScopes" ("ApiResourceId");

CREATE UNIQUE INDEX "IX_ApiScopes_Name" ON "ApiScopes" ("Name");

CREATE INDEX "IX_ApiSecrets_ApiResourceId" ON "ApiSecrets" ("ApiResourceId");

CREATE INDEX "IX_ClientClaims_ClientId" ON "ClientClaims" ("ClientId");

CREATE INDEX "IX_ClientCorsOrigins_ClientId" ON "ClientCorsOrigins" ("ClientId");

CREATE INDEX "IX_ClientGrantTypes_ClientId" ON "ClientGrantTypes" ("ClientId");

CREATE INDEX "IX_ClientIdPRestrictions_ClientId" ON "ClientIdPRestrictions" ("ClientId");

CREATE INDEX "IX_ClientPostLogoutRedirectUris_ClientId" ON "ClientPostLogoutRedirectUris" ("ClientId");

CREATE INDEX "IX_ClientProperties_ClientId" ON "ClientProperties" ("ClientId");

CREATE INDEX "IX_ClientRedirectUris_ClientId" ON "ClientRedirectUris" ("ClientId");

CREATE UNIQUE INDEX "IX_Clients_ClientId" ON "Clients" ("ClientId");

CREATE INDEX "IX_ClientScopes_ClientId" ON "ClientScopes" ("ClientId");

CREATE INDEX "IX_ClientSecrets_ClientId" ON "ClientSecrets" ("ClientId");

CREATE INDEX "IX_IdentityClaims_IdentityResourceId" ON "IdentityClaims" ("IdentityResourceId");

CREATE UNIQUE INDEX "IX_IdentityResources_Name" ON "IdentityResources" ("Name");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20180807210700_Config', '2.1.1-rtm-30846');

