package cmd

import (
	"blog/internal/db"
	"context"
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"path/filepath"
)

func RunOneMigration(args []string) {
	if len(args) == 0 {
		log.Fatal("Please provide a migration name")
	}

	num := args[0]
	fileName := num + ".sql"
	migrationDir := "./internal/db/migrations"
	path := filepath.Join(migrationDir, fileName)

	if _, err := os.Stat(path); os.IsNotExist(err) {
		log.Fatalf("Migration '%s' does not exist", fileName)
	}

	ctx := context.Background()

	if err := db.OpenPostgresConnection(ctx); err != nil {
		log.Fatalf("Error opening postgres connection: %v", err)
	}

	defer db.ClosePostgresConnection()

	fmt.Printf("Running migration %s\n", fileName)
	runFile(ctx, path)
}

func runFile(ctx context.Context, path string) {
	sqlBytes, err := ioutil.ReadFile(path)
	if err != nil {
		log.Fatalf("failed to read file %s: %v", path, err)
	}

	_, err = db.Pool.Exec(ctx, string(sqlBytes))
	if err != nil {
		log.Fatalf("failed to execute migration %s: %v", filepath.Base(path), err)
	}

	fmt.Printf("Migration %s applied successfully\n", filepath.Base(path))
}
