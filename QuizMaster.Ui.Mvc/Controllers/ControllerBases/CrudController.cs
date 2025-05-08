using Microsoft.AspNetCore.Mvc;


namespace QuizMaster.Ui.Mvc.Controllers.ControllerBases
{
    public abstract class CrudController<T> : Controller where T : class
    {
        
        public abstract Task<IActionResult> Index();
        public abstract Task<IActionResult> Detail(int id);
        public abstract IActionResult Create();
        public abstract Task<IActionResult> Create(T entity);
        public abstract Task<IActionResult> Edit(int id);
        public abstract Task<IActionResult> Edit(int id, T entity);
        public abstract Task<IActionResult> Delete(int id);
        public abstract Task<IActionResult> DeleteConfirmed(int id);

    }
}
