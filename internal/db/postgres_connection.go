package db

import (
	"context"
	"github.com/jackc/pgx/v5/pgxpool"
	"github.com/spf13/viper"
)

var Pool *pgxpool.Pool

func OpenPostgresConnection(ctx context.Context) error {
	v := viper.New()
	v.SetConfigName("postgres")
	v.SetConfigType("yaml")
	v.AddConfigPath("./conf")

	if err := v.ReadInConfig(); err != nil {
		return err
	}
	
	config, err := pgxpool.ParseConfig(v.GetString("dsn"))
	if err != nil {
		return err
	}

	config.MaxConns = v.GetInt32("max_connections")
	config.MaxConnLifetime = v.GetDuration("max_conn_lifetime")
	config.MaxConnIdleTime = v.GetDuration("max_conn_idle_time")

	Pool, err = pgxpool.NewWithConfig(ctx, config)
	if err != nil {
		return err
	}

	if err := Pool.Ping(ctx); err != nil {
		return err
	}
	return nil
}

func ClosePostgresConnection() {
	if Pool != nil {
		Pool.Close()
	}
}
