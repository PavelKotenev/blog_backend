package repo

import (
	"blog/internal/db"
	"blog/internal/entities"
	"context"
	"github.com/google/uuid"
)

type IUserRepo interface {
	Create(ctx context.Context, user entities.UserEntity) (uuid.UUID, error)
	GetByEmail(ctx context.Context, email string) (entities.UserEntity, error)
}
type UserRepo struct{}

func NewUserRepo() *UserRepo {
	return &UserRepo{}
}

func (ur *UserRepo) Create(
	ctx context.Context,
	userEntity entities.UserEntity,
) (uuid.UUID, error) {
	var userUuid uuid.UUID

	query := `
		INSERT INTO users (
		                   email,
		                   password_hash, 
		                   role,
		                   status
		                   )
		VALUES ($1, $2, $3, $4)
		RETURNING uuid
	`

	err := db.Pool.QueryRow(ctx, query,
		userEntity.Email,
		userEntity.PasswordHash,
		userEntity.Role,
		userEntity.Status,
	).Scan(&userUuid)

	if err != nil {
		return uuid.Nil, err
	}
	return userUuid, nil
}

func (ur *UserRepo) GetByEmail(ctx context.Context, email string) (*entities.UserEntity, error) {
	var user entities.UserEntity

	query := `
		SELECT uuid, email, password_hash, role, status
		FROM users
		WHERE email = $1
		LIMIT 1
	`

	err := db.Pool.QueryRow(ctx, query, email).Scan(
		&user.Uuid,
		&user.Email,
		&user.PasswordHash,
		&user.Role,
		&user.Status,
	)

	if err != nil {
		return nil, err
	}

	return &user, nil
}
