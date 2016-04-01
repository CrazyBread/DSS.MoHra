using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DSS.MoHra
{
    public static class ControllerExtensions
    {
        public static void ProcessException(this Controller controller, Exception e, string defaultMessage = null)
        {
            var metaException = e as MetaException;
            if (metaException != null)
            {
                // global error/warning
                if (string.IsNullOrEmpty(metaException.ParamName))
                {
                    controller.ModelState.AddModelError("", e);
                }
                // parameter exception
                else
                {
                    controller.ModelState.AddModelError(metaException.ParamName, metaException.Message);
                    // add null error to show header
                }
            }
            // standart exception
            else
            {
                if (defaultMessage == null)
                {
                    string action = ((string)controller.Request.RequestContext.RouteData.Values["action"]).ToLower();
                    var i = e as System.Data.SqlClient.SqlException;
                    // try get to inner exception: 
                    // TODO: REMOVE IT
                    if (i == null && e.InnerException != null)
                        i = e.InnerException as System.Data.SqlClient.SqlException;
                    // try to get sql exception for db update
                    if (e is System.Data.Entity.Infrastructure.DbUpdateException && e.InnerException != null)
                        i = e.InnerException.InnerException as System.Data.SqlClient.SqlException;
                    if (i != null)
                    {
                        // PK violation
                        if (i.Class == 14 && (i.Number == 2601 || i.Number == 2627))
                            defaultMessage = "Такие данные уже существуют.";
                        // FK voilation
                        else if (i.Number == 547 && i.Class == 16)
                        {
                            if (action.Contains("edit") || action.Contains("add"))
                                defaultMessage = "Невозможно найти запись в справочнике.";
                            else
                                defaultMessage = "Данный объект используется в системе.";
                        }
                    }
                    else if (controller.Request.RequestType == "POST")
                    {
                        if (action.Contains("delete"))
                            defaultMessage = "Ошибки при удалении";
                        else
                            defaultMessage = "Ошибки при сохранении";
                    }
                    if (defaultMessage == null)
                        defaultMessage = "Ошибки при обработке";
                }
                if (controller.Request.IsLocal)
                {
                    defaultMessage += ": " + e.Message;
                    while (e.InnerException != null)
                    {
                        defaultMessage += "; " + e.InnerException.Message;
                        e = e.InnerException;
                    }
                }
                controller.ModelState.AddModelError("", defaultMessage);
            }
        }

        public static void StoreModelStateErrors(this Controller controller)
        {
            StoreModelStateErrors(controller.Session, controller.ModelState[""].Errors);
        }

        // mark private :)
        private static void StoreModelStateErrors(HttpSessionStateBase session, ModelErrorCollection errors)
        {
            Helpers.ModelStateHelper.Add(errors, session);
        }
    }
}