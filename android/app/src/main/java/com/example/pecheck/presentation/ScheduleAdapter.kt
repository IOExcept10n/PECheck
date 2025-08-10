package com.example.pecheck.presentation

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.core.content.ContextCompat
import androidx.recyclerview.widget.RecyclerView
import com.example.pecheck.R
import com.example.pecheck.domain.model.ScheduleItem

class ScheduleAdapter(
    private var items: List<ScheduleItem>
) : RecyclerView.Adapter<ScheduleAdapter.Vh>() {

    class Vh(itemView: View) : RecyclerView.ViewHolder(itemView) {
        val statusBar: View = itemView.findViewById(R.id.statusBar)
        val tvTitle: TextView = itemView.findViewById(R.id.tvTitle)
        val tvDateTime: TextView = itemView.findViewById(R.id.tvDateTime)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): Vh {
        val view = LayoutInflater.from(parent.context).inflate(R.layout.item_schedule, parent, false)
        return Vh(view)
    }

    override fun onBindViewHolder(holder: Vh, position: Int) {
        val item = items[position]
        holder.tvTitle.text = item.title
        holder.tvDateTime.text = item.dateTime
        val colorRes = when (item.status) {
            ScheduleItem.Status.ATTENDED -> R.color.attendance_attended
            ScheduleItem.Status.CANCELED -> R.color.attendance_canceled
            ScheduleItem.Status.PLANNED -> R.color.colorPrimary
        }
        holder.statusBar.setBackgroundColor(ContextCompat.getColor(holder.itemView.context, colorRes))
    }

    override fun getItemCount(): Int = items.size

    fun submitList(newItems: List<ScheduleItem>) {
        items = newItems
        notifyDataSetChanged()
    }
} 