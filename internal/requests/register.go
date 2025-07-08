package requests

type UserRegisterRequest struct {
	Email    string `json:"email"`
	Password string `json:"password"`
}
