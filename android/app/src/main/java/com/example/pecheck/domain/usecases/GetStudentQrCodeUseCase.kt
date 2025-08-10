package com.example.pecheck.domain.usecases

import android.graphics.Bitmap
import com.example.pecheck.domain.repository.Repository

class GetStudentQrCodeUseCase(
    private val repository: Repository
) {
    suspend operator fun invoke(): Bitmap {
        return repository.getStudentQrCodeImage()
    }
} 