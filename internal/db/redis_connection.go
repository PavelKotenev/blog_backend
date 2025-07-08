package db

import (
	"context"
	"github.com/redis/go-redis/v9"
	"github.com/spf13/viper"
)

var RedisClient *redis.Client

func OpenRedisConnection(ctx context.Context) {
	v := viper.New()
	v.SetConfigName("redis")
	v.SetConfigType("yaml")
	v.AddConfigPath("./conf")

	if err := v.ReadInConfig(); err != nil {
		panic(err)
	}

	RedisClient = redis.NewClient(&redis.Options{
		Addr:            v.GetString("addr"),
		Password:        v.GetString("password"),
		DB:              v.GetInt("db"),
		Protocol:        3,
		DisableIdentity: true,
	})
}
