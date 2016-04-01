using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DSS.MoHra
{
    public class Controller : System.Web.Mvc.Controller
    {
        public Models.DataContext db = new Models.DataContext();

        protected override ViewResult View(string viewName, string masterName, object model)
        {
            // Ajax hack: if AjaxRequest, find <viewname>Ajax.cshtml. if it was finded, use
            // Author: Vladislav Moiseev
            if (Request.IsAjaxRequest())
            {
                var ownViewName = viewName ?? (string)RouteData.Values["action"] ?? "Index";
                ownViewName += "Ajax";
                var ajaxView = ViewEngines.Engines.FindView(ControllerContext, ownViewName, null);
                if (ajaxView != null && ajaxView.View != null)
                    return base.View(ownViewName, masterName, model);
            }
            return base.View(viewName, masterName, model);
        }

        /// <summary>
        /// Уведомляет об успешности выполнения операции. Указывает на страницу перехода или на возможность обновления страницы.
        /// </summary>
        /// <param name="redirectUrl"></param>
        /// <returns></returns>
        public JsonResult JsonResponse(bool needRedirect, string redirectUrl)
        {
            return _JsonResponse(new JsonResponse() { Success = true, NeedRedirect = needRedirect, NeedReload = !needRedirect, RedirectUrl = redirectUrl });
        }

        /// <summary>
        /// Уведомляет об успешности выполнения операции. Можно дать команду перезагрузить страницу.
        /// </summary>
        /// <param name="data">Данные</param>
        /// <param name="reload">Необходимость перезагрузить страницу</param>
        /// <returns></returns>
        public JsonResult JsonResponse(object data)
        {
            return _JsonResponse(new JsonResponse() { Success = true, Data = data });
        }

        /// <summary>
        /// Уведомляет об ошибке. Можно вернуть информацию об исключении и доп. информации.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public JsonResult JsonResponse(object data, Exception ex)
        {
            return _JsonResponse(new JsonResponse() { Success = false, Data = data, Message = ex.Message });
        }

        /// <summary>
        /// Уведомляет об ошибке. Можно вернуть текст ошибки.
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public JsonResult JsonResponse(string errorMessage)
        {
            return _JsonResponse(new JsonResponse() { Success = false, Message = errorMessage });
        }

        /// <summary>
        /// Уведомляет об ошибке. Собирает сведения только из ModelState.
        /// </summary>
        /// <returns></returns>
        public JsonResult JsonResponse()
        {
            return _JsonResponse(new JsonResponse() { Success = false });
        }

        private JsonResult _JsonResponse(JsonResponse response)
        {
            var modelErrors = ModelState.GetParameterErrors();
            if (modelErrors != null && modelErrors.Any())
            {
                if (modelErrors.ContainsKey(""))
                {
                    response.Message = modelErrors[""];
                    modelErrors.Remove("");
                }
                response.ModelErrors = modelErrors;
            }
            return Json(response);
        }
    }
}