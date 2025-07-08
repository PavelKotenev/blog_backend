package entities

import "github.com/google/uuid"

type PostEntity struct {
	Uuid      uuid.UUID
	Title     string
	CreatedAt int64
	UpdatedAt int64
	DeletedAt int64
}
