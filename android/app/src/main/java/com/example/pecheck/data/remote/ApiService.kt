package com.example.pecheck.data.remote

import com.example.pecheck.data.remote.dto.LoginRequestDto
import com.example.pecheck.data.remote.dto.LoginResponseDto
import com.example.pecheck.data.remote.dto.QrResponseDto
import com.example.pecheck.data.remote.dto.ScheduleItemDto
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.Header
import retrofit2.http.POST

interface ApiService {
    @POST("auth/login")
    suspend fun login(@Body request: LoginRequestDto): LoginResponseDto

    @GET("student/qr")
    suspend fun getStudentQr(@Header("Authorization") bearerToken: String): QrResponseDto

    @POST("teacher/submit")
    suspend fun submitScannedQr(
        @Header("Authorization") bearerToken: String,
        @Body qrContent: Map<String, String>
    )

    @GET("student/schedule")
    suspend fun getSchedule(@Header("Authorization") bearerToken: String): List<ScheduleItemDto>
} 