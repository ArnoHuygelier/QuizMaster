using Microsoft.AspNetCore.Mvc;


namespace QuizMaster.Ui.Mvc.Controllers.ControllerBases
{
    public abstract class CrudController<T,TId> : Controller where T : class
    {
        
        public abstract Task<IActionResult> Index();
        public abstract Task<IActionResult> Detail(TId id);
        public abstract IActionResult Create();
        public abstract Task<IActionResult> Create(T entity);
        public abstract Task<IActionResult> Edit(TId id);
        public abstract Task<IActionResult> Edit(TId id, T entity);
        public abstract Task<IActionResult> Delete(TId id);
        public abstract Task<IActionResult> DeleteConfirmed(TId id);

    }
}
