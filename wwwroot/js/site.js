// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Function to highlight weekdays in the AppointmentDate calendar based on selected ScheduleId
function highlightWeekdays() {
    // Get the selected ScheduleId from the dropdown
    var selectedScheduleId = $("#ScheduleId").val();

    // Get the weekday corresponding to the selected ScheduleId
    var selectedWeekday = $("option:selected", "#ScheduleId").text().split(" - ")[0];

    // Highlight the corresponding weekday in the AppointmentDate calendar
    // Get all the days in the calendar
    var appointmentDateCalendar = $("#appointment-date");
    var days = appointmentDateCalendar.find(".datepicker-days tbody td");

    // Remove any existing highlight classes
    days.removeClass("highlight-day");

    // Loop through each day and add the highlight class to the matching weekday
    days.each(function () {
        var dayText = $(this).text();
        if (dayText === selectedWeekday) {
            $(this).addClass("highlight-day");
        }
    });
}

// Call the highlightWeekdays function when the ScheduleId dropdown changes
$(document).ready(function () {
    $("#ScheduleId").change(function () {
        highlightWeekdays();
    });
});