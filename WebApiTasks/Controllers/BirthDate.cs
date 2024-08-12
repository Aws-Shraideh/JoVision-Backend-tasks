using Microsoft.AspNetCore.Mvc;

namespace WebApiTasks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BirthDateController : ControllerBase
    {
        public class Info 
        {
            public string? Name { get; set; } = null;
            public int? Year { get; set; } = null;
            public int? Month { get; set; } = null;
            public int? Day { get; set; } = null;
        }

        [HttpGet]
        public ActionResult Get([FromQuery] string? name = null, [FromQuery] int? year = null, [FromQuery] int? month = null, [FromQuery] int? day = null)
        {
            if (year == null || day == null || month == null)
            {
                if (string.IsNullOrEmpty(name))
                    return Ok("Hello anonymous, I can’t calculate your age without knowing your birthdate!");
                else
                    return Ok($"Hello {name}, I can’t calculate your age without knowing your birthdate!");
            }

            else if (year < 1900 || month > 12 || day > 32) 
            {
                return Ok("The values you entered are not correct, please try again");
            }

            string age = CalculateAge(year, month, day);
            return Ok($"Hello {name} your age is {age}");
        }
        [HttpPost]
        public ActionResult<Info> Create([FromForm] Info info)
        {
            if (info.Year == null || info.Day == null || info.Month == null)
            {
                if (string.IsNullOrEmpty(info.Name))
                    return Ok("Hello anonymous, I can’t calculate your age without knowing your birthdate!");
                else
                    return Ok($"Hello {info.Name}, I can’t calculate your age without knowing your birthdate!");
            }

            else if (info.Year < 1900 || info.Month > 12 || info.Day > 31)
            {
                return Ok("The date you entered is not correct, please try again");
            }

            string age = CalculateAge(info.Year, info.Month, info.Day);
            return Ok($"Hello {info.Name} your age is {age}");
        }
        private static string CalculateAge(int? years, int? months, int? days)
        {
            int day = DateTime.Today.Day;
            int month = DateTime.Today.Month;
            int year = DateTime.Today.Year;

            int currentDay = day - (int)days;
            int currentMonth = month - (int)months;
            int currentYear = year - (int)years;

            if (currentDay < 0) 
            {
                currentDay = 30 + currentDay;
                currentMonth--;
            }
            if (currentMonth < 0)
            {
                currentMonth = 12 + currentMonth;
                currentYear--;
            }

            string Age = $"{currentYear} Years, {currentMonth} Months, {currentDay} Days";
            
            return (Age);
        }
    }
}