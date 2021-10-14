using DartTracker.Models;
using DartTracker.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UnitTests
{
    [TestClass]
    public class TestSegmentParser
    {
        [TestMethod]
        public void ParseNormalSegment()
        {
            foreach (char mod in "SDT")
            {
                for (int value = 1; value <= 20; ++value)
                {
                    SegmentModifier modifier = mod switch
                    {
                        'S' => SegmentModifier.SINGLE,
                        'D' => SegmentModifier.DOUBLE,
                        'T' => SegmentModifier.TRIPLE
                    };

                    Assert.AreEqual(new NormalSegment(value, modifier), SegmentParser.parse(value.ToString() + mod));
                }
            }


            for (int value = 1; value <= 20; ++ value)
            {
                Assert.AreEqual(new NormalSegment(value, SegmentModifier.SINGLE), SegmentParser.parse(value.ToString()));
            }


            Assert.AreEqual(null, SegmentParser.parse(""));
            Assert.AreEqual(null, SegmentParser.parse("-1"));
            Assert.AreEqual(null, SegmentParser.parse("-1S"));
            Assert.AreEqual(null, SegmentParser.parse("21"));
            Assert.AreEqual(null, SegmentParser.parse("21S"));
            Assert.AreEqual(null, SegmentParser.parse("19K"));
            Assert.AreEqual(null, SegmentParser.parse("19 D"));
            Assert.AreEqual(null, SegmentParser.parse("19Dx"));
            Assert.AreEqual(null, SegmentParser.parse("S"));
            Assert.AreEqual(null, SegmentParser.parse("???"));
            Assert.AreEqual(null, SegmentParser.parse("Wololo"));
        }


        [TestMethod]
        public void ParseNamedSegment()
        {
            string[] miss_names = { "x", "miss" };
            string[] outer_bull_names = { "b", "bull", "outer bull", "outer bullseye" };
            string[] inner_bull_names = { "bs", "bullseye", "inner bull", "inner bullseye" };


            foreach (var name in miss_names)
            {
                Assert.AreEqual(new NamedSegment(NamedSegmentType.OUTSIDE_BOARD), SegmentParser.parse(name));
            }

            foreach (var name in outer_bull_names)
            {
                Assert.AreEqual(new NamedSegment(NamedSegmentType.OUTER_BULLSEYE), SegmentParser.parse(name));
            }

            foreach (var name in inner_bull_names)
            {
                Assert.AreEqual(new NamedSegment(NamedSegmentType.INNER_BULLSEYE), SegmentParser.parse(name));
            }


            Assert.AreEqual(null, SegmentParser.parse("OuterBull"));
            Assert.AreEqual(null, SegmentParser.parse("Eye"));
            Assert.AreEqual(null, SegmentParser.parse("Bull's Eye"));
            Assert.AreEqual(null, SegmentParser.parse("Eye of male cow."));
        }
    }
}
