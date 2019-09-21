using Bop.Services.Tasks;
using Bop.Web.Framework.Controllers;
using Microsoft.AspNetCore.Mvc;


namespace Bop.Web.Controllers
{

    public class ScheduleTaskController : BaseController
    {
        private readonly IScheduleTaskService _scheduleTaskService;

        public ScheduleTaskController(IScheduleTaskService scheduleTaskService)
        {
            _scheduleTaskService = scheduleTaskService;
        }

        [HttpPost]
        public virtual IActionResult RunTask(string taskType)
        {
            var scheduleTask = _scheduleTaskService.GetTaskByType(taskType);
            if (scheduleTask == null)
                return NoContent();

            var task = new Task(scheduleTask);
            task.Execute();

            return NoContent();
        }

    }
}