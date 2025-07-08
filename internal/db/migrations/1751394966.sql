/**********************************************************
 *
 *  Date          : 2025-07-05
 *  Order         : 1
 *  Filename      : 1751394966
 *  Description   : Create post table
 *
 **********************************************************/

CREATE
EXTENSION IF NOT EXISTS "uuid-ossp";
CREATE
EXTENSION IF NOT EXISTS "pgcrypto";

CREATE TABLE users
(
    uuid          UUID PRIMARY KEY             DEFAULT gen_random_uuid(),
    name          VARCHAR(70),
    email         VARCHAR(255) UNIQUE NOT NULL,
    password_hash VARCHAR(1000)       NOT NULL,
    role          SMALLINT            NOT NULL,
    status        SMALLINT            NOT NULL,
    created_at    BIGINT              NOT NULL DEFAULT (EXTRACT(EPOCH FROM NOW()) * 1000)::BIGINT,
    updated_at    BIGINT              NOT NULL DEFAULT (EXTRACT(EPOCH FROM NOW()) * 1000)::BIGINT,
    deleted_at    BIGINT              NOT NULL DEFAULT 0
);

CREATE TABLE posts
(
    uuid       UUID PRIMARY KEY      DEFAULT gen_random_uuid(),
    title      VARCHAR(255) NOT NULL,
    status     SMALLINT     NOT NULL,
    created_at BIGINT       NOT NULL DEFAULT (EXTRACT(EPOCH FROM NOW()) * 1000)::BIGINT,
    updated_at BIGINT       NOT NULL DEFAULT (EXTRACT(EPOCH FROM NOW()) * 1000)::BIGINT,
    deleted_at BIGINT       NOT NULL
);
