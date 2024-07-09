using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Blogs;
using Nop.Plugin.Widgets.CustomTest.Domain;
using Nop.Plugin.Widgets.CustomTest.Models;
using Nop.Plugin.Widgets.CustomTest.Services;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
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
    public ActionResult CrudPostCreate()
    {
        return View();
    }

    [HttpPost]
    public virtual async Task<IActionResult> CrudPostCreate(StudentModel model)
    {
        var errors = ModelState.Values.SelectMany(v => v.Errors);
        //if (ModelState.IsValid)
       // {
            var student = model.ToEntity<Student>();
            await _studentService.InsertStudentAsync(student);
            return RedirectToAction("TestCrudMenu", new { id = student.Id });
        //}

        return View(model);
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
