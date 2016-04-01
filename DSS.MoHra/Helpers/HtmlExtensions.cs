using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace DSS.MoHra.Helpers
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString ValidationSummaryHMM(this HtmlHelper helper, string addition)
        {
            var errorList = new List<ModelErrorCollection>();
            var modelState = helper.ViewData.ModelState;

            // form dom element ids
            var errorMessageWrapperId = "errorMessageWrapper";
            var errorMessageId = "errorMessage";
            if (!string.IsNullOrEmpty(addition))
            {
                errorMessageWrapperId += "_" + addition;
                errorMessageId += "_" + addition;
            }

            // restore errors from storage
            if (ModelStateHelper.Any())
            {
                errorList.AddRange(ModelStateHelper.GetList());
                ModelStateHelper.Clear();
            }
            // add modelState errors
            if (!modelState.IsValid)
            {
                errorList.AddRange(modelState.Where(d => d.Key == string.Empty && d.Value.Errors.Count > 0).Select(d => d.Value.Errors).ToList());
            }

            var result = "<div id='" + errorMessageWrapperId + "' class='alert alert-danger " + (modelState.IsValid && !errorList.Any() ? "hide" : "") + "'><strong>Были обранужены ошибки</strong><div id='" + errorMessageId + "'>";
            if (errorList.Any())
            {
                result += "<ul>";
                foreach (var errorCollection in errorList)
                {
                    foreach (var err in errorCollection)
                    {
                        if (!string.IsNullOrEmpty(err.ErrorMessage))
                            result += "<li>" + helper.Encode(err.ErrorMessage) + "</li>";
                        else
                            result += "<li>" + helper.Encode(err.Exception.Message) + "</li>";
                    }
                }
                result += "</ul>";
            }
            result += "</div></div>";

            return new MvcHtmlString(result);
        }

        public static MvcHtmlString ValidationSummaryHMM(this HtmlHelper helper)
        {
            return ValidationSummaryHMM(helper, null);
        }

        public static MvcHtmlString BackLink(this HtmlHelper helper, string defaultUrl, HttpRequestBase request)
        {
            string result = "<a href='{0}' class='btn btn-default'><i class='fa fa-undo'></i> Вернуться</a>";
            string link = defaultUrl;

            // works with referer
            if (request != null)
            {
                var referer = request.UrlReferrer;
                if (referer != null && referer.Host == request.Url.Host)
                    link = referer.AbsoluteUri;
            }

            result = string.Format(result, link);
            return new MvcHtmlString(result);
        }

        public static BootstrapDropdown BootstrapDropdown(this HtmlHelper helper, string id, string btnClass = "btn btn-sm btn-default")
        {
            return new BootstrapDropdown(helper.ViewContext, id, btnClass);
        }

        public static MvcHtmlString MarkPopupped(this HtmlHelper helper, string title)
        {
            return new MvcHtmlString(" data-show-in-popup='true' data-popup-title='" + title + "' ");
        }

        public static MvcHtmlString MarkPopupped(this HtmlHelper helper, string title, string identity)
        {
            return new MvcHtmlString(" data-show-in-popup='true' data-popup-title='" + title + "' data-popup-id='" + identity + "' ");
        }

        public static MvcHtmlString Nl2Br(this HtmlHelper helper, string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;
            return new MvcHtmlString(text.Replace("\r\n", "</br>\r\n"));
        }
    }

    public class BootstrapDropdown : IDisposable
    {
        ViewContext _context;
        //string _id;

        public BootstrapDropdown(ViewContext context, string id, string btnClass)
        {
            _context = context;
            //_id = id;

            _context.Writer.Write("<span class='dropdown'>");
            _context.Writer.Write("<button id='dd_" + id + "' class='" + btnClass + "' type='button' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'>");
            _context.Writer.Write("<i class='fa fa-ellipsis-h'></i>");
            _context.Writer.Write("</button>");
            _context.Writer.Write("<div class='dropdown-wrapper' aria-labelledby='dd_" + id + "'>");
        }

        public void Dispose()
        {
            _context.Writer.WriteLine("</div></span>");
        }
    }
}