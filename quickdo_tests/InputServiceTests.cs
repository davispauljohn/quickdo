using NSubstitute;
using NUnit.Framework;
using quickdo_terminal;
using quickdo_terminal.Interfaces;
using quickdo_terminal.Types;
using Shouldly;
using System;

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

            documentService.Received(1).QueryMostRecent(QuickDoStatus.TODO);
        }

        [Test]
        public void WhenInputIsAddSymbol_AndArgumentIsNotEmpty_ShouldCallAddTask()
        {
            inputService.ParseAndRunInput(new[] { "+", "Test 1" });

            documentService.Received(1).AddTask("Test 1");
        }

        [Test]
        public void WhenInputIsCompleteSymbol_AndArgumentIsNullOrEmpty__ShouldCallQueryMostRecentWithStatusDONE()
        {
            inputService.ParseAndRunInput(new[] { "x" });

            documentService.Received(1).QueryMostRecent(QuickDoStatus.DONE);
        }

        [Test]
        public void WhenInputIsCompleteSymbol_AndArgumentIsNotEmpty_AndArgumentIsNotInteger_ShouldCallCompleteTask()
        {
            var result = inputService.ParseAndRunInput(new[] { "x", "I should be an integer" });

            result.ShouldHaveSingleItem();
            result[0].Text.ShouldBe("Error: Only integer arguments are accepted when completing a task `x {rank:int}`");
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

            documentService.Received(1).QueryMostRecent(QuickDoStatus.NOPE);
        }

        [Test]
        public void WhenInputIsCancelSymbol_AndArgumentIsNotEmpty_AndArgumentIsNotInteger_ShouldCallCancelTask()
        {
            var result = inputService.ParseAndRunInput(new[] { "-", "I should be an integer" });

            result.ShouldHaveSingleItem();
            result[0].Text.ShouldBe("Error: Only integer arguments are accepted when cancelling a task `- {rank:int}`");
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
            result[0].Text.ShouldBe("Error: Only integer arguments are accepted when focusing a task `! {rank:int}`");
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