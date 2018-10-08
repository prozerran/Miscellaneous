using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApiDemo.Models;

//  ================= TOP ====================================
// ASP.NET WEB API 2.0 TUTURIAL
// https://www.youtube.com/watch?v=0pcM6teVdKk&list=PL6n9fhu94yhW7yoUOGNOfHurUE6bpOO2b
// ===========================================================

namespace WebApiDemo.Controllers
{
    [RoutePrefix("api/students")]
    public class StudentsController : ApiController
    {
        static List<Students> students = new List<Students>()
        {
            new Students() { Id = 1, Name = "Tom" },
            new Students() { Id = 2, Name = "Ben" },
            new Students() { Id = 3, Name = "Tim" }
        };

        // http://localhost:63609/api/students/
        [Route("")]
        public IEnumerable<Students> Get()
        {
            return students;
        }

        // http://localhost:63609/api/teachers
        // overrides RoutePrefix
        [Route("~/api/teachers")]
        public IEnumerable<string> GetTeachers()
        {
            return new List<string>() { "Teachers" };
        }

        // http://localhost:63609/api/students/1   
        // contraints int and minium value = 1
        [Route("{id:int:min(1)}")]
        //[Route("{id:int:min(1):max(3)}")]
        //[Route("{id:int:range(1,3)}")]
        public Students Get(int id)
        {
            return students.FirstOrDefault(s => s.Id == id);
        }

        // http://localhost:63609/api/students/tim
        // contraints that start with alpha
        [Route("{name:alpha}")]
        public Students Get(string name)
        {
            return students.FirstOrDefault(s => s.Name.ToLower() == name.ToLower());
        }

        // http://localhost:63609/api/routename
        // contraints that start with alpha
        [Route("~/api/routename", Name = "GetStudentById")]
        public HttpResponseMessage Post(Students student)
        {
            var resp = Request.CreateResponse(HttpStatusCode.Created);
            resp.Headers.Location = new Uri(Url.Link("GetStudentById", new { id = 1 }));
            return resp;
        }

        // http://localhost:63609/api/students/1/courses
        [Route("{id}/courses")]
        // [Route("api/students/{id}/courses")]     // without RoutePrefix
        public IEnumerable<string> GetStudentCourses(int id)
        {
            if (id == 1)
                return new List<string>() { "C#", "ASP.NET", "SQL SERVER" };
            else if (id == 2)
                return new List<string>() { "C++", "MFC", "MySQL" };
            else
                return new List<string>() { "Java", "Strut", "Oracle" };
        }

        // http://localhost:63609/api/webapi1/3
        [Route("~/api/webapi1/{id}")]
        public HttpResponseMessage GetWebApi1(int id)
        {
            var student = students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Student Not Found");
            }
            return Request.CreateResponse(HttpStatusCode.OK, student);
        }

        // http://localhost:63609/api/webapi2/3
        [Route("~/api/webapi2/{id}")]
        public IHttpActionResult GetWebApi2(int id)
        {
            var student = students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return Content(HttpStatusCode.NotFound, "Student Not Found");
            }
            return Ok(student);
        }

        // http://localhost:63609/api/getasync/1
        [Route("~/api/getasync/{id}")]
        public async Task<IHttpActionResult> GetAsync(int id)
        {
            var student = await Task.Run(() => students.FirstOrDefault(s => s.Id == id));

            if (student == null)
                return Content(HttpStatusCode.NotFound, "Student Not Found");

            return Ok(student);
        }

        // use Async Await for I/O bound job. a example code
        public async Task<string> GetFirstCharactersCountAsync(string url, int count)
        {
            // Execution is synchronous here
            var client = new HttpClient();

            // Execution of GetFirstCharactersCountAsync() is yielded to the caller here
            // GetStringAsync returns a Task<string>, which is *awaited*
            var page = await client.GetStringAsync("http://www.dotnetfoundation.org");

            // Execution resumes when the client.GetStringAsync task completes,
            // becoming synchronous again.

            if (count > page.Length)
            {
                return page;
            }
            else
            {
                return page.Substring(0, count);
            }
        }

        // use Task for CPU bound job. a example
        public async Task<int> CalculateResult(object data)
        {
            // This queues up the work on the threadpool.
            var expensiveResultTask = Task.Run(() => DoExpensiveCalculation(data));

            // Note that at this point, you can do some other work concurrently,
            // as CalculateResult() is still executing!

            // Execution of CalculateResult is yielded here!
            var result = await expensiveResultTask;

            return result;
        }

        private int DoExpensiveCalculation(object data) { return 1; }
    }
}
