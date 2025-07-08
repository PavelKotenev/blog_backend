package handlers

import (
	"blog/internal/utils"
	"github.com/gin-gonic/gin"
	"net/http"
)

func (h *UserHandler) Logout() gin.HandlerFunc {
	return func(c *gin.Context) {
		sessionToken, err := c.Cookie("OPAQUE")
		if err != nil {
			c.Status(http.StatusNoContent)
			return
		}

		if err := utils.DeleteSessionFromRedis(c, sessionToken); err != nil {
			c.JSON(http.StatusInternalServerError, gin.H{"error": "Failed to delete session"})
			return
		}

		c.SetCookie(
			"OPAQUE",
			"",
			-1,
			"/",
			"",
			false,
			true,
		)

		c.Status(http.StatusNoContent)
	}
}
