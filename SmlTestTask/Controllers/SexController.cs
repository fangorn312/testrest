using BLL.Interface;
using BLL.Interface.Dto;
using Microsoft.AspNetCore.Mvc;
using PL.API.Open.Controllers;

namespace SmlTestTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SexController : BaseCRUDApiController<SexDto, int>
    {

        public SexController(IComplexProvider unitOfWork) : base(unitOfWork)
        {
            UseService(typeof(SexDto));
        }
    }
}