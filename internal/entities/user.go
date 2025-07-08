package entities

import "github.com/google/uuid"

type UserEntity struct {
	Uuid         uuid.UUID
	Name         string
	Email        string
	PasswordHash string
	Role         int8
	Status       int8
	CreatedAt    int64
	UpdatedAt    int64
	DeletedAt    int64
}
