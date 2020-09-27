using System;
using Xunit;
using MartianRobots;
using Moq;

namespace test_MartianRobots
{
    public class MartianRobotsHandlerTests
    {
        [Fact]
        public void ShouldUpdateAllRobotsLocation()
        {
           // Arrange
            var input = "5 5\n1 2 N\nLFLFLFLFF\n3 3 E\nFFRFFRFRRF";
            var consoleMock = new Mock<IConsoleWriter>();
            var martianHandler = new MartianRobotsHandler(consoleMock.Object);

            // act
            martianHandler.Execute(input);

            //Asserts
            consoleMock.Verify(mock => mock.WriteLine("Updated location for Robot#0(1, 3, N)"), Times.Once());
            consoleMock.Verify(mock => mock.WriteLine("Updated location for Robot#1(5, 1, E)"), Times.Once());
        }


        [Fact]
        public void ShouldUpdateAllRobotsLocationWithOneRobotLostAndOneScentFound()
        {
           // Arrange
            var input = "5 3\n1 1 E\nRFRFRFRF\n3 2 N\nFRRFLLFFRRFLL\n0 3 W\nLLFFFLFLFL";
            var consoleMock = new Mock<IConsoleWriter>();
            var martianHandler = new MartianRobotsHandler(consoleMock.Object);

            // act
            martianHandler.Execute(input);

            //Asserts
            consoleMock.Verify(mock => mock.WriteLine("Updated location for Robot#0(1, 1, E)"), Times.Once());
            consoleMock.Verify(mock => mock.WriteLine("Robot Lost"), Times.Once());
            consoleMock.Verify(mock => mock.WriteLine("Scent found"), Times.Once());
            consoleMock.Verify(mock => mock.WriteLine("Updated location for Robot#1(3, 3, N)"), Times.Once());
            consoleMock.Verify(mock => mock.WriteLine("Updated location for Robot#2(2, 3, S)"), Times.Once());
        }

        [Fact]
        public void ShouldUpdateAllRobotsLocationWithOneRobotLostAndOneScentFound_Test2()
        {
           // Arrange
            var input = "5 3\n1 1 E\nRFRFRFRF\n3 2 N\nFRRFLLFFRRFLL\n3 1 N\nFFFL";
            var consoleMock = new Mock<IConsoleWriter>();
            var martianHandler = new MartianRobotsHandler(consoleMock.Object);

            // act
            martianHandler.Execute(input);

            //Asserts
            consoleMock.Verify(mock => mock.WriteLine("Updated location for Robot#0(1, 1, E)"), Times.Once());
            consoleMock.Verify(mock => mock.WriteLine("Robot Lost"), Times.Once());
            consoleMock.Verify(mock => mock.WriteLine("Scent found"), Times.Once());
            consoleMock.Verify(mock => mock.WriteLine("Updated location for Robot#1(3, 3, N)"), Times.Once());
            consoleMock.Verify(mock => mock.WriteLine("Updated location for Robot#2(3, 3, W)"), Times.Once());
        }
    }
}