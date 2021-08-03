using BLL.Interface;
using BLL.Interface.Dto;
using Microsoft.AspNetCore.Mvc;
using PL.API.Open.Controllers;

namespace SmlTestTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : BaseCRUDApiController<StudentDto, int>
    {

        public StudentController(IComplexProvider unitOfWork) : base(unitOfWork)
        {
            UseService(typeof(StudentDto));
        }
    }
}