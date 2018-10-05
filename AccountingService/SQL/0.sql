CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" varchar(150) NOT NULL,
    "ProductVersion" varchar(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

CREATE TABLE account_groups (
    id uuid NOT NULL,
    name text NOT NULL,
    CONSTRAINT "PK_account_groups" PRIMARY KEY (id)
);

CREATE TABLE account_types (
    id uuid NOT NULL,
    name text NOT NULL,
    group_id uuid NOT NULL,
    CONSTRAINT "PK_account_types" PRIMARY KEY (id)
);

CREATE TABLE organizations (
    id uuid NOT NULL,
    name text NOT NULL,
    CONSTRAINT "PK_organizations" PRIMARY KEY (id)
);

CREATE TABLE accounts (
    id uuid NOT NULL,
    name text NOT NULL,
    description text NULL,
    account_type_id uuid NOT NULL,
    group_id uuid NOT NULL,
    organization_id uuid NOT NULL,
    CONSTRAINT "PK_accounts" PRIMARY KEY (id),
    CONSTRAINT "FK_accounts_account_types_account_type_id" FOREIGN KEY (account_type_id) REFERENCES account_types (id) ON DELETE CASCADE,
    CONSTRAINT "FK_accounts_organizations_organization_id" FOREIGN KEY (organization_id) REFERENCES organizations (id) ON DELETE CASCADE
);

CREATE TABLE users (
    id uuid NOT NULL,
    name text NOT NULL,
    email text NOT NULL,
    password_hash text NOT NULL,
    organization_id uuid NULL,
    CONSTRAINT "PK_users" PRIMARY KEY (id),
    CONSTRAINT "FK_users_organizations_organization_id" FOREIGN KEY (organization_id) REFERENCES organizations (id) ON DELETE RESTRICT
);

CREATE TABLE transactions (
    id uuid NOT NULL,
    transaction_type integer NOT NULL,
    account_id uuid NULL,
    amount double precision NOT NULL,
    description text NULL,
    transaction_date timestamp without time zone NULL,
    organization_id uuid NOT NULL,
    note text NULL,
    "Discriminator" text NOT NULL,
    CONSTRAINT "PK_transactions" PRIMARY KEY (id),
    CONSTRAINT "FK_transactions_accounts_account_id" FOREIGN KEY (account_id) REFERENCES accounts (id) ON DELETE RESTRICT
);

CREATE TABLE transaction_items (
    id uuid NOT NULL,
    transaction_id uuid NOT NULL,
    type text NOT NULL,
    amount double precision NOT NULL,
    account_id uuid NOT NULL,
    organization_id uuid NOT NULL,
    "Discriminator" text NOT NULL,
    CONSTRAINT "PK_transaction_items" PRIMARY KEY (id),
    CONSTRAINT "FK_transaction_items_transactions_transaction_id" FOREIGN KEY (transaction_id) REFERENCES transactions (id) ON DELETE CASCADE,
    CONSTRAINT "FK_transaction_items_transactions_transaction_id1" FOREIGN KEY (transaction_id) REFERENCES transactions (id) ON DELETE CASCADE,
    CONSTRAINT "FK_transaction_items_accounts_account_id" FOREIGN KEY (account_id) REFERENCES accounts (id) ON DELETE CASCADE
);

INSERT INTO account_groups (id, name)
VALUES ('06e3346c-7430-48e5-bec5-5049c3fc88a4', 'Assets');
INSERT INTO account_groups (id, name)
VALUES ('15d7775b-e017-4437-938f-4f264abd3d8a', 'Liabilities');
INSERT INTO account_groups (id, name)
VALUES ('8b27c505-29c3-4a90-a6cf-5b3abc30cfeb', 'Equity');

INSERT INTO account_types (id, group_id, name)
VALUES ('b9ec0e7c-c17d-4edf-8f1d-7b8cdbcc39f4', '06e3346c-7430-48e5-bec5-5049c3fc88a4', 'Cash/Bank');
INSERT INTO account_types (id, group_id, name)
VALUES ('b4af6ec9-bbf8-46d6-8d46-e48fb614f5ce', '06e3346c-7430-48e5-bec5-5049c3fc88a4', 'Money in Transit');
INSERT INTO account_types (id, group_id, name)
VALUES ('9a12615f-f3dd-4edd-9af6-1171c998dd25', '06e3346c-7430-48e5-bec5-5049c3fc88a4', 'Payments from Sales');
INSERT INTO account_types (id, group_id, name)
VALUES ('1dcd2296-2c2f-4a28-9aba-93f148bc6a66', '15d7775b-e017-4437-938f-4f264abd3d8a', 'Credit Card');
INSERT INTO account_types (id, group_id, name)
VALUES ('4a2da79d-2ef7-47c8-b3c8-c43df9605fe2', '15d7775b-e017-4437-938f-4f264abd3d8a', 'Loan and Line of Credit');
INSERT INTO account_types (id, group_id, name)
VALUES ('129029e9-4505-4064-a230-715bae91f187', '15d7775b-e017-4437-938f-4f264abd3d8a', 'Taxes');
INSERT INTO account_types (id, group_id, name)
VALUES ('39431e66-cde1-4018-929d-ef4c3e56b907', '8b27c505-29c3-4a90-a6cf-5b3abc30cfeb', 'Own Contribution');
INSERT INTO account_types (id, group_id, name)
VALUES ('20e1fc3b-9ee8-43c4-8a71-6d39726e98bf', '8b27c505-29c3-4a90-a6cf-5b3abc30cfeb', 'Drawing');

INSERT INTO organizations (id, name)
VALUES ('a91917a6-3b46-4e58-926e-de9fe8fe497b', 'Organization 1');
INSERT INTO organizations (id, name)
VALUES ('d61ffa8d-171e-4fcd-a1ca-2248c34687fe', 'Organization 2');
INSERT INTO organizations (id, name)
VALUES ('b3520fc8-aa69-40a9-9008-5b3aa824ae36', 'Organization 3');

INSERT INTO accounts (id, account_type_id, description, group_id, name, organization_id)
VALUES ('da8a6668-786b-4eb7-8579-b20281a11f4b', 'b9ec0e7c-c17d-4edf-8f1d-7b8cdbcc39f4', NULL, '06e3346c-7430-48e5-bec5-5049c3fc88a4', 'Cash at Hand', 'a91917a6-3b46-4e58-926e-de9fe8fe497b');

INSERT INTO users (id, email, name, organization_id, password_hash)
VALUES ('eeff65d8-6ee3-4acc-bbcb-35abf13982fe', 'user1@Organization1.com', 'User 1', 'a91917a6-3b46-4e58-926e-de9fe8fe497b', 'AQAAAAEAACcQAAAAEMsaalGFShjtzERSdhrP8IF+VuQ4JRpJYnZodsPLW281BeIVwonqFxKojgb7fuBNnw==');
INSERT INTO users (id, email, name, organization_id, password_hash)
VALUES ('9ee8f165-4582-48ae-b2b0-971c36061385', 'user2@Organization2.com', 'User 2', 'd61ffa8d-171e-4fcd-a1ca-2248c34687fe', 'AQAAAAEAACcQAAAAEM3aaLzrIRQCXO27CrSfeVdYmWhGvHZ4dIz5r37y9Av6AiXaxuQCX8CTgaqkESFHDQ==');
INSERT INTO users (id, email, name, organization_id, password_hash)
VALUES ('7fb41a45-55ed-4920-bf66-4fe6c050a528', 'user3@Organization3.com', 'User 3', 'b3520fc8-aa69-40a9-9008-5b3aa824ae36', 'AQAAAAEAACcQAAAAEMjnyh00ppL0PlFKNbTJ0ZIW30OONT8ExIp6DyomCPqYe3OFcFDRkmM1yVAesmElzQ==');

CREATE INDEX "IX_accounts_account_type_id" ON accounts (account_type_id);

CREATE INDEX "IX_accounts_organization_id" ON accounts (organization_id);

CREATE INDEX "IX_transaction_items_transaction_id" ON transaction_items (transaction_id);

CREATE INDEX "IX_transaction_items_transaction_id1" ON transaction_items (transaction_id);

CREATE INDEX "IX_transaction_items_account_id" ON transaction_items (account_id);

CREATE INDEX "IX_transactions_account_id" ON transactions (account_id);

CREATE INDEX "IX_users_organization_id" ON users (organization_id);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20181005130439_InitialCreate', '2.1.2-rtm-30932');