using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Blogs;
using Nop.Plugin.Widgets.CustomTest.Domain;
using Nop.Plugin.Widgets.CustomTest.Models;
using Nop.Plugin.Widgets.CustomTest.Services;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Security;
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
    public ActionResult StudentPostCreate()
    {
        return View();
    }

    [HttpPost]
    public virtual async Task<IActionResult> StudentPostCreate(StudentCreateModel model)
    {
        if (ModelState.IsValid)
        {
            var student = model.ToEntity<Student>();
            await _studentService.InsertStudentAsync(student);
            return RedirectToAction("TestCrudMenu");
        }
        else
        {
            var student = new Student { Age = model.Age, Name = model.Name};
            await _studentService.InsertStudentAsync(student);
            return RedirectToAction("TestCrudMenu");
        }

        //return View(model);
    }

    public virtual async Task<IActionResult> StudentPostEdit(int id)
    {
        var student = await _studentService.GetStudentPostByIdAsync(id);
        if (student == null)
            return RedirectToAction("TestCrudMenu");

        //prepare model
        // var model = await _blogModelFactory.PrepareBlogPostModelAsync(null, blogPost);
        var model = new StudentCreateModel { Id = student.Id, Name = student.Name, Age = student.Age };
        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> StudentPostEdit(StudentCreateModel model)
    {
        var student = await _studentService.GetStudentPostByIdAsync(model.Id);
        if (student == null)
            return RedirectToAction("TestCrudMenu");

        if (ModelState.IsValid)
        {
            student = model.ToEntity<Student>();
            await _studentService.UpdateStudentPostAsync(student);
            return RedirectToAction("TestCrudMenu");
        }
        else
        {
            student.Name = model.Name;
            student.Age = model.Age;    
            await _studentService.UpdateStudentPostAsync(student);
            return RedirectToAction("TestCrudMenu");
        }

        //return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> Delete(int id)
    {
        var student = await _studentService.GetStudentPostByIdAsync(id);
        if (student == null)
            return RedirectToAction("TestCrudMenu");

        await _studentService.DeleteStudentPostAsync(student);

        return RedirectToAction("TestCrudMenu");
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
