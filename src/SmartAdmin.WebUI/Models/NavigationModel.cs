using System.Linq.Expressions;
using SmartAdmin.WebUI.Extensions;
#nullable disable

namespace SmartAdmin.WebUI.Models
{

    public static class NavigationModel
    {
        private const string Underscore = "_";
        private const string Dash = "-";
        private const string Space = " ";
        private static readonly string Empty = string.Empty;
        public static readonly string Void = "javascript:void(0);";

        public static SmartNavigation Seed => BuildNavigation();
        public static SmartNavigation Full => BuildNavigation(seedOnly: false);

        private static SmartNavigation BuildNavigation(bool seedOnly = true, Expression<Func<ListItem, bool>> criteria = null)
        {
            var jsonText = File.ReadAllText("nav.json");
            var navigation = NavigationBuilder.FromJson(jsonText);
            var menu = FillProperties(navigation.Lists, seedOnly,null, criteria);

            return new SmartNavigation(menu);
        }
        public static SmartNavigation GetNavigation(Expression<Func<ListItem, bool>> criteria)
        {
            return BuildNavigation(seedOnly: false, criteria:criteria);
        }

        private static List<ListItem> FillProperties(IEnumerable<ListItem> items, bool seedOnly, ListItem parent = null, Expression<Func<ListItem, bool>> criteria = null)
        {
            var result = new List<ListItem>();

            foreach (var item in items.Where(criteria.Compile()))
            {
                item.Text ??= item.Title;
                item.Tags = string.Concat(parent?.Tags, Space, item.Title.ToLower()).Trim();

                var parentRoute = (Path.GetFileNameWithoutExtension(parent?.Text ?? Empty)?.Replace(Space, Underscore) ?? Empty).ToLower();
                var sanitizedHref = parent == null ? item.Href?.Replace(Dash, Empty) : item.Href?.Replace(parentRoute, parentRoute.Replace(Underscore, Empty)).Replace(Dash, Empty);
                var route = Path.GetFileNameWithoutExtension(sanitizedHref ?? Empty)?.Split(Underscore) ?? Array.Empty<string>();

                item.Route = route.Length > 1 ? $"/{route.First()}/{string.Join(Empty, route.Skip(1))}" : item.Href;

                item.I18n = parent == null
                    ? $"nav.{item.Title.ToLower().Replace(Space, Underscore)}"
                    : $"{parent.I18n}_{item.Title.ToLower().Replace(Space, Underscore)}";
                item.Type = parent == null ? item.Href == null ? ItemType.Category : ItemType.Single : item.Items.Any() ? ItemType.Parent : ItemType.Child;
                item.Items = FillProperties(item.Items, seedOnly, item, criteria);

                if (item.Href.IsVoid() && item.Items.Any())
                    item.Type = ItemType.Sibling;

                if ((!seedOnly || item.ShowOnSeed))
                    result.Add(item);
            }

            return result;
        }
    }
}
