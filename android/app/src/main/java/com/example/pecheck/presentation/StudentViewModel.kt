package com.example.pecheck.presentation

import android.graphics.Bitmap
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.pecheck.data.RepositoryImpl
import com.example.pecheck.domain.usecases.GetStudentQrCodeUseCase
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch

class StudentViewModel: ViewModel() {
    private val repository = RepositoryImpl
    private val getStudentQrCodeUseCase = GetStudentQrCodeUseCase(repository)

    private val _qrImage = MutableStateFlow<Bitmap?>(null)
    val qrImage: StateFlow<Bitmap?> = _qrImage

    fun loadQr() {
        viewModelScope.launch {
            _qrImage.value = getStudentQrCodeUseCase()
        }
    }
} 