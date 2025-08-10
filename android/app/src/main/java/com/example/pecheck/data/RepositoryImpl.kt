package com.example.pecheck.data

import android.graphics.Bitmap
import com.example.pecheck.domain.model.UserRole
import com.example.pecheck.domain.repository.Repository

object RepositoryImpl: Repository {
    override suspend fun login(username: String, password: String): UserRole {
        TODO("Not yet implemented")
    }

    override suspend fun getStudentQrCodeImage(): Bitmap {
        TODO("Not yet implemented")
    }

    override suspend fun submitScannedQr(qrContent: String) {
        TODO("Not yet implemented")
    }

    override fun sendQrCodeData(qrData: String) {
        TODO("Not yet implemented")
    }

    override fun generateQrCodeFromString(qrData: String): Bitmap {
        TODO("Not yet implemented")
    }
}