package utils

import (
	"crypto/sha256"
	"encoding/hex"
	"github.com/alexedwards/argon2id"
)

func HashArgon2(password string) (string, error) {
	return argon2id.CreateHash(password, argon2id.DefaultParams)
}

func HashSha256(token string) string {
	h := sha256.Sum256([]byte(token))
	return hex.EncodeToString(h[:])
}
