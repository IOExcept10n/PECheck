package com.example.pecheck.domain.usecases

import com.example.pecheck.domain.model.ScheduleItem
import com.example.pecheck.domain.repository.Repository

class GetScheduleUseCase(
    private val repository: Repository
) {
    suspend operator fun invoke(): List<ScheduleItem> = repository.getSchedule()
} 