package com.example.pecheck.presentation

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.pecheck.data.RepositoryImpl
import com.example.pecheck.domain.model.UserRole
import com.example.pecheck.domain.usecases.LoginUseCase
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch

class LogInViewModel: ViewModel() {
    private val repository = RepositoryImpl
    private val loginUseCase = LoginUseCase(repository)

    private val _role = MutableStateFlow<UserRole?>(null)
    val role: StateFlow<UserRole?> = _role

    fun login(username: String, password: String) {
        viewModelScope.launch {
            val result = loginUseCase(username, password)
            _role.value = result
        }
    }
} 