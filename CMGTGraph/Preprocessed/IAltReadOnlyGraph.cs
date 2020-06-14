using System;

namespace CMGTGraph.Preprocessed
{
    // Resources:
    // https://www.cc.gatech.edu/~thad/6601-gradAI-fall2015/02-search-01-Astart-ALT-Reach.pdf
    // https://www.microsoft.com/en-us/research/wp-content/uploads/2004/07/tr-2004-24.pdf
    // http://www-or.amp.i.kyoto-u.ac.jp/members/ohshima/Paper/MThesis/MThesis.pdf
    // http://www.fabianfuchs.com/fabianfuchs_ALT.pdf
    // http://theory.stanford.edu/~amitp/GameProgramming/Heuristics.html
    // https://www.google.com/search?client=opera&q=jump+point+search&sourceid=opera&ie=UTF-8&oe=UTF-8
    // http://theory.stanford.edu/~amitp/GameProgramming/
    
    public interface IAltPreprocessedReadOnlyGraph<T> : IReadOnlyGraph<T> where T : IEquatable<T>
    {
        // needs some landmarks
    }
}