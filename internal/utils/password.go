package utils

import "github.com/alexedwards/argon2id"

func VerifyPassword(password, hash string) (bool, error) {
	return argon2id.ComparePasswordAndHash(password, hash)
}
