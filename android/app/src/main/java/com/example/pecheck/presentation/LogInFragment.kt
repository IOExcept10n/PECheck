package com.example.pecheck.presentation

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.EditText
import androidx.fragment.app.Fragment
import androidx.fragment.app.viewModels
import androidx.lifecycle.lifecycleScope
import com.example.pecheck.R
import com.example.pecheck.domain.model.UserRole
import kotlinx.coroutines.flow.collectLatest

class LogInFragment : Fragment() {

    private val viewModel: LogInViewModel by viewModels()

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        return inflater.inflate(R.layout.fragment_log_in, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        val etUsername = view.findViewById<EditText>(R.id.etUsername)
        val etPassword = view.findViewById<EditText>(R.id.etPassword)
        val btnLogin = view.findViewById<Button>(R.id.btnLogin)

        btnLogin.setOnClickListener {
            viewModel.login(
                etUsername.text?.toString().orEmpty(),
                etPassword.text?.toString().orEmpty()
            )
        }

        viewLifecycleOwner.lifecycleScope.launchWhenStarted {
            viewModel.role.collectLatest { role ->
                when (role) {
                    UserRole.STUDENT -> navigateTo(StudentFragment())
                    UserRole.TEACHER -> navigateTo(TeacherFragment())
                    null -> Unit
                }
            }
        }
    }

    private fun navigateTo(fragment: Fragment) {
        requireActivity().supportFragmentManager
            .beginTransaction()
            .replace(R.id.fragment_container, fragment)
            .addToBackStack(null)
            .commit()
    }
}