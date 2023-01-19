using Foodie.Models;

using System;
using System.Collections.Generic;
using System.Text;

namespace FoodieTests
{
    public class TestDataProvider
    {
        public static IEnumerable<RecipeItem> GetRecipeItemsForSpicyFish()
        {
            return new List<RecipeItem>()
            {
                new RecipeItem() {Id = 1, Name = "Fish", Amount = "100g", RecipeId = 1},
                new RecipeItem() {Id = 2, Name = "Spicy", Amount = "200g", RecipeId = 1}
            };
        }

        public static IEnumerable<RecipeItem> GetRecipeItemsForSpicyRibs()
        {
            return new List<RecipeItem>()
            {
                new RecipeItem() {Id = 3, Name = "Ribs", Amount = "1000g", RecipeId = 2},
                new RecipeItem() {Id = 4, Name = "Sweet", Amount = "2000g", RecipeId = 2}
            };
        }

        public static IEnumerable<RecipeItem> GetecipeItemsForSpicyPork()
        {
            return new List<RecipeItem>()
            {
                new RecipeItem() { Id = 5, Name = "Pork", Amount = "100g", RecipeId = 3},
                new RecipeItem() { Id = 6, Name = "Spicy", Amount = "200g", RecipeId = 3}
            };
        }

        public static IEnumerable<Recipe> GetAllRecipes()
        {
            return new List<Recipe>()
            {
                 new Recipe() { Id = 1, Name = "Spicy Fish", RecipeItems = GetRecipeItemsForSpicyFish() },
                 new Recipe() { Id = 2, Name = "Sweet Ribs", RecipeItems = GetRecipeItemsForSpicyRibs()},
                 new Recipe() { Id = 3, Name = "Spicy Pork", RecipeItems = GetecipeItemsForSpicyPork() }
            };
        }
    }
}
