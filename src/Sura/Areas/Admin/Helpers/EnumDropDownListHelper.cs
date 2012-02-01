using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Sura.Areas.Admin.Helpers
{
    public static class EnumDropDownListHelper
    {
        public static string GetInputName<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            if (expression.Body.NodeType == ExpressionType.Call)
            {
                var methodCallExpression = (MethodCallExpression)expression.Body;
                var name = GetInputName(methodCallExpression);

                return name.Substring(expression.Parameters[0].Name.Length + 1);
            }

            return expression.Body.ToString().Substring(expression.Parameters[0].Name.Length + 1);
        }

        private static string GetInputName(MethodCallExpression expression)
        {
            var methodCallExpression = expression.Object as MethodCallExpression;

            return methodCallExpression != null ? GetInputName(methodCallExpression) : expression.Object.ToString();
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression) where TModel : class
        {
            var value = helper.ViewData.Model == null ? default(TProperty) : expression.Compile()(helper.ViewData.Model);

            return helper.DropDownList(GetInputName(expression), ToSelectList(typeof(TProperty), value.ToString()));
        }

        public static SelectList ToSelectList(Type enumType, string selected)
        {
            var items = new List<SelectListItem>();
            foreach (var item in Enum.GetValues(enumType))
            {
                var fi = enumType.GetField(item.ToString());
                var attribute = fi.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault();
                var title = attribute == null ? item.ToString() : ((DescriptionAttribute)attribute).Description;
                var listItem = new SelectListItem
                {
                    Value = title,
                    Text = title,
                    Selected = selected == title
                };
                items.Add(listItem);
            }

            return new SelectList(items, "Value", "Text");
        }
    }
}