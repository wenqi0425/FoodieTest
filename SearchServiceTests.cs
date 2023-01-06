using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Foodie.Models;
using Moq;
using Foodie.Services.Interfaces;
using Foodie.Services.EFServices;

namespace Foodie.Areas.Identity.Pages.Account.Manage.Tests
{
    [TestClass()]
    public class SearchServiceTests
    {
        private readonly Mock<IRecipeService> _mockRecipeService;
        private readonly Mock<IRecipeItemService> _mockRecipeItemService;
        private readonly SearchService _searchService;
        //private readonly MockDbForTest _mockDb;

        private List<RecipeItem> RecipeItemsForSpicyFish;
        private List<RecipeItem> RecipeItemsForSpicyRibs;
        private List<RecipeItem> RecipeItemsForSpicyPork;
        private List<Recipe> AllRecipes;
        private List<Recipe> RecipesByRecipeSpicyFish;
        private List<Recipe> RecipesByIngredientSpicy;
        private List<Recipe> NoRecipesFound;

        //Prepare data
        [TestInitialize()]
        public void Setup()
        {
            RecipeItemsForSpicyFish = new List<RecipeItem>()
            {
                new RecipeItem() {Id = 1, Name = "Fish", Amount = "100g", RecipeId = 1},
                new RecipeItem() {Id = 2, Name = "Spicy", Amount = "200g", RecipeId = 1},
            };

            RecipeItemsForSpicyRibs = new List<RecipeItem>()
            {
                new RecipeItem() {Id = 3, Name = "Ribs", Amount = "1000g", RecipeId = 2},
                new RecipeItem() {Id = 4, Name = "Sweet", Amount = "2000g", RecipeId = 2}
            };

            RecipeItemsForSpicyPork = new List<RecipeItem>()
            {
                new RecipeItem() {Id = 3, Name = "Pork", Amount = "100g", RecipeId = 3},
                new RecipeItem() {Id = 4, Name = "Spicy", Amount = "200g", RecipeId = 3}
            };

            AllRecipes = new List<Recipe>()
            {
                new Recipe() { Id = 1, Name = "Spicy Fish", RecipeItems = RecipeItemsForSpicyFish },
                new Recipe() { Id = 2, Name = "Sweet Ribs", RecipeItems = RecipeItemsForSpicyRibs },
                new Recipe() { Id = 3, Name = "Spicy Pork", RecipeItems = RecipeItemsForSpicyPork}
            };

            RecipesByIngredientSpicy = new List<Recipe>()
            {
                new Recipe() { Id = 1, Name = "Spicy Fish", RecipeItems = RecipeItemsForSpicyFish },
                new Recipe() { Id = 3, Name = "Spicy Pork", RecipeItems = RecipeItemsForSpicyPork}
            };

            RecipesByRecipeSpicyFish = new List<Recipe>()
            {
                new Recipe() { Id = 1, Name = "Spicy Fish", RecipeItems = RecipeItemsForSpicyFish },
            };

            NoRecipesFound = new List<Recipe>() { };
        }

        public SearchServiceTests()
        {
            _mockRecipeService = new Mock<IRecipeService>();
            _mockRecipeItemService = new Mock<IRecipeItemService>();
            _searchService = new SearchService(_mockRecipeItemService.Object, _mockRecipeService.Object);
        }

        [TestMethod()]
        public void IsMockSuccessTest()
        {
            Assert.IsNotNull(_mockRecipeService);
            Assert.IsNotNull(_mockRecipeItemService);
            Assert.IsNotNull(_searchService);
        }

        [TestMethod()]
        public void SearchRecipesByIngredientTest()
        {
            //Arrange
            var searchCriteria = new RecipeCriteria
            {
                SearchCategory = "Ingredient",
                SearchCriterion = "Spicy"
            };

            // Setup
            _mockRecipeItemService.Setup(
                mockRecipeItemService => mockRecipeItemService
                .SearchRecipes(It.IsAny<string>())).Returns(RecipesByIngredientSpicy);

            //Act
            var results = _searchService.SearchRecipesByCriteria(searchCriteria).ToList();
            results.Reverse();

            int countOfResults = results.Count();

            //Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(countOfResults == RecipesByIngredientSpicy.Count());

            // In C#, Equal compare referrence, but SequenceEqual compare content
            Assert.IsTrue(RecipesByIngredientSpicy.SequenceEqual(results));
        }

        [TestMethod()]
        public void SearchRecipesByRecipeTest()
        {
            //Arrange
            var searchCriteria = new RecipeCriteria
            {
                SearchCategory = "Recipe",
                SearchCriterion = "Spicy Fish"
            };

            // Setup
            _mockRecipeService.Setup(
                mockRecipeService => mockRecipeService
                .SearchRecipes(It.IsAny<string>())).Returns(RecipesByRecipeSpicyFish);

            //Act
            var results = _searchService.SearchRecipesByCriteria(searchCriteria).ToList();
            results.Reverse();
            int countOfResults = results.Count();            

            //Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(countOfResults == RecipesByRecipeSpicyFish.Count());
            Assert.IsTrue(RecipesByRecipeSpicyFish.SequenceEqual(results));
        }

        [TestMethod()]
        public void SearchRecipesByRecipeNoResultTest()
        {
            //Arrange
            var searchCriteria = new RecipeCriteria
            {
                SearchCategory = "Recipe",
                SearchCriterion = "No this Recipe"
            };

            //Act
            var results = _searchService.SearchRecipesByCriteria(searchCriteria).ToList();
            int countOfResults = results.Count();

            // Setup
            _mockRecipeService.Setup(
                mockRecipeService => mockRecipeService
                .SearchRecipes(It.IsAny<string>())).Returns(NoRecipesFound);

            //Assert
            Assert.IsTrue(countOfResults == 0); ;
        }

        [TestMethod()]
        public void SearchRecipesByIngredientNoResultTest()
        {
            //Arrange
            var searchCriteria = new RecipeCriteria
            {
                SearchCategory = "Ingredient",
                SearchCriterion = "No this Ingredient"
            };

            //Act
            var results = _searchService.SearchRecipesByCriteria(searchCriteria).ToList();
            int countOfResults = results.Count();

            // Setup
            _mockRecipeItemService.Setup(
                mockRecipeItemService => mockRecipeItemService
                .SearchRecipes(It.IsAny<string>())).Returns(NoRecipesFound);

            //Assert
            Assert.IsTrue(countOfResults == 0); ;
        }
    }
}