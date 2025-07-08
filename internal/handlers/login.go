package handlers

import (
	"blog/internal/requests"
	"blog/internal/utils"
	"github.com/gin-gonic/gin"
	"net/http"
)

func (h *UserHandler) Login() gin.HandlerFunc {
	return func(c *gin.Context) {

		var body requests.UserLoginRequest
		if bodyParseErr := c.ShouldBindJSON(&body); bodyParseErr != nil {
			c.JSON(http.StatusBadRequest, gin.H{"error": bodyParseErr.Error()})
			return
		}

		user, errUserRepo := h.UserRepo.GetByEmail(c.Request.Context(), body.Email)
		isPasswordValid, _ := utils.VerifyPassword(body.Password, user.PasswordHash)

		if errUserRepo != nil || user == nil || !isPasswordValid {
			c.JSON(http.StatusUnauthorized, gin.H{"error": "Invalid email or password"})
			return
		}

		if err := utils.СreateSessionAndSetCookie(c, user.Uuid); err != nil {
			c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
			return
		}
		c.JSON(http.StatusCreated, gin.H{"message": "Login successful"})
	}
}
