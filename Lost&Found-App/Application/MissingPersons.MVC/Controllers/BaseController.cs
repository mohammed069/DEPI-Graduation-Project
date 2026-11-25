using Microsoft.AspNetCore.Mvc;
using MissingPersons.BLL.Models;

namespace MissingPersons.MVC.Controllers;

public class BaseController : Controller {
    protected void SetSuccessMessage(string message) {
        TempData[AppConsts.SuccessMessage] = message;
    }

    protected void SetErrorMessage(string message) {
        TempData[AppConsts.ErrorMessage] = message;
    }

    protected IActionResult HandleFailure(Result result) {
        SetErrorMessage(result.Error.Description);
        return RedirectToAction("Index");
    }
}