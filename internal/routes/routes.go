package routes

import (
	"blog/internal/handlers"
	"blog/internal/middleware"
	"github.com/gin-gonic/gin"
)

func SetupRoutes(
	r *gin.Engine,
	userHandler *handlers.UserHandler,
) {
	userGroup := r.Group("/api/user/")
	userGroup.POST("register", userHandler.Register())
	userGroup.POST("login", userHandler.Login())
	userGroup.POST("logout", userHandler.Logout())

	postGroup := r.Group("/api/post/")
	postGroup.GET("list")

	technicalGroup := r.Group("/technical").Use(middleware.AuthRequired())
	technicalGroup.GET("/ping", func(c *gin.Context) {
		c.String(200, "pong")
	})
}
