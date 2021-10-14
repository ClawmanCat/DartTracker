using DartTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DartTracker.Utility
{
    public static class SegmentParser
    {
        public static BoardSegment parse(string text)
        {
            text = text.ToLower();


            Regex normal_segment = new Regex(@"^\d{1,2}[sdt]?$");
            if (normal_segment.IsMatch(text))
            {
                SegmentModifier modifier = text.Last() switch
                {
                    's' => SegmentModifier.SINGLE,
                    'd' => SegmentModifier.DOUBLE,
                    't' => SegmentModifier.TRIPLE,
                    _   => SegmentModifier.SINGLE
                };

                var result = new NormalSegment(int.Parse(text.TrimEnd('s', 'd', 't')), modifier);

                // Normal segments must be between 1 and 20.
                return result.value >= 1 && result.value <= 20 ? result : null;
            }


            return text switch 
            {
                "x" => new NamedSegment(NamedSegmentType.OUTSIDE_BOARD),
                "miss" => new NamedSegment(NamedSegmentType.OUTSIDE_BOARD),

                "b" => new NamedSegment(NamedSegmentType.OUTER_BULLSEYE),
                "bull" => new NamedSegment(NamedSegmentType.OUTER_BULLSEYE),
                "outer bull" => new NamedSegment(NamedSegmentType.OUTER_BULLSEYE),
                "outer bullseye" => new NamedSegment(NamedSegmentType.OUTER_BULLSEYE),

                "bs" => new NamedSegment(NamedSegmentType.INNER_BULLSEYE),
                "bullseye" => new NamedSegment(NamedSegmentType.INNER_BULLSEYE),
                "inner bull" => new NamedSegment(NamedSegmentType.INNER_BULLSEYE),
                "inner bullseye" => new NamedSegment(NamedSegmentType.INNER_BULLSEYE),

                _ => null
            };
        }


        public static bool is_valid_segment(string text)
        {
            return parse(text) != null;
        }
    }
}
