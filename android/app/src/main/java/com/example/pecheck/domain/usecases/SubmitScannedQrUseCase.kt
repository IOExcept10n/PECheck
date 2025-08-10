package com.example.pecheck.domain.usecases

import com.example.pecheck.domain.repository.Repository

class SubmitScannedQrUseCase(
    private val repository: Repository
) {
    suspend operator fun invoke(qrContent: String) {
        repository.submitScannedQr(qrContent)
    }
} 