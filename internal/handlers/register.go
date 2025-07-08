package handlers

import (
	"blog/internal/entities"
	"blog/internal/requests"
	"blog/internal/utils"
	"github.com/gin-gonic/gin"
	"net/http"
)

func (h *UserHandler) Register() gin.HandlerFunc {
	return func(c *gin.Context) {
		var body requests.UserRegisterRequest
		if err := c.ShouldBindJSON(&body); err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
			return
		}

		hash, _ := utils.HashArgon2(body.Password)

		userEntity := entities.UserEntity{
			Email:        body.Email,
			PasswordHash: hash,
			Role:         1,
			Status:       1,
		}

		userUuid, createUserErr := h.UserRepo.Create(c.Request.Context(), userEntity)
		if createUserErr != nil {
			c.JSON(http.StatusInternalServerError, gin.H{"error": "Failed to create user"})
			return
		}

		if err := utils.СreateSessionAndSetCookie(c, userUuid); err != nil {
			c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
			return
		}

		c.JSON(http.StatusCreated, gin.H{"message": "User registered successfully"})
	}
}
