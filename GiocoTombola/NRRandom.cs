using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratoreCasuale
{
    class NRRandom
    {
        private int[] _numeri;
        public int Min { get; }
        public int Max { get; }
        public int Rimasti { get; private set; }
        private static void scambia(ref int a, ref int b)
        {
            int c = a;
            a = b;
            b = c;
        }

        // reimposta le condizioni iniziali
        public void Reset()
        {
            Random r = new Random();
            int n = Max - Min;

            for (int i = 0; i < n - 1; i++)
                // scambio l'iesimo con uno a caso dei successivi
                scambia(ref _numeri[i], ref _numeri[r.Next(i + 1, n)]);

            Rimasti = n;
        }

        // genera numeri compresi tra min incluso e max escluso
        public NRRandom(int min, int max)
        {
            Min = min;
            Max = max;
            int n = Max - Min;
            _numeri = new int[n];

            for (int i = 0; i < n; i++)
                _numeri[i] = i + Min;

            Reset();
        }

        // costruisce un oggetto NRRandom che genera a caso n numeri diversi
        // compresi tra 0 e n - 1
        public NRRandom(int n)
            : this(0, n)    // chiamata di un costruttore da un costruttore
        {
        }

        public int Next()   // questo potrebbe essere implementato come proprietà get
        {
            return _numeri[--Rimasti];
        }
    }
}
