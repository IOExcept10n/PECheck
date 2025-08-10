package com.example.pecheck.domain.model

data class ScheduleItem(
    val id: String,
    val title: String,
    val dateTime: String,
    val status: Status
) {
    enum class Status { ATTENDED, CANCELED, PLANNED }
} 