package com.example.pecheck.data

import android.graphics.Bitmap
import android.graphics.BitmapFactory
import android.util.Base64
import com.example.pecheck.data.remote.RetrofitProvider
import com.example.pecheck.data.remote.dto.LoginRequestDto
import com.example.pecheck.domain.model.ScheduleItem
import com.example.pecheck.domain.model.UserRole
import com.example.pecheck.domain.repository.Repository

object RepositoryImpl: Repository {
    @Volatile private var authToken: String? = null

    override suspend fun login(username: String, password: String): UserRole {
        val response = RetrofitProvider.api.login(LoginRequestDto(username, password))
        authToken = response.token
        return when (response.role.lowercase()) {
            "student" -> UserRole.STUDENT
            "teacher" -> UserRole.TEACHER
            else -> UserRole.STUDENT
        }
    }

    override suspend fun getStudentQrCodeImage(): Bitmap {
        val token = requireNotNull(authToken) { "Not authenticated" }
        val response = RetrofitProvider.api.getStudentQr("Bearer $token")
        val base64 = response.imageBase64
        if (base64 != null) {
            val bytes = Base64.decode(base64, Base64.DEFAULT)
            return BitmapFactory.decodeByteArray(bytes, 0, bytes.size)
        }
        throw NotImplementedError("QR generation from content is not implemented yet")
    }

    override suspend fun submitScannedQr(qrContent: String) {
        val token = requireNotNull(authToken) { "Not authenticated" }
        RetrofitProvider.api.submitScannedQr("Bearer $token", mapOf("qr" to qrContent))
    }

    override suspend fun getSchedule(): List<ScheduleItem> {
        val token = requireNotNull(authToken) { "Not authenticated" }
        val dtos = RetrofitProvider.api.getSchedule("Bearer $token")
        return dtos.map { dto ->
            ScheduleItem(
                id = dto.id,
                title = dto.title,
                dateTime = dto.dateTime,
                status = when (dto.status.lowercase()) {
                    "attended" -> ScheduleItem.Status.ATTENDED
                    "canceled" -> ScheduleItem.Status.CANCELED
                    else -> ScheduleItem.Status.PLANNED
                }
            )
        }
    }

    override fun sendQrCodeData(qrData: String) {
        throw NotImplementedError("sendQrCodeData not implemented")
    }

    override fun generateQrCodeFromString(qrData: String): Bitmap {
        throw NotImplementedError("generateQrCodeFromString not implemented")
    }
}