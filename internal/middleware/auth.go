package middleware

import (
	"blog/internal/db"
	"blog/internal/utils"
	"encoding/json"
	"errors"
	"github.com/gin-gonic/gin"
	"github.com/redis/go-redis/v9"
	"net/http"
)

func AuthRequired() gin.HandlerFunc {
	return func(c *gin.Context) {

		opaqueToken, err := c.Cookie("OPAQUE")

		if err != nil {
			c.AbortWithStatus(http.StatusUnauthorized)
			return
		}

		hashedOpaqueToken := utils.HashSha256(opaqueToken)

		val, err := db.RedisClient.Get(c, "opaque:"+hashedOpaqueToken).Result()

		if errors.Is(err, redis.Nil) {
			c.AbortWithStatus(http.StatusUnauthorized)
			return
		} else if err != nil {
			c.AbortWithStatus(http.StatusInternalServerError)
			return
		}
		var session map[string]string
		_ = json.Unmarshal([]byte(val), &session)
		c.Set("user_uuid", session["user_uuid"])

		c.Next()
	}
}
