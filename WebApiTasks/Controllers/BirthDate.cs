using Microsoft.AspNetCore.Mvc;

namespace WebApiTasks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BirthDateController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get([FromQuery] string? name = null, [FromQuery] int? years = null, [FromQuery] int? months = null, [FromQuery] int? days = null )
        {
            if (years == null || days == null || months == null)
            {
                if (string.IsNullOrEmpty(name))
                    return Ok("Hello anonymous, I can’t calculate your age without knowing your birthdate!");
                else
                    return Ok($"Hello {name}, I can’t calculate your age without knowing your birthdate!");
            }

            string age = CalculateAge(years, months, days);
            return Ok($"Hello {name} your age is {age}");
        }
        private string CalculateAge(int? years, int? months, int? days)
        {
            int day = DateTime.Today.Day;
            int month = DateTime.Today.Month;
            int year = DateTime.Today.Year;

            int currentDay = day - (int)days;
            int currentMonth = month -(int)months;
            int currentYear = year - (int)years;

            if (currentDay < 0) 
            {
                currentDay = 30 + currentDay;
                currentMonth = currentMonth - 1;
            }
            if (currentMonth < 0)
            {
                currentMonth = 12 + currentMonth;
                currentYear = currentYear - 1;
            }

            string Age = $"{currentYear} Years, {currentMonth} Months, {currentDay} Days";
            
            return (Age);
        }
    }
}