package com.example.pecheck.presentation

import androidx.lifecycle.ViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow

class TeacherViewModel : ViewModel() {
    
    private val _scannedStudent = MutableStateFlow<String?>(null)
    val scannedStudent: StateFlow<String?> = _scannedStudent.asStateFlow()
    
    private val _presentCount = MutableStateFlow(0)
    val presentCount: StateFlow<Int> = _presentCount.asStateFlow()
    
    private val _absentCount = MutableStateFlow(0)
    val absentCount: StateFlow<Int> = _absentCount.asStateFlow()
    
    private val _error = MutableStateFlow<String?>(null)
    val error: StateFlow<String?> = _error.asStateFlow()

    fun onQrScanned(qrCode: String) {
        // Здесь должна быть логика получения информации о студенте по QR-коду
        // Пока используем демо-данные
        val studentInfo = when (qrCode) {
            "S12345" -> "Иван Петров - Группа ИС-21, Курс 2"
            "S67890" -> "Мария Сидорова - Группа ИС-21, Курс 2"
            "S11111" -> "Алексей Козлов - Группа ИС-22, Курс 1"
            else -> "Неизвестный студент (ID: $qrCode)"
        }
        
        _scannedStudent.value = studentInfo
    }

    fun markStudentPresent() {
        _presentCount.value += 1
        _scannedStudent.value = null
    }

    fun markStudentAbsent() {
        _absentCount.value += 1
        _scannedStudent.value = null
    }

    fun clearError() {
        _error.value = null
    }
} 