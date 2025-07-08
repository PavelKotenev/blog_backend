package main

import (
	"blog/internal/db"
	"blog/internal/handlers"
	"blog/internal/repo"
	"blog/internal/routes"
	"context"
	"github.com/gin-gonic/gin"
	"log"
)

func main() {
	userRepo := repo.NewUserRepo()

	userHandler := handlers.NewUserHandler(userRepo)

	if err := db.OpenPostgresConnection(context.Background()); err != nil {
		log.Fatalf("failed to connect to postgres: %v", err)
	}
	ctx := context.Background()

	db.OpenRedisConnection(ctx)
	if err := db.RedisClient.Ping(ctx).Err(); err != nil {
		log.Fatalf("failed to connect to redis: %v", err)
	}

	defer db.ClosePostgresConnection()

	r := gin.Default()
	routes.SetupRoutes(
		r,
		userHandler,
	)

	if err := r.Run(":8080"); err != nil {
		log.Fatalf("Server starting error: %v", err)
	}
}
