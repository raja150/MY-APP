using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transmart.TS4API.Models;

namespace Transmart.TS4API
{
    public interface ITs4ApiS
    {
		[Get("/api/Organization/Schedules")]
        Task<List<ScheduleModel>> GetSchedules();

        [Post("/api/Organization/Schedules")]
        Task<List<ScheduleModel>> PostSchedules(List<ScheduleModel> model);

        [Get("/api/Organization/CodingAttendance/CDAttLogs?AttDate={AttDate}")]
        Task<List<AttendanceImportModel>> GetCodingAttLogs(DateTime AttDate);

        [Get("/api/Organization/MtAttendance/MTAttLogs?AttDate={AttDate}")]
        Task<List<AttendanceImportModel>> GetTranscriptionAttLogs(DateTime AttDate);
    }
}
