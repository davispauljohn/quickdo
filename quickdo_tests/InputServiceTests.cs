using NSubstitute;
using NUnit.Framework;
using quickdo_terminal;
using quickdo_terminal.Interfaces;
using quickdo_terminal.Models;
using quickdo_terminal.Types;
using Shouldly;
using System;
using System.Collections.Generic;

namespace quickdo_tests
{
    [TestFixture]
    public class InputServiceTests
    {
        private IDocumentService documentService;
        private IInputService inputService;

        [SetUp]
        public void Setup()
        {
            documentService = Substitute.For<IDocumentService>();
            documentService.Query(Arg.Any<int?>()).Returns(new List<TaskModel> { TaskModel.Default() });
            documentService.QueryMostRecent(Arg.Any<QuickDoLogType>()).Returns(TaskModel.Default());
            inputService = new InputService(documentService);
        }

        [Test]
        public void WhenInputIsEmpty_ShouldReturnHelp()
        {
            var result = inputService.ParseAndRunInput(Array.Empty<string>());

            documentService.Received(0);
            result.ShouldHaveSingleItem();
            result[0].Text.ShouldBe("TODO: Impluhment halp");
        }

        [Test]
        public void WhenInputIsUnrecognised_ShouldDisplayError()
        {
            var result = inputService.ParseAndRunInput(new[] { "+!x-" });

            documentService.Received(0);
            result.ShouldHaveSingleItem();
            result[0].Text.ShouldBe("Command not recognised");
        }

        [TestCase("-h")]
        [TestCase("--help")]
        public void WhenInputContainsHelpFlag_ShouldReturnHelp(string flag)
        {
            var result = inputService.ParseAndRunInput(new[] { flag });

            documentService.Received(0);
            result.ShouldHaveSingleItem();
            result[0].Text.ShouldBe("TODO: Impluhment halp");
        }

        [Test]
        public void WhenInputIsQuerySymbol_AndArgumentIsNull_ShouldCallQueryWithNoArguments()
        {
            inputService.ParseAndRunInput(new[] { "?" });

            documentService.Received(1).Query();
        }

        [Test]
        public void WhenInputIsAddSymbol_AndArgumentIsNullOrEmpty_ShouldCallQueryMostRecentWithStatusTODO()
        {
            inputService.ParseAndRunInput(new[] { "+" });

            documentService.Received(1).QueryMostRecent(QuickDoLogType.TASKCREATED);
        }

        [Test]
        public void WhenInputIsAddSymbol_AndArgumentIsNotEmpty_ShouldCallAddTask()
        {
            inputService.ParseAndRunInput(new[] { "+", "Test 1" });

            documentService.Received(1).AddTask("Test 1");
        }

        [Test]
        public void WhenInputIsCompleteSymbol_AndArgumentIsNullOrEmpty_ShouldCallQueryMostRecentWithStatusDONE()
        {
            inputService.ParseAndRunInput(new[] { "x" });

            documentService.Received(1).QueryMostRecent(QuickDoLogType.TASKCOMPLETED);
        }

        [Test]
        public void WhenInputIsCompleteSymbol_AndArgumentIsNotEmpty_AndArgumentIsNotInteger_ShouldCallCompleteTask()
        {
            var result = inputService.ParseAndRunInput(new[] { "x", "I should be an integer" });

            result.ShouldHaveSingleItem();
            result[0].Text.ShouldBe("Complete command requires an integer argument `x {rank:int}`");
            result[0].Colour.ShouldBe(ConsoleColor.Red);
        }

        [Test]
        public void WhenInputIsCompleteSymbol_AndArgumentIsNotEmpty_AndArgumentIsInteger_ShouldCallCompleteTask()
        {
            inputService.ParseAndRunInput(new[] { "x", "1" });

            documentService.Received(1).CompleteTask(1);
        }

        [Test]
        public void WhenInputIsCancelSymbol_AndArgumentIsNullOrEmpty__ShouldCallQueryMostRecentWithStatusNOPE()
        {
            inputService.ParseAndRunInput(new[] { "-" });

            documentService.Received(1).QueryMostRecent(QuickDoLogType.TASKCANCELLED);
        }

        [Test]
        public void WhenInputIsCancelSymbol_AndArgumentIsNotEmpty_AndArgumentIsNotInteger_ShouldCallCancelTask()
        {
            var result = inputService.ParseAndRunInput(new[] { "-", "I should be an integer" });

            result.ShouldHaveSingleItem();
            result[0].Text.ShouldBe("Cancel command requires an integer argument `- {rank:int}`");
            result[0].Colour.ShouldBe(ConsoleColor.Red);
        }

        [Test]
        public void WhenInputIsCancelSymbol_AndArgumentIsNotEmpty_AndArgumentIsInteger_ShouldCallCancelTask()
        {
            inputService.ParseAndRunInput(new[] { "-", "1" });

            documentService.Received(1).CancelTask(1);
        }

        [Test]
        public void WhenInputIsFocusSymbol_AndArgumentIsNullOrEmpty__ShouldCallQueryWithRankOne()
        {
            inputService.ParseAndRunInput(new[] { "!" });

            documentService.Received(1).Query(1);
        }

        [Test]
        public void WhenInputIsFocusSymbol_AndArgumentIsNotEmpty_AndArgumentIsNotInteger_ShouldCallFocusTask()
        {
            var result = inputService.ParseAndRunInput(new[] { "!", "I should be an integer" });

            result.ShouldHaveSingleItem();
            result[0].Text.ShouldBe("Focus command requires an integer argument `! {rank:int}`");
            result[0].Colour.ShouldBe(ConsoleColor.Red);
        }

        [Test]
        public void WhenInputIsFocusSymbol_AndArgumentIsNotEmpty_AndArgumentIsInteger_ShouldCallFocusTask()
        {
            inputService.ParseAndRunInput(new[] { "!", "2" });

            documentService.Received(1).FocusTask(2);
        }
    }
}