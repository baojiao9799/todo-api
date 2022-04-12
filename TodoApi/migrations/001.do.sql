CREATE TABLE IF NOT EXISTS users (
    id UUID PRIMARY KEY,
    username VARCHAR(20) UNIQUE NOT NULL,
    password VARCHAR(20) NOT NULL
);

CREATE TYPE completion_status AS ENUM ('open', 'in_progress', 'complete');

CREATE TABLE IF NOT EXISTS todos (
    id UUID PRIMARY KEY,
    user_id UUID REFERENCES users (id), 
    title VARCHAR(50) NOT NULL,
    status completion_status,
    creation_date TIMESTAMP NOT NULL,
    due_date TIMESTAMP
);

CREATE TABLE IF NOT EXISTS tokens (
    id UUID PRIMARY KEY,
    user_id UUID REFERENCES users (id),
    token text
);