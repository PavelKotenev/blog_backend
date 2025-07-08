package utils

import (
	"blog/internal/db"
	"context"
	"encoding/json"
	"fmt"
	"github.com/gin-gonic/gin"
	"github.com/google/uuid"
	"time"
)

func СreateSessionAndSetCookie(c *gin.Context, userUuid uuid.UUID) error {
	sessionToken := uuid.NewString()
	sessionData := map[string]string{
		"user_uuid":  userUuid.String(),
		"ip":         c.ClientIP(),
		"user_agent": c.Request.UserAgent(),
	}

	hashedToken := HashSha256(sessionToken)
	sessionJson, err := json.Marshal(sessionData)
	if err != nil {
		return err
	}

	err = db.RedisClient.Set(
		c.Request.Context(),
		"opaque:"+hashedToken,
		sessionJson,
		time.Hour*24*30,
	).Err()

	if err != nil {
		return err
	}

	c.SetCookie(
		"OPAQUE",
		sessionToken,
		2592000,
		"/",
		"",
		false,
		true,
	)

	return nil
}

func DeleteSessionFromRedis(ctx context.Context, token string) error {
	hashed := HashSha256(token)
	key := fmt.Sprintf("opaque:%s", hashed)
	return db.RedisClient.Del(ctx, key).Err()
}
