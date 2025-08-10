package com.example.pecheck.presentation

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.pecheck.data.RepositoryImpl
import com.example.pecheck.domain.usecases.SubmitScannedQrUseCase
import kotlinx.coroutines.launch

class TeacherViewModel: ViewModel() {
    private val repository = RepositoryImpl
    private val submitScannedQrUseCase = SubmitScannedQrUseCase(repository)

    fun onQrScanned(qrContent: String) {
        viewModelScope.launch {
            submitScannedQrUseCase(qrContent)
        }
    }
} 