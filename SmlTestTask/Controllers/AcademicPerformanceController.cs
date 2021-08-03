using BLL.Interface;
using BLL.Interface.Dto;
using Microsoft.AspNetCore.Mvc;
using PL.API.Open.Controllers;

namespace SmlTestTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AcademicPerformanceController : BaseCRUDApiController<AcademicPerformanceDto, int>
    {

        public AcademicPerformanceController(IComplexProvider unitOfWork) : base(unitOfWork)
        {
            UseService(typeof(AcademicPerformanceDto));
        }
    }
}