package main

import (
	"blog/cmd"
	"log"
	"os"
)

func main() {
	args := os.Args

	command := args[1]
	commandArgs := args[2:]

	switch command {
	case "migrations:runOne":
		cmd.RunOneMigration(commandArgs)
	default:
		log.Fatalf("Unknown command: %s", command)
	}
}
