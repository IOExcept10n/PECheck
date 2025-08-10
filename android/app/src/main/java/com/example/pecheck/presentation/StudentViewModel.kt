package com.example.pecheck.presentation

import android.graphics.Bitmap
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.pecheck.data.RepositoryImpl
import com.example.pecheck.domain.model.ScheduleItem
import com.example.pecheck.domain.usecases.GetStudentQrCodeUseCase
import com.example.pecheck.domain.usecases.GetScheduleUseCase
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch

class StudentViewModel: ViewModel() {
    private val repository = RepositoryImpl
    private val getStudentQrCodeUseCase = GetStudentQrCodeUseCase(repository)
    private val getScheduleUseCase = GetScheduleUseCase(repository)

    private val _qrImage = MutableStateFlow<Bitmap?>(null)
    val qrImage: StateFlow<Bitmap?> = _qrImage

    private val _schedule = MutableStateFlow<List<ScheduleItem>>(emptyList())
    val schedule: StateFlow<List<ScheduleItem>> = _schedule

    private val _error = MutableStateFlow<String?>(null)
    val error: StateFlow<String?> = _error

    fun loadQr() {
        viewModelScope.launch {
            try {
                _qrImage.value = getStudentQrCodeUseCase()
            } catch (t: Throwable) {
                _error.value = t.message
            }
        }
    }

    fun loadSchedule() {
        viewModelScope.launch {
            try {
                _schedule.value = getScheduleUseCase()
            } catch (t: Throwable) {
                _error.value = t.message
            }
        }
    }
} 