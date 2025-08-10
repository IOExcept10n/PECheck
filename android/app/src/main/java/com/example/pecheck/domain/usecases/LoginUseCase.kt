package com.example.pecheck.domain.usecases

import com.example.pecheck.domain.model.UserRole
import com.example.pecheck.domain.repository.Repository

class LoginUseCase(
    private val repository: Repository
) {
    suspend operator fun invoke(username: String, password: String): UserRole {
        return repository.login(username, password)
    }
}