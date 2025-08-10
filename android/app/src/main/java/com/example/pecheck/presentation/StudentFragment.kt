package com.example.pecheck.presentation

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.ImageView
import androidx.fragment.app.Fragment
import androidx.fragment.app.viewModels
import androidx.lifecycle.lifecycleScope
import com.example.pecheck.R
import kotlinx.coroutines.flow.collectLatest

class StudentFragment : Fragment() {

    private val viewModel: StudentViewModel by viewModels()

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        return inflater.inflate(R.layout.fragment_student, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        val image = view.findViewById<ImageView>(R.id.imageQr)
        val btnRefresh = view.findViewById<Button>(R.id.btnRefresh)

        btnRefresh.setOnClickListener { viewModel.loadQr() }

        viewLifecycleOwner.lifecycleScope.launchWhenStarted {
            viewModel.qrImage.collectLatest { bmp ->
                if (bmp != null) image.setImageBitmap(bmp)
            }
        }

        viewModel.loadQr()
    }
} 