using Nop.Core.Domain.Catalog;
using Nop.Core.Events;
using Nop.Plugin.Widgets.CustomTest.Domain;
using Nop.Services.Catalog;
using Nop.Services.Events;

namespace Nop.Plugin.Widgets.CustomTest.Services;

/// <summary>
/// Represents plugin event consumer
/// </summary>
public class EventConsumer :
    IConsumer<EntityInsertedEvent<Student>>

{
    #region Fields

    protected readonly IStudentService _studentService;

    #endregion

    #region Ctor

    public EventConsumer(IStudentService studentService)
    {
        _studentService = studentService;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Handle entity created event
    /// </summary>
    /// <param name="eventMessage">Event message</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityInsertedEvent<Student> eventMessage)
    {
        if (eventMessage.Entity is null)
            return;
        var evenTest = new EventTest { IsInserted = true };

        await _studentService.InsertEventTestAsync(evenTest);
    }

    #endregion
}