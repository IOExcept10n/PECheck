package com.example.pecheck.domain.usecases

import android.graphics.Bitmap
import com.example.pecheck.domain.repository.Repository

class GenerateQrCodeFromStringUseCase(
    private val repository: Repository
) {
    fun generateQrCodeFromString(qrData: String): Bitmap {
        return repository.generateQrCodeFromString(qrData)
    }
}