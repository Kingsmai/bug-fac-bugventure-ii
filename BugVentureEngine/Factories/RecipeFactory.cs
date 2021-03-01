using BugVentureEngine.Models;
using System.Collections.Generic;
using System.Linq;

namespace BugVentureEngine.Factories
{
	public static class RecipeFactory
	{
		private static readonly List<Recipe> _recipes = new List<Recipe>();

		static RecipeFactory()
		{
			Recipe granoraBar = new Recipe(1, "Granola bar");
			granoraBar.AddIngredient(3001, 1);
			granoraBar.AddIngredient(3002, 1);
			granoraBar.AddIngredient(3003, 1);
			granoraBar.AddOutputItem(2001, 1);

			_recipes.Add(granoraBar);
		}

		public static Recipe RecipeByID(int id)
		{
			return _recipes.FirstOrDefault(x => x.ID == id);
		}
	}
}
