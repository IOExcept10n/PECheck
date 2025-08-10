package com.example.pecheck.presentation

import android.content.ActivityNotFoundException
import android.content.Intent
import android.net.Uri
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.TextView
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.fragment.app.viewModels
import com.example.pecheck.R

class TeacherFragment : Fragment() {

    private val viewModel: TeacherViewModel by viewModels()

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        return inflater.inflate(R.layout.fragment_teacher, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        view.findViewById<Button>(R.id.btnOpenScanner).setOnClickListener {
            launchExternalScanner()
        }
    }

    private fun launchExternalScanner() {
        // ZXing external scanner intent
        val intent = Intent("com.google.zxing.client.android.SCAN").apply {
            putExtra("SCAN_MODE", "QR_CODE_MODE")
        }
        try {
            startActivityForResult(intent, REQUEST_SCAN)
        } catch (e: ActivityNotFoundException) {
            // Offer to install ZXing from Play Store
            val marketUri = Uri.parse("market://details?id=com.google.zxing.client.android")
            val marketIntent = Intent(Intent.ACTION_VIEW, marketUri)
            try {
                startActivity(marketIntent)
            } catch (_: Exception) {
                Toast.makeText(requireContext(), "No scanner available", Toast.LENGTH_SHORT).show()
            }
        }
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)
        if (requestCode == REQUEST_SCAN && data != null) {
            val contents = data.getStringExtra("SCAN_RESULT")
            if (!contents.isNullOrEmpty()) {
                view?.findViewById<TextView>(R.id.tvHint)?.text = getString(R.string.scanned)
                viewModel.onQrScanned(contents)
            }
        }
    }

    companion object {
        private const val REQUEST_SCAN = 0x1337
    }
} 