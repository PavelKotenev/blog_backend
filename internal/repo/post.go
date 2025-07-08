package repo

import (
	"blog/internal/entities"
	"context"
)

type IPostRepo interface {
	Create(ctx context.Context, user entities.PostEntity) error
}

type PostRepo struct{}

func NewPostRepo() *PostRepo {
	return &PostRepo{}
}
