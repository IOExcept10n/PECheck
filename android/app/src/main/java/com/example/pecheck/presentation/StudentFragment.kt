package com.example.pecheck.presentation

import android.graphics.Bitmap
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.fragment.app.viewModels
import androidx.lifecycle.lifecycleScope
import androidx.recyclerview.widget.LinearLayoutManager
import com.example.pecheck.databinding.FragmentStudentBinding
import com.google.zxing.BarcodeFormat
import com.google.zxing.EncodeHintType
import com.google.zxing.qrcode.QRCodeWriter
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.flow.collectLatest
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext

class StudentFragment : Fragment() {

    private var _binding: FragmentStudentBinding? = null
    private val binding get() = _binding!!
    
    private val viewModel: StudentViewModel by viewModels()
    private lateinit var adapter: ScheduleAdapter

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentStudentBinding.inflate(inflater, container, false)
        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        
        setupRecyclerView()
        setupUI()
        observeViewModel()
        
        // Загружаем данные
        viewModel.loadQr()
        viewModel.loadSchedule()
    }

    private fun setupRecyclerView() {
        adapter = ScheduleAdapter(emptyList())
        binding.rvSchedule.layoutManager = LinearLayoutManager(requireContext())
        binding.rvSchedule.adapter = adapter
    }

    private fun setupUI() {
        binding.btnRefresh.setOnClickListener {
            viewModel.loadQr()
        }
        
        // Устанавливаем имя студента (в реальном приложении из API)
        binding.tvStudentName.text = "Иван Петров"
    }

    private fun observeViewModel() {
        viewLifecycleOwner.lifecycleScope.launchWhenStarted {
            viewModel.qrImage.collectLatest { bitmap ->
                if (bitmap != null) {
                    binding.imageQr.setImageBitmap(bitmap)
                } else {
                    // Генерируем QR-код если нет из API
                    generateQRCode("S12345")
                }
            }
        }

        viewLifecycleOwner.lifecycleScope.launchWhenStarted {
            viewModel.schedule.collectLatest { items ->
                adapter.submitList(items)
                binding.tvNoSchedule.visibility = if (items.isEmpty()) View.VISIBLE else View.GONE
            }
        }

        viewLifecycleOwner.lifecycleScope.launchWhenStarted {
            viewModel.error.collectLatest { error ->
                if (!error.isNullOrBlank()) {
                    Toast.makeText(requireContext(), error, Toast.LENGTH_SHORT).show()
                }
            }
        }
    }

    private fun generateQRCode(content: String) {
        viewLifecycleOwner.lifecycleScope.launch(Dispatchers.IO) {
            try {
                val hints = hashMapOf<EncodeHintType, Any>().apply {
                    put(EncodeHintType.MARGIN, 1)
                }
                
                val qrCodeWriter = QRCodeWriter()
                val bitMatrix = qrCodeWriter.encode(content, BarcodeFormat.QR_CODE, 512, 512, hints)
                
                val width = bitMatrix.width
                val height = bitMatrix.height
                val bitmap = Bitmap.createBitmap(width, height, Bitmap.Config.RGB_565)
                
                for (x in 0 until width) {
                    for (y in 0 until height) {
                        bitmap.setPixel(x, y, if (bitMatrix[x, y]) 0xFF275886.toInt() else 0xFFFFFFFF.toInt())
                    }
                }
                
                withContext(Dispatchers.Main) {
                    binding.imageQr.setImageBitmap(bitmap)
                }
            } catch (e: Exception) {
                withContext(Dispatchers.Main) {
                    Toast.makeText(requireContext(), "Ошибка генерации QR-кода", Toast.LENGTH_SHORT).show()
                }
            }
        }
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
} 