package com.example.pecheck.domain.repository

import android.graphics.Bitmap
import com.example.pecheck.domain.model.ScheduleItem
import com.example.pecheck.domain.model.UserRole

interface Repository {
    suspend fun login(username: String, password: String): UserRole
    suspend fun getStudentQrCodeImage(): Bitmap
    suspend fun submitScannedQr(qrContent: String)
    suspend fun getSchedule(): List<ScheduleItem>

    fun sendQrCodeData(qrData: String)
    fun generateQrCodeFromString(qrData: String): Bitmap
}