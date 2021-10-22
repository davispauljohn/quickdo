using NUnit.Framework;
using quickdo_terminal.Types;
using Shouldly;

namespace quickdo_tests
{
    [TestFixture]
    public class DocumentTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AddTask_ShouldAddTaskAndLog()
        {
            Document document = Document.Create();

            document.AddTask("Lisa needs braces");

            document.Tasks.Count.ShouldBe(1);
            document.Tasks[0].Description.ShouldBe("Lisa needs braces");
            document.Tasks[0].Rank.ShouldBe(1);
            document.Log.Count.ShouldBe(2);
            document.Log[0].Type.ShouldBe(QuickDoLogType.DOCUMENTCREATED);
            document.Log[1].Type.ShouldBe(QuickDoLogType.TASKCREATED);
        }

        [Test]
        public void AddTask_WhenTaskAlreadyExists_ShouldSetRankTwo()
        {
            Document document = Document.Create();
            document.AddTask("Lisa needs braces");
            document.AddTask("Dental plan");

            document.Tasks.Count.ShouldBe(2);
            document.Tasks[0].Description.ShouldBe("Lisa needs braces");
            document.Tasks[0].Rank.ShouldBe(1);
            document.Tasks[1].Description.ShouldBe("Dental plan");
            document.Tasks[1].Rank.ShouldBe(2);

            document.Log.Count.ShouldBe(3);
            document.Log[0].Type.ShouldBe(QuickDoLogType.DOCUMENTCREATED);
            document.Log[1].Type.ShouldBe(QuickDoLogType.TASKCREATED);
            document.Log[2].Type.ShouldBe(QuickDoLogType.TASKCREATED);
        }

        [Test]
        public void AddTask_WhenCancelledTaskExists_ShouldSetRankOne()
        {
            Document document = Document.Create();
            document.AddTask("Lisa needs braces");
            document.CancelTask(1);

            document.AddTask("Dental plan");

            document.Tasks.Count.ShouldBe(2);
            document.Tasks[0].Description.ShouldBe("Lisa needs braces");
            document.Tasks[0].Rank.ShouldBe(2);
            document.Tasks[1].Description.ShouldBe("Dental plan");
            document.Tasks[1].Rank.ShouldBe(1);
        }

        [Test]
        public void AddTask_WhenNopeTaskExists_AndTodoTaskExists_ShouldSetRankTwo()
        {
            Document document = Document.Create();
            document.AddTask("Lisa needs braces");
            document.AddTask("Den'al plan");
            document.CancelTask(2);

            document.AddTask("Dental plan");

            document.Tasks.Count.ShouldBe(3);
            document.Tasks[0].Description.ShouldBe("Lisa needs braces");
            document.Tasks[0].Rank.ShouldBe(1);
            document.Tasks[1].Description.ShouldBe("Den'al plan");
            document.Tasks[1].Rank.ShouldBe(3);
            document.Tasks[2].Description.ShouldBe("Dental plan");
            document.Tasks[2].Rank.ShouldBe(2);
        }
    }
}