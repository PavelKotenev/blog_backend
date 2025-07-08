package handlers

import "blog/internal/repo"

type UserHandler struct {
	UserRepo *repo.UserRepo
}

type PostHandler struct {
	PostRepo *repo.PostRepo
}

func NewUserHandler(
	userRepo *repo.UserRepo,
) *UserHandler {
	return &UserHandler{
		userRepo,
	}
}

func NewPostHandler(repo *repo.PostRepo) *PostHandler {
	return &PostHandler{repo}
}
