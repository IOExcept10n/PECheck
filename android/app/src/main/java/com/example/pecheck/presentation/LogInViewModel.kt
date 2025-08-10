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

    private val _loading = MutableStateFlow(false)
    val loading: StateFlow<Boolean> = _loading

    private val _error = MutableStateFlow<String?>(null)
    val error: StateFlow<String?> = _error

    fun login(username: String, password: String) {
        if (_loading.value) return
        _loading.value = true
        _error.value = null
        viewModelScope.launch {
            try {
                val result = loginUseCase(username, password)
                _role.value = result
            } catch (t: Throwable) {
                _error.value = t.message ?: "Login failed"
            } finally {
                _loading.value = false
            }
        }
    }
} 