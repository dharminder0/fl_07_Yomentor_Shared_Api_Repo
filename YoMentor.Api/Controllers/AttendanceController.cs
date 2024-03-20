using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Sevices.Abstract;
using Hub.Web.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace YoMentor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : BaseApiController
    {
        private readonly IAttendanceService _attendanceServices;
        public AttendanceController(IAttendanceService attendanceServices)
        {
            _attendanceServices = attendanceServices;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attendance"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> InsertAttendance(Attendance attendance)
        {
            try
            {
                var res = await _attendanceServices.InsertAttendance(attendance);
                return JsonExt(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("StudentsAttendance")]
        public async Task<IActionResult> GetStudentsAttendance(AttendanceRequest request) {
            var response = await _attendanceServices.GetStudentsAttendance(request);
            return JsonExt(response);
        }

        [HttpPost]
        [Route("Bulk/Add")]

        public async Task<IActionResult> BulkInsertAttendance(AttendanceV2 attendanceV2)
        {
            try
            {
                var res = await _attendanceServices.BulkInsertAttendance(attendanceV2);
                return JsonExt(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
