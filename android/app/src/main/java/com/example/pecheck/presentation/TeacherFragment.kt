package com.example.pecheck.presentation

import android.Manifest
import android.content.pm.PackageManager
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.activity.result.contract.ActivityResultContracts
import androidx.camera.core.*
import androidx.camera.lifecycle.ProcessCameraProvider
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import androidx.fragment.app.viewModels
import androidx.lifecycle.lifecycleScope
import com.example.pecheck.R
import com.example.pecheck.databinding.FragmentTeacherBinding
import com.google.mlkit.vision.barcode.BarcodeScanning
import com.google.mlkit.vision.common.InputImage
import kotlinx.coroutines.flow.collectLatest
import java.util.concurrent.ExecutorService
import java.util.concurrent.Executors

class TeacherFragment : Fragment() {

    private var _binding: FragmentTeacherBinding? = null
    private val binding get() = _binding!!
    
    private val viewModel: TeacherViewModel by viewModels()
    private var imageCapture: ImageCapture? = null
    private lateinit var cameraExecutor: ExecutorService
    private var camera: Camera? = null
    private var isScanning = false

    private val requestPermissionLauncher = registerForActivityResult(
        ActivityResultContracts.RequestPermission()
    ) { isGranted: Boolean ->
        if (isGranted) {
            startCamera()
        } else {
            Toast.makeText(requireContext(), R.string.error_permission, Toast.LENGTH_SHORT).show()
        }
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentTeacherBinding.inflate(inflater, container, false)
        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        
        cameraExecutor = Executors.newSingleThreadExecutor()
        
        setupUI()
        observeViewModel()
        checkCameraPermission()
    }

    private fun setupUI() {
        binding.btnOpenScanner.setOnClickListener {
            if (!isScanning) {
                startScanning()
            }
        }
        
        binding.btnStopScanner.setOnClickListener {
            if (isScanning) {
                stopScanning()
            }
        }
        
        binding.btnMarkPresent.setOnClickListener {
            viewModel.markStudentPresent()
            hideScanResult()
        }
        
        binding.btnMarkAbsent.setOnClickListener {
            viewModel.markStudentAbsent()
            hideScanResult()
        }
    }

    private fun observeViewModel() {
        viewLifecycleOwner.lifecycleScope.launchWhenStarted {
            viewModel.scannedStudent.collectLatest { student ->
                if (student != null) {
                    showScanResult(student)
                }
            }
        }
        
        viewLifecycleOwner.lifecycleScope.launchWhenStarted {
            viewModel.presentCount.collectLatest { count ->
                binding.tvPresentCount.text = count.toString()
            }
        }
        
        viewLifecycleOwner.lifecycleScope.launchWhenStarted {
            viewModel.absentCount.collectLatest { count ->
                binding.tvAbsentCount.text = count.toString()
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

    private fun checkCameraPermission() {
        when {
            ContextCompat.checkSelfPermission(
                requireContext(),
                Manifest.permission.CAMERA
            ) == PackageManager.PERMISSION_GRANTED -> {
                startCamera()
            }
            shouldShowRequestPermissionRationale(Manifest.permission.CAMERA) -> {
                Toast.makeText(requireContext(), R.string.error_permission, Toast.LENGTH_SHORT).show()
            }
            else -> {
                requestPermissionLauncher.launch(Manifest.permission.CAMERA)
            }
        }
    }

    private fun startCamera() {
        val cameraProviderFuture = ProcessCameraProvider.getInstance(requireContext())

        cameraProviderFuture.addListener({
            val cameraProvider: ProcessCameraProvider = cameraProviderFuture.get()

            val preview = Preview.Builder()
                .build()
                .also {
                    it.setSurfaceProvider(binding.viewFinder.surfaceProvider)
                }

            imageCapture = ImageCapture.Builder()
                .setCaptureMode(ImageCapture.CAPTURE_MODE_MINIMIZE_LATENCY)
                .build()

            val imageAnalyzer = ImageAnalysis.Builder()
                .setBackpressureStrategy(ImageAnalysis.STRATEGY_KEEP_ONLY_LATEST)
                .build()
                .also {
                    it.setAnalyzer(cameraExecutor, QRCodeAnalyzer { qrCode ->
                        if (isScanning && qrCode.isNotEmpty()) {
                            viewModel.onQrScanned(qrCode)
                            stopScanning()
                        }
                    })
                }

            try {
                cameraProvider.unbindAll()
                camera = cameraProvider.bindToLifecycle(
                    this,
                    CameraSelector.DEFAULT_BACK_CAMERA,
                    preview,
                    imageCapture,
                    imageAnalyzer
                )
            } catch (exc: Exception) {
                Toast.makeText(requireContext(), R.string.error_camera, Toast.LENGTH_SHORT).show()
            }
        }, ContextCompat.getMainExecutor(requireContext()))
    }

    private fun startScanning() {
        isScanning = true
        binding.btnOpenScanner.text = getString(R.string.scanning)
        binding.btnOpenScanner.isEnabled = false
        binding.btnStopScanner.isEnabled = true
        binding.scanLine.visibility = View.VISIBLE
        
        // Анимация сканирующей линии
        binding.scanLine.animate()
            .translationY(200f)
            .setDuration(2000)
            .withEndAction {
                binding.scanLine.translationY = 0f
                if (isScanning) {
                    binding.scanLine.animate()
                        .translationY(200f)
                        .setDuration(2000)
                        .start()
                }
            }
            .start()
    }

    private fun stopScanning() {
        isScanning = false
        binding.btnOpenScanner.text = getString(R.string.start_scanning)
        binding.btnOpenScanner.isEnabled = true
        binding.btnStopScanner.isEnabled = false
        binding.scanLine.visibility = View.GONE
        binding.scanLine.clearAnimation()
    }

    private fun showScanResult(studentInfo: String) {
        binding.cardScanResult.visibility = View.VISIBLE
        binding.tvScannedStudent.text = studentInfo
    }

    private fun hideScanResult() {
        binding.cardScanResult.visibility = View.GONE
    }

    override fun onDestroyView() {
        super.onDestroyView()
        cameraExecutor.shutdown()
        _binding = null
    }

    private class QRCodeAnalyzer(private val onQRCodeDetected: (String) -> Unit) : ImageAnalysis.Analyzer {
        private val scanner = BarcodeScanning.getClient()

        @androidx.camera.core.ExperimentalGetImage
        override fun analyze(imageProxy: ImageProxy) {
            val mediaImage = imageProxy.image
            if (mediaImage != null) {
                val image = InputImage.fromMediaImage(mediaImage, imageProxy.imageInfo.rotationDegrees)
                
                scanner.process(image)
                    .addOnSuccessListener { barcodes ->
                        for (barcode in barcodes) {
                            barcode.rawValue?.let { qrCode ->
                                onQRCodeDetected(qrCode)
                            }
                        }
                    }
                    .addOnCompleteListener {
                        imageProxy.close()
                    }
            } else {
                imageProxy.close()
            }
        }
    }
} 