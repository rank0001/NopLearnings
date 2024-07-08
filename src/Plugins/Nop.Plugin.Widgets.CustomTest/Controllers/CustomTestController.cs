using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.CustomTest.Models;
using Nop.Plugin.Widgets.CustomTest.Services;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Models.DataTables;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Widgets.CustomTest.Controllers;

[Area(AreaNames.ADMIN)]
[AuthorizeAdmin]
[AutoValidateAntiforgeryToken]
public class CustomTestController : BasePluginController
{
    private readonly IStudentService _studentService;
    public CustomTestController(IStudentService studentService)
    {
        _studentService = studentService;
    }
    public ActionResult Test()
    {
        return View();
    }

    public ActionResult TestCrudMenu()
    {
        return View();
    }
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> GetAllStudentListAsync()
    {
        try
        {
            return Ok(new DataTablesModel { Data = await _studentService.GetStudentListAsync() });
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
}
