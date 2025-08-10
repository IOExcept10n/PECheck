package com.example.pecheck.presentation

import androidx.lifecycle.ViewModel
import com.example.pecheck.data.RepositoryImpl
import com.example.pecheck.domain.usecases.GenerateQrCodeFromStringUseCase
import com.example.pecheck.domain.usecases.SendQrCodeDataUseCase

class MainViewModel: ViewModel() {
    private val repository = RepositoryImpl

    private val generateQrCodeFromStringUseCase = GenerateQrCodeFromStringUseCase(repository)
    private val sendQrCodeDataUseCase = SendQrCodeDataUseCase(repository)
}