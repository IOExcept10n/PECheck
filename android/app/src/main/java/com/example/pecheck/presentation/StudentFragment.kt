package com.example.pecheck.presentation

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.ImageView
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.fragment.app.viewModels
import androidx.lifecycle.lifecycleScope
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.pecheck.R
import kotlinx.coroutines.flow.collectLatest

class StudentFragment : Fragment() {

    private val viewModel: StudentViewModel by viewModels()
    private lateinit var adapter: ScheduleAdapter

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
        val rv = view.findViewById<RecyclerView>(R.id.rvSchedule)

        adapter = ScheduleAdapter(emptyList())
        rv.layoutManager = LinearLayoutManager(requireContext())
        rv.adapter = adapter

        btnRefresh.setOnClickListener { viewModel.loadQr() }

        viewLifecycleOwner.lifecycleScope.launchWhenStarted {
            viewModel.qrImage.collectLatest { bmp ->
                if (bmp != null) image.setImageBitmap(bmp)
            }
        }

        viewLifecycleOwner.lifecycleScope.launchWhenStarted {
            viewModel.schedule.collectLatest { items ->
                adapter.submitList(items)
            }
        }

        viewLifecycleOwner.lifecycleScope.launchWhenStarted {
            viewModel.error.collectLatest { err ->
                if (!err.isNullOrBlank()) {
                    Toast.makeText(requireContext(), err, Toast.LENGTH_SHORT).show()
                }
            }
        }

        // Initial loads; if not authenticated, errors will be shown via toast
        viewModel.loadQr()
        viewModel.loadSchedule()
    }
} 