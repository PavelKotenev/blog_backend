package utils

import (
	"crypto/rand"
	"encoding/base64"
)

var jwtSecret = []byte("your-secret-key")

func GenerateRandomToken(n int) (string, error) {
	b := make([]byte, n)
	_, err := rand.Read(b)
	if err != nil {
		return "", err
	}
	return base64.RawURLEncoding.EncodeToString(b), nil
}
