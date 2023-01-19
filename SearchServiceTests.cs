using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Foodie.Models;
using Moq;
using Foodie.Services.Interfaces;
using Foodie.Services.EFServices;
using FoodieTests;

namespace Foodie.Areas.Identity.Pages.Account.Manage.Tests
{
    [TestClass()]
    public class SearchServiceTests
    {
        private readonly Mock<IRecipeService> _mockRecipeService;
        private readonly Mock<IRecipeItemService> _mockRecipeItemService;
        private readonly SearchService _searchService;

        private List<Recipe> RecipesByRecipeSpicyFish;
        private List<Recipe> RecipesByIngredientSpicy;
        private List<Recipe> NoRecipesFound;

        //Prepare data
        [TestInitialize()]
        public void Setup()
        {
             RecipesByIngredientSpicy = new List<Recipe>()
            {
                new Recipe() { Id = 1, Name = "Spicy Fish", RecipeItems = TestDataProvider.GetRecipeItemsForSpicyFish().ToList() },
                new Recipe() { Id = 3, Name = "Spicy Pork", RecipeItems = TestDataProvider.GetecipeItemsForSpicyPork().ToList() }
            };

            RecipesByRecipeSpicyFish = new List<Recipe>()
            {
                new Recipe() { Id = 1, Name = "Spicy Fish", RecipeItems = TestDataProvider.GetRecipeItemsForSpicyFish().ToList() },
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