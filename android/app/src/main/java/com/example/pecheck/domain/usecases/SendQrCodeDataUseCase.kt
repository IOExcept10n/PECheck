package com.example.pecheck.domain.usecases

import com.example.pecheck.domain.repository.Repository

class SendQrCodeDataUseCase(
    private val repository: Repository
){
    fun sendQrCodeDataUseCase(qrData: String){
        repository.sendQrCodeData(qrData)
    }
}