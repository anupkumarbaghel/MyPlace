using System.Web.Mvc;
using MyPlace.Web.Models;

namespace MyPlace.Web.Controllers 
{
    public class TinyMCESampleJQueryController : Controller 
	{
		public ActionResult Index() 
		{
			return View(new TinyMCEModelJQuery { 
				Basic = "This editor instance is using the 'tinymce_jquery_basic_compressed' template.", 
				Classic = "This editor instance is using the 'tinymce_jquery_classic_compressed' template.", 
				Full = "This editor instance is using the 'tinymce_jquery_full_compressed' template.", 
			});
        }
    }
}