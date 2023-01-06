﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        //private readonly MockDbForTest _mockDb;

        //Prepare data
        private static List<RecipeItem> recipeItemsForSpicyFish = new List<RecipeItem>()
            {
                new RecipeItem() {Id = 1, Name = "Fish", Amount = "100g", RecipeId = 1},
                new RecipeItem() {Id = 2, Name = "Spicy", Amount = "200g", RecipeId = 1},
            };

        private static List<RecipeItem> recipeItemsForSpicyRibs = new List<RecipeItem>()
            {
                new RecipeItem() {Id = 3, Name = "Ribs", Amount = "1000g", RecipeId = 2},
                new RecipeItem() {Id = 4, Name = "Sweet", Amount = "2000g", RecipeId = 2}
            };

        private static List<RecipeItem> recipeItemsForSpicyPork = new List<RecipeItem>()
            {
                new RecipeItem() {Id = 3, Name = "Pork", Amount = "100g", RecipeId = 3},
                new RecipeItem() {Id = 4, Name = "Spicy", Amount = "200g", RecipeId = 3}
            };

        private static List<Recipe> _recipes = new List<Recipe>()
            {
                new Recipe() { Id = 1, Name = "Spicy Fish", RecipeItems = recipeItemsForSpicyFish },
                new Recipe() { Id = 2, Name = "Sweet Ribs", RecipeItems = recipeItemsForSpicyRibs },
                new Recipe() { Id = 3, Name = "Spicy Pork", RecipeItems = recipeItemsForSpicyPork}
            };

        public SearchServiceTests()
        {
            _mockRecipeService = new Mock<IRecipeService>();
            _mockRecipeItemService = new Mock<IRecipeItemService>();
            _searchService = new SearchService(_mockRecipeItemService.Object, _mockRecipeService.Object);
            //_mockDb = new MockDbForTest();
        }


        [TestMethod()]
        public void IsMockSuccessTest()
        {
            Assert.IsNotNull(_mockRecipeService);
            Assert.IsNotNull(_mockRecipeItemService);
            Assert.IsNotNull(_searchService);
            //Assert.IsNotNull(_mockDb);
        }

        [TestMethod()]
        public void SearchRecipesByIngredientTest()
        {
            //Arrange
            var searchCriteria = new RecipeCriteriaModel
            {
                SearchCategory = "Ingredient",
                SearchCriterion = "Spicy"
            };

            List<Recipe> recipesByIngredient = new List<Recipe>();

            // Setup
            _mockRecipeItemService.Setup(
                mockRecipeItemService => mockRecipeItemService
                .SearchRecipes(It.IsAny<string>())).Returns(recipesByIngredient);

            //Act
            var results = _searchService.SearchRecipesByCriteria(searchCriteria).ToList();  
            results.Reverse();

            int countOfResults = results.Count();

            //Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(recipesByIngredient.Count() == countOfResults);

            // In C#, Equal compare referrence, but SequenceEqual compare content
            Assert.IsTrue(recipesByIngredient.SequenceEqual(results));   
        }

        [TestMethod()]
        public void SearchRecipesByRecipeTest()
        {
            //Arrange
            var searchCriteria = new RecipeCriteriaModel
            {
                SearchCategory = "Recipe",
                SearchCriterion = "Spicy Fish"
            };

            List<Recipe> recipesByRecipe = new List<Recipe>();

            //Act
            var results = _searchService.SearchRecipesByCriteria(searchCriteria).ToList();
            results.Reverse();
            int countOfResults = results.Count();

            // Setup
            _mockRecipeService.Setup(
                mockRecipeService => mockRecipeService
                .SearchRecipes(It.IsAny<string>())).Returns(recipesByRecipe);

            //Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(recipesByRecipe.Count() == countOfResults);
            Assert.IsTrue(recipesByRecipe.SequenceEqual(results));
        }

        [TestMethod()]
        public void SearchRecipesByRecipeNoResultTest()
        {
            //Arrange
            var searchCriteria = new RecipeCriteriaModel
            {
                SearchCategory = "Recipe",
                SearchCriterion = "Null"
            };

            List<Recipe> recipesByRecipe = new List<Recipe>();

            //Act
            var results = _searchService.SearchRecipesByCriteria(searchCriteria).ToList();
            int countOfResults = results.Count();

            // Setup
            _mockRecipeService.Setup(
                mockRecipeService => mockRecipeService
                .SearchRecipes(It.IsAny<string>())).Returns(recipesByRecipe);

            //Assert
            Assert.IsTrue(countOfResults == 0); ;
        }

        [TestMethod()]
        public void SearchRecipesByIngredientNoResultTest()
        {
            //Arrange
            var searchCriteria = new RecipeCriteriaModel
            {
                SearchCategory = "Ingredient",
                SearchCriterion = "Null"
            };

            List<Recipe> recipesByIngredient = new List<Recipe>();

            //Act
            var results = _searchService.SearchRecipesByCriteria(searchCriteria).ToList();
            int countOfResults = results.Count();

            // Setup
            _mockRecipeItemService.Setup(
                mockRecipeItemService => mockRecipeItemService
                .SearchRecipes(It.IsAny<string>())).Returns(recipesByIngredient);

            //Assert
            Assert.IsTrue(countOfResults == 0); ;
        }
    }
}