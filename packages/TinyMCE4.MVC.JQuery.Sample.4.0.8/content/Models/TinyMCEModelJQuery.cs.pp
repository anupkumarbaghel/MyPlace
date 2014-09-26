using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace $rootnamespace$.Models 
{
    public class TinyMCEModelJQuery 
	{
		[AllowHtml]
        [UIHint("tinymce_jquery_basic_compressed")]
        public string Basic { get; set; }
		
		[AllowHtml]
        [UIHint("tinymce_jquery_classic_compressed")]
        public string Classic { get; set; }
		
        [AllowHtml]
        [UIHint("tinymce_jquery_full_compressed")]
        public string Full { get; set; }
    }
}