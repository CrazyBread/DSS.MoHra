using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DSS.MoHra.Helpers
{
    public static class ModelStateHelper
    {
        static readonly string Key = "SavedHMMModelState";
        static readonly object safeLock = new object();

        public static bool Any()
        {
            var list = (List<ModelErrorCollection>)HttpContext.Current.Session[Key];
            return list != null && list.Count > 0;
        }

        public static List<ModelErrorCollection> GetList()
        {
            return (List<ModelErrorCollection>)HttpContext.Current.Session[Key];
        }

        public static void Clear()
        {
            lock (safeLock)
            {
                HttpContext.Current.Session[Key] = null;
            }
        }

        static void Init(HttpSessionStateBase Session)
        {
            lock (safeLock)
            {
                if (!(Session[Key] is List<ModelErrorCollection>))
                    Session[Key] = new List<ModelErrorCollection>();
            }
        }

        public static void Add(ModelErrorCollection e, HttpSessionStateBase Session)
        {
            if (e == null)
                return;
            Init(Session);
            lock (safeLock)
            {
                ((List<ModelErrorCollection>)Session[Key]).Add(e);
            }
        }
    }
}

namespace DSS.MoHra
{
    public static class ModelStateExtensions
    {
        /// <summary>
        /// Возвращает словарь параметр:текст_первой_ошибки.
        /// </summary>
        public static Dictionary<string, string> GetParameterErrors(this ModelStateDictionary modelState)
        {
            if (!modelState.Any())
                return null;
            Dictionary<string, string> d = new Dictionary<string, string>();
            foreach (var parameters in modelState)
            {
                if (parameters.Value != null && parameters.Value.Errors != null && parameters.Value.Errors.Any())
                {
                    var firstError = parameters.Value.Errors.First();
                    d.Add(parameters.Key, (firstError.Exception != null) ? firstError.Exception.Message : firstError.ErrorMessage);
                }
            }
            return d;
        }
    }
}