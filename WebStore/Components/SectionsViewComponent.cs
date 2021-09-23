using Microsoft.AspNetCore.Mvc;

namespace WebStore.Components
{
    public class SectionsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View();
    }
}
