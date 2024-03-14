using Core.Business.Entities.DataModels;
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
    }
}
