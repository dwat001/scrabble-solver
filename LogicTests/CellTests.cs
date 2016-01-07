using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kakariki.Scrabble.Logic.Tests
{
    [TestClass()]
    public class CellTests
    {
        [TestMethod()]
        public void CellTest()
        {
            Cell cell = new Cell(CellType.NORMAL, 3, 4, null);
            Assert.AreEqual(CellType.NORMAL, cell.Type);
            Assert.AreEqual(3, cell.Column);
            Assert.AreEqual(4, cell.Row);
            Assert.IsNull(cell.Letter);

            cell = new Cell(CellType.TRIPPLE_LETTER, 4, 5, 'z');
            Assert.AreEqual(CellType.TRIPPLE_LETTER, cell.Type);
            Assert.AreEqual(4, cell.Column);
            Assert.AreEqual(5, cell.Row);
            Assert.IsTrue(cell.Letter.HasValue);
            Assert.AreEqual('z', cell.Letter.Value);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            Cell cell = new Cell(CellType.NORMAL, 3, 4, null);
            Assert.AreEqual("(3,4) [.]", cell.ToString());

            cell = new Cell(CellType.NORMAL, 4, 3, 'z');
            Assert.AreEqual("(4,3) [z]", cell.ToString());
        }

        [TestMethod()]
        public void ToCharTest()
        {
            Cell cell = new Cell(CellType.NORMAL, 3, 4, null);
            Assert.AreEqual('.', cell.ToChar());

            cell = new Cell(CellType.NORMAL, 3, 4, 'x');
            Assert.AreEqual('x', cell.ToChar());

            cell = new Cell(CellType.DOUBLE_LETTER, 3, 4, null);
            Assert.AreEqual('D', cell.ToChar());

            cell = new Cell(CellType.TRIPPLE_LETTER, 3, 4, null);
            Assert.AreEqual('T', cell.ToChar());

            cell = new Cell(CellType.DOUBLE_WORD, 3, 4, null);
            Assert.AreEqual('2', cell.ToChar());

            cell = new Cell(CellType.TRIPPLE_WORD, 3, 4, null);
            Assert.AreEqual('3', cell.ToChar());
        }
    }
}